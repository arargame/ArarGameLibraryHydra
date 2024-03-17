using ArarGameLibrary.Manager;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{
    public interface IClickableObject
    {
        bool IsClickable { get; set; }
        bool IsDragable { get; set; }
        bool IsDragging { get; set; }
        bool IsHovering { get; set; }
        bool IsSelecting { get; set; }
        void SetClickable(bool enable);
        void SetDragable(bool enable);
        Vector2 DroppingRange { get; set; }
    }

    public partial interface ISprite
    {
        BlendState BlendState { get; set; }

        Rectangle CollisionRectangle { get; set; }

        Color Color { get; set; }

        ClampManager ClampManager { get; set; }

        DepthStencilState DepthStencilState { get; set; }

        Rectangle DestinationRectangle { get; }

        int DrawMethodType { get; set; }

        Vector2 DroppingRange { get; set; }

        Microsoft.Xna.Framework.Graphics.Effect Effect { get; set; }

        bool IsActive { get; set; }
        bool IsAlive { get; set; }
        bool IsClickable { get; set; }
        bool IsDragable { get; set; }
        bool IsDragging { get; set; }
        bool IsPulsating { get; set; }
        bool IsHovering { get; set; }
        bool IsSelecting { get; set; }
        bool IsVisible { get; set; }
        float LayerDepth { get; set; }

        Offset Margin { get; set; }

        Vector2 Origin { get; set; }

        Offset Padding { get; set; }
        Vector2 Position { get; set; }

        Vector2 PositionChangingRatio { get; }
        RasterizerState RasterizerState { get; set; }
        float Rotation { get; set; }

        SamplerState SamplerState { get; set; }
        float Scale { get; set; }
        Vector2 Size { get; set; }
        Vector2? SizeChangingRatio { get; }

        Rectangle SourceRectangle { get; set; }
        Vector2 Speed { get; set; }
        SpriteEffects SpriteEffects { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        SpriteSortMode SpriteSortMode { get; set; }

        TestInfo TestInfo { get; set; }
        Texture2D Texture { get; set; }
        Matrix? TransformMatrix { get; set; }
    }

    public abstract partial class Sprite : ISprite,IClickableObject
    {
        public delegate void SomethingHasBeenChanged();
        public event SomethingHasBeenChanged OnChangeRectangle;

        //public virtual void Refresh(Action action = null)
        //{
        //    if (action != null)
        //        action.Invoke();
        //}

        //public List<EffectManager> Effects = new List<EffectManager>();
        public List<EventManager> Events = new List<EventManager>();

        public BlendState BlendState { get; set; }

        public Rectangle CollisionRectangle { get; set; }
        public Color Color { get; set; }
        public ClampManager ClampManager { get; set; }

        public DepthStencilState DepthStencilState { get; set; }
        public Rectangle DestinationRectangle
        {
            get
            {
                return new Rectangle((int)Math.Ceiling(Position.X), (int)Math.Ceiling(Position.Y), (int)Math.Ceiling(Size.X * Scale), (int)Math.Ceiling((Size.Y * Scale)));
            }
        }
        public int DrawMethodType { get; set; }
        public Vector2 DroppingRange { get; set; }

        public Microsoft.Xna.Framework.Graphics.Effect Effect { get; set; }

        public bool IsActive { get; set; }
        public bool IsAlive { get; set; }
        public bool IsClickable { get; set; }
        public bool IsDragable { get; set; }
        public bool IsDragging { get; set; }
        public bool IsPulsating { get; set; }
        public bool IsHovering { get; set; }
        public bool IsSelecting { get; set; }
        public bool IsVisible { get; set; }

        //From 0f to 1f where 1f is the top layer
        public float LayerDepth { get; set; }
        public const float LayerDepthPlus = 0.01f;

        public Offset Margin { get; set; }

        public Vector2 Origin { get; set; }

        public Offset Padding { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 PositionChangingRatio
        {
            get
            {
                var histories = ValueHistoryManager.GetRecordsByPropertyName("Position", 2).ToList();

                if (histories.Count() == 2)
                {
                    var previousValue = (Vector2)histories[1].Value;

                    var currentValue = (Vector2)histories[0].Value;

                    return (previousValue != Vector2.Zero && currentValue != Vector2.Zero) || previousValue != Vector2.Zero ? currentValue / previousValue : new Vector2(1, 1);
                }

                return new Vector2(1, 1);
            }
        }

        public RasterizerState RasterizerState { get; set; }
        public float Rotation { get; set; }

        public SamplerState SamplerState { get; set; }
        public float Scale { get; set; }
        //public bool SimpleShadowVisibility { get; set; }
        public Vector2 Size { get; set; }
        public Vector2? SizeChangingRatio
        {
            get
            {
                var histories = ValueHistoryManager.GetRecordsByPropertyName("Size", 2).ToList();

                if (histories.Count() == 2)
                {
                    var previousValue = (Vector2)histories[1].Value;

                    var currentValue = (Vector2)histories[0].Value;

                    var result = ((currentValue - previousValue) / previousValue) * 100;

                    if (float.IsInfinity(result.X) || float.IsNaN(result.X))
                        result.X = 0;

                    if (float.IsInfinity(result.Y) || float.IsNaN(result.Y))
                        result.Y = 0;

                    return result != Vector2.Zero ? result : (Vector2?)null;
                }

                return null;
            }
        }
        public Rectangle SourceRectangle { get; set; }
        public Vector2 Speed { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteSortMode SpriteSortMode { get; set; }

        public TestInfo TestInfo { get; set; }
        public Texture2D Texture { get; set; }
        public Matrix? TransformMatrix { get; set; }


    }
}
