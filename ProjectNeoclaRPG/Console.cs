using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectNeoclaRPG
{
	public class Console
	{
		private SpriteFont font;
		private String[] content;
		private const int contentLines = 10;
		private Vector2 position;

		#region Constructor
		public Console(Game1 game)
		{
			font = game.Content.Load<SpriteFont>("System/Fonts/Console");
			position = Vector2.Zero;
			content = new String[10];
			for (int i = 0; i < contentLines; i++)
				content[i] = "d";
		}
		#endregion

		public void Draw(
			SpriteBatch spriteBatch)
		{
			for (int i=0; i<contentLines; i++)
				spriteBatch.DrawString(font, content[i], new Vector2(position.X,position.Y+i*font.LineSpacing), Color.White);
		}

		public void AppendLine(String text)
		{
			for (int i = 0; i < contentLines-1; i++)
				content[i] = content[i + 1];
			content[contentLines - 1] = text;
		}

	}
}
