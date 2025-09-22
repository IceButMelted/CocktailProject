using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.UI.DataTypes;
using GeonBit.UI.Entities;

namespace GeonBit.UI.Source.Entities
{
    public class FullImagePanel : Panel
    {
        protected Texture2D _background;

        public FullImagePanel(Texture2D texture, Vector2 size, Anchor anchor = Anchor.Auto)
            : base(size, PanelSkin.None, anchor)
        {
            _background = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Draw full texture stretched into panel rect
            spriteBatch.Draw(
                _background,
                GetActualDestRect(),
                Color.White
            );
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}
