using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Assets
{
    public class Building
    {
        public string Type;
        public string Allegiance;
        public int Supplies;
        public int PosX;
        public int PosY;
        public Dictionary<string,int> InfluenceList = new();
        public Texture2D Sprite;

        public Building(string type, string allegiance, int posX, int posY, Texture2D sprite)
        {
            Supplies = 80;
            Allegiance = allegiance;
            Type = type;
            PosX = posX;
            PosY = posY;
            Sprite = sprite;
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int tilesX, int tilesY, int cameraX, int cameraY)
        {
            if (PosX < cameraX - tilesX || PosX >= cameraX + tilesX || PosY < cameraY - tilesY ||
                PosY >= cameraY + tilesY) return;
            var tileWidth = (float)viewport.Width / tilesX;
            var tileHeight = (float)viewport.Height / tilesY;
            var scale = Math.Min(tileWidth / Sprite.Width, tileHeight / Sprite.Height);
            var position = new Vector2((PosX - cameraX + tilesX) * tileWidth, (PosY - cameraY + tilesY) * tileHeight);
            var origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            spriteBatch.Draw(Sprite, position, null, new Color(100), 0f, origin, scale, SpriteEffects.None, 0f);
        }


        public void Siege(string playerId)
        {
            if (playerId != Allegiance)
            {
                InfluenceList.Add(playerId,+1);
                Supplies -= 10;
            }
            else
            {
                Supplies += 10;
                Supplies = Supplies > 80 ? 80 : Supplies;
            }

            if (Supplies <= 0)
            {
                Allegiance = InfluenceList.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                Supplies = 80;
                InfluenceList.Clear();
                Sprite = Game1.SpriteDict[Allegiance+Type];
            }
        }

        public void LiftSiege()
        {
            Supplies = 80;
            InfluenceList.Clear();
        }

        public static Building CreateBuilding(string type, string allegiance, int posX, int posY)
        {
            switch (type)
            {
                case "factory":
                    return new Building(type, allegiance, posX, posY, Game1.SpriteDict[allegiance + "Factory"]);
                                
            }

            throw new Exception("CreateBuilding called with invalid type parameter");
        }
    }
}
