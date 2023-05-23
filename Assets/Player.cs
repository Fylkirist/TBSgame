using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace TBSgame.Assets
{
    internal class Player
    {
        public string Name;
        public string Id;
        public int Money;

        public Player(string name, string id, int money)
        {
            Name = name;
            Id = id;
            Money = money;
        }
    }
}
