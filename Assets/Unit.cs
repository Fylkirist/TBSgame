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
        public Dictionary<string,Texture2D> Sprites;
        public string Type;
        public int Health;
        public int PosX;
        public int PosY;
        public string Allegiance;
        public int Movement;
        public int Damage;
        public string AttackType;

        Unit(Dictionary<string,Texture2D> sprites, string type, int posX, int posY, string playerId, int movement, int damage, string attackType)
        {
            Sprites = sprites;
            Type = type;
            Health = 100;
            PosX = posX;
            PosY = posY;
            Allegiance = playerId;
            Movement = movement;
            Damage = damage;
            AttackType = attackType;
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            var drawPosX = PosX - cameraX;
            drawPosX = drawPosX * viewport.Width / tilesX;
            var drawPosY = PosY - cameraY;
            drawPosY = drawPosY * viewport.Height / tilesY;
            var scale = Math.Min(viewport.Width / tilesX / Sprites["idle"].Width, viewport.Height / tilesY / Sprites["idle"].Height);
            spriteBatch.Draw(Sprites["idle"], new Vector2(drawPosX, drawPosY), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public static Unit CreateUnit(string type, string allegiance,int posX, int posY)
        {
            string moveType = "default";
            string attackType = "default";
            int movement = 0;
            int damage = 0;
            switch (type)
            {
                case "Musketeer":
                    moveType = "infantry";
                    attackType = "smallArms";
                    movement = 3;
                    damage = 40;
                    break;
            }

            Dictionary<string, Texture2D> sprites = new()
            {
                { "idle", Game1.SpriteDict["idle" + type + allegiance] },
                { "north1", Game1.SpriteDict["north1" + type + allegiance] },
                { "north2", Game1.SpriteDict["north2" + type + allegiance] },
                { "east1", Game1.SpriteDict["east1" + type + allegiance] },
                { "east2", Game1.SpriteDict["east2" + type + allegiance] },
                { "south1", Game1.SpriteDict["south1" + type + allegiance] },
                { "south2", Game1.SpriteDict["south2" + type + allegiance] },
                { "west1", Game1.SpriteDict["west1" + type + allegiance] },
                { "west2", Game1.SpriteDict["west2" + type + allegiance] }
            };
            return new Unit(sprites,moveType,posX, posY,allegiance,movement,damage, attackType);
        } 
    }
}
