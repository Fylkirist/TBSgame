using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace TBSgame.Assets
{
    public class Fight
    {
        private Unit _attacker;
        private Unit _defender;
        private Tile _attackerTile;
        private Tile _defenderTile;
        public Fight(Unit attacker, Unit defender, Tile attackerTile, Tile defenderTile)
        {
            _attackerTile = attackerTile;
            _defenderTile = defenderTile;
            _attacker = attacker;
            _defender = defender;
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
