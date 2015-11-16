using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace GraphColoring
{
    class Game
    {
        GameType gameType;
        GardenGraph graph;
        Color[] colors;

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

    }
}
