using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Assets
{
    public class Tile
    {
        public int Protection;
        public string Type;
        public readonly Dictionary<string, int> MovePenaltyDictionary;
        private readonly Texture2D _texture;
        private Texture2D _border;

        public Tile(int protection, string type, Dictionary<string,int> movePenalty, Texture2D texture)
        {
            Protection = protection;
            Type = type;
            _texture = texture;
            MovePenaltyDictionary = movePenalty;
            _border = Game1.SpriteDict["TileBorder"];
        }

        public void Render(Rectangle destination, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,destination,Color.White );
            spriteBatch.Draw(_border,destination,Color.White);
        }

        public static Tile CreateTile(string type)
        {
            switch (type)
            {
                case "plains":
                    return new Tile(1,"plains",new Dictionary<string, int>(){{ "infantry", 1 }},Game1.SpriteDict["PlainTile"]);
                case "forest":
                    return new Tile(2, "forest", new Dictionary<string, int>() { { "infantry", 2 } }, Game1.SpriteDict["ForestTile"]);
                case "path":
                    return new Tile(0, "path", new Dictionary<string, int>() { { "infantry", 1 } }, Game1.SpriteDict["PathTile"]);
                case "mountain":
                    return new Tile(3, "mountain", new Dictionary<string, int>() { { "infantry", 3 } }, Game1.SpriteDict["MountainTile"]);
            }
            return new Tile(1, "plains", new Dictionary<string, int>() { { "infantry", 1 } }, Game1.SpriteDict["PlainTile"]);
        }
    }
}
