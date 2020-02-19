using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keyboardState;
        Texture2D grass;
        Rectangle grassRect;
        Random random = new Random();
        int score = 0;
        SpriteFont font;
        Block block1;
        Block block2;
        Player player;
        Cake cake;
        Cookie cookie;
        Carrot carrot;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            player = new Player(this);
            block1 = new Block(this, 0, 300);
            block2 = new Block(this, 550, 300);
            cake = new Cake(this, 2, random);
            cookie = new Cookie(this, 1, random);
            carrot = new Carrot(this, -5, random);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();

            grassRect.Width = 720; //width of screen
            grassRect.Height = 65;
            grassRect.X = 0;
            grassRect.Y = graphics.PreferredBackBufferHeight - grassRect.Height;

            block1.Initialize();
            block2.Initialize();

            cake.Initialize();
            cookie.Initialize();
            carrot.Initialize();

            cake.Bounds.X = RandomizeItem();
            cookie.Bounds.X = RandomizeItem();
            carrot.Bounds.X = RandomizeItem();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            grass = Content.Load<Texture2D>("grass");
            font = Content.Load<SpriteFont>("score");
            block1.LoadContent(Content);
            block2.LoadContent(Content);
            player.LoadContent();


            cake.LoadContent(Content);
            cookie.LoadContent(Content);
            carrot.LoadContent(Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (keyboardState.IsKeyDown(Keys.Tab))
            {
                this.Reset();
            }

            player.Update(gameTime);
            cake.Update(gameTime);
            cookie.Update(gameTime);
            carrot.Update(gameTime);

            if (player.collidesWithCake(cake))
            {
                cake.eating.Play();
                score += cake.pointVal;
                cake.Bounds.Y = 0;
                cake.Bounds.X = RandomizeItem();
            }
            if (player.collidesWithCookie(cookie))
            {
                cookie.eating.Play();
                score += cookie.pointVal;
                cookie.Bounds.Y = 0;
                cookie.Bounds.X = RandomizeItem();
            }
            if (player.collidesWithCarrot(carrot))
            {
                carrot.cough.Play();
                score += carrot.pointVal;
                carrot.Bounds.Y = 0;
                carrot.Bounds.X = RandomizeItem();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            spriteBatch.Draw(grass, grassRect, Color.White);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(0, 0), Color.Black);

            block1.Draw(spriteBatch);
            block2.Draw(spriteBatch);

            player.Draw(spriteBatch);
            cake.Draw(spriteBatch);
            cookie.Draw(spriteBatch);
            carrot.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public int GetWidth()
        {
            return graphics.PreferredBackBufferWidth;
        }

        public int GetHeight()
        {
            return graphics.PreferredBackBufferHeight;
        }

        private void Reset()
        {
            score = 0;
            this.Initialize();
        }

        private int RandomizeItem()
        {
            int temp = random.Next(0, graphics.PreferredBackBufferWidth - (int)cake.Bounds.Width);
            return temp;
        }

    }
}
