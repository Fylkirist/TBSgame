using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using TBSgame.Assets;

namespace TBSgame.Scene
{
    internal class UnitSelectedState : ISubState
    {
        private Unit _selectedUnit;
        private BattleState _stateUpdate;
        private List<Path> _paths;
        private BattleScene _scene;
        private Vector2Int _pointer;

        public UnitSelectedState(Unit selectedUnit, List<Path> paths, BattleScene parent)
        {
            _selectedUnit = selectedUnit;
            _stateUpdate = BattleState.Selected;
            _paths = paths;
            _scene = parent;
            _pointer = new Vector2Int(selectedUnit.PosX, selectedUnit.PosY);
        }

        public void Update(MouseState mouse, MouseState previousMouse,GameTime gameTime)
        {
            var tilesX = (int)(Game1._viewport.Width / _scene.TileSize.X);
            var tilesY = (int)(Game1._viewport.Height / _scene.TileSize.Y);
            var x = (int)(mouse.X / _scene.TileSize.X + (_scene.Camera.X - tilesX * 0.5));
            var y = (int)(mouse.Y / _scene.TileSize.Y + (_scene.Camera.Y - tilesY * 0.5));
            if (_paths.Any(path => x == path.Positions[0].X && y == path.Positions[0].Y))
            {
                _pointer.X = x; _pointer.Y = y;
            }

            if (mouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
            {
                var path = _paths.FirstOrDefault(p => p.Positions[0] == _pointer);
                _scene.SelectMove(path,_selectedUnit,new Vector2Int(mouse.X,mouse.Y));
                _scene.UpdateState(BattleState.MoveMenu);
            }
            if (mouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed)
            {
                _scene.UpdateState(BattleState.Idle);
            }
        }

        public BattleState CheckState()
        {
            return _stateUpdate;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            var tilesX = (int)(Game1._viewport.Width / _scene.TileSize.X);
            var tilesY = (int)(Game1._viewport.Height / _scene.TileSize.Y);

            foreach (var path in _paths)
            {
                var coordinate = path.Positions[0];
                var destinationPoint = new Point((int)((coordinate.X - (_scene.Camera.X - tilesX / 2)) * _scene.TileSize.X), (int)((coordinate.Y - (_scene.Camera.Y - tilesY / 2)) * _scene.TileSize.Y));
                var size = new Point((int)_scene.TileSize.X, (int)_scene.TileSize.Y);
                var destination = new Rectangle(destinationPoint, size);
                spriteBatch.Draw(Game1.SpriteDict["AvailableTileBorder"], destination, Color.White);
                if (_pointer.X == path.Positions[0].X && _pointer.Y == path.Positions[0].Y)
                {
                    path.DrawPath(spriteBatch, _scene.TileSize.X, _scene.TileSize.Y, tilesX, tilesY, _scene.Camera.X, _scene.Camera.Y);
                }
            }
            var markerRect =
                new Rectangle(
                    new Point((int)((_pointer.X - (_scene.Camera.X - tilesX / 2)) * _scene.TileSize.X), (int)((_pointer.Y - (_scene.Camera.Y - tilesY / 2)) * _scene.TileSize.Y)),
                    new Point((int)_scene.TileSize.X, (int)_scene.TileSize.Y));
            spriteBatch.Draw(Game1.SpriteDict["SelectionMarker"], markerRect, Color.White);
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"], _pointer.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"], new StringBuilder("Selected!"), new Vector2(50, 0), Color.White);
        }
    }
}
