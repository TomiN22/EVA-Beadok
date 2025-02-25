using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidsModel.Model
{
    public class Meteor
    {
        public enum Directions
        {
            Up, Down, Left, Right
        }

        #region Fields
        private Random r = new Random();
        #endregion

        #region Properties
        public event EventHandler<MeteorStepEventArgs>? MeteorStepEvent;

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public int RtoX {  get; private set; }
        public int RtoY { get; private set; }

        public Directions Direction { get; private set; }
        #endregion

        #region Constructors
        public Meteor(int x, int y)
        {
            PositionX = x;
            PositionY = y;
            
        }
        #endregion

        #region Public methods
        public void Step()
        {
            int toY;
            int toX = PositionX;

            toY = PositionY + 1;
                    
            MeteorStepEvent?.Invoke(this, new MeteorStepEventArgs(toX, toY));
        }

        public void StepTo(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }
        #endregion
    }
}
