#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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
        private bool _active;
        private int _currentPlayerTurn;
        private string[] _turnOrder;
        private Unit? _selectedUnit;
        private List<Path>? _selectedMoveList;
        private GameState _updateState;
        private int _pointerX;
        private int _pointerY;
        private Building? _selectedBuilding;

        public BattleScene(Map map, LinkedList<Unit> units, string name)
        {
            _unitList = units;
            _map = map;
            _active = false;
            _player = new Player(name, "Player1", map.Money, map.MapGrid.GetLength(0)/2, map.MapGrid.GetLength(1)/2);
            _tilesX = 32; _tilesY = 18;
            _currentPlayerTurn = 0;
            _turnOrder = new[] { "red", "blue" };
            _updateState = GameState.BattleScene;
            _enemy = new Computer();
            _pointerX = _player.CameraX;
            _pointerY = _player.CameraY;
        }

        public void Initialize()
        {
            //Spill av start animasjoner
            //Gi startpenger til begge spillere
            //_active blir false etter animasjoner er ferdig
        }
        public void Render(SpriteBatch spriteBatch, Viewport viewport)
        {
            int tileWidth = viewport.Width / _tilesX;
            int tileHeight = viewport.Height / _tilesY;
            _map.Render(spriteBatch,viewport, _player.CameraX,_player.CameraY,_tilesX,_tilesY);
            foreach (var unit in _unitList)
            {
                unit.Render(spriteBatch, viewport, _player.CameraX,_player.CameraY,_tilesX,_tilesY);
            }

            if (_selectedUnit != null)
            {
                foreach (var path in _selectedMoveList)
                {
                    var coordinate = path.Positions[0];
                    var destinationPoint = new Point(((int)coordinate.X - (_player.CameraX-_tilesX/2)) * tileWidth, ((int)coordinate.Y - (_player.CameraY-_tilesY/2)) * tileHeight);
                    var size = new Point(tileWidth,tileHeight);
                    var destination = new Rectangle(destinationPoint,size);
                    spriteBatch.Draw(Game1.SpriteDict["AvailableTileBorder"],destination,Color.White);
                    if (_pointerX == (int)path.Positions[0].X && _pointerY == (int)path.Positions[0].Y)
                    {
                        path.DrawPath(spriteBatch, tileWidth, tileHeight, _tilesX, _tilesY);
                    }
                }
                spriteBatch.DrawString(Game1.Fonts["placeholderFont"], new StringBuilder("Selected!"), new Vector2(50, 0), Color.White);
            }

            if (!_active)
            {

            }
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"],new StringBuilder(_pointerX.ToString()+_pointerY.ToString()),new Vector2(0,0),Color.White);
            var markerRect =
                new Rectangle(
                    new Point((_pointerX - (_player.CameraX - _tilesX / 2)) * tileWidth, (_pointerY - (_player.CameraY - _tilesY / 2)) * tileHeight),
                    new Point(tileWidth, tileHeight));
            spriteBatch.Draw(Game1.SpriteDict["SelectionMarker"],markerRect,Color.White);
        }

        public void HandleInput(MouseState mouse, MouseState previousMouse, KeyboardState keyboard, KeyboardState previousKeyboard, GameTime gameTime)
        {

            _player.HandleInput(keyboard,previousKeyboard,gameTime);
            var tileSizeX = Game1._viewport.Width / _tilesX;
            var tileSizeY = Game1._viewport.Height / _tilesY;
            _pointerX = mouse.X / tileSizeX + (_player.CameraX - _tilesX / 2);
            _pointerY = mouse.Y / tileSizeY + (_player.CameraY - _tilesY / 2);

            CheckClick(mouse,previousMouse);
            

        }

        private void CheckClick(MouseState mouse, MouseState previousMouse)
        {
            if (mouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed )
            {

                foreach (var unit in _unitList.Where(unit => unit.PosX == _pointerX && unit.PosY == _pointerY && unit.Allegiance == _turnOrder[_currentPlayerTurn]))
                {
                    SelectUnit(unit);
                    return;
                }

                var selected = _map.CheckBuildingSelection(_pointerY, _pointerX);
                if (selected != null) _selectedBuilding = selected;
            }
            else if (mouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed)
            {
                _selectedBuilding = null;
                _selectedUnit = null;
            }
        }

        private void SelectUnit(Unit unit)
        {
            if (unit.State != UnitStates.Idle) return;
            _selectedUnit = unit;
            _selectedMoveList = CalculateAvailableMoves(unit);
        }

        private List<Path> CalculateAvailableMoves(Unit unit)
        {
            List<Path> availableMoves = new();
            List<Vector2> candidateMoves = new();
            for (var x = unit.PosX - unit.Movement; x <= unit.PosX + unit.Movement; x++)
            {
                for (var y = unit.PosY - unit.Movement; y <= unit.PosY + unit.Movement; y++)
                {
                    var dx = Math.Abs(x - unit.PosX);
                    var dy = Math.Abs(y - unit.PosY);
                    if (dx + dy <= unit.Movement && x>=0 && y>=0 && x<_map.MapGrid.GetLength(0) && y<_map.MapGrid.GetLength(1))
                    {
                        candidateMoves.Add(new Vector2(x, y));
                    }
                }
            }

            foreach (var position in candidateMoves)
            {
                var path = CalculateShortestPath(unit, position, candidateMoves);
                if (path.Viable && !CheckTileCollision(position))
                {
                    availableMoves.Add(path);
                }
            }
            return availableMoves;
        }

        private Path CalculateShortestPath(Unit unit,Vector2 target , List<Vector2> candidateMoves)
        {
            var directions = new Vector2[]
            {
                new(1,0),
                new(0,1),
                new(-1,0),
                new(0,-1)
            };
            var startPath = new Path(target,0,new List<Vector2> { target }, directions[1]);
            var counter = 0;
            var pathList = new List<Path>{startPath};
            while (counter <= unit.Movement)
            {
                var currentPathList = new List<Path>(pathList);
                foreach (var path in currentPathList)
                {
                    if ((int)path.Position.X == unit.PosX && (int)path.Position.Y == unit.PosY)
                    {
                        return path;
                    }
                    var movable = false;
                    foreach (var direction in directions)
                    {
                        var destination = path.Position + direction;
                        if (!candidateMoves.Contains(destination) || path.Length > unit.Movement || CheckEnemyCollision(destination,unit)) continue;
                        if (path.Direction == direction)
                        {
                            movable = true;
                        }
                        else
                        {
                            if (path.Positions.Contains(destination)) continue;
                            var newPath = new Path(path.Position, path.Length, path.Positions, direction);
                            newPath.Move(_map.MapGrid[(int)destination.X, (int)destination.Y].MovePenaltyDictionary[unit.MovementType]);
                            pathList.Add(newPath);
                        }
                    }

                    if (!movable)
                    {
                        pathList.Remove(path);
                    }
                    else
                    {
                        var destination = path.Position + path.Direction;
                        path.Move(_map.MapGrid[(int)destination.X, (int)destination.Y].MovePenaltyDictionary[unit.MovementType]);
                    }
                }

                counter++;
            }
            return new Path();
        }

        private bool CheckEnemyCollision(Vector2 position, Unit unit)
        {
            foreach (var checkedUnit in _unitList)
            {
                if (checkedUnit.PosX == (int)position.X && (int)position.Y == checkedUnit.PosY &&
                    checkedUnit.Allegiance != unit.Allegiance)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckTileCollision(Vector2 target)
        {
            foreach (var unit in _unitList)
            {
                if (unit.PosX == (int)target.X && unit.PosY == (int)target.Y)
                {
                    return true;
                }
            }
            return false;
        }

        private void HandleTurn()
        {
            
        }

        private void StartTurn()
        {
            var moneyCount = _map.CheckAllegiance(_turnOrder[_currentPlayerTurn]) * 100;
            _active = false;
        }

        private void EndTurn()
        {
            _active = true;
            _map.UpdateSiege(_unitList, _turnOrder[_currentPlayerTurn]);
            _currentPlayerTurn = _currentPlayerTurn<_turnOrder.Length-1?_currentPlayerTurn++:0;

        }
        public GameState CheckStateUpdate()
        {
            return _updateState;
        }
    }

    public class Path
    {
        public Vector2 Position;
        public int Length;
        public List<Vector2> Positions;
        public Vector2 Direction;
        public bool Viable;

        public Path(Vector2 pos, int length, List<Vector2> posList,Vector2 direction)
        {
            Position = pos;
            Length=length;
            Positions = new List<Vector2>(posList);
            Direction = direction;
            Viable = true;
        }

        public Path()
        {
            Position = new Vector2(0,0);
            Positions = new List<Vector2>();
            Direction = Vector2.Zero;
            Viable = false;
        }

        public void Move(int cost)
        {
            Length+=cost;
            Position += Direction;
            Positions.Add(Position);
        }

        public void DrawPath(SpriteBatch spriteBatch,int tileWidth, int tileHeight, int tilesX, int tilesY)
        {

        }
    }
}