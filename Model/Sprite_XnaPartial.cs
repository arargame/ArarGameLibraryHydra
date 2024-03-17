using ArarGameLibrary.Effect;
using ArarGameLibrary.Event;
using ArarGameLibrary.Extension;
using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ValueHistoryManagement;

namespace ArarGameLibrary.Model
{
    public interface IXna
    {
        void Initialize();

        void LoadContent(Texture2D texture = null);

        void UnloadContent();

        void Update(GameTime gameTime = null);

        void Draw(SpriteBatch spriteBatch = null);
    }


    public abstract partial class Sprite : BaseObject, IDrawableObject
    {
        public Sprite()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            SetName(MemberInfoName);

            OnChangeRectangle += SetRectangle;

            IsActive = IsAlive = IsVisible = true;

            ClampManager = new ClampManager(this)
                .Add(new ClampObject("Size.X", 0f, float.MaxValue))
                .Add(new ClampObject("Size.Y", 0f, float.MaxValue));

            SetStartingSettings();

            //DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            //destinationRectangle = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)size.X, (int)size.Y);

            SetSourceRectangle(new Rectangle(0, 0, (int)Size.X, (int)Size.Y));

            Color = Color.White;

            //Effects.Add(new PulsateEffect(this));

            Events.Add(new DraggingEvent(this));

            Events.Add(new SimpleShadowEffect(this, new Vector2(-6, -6)));

            Events.Add(new PulsateEffect(this));

            TestInfo = new TestInfo(this);

            SpriteBatch = Global.SpriteBatch;

            var equalityFunctionForVector2 = new Func<object, object, bool>((previousValue, currentValue) =>
            {
                var previousValueAsVector2 = (Vector2)previousValue;

                var currentValueAsVector2 = (Vector2)currentValue;

                return previousValueAsVector2 != currentValueAsVector2;
            });

            ValueHistoryManager.AddSetting(new ValueHistorySetting("Position", 2, equalityFunctionForVector2));

            ValueHistoryManager.AddSetting(new ValueHistorySetting("Size", 2, equalityFunctionForVector2));
        }

        public virtual void LoadContent(Texture2D texture = null)
        {
            SetTexture(texture);
        }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime = null)
        {
            if (IsActive)
            {
                SetRectangle();

                SetOrigin();

                //if (IsPulsating)
                //{
                //    Scale = General.Pulsate();
                //}

                //foreach (var effect in Effects)
                //{
                //    effect.Update();
                //}

                foreach (var e in Events)
                {
                    e.Update();
                }

                ClampManager.Update();

                IsHovering = InputManager.IsHovering(DestinationRectangle);

                if (IsClickable)
                {
                    IsSelecting = InputManager.Selected(DestinationRectangle);
                }

                TestInfo.Update();

                ValueHistoryManager.Update();
            }
        }

        public void Draw(Action drawingFunction)
        {
            Global.SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, TransformMatrix);

            drawingFunction();

            Global.SpriteBatch.End();
        }

        public virtual void Draw(SpriteBatch spriteBatch = null)
        {
            SetSpriteBatch(spriteBatch);

            if (IsActive && IsVisible)
            {
                if (Texture != null)
                {
                    switch (DrawMethodType)
                    {
                        case 1:
                            SpriteBatch.Draw(Texture, DestinationRectangle, Color);
                            break;

                        case 2:
                            SpriteBatch.Draw(Texture, Position, Color);
                            break;

                        case 3:
                            SpriteBatch.Draw(Texture, DestinationRectangle, SourceRectangle, Color);
                            break;

                        case 4:
                            SpriteBatch.Draw(Texture, Position, SourceRectangle, Color);
                            break;

                        case 5:
                            SpriteBatch.Draw(Texture, DestinationRectangle, SourceRectangle, Color, Rotation, Origin, SpriteEffects, LayerDepth);
                            break;

                        case 6:
                            SpriteBatch.Draw(Texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
                            break;

                        case 7:
                            SpriteBatch.Draw(Texture, Position, SourceRectangle, Color, Rotation, Origin, new Vector2(Scale), SpriteEffects, LayerDepth);
                            break;

                        default:
                            SpriteBatch.Draw(Texture, DestinationRectangle, Color);
                            break;
                    }
                }

                foreach (var e in Events)
                {
                    e.Draw();
                }

                TestInfo.Draw();
            }
        }
    }
}
