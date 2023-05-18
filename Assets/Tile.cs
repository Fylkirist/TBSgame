using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Assets
{
    public class Tile
    {
        public int Protection;
        private string _type;
        public readonly Dictionary<string, int> MovePenaltyDictionary;
        private readonly Texture2D _texture;

        public Tile(int protection, string type, Dictionary<string,int> movePenalty, Texture2D texture)
        {
            Protection = protection;
            _type = type;
            _texture = texture;
            MovePenaltyDictionary = movePenalty;
        }

        public void Render(Rectangle destination, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,destination,Color.White );
        }

        public static Tile CreateTile(string type)
        {
            switch (type)
            {
                case "plains":
                    return new Tile(1,"plains",new Dictionary<string, int>(){{ "infantry", 1 }},Game1.SpriteDict["PlainTile"]);
            }
            return new Tile(1, "plains", new Dictionary<string, int>() { { "infantry", 1 } }, Game1.SpriteDict["PlainTile"]);
        }
    }
}
