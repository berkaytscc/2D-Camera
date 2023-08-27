using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics.CodeAnalysis;

namespace _2D_Camera
{
    public class Game1 : Game
    {
        // map px (672, 512)
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _map;
        private Viewport _viewPort;

        private Vector2 _screenCenter;
        private Vector2 _mapCenter;

        public static readonly Camera _camera = new Camera();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;

            _graphics.ApplyChanges();

            _camera.ViewportWidth = _graphics.GraphicsDevice.Viewport.Width;
            _camera.ViewportHeight = _graphics.GraphicsDevice.Viewport.Height;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _viewPort = _graphics.GraphicsDevice.Viewport;
            _screenCenter = new Vector2(_viewPort.Width / 2f, _viewPort.Height / 2f);

            _map = Content.Load<Texture2D>("map");

            _mapCenter = new Vector2(_map.Width / 2f, _map.Height / 2f);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputState.Update();
            _camera.HandleInput();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, _camera.TranslationMatrix);

            _spriteBatch.Draw(_map, _screenCenter, null, Color.White, 0f, _mapCenter, 1f, SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}