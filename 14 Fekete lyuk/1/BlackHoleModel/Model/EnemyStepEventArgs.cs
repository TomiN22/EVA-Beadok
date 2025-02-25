using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHoleModel.Model
{
    public class EnemyStepEventArgs
    {
        private int x; //TODO private
        private int y;

        public int X { get; private set; }
        public int Y { get; private set; }

        public EnemyStepEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
