using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GraphColoring
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Game game;
        Flower lastClicked = null;
        PlayerInterface playerInterface;
        Texture2D background;
        Rectangle screenRectangle;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1200;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here      
            playerInterface = new PlayerInterface(Content);
            IsMouseVisible = true;
            int colorsNr =2;
            Player p1 = new Player("Player 1");
            Computer c1 = new Computer(true);
           
            game = new Game(GameType.VerticesColoring, PredefinedGraphs.GraphTwo(Content), colorsNr, Content,p1,c1);
            background = Content.Load<Texture2D>("tlo");

            screenRectangle = new Rectangle(0, 0, 
                GraphicsDevice.PresentationParameters.BackBufferWidth, 
                GraphicsDevice.PresentationParameters.BackBufferHeight);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
           
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if(playerInterface.state == InterfaceState.MainMenu)
                {
                    var mousePos = new Point(mouseState.X, mouseState.Y);
                    playerInterface.MainMenuCheck(mousePos);
                   
                }
                else if(playerInterface.state == InterfaceState.NewGame)
                {
                    var mousePos = new Point(mouseState.X, mouseState.Y);
                    playerInterface.NewGameCheck(mousePos);
                }
                else if(playerInterface.state == InterfaceState.Game)
                {
                    CheckForFlowersClicked(mouseState);
                    CheckForColorsClicked(mouseState);
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            DrawBackground(spriteBatch);
            if (playerInterface.state == InterfaceState.MainMenu)
            {
                playerInterface.MainMenuDraw(spriteBatch);
            }
            else if(playerInterface.state == InterfaceState.NewGame)
            {
                playerInterface.NewGameDraw(spriteBatch);
            }
            else if (playerInterface.state== InterfaceState.Game)
            {
                game.graph.DrawAllElements(spriteBatch);
                game.DrawColorPalete(spriteBatch);
            }
            base.Draw(gameTime);
        }

        public void DrawBackground(SpriteBatch sBatch)
        {
            sBatch.Begin();
            sBatch.Draw(background, screenRectangle, Color.White);
            sBatch.End();
        }

        

        public void CheckForFlowersClicked(MouseState mouseState)
        {
            var mousePos = new Point(mouseState.X, mouseState.Y);
            int index = 0;
            if (lastClicked == null && game.CheckIfMouseClickedOnFlower(mousePos, out index))
            {
                game.graph.flowers[index].color = Color.LightBlue;
                lastClicked = game.graph.flowers[index];

            }
        }
        public void CheckForColorsClicked(MouseState mouseState)
        {
            var mousePos = new Point(mouseState.X, mouseState.Y);
            int index = 0;
            if (lastClicked != null && game.CheckIfMouseClickedOnColor(mousePos, out index))
            {
                if(game.CheckIfValidMove(lastClicked, game.colors[index]))
                {
                    game.graph.MakeMove(lastClicked, game.colors[index]);
                    //lastClicked.color = game.colors[index];
                    lastClicked = null;
                    if (game.graph.IsColoringPossible(game.gameType, game.colors))
                    {
                        if (game.player2 is Computer)
                            ((Computer)game.player2).CalculateMove(game);
                    }
                }
            }
        }

    }
}
