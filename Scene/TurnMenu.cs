using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBSgame.Scene
{
    internal class TurnMenu : ISubState
    {
        private BattleScene _scene;
        private Menu _menu;
        private BattleState _battleState;

        internal TurnMenu(BattleScene scene)
        {
            _menu = new Menu(new Dictionary<string, MenuItem.MenuItemAction>
            {
                {"End turn",EndTurn},
                {"Reset Battle",ResetGame},
                {"Exit",ExitBattle}
            },"Game Menu","placeholder",0,new Vector2(200,100),Game1._viewport.Width/2, Game1._viewport.Height/2);
            _scene = scene;
            _battleState = BattleState.GameMenu;
        }
        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            _menu.HandleInput(mouse,previousMouse);
            if (mouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed)
            {
                _battleState = BattleState.Idle;
                _scene.UpdateState(_battleState);
            }
        }

        public BattleState CheckState()
        {
            return _battleState;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            _menu.Render(spriteBatch);
        }

        private void EndTurn()
        {
            _scene.EndTurn();
        }

        private void ResetGame()
        {

        }

        private void ExitBattle()
        {

        }
    }
}
