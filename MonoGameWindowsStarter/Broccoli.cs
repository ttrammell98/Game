﻿using System;
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
    public class Broccoli
    {
        Game1 game;

        Texture2D texture;

        public SoundEffect cough;

        Random random;

        public BoundingRectangle Bounds;

        public int pointVal; //-5
        public Broccoli(Game1 game, int pv, Random r)
        {
            this.game = game;
            pointVal = pv;
            random = r;
        }

        public void Initialize()
        {
            Bounds.Width = 40;
            Bounds.Height = 40;
            Bounds.Y = 0;
            Bounds.X = RandomizeX();//randomize
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("broccoli");
            cough = content.Load<SoundEffect>("Cough");
        }

        public void Update(GameTime gameTime)
        {
            Bounds.Y += random.Next(2, 5);

            if (Bounds.Y > (game.GetHeight() - (int)Bounds.Height))
            {
                Bounds.Y = 0;
                Bounds.X = RandomizeX(); ;
                if (Bounds.X + Bounds.Width > game.GetWorldWidth())
                {
                    Bounds.X = game.GetWorldWidth() - Bounds.Width;
                }
                if (Bounds.X < 0)
                {
                    Bounds.X = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Bounds, Color.Tan);
        }

        public int RandomizeX()
        {
            int temp;
            temp = random.Next(0, game.GetWorldWidth() - (int)Bounds.Width);
            return temp;
        }


        public void SendBack()
        {
            this.Initialize();
        }


    }
}
