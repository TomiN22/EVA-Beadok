using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisModel.Model
{
    public class ShipStepEventArgs
    {
        public int x; //TODO private
        public int y;

        public ShipStepEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
