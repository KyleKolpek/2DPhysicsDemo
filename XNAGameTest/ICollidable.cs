﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
	interface ICollidable
	{
		// Throw exception if this fails?
		Texture2D Texture
		{
			get;
		}

		// Added GetSeparatingAxes to replace the above properties
		Vector2[] GetSeparatingAxes();
		// GetPoints provides the points to project onto a given axis
		Vector2[] GetPoints();
		CollidableType GetCollidableType();
		ImplementingType GetImplementingType();

		//void React(ICollidable collidingObject);
	}

	enum CollidableType
	{
		Line,
		CircleNoTexture,
		CircleWithTexture,
		BoxNoTexture,
		BoxWithTeture,
	}

	enum ImplementingType
	{
		Actor,
		CollidableLine,
		Player,
	}
}
