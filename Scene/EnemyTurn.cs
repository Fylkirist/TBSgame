using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBSgame.Assets;

namespace TBSgame.Scene
{
    public enum TurnPhase
    {
        Units,
        Buildings,
        End
    }
    internal class EnemyTurn : ISubState
    {
        private BattleScene _scene;
        private BattleState _updateState;
        private Unit _currentUnit;
        private Player _player;
        private TurnPhase _turnPhase;

        internal EnemyTurn(BattleScene scene,Player player)
        {
            _scene = scene;
            _updateState = BattleState.EnemyTurn;
            _turnPhase = TurnPhase.Units;
            _player = player;
            _currentUnit = _scene.GetNextUnit(_player);
        }
        public void Update(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            switch (_turnPhase)
            {
                case TurnPhase.Units:
                    HandleUnits();
                    break;
                case TurnPhase.Buildings:
                    HandleBuildings();
                    break;
                case TurnPhase.End:
                    _updateState = BattleState.TurnStart;
                    _scene.UpdateState(_updateState);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleUnits()
        {
            if (_currentUnit == null)
            {
                _turnPhase = TurnPhase.Buildings;
                return;
            }

            _scene.Camera = new Vector2Int(_currentUnit.PosX, _currentUnit.PosY);

            if (_currentUnit.State == UnitStates.Idle)
            {
                var move = _scene.GetOptimalMove(_currentUnit);
                var target = _scene.GetOptimalTarget(_currentUnit, move);

                if (target != null)
                {
                    _currentUnit.MoveAndFight(move, target, _scene.Map);
                }
                else
                {
                    _currentUnit.MoveUnit(move);
                }
            }

            if (_currentUnit.State is UnitStates.Tapped or UnitStates.Dead)
            {
                _currentUnit = _scene.GetNextUnit(_currentUnit, _player);
            }

            if (_currentUnit == null)
            {
                _turnPhase = TurnPhase.Buildings;
            }
        }


        private void HandleBuildings()
        {
            Dictionary<int,string> unitDict = new Dictionary<int,string> {{1000, "Musketeer" } };
            foreach (var building in _scene.GetAlignedBuildings(_player))
            {
                var price = 0;
                foreach (var unit in unitDict)
                {
                    if (unit.Key > price && unit.Key <= _player.Money )
                    {
                        price = unit.Key;
                    }
                }

                if (price > 0 && building.Type=="factory")
                {
                    var unit = Unit.CreateUnit(unitDict[price], _player.Id, building.PosX, building.PosY, true);
                    _scene.BuyUnit(unit,_player);
                }
            }
            _turnPhase = TurnPhase.End;
        }

        public BattleState CheckState()
        {
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            
        }
    }
}
