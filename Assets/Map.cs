using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            var tileWidth = viewport.Width / tilesX;
            var tileHeight = viewport.Height / tilesY;

            for (var col = cameraX - tilesX / 2; col < cameraX + tilesX / 2; col++)
            {
                for (var row = cameraY - tilesY / 2; row < cameraY + tilesY / 2; row++)
                {
                    if (row < 0 || col < 0 || row >= MapGrid.GetLength(1) || col >= MapGrid.GetLength(0)) continue;
                    var positionX = (col - (cameraX - tilesX / 2)) * tileWidth;
                    var positionY = (row - (cameraY - tilesY / 2)) * tileHeight;
                    var position = new Point(positionX, positionY);
                    var size = new Point(tileWidth, tileHeight);
                    var destination = new Rectangle(position, size);

                    MapGrid[col, row].Render(destination, spriteBatch);
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

        public void UpdateSiege(LinkedList<Unit> units,string playerId)
        {
            foreach (var building in Buildings)
            {
                int siegeCount = 0;
                foreach (var unit in units.Where(unit => Math.Abs(unit.PosX - building.PosX) <= 1 && Math.Abs(unit.PosY - building.PosY) <= 1 && unit.Allegiance == playerId))
                {
                    siegeCount++;
                    building.Siege(unit.Allegiance);
                }

                if (siegeCount == 0 && building.Allegiance != playerId)
                {
                    building.LiftSiege();
                }
            }
        }

        public Building CheckBuildingSelection(int posY,int posX)
        {
            return Buildings.FirstOrDefault(building => building.PosX == posX && building.PosY == posY);
        }
    }
}
