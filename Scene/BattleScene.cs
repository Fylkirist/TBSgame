using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBSgame.Assets;

namespace TBSgame.Scene
{
    public class BattleScene : IScene
    {
        private LinkedList<Unit> _unitList;
        private Map _map;
        private Player _player;
        private Computer _enemy;
        private int _tilesX;
        private int _tilesY;
        private bool _animFlag;
        private bool _active;
        private int _currentPlayerTurn;
        private string[] _turnOrder;

        BattleScene(Map map, List<Unit> units, string name)
        {
            _unitList = new LinkedList<Unit>();
            foreach (Unit unit in units)
            {
                _unitList.AddLast(unit);
            }
            _map = map;
            _active = true;
            _player = new Player(name, "Player1", map.Money, map.MapGrid.GetLength(0), map.MapGrid.GetLength(1));
            _tilesX = 32; _tilesY = 18;
            _animFlag = false;
            _currentPlayerTurn = 0;
            _turnOrder = new string[] { "Player1", "Player2" };
        }

        public void Initialize()
        {
            //Spill av start animasjoner
            //Gi startpenger til begge spillere
            //_active blir false etter animasjoner er ferdig
        }
        public void Render(SpriteBatch spriteBatch, Viewport viewport)
        {
            _map.Render(spriteBatch,viewport, _player.CameraX,_player.CameraY,_tilesX,_tilesY);
        }

        public void HandleInput(InputKeyEventArgs input)
        {
            if (_active)
            {

            }
        }

        private void HandleTurn()
        {

        }

        private void StartTurn()
        {

        }

        private void EndTurn()
        {

        }
    }
}
