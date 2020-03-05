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
        private Vector2 source;
        private SpriteFont font;
        private Color color = Color.Gold;
        public string text;

        public Message(string v)
        {
            this.text = v;
        }

        public void WriteMessage(SpriteBatch SpriteBatch, string txt, Vector2 pos, Color c)
        {
            SpriteBatch.DrawString(font, text, source, color);
        }

    }
}
