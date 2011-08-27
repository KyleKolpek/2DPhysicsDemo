using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyGame
{
	class GroundQuad: CollidablePolygon
	{
		// TODO: Clarify that these are angles for the normals
		public static float MaxAngle
		{
			get { return maxAngle; }
		}
		private static float maxAngle = (float)( -Math.PI / 4);
		public static float MinAngle
		{
			get { return minAngle; }
		}
		private static float minAngle = (float)(-3 * Math.PI / 4);

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

		// Constructor
		public GroundQuad(Vector2[] points):base(points)
		{
			staticFriction  = .5f;
			kineticFriction = .4f;
		}

	}
}
