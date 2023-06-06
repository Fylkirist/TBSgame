using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBSgame.Scene;

namespace TBSgame.Assets
{
    internal class UnitBattleSpriteAnim
    {
        private string _unitType;
        private double _time;
        private double _duration;
        private Vector2 _position;
        private string[] _orders;
        private int _ordersIndex;
        private Texture2D _currentFrame;
        private bool _isAttacker;
        private string _allegiance;
        private SoundEffectInstance _effectInstance;
        private bool _soundPlayed;

        internal UnitBattleSpriteAnim(string unitType, double duration, bool isAttacker, string[] orders, string allegiance, Vector2 initialPosition)
        {
            _unitType = unitType;
            _duration = duration;
            _time = 0;
            _position = initialPosition;
            _orders = orders;
            _ordersIndex = 0;
            _allegiance = allegiance;
            _isAttacker = isAttacker;
            _currentFrame = Game1.SpriteDict[_unitType + "BattleIdle" + _allegiance];
            Random rand = new Random();
            _time += rand.NextDouble()/6;
            _effectInstance = Game1.SoundEffects[_unitType + "Fire"].CreateInstance();
            _soundPlayed = false;
        }
        public void Update(GameTime gameTime)
        {
            _time += gameTime.ElapsedGameTime.TotalSeconds;
            switch (_orders[_ordersIndex])
            {
                case "Walk":
                    WalkCycle();
                    break;
                case "Fire":
                    FireAnim();
                    break;
                case "Die":
                    DeathAnim();
                    break;
                case "Idle":
                    IdleAnim();
                    break;
            }
            _ordersIndex = _time > _orders.Length?_orders.Length-1:(int)Math.Floor(_time);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            var destinationRect = new Rectangle(
                new Point((int)_position.X-50, (int)_position.Y-50),
                new Point(100, 100)
            );

            SpriteEffects spriteEffects = _isAttacker ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(_currentFrame, destinationRect, null, Color.White, 0f, Vector2.Zero, spriteEffects, 0f);
        }


        public void WalkCycle()
        {
            if (_isAttacker)
            {
                _position.X += 2 * (float)(_time - Math.Floor(_time));
            }
            else
            {
                _position.X -= 2 * (float)(_time - Math.Floor(_time));
            }

            var cycleTime = _time - Math.Floor(_time);

            _currentFrame = cycleTime is > 0.25 and < 0.50 or > 0.75 and < 0.99 ? 
                Game1.SpriteDict[_unitType + "BattleWalk" + 0 + _allegiance] :
                Game1.SpriteDict[_unitType + "BattleWalk" + 1 + _allegiance];
        }

        public void DeathAnim()
        {
            var cycleTime = _time - _ordersIndex;
            
            if ( cycleTime is > 0 and < 0.25)
            {
                _currentFrame = Game1.SpriteDict[_unitType + "BattleDeath" + 0 + _allegiance];

            }
            else if (cycleTime is > 0.25 and <0.50)
            { 
                _currentFrame = Game1.SpriteDict[_unitType + "BattleDeath" + 1 + _allegiance];
            }
            else
            {
                _currentFrame = Game1.SpriteDict[_unitType + "BattleDeath" + 2 + _allegiance];
            }
            
        }

        public void FireAnim()
        {
            var cycleTime = _time - _ordersIndex;
            if (!_soundPlayed)
            {
                _effectInstance.Play();
            }
            if (cycleTime is > 0 and < 0.25)
            {
                _currentFrame = Game1.SpriteDict[_unitType + "BattleFire" + 0 + _allegiance];

            }
            else if (cycleTime is > 0.25 and < 0.50)
            {
                _currentFrame = Game1.SpriteDict[_unitType + "BattleFire" + 1 + _allegiance];
            }
            else if(cycleTime is > 0.50 and < 0.75)
            {
                _currentFrame = Game1.SpriteDict[_unitType + "BattleFire" + 2 + _allegiance];
            }
            else
            {
                _currentFrame = Game1.SpriteDict[_unitType + "BattleIdle" + _allegiance];
            }
        }

        public void IdleAnim()
        {
            _currentFrame = Game1.SpriteDict[_unitType + "BattleIdle" + _allegiance];
        }
    }
}
