using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBSgame.Scene;
using MonoGame.Extended.Animations;
using MonoGame.Extended.BitmapFonts;

namespace TBSgame.Assets
{
    public enum UnitStates
    {
        Tapped,
        Moving,
        Idle,
        Dead
    }
    public class Unit
    {
        public string UnitType;
        public string MovementType;
        public int Health;
        public int PosX;
        public int PosY;
        public string Allegiance;
        public int Movement;
        public int Damage;
        public string AttackType;
        public int AttackRange;
        public int Price;
        private LinkedListNode<ITimedAnimation> _animation;
        public UnitStates State;
        private LinkedList<ITimedAnimation> _animationQueue;

        Unit(string unitType, string movementType, int posX, int posY, string playerId, int movement, int damage, string attackType, int attackRange, int price, bool tapped)
        {
            UnitType = unitType;
            MovementType = movementType;
            Health = 100;
            PosX = posX;
            PosY = posY;
            Allegiance = playerId;
            Movement = movement;
            Damage = damage;
            AttackType = attackType;
            State = UnitStates.Idle;
            AttackRange = attackRange;
            Price = price;
            _animationQueue = new LinkedList<ITimedAnimation>();
            _animationQueue.AddLast(tapped? new UnitTappedAnimation(this):new UnitIdleAnimation(this));
            _animation = _animationQueue.First;
        }

        public void Update(GameTime gameTime, MouseState mouse, MouseState previousMouse)
        {
            if (Health <= 0)
            {
                State = UnitStates.Dead;
                return;
            }
            var stateUpdate = _animation.Value.Update(gameTime, mouse, previousMouse);
            if (stateUpdate != State && stateUpdate == UnitStates.Tapped && _animation.Next != null)
            {
                _animation = _animation.Next;
            }
            else if (stateUpdate != State && stateUpdate == UnitStates.Idle && _animation.Next == null)
            {
                _animationQueue = new LinkedList<ITimedAnimation>();
                _animationQueue.AddLast(new UnitIdleAnimation(this));
                _animation = _animationQueue.Last;
            }
            else if (stateUpdate != State && stateUpdate == UnitStates.Tapped && _animation.Next == null)
            {
                _animationQueue = new LinkedList<ITimedAnimation>();
                _animationQueue.AddLast(new UnitTappedAnimation(this));
                _animation = _animationQueue.Last;
            }

            State = stateUpdate;
        }

        public UnitStates CheckStateUpdate()
        {
            return State;
        }

        public void RefreshUnit()
        {
            _animationQueue = new LinkedList<ITimedAnimation>();
            _animationQueue.AddLast(new UnitIdleAnimation(this));
            _animation = _animationQueue.Last;
            State = UnitStates.Idle;
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            int positionX = (PosX - (cameraX - tilesX / 2)) * (viewport.Width / tilesX);
            int positionY = (PosY - (cameraY - tilesY / 2)) * (viewport.Height / tilesY);
            _animation.Value.Render(spriteBatch,viewport,cameraX,cameraY,tilesX,tilesY);
            spriteBatch.DrawString(Game1.Fonts["placeholderFont"], new StringBuilder((Health / 10).ToString()),
                new Vector2(positionX, positionY), Color.Black);
        }

        public void MoveUnit(Path path)
        {
            _animationQueue.AddLast(new MoveAnimation(path, this));
            _animation = _animationQueue.Last;
            State = UnitStates.Moving;
            PosX = path.Positions[0].X;
            PosY = path.Positions[0].Y;
        }

        public void MoveAndFight(Path path, Unit target, Map map)
        {
            _animationQueue.AddLast(new MoveAnimation(path, this));
            _animation = _animationQueue.Last;
            State = UnitStates.Moving;
            PosX = path.Positions[0].X;
            PosY = path.Positions[0].Y;
            var fight = new Fight(this, target, map.GetTile(PosX, PosY),
                map.GetTile(target.PosX, target.PosY));
            fight.CalculateFight();
            _animationQueue.AddLast(fight);
        }

        public static Unit CreateUnit(string type, string allegiance,int posX, int posY, bool tapped)
        {
            string moveType = "default";
            string attackType = "default";
            int movement = 0;
            int damage = 0;
            string unitType = type;
            int range = 0;
            int price = 0;
            switch (type)
            {
                case "Musketeer":
                    moveType = "infantry";
                    attackType = "smallArms";
                    movement = 4;
                    damage = 100;
                    range = 1;
                    price = 1000;
                    break;
            }

            return new Unit(unitType,moveType,posX, posY,allegiance,movement,damage, attackType, range, price, tapped);
        } 
    }
}
