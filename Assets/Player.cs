using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace TBSgame.Assets
{
    internal class Player
    {
        public string Name;
        public string Id;
        public int Money;
        public int CameraX;
        public int CameraY;
        private bool _active;
        private float _animTimeout;

        public Player(string name, string id, int money, int cameraX, int cameraY)
        {
            Name = name;
            Id = id;
            Money = money;
            CameraX = cameraX;
            CameraY = cameraY;
            _active = false;
            _animTimeout = 0;
        }

        public void HandleInput(KeyboardState keyboard, KeyboardState previousKeyboard, GameTime gameTime)
        {
            if (_active)
            {
                return;
            }
            if (keyboard.IsKeyDown(Keys.A) && previousKeyboard.IsKeyDown(Keys.A))
            {
                _animTimeout = 0.3f;
                CameraX -= 1;
            }
            if (keyboard.IsKeyDown(Keys.S) && previousKeyboard.IsKeyDown(Keys.S))
            {
                _animTimeout = 0.3f;
                CameraY += 1;
            }
            if (keyboard.IsKeyDown(Keys.D) && previousKeyboard.IsKeyDown(Keys.D))
            {
                _animTimeout = 0.3f;
                CameraX += 1;
            }
            if (keyboard.IsKeyDown(Keys.W) && previousKeyboard.IsKeyDown(Keys.W))
            {
                _animTimeout = 0.3f;
                CameraY -= 1;
            }
            
        }
    }
}
