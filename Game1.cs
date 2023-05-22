using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.Json.Serialization;
using TBSgame.Assets;
using TBSgame.Scene;

namespace TBSgame
{
    public enum GameState
    {
        TitleScreen,
        BattleScene
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public static Viewport _viewport;
        private SpriteBatch _spriteBatch;
        private static Dictionary<string, Texture2D> _spriteDict = new();
        private static Dictionary<string, SpriteFont> _fonts = new();
        private GameState _state;
        public MouseState MouseStateCurrent;
        public MouseState MouseStatePrevious;
        private KeyboardState _keyboardStatePrevious;
        private KeyboardState _keyboardStateCurrent;

        public static Dictionary<string, Texture2D> SpriteDict { get => _spriteDict; }
        public static Dictionary<string, SpriteFont> Fonts { get => _fonts; }
        private IScene _currentScene;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferHeight = 780;
            _graphics.PreferredBackBufferWidth = 1280;
            _viewport = new Viewport(0, 0, _graphics.PreferredBackBufferWidth = 1280, _graphics.PreferredBackBufferHeight = 780);
        }

        protected override void Initialize()
        {
            base.Initialize();
            _state = GameState.TitleScreen;
            _currentScene = new TitleScreen(_viewport);

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteDict.Add("Factory",Content.Load<Texture2D>("Sprites/Map/factory"));
            _spriteDict.Add("redFactory", Content.Load<Texture2D>("Sprites/Map/factory"));
            _spriteDict.Add("ForestTile", Content.Load<Texture2D>("Sprites/Map/ForestTile"));
            _spriteDict.Add("PathTile", Content.Load<Texture2D>("Sprites/Map/PathTile"));
            _spriteDict.Add("PlainTile", Content.Load<Texture2D>("Sprites/Map/PlainTile"));
            _spriteDict.Add("MountainTile", Content.Load<Texture2D>("Sprites/Map/MountainTile"));
            _spriteDict.Add("placeholderButton",Content.Load<Texture2D>("UI/PlaceholderButton"));
            _spriteDict.Add("placeholderTitle", Content.Load<Texture2D>("UI/PlaceholderTitle"));
            _spriteDict.Add("idleMusketeerred", Content.Load<Texture2D>("UI/PlaceholderButton"));
            _spriteDict.Add("idleMusketeerblue", Content.Load<Texture2D>("UI/PlaceholderButton"));
            _spriteDict.Add("TileBorder",Content.Load<Texture2D>("Sprites/Map/TileBorder"));
            _spriteDict.Add("AvailableTileBorder", Content.Load<Texture2D>("Sprites/Map/AvailableTileBorder"));
            _spriteDict.Add("SelectionMarker", Content.Load<Texture2D>("Sprites/Map/SelectionMarker"));

            _fonts.Add("placeholderFont",Content.Load<SpriteFont>("Fonts/Font"));
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            MouseStateCurrent = Mouse.GetState();
            _keyboardStateCurrent = Keyboard.GetState();

            _currentScene.HandleInput(MouseStateCurrent,MouseStatePrevious,_keyboardStateCurrent,_keyboardStatePrevious,gameTime);

            
            _keyboardStatePrevious = _keyboardStateCurrent;
            MouseStatePrevious = MouseStateCurrent;

            var stateUpdate = _currentScene.CheckStateUpdate();
            if (stateUpdate != _state)
            {
                UpdateState(stateUpdate);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _currentScene.Render(_spriteBatch,_viewport);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void UpdateState(GameState newState)
        {
            switch (newState)
            {
                case GameState.BattleScene:
                    var map = Test.TestMap();
                    _currentScene = new BattleScene(map,Test.TestList(),"bruh");
                    _state = newState;
                    break;
                case GameState.TitleScreen:
                    _currentScene = new TitleScreen(_viewport);
                    _state = newState;
                    break;
            }
        }

        private void Save()
        {

        }

        private void Load()
        {

        }
    }
}