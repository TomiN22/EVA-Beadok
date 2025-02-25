using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombazoModel.Model
{
    public class EnemyStepEventArgs
    {
        public int x; //TODO private
        public int y;

        public EnemyStepEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
