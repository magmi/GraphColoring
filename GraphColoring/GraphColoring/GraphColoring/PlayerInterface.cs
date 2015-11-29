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

        public Texture2D titleTexture;
        public Vector2 titleVector;
        public GardenGraph chosenGraph;
        public PlayerInterface(ContentManager content)
        {
            state = InterfaceState.MainMenu;
            MainMenuButtons = new List<Button>();
            MainMenuButtons.Add(new Button(new Vector2(470, 300), content, "nowa-gra"));
            NewGameButtons = new List<Button>(){
                new Button(new Vector2(50, 50), content, "graf1"),
                new Button(new Vector2(250, 50), content, "graf2"),
                new Button(new Vector2(350, 730), content, "anuluj"),
                new Button(new Vector2(650, 730), content, "start"),
            };

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

        public void NewGameCheck(Point mousePos)
        {
            for (int i = 0; i < NewGameButtons.Count; i++)
            {
                if (NewGameButtons[i].ContainsPoint(mousePos))
                {
                    switch (NewGameButtons[i].name)
                    {
                        case "start":
                            state = InterfaceState.Game;
                            break;
                    }
                }
            }
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
        }

    }
}
