using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Assets
{
    public class Tile
    {
        public int Protection;
        private string _type;
        public Dictionary<string, int> MovePenaltyDictionary;
        private readonly Texture2D _texture;

        public Tile(int protection, string type, Dictionary<string,int> movePenalty, Texture2D texture)
        {
            Protection = protection;
            _type = type;
            _texture = texture;
            MovePenaltyDictionary = movePenalty;
        }

        public void Render(int posX, int posY, SpriteBatch spriteBatch, Viewport viewport, int tilesX, int tilesY)
        {
            var drawPosX = posX * viewport.Width / tilesX;
            var drawPosY = posY * viewport.Height / tilesY;
            var scale = (float)viewport.Width / tilesX / _texture.Width; // calculate scale factor
            spriteBatch.Draw(_texture, new Vector2(drawPosX, drawPosY), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
