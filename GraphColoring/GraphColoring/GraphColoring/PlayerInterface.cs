using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    public enum InterfaceState { MainMenu, NewGame, Game }
    class PlayerInterface
    {
        
        public InterfaceState state;

        public List<Button> MainMenuButtons;
        public List<Button> NewGameButtons;
        public List<Button> GraphButtons;
        public List<Button> GameTypeButtons;
        public Texture2D titleTexture;
        public Vector2 titleVector;
        public GardenGraph chosenGraph;
        public int colorsNr = 3;
        public Player p1;
        public Player p2;
        public PlayerInterface(ContentManager content)
        {
            state = InterfaceState.MainMenu;
            MainMenuButtons = new List<Button>();
            MainMenuButtons.Add(new Button(new Vector2(470, 300), content, "nowa-gra"));
            GraphButtons = new List<Button>() 
            {
                new Button(new Vector2(50, 50), content, "graf1"),
                new Button(new Vector2(250, 50), content, "graf2"),
            };
            GameTypeButtons = new List<Button>(){
                new Button(new Vector2(650, 130), content, "gra-vs-gra"),
                new Button(new Vector2(650, 330), content, "gra-vs-komp"),
            };
            NewGameButtons = new List<Button>(){                
                new Button(new Vector2(350, 730), content, "anuluj"),
                new Button(new Vector2(650, 730), content, "start"),
            };
            foreach (Button b in GameTypeButtons)
                NewGameButtons.Add(b);
            foreach (Button b in GraphButtons)
                NewGameButtons.Add(b);

            titleTexture = content.Load<Texture2D>("title");
            titleVector = new Vector2(250, 100);

        }

        public void MainMenuCheck(Point mousePos)
        {
            for (int i = 0; i < MainMenuButtons.Count; i++)
            {
                if (MainMenuButtons[i].ContainsPoint(mousePos))
                {
                    switch (MainMenuButtons[i].name)
                    {
                        case "nowa-gra":
                            state = InterfaceState.NewGame;
                            break;
                    }
                }
            }
        }

        public void NewGameCheck(Point mousePos, ref Game game, ContentManager content)
        {
            for (int i = 0; i < NewGameButtons.Count; i++)
            {
                if (NewGameButtons[i].ContainsPoint(mousePos))
                {
                    switch (NewGameButtons[i].name)
                    {
                        case "start":
                            state = InterfaceState.Game;
                            game = new Game(GameType.VerticesColoring, chosenGraph, colorsNr, content, p1, p2);
                            break;
                        case "anuluj":
                            state = InterfaceState.MainMenu;                           
                            break;
                        case "graf1":
                            ClearButtons(GraphButtons);
                            chosenGraph = PredefinedGraphs.graphs[0];
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "graf2":
                            ClearButtons(GraphButtons);
                            chosenGraph = PredefinedGraphs.graphs[1];
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "gra-vs-gra":
                            ClearButtons(GameTypeButtons);
                            p1 = new Player();
                            p2 = new Player();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "gra-vs-komp":
                            ClearButtons(GameTypeButtons);
                            p1 = new Player();
                            p2 = new Computer(true);
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                    }
                }
               
                    
            }
        }
        public void ClearButtons(List<Button> list)
        {
            foreach (Button b in list)
                b.color = Color.White;
        }

        public void MainMenuDraw(SpriteBatch sBatch)
        {
            sBatch.Begin();
            sBatch.Draw(titleTexture, titleVector, Color.White);
            sBatch.End();
            foreach (Button b in MainMenuButtons)
                b.Draw(sBatch);
        }

        public void NewGameDraw(SpriteBatch sBatch)
        {            
            foreach (Button b in NewGameButtons)
                b.Draw(sBatch);
            foreach (Button b in GraphButtons)
                b.Draw(sBatch);
            foreach (Button b in GameTypeButtons)
                b.Draw(sBatch);
        }

    }
}
