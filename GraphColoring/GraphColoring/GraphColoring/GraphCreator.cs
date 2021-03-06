﻿using System;
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
        private ColorableObject lastClicked;
        private Texture2D fenceTexture;
        public Vector2 previousMousePosition = Vector2.Zero;
        public bool movingFlower = false;

        public GraphCreator(ContentManager content)
        {
            int gameHeight = Game1.GetHeight();
            int gameWidth = Game1.GetWidth();

            VerticesNrBuilder = new StringBuilder("3");
            VerticesTextBox = new TextBox(content, "3", Game1.GetRatioDimensions(new Vector2(340, 200)), Game1.GetRatioDimensions(new Vector2(450, 250)), "liczba-kwiatkow", 0, "CzcionkaUI");
            VerticesButtons = new List<Button>(){
                new Button(Game1.GetRatioDimensions(new Vector2(350, 730)), content, "anuluj"),
                new Button(Game1.GetRatioDimensions(new Vector2(650, 730)), content, "start")};
            GCButtons = new List<Button>(){
                new Button(Game1.GetRatioDimensions(new Vector2(0, 730)), content, "anuluj"),
                new Button(Game1.GetRatioDimensions(new Vector2(0, 660)), content, "zapisz-graf"),
                new Button(Game1.GetRatioDimensions(new Vector2(0, 590)), content, "usun", Color.Gray),
                new Button(Game1.GetRatioDimensions(new Vector2(0, 520)), content, "dodaj-kwiatek")
            };
            
            fenceTexture = content.Load<Texture2D>("Plotek");
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
        /// <summary>
        /// Funkcja sprawdzajaca nacisk myszy na element okna Kreatora grafu
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="pi"></param>
        /// <param name="content"></param>
        public void CheckGraphCreator(Point mousePos, PlayerInterface pi, ContentManager content)
        {
            int index = GetIndex(GCButtons, mousePos);
            List<Fence> outFences;
            if (index > -1)
            {
                switch (GCButtons[index].name)
                {
                    case "anuluj":
                        pi.state = InterfaceState.MainMenu;
                        break;
                    case "zapisz-graf":
                        DateTime now = DateTime.Now;
                        PrepareGraphForSerialization();
                        SerializationManager.SerializeObject(graph, SerializationManager.CreateFileName(now));
                        pi.state = InterfaceState.MainMenu;
                        break;
                    case "usun":
                        if(GCButtons[index].color == Color.White)
                        {
                            if (lastClicked is Fence)
                            {
                                graph.fences.Remove((Fence)lastClicked);
                                graph.fencesNumber--;
                            }
                            else if(lastClicked is Flower)
                            {
                                outFences = graph.GetOutFences((Flower)lastClicked);
                                if (outFences.Count > 0)
                                    Game1.MessageBox(new IntPtr(), "Najpierw usuń wychodzące krawędzie", "", 0);
                                else
                                {
                                    graph.flowers.Remove((Flower)lastClicked);
                                    graph.flowersNumber--;
                                }
                            }

                            GCButtons[index].color = Color.Gray;
                            lastClicked.color = Color.White;
                            lastClicked = null;
                            GCButtons[3].color = Color.White;
                            return;
                        }
                        break;
                    case "dodaj-kwiatek":
                        if (GCButtons[index].color == Color.White)
                        {
                            List<int> indices = new List<int>();
                            foreach(Flower f in graph.flowers)
                            {
                                indices.Add(f.index);
                            }
                            indices.Sort();

                            int previous_index = -1;
                            int flower_index = graph.flowersNumber;
                            foreach(int ind in indices)
                            {
                                if(ind - previous_index > 1)
                                {
                                    flower_index = previous_index + 1;
                                    break;
                                }
                                previous_index = ind;
                            }

                            Flower flower = new Flower(Game1.GetRatioDimensions(new Vector2(0, 390)), "Kwiatek", flower_index);
                            graph.flowers.Add(flower);
                            graph.flowersNumber++;
                            return;
                        }
                        break;
                }
            }

            if (!(lastClicked is Fence))
            {
                for (int i = 0; i < graph.flowersNumber; i++)
                {
                    if (graph.flowers[i].ContainsPoint(mousePos))
                    {
                        if (lastClicked == null)
                        {
                            lastClicked = graph.flowers[i];
                            lastClicked.color = Color.LightBlue;
                            movingFlower = true;
                            GCButtons[2].color = Color.White;
                            GCButtons[3].color = Color.Gray;
                            break;
                        }
                        else if (!movingFlower)
                        {
                            if (graph.flowers[i] != lastClicked)
                            {
                                Fence f = new Fence((Flower)lastClicked, graph.flowers[i], "Plotek");

                                outFences = graph.GetOutFences((Flower)lastClicked);
                                if (!outFences.Exists(x => (x.f1.Equals(lastClicked) && x.f2.Equals(graph.flowers[i])) || (x.f1.Equals(graph.flowers[i]) && x.f2.Equals(lastClicked))))
                                {
                                    graph.fences.Add(f);
                                    graph.fencesNumber += 1;
                                }
                            }
                            lastClicked.color = Color.White;
                            lastClicked = null;
                            GCButtons[2].color = Color.Gray;
                            GCButtons[3].color = Color.White;
                            return;
                        }
                    }
                }
            }

            if (movingFlower)
            {
                if (previousMousePosition != Vector2.Zero)
                {
                    ((Flower)lastClicked).position.X -= previousMousePosition.X - mousePos.X;
                    ((Flower)lastClicked).position.Y -= previousMousePosition.Y - mousePos.Y;

                    ((Flower)lastClicked).center.X -= previousMousePosition.X - mousePos.X;
                    ((Flower)lastClicked).center.Y -= previousMousePosition.Y - mousePos.Y;
                }

                previousMousePosition = new Vector2(mousePos.X, mousePos.Y);
            }
            else
            {
                if (lastClicked == null || lastClicked is Fence)
                {
                    for (int i = 0; i < graph.fencesNumber; i++)
                    {
                        if (graph.fences[i].ContainsPoint(mousePos))
                        {
                            if (lastClicked == null)
                            {
                                lastClicked = graph.fences[i];
                                lastClicked.color = Color.LightBlue;
                                GCButtons[2].color = Color.White;
                                GCButtons[3].color = Color.Gray;
                            }
                            else
                            {
                                if (graph.fences[i] == lastClicked)                               
                                {
                                    lastClicked.color = Color.White;
                                    lastClicked = null;
                                    GCButtons[2].color = Color.Gray;
                                    GCButtons[3].color = Color.White;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

        }

        private void PrepareGraphForSerialization()
        {
            int iterator = 0;
            foreach(Flower f in graph.flowers)
            {
                f.index = iterator++;
            }
        }

        /// <summary>
        /// Funkcja sprawdzajaca nacisk myszy na element okna zapytania o ilosc kwiatkow w grafie
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="pi"></param>
        /// <param name="content"></param>
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

        /// <summary>
        /// Funkcja sprawdzajaca wpisywanie liczby wierzchlkow w oknie zapytania o ilosc kwiatkow w grafie
        /// </summary>
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
