using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// An enum representing the states the player can be in
    /// </summary>
    enum State
    {
        South = 0,
        East = 1,
        West = 2,
        North = 3,
        Idle = 4,
    }

    enum VerticalMovementState
    {
        OnGround,
        Jumping,
        Falling
    }

    /// <summary>
    /// A class representing a player
    /// </summary>
    public class Player
    {
        /// <summary>
        /// How quickly the animation should advance frames (1/8 second as milliseconds)
        /// </summary>
        const int ANIMATION_FRAME_RATE = 124;

        /// <summary>
        /// How quickly the player should move
        /// </summary>
        const float PLAYER_SPEED = 225;

        /// <summary>
        /// The width of the animation frames
        /// </summary>
        public int FRAME_WIDTH = 49;

        /// <summary>
        /// The hieght of the animation frames
        /// </summary>
        public int FRAME_HEIGHT = 64;

        //Starting on ground
        VerticalMovementState verticalState = VerticalMovementState.OnGround;

        const int JUMP_TIME = 500;

        int speed = 3;

        // Private variables
        Game1 game;
        Texture2D texture;
        State state;
        TimeSpan timer;
        TimeSpan jumpTimer;
        int frame;
        public Vector2 position;

        /// <summary>
        /// Creates a new player object
        /// </summary>
        /// <param name="game"></param>
        public Player(Game1 game)
        {
            this.game = game;
            timer = new TimeSpan(0);
            position = new Vector2(360, 351);
            state = State.Idle;
        }

        /// <summary>
        /// Loads the sprite's content
        /// </summary>
        public void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("spritesheet");
        }

        /// <summary>
        /// Update the sprite, moving and animating it
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Vertical movement
            switch (verticalState)
            {
                case VerticalMovementState.OnGround:
                    if (keyboard.IsKeyDown(Keys.Space))
                    {
                        verticalState = VerticalMovementState.Jumping;
                        jumpTimer = new TimeSpan(0);
                    }
                    break;
                case VerticalMovementState.Jumping:
                    jumpTimer += gameTime.ElapsedGameTime;
                    // Simple jumping with platformer physics
                    position.Y -= (350 / (float)jumpTimer.TotalMilliseconds);
                    if (jumpTimer.TotalMilliseconds >= JUMP_TIME) verticalState = VerticalMovementState.Falling;
                    break;
                case VerticalMovementState.Falling:
                    position.Y += speed;
                    //collision logic
                    if (position.Y > 351)
                    {
                        position.Y = 351;
                        verticalState = VerticalMovementState.OnGround;
                    }
                    break;
            }

            // Horizontal movement
            if (keyboard.IsKeyDown(Keys.Left))
            {
                state = State.West;
                position.X -= delta * PLAYER_SPEED;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                state = State.East;
                position.X += delta * PLAYER_SPEED;
            }
            else state = State.Idle;

            // Update the player animation timer when the player is moving
            if (state != State.Idle) timer += gameTime.ElapsedGameTime;

            // Determine the frame should increase.  Using a while 
            // loop will accomodate the possiblity the animation should 
            // advance more than one frame.
            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                // increase by one frame
                frame++;
                // reduce the timer by one frame duration
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }

            // Keep the frame within bounds (there are four frames)
            frame %= 4;

            //Will need to comment this out for project 4
            if(position.X < 0)
            {
                position.X = game.GetWidth() - FRAME_WIDTH;
            }
            if(position.X + FRAME_WIDTH > game.GetWidth())
            {
                position.X = 0;
            }

            //Ensures the player falls off the blocks 
            if ((position.X > 170 && position.X < 550) && (position.Y + FRAME_HEIGHT <= 300))
            {
                verticalState = VerticalMovementState.Falling;
            }

        }

        /// <summary>
        /// Renders the sprite on-screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // determine the source rectagle of the sprite's current frame
            var source = new Rectangle(
                frame * FRAME_WIDTH, // X value 
                (int)state % 4 * FRAME_HEIGHT, // Y value
                FRAME_WIDTH, // Width 
                FRAME_HEIGHT // Height
                );

            // render the sprite
            spriteBatch.Draw(texture, position, source, Color.White);

        }

        public bool collidesWithCake(Cake cake)
        {
            if ((cake.Bounds.X < position.X + FRAME_WIDTH) && (position.X < (cake.Bounds.X + cake.Bounds.Width)) && (cake.Bounds.Y < position.Y + FRAME_HEIGHT) && (position.Y < cake.Bounds.Y + cake.Bounds.Height))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool collidesWithCookie(Cookie cookie)
        {
            if ((cookie.Bounds.X < position.X + FRAME_WIDTH) && (position.X < (cookie.Bounds.X + cookie.Bounds.Width)) && (cookie.Bounds.Y < position.Y + FRAME_HEIGHT) && (position.Y < cookie.Bounds.Y + cookie.Bounds.Height))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool collidesWithCarrot(Carrot carrot)
        {
            if ((carrot.Bounds.X < position.X + FRAME_WIDTH) && (position.X < (carrot.Bounds.X + carrot.Bounds.Width)) && (carrot.Bounds.Y < position.Y + FRAME_HEIGHT) && (position.Y < carrot.Bounds.Y + carrot.Bounds.Height))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void CheckForBlockCollision(IEnumerable<IBoundable> blocks)
        {
            foreach (Block block in blocks)
            {
                if ((block.Bounds.X < position.X + FRAME_WIDTH) && (position.X < (block.Bounds.X + block.Bounds.Width)) && (block.Bounds.Y < position.Y + FRAME_HEIGHT) && (position.Y < block.Bounds.Y + block.Bounds.Height))
                {
                    position.Y = block.Bounds.Y - FRAME_HEIGHT;
                    verticalState = VerticalMovementState.OnGround;
                }
                else
                {
                    verticalState = VerticalMovementState.Falling;
                }
            }
        }

    }
}
