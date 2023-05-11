using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Assets
{
    public class Tile
    {
        public int Protection;
        private string _type;
        public Dictionary<string, int> MovePenaltyDictionary;
        private Texture2D _texture;

        public Tile(int protection, string type, Dictionary<string,int> movePenalty, Texture2D texture)
        {
            Protection = protection;
            _type = type;
            _texture = texture;
            MovePenaltyDictionary = movePenalty;
        }

        public void Render(int posX, int posY, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Vector2(posX,posY),new Color(100));
        }
    }
}
