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
using System.Runtime.InteropServices;

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

        PlayerInterface playerInterface;

        Texture2D background;
        Rectangle screenRectangle;
        bool wasChecked;
        bool gameStarted;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWndle, String text, String caption, int buttons);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1200;
            Content.RootDirectory = "Content";
            this.wasChecked = false;
            this.gameStarted = false;
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
            PredefinedGraphs.graphs = new List<GardenGraph>() { PredefinedGraphs.GraphZero(Content), PredefinedGraphs.GraphOne(Content), PredefinedGraphs.GraphTwo(Content) };
            
            game = new Game(GameType.VerticesColoring, GameMode.SinglePlayer, PredefinedGraphs.GraphTwo(Content), colorsNr, Content,p1,c1);
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
            bool didGardenerWon;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            var mouseState = Mouse.GetState();

            if(playerInterface.state == InterfaceState.MainMenu)
            {
                var mousePos = new Point(mouseState.X, mouseState.Y);
                playerInterface.MainMenuCheck(mousePos);
                   
            }
            else if(playerInterface.state == InterfaceState.NewGame)
            {
                var mousePos = new Point(mouseState.X, mouseState.Y);
                playerInterface.NewGameCheck(mousePos, ref game, Content);
            }
            else if (playerInterface.state == InterfaceState.LoginSingle)
            {
                var mousePos = new Point(mouseState.X, mouseState.Y);
                playerInterface.LoginSingleCheck(mousePos, ref game, Content);
            }
            else if (playerInterface.state == InterfaceState.LoginMulti)
            {
                var mousePos = new Point(mouseState.X, mouseState.Y);
                playerInterface.LoginMultiCheck(mousePos, ref game, Content);
            }
            else if (playerInterface.state == InterfaceState.Game)
            {

                if (!wasChecked)
                {
                    if (game.whoseTurn == 0)
                    {

                        if (!game.gardenerStartedMove && gameStarted)
                        {
                            MessageBox(new IntPtr(), "Gardener turn", "Next turn", 0);
                            game.gardenerStartedMove = true;
                        }

                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            CheckForFlowersClicked(mouseState);
                            CheckForColorsClicked(mouseState);
                        }
                    }
                    else if (game.whoseTurn == 1)
                    {
                        if (game.gameMode == GameMode.SinglePlayer)
                        {
                            ((Computer)game.player2).CalculateMove(game);
                            ((Computer)game.player2).elapsed += gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            if (game.gardenerStartedMove && gameStarted)
                            {
                                MessageBox(new IntPtr(), "Neighbour turn", "Next turn", 0);
                                game.gardenerStartedMove = false;
                            }

                            if (mouseState.LeftButton == ButtonState.Pressed)
                            {
                                CheckForFlowersClicked(mouseState);
                                CheckForColorsClicked(mouseState);
                            }
                        }
                    }
                }

                if (game.CheckIfEnd(out didGardenerWon))
                {

                    if (wasChecked)
                    {
                        if (didGardenerWon)
                            MessageBox(new IntPtr(), "Gardener won", "Game over", 0);
                        else
                            MessageBox(new IntPtr(), "Neighbour won", "Game over", 0);

                        this.Exit();
                    }

                    if (!wasChecked)
                    {
                        wasChecked = true;
                    }
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

            if (!gameStarted)
                gameStarted = true;

            if (playerInterface.state == InterfaceState.MainMenu)
            {
                playerInterface.MainMenuDraw(spriteBatch);
            }
            else if(playerInterface.state == InterfaceState.NewGame)
            {
                playerInterface.NewGameDraw(spriteBatch);
            }
            else if (playerInterface.state == InterfaceState.LoginSingle)
            {
                playerInterface.LoginSingleDraw(spriteBatch);
            }
            else if (playerInterface.state == InterfaceState.LoginMulti)
            {
                playerInterface.LoginMultiDraw(spriteBatch);
            }
            else if (playerInterface.state == InterfaceState.Game)
            {
                if (game.graph != null)
                {
                    game.graph.DrawAllElements(spriteBatch);
                    game.DrawColorPalete(spriteBatch);
                    game.DrawPlayers(spriteBatch);
                }
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
            if (game.lastClicked == null && game.CheckIfMouseClickedOnFlower(mousePos, out index))
            {
                game.graph.flowers[index].color = Color.LightBlue;
                game.lastClicked = game.graph.flowers[index];
            }
        }
        public void CheckForColorsClicked(MouseState mouseState)
        {
            var mousePos = new Point(mouseState.X, mouseState.Y);
            int index = 0;
            if (game.lastClicked != null && game.CheckIfMouseClickedOnColor(mousePos, out index))
            {
                if(game.CheckIfValidMove(game.lastClicked, game.colors[index]))
                {
                    game.graph.MakeMove(game.lastClicked, game.colors[index]);
                    game.lastClicked = null;
                    game.whoseTurn = (game.whoseTurn + 1)%2;
                }
            }
        }

    }
}
