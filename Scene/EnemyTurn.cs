using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBSgame.Scene
{
    internal class EnemyTurn : ISubState
    {
        private BattleScene _scene;
        private BattleState _updateState;

        internal EnemyTurn(BattleScene scene)
        {
            _scene = scene;
            _updateState = BattleState.EnemyTurn;
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
