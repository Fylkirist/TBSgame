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
    internal class UnitMoveMenu : ISubState
    {
        private BattleScene _scene;
        private Path _path;
        private BattleState _updateState;
        private Menu _menu;
        private Unit _unit;
        private Vector2Int[] _targets;

        internal UnitMoveMenu(BattleScene scene, Path path,Unit unit, Vector2Int[] targets,int posX, int posY)
        {
            _scene = scene;
            _path = path;
            _updateState = BattleState.MoveMenu;
            Dictionary<string, MenuItem.MenuItemAction> actions;
            actions = targets.Length > 0 ? new Dictionary<string, MenuItem.MenuItemAction> {{"fight",Fight},{"wait",Wait}} : new Dictionary<string, MenuItem.MenuItemAction> { { "wait", Wait } };
            _menu = new Menu(actions, "Actions", "placeholder", 0, new Vector2(20, 20), posX, posY);
            _unit = unit;
            _targets = targets;
        }


        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            if (mouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed)
            {
                _scene.SelectUnit(new Vector2Int(_unit.PosX,_unit.PosY));
            }
            _menu.HandleInput(mouse,previousMouse);
        }

        public BattleState CheckState()
        {
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            int tilesX = (int)(Game1._viewport.Width / _scene.TileSize.X);
            int tilesY = (int)(Game1._viewport.Height / _scene.TileSize.Y);
            _path.DrawPath(spriteBatch,_scene.TileSize.X,_scene.TileSize.Y,tilesX,tilesY,_scene.Camera.X,_scene.Camera.Y);
            _menu.Render(spriteBatch);
        }

        public void Wait()
        {
            _unit.MoveUnit(_path);
            _scene.UpdateState(BattleState.Idle);
        }

        public void Fight()
        {
            _scene.OpenFightMenu(_path,_unit,_targets);
        }
    }
}
