using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Assets
{
    public class Map
    {
        public Tile[,] MapGrid;
        public Building[] Buildings;
        public int Money;

        public Map(Tile[,] grid, Building[] buildings, int money)
        {
            MapGrid = grid;
            Buildings = buildings;
            Money = money;
        }

        public void Render(SpriteBatch spriteBatch,Viewport viewport, int cameraX, int cameraY,int tilesX,int tilesY)
        {
            for (int col = cameraY-tilesX; col < cameraY+tilesX;col++)
            {
                for (int row = cameraX - tilesY; row < MapGrid.GetLength(1)+tilesY; row++)
                {
                    if (row >= 0 && col >= 0 && row < MapGrid.GetLength(1) && col < MapGrid.GetLength(0))
                        MapGrid[col, row].Render(col * ((int)Math.Floor((double)(viewport.Width / (tilesX*2)))), row * ((int)Math.Floor((double)(viewport.Height / (tilesY * 2)))), spriteBatch);
                }
            }

            foreach (var building in Buildings)
            {
                building.Render(spriteBatch,viewport,tilesX,tilesY,cameraX,cameraY);
            }
        }

        public int CheckAllegiance(string playerId)
        {
            int ownershipCount = 0;
            foreach (var building in Buildings)
            {
                if (building.Allegiance == playerId)
                {
                    ownershipCount++;
                }
            }
            return ownershipCount;
        }
    }
}
