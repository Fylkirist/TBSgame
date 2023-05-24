using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBSgame.Assets;

namespace TBSgame.Scene
{
    internal class FactoryMenu : ISubState
    {
        private BattleScene _scene;
        private Building _building;
        private BattleState _updateState;
        private string _type;
        private List<MenuItem> _options;
        private string[] _optionKeys;
        private int _selected;
        private Player _player;
        public FactoryMenu(BattleScene scene, Building building, Player player)
        {
            _scene = scene;
            _building = building;
            _updateState = BattleState.FactoryMenu;
            _type = building.Type;
            _options = new List<MenuItem>();
            _optionKeys = building.Type == "factory" ? new string [] { "musketeer", "crabTank" } : new string[] {  "zeppelin" };

            foreach (var unitKey in _optionKeys)
            {
                _options.Add( new MenuItem(unitKey,BuyUnit,new Vector2(100,50),new Vector2(400, 400), Game1.SpriteDict["placeholderButton"], Game1.Fonts["placeholderFont"]));
            }
            _selected = 0;

            _player = player;
        }
        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            for (var i = 0; i < _options.Count; i++)
            {
                _options[i].HandleInput(mouse,previousMouse);
                if (_options[i].Selected)
                {
                    _selected = i;
                    break;
                }
            }
        }

        public BattleState CheckState()
        {
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var item in _options)
            {
                item.Render(spriteBatch);
            }
        }

        public void BuyUnit()
        {

        }
    }
}
