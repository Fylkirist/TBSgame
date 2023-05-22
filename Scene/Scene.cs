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
    public interface IScene
    {
        public void Render(SpriteBatch spriteBatch, Viewport viewport);

        public void HandleInput(MouseState mouse,MouseState previousMouse,KeyboardState keyboard, KeyboardState previousKeyboard,GameTime gameTime);

        public GameState CheckStateUpdate();
    }
}
