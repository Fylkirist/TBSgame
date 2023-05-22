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
        private GameState _updateState;

        public TitleScreen(Viewport viewport)
        {
            _menu = new Menu(new Dictionary<string, MenuItem.MenuItemAction>{{"test",StartGame},{"foo",StartGame } },"TitleMenu","placeholder", 20,new Vector2(200,50), (int)Math.Floor(viewport.Width * 0.5),(int)Math.Floor(viewport.Height * 0.5));
            _backgroundImg = Game1.SpriteDict["placeholderTitle"];
            _updateState = GameState.TitleScreen;
            _viewport = viewport;
        }
        public void Render(SpriteBatch spriteBatch, Viewport viewport)
        {
            spriteBatch.Draw(_backgroundImg, _viewport.Bounds, Color.White);
            _menu.Render(spriteBatch);
        }

        public void HandleInput(MouseState mouse, MouseState previousMouse, KeyboardState keyboard, KeyboardState previousKeyboard, GameTime gameTime)
        {
            _menu.HandleInput(mouse,previousMouse);
        }

        public GameState CheckStateUpdate()
        {
            return _updateState;
        }

        public void StartGame()
        {
            _updateState = GameState.BattleScene;
        }
    }
}

