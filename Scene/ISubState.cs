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
    public interface ISubState
    {
        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime);

        public BattleState CheckState();

        public void Render(SpriteBatch spriteBatch);
    }
}
