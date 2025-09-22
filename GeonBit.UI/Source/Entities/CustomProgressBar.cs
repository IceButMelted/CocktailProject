using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class CustomProgressBar : ProgressBar
{
    private Texture2D _backgroundTexture;
    private Texture2D _fillTexture;

    /// <summary>
    /// Rotation in radians.
    /// </summary>
    public float Rotation { get; set; } = 0f;

    /// <summary>
    /// Rotation origin (0..1 normalized, default = center).
    /// </summary>
    public Vector2 RotationOrigin { get; set; } = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// Create a custom progress bar. If background or fill are null, fall back to built-in GeonBit skin.
    /// </summary>
    public CustomProgressBar(int min,int max, Vector2 size,Texture2D background = null, Texture2D fill = null , Anchor anchor = Anchor.Auto, Vector2? offset = null)
        : base(min,max,size, anchor, offset)
    {
        _backgroundTexture = background;
        _fillTexture = fill;
    }

    protected override void DrawEntity(SpriteBatch spriteBatch, DrawPhase phase)
    {
        if (phase != DrawPhase.Base) return;

        // destination rect (UI coordinates)
        var dest = _destRect;

        // fill rectangle (percentage of width)
        int fillWidth = (int)(dest.Width * Value);
        var fillRect = new Rectangle(dest.X, dest.Y, fillWidth, dest.Height);

        // calculate pixel origin for rotation
        Vector2 origin = new Vector2(dest.Width * RotationOrigin.X, dest.Height * RotationOrigin.Y);

        // --- draw background ---
        if (_backgroundTexture != null)
        {
            spriteBatch.Draw(
                _backgroundTexture,
                destinationRectangle: dest,
                sourceRectangle: null,
                color: FillColor,
                rotation: Rotation,
                origin: origin,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }
        else
        {
            // fallback: use built-in GeonBit skin
            base.DrawEntity(spriteBatch, phase);
            return;
        }

        // --- draw fill ---
        if (_fillTexture != null)
        {
            spriteBatch.Draw(
                _fillTexture,
                destinationRectangle: fillRect,
                sourceRectangle: null,
                color: FillColor,
                rotation: Rotation,
                origin: origin,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }
        else
        {
            // fallback to built-in fill
            base.DrawEntity(spriteBatch, phase);
        }
    }
}
