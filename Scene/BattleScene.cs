#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private Unit? _selectedUnit;

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

        private List<KeyValuePair<int, int>> CalculateAvailableMoves(Unit unit)
        {
            List<KeyValuePair<int, int>> availableMoves = new();
            List<KeyValuePair<int, int>> candidateMoves = new();
            for (var x = unit.PosX - unit.Movement; x <= unit.PosX + unit.Movement; x++)
            {
                for (var y = unit.PosY - unit.Movement; y <= unit.PosX + unit.Movement; y++)
                {
                    var dx = Math.Abs(x - unit.PosX);
                    var dy = Math.Abs(y - unit.PosY);
                    if (dx + dy <= unit.Movement)
                    {
                        candidateMoves.Add(new KeyValuePair<int, int>(x, y));
                    }
                }
            }

            foreach (var target in candidateMoves)
            {
                var path = EvaluateShortestPath(target, unit,candidateMoves);
                if (path.Length <= unit.Movement && path.Viable && !CheckTileCollision(target))
                {
                    availableMoves.Add(target);
                }
            }
            return availableMoves;
        }

        private Path EvaluateShortestPath(KeyValuePair<int, int> target, Unit unit,List<KeyValuePair<int,int>> candidateMoves)
        {
            var possiblePaths = new List<Path>()
            {
                new(target.Key, target.Value, 0,new List<Vector2>{new(target.Key,target.Value)},new Vector2(0,-1))
            };
            var counter = 0;
            while (true)
            {
                var possiblePathsCopy = possiblePaths.ToList();
                foreach (var path in possiblePathsCopy)
                {
                    if (path.X == unit.PosX && path.Y == unit.PosY)
                    {
                        return path;
                    }
                    var pathMove = false;
                    if (candidateMoves.Contains(new KeyValuePair<int, int>(path.X,path.Y)))
                    {
                        if (path.X<_map.MapGrid.GetLength(0)-1 && _map.MapGrid[path.X+1, path.Y].MovePenaltyDictionary[unit.Type]<= unit.Movement - path.Length && !CheckEnemyCollision(path.X + 1, path.Y, unit))
                        {
                            if (path.Direction == new Vector2(1, 0))
                            {
                                pathMove = true;
                            }
                            else if (!path.Positions.Contains(new Vector2(path.X+1, path.Y)))
                            {
                                var newPath = new Path(path.X, path.Y, path.Length, path.Positions, new Vector2(1, 0));
                                newPath.Move();
                                possiblePaths.Add(newPath);
                            }

                        }
                        if (path.X > 0 && _map.MapGrid[path.X - 1, path.Y].MovePenaltyDictionary[unit.Type] <= unit.Movement - path.Length && !CheckEnemyCollision(path.X-1,path.Y,unit))
                        {
                            if (path.Direction == new Vector2(-1, 0))
                            {
                                pathMove = true;
                            }
                            else if (!path.Positions.Contains(new Vector2(path.X-1, path.Y)))
                            {
                                var newPath = new Path(path.X - 1, path.Y, path.Length + 1, path.Positions, new Vector2(-1, 0));
                                newPath.Move();
                                possiblePaths.Add(newPath);
                            }
                        }
                        if (path.Y < _map.MapGrid.GetLength(0) - 1 && _map.MapGrid[path.X, path.Y+1].MovePenaltyDictionary[unit.Type] <= unit.Movement - path.Length && !CheckEnemyCollision(path.X, path.Y+1, unit))
                        {
                            if (path.Direction == new Vector2(0, +1))
                            {
                                pathMove = true;
                            }
                            else if (!path.Positions.Contains(new Vector2(path.X, path.Y + 1)))
                            {
                                var newPath = new Path(path.X + 1, path.Y, path.Length + 1, path.Positions, new Vector2(0, +1));
                                newPath.Move();
                                possiblePaths.Add(newPath);
                            }
                        }
                        if (path.Y > 0 && _map.MapGrid[path.X, path.Y-1].MovePenaltyDictionary[unit.Type] <= unit.Movement - path.Length && !CheckEnemyCollision(path.X, path.Y-1, unit))
                        {
                            if (path.Direction == new Vector2(0, -1))
                            {
                                pathMove = true;
                            }
                            else if(!path.Positions.Contains(new Vector2(path.X,path.Y-1)))
                            {
                                var newPath = new Path(path.X, path.Y - 1, path.Length + 1, path.Positions, new Vector2(0, -1));
                                newPath.Move();
                                possiblePaths.Add(newPath);
                            }
                        }
                    }
                    if (pathMove)
                    {
                        path.Move();
                    }
                    else
                    {
                        possiblePaths.Remove(path);
                    }
                }

                if (counter > unit.Movement)
                { 
                    return new Path();
                }
                counter++;
            }
        }

        private bool CheckEnemyCollision(int posX, int posY, Unit unit)
        {
            foreach (var checkedUnit in _unitList)
            {
                if (checkedUnit.PosX == posX && posY == checkedUnit.PosY &&
                    checkedUnit.Allegiance != unit.Allegiance)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckTileCollision(KeyValuePair<int, int> target)
        {
            foreach (var unit in _unitList)
            {
                if (unit.PosX == target.Key && unit.PosY == target.Value)
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

        }

        private void EndTurn()
        {
            _active = true;
            _map.UpdateSiege(_unitList, _turnOrder[_currentPlayerTurn]);
            _currentPlayerTurn = _currentPlayerTurn<_turnOrder.Length-1?_currentPlayerTurn++:0;

        }
    }

    internal class Path
    {
        public int X, Y;
        public int Length;
        public List<Vector2> Positions;
        public Vector2 Direction;
        public bool Viable;

        public Path(int x, int y, int length, List<Vector2> posList,Vector2 direction)
        {
            X=x; Y=y; Length=length;
            Positions = posList;
            Direction = direction;
            Viable = true;
        }

        public Path()
        {
            X = 0; Y = 0;
            Positions = new List<Vector2>();
            Direction = Vector2.Zero;
            Viable = false;
        }

        public void Move()
        {
            Length++;
            Positions.Add(new Vector2(X,Y));
            X += (int)Direction.X;
            Y += (int)Direction.Y;
        }
    }
}
