using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CocktailProject.ClassMotion
{
    public class BG_NPC
    {
        private Texture2D texture;
        private Vector2 pointA = new Vector2(0, 300);
        private Vector2 pointB = new Vector2(1920, 300);
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

        public BG_NPC(Texture2D tex, Rectangle size, MovementMode mode)
        {
            texture = tex;
            drawSize = size;
            this.mode = mode;

            movingToB = rng.Next(2) == 0; // true = A->B, false = B->A
            position = movingToB ? pointA : pointB;

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
                    }
                    else if (mode == MovementMode.PingPong)
                    {
                        movingToB = !movingToB;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, drawSize.Width, drawSize.Height), Color.Black);
        }
    }
}