using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenymotorModel.Model
{
    public class PlayerStepEventArgs
    {
        public int x; //TODO private
        public int y;

        public PlayerStepEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
