using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using GeonBit.UI.Entities; // Assuming you're using GeonBit.UI

namespace CocktailProject.Utilities
{
    public static class FadeHelper
    {
        /// <summary>
        /// Smoothly fades an entity's opacity (0–255) from startOpacity to endOpacity
        /// over fadeDuration seconds. Returns true when fade completes.
        /// </summary>
        public static bool FadeEntity(Entity entity, GameTime gameTime,
                                      byte startOpacity, byte endOpacity,
                                      float fadeDuration, ref float fadeElapsed)
        {
            if (entity == null) return true;

            // Avoid divide by zero
            if (fadeDuration <= 0f)
            {
                entity.Opacity = endOpacity;
                return true;
            }

            // Time progression
            fadeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate normalized progress 0–1
            float progress = Math.Min(fadeElapsed / fadeDuration, 1f);

            // Lerp byte opacity (convert to float, then back to byte)
            float newOpacity = MathHelper.Lerp(startOpacity, endOpacity, progress);
            entity.Opacity = (byte)MathHelper.Clamp(newOpacity, 0, 255);

            // Done fading?
            if (progress >= 1f)
            {
                fadeElapsed = 0f; // reset for next use
                return true;
            }

            return false;
        }
    }
}
