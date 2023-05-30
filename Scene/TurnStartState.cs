using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBSgame.Assets;

namespace TBSgame.Scene
{
    internal class TurnStartState : ISubState
    {
        private BattleScene _scene;
        private Player _player;
        private BattleState _updateState;

        internal TurnStartState(BattleScene scene, Player player)
        {
            _scene = scene;
            _player = player;
            _updateState = BattleState.TurnStart;
        }

        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            _scene.StartTurn(_player);
            _updateState = BattleState.Idle;
            _scene.UpdateState(BattleState.Idle);
        }

        public BattleState CheckState()
        {
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            
        }
    }
}
