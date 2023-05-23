using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public UnitStates State { get; private set; }

        Unit(string unitType, string movementType, int posX, int posY, string playerId, int movement, int damage, string attackType)
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
        }

        public void Update(GameTime gameTime)
        {
            switch (State)
            {
                case UnitStates.Idle:
                    break;
                case UnitStates.Moving:
                    break;
                case UnitStates.Tapped:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public BattleState CheckStateUpdate()
        {
            switch (State)
            {
                case UnitStates.Tapped:
                    return BattleState.Idle;
                case UnitStates.Moving:
                    return BattleState.Moving;
                case UnitStates.Idle:
                    return BattleState.Idle;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            if (State == UnitStates.Idle)
            {
                int positionX = (PosX - (cameraX - tilesX / 2)) * (viewport.Width/tilesX);
                int positionY = (PosY - (cameraY - tilesY / 2)) * (viewport.Height/tilesY);
                var drawPoint = new Point(positionX,positionY);
                var drawSize = new Point(viewport.Width / tilesX, viewport.Height / tilesY);
                var destination = new Rectangle(drawPoint,drawSize);
                spriteBatch.Draw(Game1.SpriteDict["idle" + UnitType + Allegiance], destination,Color.White);
            }
            else if (State == UnitStates.Tapped)
            {
                int positionX = (PosX - (cameraX - tilesX / 2)) * (viewport.Width / tilesX);
                int positionY = (PosY - (cameraY - tilesY / 2)) * (viewport.Height / tilesY);
                var drawPoint = new Point(positionX, positionY);
                var drawSize = new Point(viewport.Width / tilesX, viewport.Height / tilesY);
                var destination = new Rectangle(drawPoint, drawSize);
                spriteBatch.Draw(Game1.SpriteDict["tapped" + UnitType + Allegiance], destination, Color.White);
            }
            else if (State == UnitStates.Moving)
            {
                int positionX = (PosX - (cameraX - tilesX / 2)) * (viewport.Width / tilesX);
                int positionY = (PosY - (cameraY - tilesY / 2)) * (viewport.Height / tilesY);
                var drawPoint = new Point(positionX, positionY);
                var drawSize = new Point(viewport.Width / tilesX, viewport.Height / tilesY);
                var destination = new Rectangle(drawPoint, drawSize);
                spriteBatch.Draw(Game1.SpriteDict["idle" + UnitType + Allegiance], destination, Color.White);
            }
        }

        public void MoveUnit(Path path)
        {
            State = UnitStates.Moving;
            PosX = (int)path.Position.X;
            PosY = (int)path.Position.Y;
            State = UnitStates.Tapped;
        }

        public static Unit CreateUnit(string type, string allegiance,int posX, int posY)
        {
            string moveType = "default";
            string attackType = "default";
            int movement = 0;
            int damage = 0;
            string unitType = type;
            switch (type)
            {
                case "Musketeer":
                    moveType = "infantry";
                    attackType = "smallArms";
                    movement = 6;
                    damage = 40;
                    break;
            }

            return new Unit(unitType,moveType,posX, posY,allegiance,movement,damage, attackType);
        } 
    }
}
