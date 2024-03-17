using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueHistoryManagement;
using ArarGameLibrary.Extension;

namespace ArarGameLibrary.Model
{
    public partial interface ISprite
    {
        ISprite SetSourceRectangle(Rectangle sourceRectangle);
    }

    public abstract partial class Sprite
    {
        public Sprite AsSprite()
        {
            return this as Sprite;
        }

        public virtual void Align(Vector2 offset, Rectangle? parentRect = null)
        {
            if (parentRect == null)
                parentRect = Global.ViewportRect;

            SetPosition(new Vector2((int)(parentRect.Value.Position().X + offset.X), (int)(parentRect.Value.Position().Y + offset.Y)));
        }


        //public T GetEffect<T>() where T : EffectManager
        //{
        //    return EffectManager.Get<T>(Effects);
        //}

        public T GetEvent<T>() where T : EventManager
        {
            return EventManager.Get<T>(Events);
        }

        public virtual void IncreaseLayerDepth(float? additionalDepth = null, float? baseDepth = null)
        {
            SetLayerDepth((baseDepth ?? LayerDepth) + (additionalDepth ?? LayerDepthPlus));
        }

        //public void Pulsate(bool enable)
        //{
        //    //IsPulsating = enable;

        //    //var pulsateEffect = GetEffect<PulsateEffect>();

        //    //if (pulsateEffect == null)
        //    //    return;

        //    //if (IsPulsating)
        //    //    pulsateEffect.Start();
        //    //else
        //    //    pulsateEffect.End();
        //}

        public void RefreshRectangle()
        {
            OnChangeRectangle();
        }

        //public void ShowSimpleShadow(bool enable)
        //{
        //    //SimpleShadowVisibility = enable;

        //    //var simpleShadowEffect = GetEffect<SimpleShadowEffect>();

        //    //if (simpleShadowEffect == null)
        //    //    return;

        //    //if (SimpleShadowVisibility)
        //    //    simpleShadowEffect.Start();
        //    //else
        //    //    simpleShadowEffect.End();
        //}

        public void SetActive(bool enable)
        {
            IsActive = enable;
        }

        public void SetAlive(bool enable)
        {
            IsAlive = enable;
        }

        public void SetBlendState(BlendState blendState)
        {
            BlendState = blendState;
        }

        public void SetClickable(bool enable)
        {
            IsClickable = enable;
        }

        public void SetColor(Color color)
        {
            Color = color;
        }

        public void SetDepthStencilState(DepthStencilState depthStencilState)
        {
            DepthStencilState = depthStencilState;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetDragable(bool enable = true)
        {
            IsDragable = enable;

            if (enable)
                SetClickable(true);
        }

        public void SetDrawMethodType(int methodType)
        {
            DrawMethodType = methodType;
        }

        public void SetEffect(Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            Effect = effect;
        }

        public void SetLayerDepth(float layerDepth)
        {
            LayerDepth = MathHelper.Clamp(layerDepth, 0f, 1f);
        }

        public void SetMargin(Offset margin)
        {
            Margin = margin;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetOrigin(Vector2? origin = null)
        {
            if (origin != null)
                Origin = origin.Value;
            else
                Origin = Vector2.Zero;
        }

        public void SetPadding(Offset padding)
        {
            Padding = padding;
        }

        public void SetPosition(Vector2 position)
        {
            //if (Position != Vector2.Zero && Position == position)
            //    return;

            Position = position;

            if (ValueHistoryManager.HasChangedFor(new ValueHistoryRecord("Position", position)))
                OnChangeRectangle?.Invoke();
        }

        public void SetRasterizerState(RasterizerState rasterizerState)
        {
            RasterizerState = rasterizerState;
        }

        public void SetRectangle()
        {
            SourceRectangle = new Rectangle(0, 0, Texture != null ? Texture.Width : DestinationRectangle.Width, Texture != null ? Texture.Height : DestinationRectangle.Height);
            //CollisionRectangle = new Rectangle(Des);
            //SourceRectangle = new Rectangle(animation.FrameBounds.X, animation.FrameBounds.Y, (int)Size.X, (int)Size.Y);
            //            destinationRectangle = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)size.X, (int)size.Y);
        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
        }

        public void SetSamplerState(SamplerState samplerState)
        {
            SamplerState = samplerState;
        }

        public ISprite SetSourceRectangle(Rectangle sourceRectangle)
        {
            SourceRectangle = sourceRectangle;

            return this;
        }

        public void SetScale(float scale)
        {
            //if (Scale != 1f && Scale == scale)
            //    return;

            Scale = scale;

            OnChangeRectangle?.Invoke();
        }

        public void SetSize(Vector2 size)
        {
            //if (Size != Vector2.Zero && Size == size)
            //    return;
            Size = size;

            ClampManager.Update();

            if (ValueHistoryManager.HasChangedFor(new ValueHistoryRecord("Size", Size)))
                OnChangeRectangle?.Invoke();
        }

        public void SetSpeed(Vector2 speed)
        {
            Speed = speed;
        }

        public void SetSpriteBatch(GraphicsDevice graphicsDevice = null)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice ?? Global.GraphicsDevice);
        }

        public void SetSpriteBatch(SpriteBatch spriteBatch = null)
        {
            if (spriteBatch != null)
                SpriteBatch = spriteBatch;
        }

        public void SetSpriteEffects(SpriteEffects effects)
        {
            SpriteEffects = effects;
        }

        public void SetSpriteSortMode(SpriteSortMode spriteSortMode)
        {
            SpriteSortMode = spriteSortMode;
        }

        public void SetTexture(string assetName, string rootDirectory = "Content")
        {
            Texture = TextureManager.CreateTexture2D(assetName, rootDirectory);
        }

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public void SetTexture()
        {
            Texture = TextureManager.CreateTexture2DByRandomColor();
        }

        public void SetTexture(Color color, int width = 1, int height = 1)
        {
            Texture = TextureManager.CreateTexture2DBySingleColor(color, width, height);
        }

        public void SetTransformMatrix(Matrix transformMatrix)
        {
            TransformMatrix = transformMatrix;
        }

        public virtual void SetVisible(bool enable)
        {
            IsVisible = enable;
        }

        private void SetStartingSettings()
        {
            SetBlendState(BlendState.AlphaBlend);

            SetClickable(false);

            SetStartingLayerDepth();

            SetOrigin();

            SetSpriteSortMode(SpriteSortMode.FrontToBack);
            SetStartingPosition();
            SetStartingRotation();
            SetStartingScale();
            SetStartingSize();
            SetStartingSpeed();
            SetStartingSpriteEffects();
        }

        public virtual void SetStartingLayerDepth()
        {
            SetLayerDepth(0.5f);
        }

        public virtual void SetStartingScale()
        {
            SetScale(1f);
        }

        public virtual void SetStartingSpriteEffects()
        {
            SpriteEffects = SpriteEffects.None;
        }

        public virtual void SetStartingPosition()
        {
            SetPosition(Vector2.Zero);
        }

        public virtual void SetStartingRotation()
        {
            SetRotation(0f);
        }

        public virtual void SetStartingSize()
        {
            SetSize(Vector2.Zero);
        }

        public virtual void SetStartingSpeed()
        {
            SetSpeed(Vector2.Zero);
        }
    }
}
