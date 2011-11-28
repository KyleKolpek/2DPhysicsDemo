using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectNeoclaRPG
{
	class GroundQuad: CollidablePolygon
	{
		#region Static Fields and Properties

		// TODO: Clarify that these are angles for the normals
		public static float MaxAngle
		{
			get { return maxAngle; }
		}
		private static float maxAngle = (float)(-Math.PI / 4);
		public static float MinAngle
		{
			get { return minAngle; }
		}
		private static float minAngle = (float)(-3 * Math.PI / 4);

		#endregion

		#region Fields and Properties

		#endregion

		#region Constructors

		public GroundQuad(
			Vector2[] points)
			: base(points)
		{
			if (points.Length != 4)
			{
				throw new ArgumentException(
					"Vector2 points[] must contain 4 Vector2s", "points");
			}
			staticFriction = .5f;
			kineticFriction = .4f;
		}

		#endregion


		#region Friction

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


	}
}
