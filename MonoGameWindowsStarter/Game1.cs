using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;
using GameLibrary;

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
        Block block3;
        Player player;
        Cake cake;
        Cookie cookie;
        Donut donut;
        Carrot carrot;
        Broccoli broccoli;
        AxisList world;
        List<Block> blocks;
        int worldWidth = 1440;
        Message message;
        Texture2D button;
        Rectangle buttonRect;
        bool hasPressedButton = false;
        ParticleSystem particleSystem;
        Texture2D particleTexture;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            player = new Player(this);
            block1 = new Block(this, 0, 300);
            block2 = new Block(this, 550, 300);
            block3 = new Block(this, 995, 300); // middle of the new part of the world
            cake = new Cake(this, 2, random);
            cookie = new Cookie(this, 1, random);
            donut = new Donut(this, 5, random);
            carrot = new Carrot(this, -3, random);
            broccoli = new Broccoli(this, -5, random);
            blocks = new List<Block>();
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

            grassRect.Width = worldWidth; //width of world
            grassRect.Height = 65;
            grassRect.X = 0;
            grassRect.Y = graphics.PreferredBackBufferHeight - grassRect.Height;

            buttonRect.Width = 60;
            buttonRect.Height = 20;
            buttonRect.X = 0;
            buttonRect.Y = (int)block1.Bounds.Y - 15;

            block1.Initialize();
            block2.Initialize();
            block3.Initialize();

            cake.Initialize();
            cookie.Initialize();
            carrot.Initialize();
            broccoli.Initialize();
            donut.Initialize();

            cake.Bounds.X = RandomizeItem();
            cookie.Bounds.X = RandomizeItem();
            carrot.Bounds.X = RandomizeItem();
            broccoli.Bounds.X = RandomizeItem();
            donut.Bounds.X = RandomizeItem();

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
            button = Content.Load<Texture2D>("button");
            block1.LoadContent(Content);
            block2.LoadContent(Content);
            block3.LoadContent(Content);

            // TODO: use this.Content to load your game content here
            particleTexture = Content.Load<Texture2D>("Particle");
            particleSystem = new ParticleSystem(this.GraphicsDevice, 1000, particleTexture);
            particleSystem.Emitter = new Vector2(0, 0);
            particleSystem.SpawnPerFrame = 4;

            player.LoadContent();
            cake.LoadContent(Content);
            cookie.LoadContent(Content);
            donut.LoadContent(Content);
            carrot.LoadContent(Content);
            broccoli.LoadContent(Content);

            message = Content.Load<Message>("message1");

            blocks.Add(block1);
            blocks.Add(block2);
            blocks.Add(block3);


            world = new AxisList();
            foreach (Block block in blocks)
            {
               world.AddGameObject(block);
            }

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
            // TODO: Add your update logic here
            particleSystem.Update(gameTime);
            player.Update(gameTime);
            cake.Update(gameTime);
            cookie.Update(gameTime);
            donut.Update(gameTime);
            carrot.Update(gameTime);
            broccoli.Update(gameTime);

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
            if (player.collidesWithBroccoli(broccoli))
            {
                broccoli.cough.Play();
                score += broccoli.pointVal;
                broccoli.Bounds.Y = 0;
                broccoli.Bounds.X = RandomizeItem();
            }
            if (player.collidesWithDonut(donut))
            {
                donut.eating.Play();
                score += donut.pointVal;
                donut.Bounds.Y = 0;
                donut.Bounds.X = RandomizeItem();
            }

            if (PlayerCollidesWithButton())
            {
                buttonRect.Y += 20;
                score += 10;
            }
            var blockQuery = world.QueryRange(player.position.X, player.position.X + player.FRAME_WIDTH);
            player.CheckForBlockCollision(blockQuery); 
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var offset = new Vector2(360, 351) - player.position;
            var t = Matrix.CreateTranslation(offset.X, 0, 0);
            var gameVect = new Vector2(1080, 351);
            var o = new Vector2(360, 351) - gameVect;
            var tmp = Matrix.CreateTranslation(o.X, o.Y, 0);
            
            if (player.position.X <= 360)
            {
                spriteBatch.Begin();
            }
            else if (player.position.X >= (worldWidth - (0.5*graphics.PreferredBackBufferWidth))) //1080 in our case, 3/4 of full world
            {

                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, tmp);
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);
            }

            GraphicsDevice.Clear(Color.White);
            //spriteBatch.Begin();
            Vector2 midScreen = new Vector2(player.position.X, 0);
            if (player.position.X <= 360)
            {
                midScreen.X = 360;
            }
            if (player.position.X >= (worldWidth - (0.5 * graphics.PreferredBackBufferWidth)))
            {
                midScreen.X = 1080;
            }

            spriteBatch.Draw(grass, grassRect, Color.White);    
            spriteBatch.DrawString(font, "Score: " + score, midScreen, Color.Black);
            if (player.position.X <= buttonRect.X + buttonRect.Width && player.position.Y < 340)
            {
                hasPressedButton = true;
            }

            if (hasPressedButton)
            {
                spriteBatch.DrawString(font, message.text, new Vector2(0, 0), Color.Gold);
            }
            //MouseState mouse = Mouse.GetState();
            //Console.WriteLine("X: " + mouse.X + "Y: " + mouse.Y);
            spriteBatch.Draw(button, buttonRect, Color.White);            
            block1.Draw(spriteBatch);
            block2.Draw(spriteBatch);
            block3.Draw(spriteBatch);

            player.Draw(spriteBatch);
            cake.Draw(spriteBatch);
            cookie.Draw(spriteBatch);
            donut.Draw(spriteBatch);
            carrot.Draw(spriteBatch);
            broccoli.Draw(spriteBatch);
            
            spriteBatch.End();

            particleSystem.Draw();
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
        public int GetWorldWidth()
        {
            return worldWidth;
        }

        private void Reset()
        {
            score = 0;
            block1.Initialize();
            block2.Initialize();

            cake.Initialize();
            cookie.Initialize();
            carrot.Initialize();

            buttonRect.Width = 60;
            buttonRect.Height = 20;
            buttonRect.X = 0;
            buttonRect.Y = (int)block1.Bounds.Y - 15;
            hasPressedButton = false;

            cake.Bounds.X = RandomizeItem();
            cookie.Bounds.X = RandomizeItem();
            carrot.Bounds.X = RandomizeItem();
        }

        private int RandomizeItem()
        {
            int temp = random.Next(0, graphics.PreferredBackBufferWidth - (int)cake.Bounds.Width);
            return temp;
        }

        private bool PlayerCollidesWithButton()
        {
            if ((buttonRect.X < player.position.X + player.FRAME_WIDTH) && (player.position.X < (buttonRect.X + buttonRect.Width)) && (buttonRect.Y < player.position.Y + player.FRAME_HEIGHT) && (player.position.Y < buttonRect.Y + buttonRect.Height))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
