using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBSgame.Assets
{
    public interface IUnitAnimation
    {
        void Update(GameTime gameTime);
        void Play();
        void Stop();
        void Reset();
        void Render(SpriteBatch spriteBatch, Vector2 position);
    }

}
