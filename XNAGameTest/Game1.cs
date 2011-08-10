using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		//Drawing
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private PrimitiveBatch primitiveBatch;
		private Player player1, player2;
		private Player activePlayer;
		private CollidableTriangle[] lines;
		private bool consoleEnabled;
		private Console console;

		//Game Data
		private Level currentLevel;

		//Input States
		private KeyboardState keyboardState;
		private GamePadState gamePadState;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			Content.RootDirectory = "Content";
			consoleEnabled = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			primitiveBatch = new PrimitiveBatch(GraphicsDevice);
			currentLevel = new Level(Services, 0);
			player1 = new Player(this);
			player2 = new Player(this);
			activePlayer = player1;
			lines = new CollidableTriangle[8];
			lines[0] = new CollidableTriangle(0, 400, 320, 500);
			lines[1] = new CollidableTriangle(320, 500, 840, 500);
			lines[2] = new CollidableTriangle(840, 500, 1280, 450);
			lines[3] = new CollidableTriangle(1080, 270, 1110, 270);
			lines[4] = new CollidableTriangle(1080, 320, 1110, 320);
			lines[5] = new CollidableTriangle(1080, 370, 1110, 370);
			lines[6] = new CollidableTriangle(1080, 420, 1110, 420);
			lines[7] = new CollidableTriangle(1050, 210, 1150, 200);
			console = new Console(this);

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			ICollidable[] collidingObjects = new ICollidable[20];

			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			keyboardState = Keyboard.GetState();
			if (keyboardState.IsKeyDown(Keys.Tab))
			{
				if (activePlayer == player1)
					activePlayer = player2;
				else
					activePlayer = player1;
			}

			Vector2 projAxis = -player1.velocity;
			projAxis.Normalize();
			foreach (CollidableTriangle line in lines)
			{
				Vector2 tmp2 = Vector2.Zero;

				bool collides = CollisionHandler.CheckCollision(line, player1, Vector2.Zero, ref tmp2);
				if ( collides )
				{
					if (consoleEnabled)
					{
						console.AppendLine("Projection vector: "+tmp2.ToString());
					}
					player1.devMove(tmp2.X, tmp2.Y);
					player1.acceleration = Vector2.Zero;
					player1.velocity = Vector2.Zero;
					player1.devSetOnGround(true);
					//player1.React(line, gameTime.ElapsedGameTime.Milliseconds);
				}
				if (CollisionHandler.CheckCollision(line, player2, projAxis))
				{
					//player2.React(line, gameTime.ElapsedGameTime.Milliseconds);
				}
			}
			if (CollisionHandler.CheckCollision(player1, player2, Vector2.Zero))
			{
				player1.SetColor(Color.Blue);
				player2.SetColor(Color.Red);
			}
			else
			{
				player1.SetColor(Color.White);
				player2.SetColor(Color.White);
			}
			activePlayer.Update(gameTime, keyboardState);
			base.Update(gameTime);
			if (keyboardState.IsKeyDown(Keys.C))
			{
				consoleEnabled = !consoleEnabled;
			}
			if (consoleEnabled)
			{
				//console.AppendLine(activePlayer.velocity.ToString());
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			// Start sprite batch
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			// Draw Sprites
			currentLevel.DrawBackground(gameTime,spriteBatch);
			player1.Draw(gameTime, spriteBatch);
			player2.Draw(gameTime, spriteBatch);
			currentLevel.DrawForeground(gameTime, spriteBatch);


			// Draw console
			console.Draw(spriteBatch);
			base.Draw(gameTime);

			// End sprite batch
			spriteBatch.End();

			// Draw primitives
			primitiveBatch.Begin(PrimitiveType.LineList);
			foreach (CollidableTriangle line in lines)
				line.Draw(primitiveBatch);
			primitiveBatch.End();

		}


	}
}
