using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBSgame.Assets
{
    internal class Player
    {
        public string Name;
        public string Id;
        public int Money;
        public int PointerX;
        public int PointerY;
        public int CameraX;
        public int CameraY;

        public Player(string name, string id, int money, int pointerX, int pointerY)
        {
            Name = name;
            Id = id;
            Money = money;
            PointerX = pointerX;
            PointerY = pointerY;
            CameraX = pointerX;
            CameraY = pointerY;
        }

        public void Update()
        {

        }

        public void Render()
        {

        }
    }
}
