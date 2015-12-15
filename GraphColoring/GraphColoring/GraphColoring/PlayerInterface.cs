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
    public enum InterfaceState { MainMenu, NewGame, Game, LoginSingle, LoginMulti, GraphCreation, GCVertices }
    class PlayerInterface
    {
        
        public InterfaceState state;

        //MainMenu
        public List<Button> MainMenuButtons;
        public Texture2D titleTexture;
        public Vector2 titleVector;

        //NewGame
        public List<ClickableObject> NewGameButtons;
        public List<TextBox> NewGameTextBoxes;
        public List<ClickableObject> GraphButtons;
        public List<Button> GameTypeButtons;
        public List<Button> GameColoringButtons;
        public List<Button> GameModeButtons;
       
        public GardenGraph chosenGraph;
        public int colorsNr = 3;
        public Player p1;
        public Player p2;
        public GameType gT = GameType.EdgesColoring;
        public GameOrder gO = GameOrder.GN;

        //LoginSingle
        public List<TextBox> LoginTextBoxes;
        public List<Button> LoginButtons;
        public int ActiveTextBox = 0;
        public StringBuilder[] PlayerSb;

        public PlayerInterface(ContentManager content)
        {
            PlayerSb = new StringBuilder[] { new StringBuilder("Player1"), new StringBuilder("Player2") };
            state = InterfaceState.MainMenu;
            MainMenuButtons = new List<Button>() { new Button(new Vector2(470, 300), content, "nowa-gra"),
                new Button(new Vector2(470, 500), content, "anuluj"),};

            NewGameTextBoxes = new List<TextBox>() { 
                new TextBox(content,colorsNr.ToString(),new Vector2(650,390),new Vector2(845,475),"liczba-kolorow"),
                new TextBox(content,"",new Vector2(650,50),new Vector2(0,0),"trybBox"),
                new TextBox(content,"",new Vector2(650,220),new Vector2(0,0),"kolorowanie"),
                new TextBox(content,"",new Vector2(650,560),new Vector2(0,0),"gra"),

            };
            p1 = new Player();
            p2 = new Player();
            GraphButtons = new List<ClickableObject>() 
            {
                new Button(new Vector2(50, 50), content, "graf1"),
                new Button(new Vector2(250, 50), content, "graf2"),
                new Button(new Vector2(50, 250), content, "graf3"),
            };
            GameTypeButtons = new List<Button>(){
                new Button(new Vector2(660, 100), content, "gra-vs-gra"),
                new Button(new Vector2(660, 150), content, "gra-vs-komp"),
            };
            GameColoringButtons = new List<Button>(){
                new Button(new Vector2(660, 260), content, "kwiatkow"),
                new Button(new Vector2(660, 310), content, "plotkow"),
            };
            GameModeButtons = new List<Button>(){
                new Button(new Vector2(660, 610), content, "sasiad-ogrodnik"),
                new Button(new Vector2(660, 660), content, "ogrodnik-sasiad"),
            };
            NewGameButtons = new List<ClickableObject>(){                
                new Button(new Vector2(350, 730), content, "anuluj"),
                new Button(new Vector2(650, 730), content, "start"),
            };
            foreach (Button b in GameTypeButtons)
                NewGameButtons.Add(b);
            foreach (Button b in GameColoringButtons)
                NewGameButtons.Add(b);
            foreach (Button b in GameModeButtons)
                NewGameButtons.Add(b);
            foreach (ClickableObject b in GraphButtons)
                NewGameButtons.Add(b);

            LoginTextBoxes = new List<TextBox>() 
            {   new TextBox(content, "Player1", new Vector2(100, 200), new Vector2(550, 250), "Gracz1") ,
                new TextBox(content, "Player2", new Vector2(100, 500), new Vector2(550, 550), "Gracz2")
            };
            LoginButtons = new List<Button>(){                
                new Button(new Vector2(350, 30), content, "anuluj"),
                new Button(new Vector2(650, 30), content, "start"),
            };

            titleTexture = content.Load<Texture2D>("title");
            titleVector = new Vector2(250, 100);
            chosenGraph = null;
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
                        case "anuluj":
                            state = InterfaceState.GCVertices;
                            break;
                    }
                }
            }
        }

        public void LoginSingleCheck(Point mousePos,ref Game game, ContentManager content)
        {
            for (int i = 0; i < LoginButtons.Count; i++)
            {
                if (LoginButtons[i].ContainsPoint(mousePos))
                {
                    switch (LoginButtons[i].name)
                    {
                        case "start": 
                            state = InterfaceState.Game;
                            game = new Game(gT, p2 is Computer ? GameMode.SinglePlayer : GameMode.MultiPlayer, chosenGraph, colorsNr, content, p1, p2,gO);                     
                            break;
                        case "anuluj":
                            state = InterfaceState.MainMenu;
                            break;
                    }
                }
            }
            if (LoginTextBoxes[0].ContainsPoint(mousePos))
                ActiveTextBox = 0;
           
        }

        public void LoginMultiCheck(Point mousePos, ref Game game, ContentManager content)
        {
            for (int i = 0; i < LoginButtons.Count; i++)
            {
                if (LoginButtons[i].ContainsPoint(mousePos))
                {
                    switch (LoginButtons[i].name)
                    {
                        case "start":
                            state = InterfaceState.Game;
                            game = new Game(gT, p2 is Computer ? GameMode.SinglePlayer : GameMode.MultiPlayer, chosenGraph, colorsNr, content, p1, p2, gO);
                            break;
                        case "anuluj":
                            state = InterfaceState.MainMenu;
                            break;
                    }
                }
            }
            for (int i = 0; i < LoginTextBoxes.Count;i++ )
                if (LoginTextBoxes[i].ContainsPoint(mousePos))
                    ActiveTextBox = i;            

        }

        public void LoginKeyCheck()
        {
            KeyboardState keybState = Keyboard.GetState();
            Keys[] k = keybState.GetPressedKeys();
            if (k.Length > 0)
            {                
                if (k[0] == Keys.Back)
                {
                    if (PlayerSb[ActiveTextBox].Length > 0)
                        PlayerSb[ActiveTextBox].Remove(PlayerSb[ActiveTextBox].Length - 1, 1);
                    UpdateLogins();
                    return;
                }
                PlayerSb[ActiveTextBox].Append(k[0].ToString());
                UpdateLogins();
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
                                state = p2 is Computer ? InterfaceState.LoginSingle : InterfaceState.LoginMulti;                            
                            break;
                        case "anuluj":
                            state = InterfaceState.MainMenu;                           
                            break;
                        case "graf1":
                            ClearButtons(GraphButtons);
                            chosenGraph = PredefinedGraphs.graphs[0].Copy();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "graf2":
                            ClearButtons(GraphButtons);
                            chosenGraph = PredefinedGraphs.graphs[1].Copy();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "graf3":
                            ClearButtons(GraphButtons);
                            chosenGraph = PredefinedGraphs.graphs[2].Copy();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "graf":
                            ClearButtons(GraphButtons);                               
                            chosenGraph = PredefinedGraphs.graphs[NewGameButtons[i].index].Copy();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "gra-vs-gra":
                            ClearButtons(GameTypeButtons);
                            p1 = new Player("Player1");
                            p2 = new Player("Player2");
                            UpdateLogins();
                            UpdatePlayers();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "gra-vs-komp":
                            ClearButtons(GameTypeButtons);
                            p1 = new Player("Player1");
                            p2 = new Computer(true);
                            UpdateLogins();
                            UpdatePlayers();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "kwiatkow":
                            ClearButtons(GameColoringButtons);
                            gT = GameType.VerticesColoring;
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "plotkow":
                            ClearButtons(GameColoringButtons);
                            gT = GameType.EdgesColoring;
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "sasiad-ogrodnik":
                            ClearButtons(GameModeButtons);
                            gO = GameOrder.NG;
                            UpdatePlayers();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "ogrodnik-sasiad":
                            ClearButtons(GameModeButtons);
                            gO = GameOrder.GN;
                            UpdatePlayers();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;

                    }
                }
            }                    
           
        
        }

        public void UpdatePlayers()
        {
            if(gO == GameOrder.GN)
            {
                p1.isGardener = true;
                p2.isGardener = false;
            }
            else
            {
                p1.isGardener = false;
                p2.isGardener = true;
            }
        }

        public void NewGameKeyCheck()
        {
            KeyboardState keybState = Keyboard.GetState();
            Keys[] k = keybState.GetPressedKeys();
            if (k.Length > 0)
            {
                string nrS = k[0].ToString();
                if (nrS.Length > 1)
                {
                    int nr = int.Parse(nrS[1].ToString());
                    if (nr > 0 && nr < 10)
                    {
                        colorsNr = nr;
                        NewGameTextBoxes[0].text = colorsNr.ToString();
                    }
                }
            }
        }


        public void UpdateLogins()
        {
            p1.login = PlayerSb[0].ToString();
            p2.login = PlayerSb[1].ToString();

            LoginTextBoxes[0].text = PlayerSb[0].ToString();
            LoginTextBoxes[1].text = PlayerSb[1].ToString();
        }
        
        public void ClearButtons(List<ClickableObject> list)
        {
            foreach (ClickableObject b in list)
                b.color = Color.White;
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
            foreach (ClickableObject b in NewGameButtons)
                b.Draw(sBatch);
        }
        public void LoginSingleDraw(SpriteBatch sBatch)
        {
            LoginTextBoxes[0].Draw(sBatch);
            foreach (Button b in LoginButtons)
                b.Draw(sBatch);
        }
        public void LoginMultiDraw(SpriteBatch sBatch)
        {
            foreach(TextBox t in LoginTextBoxes)
                t.Draw(sBatch);
            foreach (Button b in LoginButtons)
                b.Draw(sBatch);
        }
    }
}
