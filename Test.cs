using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBSgame.Assets;

namespace TBSgame
{
    public class Test
    {
        public static Map TestMap()
        {
            Tile grassTile = Tile.CreateTile("plains");
            Tile pathTile = Tile.CreateTile("path");
            Tile mountainTile = Tile.CreateTile("mountain");
            Tile forestTile = Tile.CreateTile("forest");
            Tile[,] grid = new Tile[15, 15];
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y] = grassTile;
                }
            }
            grid[7,7] = forestTile;
            grid[9, 9] = forestTile;
            grid[7, 9] = forestTile;
            grid[9, 7] = forestTile;
            grid[5, 7] = forestTile;
            grid[5, 9] = forestTile;
            grid[11, 7] = forestTile;
            grid[11, 9] = forestTile;
            grid[8, 0] = mountainTile;
            grid[8, 1] = mountainTile;
            grid[8, 2] = mountainTile;
            grid[8, 14] = mountainTile;
            grid[8, 13] = mountainTile;
            grid[4, 8] = pathTile;
            grid[5, 8] = pathTile;
            grid[6, 8] = pathTile;
            grid[7, 8] = pathTile;
            grid[8, 8] = pathTile;
            grid[9, 8] = pathTile;
            grid[10, 8] = pathTile;
            grid[11, 8] = pathTile;
            Building building = Building.CreateBuilding("factory","blue",13,7);
            Building building2 = Building.CreateBuilding("factory", "red", 3,8);
            Building building3 = Building.CreateBuilding("factory", "", 8, 8);
            Building hq1 = Building.CreateBuilding("hq","red",1,7);
            Building hq2 = Building.CreateBuilding("hq", "blue", 14, 8);
            Building[] buildings = {building,building2,building3,hq1,hq2};
            Map map = new(grid,buildings,10000);
            return map;
        }

        public static LinkedList<Unit> TestList()
        {
            LinkedList<Unit> list = new();
            list.AddLast(Unit.CreateUnit("Musketeer", "red", 2, 3,false));
            list.AddLast(Unit.CreateUnit("Musketeer", "red", 2, 8, false));
            list.AddLast(Unit.CreateUnit("Musketeer", "red", 2, 10, false));
            list.AddLast(Unit.CreateUnit("Musketeer", "blue", 13, 3, false));
            list.AddLast(Unit.CreateUnit("Musketeer", "blue", 13, 8, false));
            list.AddLast(Unit.CreateUnit("Musketeer", "blue", 13, 10, false));
            return list;
        }
    }
}
