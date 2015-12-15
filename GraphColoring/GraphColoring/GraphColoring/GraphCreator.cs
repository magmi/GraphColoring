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
    class GraphCreator
    {
        public TextBox VerticesTextBox;
        public List<Button> VerticesButtons;
        public List<Button> GCButtons;
        public StringBuilder VerticesNrBuilder;
        public int VerticesNr;
        private GardenGraph graph;
        private Flower lastClicked;
        public GraphCreator(ContentManager content)
        {
            VerticesNrBuilder = new StringBuilder("3");
            VerticesTextBox = new TextBox(content, "3", new Vector2(340, 200), new Vector2(450, 250),"liczba-kwiatkow");
            VerticesButtons = new List<Button>(){
                new Button(new Vector2(350, 730), content, "anuluj"),
                new Button(new Vector2(650, 730), content, "start")};
            GCButtons = new List<Button>(){
                new Button(new Vector2(0, 730), content, "anuluj"),
                new Button(new Vector2(0, 660), content, "zapisz-graf"),
            };

        }

        public int GetIndex(List<Button> list, Point p)
        {
            for (int i = 0; i < list.Count;i++ )
            {
                if (list[i].ContainsPoint(p))
                    return i;
            }
            return -1;
        }

        public void IntefaceUpdate(PlayerInterface pi,ContentManager content)
        {
            int n = pi.GraphButtons.Count;

            int x = n%2 ==0 ? 50 : 210;
            int y = (n/2)*160 + 50;
            pi.GraphButtons.Add(new TextBox(content,(n-2).ToString(),new Vector2(x, y),new Vector2(x+20,y+20), "graf", n));
            pi.NewGameButtons.Add(pi.GraphButtons[n]);
        }


        public void CheckGraphCreator(Point mousePos, PlayerInterface pi, ContentManager content)
        {
            int index = GetIndex(GCButtons, mousePos);
            if (index > -1)
            {
                switch (GCButtons[index].name)
                {
                    case "anuluj":
                        pi.state = InterfaceState.MainMenu;
                        break;
                    case "zapisz-graf":
                        PredefinedGraphs.graphs.Add(graph);
                        IntefaceUpdate(pi, content);
                        pi.state = InterfaceState.MainMenu;
                        break;
                }
            }

            for(int i =0;i<VerticesNr;i++)
            {
                if(graph.flowers[i].ContainsPoint(mousePos))
                {
                    if(lastClicked==null)
                    {
                        lastClicked = graph.flowers[i];
                        lastClicked.color = Color.LightBlue;
                    }
                    else
                    {
                        if (graph.flowers[i] != lastClicked)
                        {
                            Fence f = new Fence(lastClicked, graph.flowers[i], content);

                            if (!lastClicked.outFences.Exists(x => (x.f1.Equals(lastClicked) && x.f2.Equals(graph.flowers[i])) || (x.f1.Equals(graph.flowers[i]) && x.f2.Equals(lastClicked))))
                            {
                                lastClicked.outFences.Add(f);
                                graph.flowers[i].outFences.Add(f);
                                graph.fences.Add(f);
                                graph.fencesNumber += 1;
                            }
                        }
                        lastClicked.color = Color.White;
                        lastClicked = null;
                        
                    }
                }

            }

        }

        public void CheckVerticesAsking(Point mousePos, PlayerInterface pi, ContentManager content)
        {
            int i = GetIndex(VerticesButtons, mousePos);
            if(i>-1)
            {
                switch(VerticesButtons[i].name)
                {
                    case "start":
                        VerticesNr = int.Parse(VerticesNrBuilder.ToString());
                        if (VerticesNr > 2)
                        {
                            graph = PredefinedGraphs.CreateEmptyGraph(VerticesNr, content);
                            pi.state = InterfaceState.GraphCreation;
                        }
                        break;
                    case "anuluj":
                        pi.state = InterfaceState.MainMenu;
                        break;
                }
            }
            
            
        }

       public void CheckKeyVerticesAsking()
        {
            KeyboardState keybState = Keyboard.GetState();
            Keys[] k = keybState.GetPressedKeys();
            if (k.Length > 0)
            {
                if (k[0] == Keys.Back)
                {
                    if (VerticesNrBuilder.Length > 0)
                        VerticesNrBuilder.Remove(VerticesNrBuilder.Length - 1, 1);
                    VerticesTextBox.text = VerticesNrBuilder.ToString();
                    return;
                }
                string nrS = k[0].ToString();
                if (nrS.Length > 1)
                {
                    int nr = int.Parse(nrS[1].ToString());
                    if (nr >= 0 && nr < 10)
                    {
                        VerticesNrBuilder.Append(nr.ToString());
                        VerticesTextBox.text = VerticesNrBuilder.ToString();
                    }
                }
            }
        }

        public void DrawGraphCreator(SpriteBatch sb)
        {
            graph.DrawAllElements(sb);
            foreach (Button b in GCButtons)
                b.Draw(sb);
        }

        public void DrawVericesAsking(SpriteBatch sb)
        {
            VerticesTextBox.Draw(sb);
            foreach (Button b in VerticesButtons)
                b.Draw(sb);
        }

    }
}
