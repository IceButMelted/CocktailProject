using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Input;

namespace CocktailProject.MiniGame
{
    static class ShakingMinigame
    {
        public static float MaxValue;
        public static float MinValue;

        public static float TopBound;
        public static float BottomBound;

        public static float currentVale;

        public static float increaseSpeed;
        public static float decreaseSpeed;

        public static float cooldownNextClick;

        private static float CurrentCoolDown;

        public static void Update(GameTime gameTime) { 
            
        }

        private static void DecreaseValue() {
            if (CurrentCoolDown > 0) 
                currentVale -= decreaseSpeed;
        }

        private static void IncreaseValue() { 
            if(currentVale < 100)
                currentVale += increaseSpeed;
            if (currentVale > 100)
                currentVale = 100;
        }
    }
}
