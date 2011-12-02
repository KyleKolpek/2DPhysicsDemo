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

namespace ProjectNeoclaRPG
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
        private GroundQuad[] lines;
        private Boolean consoleEnabled;
        private Console console;

        //Game Data
        private Level currentLevel;

        //Input States
        private KeyboardState keyboardState;
        private KeyboardState prevKeyboardState;
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
            Vector2[] points = new Vector2[4];
            lines = new GroundQuad[4];
            points[0] = new Vector2(0, 400);
            points[1] = new Vector2(350, 500);
            points[2] = new Vector2(350, 720);
            points[3] = new Vector2(0, 720);
            lines[0] = new GroundQuad(points);
            points[0] = new Vector2(320, 500);
            points[1] = new Vector2(840, 500);
            points[2] = new Vector2(840, 720);
            points[3] = new Vector2(320, 720);
            lines[1] = new GroundQuad(points);
            points[0] = new Vector2(840, 500);
            points[1] = new Vector2(1280, 450);
            points[2] = new Vector2(1280, 720);
            points[3] = new Vector2(840, 720);
            lines[2] = new GroundQuad(points);
            points[0] = new Vector2(410, 320);
            points[1] = new Vector2(250, 360);
            points[2] = new Vector2(290, 310);
            points[3] = new Vector2(390, 290);
            lines[3] = new GroundQuad(points);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Tab))
            {
                if (activePlayer == player1)
                    activePlayer = player2;
                else
                    activePlayer = player1;
            }

            foreach (GroundQuad line in lines)
            {
                Vector2 tmp2 = Vector2.Zero;

                Boolean collides = CollisionHandler.CheckCollision(line, player1, ref tmp2);
                if ( collides )
                {
                    if (consoleEnabled)
                    {
                        console.AppendLine("Collided with "+ line);
                    }
                    player1.ReactToGroundQuad(line, tmp2, gameTime);
                }
            }
            activePlayer.Update(gameTime, keyboardState);
            base.Update(gameTime);
            if (keyboardState.IsKeyDown(Keys.C))
            {
                consoleEnabled = !consoleEnabled;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Camera camera = new Camera();
            camera.Position = activePlayer.position;
            camera.Rotation = -activePlayer.rotation;
            camera.Zoom = 1/activePlayer.scale;
            camera.Focus = new Vector2(GraphicsDevice.Viewport.Width * .5f,
                GraphicsDevice.Viewport.Height * .5f);
            camera.UpdateTransformation();

            // TODO: Add your drawing code here
            // Start sprite batch
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                camera.GetTransformation());

            // Draw objects affected by camera
            currentLevel.DrawBackground(gameTime,spriteBatch);
            player1.Draw(gameTime, spriteBatch);
            currentLevel.DrawForeground(gameTime, spriteBatch);



            // End draw
            spriteBatch.End();

            // Draw primitives
            primitiveBatch.Begin(PrimitiveType.LineList, camera.GetTransformation());
            foreach (CollidablePolygon line in lines)
                line.Draw(primitiveBatch);
            primitiveBatch.End();

            // Draw static objects
            /*spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend);

            // Draw console
            console.Draw(spriteBatch);

            // End drawing static objects
            spriteBatch.End();*/

            base.Draw(gameTime);
        }


    }
}
