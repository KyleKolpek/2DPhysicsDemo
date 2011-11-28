using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ProjectNeoclaRPG
{
	// Works best with CW point ordering
	class CollidablePolygon:ICollidable
	{
		#region Fields and Properties

		private Vector2[] points;

		#endregion

		#region Constructors

		public CollidablePolygon(
			Vector2[] points)
		{
			if (points.Length < 3)
			{
				throw new ArgumentException(
					"Vector2 points[] must contain at least 3 Vector2s");
			}
			this.points = new Vector2[points.Length];
			for(int i=0; i<points.Length; i++)
			{
				this.points[i] = points[i];
			}
		}

		#endregion

		public override string ToString()
		{
			return points.ToString();
		}

		public void Draw(PrimitiveBatch primitiveBatch)
		{
			int i;
			for(i=0; i<points.Length-1;i++)
			{
				primitiveBatch.AddVertex(points[i], Color.Red);
				primitiveBatch.AddVertex(points[i+1], Color.Red);
			}
			primitiveBatch.AddVertex(points[i], Color.Red);
			primitiveBatch.AddVertex(points[0], Color.Red);
		}

		#region ICollidable Methods

		public virtual Vector2[] GetPoints()
		{
			return points;
		}

		public virtual Vector2[] GetSeparatingAxes()
		{
			Vector2[] axes = new Vector2[points.Length];
			int i;
			for (i = 0; i < points.Length-1; i++)
			{
				axes[i] = Vector2Utilities.RotateRightCW(points[i+1] - points[i]);
				axes[i].Normalize();
			}
			axes[i] = Vector2Utilities.RotateRightCW(points[0] - points[i]);
			axes[i].Normalize();
			return axes;
		}

		#endregion
	}
}
