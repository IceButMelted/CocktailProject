using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailProject.ClassTime
{
    public static class Time
    {
        public static float Time_Global = 0f;

        public static void Update(GameTime gt) {
            Time_Global = (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}
