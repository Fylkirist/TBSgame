using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Scene
{
    internal class TitleScreen : IScene
    {
        private Texture2D _backgroundImg;
        private Menu _menu;

        public TitleScreen()
        {
            _menu = new Menu(new Dictionary<string, dynamic>(){{"test","test"},{"foo","bar"}},"TitleMenu","placeholder");
            _backgroundImg = Game1.SpriteDict["placeholderTitle"];
        }
        public void Render(SpriteBatch spriteBatch, Viewport viewport)
        {
            spriteBatch.Draw(_backgroundImg,new Vector2(0,0),Color.White);
            int resultX = (int)Math.Floor(viewport.Width * 0.5);
            int resultY = (int)Math.Floor(viewport.Height * 0.3);
            _menu.Render(resultX,resultY,spriteBatch,20);
        }

        public void HandleInput(InputKeyEventArgs input)
        {
            
        }

    }
}

