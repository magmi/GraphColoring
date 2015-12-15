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
        GraphCreator graphCreator;
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
            graphCreator = new GraphCreator(Content);
            IsMouseVisible = true;
            int colorsNr =2;
            Player p1 = new Player("Player 1");
            Computer c1 = new Computer(true);
            PredefinedGraphs.graphs = new List<GardenGraph>() { PredefinedGraphs.GraphZero(Content), PredefinedGraphs.GraphOne(Content), PredefinedGraphs.GraphTwo(Content) };
            
            //game = new Game(GameType.VerticesColoring, GameMode.SinglePlayer, PredefinedGraphs.GraphTwo(Content), colorsNr, Content,p1,c1);
            background = Content.Load<Texture2D>("tlo");

            screenRectangle = new Rectangle(0, 0, 
                GraphicsDevice.PresentationParameters.BackBufferWidth, 
                GraphicsDevice.PresentationParameters.BackBufferHeight);
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 10.0f);
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
            var mousePos = new Point(mouseState.X, mouseState.Y);
            switch (playerInterface.state)
            {
                case InterfaceState.MainMenu:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                        playerInterface.MainMenuCheck(mousePos);
                    break;

                case InterfaceState.NewGame:
                    playerInterface.NewGameKeyCheck();
                    if (mouseState.LeftButton == ButtonState.Pressed)
                        playerInterface.NewGameCheck(mousePos, ref game, Content);
                    break;

                case InterfaceState.LoginSingle:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                        playerInterface.LoginSingleCheck(mousePos, ref game, Content);
                    playerInterface.LoginKeyCheck();
                    break;
                case InterfaceState.LoginMulti:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                        playerInterface.LoginMultiCheck(mousePos, ref game, Content);
                    playerInterface.LoginKeyCheck();
                    break;
                case InterfaceState.Game:
                    ManageGame(mouseState, gameTime);
                    break;
                case InterfaceState.GCVertices:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                        graphCreator.CheckVerticesAsking(mousePos, playerInterface, Content);
                    graphCreator.CheckKeyVerticesAsking();
                    break;
                case InterfaceState.GraphCreation:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                        graphCreator.CheckGraphCreator(mousePos, playerInterface, Content);
                    break;
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
            switch (playerInterface.state)
            {
                case InterfaceState.MainMenu:
                    playerInterface.MainMenuDraw(spriteBatch);
                    break;
                case InterfaceState.NewGame:
                    playerInterface.NewGameDraw(spriteBatch);
                    break;
                case InterfaceState.LoginSingle:
                    playerInterface.LoginSingleDraw(spriteBatch);
                    break;
                case InterfaceState.LoginMulti:
                    playerInterface.LoginMultiDraw(spriteBatch);
                    break;
                case InterfaceState.Game:
                    if (game.graph != null)
                    {
                        game.graph.DrawAllElements(spriteBatch);
                        game.DrawColorPalete(spriteBatch);
                        game.DrawPlayers(spriteBatch);
                    }
                    break;
                case InterfaceState.GCVertices:
                    graphCreator.DrawVericesAsking(spriteBatch);
                    break;
                case InterfaceState.GraphCreation:
                    graphCreator.DrawGraphCreator(spriteBatch);
                    break;
            }

            base.Draw(gameTime);
        }

        public void ManageGame(MouseState mouseState, GameTime gameTime)
        {
            bool didGardenerWon;
            if (!wasChecked)
            {
                if (game.whoseTurn == 0)
                {

                    if (!game.gardenerStartedMove && gameStarted)
                    {
                        //MessageBox(new IntPtr(), "Gardener turn", "Next turn", 0);
                        game.ChangeTurn(game.player1.isGardener);
                        game.gardenerStartedMove = true;
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (game.gameType == GameType.VerticesColoring)
                            CheckForFlowersClicked(mouseState);
                        else
                            CheckForFencesClicked(mouseState);

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
                            //MessageBox(new IntPtr(), "Neighbour turn", "Next turn", 0);
                            game.ChangeTurn(!game.player1.isGardener);
                            game.gardenerStartedMove = false;
                        }

                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            if(game.gameType == GameType.VerticesColoring)
                                CheckForFlowersClicked(mouseState);
                            else
                                CheckForFencesClicked(mouseState);

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

                    game = null;
                    wasChecked = false;
                    gameStarted = false;
                    playerInterface = new PlayerInterface(Content);
                    playerInterface.state = InterfaceState.MainMenu;
                }
                else
                    wasChecked = true;
            }
        }

        public void DrawBackground(SpriteBatch sBatch)
        {
            sBatch.Begin();
            sBatch.Draw(background, screenRectangle, Color.White);
            sBatch.End();
        }

        public void CheckForFencesClicked(MouseState mouseState)
        {
            var mousePos = new Point(mouseState.X, mouseState.Y);
            int index = 0;
            if (game.lastClicked == null && game.CheckIfMouseClickedOnFence(mousePos, out index))
            {
                game.graph.fences[index].color = Color.LightBlue;
                game.lastClicked = game.graph.fences[index];
            }
        }

        public void CheckForFlowersClicked(MouseState mouseState)
        {
            var mousePos = new Point(mouseState.X, mouseState.Y);
            int index = 0;
            bool b = game.CheckIfMouseClickedOnFlower(mousePos, out index);
            if (game.lastClicked == null && b)
            {
                game.graph.flowers[index].color = Color.LightBlue;
                game.lastClicked = game.graph.flowers[index];
            }
            else if(b && game.lastClicked == game.graph.flowers[index])
            {
                game.graph.flowers[index].color = Color.White;
                game.lastClicked = null;
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
                    game.AddPoints();
                    game.lastClicked = null;
                    game.whoseTurn = (game.whoseTurn + 1)%2;
                }
            }
        }

    }
}
