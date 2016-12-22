using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;
namespace GraphColoring
{
    public enum InterfaceState { MainMenu, NewGame, Game, LoginSingle, LoginMulti, GraphCreation, GCVertices }
    public class PlayerInterface
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
        public List<Button> GameDifButtons;
       
        public GardenGraph chosenGraph;
        public int colorsNr = 3;
        public Player p1;
        public Player p2;
        public bool easyMode;
        public GameType gT = GameType.VerticesColoring;
        public GameOrder gO = GameOrder.GN;

        //LoginSingle
        public List<TextBox> LoginTextBoxes;
        public List<Button> LoginButtons;
        public int ActiveTextBox = 0;
        public StringBuilder[] PlayerSb;


        public PlayerInterface(ContentManager content)
        {
            PlayerSb = new StringBuilder[] { new StringBuilder("Gracz1"), new StringBuilder("Gracz2") };
            state = InterfaceState.MainMenu;
            MainMenuButtons = new List<Button>() { new Button(Game1.GetRatioDimensions(new Vector2(470, 300)), content, "nowa-gra"),
                new Button(Game1.GetRatioDimensions(new Vector2(440, 400)), content, "stworz-graf"),
                new Button(Game1.GetRatioDimensions(new Vector2(485, 500)), content, "wyjscie"),
            };

            NewGameTextBoxes = new List<TextBox>() { 
                new TextBox(content,colorsNr.ToString(),Game1.GetRatioDimensions(new Vector2(650,390)),Game1.GetRatioDimensions(new Vector2(845,475)),"liczba-kolorow",0,"CzcionkaUI"),
                new TextBox(content,"",Game1.GetRatioDimensions(new Vector2(650,50)),new Vector2(0,0),"trybBox"),
                new TextBox(content,"",Game1.GetRatioDimensions(new Vector2(650,220)),new Vector2(0,0),"kolorowanie"),
                new TextBox(content,"",Game1.GetRatioDimensions(new Vector2(650,560)),new Vector2(0,0),"gra"),
                new TextBox(content,"",Game1.GetRatioDimensions(new Vector2(50,560)),new Vector2(0,0),"poziom"),

            };
            p1 = new Player();
            p1.isGardener = true;
            p1.login = "Gracz1";
            p2 = new Player();
            p2.login = "Gracz2";
            easyMode = true;
            GraphButtons = new List<ClickableObject>() 
            {
                new Button(Game1.GetRatioDimensions(new Vector2(50, 50)), content, "graf1"),
                new Button(Game1.GetRatioDimensions(new Vector2(210, 50)), content, "graf2"),
                new Button(Game1.GetRatioDimensions(new Vector2(370, 50)), content, "graf3"),
            };
            GameTypeButtons = new List<Button>(){
                new Button(Game1.GetRatioDimensions(new Vector2(660, 100)), content, "gra-vs-gra", Color.LightBlue),
                new Button(Game1.GetRatioDimensions(new Vector2(660, 150)), content, "gra-vs-komp"),
            };
            GameDifButtons = new List<Button>(){
                new Button(Game1.GetRatioDimensions(new Vector2(60, 610)), content, "latwy", Color.LightBlue),
                new Button(Game1.GetRatioDimensions(new Vector2(60, 660)), content, "trudny"),
            };
            GameColoringButtons = new List<Button>(){
                new Button(Game1.GetRatioDimensions(new Vector2(660, 260)), content, "kwiatkow", Color.LightBlue),
                new Button(Game1.GetRatioDimensions(new Vector2(660, 310)), content, "plotkow"),
            };
            GameModeButtons = new List<Button>(){
                new Button(Game1.GetRatioDimensions(new Vector2(660, 610)), content, "sasiad-ogrodnik"),
                new Button(Game1.GetRatioDimensions(new Vector2(660, 660)), content, "ogrodnik-sasiad", Color.LightBlue),
            };
            NewGameButtons = new List<ClickableObject>(){                
                new Button(Game1.GetRatioDimensions(new Vector2(350, 730)), content, "anuluj"),
                new Button(Game1.GetRatioDimensions(new Vector2(650, 730)), content, "start"),
            };
            foreach (Button b in GameDifButtons)
                NewGameButtons.Add(b);
            foreach (Button b in GameTypeButtons)
                NewGameButtons.Add(b);
            foreach (Button b in GameColoringButtons)
                NewGameButtons.Add(b);
            foreach (Button b in GameModeButtons)
                NewGameButtons.Add(b);
            foreach (ClickableObject b in GraphButtons)
                NewGameButtons.Add(b);

            LoginTextBoxes = new List<TextBox>() 
            {   new TextBox(content, "Gracz1", Game1.GetRatioDimensions(new Vector2(100, 200)), Game1.GetRatioDimensions(new Vector2(550, 250)), "Gracz1",0,"CzcionkaUI") ,
                new TextBox(content, "Gracz2", Game1.GetRatioDimensions(new Vector2(100, 500)), Game1.GetRatioDimensions(new Vector2(550, 550)), "Gracz2",0,"CzcionkaUI")
            };
            LoginButtons = new List<Button>(){                
                new Button(Game1.GetRatioDimensions(new Vector2(350, 30)), content, "anuluj"),
                new Button(Game1.GetRatioDimensions(new Vector2(650, 30)), content, "start"),
            };

            titleTexture = content.Load<Texture2D>("title");
            titleVector = Game1.GetRatioDimensions(new Vector2(250, 100));
            chosenGraph = null;
        }

        /// <summary>
        /// Funkcja aktualizajaca ilosc grafow do wyboru
        /// </summary>
        /// <param name="content"></param>
        public void InterfaceUpdate(ContentManager content)
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml", SearchOption.TopDirectoryOnly);
            GardenGraph newGraph;
            int index;
            string name;

            if (files.Length + 3 == GraphButtons.Count)
                return;

            foreach (var file in files)
            {
                index = -1;

                for (int i = 0; i < file.Length; i++)
                    if (file[i] == '\\')
                        index = i;
                name = file.Substring(index + 1, file.Length - index - 5);

                if (GraphButtons.Exists(x => x is TextBox && ((TextBox)x).text == name))
                    continue;

                newGraph = null;
                newGraph = SerializationManager.DeSerializeObject<GardenGraph>(file);

                if (newGraph != null)
                {
                    int n = GraphButtons.Count;

                    int x;
                    if (n % 3 == 0)
                        x = (int)(50 * Game1.widthRatio);
                    else if (n % 3 == 1)
                        x = (int)(210 * Game1.widthRatio);
                    else
                        x = (int)(370 * Game1.widthRatio);

                    int y = (n / 3) * (int)(50 * Game1.heightRatio) + (int)(160 * Game1.heightRatio);
                    PredefinedGraphs.graphs.Add(newGraph);
                    GraphButtons.Add(new TextBox(content, name, new Vector2(x, y), new Vector2(x + (int)(8 * Game1.widthRatio), y + (int)(15 * Game1.heightRatio)), "graphcreated", n));
                    NewGameButtons.Add(GraphButtons[n]);
                }
            }
        }

        /// <summary>
        /// Funkcja sprawdzajaca nacisk myszy na element okna Menu Glownego
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="content"></param>
        public void MainMenuCheck(Point mousePos, ContentManager content)
        {
            for (int i = 0; i < MainMenuButtons.Count; i++)
            {
                if (MainMenuButtons[i].ContainsPoint(mousePos))
                {
                    switch (MainMenuButtons[i].name)
                    {
                        case "nowa-gra":
                            InterfaceUpdate(content);
                            state = InterfaceState.NewGame;
                            break;
                        case "stworz-graf":
                            state = InterfaceState.GCVertices;
                            break;
                        case "wyjscie":
                            Game1.ExitGame();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Funkcja sprawdzajaca nacisk myszy na element okna pojedynczego loginu
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="game"></param>
        /// <param name="content"></param>
        public void LoginSingleCheck(Point mousePos,ref Game game, ContentManager content)
        {
            p2.login = "Komputer";

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

        /// <summary>
        /// Funkcja sprawdzajaca nacisk myszy na element okna loginu z dwoma graczami
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="game"></param>
        /// <param name="content"></param>
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

        /// <summary>
        /// Funkcja sprawdzajaca wpisywanie loginu w polu tekstowym
        /// </summary>
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

        /// <summary>
        /// Funkcja sprawdzajaca nacisk myszy na element w oknie kreatora nowej gry
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="game"></param>
        /// <param name="content"></param>
        public void NewGameCheck(Point mousePos, ref Game game, ContentManager content)
        {
            for (int i = 0; i < NewGameButtons.Count; i++)
            {
                if (NewGameButtons[i].ContainsPoint(mousePos))
                {
                    switch (NewGameButtons[i].name)
                    {
                        case "start":
                            if (p1 != null && chosenGraph != null)
                                state = p2 is Computer ? InterfaceState.LoginSingle : InterfaceState.LoginMulti;
                            else
                                Game1.MessageBox(new IntPtr(), "Wybierz graf do rozgrywki", "", 0);
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
                        case "graphcreated":
                            ClearButtons(GraphButtons);                               
                            chosenGraph = PredefinedGraphs.graphs[NewGameButtons[i].index].Copy();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "gra-vs-gra":
                            ClearButtons(GameTypeButtons);
                            p1 = new Player("Gracz1");
                            p2 = new Player("Gracz2");
                            UpdateLogins();
                            UpdatePlayers();
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "gra-vs-komp":
                            ClearButtons(GameTypeButtons);
                            p1 = new Player("Gracz1");
                            p2 = new Computer(easyMode);
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
                        case "latwy":
                            ClearButtons(GameDifButtons);
                            easyMode = true;
                            if(p2 is Computer)
                            {
                                Computer c = p2 as Computer;
                                c.easyMode = true;
                            }                           
                            NewGameButtons[i].color = Color.LightBlue;
                            break;
                        case "trudny":
                            ClearButtons(GameDifButtons);
                            easyMode = false;
                            if (p2 is Computer)
                            {
                                Computer c = p2 as Computer;
                                c.easyMode = false;
                            }
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

        /// <summary>
        /// Funkcja sprawdzajaca wpisywanie liczby kolorow w oknie kreatora nowej gry
        /// </summary>
        public void NewGameKeyCheck()
        {
            KeyboardState keybState = Keyboard.GetState();
            Keys[] k = keybState.GetPressedKeys();
            if (k.Length > 0)
            {
                string nrS = k[0].ToString();
                if (nrS.Length == 2)
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




        ///Ponizej funkcje rysujace

        public void MainMenuDraw(SpriteBatch sBatch)
        {
            Rectangle destRect = new Rectangle((int)titleVector.X, (int)titleVector.Y, (int)(titleTexture.Width * Game1.widthRatio), (int)(titleTexture.Height * Game1.heightRatio));
            sBatch.Begin();
            sBatch.Draw(titleTexture, destRect, Color.White);
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
