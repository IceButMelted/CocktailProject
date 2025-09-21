using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Input;

namespace CocktailProject.MiniGame
{
    static class ShakingMinigame
    {
        // Value Range
        public const float MinSize = 1;
        public const float MaxSize = 100;

        // Target Zone
        public static float TargetZone_MaxSize = 40;
        public static float TargetZone_MinSize = 20;
        public static float TargetZone_CurrentSize;
        public static float InitTargetZone;   // center point of the zone
        public static float InitTargetZone_MinValue = 40f; // must not go below this

        // Progress Bar
        public const float ProgressBar_MaxValue = 100;
        public static float ProgressBar_CurrentValue = 1;
        public static float ProgressBar_IncreaseRate = 20f; // per second

        // Shrink factor
        public static float TargetZone_DecreasePerProgress = 0.5f;

        // Bar Values
        public static float CurrentValue = 1;
        public static float CurrentValue_IncreaseRate = 10f; // per click
        public static float CurrentValue_DecreaseRate = 30f; // per second

        private static Random random = new Random();
        private static bool initialized = false;

        public static bool Start = false;

        public static void Init()
        {
            TargetZone_CurrentSize = TargetZone_MaxSize;

            float halfSize = TargetZone_CurrentSize / 2f;
            float minCenter = Math.Max(InitTargetZone_MinValue, MinSize + halfSize); // never below InitTargetZone_MinValue
            float maxCenter = MaxSize - halfSize;

            InitTargetZone = (float)(random.NextDouble() * (maxCenter - minCenter) + minCenter);

            initialized = true;
        }

        public static void Update(GameTime gameTime)
        {
            if (!Start) return;
            if (!initialized) Init();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Core.Input.Mouse.WasButtonJustPressed(MouseButton.Left))
                IncreaseValue();
            else
                DecreaseValue(dt);

            CurrentValue = MathHelper.Clamp(CurrentValue, MinSize, MaxSize);

            float halfSize = TargetZone_CurrentSize / 2f;
            float zoneStart = InitTargetZone - halfSize;
            float zoneEnd = InitTargetZone + halfSize;

            // keep zone inside range
            if (zoneStart < MinSize) zoneStart = MinSize;
            if (zoneEnd > MaxSize) zoneEnd = MaxSize;

            if (CurrentValue >= zoneStart && CurrentValue <= zoneEnd)
            {
                float oldProgress = ProgressBar_CurrentValue;
                ProgressBar_CurrentValue += ProgressBar_IncreaseRate * dt;

                if (ProgressBar_CurrentValue > oldProgress)
                {
                    TargetZone_CurrentSize -= TargetZone_DecreasePerProgress * (ProgressBar_CurrentValue - oldProgress);
                    if (TargetZone_CurrentSize < TargetZone_MinSize)
                        TargetZone_CurrentSize = TargetZone_MinSize;
                }
            }

            ProgressBar_CurrentValue = MathHelper.Clamp(ProgressBar_CurrentValue, MinSize, ProgressBar_MaxValue);
        }

        private static void DecreaseValue(float dt)
        {
            CurrentValue -= CurrentValue_DecreaseRate * dt;
        }

        private static void IncreaseValue()
        {
            CurrentValue += CurrentValue_IncreaseRate;
        }

        public static void Reset()
        {
            initialized = false;
            ProgressBar_CurrentValue = 1;
            CurrentValue = 1;
        }

        public static bool IsComplete()
        {
            return ProgressBar_CurrentValue >= ProgressBar_MaxValue;
        }

        public static void Stop()
        {
            Start = false;
            Reset();
        }
        public static void StartGame()
        {
            Start = true;
            Reset();
        }
    }
}
