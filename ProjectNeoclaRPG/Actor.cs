using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectNeoclaRPG
{
    class Actor : ICollidable
    {
        #region Fields and Properties
        protected Vector2 acceleration;
        protected Vector2 velocity;

        // position represents the location of the origin
        public Vector2 position;
        protected float mass;

        // pixels/second/second
        protected Vector2 gravity;
        protected Boolean isOnGround;
        protected Boolean isSettled;
        public float rotation;

        // Origin is the point that the image is manipulated relative to
        // (rotations are centered at this point, the texture is centered at this point,
        // etc.)
        protected Vector2 origin;
        public float scale;
        protected Texture2D texture;

        // Rectangle is stored with respect to the texture
        protected Rectangle boundingBox;
        protected Color color;
        protected SpriteEffects flip;
        #endregion



        //Constructor
        public Actor(Game1 game)
        {
            mass            = 1.0f;
            gravity         = new Vector2(0, 800);
            velocity        = Vector2.Zero;
            acceleration    = Vector2.Zero;
            isOnGround      = false;
            isSettled       = false;
            rotation        = 0;
            scale           = 1.0f;
            color           = Color.White;
            texture         = game.Content.Load<Texture2D>("Characters/GrayCharacter");
            origin          = new Vector2(texture.Width / 2, texture.Height);
            position        = new Vector2(600, 0);
            boundingBox     = new Rectangle(0,0,texture.Width,texture.Height);
            flip            = SpriteEffects.None;
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

        // NOTE: Watch this since the changes to where the origin is applied
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
            position += velocity * (ms / 1000.0f);

            // Loop if falling through the ground
            if (position.Y > 720)
            {
                position = new Vector2(640,0);
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
        protected void Move(Vector2 direction)
        {
            position += direction;
        }
        #endregion

        public void ReactToGroundQuad(
            GroundQuad ground,
            Vector2 projectionVector,
            GameTime gameTime)
        {
            // Do nothing if we are settled
            if (isSettled)
            {
                return;
            }

            int ms = gameTime.ElapsedGameTime.Milliseconds;

            // First move out of the ground.
            Move(projectionVector);

            // Account for "steep" slopes
            float angle = (float)Math.Atan2(projectionVector.Y,
                                            projectionVector.X);
            if (angle < GroundQuad.MaxAngle && angle > GroundQuad.MinAngle)
            {
                isOnGround = true;
            }

            // Do checks to prevent normalizing zero vectors
            if (projectionVector == Vector2.Zero ||
                velocity == Vector2.Zero)
            {
                return;
            }

            float normalForce;
            Vector2 slopeDirection;
            Vector2 normal;

            // Assign normal based off projectionVector
            normal = projectionVector;
            normal.Normalize();

            // Get a unit vector in the direction of the slope
            slopeDirection = Vector2Utilities.RotateRightCW(projectionVector);
            slopeDirection.Normalize();

            // Calculate modified trajectory data
            normalForce = Math.Abs(Vector2.Dot(normal, acceleration * mass));
            velocity = Vector2.Dot(velocity, slopeDirection) * slopeDirection;
            
            // Don't think this should be here
            //acceleration = Vector2.Dot(acceleration, slopeDirection) * slopeDirection;  

            // If our the friction is greater than our force we stop the object.
            // We calculate the frictional force using the coefficient of
            // kinetic friction and add that to the acceleration vector.

            // TODO: See if this is unnecessary.
            Vector2 tmp = -velocity;
            if (tmp != Vector2.Zero)
            {
                tmp.Normalize();
            }

            // I think we should modify the velocity directly here,
            // rather than the acceleration.
            // Try modifying velocity and setting acceleration to 0.
            // Woo, it worked! There could still be an issue since we
            // are updating velocity by time twice in the same update
            // loop now.
            acceleration = Vector2.Zero;
            Vector2 velocityDiff = normalForce * ground.KineticFriction *
                tmp * (ms / 1000.0f);
            if (Math.Abs(velocity.Length()) - Math.Abs(velocityDiff.Length()) > 0)
            {
                velocity = velocity + velocityDiff;
            }
            else
            {
                if (isOnGround)
                {
                    isSettled = true;
                }
                velocity = Vector2.Zero;
            }
            // Does not take effect because the force is cleared when Update(...) starts
            //isOnGround = true;

        }
    }
}
