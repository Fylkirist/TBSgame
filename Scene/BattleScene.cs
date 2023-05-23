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
    public enum BattleState
    {
        TurnStart,
        Idle,
        Selected,
        Fight,
        Moving,
        MoveMenu,
        FactoryMenu,
        EnemyTurn,
    }
    public class BattleScene : IScene
    {
        private LinkedList<Unit> _unitList;
        private Map _map;
        private Player _player;
        private Computer _enemy;
        private int _tilesX;
        private int _tilesY;
        private int _currentPlayerTurn;
        private string[] _turnOrder;
        public Vector2Int Camera;
        
        private GameState _updateState;
        private BattleState _sceneState;
        private ISubState _currentState;
        public Vector2 TileSize;

        public BattleScene(Map map, LinkedList<Unit> units, string name)
        {
            _unitList = units;
            _map = map;
            _player = new Player(name, "Player1", map.Money);
            _tilesX = 32; _tilesY = 18;
            _currentPlayerTurn = 0;
            _turnOrder = new[] { "red", "blue" };
            _updateState = GameState.BattleScene;
            _enemy = new Computer();
            _sceneState = BattleState.Idle;
            Camera = new Vector2Int(0, 0);
            _currentState = new IdleState(Camera,this);
            TileSize = new Vector2(Game1._viewport.Width / _tilesX, Game1._viewport.Height / _tilesY);
        }

        public Vector2Int GetMapSize()
        {
            return new Vector2Int(_map.MapGrid.GetLength(0), _map.MapGrid.GetLength(1));
        }
        public void Initialize()
        {
            //Spill av start animasjoner
            //Gi startpenger til begge spillere
            //_active blir false etter animasjoner er ferdig
        }
        public void Render(SpriteBatch spriteBatch, Viewport viewport)
        {
            _map.Render(spriteBatch,viewport, Camera.X,Camera.Y,_tilesX,_tilesY);
            foreach (var unit in _unitList)
            {
                unit.Render(spriteBatch, viewport, Camera.X, Camera.Y,_tilesX,_tilesY);
            }
            _currentState.Render(spriteBatch);
        }

        public void HandleInput(MouseState mouse, MouseState previousMouse, KeyboardState keyboard, KeyboardState previousKeyboard, GameTime gameTime)
        {
            _currentState.Update(mouse,previousMouse,gameTime);
            if (_sceneState is BattleState.Selected or BattleState.Idle)
            {
                if (keyboard.IsKeyDown(Keys.A) && previousKeyboard.IsKeyDown(Keys.A))
                {
                    Camera.X -= 1;
                }
                if (keyboard.IsKeyDown(Keys.S) && previousKeyboard.IsKeyDown(Keys.S))
                {
                    Camera.Y += 1;
                }
                if (keyboard.IsKeyDown(Keys.D) && previousKeyboard.IsKeyDown(Keys.D))
                {
                    Camera.X += 1;
                }
                if (keyboard.IsKeyDown(Keys.W) && previousKeyboard.IsKeyDown(Keys.W))
                {
                    Camera.Y -= 1;
                }
            }
        }

        public void SelectMove(Path path, Unit selectedUnit, Vector2Int position)
        {
            Vector2Int[] targets =
            {

            };
            _currentState = new UnitMoveMenu(this, path, selectedUnit, targets, position.X, position.Y);
        }

        public void SelectUnit(Vector2Int pos)
        {
            var selectedUnit = _unitList.FirstOrDefault(unit => unit.PosX == pos.X && unit.PosY == pos.Y && unit.Allegiance == _turnOrder[_currentPlayerTurn]);
            if (selectedUnit != null)
            {
                _sceneState = BattleState.Selected;
                _currentState = new UnitSelectedState(selectedUnit, CalculateAvailableMoves(selectedUnit), this);
                return;
            }

            var selectedBuilding = _map.CheckBuildingSelection(pos.Y, pos.X);
            if (selectedBuilding == null) return;
            _sceneState = BattleState.FactoryMenu;
            _currentState = new FactoryMenu(this, selectedBuilding);
        }

        private List<Path> CalculateAvailableMoves(Unit unit)
        {
            List<Path> availableMoves = new();
            List<Vector2Int> candidateMoves = new();
            for (var x = unit.PosX - unit.Movement; x <= unit.PosX + unit.Movement; x++)
            {
                for (var y = unit.PosY - unit.Movement; y <= unit.PosY + unit.Movement; y++)
                {
                    var dx = Math.Abs(x - unit.PosX);
                    var dy = Math.Abs(y - unit.PosY);
                    if (dx + dy <= unit.Movement && x>=0 && y>=0 && x<_map.MapGrid.GetLength(0) && y<_map.MapGrid.GetLength(1))
                    {
                        candidateMoves.Add(new Vector2Int(x, y));
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

        private Path CalculateShortestPath(Unit unit,Vector2Int target , List<Vector2Int> candidateMoves)
        {
            var directions = new Vector2Int[]
            {
                new(1,0),
                new(0,1),
                new(-1,0),
                new(0,-1)
            };
            var startPath = new Path(target,0,new List<Vector2Int> { target }, directions[1]);
            var counter = 0;
            var pathList = new List<Path>{startPath};
            while (counter <= unit.Movement)
            {
                var currentPathList = new List<Path>(pathList);
                foreach (var path in currentPathList)
                {
                    if (path.Position.X == unit.PosX && path.Position.Y == unit.PosY)
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
                            newPath.Move(_map.MapGrid[destination.X, destination.Y].MovePenaltyDictionary[unit.MovementType]);
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
                        path.Move(_map.MapGrid[destination.X, destination.Y].MovePenaltyDictionary[unit.MovementType]);
                    }
                }

                counter++;
            }
            return new Path();
        }

        private bool CheckEnemyCollision(Vector2Int position, Unit unit)
        {
            foreach (var checkedUnit in _unitList)
            {
                if (checkedUnit.PosX == position.X && position.Y == checkedUnit.PosY &&
                    checkedUnit.Allegiance != unit.Allegiance)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckTileCollision(Vector2Int target)
        {
            foreach (var unit in _unitList)
            {
                if (unit.PosX == target.X && unit.PosY == target.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateState(BattleState newState)
        {
            switch (newState)
            {
                case BattleState.Idle:
                    _currentState = new IdleState(new Vector2Int(0, 0), this);
                    _sceneState = newState;
                    break;
                case BattleState.MoveMenu:
                    _sceneState = newState;
                    break;
            }
        }

        public void OpenFightMenu(Path path,Unit unit, Vector2Int[] _targets)
        {

        }
        private void HandleTurn()
        {
            
        }

        private void StartTurn()
        {
            var moneyCount = _map.CheckAllegiance(_turnOrder[_currentPlayerTurn]) * 100;
        }

        private void EndTurn()
        {
            _map.UpdateSiege(_unitList, _turnOrder[_currentPlayerTurn]);
            _currentPlayerTurn = _currentPlayerTurn<_turnOrder.Length-1?_currentPlayerTurn++:0;
            _sceneState = BattleState.EnemyTurn;

        }
        public GameState CheckStateUpdate()
        {
            return _updateState;
        }
    }

    public class Path
    {
        public Vector2Int Position;
        public int Length;
        public List<Vector2Int> Positions;
        public Vector2Int Direction;
        public bool Viable;

        public Path(Vector2Int pos, int length, List<Vector2Int> posList, Vector2Int direction)
        {
            Position = pos;
            Length=length;
            Positions = new List<Vector2Int>(posList);
            Direction = direction;
            Viable = true;
        }

        public Path()
        {
            Position = new Vector2Int(0,0);
            Positions = new List<Vector2Int>();
            Direction = new Vector2Int(0,0);
            Viable = false;
        }

        public void Move(int cost)
        {
            Length+=cost;
            Position += Direction;
            Positions.Add(Position);
        }

        public void DrawPath(SpriteBatch spriteBatch,float tileWidth, float tileHeight, int tilesX, int tilesY,int cameraX, int cameraY)
        {
            foreach (var position in Positions)
            {
                var destinationPoint = new Point(
                    (int)((position.X - (cameraX - tilesX / 2)) * tileWidth),
                    (int)((position.Y - (cameraY - tilesY / 2)) * tileHeight)
                );
                var size = new Point((int)tileWidth, (int)tileHeight);
                var destination = new Rectangle(destinationPoint, size);
                spriteBatch.Draw(Game1.SpriteDict["PathIndicator"],destination,Color.White);
            }
        }
    }

    public struct Vector2Int
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X - b.X, a.Y - b.Y);
        }

        public static bool operator ==(Vector2Int a, Vector2Int b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2Int a, Vector2Int b)
        {
            return !(a == b);
        }
        public override bool Equals(object? obj)
        {
            if (obj is Vector2Int other)
            {
                return this == other;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

}