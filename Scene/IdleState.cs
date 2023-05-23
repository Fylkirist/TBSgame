using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBSgame.Assets;

namespace TBSgame.Scene
{
    public class IdleState : ISubState
    {
        private Vector2Int _pointer;
        private BattleScene _scene;
        private BattleState _updateState;

        public IdleState(Vector2Int pointer, BattleScene scene)
        {
            _pointer = pointer;
            _scene = scene;
            _updateState = BattleState.Idle;
        }

        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            var tilesX = (int)(Game1._viewport.Width / _scene.TileSize.X);
            var tilesY = (int)(Game1._viewport.Height / _scene.TileSize.Y);
            var x = (int)(mouse.X / _scene.TileSize.X + (_scene.Camera.X - tilesX * 0.5));
            var y = (int)(mouse.Y / _scene.TileSize.Y + (_scene.Camera.Y - tilesY * 0.5));
            var mapSize = _scene.GetMapSize();
            if (x>=0 && x<mapSize.X && y>=0 && y<mapSize.Y)
            {
                _pointer.X = x;
                _pointer.Y = y;
            }
            if (mouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                _scene.SelectUnit(_pointer);
        }

        public BattleState CheckState()
        {
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            var tilesX = (int)(Game1._viewport.Width / _scene.TileSize.X);
            var tilesY = (int)(Game1._viewport.Height / _scene.TileSize.Y);
            var markerRect = new Rectangle(
                new Point(
                    (int)((_pointer.X - (_scene.Camera.X - tilesX / 2)) * _scene.TileSize.X),
                    (int)((_pointer.Y - (_scene.Camera.Y - tilesY / 2)) * _scene.TileSize.Y)
                ),
                new Point((int)_scene.TileSize.X, (int)_scene.TileSize.Y)
            );
            spriteBatch.Draw(Game1.SpriteDict["SelectionMarker"], markerRect, Color.White);
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"], _pointer.ToString(), new Vector2(50, 0), Color.White);
        }
    }
}
