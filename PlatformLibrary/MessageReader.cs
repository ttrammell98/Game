using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TRead = GameLibrary.Message;

namespace GameLibrary
{
    /// <summary>
    /// A content reader for Tiled tilesets
    /// </summary>
    public class MessageReader: ContentTypeReader<TRead>
    {
        protected override TRead Read (ContentReader input, TRead existingInstance)
        {
            string messageData = input.ReadString();
            return new Message(messageData);
        }
       
    }
}
