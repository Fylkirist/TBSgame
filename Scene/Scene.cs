using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Scene
{
    public interface IScene
    {
        public void Render(SpriteBatch spriteBatch, Viewport viewport);

        public void HandleInput(InputKeyEventArgs input);
    }
}
