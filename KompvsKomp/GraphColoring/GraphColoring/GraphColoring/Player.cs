using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphColoring
{
    public class Player
    {
        public string login;
        public int points;
        public bool isGardener;
        public Player(string log=null)
        {            
            login = log;
        }
    }
}
