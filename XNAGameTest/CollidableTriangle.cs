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
		// We treat the line as a box we can project out of.
		// If |point2-point1| is the length then BOX_DEPTH is the width.
		private const int BOX_DEPTH = 40;
		private Vector2 point1, point2;
		public float slope;
		public Vector2 Normal
		{
			get { return normal; }
		}
		private Vector2 normal;
		public float StaticFriction
		{
			get { return staticFriction; }
		}
		private float staticFriction;
		public float KineticFriction
		{
			get { return kineticFriction; }
		}
		private float kineticFriction;
		#endregion

		#region Constructors

		public CollidableTriangle(
			Vector2 point1,
			Vector2 point2)
		{
			this.point1 = point1;
			this.point2 = point2;
			slope = Vector.Y / Vector.X;
			normal = new Vector2(Vector.Y, -Vector.X);
			normal.Normalize();
			staticFriction	= 0.5f;
			kineticFriction = 0.4f;
		}

		public CollidableTriangle(
			float x1, float y1,
			float x2, float y2) :
			this(new Vector2(x1, y1), new Vector2(x2, y2)) { }

		#endregion

		// Resolves the line segment to a vector with point1 at
		// the origin.
		public Vector2 Vector
		{
			get { return point2 - point1; }
		}

		public override string ToString()
		{
			String str;
			str = point1 + " " + point2;
			return str;
		}

		public float GetYatX(float x)
		{
			return slope * (x - point1.X) + point1.Y;
		}

		public void Draw(PrimitiveBatch primitiveBatch)
		{
			primitiveBatch.AddVertex(point1, Color.Red);
			primitiveBatch.AddVertex(point2, Color.Red);
		}

		#region ICollidable Methods

		public Texture2D Texture
		{
			get { return null; }
		}
		public virtual Vector2[] GetPoints()
		{
			Vector2[] points = { point1, point2,
							     point1-(normal*BOX_DEPTH),
							     point2-(normal*BOX_DEPTH) };
			return points;
		}
		public virtual Vector2[] GetSeparatingAxes()
		{
			Vector2[] axes = new Vector2[2];
			axes[0] = point2 - point1;
			axes[0].Normalize();
			axes[1] = normal;
			return axes;
		}
		public virtual CollidableType GetCollidableType()
		{
			return CollidableType.Line;
		}
		public virtual ImplementingType GetImplementingType()
		{
			return ImplementingType.CollidableLine;
		}

		#endregion
	}
}
