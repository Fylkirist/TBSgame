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
    public class Fight : ISubState
    {
        private Unit _attacker;
        private Unit _defender;
        private Tile _attackerTile;
        private Tile _defenderTile;
        private float _animationTimer;
        private BattleState _updateState;

        public Fight(Unit attacker, Unit defender, Tile attackerTile, Tile defenderTile)
        {
            _attackerTile = attackerTile;
            _defenderTile = defenderTile;
            _attacker = attacker;
            _defender = defender;
            _animationTimer = 10;
            _updateState = BattleState.Fight;
        }

        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public BattleState CheckState()
        {
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
