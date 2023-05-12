using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Assets
{
    public class Unit
    {
        public Texture2D Sprite;
        public string Type;
        public int Health;
        public int PosX;
        public int PosY;
        public string Allegiance;
        public int Movement;

        Unit(Texture2D sprite, string type, int posX, int posY, string playerId, int movement)
        {
            Sprite = sprite;
            Type = type;
            Health = 100;
            PosX = posX;
            PosY = posY;
            Allegiance = playerId;
            Movement = movement;
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            var drawPosX = PosX - cameraX;
            drawPosX = drawPosX * viewport.Width / tilesX;
            var drawPosY = PosY - cameraY;
            drawPosY = drawPosY * viewport.Height / tilesY;
            var scale = Math.Min(viewport.Width / tilesX / Sprite.Width, viewport.Height / tilesY / Sprite.Height);
            spriteBatch.Draw(Sprite, new Vector2(drawPosX, drawPosY), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
