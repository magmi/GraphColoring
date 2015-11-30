using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
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
        public List<TextBox> NewGameTextBoxes;
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
            NewGameTextBoxes = new List<TextBox>() { new TextBox(content,"liczba-kolorow",colorsNr.ToString(),new Vector2(650,400),new Vector2(845,485)),
                                                     new TextBox(content,"trybBox","",new Vector2(650,50),new Vector2(0,0))};
            GraphButtons = new List<Button>() 
            {
                new Button(new Vector2(50, 50), content, "graf1"),
                new Button(new Vector2(250, 50), content, "graf2"),
                new Button(new Vector2(50, 250), content, "graf3"),
            };
            GameTypeButtons = new List<Button>(){
                new Button(new Vector2(660, 100), content, "gra-vs-gra"),
                new Button(new Vector2(660, 150), content, "gra-vs-komp"),
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
                            if(p1!=null && chosenGraph != null)
                            { 
                                state = InterfaceState.Game;
                                game = new Game(GameType.VerticesColoring, p2 is Computer ? GameMode.SinglePlayer : GameMode.MultiPlayer, chosenGraph, colorsNr, content, p1, p2);
                            }
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
                        case "graf3":
                            ClearButtons(GraphButtons);
                            chosenGraph = PredefinedGraphs.graphs[2];
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
                    
            KeyboardState keybState = Keyboard.GetState();
            
            Keys[] k = keybState.GetPressedKeys();
            if (k.Length > 0)
            {
                string nrS = k[0].ToString();
                int nr = int.Parse(nrS[1].ToString());
                if (nr > 0 && nr < 10)
                    colorsNr = nr;
                NewGameTextBoxes[0].text = colorsNr.ToString();
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
            foreach (TextBox b in NewGameTextBoxes)
                b.Draw(sBatch);
            foreach (Button b in NewGameButtons)
                b.Draw(sBatch);


        }

    }
}
