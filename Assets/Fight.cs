using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using TBSgame.Scene;

namespace TBSgame.Assets
{
    public class Fight
    {
        private Unit _attacker;
        private Unit _defender;
        private Tile _attackerTile;
        private Tile _defenderTile;
        private float _animationTimer;
        public Fight(Unit attacker, Unit defender, Tile attackerTile, Tile defenderTile)
        {
            _attackerTile = attackerTile;
            _defenderTile = defenderTile;
            _attacker = attacker;
            _defender = defender;
            _animationTimer = 10;
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public BattleState Update(GameTime gameTime, MouseState mouse, MouseState previousMouse)
        {
            return BattleState.Fight;
        }
    }
}
