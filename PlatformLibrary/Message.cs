using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary
{
    public class Message
    {
        //private SpriteFont font;
        //private Color color = Color.Gold;
        public string text;

        public Message(string v)
        {
            this.text = v;
        }

        //public void WriteMessage(SpriteBatch SpriteBatch, Vector2 pos)
        //{
        //    SpriteBatch.DrawString(font, this.text, pos, color);
        //}

    }
}
