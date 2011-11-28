using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame
{
	class FloatUtilities
	{
		public static float Min(float[] floats)
		{
			if (floats.Length <= 0)
			{
				throw new ArgumentException("FloatUtilities.Min(): float[] floats cannot be empty");
			}
			float min = floats[0];
			foreach (float f in floats)
			{
				if (f < min)
				{
					min = f;
				}
			}
			return min;
		}

		public static float Max(float[] floats)
		{
			if (floats.Length <= 0)
			{
				throw new ArgumentException("FloatUtilities.Max(): float[] floats cannot be empty");
			}
			float max = floats[0];
			foreach (float f in floats)
			{
				if (max < f)
				{
					max = f;
				}
			}
			return max;
		}
	}
}
