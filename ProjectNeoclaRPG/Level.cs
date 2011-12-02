using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace ProjectNeoclaRPG
{
    public class Level : IDisposable
    {
        private Texture2D backgroundTexture;
        private Texture2D foregroundTexture;

        //Consructor
        public Level(
            IServiceProvider serviceProvider,
            int levelIndex)
        {
            content = new ContentManager(serviceProvider, "Content");
            LoadImages(levelIndex);
        }

        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        private void LoadImages(
            int levelIndex)
        {
            // write a line of text to the file
            //TextWriter tw = new StreamWriter("log.txt");

            //tw.WriteLine(Content.RootDirectory.ToString()+"/Levels/Level"+levelIndex+"/CollisionMap");

            // close the stream
            //tw.Close();

            backgroundTexture = Content.Load<Texture2D>("Levels/Level" + levelIndex + "/Junkyard");
            foregroundTexture = Content.Load<Texture2D>("Levels/Level" + levelIndex + "/JunkyardForeground");
        }

        //GC method
        public void Dispose()
        {
            Content.Unload();
        }

        //Update
        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState)
        {
        }

        //Draw
        public void DrawForeground(
            GameTime gameTime,
            SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(foregroundTexture, Vector2.Zero, Color.White);
        }
        public void DrawBackground(
            GameTime gameTime,
            SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Vector2(0, -75), Color.White);
        }
    }
}