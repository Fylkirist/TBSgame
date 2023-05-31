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
    public enum BuildingType
    {
        HQ,
        Factory,
        Town
    }
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
            int positionX = (PosX - (cameraX - tilesX / 2)) * (viewport.Width / tilesX);
            int positionY = (PosY - (cameraY - tilesY / 2)) * (viewport.Height / tilesY);
            var drawPoint = new Point(positionX, positionY);
            var drawSize = new Point(viewport.Width / tilesX, viewport.Height / tilesY);
            var destination = new Rectangle(drawPoint, drawSize);
            var color = Allegiance == "red" ? Color.Red : Allegiance == "blue" ? Color.Blue : Color.White;
            spriteBatch.Draw(Sprite, destination,color);
        }

        public void Siege(string playerId)
        {
            if (playerId != Allegiance)
            {
                if (InfluenceList.ContainsKey(playerId))
                {
                    InfluenceList[playerId] += 1;
                }
                else
                {
                    InfluenceList.Add(playerId,1);
                }
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

        public void LiftSiege(string playerId)
        {
            if (InfluenceList.ContainsKey(playerId))
            {
                Supplies = 80;
                InfluenceList.Clear();
            }
        }

        public static Building CreateBuilding(string type, string allegiance, int posX, int posY)
        {
            return type switch
            {
                "factory" => new Building(type, allegiance, posX, posY, Game1.SpriteDict[allegiance + "factory"]),
                "hq" => new Building(type, allegiance, posX, posY, Game1.SpriteDict[allegiance + "hq"]),
                "town" => new Building(type, allegiance, posX, posY, Game1.SpriteDict[allegiance + "town"]),
                "airport" => new Building(type, allegiance, posX, posY, Game1.SpriteDict[allegiance + "airport"]),
                _ => throw new Exception("CreateBuilding called with invalid type parameter")
            };
        }
    }
}
