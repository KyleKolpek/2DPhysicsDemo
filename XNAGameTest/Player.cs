using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
	class Player:Actor
	{
		// Constructor
		public Player(Game1 game):base(game)
		{
		}

		new public void Update(
			GameTime gameTime,
			KeyboardState keyboardState)
		{
			base.Update(gameTime, keyboardState);

			// Calculate user forces first so that the reaction can modify them
			// if desired.
			if (keyboardState.IsKeyDown(Keys.Up))
			{
				if (isOnGround)
				{
					ApplyForce(new Vector2(0, -25000));
				}
				//Move(0, -2);
				//rotationOrigin.Y -= 2;
			}
			if (keyboardState.IsKeyDown(Keys.Down))
			{
				ApplyForce(new Vector2(0, 100));
				//Move(0, 2);
				//rotationOrigin.Y += 2;
			}
			if (keyboardState.IsKeyDown(Keys.Left))
			{
				if (isOnGround)
				{
					ApplyForce(new Vector2(-1000, 0));
				}
				flip = SpriteEffects.None;
				//Move(-2, 0);
				//rotationOrigin.X -= 2;
			}
			if (keyboardState.IsKeyDown(Keys.Right))
			{
				if (isOnGround)
				{
					ApplyForce(new Vector2(1000, 0));
				}
				flip = SpriteEffects.FlipHorizontally;
				//Move(2, 0);
				//rotationOrigin.X += 2;
			}

			if (keyboardState.IsKeyDown(Keys.W))
			{
				Rotate(-0.1f);
			}
			if (keyboardState.IsKeyDown(Keys.Q))
			{
				Rotate(0.1f);
			}
			if (keyboardState.IsKeyDown(Keys.A))
			{
				SetScale(scale + 0.1f);
			}
			if (keyboardState.IsKeyDown(Keys.S))
			{
				SetScale(scale - 0.1f);
			}
			ApplyForce(gravity);
			isOnGround = false;
		}

	}
}
