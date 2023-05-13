using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBSgame.Scene
{
    internal class TitleScreen : IScene
    {
        private Texture2D _backgroundImg;
        private Menu _menu;
        private Viewport _viewport;

        public TitleScreen(Viewport viewport)
        {
            _menu = new Menu(new Dictionary<string, MenuItem.MenuItemAction>{{"test",Test},{"foo",Test} },"TitleMenu","placeholder", 20,new Vector2(200,50), (int)Math.Floor(viewport.Width * 0.5),(int)Math.Floor(viewport.Height * 0.5));
            _backgroundImg = Game1.SpriteDict["placeholderTitle"];
        }
        public void Render(SpriteBatch spriteBatch, Viewport viewport)
        {
            spriteBatch.Draw(_backgroundImg, viewport.Bounds, Color.White);
            _menu.Render(spriteBatch);
        }

        public void HandleInput(MouseState mouse,MouseState previousMouse)
        {
            _menu.HandleInput(mouse,previousMouse);
        }

        public void Test()
        {
            Console.WriteLine("working");
        }
    }
}

