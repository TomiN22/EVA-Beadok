using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaciModel.Model
{
    public class RangerStepEventArgs
    {
        public int x; //TODO private
        public int y;

        public RangerStepEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
