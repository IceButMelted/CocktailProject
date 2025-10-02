using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public class BG_Parallax
{
    private Texture2D texture;
    private float xSensitivity;
    private float ySensitivity;
    private float extraScale;

    private Vector2 offset;
    private Point screenCenter;

    public BG_Parallax(Texture2D tex, Point screenCenter, float xSensitivity = 0.01f, float ySensitivity = 0.01f, float extraScale = 1.1f)
    {
        texture = tex;
        this.screenCenter = screenCenter;
        this.xSensitivity = xSensitivity;
        this.ySensitivity = ySensitivity;
        this.extraScale = extraScale;
    }

    public void Update(MouseState mouse)
    {
        float deltaX = mouse.X - screenCenter.X;
        float deltaY = mouse.Y - screenCenter.Y;

        offset = new Vector2(deltaX * xSensitivity, deltaY * ySensitivity);
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 manualOffset)
    {
        float scaleX = (graphics.PreferredBackBufferWidth / (float)texture.Width) * extraScale;
        float scaleY = (graphics.PreferredBackBufferHeight / (float)texture.Height) * extraScale;

        float width = texture.Width * scaleX;
        float height = texture.Height * scaleY;

        Vector2 position = new Vector2((graphics.PreferredBackBufferWidth - width) / 2,
                                       (graphics.PreferredBackBufferHeight - height) / 2);

        spriteBatch.Draw(
            texture,
            position - offset + manualOffset,
            null,
            Color.White,
            0f,
            Vector2.Zero,
            new Vector2(scaleX, scaleY),
            SpriteEffects.None,
            0f
        );
    }
}
