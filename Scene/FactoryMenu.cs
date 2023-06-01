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
        private string[] _optionKeys;
        private Unit _selected;
        private Player _player;

        public FactoryMenu(BattleScene scene, Building building, Player player)
        {
            _scene = scene;
            _building = building;
            _updateState = BattleState.FactoryMenu;
            _type = building.Type;
            _optionKeys = building.Type == "factory" ? new[] { "Musketeer" } : new[] {  "Zeppelin" };
            _selected = Unit.CreateUnit("Musketeer",player.Id,building.PosX,building.PosY,true);
            _player = player;
        }
        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            for (var i = 0; i < _optionKeys.Length; i++)
            {
                var optionPosX = 450;
                var optionPosY = 120 + 50 * i;
                if (mouse.X > optionPosX && mouse.Y > optionPosY && mouse.X < optionPosX+300 && mouse.Y < optionPosY+50)
                {
                    _selected = _selected.UnitType == _optionKeys[i]
                        ? _selected
                        : Unit.CreateUnit(_optionKeys[i], _player.Id, _building.PosX, _building.PosY, true);
                    break;
                }
            }

            if (mouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
            {
                if (_player.Money >= _selected.Price)
                {
                    BuyUnit();
                }
            }

            if (mouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed)
            {
                _scene.UpdateState(BattleState.Idle);
            }
        }

        public BattleState CheckState()
        {
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            var containerRect = new Rectangle(
                new Point(100,100),
                new Point(1080,520)
            );
            var menuRect = new Rectangle(
                new Point(450,120),
                new Point(300,500)
            );
            var previewImgRect = new Rectangle(
                new Point(800,250),
                new Point(300,300)
            );
            spriteBatch.Draw(Game1.SpriteDict["FactoryMenuContainer"],containerRect,Color.White);
            spriteBatch.Draw(Game1.SpriteDict["FactoryMenuContainer"],menuRect,Color.White);
            for (var i = 0; i < _optionKeys.Length; i++)
            {
                var itemRect = new Rectangle(
                    new Point(menuRect.Location.X,menuRect.Location.Y+50*i),
                    new Point(300,50)
                );
                spriteBatch.Draw(Game1.SpriteDict["FactoryMenuContainer"],itemRect,_selected.UnitType==_optionKeys[i]?_player.Money>_selected.Price?Color.Yellow:Color.Gray:Color.White);
                spriteBatch.DrawString(Game1.Fonts["placeholderFont"], _optionKeys[i],itemRect.Location.ToVector2(),Color.Black);
            }
            spriteBatch.Draw(Game1.SpriteDict["preview"+_selected.UnitType+_player.Id],previewImgRect, Color.White);
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"],"Cost: "+_selected.Price,new Vector2(250,150),Color.Black);
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"], "Type: " + _selected.MovementType, new Vector2(250, 175), Color.Black);
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"], "Movement: " + _selected.Movement, new Vector2(250, 200), Color.Black);
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"], "Attack: " + _selected.AttackType, new Vector2(250, 225), Color.Black);
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"], "Your money: " + _player.Money, new Vector2(250, 300), Color.Black);
            if (_player.Money < _selected.Price)
            {
                spriteBatch.DrawString(Game1.Fonts["placeholderFont"], "Insufficient funds", new Vector2(250, 325), Color.Black);
            }
        }

        public void BuyUnit()
        {
            _updateState = BattleState.Idle;
            _scene.BuyUnit(Unit.CreateUnit(_selected.UnitType,_player.Id,_building.PosX,_building.PosY,true),_player);
            _scene.UpdateState(_updateState);
        }
    }
}
