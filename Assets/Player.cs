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
    internal class Player
    {
        public string Name;
        public string Id;
        public int Money;
        public int PointerX;
        public int PointerY;
        public int CameraX;
        public int CameraY;

        public Player(string name, string id, int money, int pointerX, int pointerY)
        {
            Name = name;
            Id = id;
            Money = money;
            PointerX = pointerX;
            PointerY = pointerY;
            CameraX = pointerX;
            CameraY = pointerY;
        }

        public void HandleInput(KeyboardState keyboard, KeyboardState previousKeyboard)
        {
            if (keyboard.IsKeyDown(Keys.A) && previousKeyboard.IsKeyUp(Keys.A))
            {
                CameraX -= 1;
            }
            if (keyboard.IsKeyDown(Keys.S) && previousKeyboard.IsKeyUp(Keys.S))
            {
                CameraY -= 1;
            }
            if (keyboard.IsKeyDown(Keys.D) && previousKeyboard.IsKeyUp(Keys.D))
            {
                CameraX += 1;
            }
            if (keyboard.IsKeyDown(Keys.W) && previousKeyboard.IsKeyUp(Keys.W))
            {
                CameraY += 1;
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            string camX = CameraX.ToString();
            string camY = CameraY.ToString();
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"],new StringBuilder(camX + "X " + camY + " Y"),new Vector2(0,0), Color.White);
        }
    }
}
