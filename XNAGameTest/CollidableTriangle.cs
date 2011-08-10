using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
	class CollidableTriangle:ICollidable
	{
		#region Fields and Properties
		private Vector2 point1, point2, point3;
		#endregion

		#region Constructors

		public CollidableTriangle(
			Vector2 point1,
			Vector2 point2,
			Vector2 point3)
		{
			this.point1 = point1;
			this.point2 = point2;
			this.point3 = point3;
		}

		public CollidableTriangle(
			float x1, float y1,
			float x2, float y2,
			float x3, float y3) :
		this(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3))
		{
		}

		#endregion

		public override string ToString()
		{
			return point1 + " " + point2 + " " + point3;
		}

		public void Draw(PrimitiveBatch primitiveBatch)
		{
			primitiveBatch.AddVertex(point1, Color.Red);
			primitiveBatch.AddVertex(point2, Color.Red);
			primitiveBatch.AddVertex(point3, Color.Red);
		}

		#region ICollidable Methods

		public Texture2D Texture
		{
			get { return null; }
		}
		public virtual Vector2[] GetPoints()
		{
			Vector2[] points = { point1, point2, point3 };
			return points;
		}
		public virtual Vector2[] GetSeparatingAxes()
		{
			Vector2[] axes = new Vector2[3];
			axes[0] = Vector2Utilities.RotateRightCW(point2 - point1);
			axes[0].Normalize();
			axes[1] = Vector2Utilities.RotateRightCW(point3 - point2);
			axes[1].Normalize();
			axes[2] = Vector2Utilities.RotateRightCW(point1 - point3);
			axes[2].Normalize();
			return axes;
		}

		#endregion
	}
}
