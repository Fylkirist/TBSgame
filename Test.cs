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
            Tile[,] grid = new Tile[10, 10];
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y] = grassTile;
                }
            }
            Building building = Building.CreateBuilding("factory","",0,0);
            Building building2 = Building.CreateBuilding("factory", "red", 0, 0);
            Building[] buildings = {building};
            Map map = new(grid,buildings,10000);
            return map;
        }

        public static LinkedList<Unit> TestList()
        {
            LinkedList<Unit> list = new();
            list.AddLast(Unit.CreateUnit("Musketeer", "red", 0, 0));
            list.AddLast(Unit.CreateUnit("Musketeer", "blue", 10, 10));
            return list;
        }
    }
}
