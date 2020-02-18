using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

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
        int score = 0;
        SpriteFont font;
        Block block1;
        Block block2;
        SpriteSheet sheet;
        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            block1 = new Block(this, 0, 300);
            block2 = new Block(this, 550, 300);
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
            var tex = Content.Load<Texture2D>("spritesheet");
            sheet = new SpriteSheet(tex, 49, 64, 0, 0); //good for top row

            var playerFrames = from index in Enumerable.Range(9, 1) select sheet[index];
            player = new Player(playerFrames);

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

            player.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(grass, grassRect, Color.White);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(0, 0), Color.Black);

            block1.Draw(spriteBatch);
            block2.Draw(spriteBatch);

            player.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
