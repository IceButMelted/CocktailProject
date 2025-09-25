using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;

namespace CocktailProject.ClassMotion
{
    public class BG_NPC
    {
        private Image npcImage;
        private Vector2 pointA = new Vector2(0, 100);
        private Vector2 pointB = new Vector2(1920, 100);
        private Vector2 position;
        private float speed;
        private bool movingToB;
        private double cooldownTime;
        private double cooldownTimer = 0;
        private bool waiting = false;
        private Rectangle drawSize;
        public enum MovementMode { Warp, PingPong }
        private MovementMode mode;
        private static Random rng = new Random();

        public BG_NPC(Image imageEntity, Rectangle size, MovementMode mode)
        {
            rng = new Random();
            npcImage = imageEntity;
            drawSize = size;
            this.mode = mode;

            movingToB = rng.Next(2) == 0; // true = A->B, false = B->A
            position = movingToB ? pointA : pointB;

            // Set initial position & size for GeonBit image
            //npcImage.Anchor = Anchor.TopLeft;
            npcImage.Size = new Vector2(drawSize.Width, drawSize.Height);
            npcImage.Offset = position;

            // Random Values
            speed = rng.Next(80, 100);
            cooldownTime = rng.NextDouble() * 2 + 2; // Cooldown Time
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!waiting)
            {
                Vector2 target = movingToB ? pointB : pointA;
                Vector2 direction = target - position;

                if (direction.Length() > 1f)
                {
                    direction.Normalize();
                    position += direction * speed * delta;

                    // Update GeonBit image position
                    npcImage.Offset = position;
                }
                else
                {
                    waiting = true;
                    cooldownTimer = cooldownTime;
                }
            }
            else
            {
                cooldownTimer -= delta;
                if (cooldownTimer <= 0)
                {
                    waiting = false;
                    if (mode == MovementMode.Warp)
                    {
                        if (movingToB)
                            position = pointA;
                        movingToB = true;
                        npcImage.Offset = position;
                    }
                    else if (mode == MovementMode.PingPong)
                    {
                        movingToB = !movingToB;
                    }
                }
            }
        }
    }
}
