using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBSgame.Assets;

namespace TBSgame.Scene
{
    internal class FightTargetSelection : ISubState
    {
        private Unit[] _targets;
        private Vector2Int _pointer;
        private Unit _unit;
        private BattleScene _scene;
        private BattleState _updateState;
        private ISubState _cachedState;
        private Path _movePath;

        internal FightTargetSelection(Unit unit, Unit[] targets, BattleScene scene, ISubState cache, Path path)
        {
            _scene = scene;
            _targets = targets;
            _unit = unit;
            _pointer = new Vector2Int(targets[0].PosX, targets[0].PosY);
            _updateState = BattleState.Fight;
            _cachedState = cache;
            _movePath = path;
        }

        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            var tilesX = (int)(Game1._viewport.Width / _scene.TileSize.X);
            var tilesY = (int)(Game1._viewport.Height / _scene.TileSize.Y);
            var x = (int)(mouse.X / _scene.TileSize.X + (_scene.Camera.X - tilesX * 0.5));
            var y = (int)(mouse.Y / _scene.TileSize.Y + (_scene.Camera.Y - tilesY * 0.5));
            if (_targets.Any(unit => unit.PosX == x && unit.PosY == y))
            {
                _pointer.X = x;
                _pointer.Y = y;
            }

            if (mouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
            {
                var target = _targets.FirstOrDefault(unit => unit.PosX == _pointer.X && unit.PosY == _pointer.Y);
                if (target == null) return;
                _unit.MoveUnit(_movePath);
                _scene.StartAttack(_unit,target);
            }

            if (mouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed)
            {
                _scene.PreviousState(_cachedState);
            }
        }

        public BattleState CheckState()
        {
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            var tilesX = (int)(Game1._viewport.Width / _scene.TileSize.X);
            var tilesY = (int)(Game1._viewport.Height / _scene.TileSize.Y);
            var markerRect =
                new Rectangle(
                    new Point((int)((_pointer.X - (_scene.Camera.X - tilesX / 2)) * _scene.TileSize.X), (int)((_pointer.Y - (_scene.Camera.Y - tilesY / 2)) * _scene.TileSize.Y)),
                    new Point((int)_scene.TileSize.X, (int)_scene.TileSize.Y));
            spriteBatch.Draw(Game1.SpriteDict["SelectionMarker"], markerRect, Color.White);
        }
    }
}
