using System;
using System.Collections.Generic;
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

        public void Render(SpriteBatch spriteBatch,Viewport viewport, int tilesX, int tilesY,int cameraX, int cameraY)
        {
            if (PosX >= cameraX-tilesX && PosX < cameraX+tilesX && PosY >= cameraY-tilesY && PosY < cameraY+tilesY)
            {
                int xOffset = viewport.Width / tilesX;
                int yOffset = viewport.Height / tilesY;
                spriteBatch.Draw(Sprite,new Vector2(PosX*xOffset,PosY*yOffset),new Color(100));
            }
        }

        public void Siege(string playerId)
        {
            if (playerId != Allegiance)
            {
                InfluenceList.Add(playerId,+1);
                Supplies -= 10;
            }

            if (Supplies <= 0)
            {
                Allegiance = InfluenceList.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                Supplies = 80;
                InfluenceList.Clear();
            }
        }

        public void LiftSiege()
        {
            Supplies = 80;
            InfluenceList.Clear();
        }
    }
}
