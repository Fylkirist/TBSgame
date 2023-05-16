using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBSgame.Assets
{
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
        private bool _animFlag;

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
            _animFlag = false;
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            var drawPosX = PosX - cameraX;
            drawPosX = drawPosX * viewport.Width / tilesX;
            var drawPosY = PosY - cameraY;
            drawPosY = drawPosY * viewport.Height / tilesY;
            var scale = Math.Min(viewport.Width / tilesX / Game1.SpriteDict["idle"+UnitType+Allegiance].Width, viewport.Height / tilesY / Game1.SpriteDict["idle" + UnitType + Allegiance].Height);
            spriteBatch.Draw(Game1.SpriteDict["idle" + UnitType + Allegiance], new Vector2(drawPosX, drawPosY), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
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
                    movement = 3;
                    damage = 40;
                    break;
            }

            return new Unit(unitType,moveType,posX, posY,allegiance,movement,damage, attackType);
        } 
    }
}
