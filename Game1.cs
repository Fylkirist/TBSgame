﻿using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.Json.Serialization;
using TBSgame.Scene;

namespace TBSgame
{
    enum GameState
    {
        TitleScreen,
        BattleScene
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Viewport _viewport;
        private SpriteBatch _spriteBatch;
        private static Dictionary<string, Texture2D> _spriteDict = new();
        private static Dictionary<string, SpriteFont> _fonts = new();
        private GameState _state;
        public static MouseState MouseStateCurrent;
        public static MouseState MouseStatePrevious;

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

            _spriteDict.Add("factory",Content.Load<Texture2D>("Sprites/Map/factory"));
            _spriteDict.Add("ForestTile", Content.Load<Texture2D>("Sprites/Map/ForestTile"));
            _spriteDict.Add("PathTile", Content.Load<Texture2D>("Sprites/Map/PathTile"));
            _spriteDict.Add("PlainTile", Content.Load<Texture2D>("Sprites/Map/PlainTile"));
            _spriteDict.Add("MountainTile", Content.Load<Texture2D>("Sprites/Map/MountainTile"));
            _spriteDict.Add("placeholderButton",Content.Load<Texture2D>("UI/PlaceholderButton"));
            _spriteDict.Add("placeholderTitle", Content.Load<Texture2D>("UI/PlaceholderTitle"));

            _fonts.Add("placeholderFont",Content.Load<SpriteFont>("Fonts/Font"));
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            MouseStateCurrent = Mouse.GetState();
            _currentScene.HandleInput(MouseStateCurrent,MouseStatePrevious);
            MouseStatePrevious = MouseStateCurrent;

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

        private void Save()
        {

        }

        private void Load()
        {

        }
    }
}