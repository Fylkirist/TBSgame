using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using TBSgame.Scene;

namespace TBSgame.Assets
{
    internal class FactoryMenu
    {
        private List<MenuItem> _menuItems;
        public FactoryMenu()
        {
            _menuItems = new List<MenuItem>();
        }

        public void Render(SpriteBatch spriteBatch)
        {
            
        }

        public void HandleInput(MouseState mouse, MouseState previousMouse)
        {
            foreach (var menuItem in _menuItems)
            {
                menuItem.HandleInput(mouse,previousMouse);
            }
        }
    }
}
