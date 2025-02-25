using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AknamezoModel.Model
{
    public class Ship
    {
        public enum Directions
        {
            Left, Right
        }

        #region Fields
        private Random r = new Random();
        #endregion

        #region Properties
        public event EventHandler<ShipStepEventArgs>? ShipStepEvent;

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public int RtoX { get; private set; }
        public int RtoY { get; private set; }

        public Directions Direction { get; set; }
        #endregion

        #region Constructors
        public Ship(int x, int y)
        {
            PositionX = x;
            PositionY = y;
            Direction = (Directions)r.Next(2);
        }
        #endregion

        #region Public methods
        public void Step()
        {
            int toY;
            int toX;

            switch (Direction)
            {
                case Directions.Left:
                    toX = PositionX;
                    toY = PositionY-1;
                    break;
                case Directions.Right:
                    toX = PositionX;
                    toY = PositionY+1;
                    break;
                default:
                    throw new ApplicationException();
            }

            ShipStepEvent?.Invoke(this, new ShipStepEventArgs(toX, toY));
        }

        public void ChangeDirection()
        {
            if (Direction == Directions.Right)
                Direction = Directions.Left;
            else if (Direction == Directions.Left)
                Direction = Directions.Right;
        }

        public void StepTo(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }
        #endregion
    }
}
