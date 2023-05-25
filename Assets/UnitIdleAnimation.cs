using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBSgame.Assets
{
    internal class UnitIdleAnimation : ITimedAnimation
    {
        private string _unitType;
        private string _allegiance;
        private UnitStates _updateState;
        private int _posX;
        private int _posY;

        internal UnitIdleAnimation(Unit unit)
        {
            _unitType = unit.UnitType;
            _allegiance = unit.Allegiance;
            _posX = unit.PosX;
            _posY = unit.PosY;
            _updateState = UnitStates.Idle;
        }

        public UnitStates Update(GameTime gameTime, MouseState mouse, MouseState previousMouse)
        {
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            int positionX = (_posX - (cameraX - tilesX / 2)) * (viewport.Width / tilesX);
            int positionY = (_posY - (cameraY - tilesY / 2)) * (viewport.Height / tilesY);
            var drawPoint = new Point(positionX, positionY);
            var drawSize = new Point(viewport.Width / tilesX, viewport.Height / tilesY);
            var destination = new Rectangle(drawPoint, drawSize);
            spriteBatch.Draw(Game1.SpriteDict["idle" + _unitType + _allegiance], destination, Color.White);
        }
    }
}
