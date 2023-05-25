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

namespace TBSgame.Assets
{
    public enum UnitStates
    {
        Tapped,
        Moving,
        Idle
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
        private ITimedAnimation _animation;
        public UnitStates State;

        Unit(string unitType, string movementType, int posX, int posY, string playerId, int movement, int damage, string attackType, int attackRange, int price)
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
            _animation = new UnitIdleAnimation(this);
        }

        public void Update(GameTime gameTime, MouseState mouse, MouseState previousMouse)
        {
            var stateUpdate = _animation.Update(gameTime, mouse, previousMouse);
            if (stateUpdate != State && stateUpdate == UnitStates.Idle)
            {
                _animation = new UnitIdleAnimation(this);
            }
            if (stateUpdate != State && stateUpdate == UnitStates.Tapped)
            {
                _animation = new UnitTappedAnimation(this);
            }

            State = stateUpdate;
        }

        public UnitStates CheckStateUpdate()
        {
            return State;
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            _animation.Render(spriteBatch,viewport,cameraX,cameraY,tilesX,tilesY);
        }

        public void MoveUnit(Path path)
        {
            _animation = new MoveAnimation(path, this);
            State = UnitStates.Moving;
            PosX = path.Positions[0].X;
            PosY = path.Positions[0].Y;
        }

        public static Unit CreateUnit(string type, string allegiance,int posX, int posY)
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
                    damage = 40;
                    range = 1;
                    price = 1000;
                    break;
            }

            return new Unit(unitType,moveType,posX, posY,allegiance,movement,damage, attackType, range, price);
        } 
    }
}
