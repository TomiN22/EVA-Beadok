using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AknamezoModel.Model
{
    public class MeteorStepEventArgs
    {
        public int x; //TODO private
        public int y;

        public MeteorStepEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
