using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PhysDemo2D
{
    class Player:Actor
    {
        const int jumpVelocity = -350;
        // Constructor
        public Player(Game1 game):base(game)
        {
        }

        new public void Update(
            GameTime gameTime,
            KeyboardState keyboardState)
        {
            base.Update(gameTime, keyboardState);

            // Apply drag
            if (isOnGround)
            {
                ApplyForce(-0.01f * new Vector2(velocity.X * Math.Abs(velocity.X),
                                               velocity.Y * Math.Abs(velocity.Y)));
            }

            // Calculate user forces first so that the reaction can modify them
            // if desired.
            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (isOnGround)
                {
                    isSettled = false;
                    velocity = new Vector2(velocity.X, jumpVelocity);
                }
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (isOnGround)
                {
                    isSettled = false;
                    ApplyForce(new Vector2(-2000, 0));
                }
                flip = SpriteEffects.None;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (isOnGround)
                {
                    isSettled = false;
                    ApplyForce(new Vector2(2000, 0));
                }
                flip = SpriteEffects.FlipHorizontally;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                isSettled = false;
                Rotate(-0.1f);
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                isSettled = false;
                Rotate(0.1f);
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                isSettled = false;
                SetScale(scale + 0.05f);
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                isSettled = false;
                SetScale(scale - 0.05f);
            }
            // If we aren't settled we want to let gravity happen
            if (!isSettled)
            {
                ApplyForce(gravity);
                isOnGround = false;
            }
        }

    }
}
