using System;
using System.Diagnostics;
using CocktailProject.ClassCocktail;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Input;

namespace CocktailProject.MiniGame
{
    public static class StiringMinigame
    {
        public static bool Start = false;

        // Value Range
        public const float MaxSize = 100;
        public const float MinSize = 1;

        //Pointing Arrow
        public static float PointingArrow_CurrentValue = 1;
        public static float PointingArrow_InitSpeed = 50f; // per second
        public static float PointingArrow_Speed; // per second
        public static float PointingArrow_SpeedIncreaseRate = 10f; // per second
        public static float PointingArrow_SpeedCap = 100f; // max speed

        // Target Zone
        public static float TargetZone_MaxSize = 40;
        public static float TargetZone_MinSize = 20;
        public static float TargetZone_DecreasePerSuccess = 5f; // decrease size per successful stir
        public static float TargetZone_IncreaseOnFail = 10f; // increase size on fail
        public static float TargetZone_CurrentSize;
        public static float TargetZone_Init;   // center point of the zone

        public static Enum_Direction PointingArrow_Direction = Enum_Direction.Right;

        //progress bar
        public const float ProgressBar_MaxValue = 100;
        public static float ProgressBar_CurrentValue = 1;
        public static int ProgressBar_SuccessTimeToWin = 6;
        public static int ProgressBar_Success = 0;

        //is hit on correct value
        public static bool IsHitCorrectValue = false;

        public static void InitTargetZone()
        {
            TargetZone_CurrentSize = TargetZone_MaxSize;
            PointingArrow_Speed = PointingArrow_InitSpeed;

            // Ensure the center is never placed too close to the edges
            float minCenter = MinSize + TargetZone_MaxSize;
            float maxCenter = MaxSize - TargetZone_MaxSize;

            if (minCenter > maxCenter)
                minCenter = maxCenter; // safety clamp if sizes are weird

            Random random = new Random();
            TargetZone_Init = (float)(random.NextDouble() * (maxCenter - minCenter) + minCenter);
        }

        public static void InitNewTargetZone() {
            // Ensure the center is never placed too close to the edges
            float minCenter = MinSize + TargetZone_MaxSize;
            float maxCenter = MaxSize - TargetZone_MaxSize;

            if (minCenter > maxCenter)
                minCenter = maxCenter; // safety clamp if sizes are weird

            Random random = new Random();
            TargetZone_Init = (float)(random.NextDouble() * (maxCenter - minCenter) + minCenter);
        }


        public static void Update(GameTime gameTime) {
            if (!Start) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check for input
            if (Core.Input.Mouse.WasButtonJustPressed(MouseButton.Left))
            {
                // Check if within target zone
                float halfZoneSize = TargetZone_CurrentSize / 2f;
                float zoneMin = TargetZone_Init - halfZoneSize;
                float zoneMax = TargetZone_Init + halfZoneSize;
                if (PointingArrow_CurrentValue >= zoneMin && PointingArrow_CurrentValue <= zoneMax)
                {
                    Debug.WriteLine("Successful Stir");
                    // Successful stir
                    TargetZone_CurrentSize -= TargetZone_DecreasePerSuccess; // Decrease target zone size
                    if (TargetZone_CurrentSize < TargetZone_MinSize)
                    {
                        TargetZone_CurrentSize = TargetZone_MinSize; // Clamp to minimum size
                    }
                    ProgressBar_Success++;
                    if(PointingArrow_Speed < PointingArrow_SpeedCap)
                        PointingArrow_Speed += PointingArrow_SpeedIncreaseRate; // Increase arrow speed
                    InitNewTargetZone(); // Reinitialize target zone position
                    IsHitCorrectValue = true;
                }
                else
                {
                    // Failed stir, reset target zone size
                    if (!(TargetZone_CurrentSize >= TargetZone_MaxSize))
                        TargetZone_CurrentSize += TargetZone_IncreaseOnFail; // Increase target zone size

                    PointingArrow_Speed = PointingArrow_InitSpeed; // Decrease arrow speed
                    InitTargetZone();
                    IsHitCorrectValue = false;
                }
            }

            // Update Pointing Arrow
            if (PointingArrow_Direction == Enum_Direction.Right)
            {
                PointingArrow_CurrentValue += (PointingArrow_Speed + PointingArrow_SpeedIncreaseRate * (PointingArrow_CurrentValue / MaxSize)) * dt;
                //Debug.WriteLine(PointingArrow_CurrentValue);
                if (PointingArrow_CurrentValue >= MaxSize)
                {
                    PointingArrow_CurrentValue = MaxSize;
                    PointingArrow_Direction = Enum_Direction.Left;
                }
            }
            else
            {
                PointingArrow_CurrentValue -= (PointingArrow_Speed + PointingArrow_SpeedIncreaseRate * ((MaxSize - PointingArrow_CurrentValue) / MaxSize)) * dt;
                if (PointingArrow_CurrentValue <= MinSize)
                {
                    PointingArrow_CurrentValue = MinSize;
                    PointingArrow_Direction = Enum_Direction.Right;
                }
            }

        }

        public static bool IsComplated()
        {
            return ProgressBar_Success >= ProgressBar_SuccessTimeToWin;
        }

        public static void Reset()
        {
            ProgressBar_Success = 0;
            PointingArrow_CurrentValue = 1;
            PointingArrow_Direction = Enum_Direction.Right;
            InitTargetZone();
        }

        public static void StartGame()
        {
            Start = true;
            Reset();
        }

        public static void Stop()
        {
            Start = false;
        }

    }
}
