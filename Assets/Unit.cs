using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        Unit(Texture2D sprite, string type, int posX, int posY)
        {
            Sprite = sprite;
            Type = type;
            Health = 100;
            PosX = posX;
            PosY = posY;
        }
    }
}
