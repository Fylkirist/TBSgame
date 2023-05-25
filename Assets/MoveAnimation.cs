using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBSgame.Scene;

namespace TBSgame.Assets
{
    internal class MoveAnimation : ITimedAnimation
    {
        private double _animTimer;
        private Vector2Int[] _path;
        private int _currentFrame;
        private string[] _frames;
        private double _duration;
        private UnitStates _unitState;

        internal MoveAnimation(Path path, Unit unit)
        {
            var pathList = path.Positions;
           
            _path = pathList.ToArray();
            Array.Reverse(_path);
            _animTimer = 0;
            _duration = path.Positions.Count;
            _currentFrame = 0;
            _unitState = UnitStates.Moving;
            _frames = new[] {"move0" + unit.UnitType + unit.Allegiance,"move1" + unit.UnitType + unit.Allegiance};
        }

        public UnitStates Update(GameTime gameTime, MouseState mouse, MouseState previousMouse)
        {
            _animTimer += gameTime.ElapsedGameTime.TotalSeconds;

            _currentFrame = gameTime.ElapsedGameTime.TotalSeconds - Math.Floor(gameTime.ElapsedGameTime.TotalSeconds) > 0.5? 1:0;
            if (_animTimer > 0.5 && mouse.LeftButton == ButtonState.Released &&
                previousMouse.LeftButton == ButtonState.Pressed)
            {
                _unitState = UnitStates.Tapped;
            }
            if (_animTimer >= _duration)
            {
                _unitState = UnitStates.Tapped;
            }
            return _unitState;
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            var tileSize = new Vector2Int(viewport.Width / tilesX, viewport.Height / tilesY);
            var xOffset = cameraX - tilesX / 2;
            var yOffset = cameraY - tilesY / 2;

            if (_animTimer >= _duration) return;

            var currentIndex = (int)Math.Floor(_animTimer);
            var nextIndex = currentIndex + 1;

            if (nextIndex >= _path.Length)
            {
                nextIndex = _path.Length - 1;
            }

            var currentPos = _path[currentIndex];
            var nextPos = _path[nextIndex];

            var fractionalPart = _animTimer - Math.Floor(_animTimer);

            var posX = currentPos.X + (nextPos.X - currentPos.X) * fractionalPart;
            var posY = currentPos.Y + (nextPos.Y - currentPos.Y) * fractionalPart;

            int positionX = (int)((posX - xOffset) * tileSize.X);
            int positionY = (int)((posY - yOffset) * tileSize.Y);

            spriteBatch.Draw(Game1.SpriteDict[_frames[_currentFrame]], new Rectangle(positionX, positionY, tileSize.X, tileSize.Y), Color.White);
        }

    }
}
