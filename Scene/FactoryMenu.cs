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
    internal class FactoryMenu : ISubState
    {
        private BattleScene _scene;
        private Building _building;
        private BattleState _updateState;

        public FactoryMenu(BattleScene scene, Building building)
        {
            _scene = scene;
            _building = building;
            _updateState = BattleState.FactoryMenu;
        }
        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            
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
