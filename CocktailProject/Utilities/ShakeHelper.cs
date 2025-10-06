using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CocktailProject.Utilities
{
    public static class ShakeHelper
    {
        private class ShakeData
        {
            public Entity Entity;
            public Vector2 BaseOffset;
            public float Timer;
            public float Duration;   // 0 = continuous
            public float Speed;
            public float Amplitude;
            public bool ShakeX;
        }

        private static readonly List<ShakeData> activeShakes = new List<ShakeData>();

        /// <summary>
        /// Start shaking a GeonBit UI Entity.
        /// </summary>
        /// <param name="entity">Target Entity</param>
        /// <param name="amp">Amplitude of shake (distance)</param>
        /// <param name="XorY">true = shake X axis, false = Y axis</param>
        /// <param name="duration">How long to shake (seconds). If 0 or less = infinite.</param>
        /// <param name="speed">Oscillation speed (default 20)</param>
        public static void ShakingEntity(Entity entity, float amp, bool XorY, float duration = 0f, float speed = 20f)
        {
            if (entity == null) return;

            // If already shaking, refresh
            foreach (var s in activeShakes)
            {
                if (s.Entity == entity)
                {
                    s.Amplitude = amp;
                    s.ShakeX = XorY;
                    s.Speed = speed;
                    s.Duration = duration;
                    s.Timer = 0f;
                    s.BaseOffset = entity.Offset;
                    return;
                }
            }

            // Add new shake
            activeShakes.Add(new ShakeData
            {
                Entity = entity,
                BaseOffset = entity.Offset,
                Timer = 0f,
                Duration = duration,
                Speed = speed,
                Amplitude = amp,
                ShakeX = XorY
            });
        }

        /// <summary>
        /// Toggle shake direction (horizontal ↔ vertical) for an active entity.
        /// </summary>
        /// <param name="entity">Target entity currently shaking</param>
        public static void ToggleShakeDirection(Entity entity)
        {
            foreach (var s in activeShakes)
            {
                if (s.Entity == entity)
                {
                    s.ShakeX = !s.ShakeX;
                    s.Timer = 0f; // reset for smooth transition
                    s.BaseOffset = entity.Offset;
                    return;
                }
            }

            // If entity not shaking, optionally start new default shake
            // You can comment this out if you only want to toggle existing shakes
            ShakingEntity(entity, 10, true);
        }

        /// <summary>
        /// Stop shaking a specific UI entity.
        /// </summary>
        public static void StopShake(Entity entity)
        {
            for (int i = activeShakes.Count - 1; i >= 0; i--)
            {
                if (activeShakes[i].Entity == entity)
                {
                    activeShakes[i].Entity.Offset = activeShakes[i].BaseOffset;
                    activeShakes.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Stop all active shakes.
        /// </summary>
        public static void StopAll()
        {
            foreach (var s in activeShakes)
            {
                if (s.Entity != null)
                    s.Entity.Offset = s.BaseOffset;
            }
            activeShakes.Clear();
        }

        /// <summary>
        /// Update all shake effects (call once per frame).
        /// </summary>
        public static void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = activeShakes.Count - 1; i >= 0; i--)
            {
                var s = activeShakes[i];

                if (s.Entity == null)
                {
                    activeShakes.RemoveAt(i);
                    continue;
                }

                s.Timer += delta;

                // Re-sync base offset if entity moves
                s.BaseOffset = s.Entity.Offset;

                float offset = (float)Math.Sin(s.Timer * s.Speed) * s.Amplitude;

                if (s.ShakeX)
                    s.Entity.Offset = s.BaseOffset + new Vector2(offset, 0);
                else
                    s.Entity.Offset = s.BaseOffset + new Vector2(0, offset);

                // Stop if timed shake finished
                if (s.Duration > 0 && s.Timer >= s.Duration)
                {
                    s.Entity.Offset = s.BaseOffset;
                    activeShakes.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Change the amplitude (shake strength) of a currently shaking entity.
        /// </summary>
        public static void SetShakeAmplitude(Entity entity, float newAmp)
        {
            foreach (var s in activeShakes)
            {
                if (s.Entity == entity)
                {
                    s.Amplitude = newAmp;
                    return;
                }
            }
        }

        /// <summary>
        /// Change the shake speed (frequency) of a currently shaking entity.
        /// </summary>
        public static void SetShakeSpeed(Entity entity, float newSpeed)
        {
            foreach (var s in activeShakes)
            {
                if (s.Entity == entity)
                {
                    s.Speed = newSpeed;
                    return;
                }
            }
        }

    }
}
