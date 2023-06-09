﻿#nullable enable
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
        TargetSelect,
        GameMenu
    }
    public class BattleScene : IScene
    {
        private LinkedList<Unit> _unitList;
        public LinkedList<Unit> Units => _unitList;
        private Map _map;
        public Map Map => _map;
        private Player _player;
        private Player _enemy;
        private int _tilesX;
        private int _tilesY;
        private int _currentPlayerTurn;
        private string[] _turnOrder;
        public Vector2Int Camera;
        
        private GameState _updateState;
        private BattleState _sceneState;
        private ISubState _currentState;
        public Vector2 TileSize;
        private LinkedList<Unit> _deadUnits;

        public BattleScene(Map map, LinkedList<Unit> units, string name)
        {
            _unitList = units;
            _map = map;
            _player = new Player(name, "red", map.Money);
            _tilesX = 32; _tilesY = 18;
            _currentPlayerTurn = 0;
            _turnOrder = new[] { "red", "blue" };
            _updateState = GameState.BattleScene;
            _enemy = new Player("Bruh", "blue",map.Money);
            _sceneState = BattleState.Idle;
            Camera = new Vector2Int(_map.MapGrid.GetLength(0)/2, _map.MapGrid.GetLength(1)/2);
            _currentState = new IdleState(Camera,this);
            TileSize = new Vector2(Game1._viewport.Width / _tilesX, Game1._viewport.Height / _tilesY);
            _deadUnits = new LinkedList<Unit>();
        }

        public Vector2Int GetMapSize()
        {
            return new Vector2Int(_map.MapGrid.GetLength(0), _map.MapGrid.GetLength(1));
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport)
        {
            _map.Render(spriteBatch,viewport, Camera.X,Camera.Y,_tilesX,_tilesY);
            Unit? storedUnit = null;
            foreach (var unit in _unitList)
            {
                if (unit.State == UnitStates.Moving)
                    storedUnit = unit;
                else
                    unit.Render(spriteBatch, viewport, Camera.X, Camera.Y,_tilesX,_tilesY);
            }
            _currentState.Render(spriteBatch);
            if (storedUnit != null)
            {
                storedUnit.Render(spriteBatch, viewport, Camera.X, Camera.Y, _tilesX, _tilesY);
            }
        }

        public Unit? GetNextUnit(Unit unit, Player player)
        {
            LinkedListNode<Unit> unitPointer = _unitList.Find(unit);
            unitPointer = unitPointer.Next;
            while (unitPointer != null && unitPointer.Value.Allegiance != player.Id)
            {
                unitPointer = unitPointer.Next;
            }
            return unitPointer?.ValueRef;
        }

        public Unit? GetNextUnit(Player player)
        {
            return _unitList.FirstOrDefault(unit => unit.Allegiance == player.Id);
        }

        public void HandleInput(MouseState mouse, MouseState previousMouse, KeyboardState keyboard, KeyboardState previousKeyboard, GameTime gameTime)
        {
            if(_sceneState != BattleState.Moving)
                _currentState.Update(mouse,previousMouse,gameTime);
            if (_sceneState is (BattleState.Selected or BattleState.Idle))
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

            CheckUnitMoves(mouse, previousMouse, gameTime);
        }

        private void CheckUnitMoves(MouseState mouse, MouseState previousMouse, GameTime gameTime)
        {
            bool moving = false;
            foreach (var unit in _unitList)
            {
                unit.Update(gameTime, mouse, previousMouse);
                if (unit.CheckStateUpdate() == UnitStates.Moving)
                {
                    moving = true;
                }
            }

            _sceneState = moving ? BattleState.Moving : BattleState.Idle;

            if (_sceneState == BattleState.Idle)
            {
                Unit? checkedUnit = null;
                foreach (var unit in _unitList.Where(unit => unit.CheckStateUpdate() == UnitStates.Dead))
                {
                    checkedUnit = unit;
                }

                if (checkedUnit != null)
                {
                    _unitList.Remove(checkedUnit);
                    _deadUnits.AddLast(checkedUnit);
                }
            }
        }

        public void BuyUnit(Unit unit, Player player)
        {
            player.Money -= unit.Price;
            unit.State = UnitStates.Tapped;
            _unitList.AddLast(unit);
        }
        public void PreviousState(ISubState cachedState)
        {
            _currentState = cachedState;
        }

        public void SelectMove(Path path, Unit selectedUnit, Vector2Int position)
        {
            Unit[] targets = GetPossibleTargets(selectedUnit, path);
            _currentState = new UnitMoveMenu(this, path, selectedUnit, targets, position.X, position.Y);
        }

        public Unit[] GetPossibleTargets(Unit unit,Path path)
        {
            var tempList = new List<Unit>();
            foreach (var target in _unitList)
            {
                var distance = Math.Abs(target.PosX - path.Positions[0].X) + Math.Abs(target.PosY - path.Positions[0].Y);
                if (distance <= unit.AttackRange && target.Allegiance != unit.Allegiance)
                {
                    tempList.Add(target);
                }
            }

            return tempList.ToArray();
        }

        public void MoveAndFight(Path path,Unit attacker, Unit target)
        {
            attacker.MoveAndFight(path,target,_map);
            UpdateState(BattleState.Idle);
        }

        public void SelectUnit(Vector2Int pos)
        {
            var selectedUnit = _unitList.FirstOrDefault(unit => unit.PosX == pos.X && unit.PosY == pos.Y);
            if (selectedUnit != null)
            {
                if (selectedUnit.Allegiance != _player.Id)
                {
                    _sceneState = BattleState.GameMenu;
                    _currentState = new TurnMenu(this);
                    return;
                }
                if (selectedUnit.State == UnitStates.Idle)
                {
                    _sceneState = BattleState.Selected;
                    _currentState = new UnitSelectedState(selectedUnit, CalculateAvailableMoves(selectedUnit), this);
                }
                else if (selectedUnit.State == UnitStates.Tapped)
                {
                    _sceneState = BattleState.GameMenu;
                    _currentState = new TurnMenu(this);
                }
                return;
            }

            var selectedBuilding = _map.CheckBuildingSelection(pos.Y, pos.X);
            if (selectedBuilding != null && selectedBuilding.Allegiance == _player.Id && selectedBuilding.Type != "hq")
            {
                _sceneState = BattleState.FactoryMenu;
                _currentState = new FactoryMenu(this, selectedBuilding, _player);
                return;
            }

            _sceneState = BattleState.GameMenu;
            _currentState = new TurnMenu(this);
        }

        public List<Building> GetAlignedBuildings(Player player)
        {
            return _map.Buildings.Where(building => building.Allegiance == player.Id).ToList();
        }

        public Unit? GetOptimalTarget(Unit attacker, Path path)
        {
            var targets = GetPossibleTargets(attacker, path);
            if (targets.Length == 0)
            {
                return null;
            }
            Unit unit = targets[0];
            int damageCount = 0;
            foreach (var target in targets)
            {
                var damage = Fight.CalculateDamage(attacker, target, _map.GetTile(attacker.PosX, attacker.PosY),
                    _map.GetTile(target.PosX, target.PosY));
                if (damage > damageCount)
                {
                    damageCount=damage;
                    unit = target;
                }
            }
            return unit;
        }

        public Path GetOptimalMove(Unit unit)
        {
            var availableMoves = CalculateAvailableMoves(unit);
            var pathWeightDict = new Dictionary<Path,int>();
            foreach (var path in availableMoves)
            {
                pathWeightDict.Add(path,0);
                foreach (var target in _unitList)
                {
                    if (Math.Abs(path.Positions[0].X - target.PosX) + Math.Abs(path.Positions[0].Y - target.PosY) <= unit.AttackRange)
                    {
                        var damage = Fight.CalculateDamage(unit, target,
                            _map.GetTile(path.Positions[0].X, path.Positions[0].Y),
                            _map.GetTile(target.PosX, target.PosY));
                        pathWeightDict[path] = damage > pathWeightDict[path]?damage: pathWeightDict[path];
                    }
                }

                foreach (var building in _map.Buildings)
                {
                    if (Math.Abs(path.Positions[0].X - building.PosX) + Math.Abs(path.Positions[0].Y - building.PosY) == 1 && unit.Allegiance != building.Allegiance)
                    {
                        pathWeightDict[path] += 30;
                    }

                    if (building.Type == "hq" && building.Allegiance != unit.Allegiance)
                    {
                        pathWeightDict[path] -= Math.Abs(path.Positions[0].X - building.PosX) + Math.Abs(path.Positions[0].Y - building.PosY)/2;
                    }
                }
                
            }

            return pathWeightDict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
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
                if (path.Viable && !CheckTileCollision(position,unit))
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
                        var distance = (path.Position.X - unit.PosX)+(path.Position.Y - unit.PosY);
                        if (!candidateMoves.Contains(destination) || path.Length > unit.Movement || CheckEnemyCollision(destination,unit) || distance + path.Length > unit.Movement) continue;
                        if (path.Direction == direction)
                        {
                            movable = true;
                        }
                        else
                        {
                            if (path.Positions.Contains(destination) || destination.X + destination.Y - unit.PosX - unit.PosY + path.Length+1 > unit.Movement) continue;
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

        private bool CheckTileCollision(Vector2Int target, Unit selectedUnit)
        {
            foreach (var unit in _unitList)
            {
                if (unit.PosX == target.X && unit.PosY == target.Y && unit != selectedUnit)
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
                case BattleState.TurnStart:
                    _currentState = new TurnStartState(this,_player);
                    _sceneState = newState;
                    break;
            }
        }

        public void OpenFightMenu(Unit unit, Unit[] targets, ISubState state, Path path)
        {
            _currentState = new FightTargetSelection(unit, targets, this,state,path);
        }

        public void StartTurn(Player player)
        {
            _map.UpdateSiege(_unitList,player.Id);
            var moneyCount = _map.CheckAllegiance(player.Id) * 1000;
            player.Money += moneyCount;
            foreach (var unit in _unitList)
            {
                if (unit.Allegiance == player.Id)
                {
                    unit.RefreshUnit();
                }
            }
        }

        public void EndTurn()
        {
            _currentPlayerTurn = _currentPlayerTurn<_turnOrder.Length-1?_currentPlayerTurn++:0;
            _sceneState = BattleState.EnemyTurn;
            _currentState = new EnemyTurn(this,_enemy);
            StartTurn(_enemy);
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