using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<UnitBattleSpriteAnim> _animations;

        public Fight(Unit attacker, Unit defender, Tile attackerTile, Tile defenderTile)
        {
            _attackerTile = attackerTile;
            _defenderTile = defenderTile;
            _attacker = attacker;
            _defender = defender;
            _animationTimer = 0;
            _updateState = UnitStates.Moving;
            _duration = 5;
            _animations = new List<UnitBattleSpriteAnim>();
            InitializeAnimations();
        }

        public void InitializeAnimations()
        {
            var postDamageDefender = new Unit(_defender);
            postDamageDefender.Health -= CalculateDamage(_attacker, postDamageDefender, _attackerTile, _defenderTile);
            var postDamageAttackerHealth = _attacker.Health - CalculateDamage(postDamageDefender, _attacker, _defenderTile, _attackerTile);
            var attackerSprites = (int)Math.Ceiling((double)_attacker.NumSprites / 100 * _attacker.Health);
            var survivingAttackers = (int)Math.Ceiling((decimal)attackerSprites / 100 * postDamageAttackerHealth);
            var defenderSprites = (int)Math.Ceiling((double)_defender.NumSprites / 100 * _defender.Health);
            var survivingDefenders = (int)Math.Ceiling((decimal)defenderSprites / 100 * postDamageDefender.Health);

            var shuffledAttackerIndices = Enumerable.Range(0, attackerSprites).ToList();
            var shuffledDefenderIndices = Enumerable.Range(0, defenderSprites).ToList();
            ShuffleList(shuffledAttackerIndices);
            ShuffleList(shuffledDefenderIndices);

            for (int i = 0; i < attackerSprites; i++)
            {
                bool dies = shuffledAttackerIndices[i] >= survivingAttackers;
                var position = new Vector2(Game1._viewport.Width / 4 + 40, Game1._viewport.Height / 2 - (attackerSprites - i * 25) / 2);
                _animations.Add(new UnitBattleSpriteAnim(
                    _attacker.UnitType,
                    5,
                    true,
                    dies ? new[] { "Walk", "Fire", "Die" } : new[] { "Walk", "Fire", "Idle" },
                    _attacker.Allegiance,
                    position
                ));
            }

            for (int i = 0; i < defenderSprites; i++)
            {
                bool dies = shuffledDefenderIndices[i] >= survivingDefenders;
                var position = new Vector2(Game1._viewport.Width / 4 * 3 - 40, Game1._viewport.Height / 2 - (defenderSprites - i * 25) / 2);
                _animations.Add(new UnitBattleSpriteAnim(
                    _defender.UnitType,
                    5,
                    false,
                    dies ? new[] { "Walk", "Die" } : new[] { "Walk", "Idle", "Fire", "Idle" },
                    _defender.Allegiance,
                    position
                ));
            }
        }

        private void ShuffleList<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
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
                _updateState = UnitStates.Queued;
            }

            foreach (var animation in _animations)
            {
                animation.Update(gameTime);
            }
            return _updateState;
        }

        public void Render(SpriteBatch spriteBatch, Viewport viewport, int cameraX, int cameraY, int tilesX, int tilesY)
        {
            var backgroundTopLeft = new Point(viewport.Width / 4, viewport.Height / 4);
            var backgroundSize = new Point(backgroundTopLeft.X*2, backgroundTopLeft.Y*2);
            var backgroundRect = new Rectangle(backgroundTopLeft, backgroundSize);
            var tileBackgroundLeft = new Rectangle(
                backgroundTopLeft,
                new Point(backgroundSize.X/2,backgroundSize.Y)
            );
            var tileBackgroundRight = new Rectangle(
                new Point(backgroundTopLeft.X+backgroundTopLeft.X,backgroundTopLeft.Y),
                new Point(backgroundSize.X/2, backgroundSize.Y)
            );
            spriteBatch.Draw(Game1.SpriteDict["placeholderButton"],backgroundRect,Color.White);
            spriteBatch.Draw(Game1.SpriteDict[_attackerTile.Type + "Background"],tileBackgroundLeft,Color.White);
            spriteBatch.Draw(Game1.SpriteDict[_defenderTile.Type + "Background"], tileBackgroundRight, Color.White);
            foreach (var animation in _animations)
            {
                animation.Render(spriteBatch);
            }
        }
    }
}
