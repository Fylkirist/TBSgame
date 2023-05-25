using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBSgame.Scene
{
    public class Menu
    {
        private string _name;
        private List<MenuItem> _menuItems;
        private string _style;
        private int _gapY;
        private int _posX;
        private int _posY;
        public Menu(Dictionary<string,MenuItem.MenuItemAction> items, string name, string style,int gapY, Vector2 buttonSize, int posX, int posY)
        {
            _name = name;
            _style = style;
            var buttonTexture = Game1.SpriteDict[$"{style}Button"];
            var font = Game1.Fonts[$"{style}Font"];
            _gapY = gapY;
            _posX = posX;
            _posY = posY;
            _menuItems = new List<MenuItem>();
            var widthOffset = (int)Math.Floor(buttonSize.X * 0.5);
            var heightOffset = (int)Math.Floor(buttonSize.Y * 0.5);
            var counter = 0;
            foreach (var item in items)
            {
                _menuItems.Add(new MenuItem(item.Key, item.Value, buttonSize, new Vector2(posX - widthOffset, posY-heightOffset+((int)buttonSize.Y+gapY)*counter),buttonTexture,font));
                counter++;
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var option in _menuItems)
            {
               option.Render(spriteBatch);
            }   
        }

        public void HandleInput(MouseState mouse, MouseState previousMouse)
        {
            foreach (var button in _menuItems)
            {
                button.HandleInput(mouse,previousMouse);
            }
        }
    }
}