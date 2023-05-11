using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Scene
{
    public class Menu
    {
        private string _name;
        private Dictionary<string, dynamic> _menuItems;
        private string _style;
        private int _selected;
        private Texture2D _buttonTexture;
        private SpriteFont _font;
        
        public Menu(Dictionary<string,dynamic> items, string name, string style)
        {
            _menuItems = items;
            _name = name;
            _style = style;
            _selected = -1;
            _buttonTexture = Game1.SpriteDict[$"{style}Button"];
            _font = Game1.Fonts[$"{style}Font"];
        }

        public void Render(int posX, int posY, SpriteBatch spriteBatch,int gap)
        {
            int widthOffset = (int)Math.Floor(_buttonTexture.Width * 0.5);
            int heightOffset = (int)Math.Floor(_buttonTexture.Height * 0.5);
            int yadd = 0;
            foreach (var option in _menuItems)
            {
                spriteBatch.Draw(_buttonTexture, new Vector2(posX-widthOffset,posY+yadd), Color.White);
                spriteBatch.DrawString(_font,new StringBuilder(option.Key),new Vector2(posX,posY+heightOffset+yadd),Color.Black);

                yadd += _buttonTexture.Height + gap;
            }
        }

        public void HandleInput()
        {

        }
    }
}