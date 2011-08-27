using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
	class Camera
	{
		protected float zoom;
		protected Vector2 position;
		protected Vector2 focus;
		protected float rotation;
		protected Matrix transform;

		public Camera()
		{
			zoom = 1.0f;
			position = Vector2.Zero;
			focus = Vector2.Zero;
			rotation = 0.0f;
		}

		public float Zoom
		{
			get	{ return zoom; }
			set { zoom = (value > .01f) ? value : .01f; }
		}
		public float Rotation
		{
			get { return rotation; }
			set { rotation = value; }
		}
		public Vector2 Position
		{
			get { return position; }
			set { position = value; }
		}
		public Vector2 Focus
		{
			get { return focus; }
			set { focus = value; }
		}
		public void Move(Vector2 amount)
		{
			position += amount;
		}
		public void Rotate(float amount)
		{
			rotation += amount;
		}
		public void UpdateTransformation()
		{
			transform =
				Matrix.CreateTranslation(-position.X, -position.Y, 0) *
				Matrix.CreateRotationZ(rotation) *
				Matrix.CreateScale(zoom, zoom, 1) *
				Matrix.CreateTranslation(focus.X, focus.Y, 0);
		}
		public Matrix GetTransformation()
		{
			return transform;
		}
	}
}
