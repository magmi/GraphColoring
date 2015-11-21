﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace GraphColoring
{
    class Game
    {
        public GameType gameType;
        public GardenGraph graph;
        public Color[] colors;

        public Game(GameType gT, GardenGraph g, int c)
        {
            gameType = gT;
            graph = g;
            colors = ColorsCreator.GetColors(c);
        }

        public bool CheckIfEnd(out bool didGardenerWon)
        {
            didGardenerWon = false;

            if ( ( (this.gameType == GameType.VerticesColoring) && (this.graph.coloredFlowersNumber == this.graph.flowersNumber) )
                || ( (this.gameType == GameType.EdgesColoring) && (this.graph.coloredFencesNumber == this.graph.fencesNumber) ) )
            {
                didGardenerWon = true;
                return true;
            }

            if (!this.graph.IsColoringPossible(this.gameType, this.colors))
                return true;

            return false;
        }

        public bool CheckIfMouseClickedOnFlower(Point mousePos, out int index)
        {
            index = 0;
            for(int i=0;i<graph.flowers.Count;i++)                
            {
                Flower f = graph.flowers[i];
                if(f.ContainsPoint(mousePos))
                {
                    index = i;
                    return true;
                }
            }
            return false;
        }

        public void StartGame()
        {

        }

    }
}
