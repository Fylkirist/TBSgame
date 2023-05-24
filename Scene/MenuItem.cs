using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBSgame.Scene
{
    public class MenuItem
    {
        private string _text;
        private MenuItemAction _action;
        private bool _selected;
        private Vector2 _position;
        private Vector2 _size;
        private Texture2D _texture;
        private SpriteFont _font;

        public bool Selected => _selected;

        public delegate void MenuItemAction();

        public MenuItem(string text, MenuItemAction action, Vector2 size, Vector2 position, Texture2D texture, SpriteFont font)
        {
            _text = text;
            _action = action;
            _selected = false;
            _size = size;
            _position = position;
            _texture = texture;
            _font = font;
        }

        public void HandleInput(MouseState mouse, MouseState previousMouse)
        {
            if (mouse.X > _position.X && mouse.X < _position.X + _size.X &&
                mouse.Y > _position.Y && mouse.Y < _position.Y + _size.Y)
            {
                _selected = true;
            }
            else
            {
                _selected = false;
            }

            if (_selected && mouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
            {
                _action();
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            Color colour = _selected ? Color.Yellow : Color.White;
            Rectangle drawRect = new Rectangle(new Point((int)_position.X, (int)_position.Y), new Point((int)_size.X,(int)_size.Y));
            spriteBatch.Draw(_texture,drawRect ,colour);

            Vector2 textPosition = new Vector2(_position.X + _size.X / 2f, _position.Y + _size.Y / 2f);
            Vector2 textSize = _font.MeasureString(_text);
            textPosition -= textSize / 2f;
            spriteBatch.DrawString(_font, _text, textPosition, Color.Black);
        }

    }
}
