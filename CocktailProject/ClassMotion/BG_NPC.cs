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
        private bool spawned = false; // New: track if NPC has spawned
        private double spawnTimer = 0; // New: spawn delay timer
        private Rectangle drawSize;
        private Rectangle faceLeftRect;
        private Rectangle faceRightRect;

        public enum MovementMode { Warp, PingPong }
        private MovementMode mode;
        private static Random rng = new Random();

        public BG_NPC(Image imageEntity, Rectangle size, MovementMode mode, Rectangle faceLeft, Rectangle faceRight, float spawnDelay = 0f)
        {
            rng = new Random();
            npcImage = imageEntity;
            drawSize = size;
            this.mode = mode;
            faceLeftRect = faceLeft;
            faceRightRect = faceRight;
            spawnTimer = spawnDelay;

            movingToB = rng.Next(2) == 0;
            position = movingToB ? pointA : pointB;

            UpdateFaceDirection();

            npcImage.Size = new Vector2(drawSize.Width, drawSize.Height);
            npcImage.Offset = position;
            npcImage.Visible = (spawnDelay == 0); // Hide until spawn time

            speed = rng.Next(80, 120);
            cooldownTime = rng.NextDouble() * 2 + 2;
        }

        private void UpdateFaceDirection()
        {
            npcImage.SourceRectangle = movingToB ? faceRightRect : faceLeftRect;
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Handle spawn delay
            if (!spawned)
            {
                spawnTimer -= delta;
                if (spawnTimer <= 0)
                {
                    spawned = true;
                    npcImage.Visible = true;
                }
                return; // Don't update movement until spawned
            }

            if (!waiting)
            {
                Vector2 target = movingToB ? pointB : pointA;
                Vector2 direction = target - position;
                if (direction.Length() > 1f)
                {
                    direction.Normalize();
                    position += direction * speed * delta;
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
                        UpdateFaceDirection();
                    }
                }
            }
        }
    }
}