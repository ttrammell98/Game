using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;


namespace MonoGameWindowsStarter
{
    public class Block : IBoundable
    {
        Game1 game;

        Texture2D texture;

        BoundingRectangle bounds;

        public BoundingRectangle Bounds => bounds;

        public Block(Game1 game, float x, float y)
        {
            this.game = game;
            bounds.X = x;
            bounds.Y = y;
        }

        public void Initialize()
        {
            bounds.Width = 170;
            bounds.Height = 25;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("block");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.Tan);
        }

        public BoundingRectangle GetBounding()
        {
            return bounds;
        }
    }
}
