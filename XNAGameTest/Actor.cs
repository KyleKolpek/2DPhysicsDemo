using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
	class Actor : ICollidable
	{
		#region Fields and Properties
		// TODO: REVERT THESE TO PROTECTED
		public Vector2 acceleration;
		public Vector2 velocity;
		// position represents the location of the origin
		public Vector2 position;
		protected float mass;
		private Vector2 previousPosition;
		// pixels/second/second
		protected Vector2 gravity;
		protected Boolean isOnGround;
		protected float rotation;
		// Origin is the point that the image is manipulated relative to
		// (rotations are centered at this point, the texture is centered at this point,
		// etc.)
		private Vector2 origin;
		protected float scale;
		private Texture2D texture;
		// Rectangle is stored with respect to the texture
		private Rectangle boundingBox;
		private Color color;
		protected SpriteEffects flip;
		#endregion



		//Constructor
		public Actor(Game1 game)
		{
			mass				= 1.0f;
			gravity				= new Vector2(0, 800);
			velocity			= Vector2.Zero;
			acceleration		= Vector2.Zero;
			previousPosition	= position;
			isOnGround			= false;
			rotation			= 0;
			scale				= 1.0f;
			color				= Color.White;
			texture				= game.Content.Load<Texture2D>("Characters/GrayCharacter");
			origin				= new Vector2(texture.Width / 2, texture.Height);
			position			= new Vector2(600, 0);
			boundingBox			= new Rectangle(0,0,texture.Width,texture.Height);
			flip				= SpriteEffects.None;
		}
		public void SetColor(Color color)
		{
			this.color = color;
		}


		#region Draw and Update

		public void Draw(
			GameTime gameTime,
			SpriteBatch spriteBatch)
		{
			//Sprite
			spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, flip, 0);
			//spriteBatch.Draw(texture, position, Color.White);
		}

		// Call at the beginning of overwritten Update classes
		public void Update(
			GameTime gameTime,
			KeyboardState keyboardState)
		{
			// Store ms up front in case gameTime can be updated as we run
			int elapsedMillieconds = gameTime.ElapsedGameTime.Milliseconds;
			// Apply forces
			UpdateVelocity(elapsedMillieconds);
			UpdatePosition(elapsedMillieconds);
			// Set acceleration to zero each time
			acceleration = Vector2.Zero;

		}

		#endregion

		#region ICollidable Methods

		public Texture2D Texture
		{
			get { return texture; }
		}
		// Watch this since the changes to where the origin is applied
		public virtual Vector2[] GetSeparatingAxes()
		{
			Vector2[] axes = new Vector2[2];
			axes[0] = GetTopRightCorner()
						- GetTopLeftCorner();
			axes[0].Normalize();
			axes[1] = GetBottomLeftCorner()
						- GetTopLeftCorner();
			axes[1].Normalize();
			return axes;
		}
		public virtual Vector2[] GetPoints()
		{
			Vector2[] points = {GetTopLeftCorner(),
								   GetTopRightCorner(),
								   GetBottomLeftCorner(),
								   GetBottomRightCorner()};
			return points;
		}
		#endregion

		#region Bounding Rectangle Methods

		private Vector2 GetTopLeftCorner()
		{
			Vector2 corner = new Vector2(
				boundingBox.Left,
				boundingBox.Top);
			corner = Vector2Utilities.RotatePoint(
				corner,
				origin,
				rotation);
			corner = Vector2Utilities.ScalePoint(
				corner,
				origin,
				scale);
			corner += position - origin;
			return corner;
		}
		private Vector2 GetTopRightCorner()
		{
			Vector2 corner = new Vector2(
				boundingBox.Right,
				boundingBox.Top);
			corner = Vector2Utilities.RotatePoint(
				corner,
				origin,
				rotation);
			corner = Vector2Utilities.ScalePoint(
				corner,
				origin,
				scale);
			corner += position - origin;
			return corner;
		}
		private Vector2 GetBottomLeftCorner()
		{
			Vector2 corner = new Vector2(
				boundingBox.Left,
				boundingBox.Bottom);
			corner = Vector2Utilities.RotatePoint(
				corner,
				origin,
				rotation);
			corner = Vector2Utilities.ScalePoint(
				corner,
				origin,
				scale);
			corner += position - origin;
			return corner;
		}
		private Vector2 GetBottomRightCorner()
		{
			Vector2 corner = new Vector2(
				boundingBox.Right,
				boundingBox.Bottom);
			corner = Vector2Utilities.RotatePoint(
				corner,
				origin,
				rotation);
			corner = Vector2Utilities.ScalePoint(
				corner,
				origin,
				scale);
			corner += position - origin;
			return corner;
		}

		#endregion

		#region Physics

		private void UpdatePosition(int ms)
		{	
			// Add velocity pixels/second
			previousPosition = position;
			position += velocity * (ms / 1000.0f);

			// Loop if falling through the ground
			if (position.Y > 1280)
			{
				position.Y = 0;
			}
		}
		private void UpdateVelocity(int ms)
		{
			// Add acceleration pixels/second/second
			velocity += acceleration * (ms / 1000.0f);
		}
		public void ApplyForce(Vector2 force)
		{
			acceleration += force / mass;
		}
		#endregion

		#region Mutators

		protected void Rotate(float theta)
		{
			this.rotation += theta;
		}
		protected void SetScale(float scale)
		{
			this.scale = scale;
		}
		// Moves the Actor.
		protected void Move(float x, float y)
		{
			position.X += x;
			position.Y += y;
		}
		// dev things to be deleted
		public void devMove(float x, float y)
		{
			Move(x, y);
		}
		public void devSetOnGround(bool isOnGround)
		{
			this.isOnGround = isOnGround;
		}

		#endregion
		
		//public void React(ICollidable collidingObject, int ms)
		//{
		//    switch (collidingObject.GetImplementingType())
		//    {
		//        case ImplementingType.CollidableLine:
		//            CollidableTriangle line = (CollidableTriangle)collidingObject;

		//            // Check to see if the player is within the X coords of the line
		//            /*float min, max;
		//            min = (line.FirstPoint.X > line.SecondPoint.X) ?
		//                line.SecondPoint.X : line.FirstPoint.X;
		//            max = (line.FirstPoint.X < line.SecondPoint.X) ?
		//                line.SecondPoint.X : line.FirstPoint.X;
		//            if (position.X < min || position.X > max)
		//            {
		//                break;
		//            }*/

		//            // If the actual player hit the ground.
		//            // Likely needs fixing. Only checks one line. If the player is above
		//            // a different line at previousPosition then the logic is definitely
		//            // not sound.
		//            if (previousPosition.Y <= line.GetYatX(previousPosition.X)
		//                && position.Y > line.GetYatX(position.X))
		//            {
		//                float normalForce;
		//                Vector2 slopeDirection;

		//                // Get a unit vector in the direction of the slope
		//                slopeDirection = line.Vector;
		//                slopeDirection.Normalize();

		//                // Project the Player above the line
		//                position.Y = line.GetYatX(position.X) - 0.001f;

		//                // Calculate modified trajectory data
		//                normalForce = Math.Abs(Vector2.Dot(line.Normal, acceleration * mass));
		//                velocity = Vector2.Dot(velocity, slopeDirection) * slopeDirection;
		//                acceleration = Vector2.Dot(acceleration, slopeDirection) * slopeDirection;

		//                // If our the static friction is greater than our force we stop the object.
		//                // Otherwise we calculate the frictional force using the coefficient of
		//                // kinetic friction and add that to the acceleration vector.
		//                if ((normalForce * line.StaticFriction > acceleration.Length() * mass)
		//                    && (velocity.Length() == 0))
		//                {
		//                    acceleration = Vector2.Zero;
		//                    velocity = Vector2.Zero;
		//                }
		//                else
		//                {
		//                    Vector2 tmp = -velocity;
		//                    tmp.Normalize();
		//                    acceleration += normalForce * line.KineticFriction * tmp;
		//                }
		//                // Does not take effect because the force is cleared when Update(...) starts
		//                isOnGround = true;
		//            }
		//            break;
		//        case ImplementingType.Player:
		//            break;
		//        default:
		//            break;
		//    }
		//}

	}
}
