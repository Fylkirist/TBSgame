using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public string Allegiance;
        public int Movement;

        Unit(Texture2D sprite, string type, int posX, int posY, string playerId, int movement)
        {
            Sprite = sprite;
            Type = type;
            Health = 100;
            PosX = posX;
            PosY = posY;
            Allegiance = playerId;
            Movement = movement;
        }
    }
}
