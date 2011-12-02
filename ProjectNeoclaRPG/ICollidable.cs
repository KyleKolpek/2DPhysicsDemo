using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectNeoclaRPG
{
    interface ICollidable
    {
        // Added GetSeparatingAxes to replace the above properties
        Vector2[] GetSeparatingAxes();
        // GetPoints provides the points to project onto a given axis
        Vector2[] GetPoints();

        //void React(ICollidable collidingObject);
    }
}
