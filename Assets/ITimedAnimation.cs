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
    internal interface ITimedAnimation
    {
        public UnitStates Update(GameTime gameTime, MouseState mouse, MouseState previousMouse);

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY);
    }
}
