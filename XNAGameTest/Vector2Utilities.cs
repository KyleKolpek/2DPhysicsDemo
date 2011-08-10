using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyGame
{
	class Vector2Utilities
	{
		public static Vector2 RotatePoint(
		Vector2 thePoint,
		Vector2 theOrigin,
		float theRotation)
		{
			if (thePoint == null)
			{
				throw new NullReferenceException("Vector2Utilities.RotatePoint(): Vector2 thePoint must not be null");
			}
			if (theOrigin == null)
			{
				throw new NullReferenceException("Vector2Utilities.RotatePoint(): Vector2 theOrigin must not be null");
			}
			Vector2 aTranslatedPoint = new Vector2();
			aTranslatedPoint.X = (float)(theOrigin.X
				+ (thePoint.X - theOrigin.X) * Math.Cos(theRotation)
				- (thePoint.Y - theOrigin.Y) * Math.Sin(theRotation));
			aTranslatedPoint.Y = (float)(theOrigin.Y
				+ (thePoint.Y - theOrigin.Y) * Math.Cos(theRotation)
				+ (thePoint.X - theOrigin.X) * Math.Sin(theRotation));
			return aTranslatedPoint;
		}

		public static Vector2 ScalePoint(
			Vector2 thePoint,
			Vector2 theOrigin,
			float theScale)
		{
			if (thePoint == null)
			{
				throw new NullReferenceException("Vector2Utilities.ScalePoint(): Vector2 thePoint must not be null");
			}
			if (theOrigin == null)
			{
				throw new NullReferenceException("Vector2Utilities.ScalePoint(): Vector2 theOrigin must not be null");
			}
			Vector2 aScaledPoint;
			aScaledPoint = thePoint - theOrigin;
			aScaledPoint *= theScale;
			aScaledPoint += theOrigin;
			return aScaledPoint;
		}

		public static Vector2 Min(Vector2[] vectors)
		{
			if (vectors.Length <= 0)
			{
				throw new ArgumentException("Vector2Utilities.Min(): Vector2[] vectors cannot be empty");
			}
			Vector2 min = vectors[0];
			foreach (Vector2 v in vectors)
			{
				if (v.Length() < min.Length())
				{
					min = v;
				}
			}
			return min;
		}

		public static Vector2 Max(Vector2[] vectors)
		{
			if (vectors.Length <= 0)
			{
				throw new ArgumentException("Vector2Utilities.Max(): Vector2[] vectors cannot be empty");
			}
			Vector2 max = vectors[0];
			foreach (Vector2 v in vectors)
			{
				if (max.Length() < v.Length())
				{
					max = v;
				}
			}
			return max;
		}
	}
}
