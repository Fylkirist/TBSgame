using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using TBSgame.Scene;

namespace TBSgame.Assets
{
    public class Fight : ITimedAnimation
    {
        private Unit _attacker;
        private Unit _defender;
        private Tile _attackerTile;
        private Tile _defenderTile;
        private double _animationTimer;
        private UnitStates _updateState;
        private double _duration;

        public Fight(Unit attacker, Unit defender, Tile attackerTile, Tile defenderTile)
        {
            _attackerTile = attackerTile;
            _defenderTile = defenderTile;
            _attacker = attacker;
            _defender = defender;
            _animationTimer = 0;
            _updateState = UnitStates.Moving;
            _duration = 5;
        }

        public static int CalculateDamage(Unit attacker, Unit defender, Tile attackTile, Tile defendTile)
        {
            var damage = attacker.Damage * attacker.Health / 100;
            double defenceMod = (double)(10 - defendTile.Protection) / 10;
            double damageMod = 1;

            return (int)(damage * defenceMod * damageMod);
        }

        public void CalculateFight()
        {
            _defender.Health -= CalculateDamage(_attacker, _defender, _attackerTile, _defenderTile);
            if (_defender.Health <= 0)
            {
                _defender.Health = 0;
            }

            _attacker.Health -= CalculateDamage(_defender, _attacker, _defenderTile, _attackerTile);
            if (_attacker.Health <= 0)
            {
                _attacker.Health = 0;
            }
        }

        public UnitStates Update(GameTime gameTime, MouseState mouse, MouseState previousMouse)
        {
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer > _duration)
            {
                _updateState = UnitStates.Tapped;
            }
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            var backgroundTopLeft = new Point(viewport.Width / 4, viewport.Height / 4);
            var backgroundSize = new Point(backgroundTopLeft.X*2, backgroundTopLeft.Y*2);
            var backgroundRect = new Rectangle(backgroundTopLeft, backgroundSize);
            spriteBatch.Draw(Game1.SpriteDict["placeholderButton"],backgroundRect,Color.White);
        }
    }
}
