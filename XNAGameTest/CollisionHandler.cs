using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
	class CollisionHandler
	{
		// Just to pass in as an unused argument to
		// CheckSACollision
		private static Vector2 x; 

		public static bool CheckCollision(
			ICollidable staticObject,
			ICollidable dynamicObject,
			ref Vector2 projectionVector)
		{
			return CheckSACollision(staticObject,dynamicObject,
				ref projectionVector);
		}
		public static bool CheckCollision(
			ICollidable staticObject,
			ICollidable dynamicObject,
			Vector2 projectionAxis)
		{
			return CheckSACollision(staticObject, dynamicObject, ref x);
		}

		// Quick check for collission using the Separating Axis Theorem

		private static bool CheckSACollision(
			ICollidable staticObject,
			ICollidable dynamicObject,
			ref Vector2 projectionVector)
		{
			// Get normalized separating axes
			Vector2[] sAxes = staticObject.GetSeparatingAxes();
			Vector2[] dAxes = dynamicObject.GetSeparatingAxes();

			// Create an array that can be used to track projections
			Vector2[] projections;
			int projectionCount = 0;

			// Create an array for every axis that will be tested
			projections = new Vector2[dAxes.Length + sAxes.Length];

			// Check each axis for collision
			bool collision;
			foreach (Vector2 axis in sAxes)
			{
				collision = CheckAxisCollision(staticObject, dynamicObject,
					axis, ref projectionVector);
				if (!collision)
				{
					// Bail early if there is no collision
					// This is the core benefit of the SAT
					projectionVector = Vector2.Zero;
					return false;
				}
				else
				{
					// Add the projection to the projections array
					projections[projectionCount] = projectionVector;
					projectionCount++;
				}
			}
			foreach (Vector2 axis in dAxes)
			{
				collision = CheckAxisCollision(staticObject, dynamicObject, axis,
					ref projectionVector);
				if (!collision)
				{
					// Bail early if there is no collision
					// This is the core benefit of the SAT
					projectionVector = Vector2.Zero;
					return false;
				}
				else
				{
					// Add the projection to the projections array
					projections[projectionCount] = projectionVector;
					projectionCount++;
				}
			}

			projectionVector = Vector2Utilities.Min(projections);
			return true;
		}

		//Accurate per-pixel collision check
		private bool CheckPixelCollision(
			ICollidable object1,
			ICollidable object2)
		{
			bool collided = false;
			//for (int y = 0; y < boundingTexture.Height; y++)
			//	for (int x = 0; x < boundingTexture.Width; x++ )
			return collided;
		}

		// Check a particular axis for collision
		// Returns true if there is a collision
		private static bool CheckAxisCollision(
			ICollidable staticObject,
			ICollidable dynamicObject,
			Vector2 testAxis,
			ref Vector2 projectionVector)
		{
			// Project each corner of the staticObject onto the axis
			Vector2[] sVectors = staticObject.GetPoints();
			float[] sValues = new float[sVectors.Length];
			for (int i = 0; i < sVectors.Length; i++)
			{
				sValues[i] = Vector2.Dot(sVectors[i], testAxis);
			}

			// Project each corner of the dynamicObject onto the axis
			Vector2[] dVectors = dynamicObject.GetPoints();
			float[] dValues = new float[dVectors.Length];
			for (int i = 0; i < dVectors.Length; i++)
			{
				dValues[i] = Vector2.Dot(dVectors[i], testAxis);
			}

			// Store minimums and maximums
			float sMin = FloatUtilities.Min(sValues);
			float sMax = FloatUtilities.Max(sValues);
			float dMin = FloatUtilities.Min(dValues);
			float dMax = FloatUtilities.Max(dValues);

			// Check overlaps
			// NOTE: these vectors may cause issues because of
			// floating point precision.
			if (sMin <= dMax && sMax >= dMax)
			{
				projectionVector = testAxis * (sMax - dMin);
				return true;
			}
			else if (dMin <= sMax && dMax >= sMax)
			{
				projectionVector = testAxis * (sMax - dMin);
				return true;
			}
			projectionVector = Vector2.Zero;
			return false;
		}
	}
}
