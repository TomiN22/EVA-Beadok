using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KameleonModel.Model
{
    public class FigureStepEventArgs
    {
        public int x; //TODO private
        public int y;

        public FigureStepEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
