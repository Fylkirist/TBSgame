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
            Tile[,] grid = new Tile[50, 50];
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y] = grassTile;
                }
            }
            Building building = Building.CreateBuilding("factory","",1,1);
            Building building2 = Building.CreateBuilding("factory", "red", 11,11);
            Building[] buildings = {building,building2};
            Map map = new(grid,buildings,10000);
            return map;
        }

        public static LinkedList<Unit> TestList()
        {
            LinkedList<Unit> list = new();
            list.AddLast(Unit.CreateUnit("Musketeer", "red", 0, 0,false));
            list.AddLast(Unit.CreateUnit("Musketeer", "red", 0, 1, false));
            list.AddLast(Unit.CreateUnit("Musketeer", "red", 1, 1, false));
            list.AddLast(Unit.CreateUnit("Musketeer", "blue", 1, 0, false));
            list.AddLast(Unit.CreateUnit("Musketeer", "blue", 0, 2, false));
            list.AddLast(Unit.CreateUnit("Musketeer", "blue", 2, 1, false));
            return list;
        }
    }
}
