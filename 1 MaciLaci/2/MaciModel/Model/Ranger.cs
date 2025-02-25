using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaciModel.Model
{
    public class Ranger
    {
        public enum Directions
        {
            Up, Down, Left, Right
        }

        #region Fields
        private Random r = new Random();
        #endregion

        #region Properties
        public event EventHandler<RangerStepEventArgs>? RangerStepEvent;

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public int RtoX {  get; private set; }
        public int RtoY { get; private set; }

        public Directions Direction { get; private set; }
        #endregion

        #region Constructors
        public Ranger(int x, int y)
        {
            PositionX = x;
            PositionY = y;
            Direction = (Directions)r.Next(4);
        }
        #endregion

        #region Public methods
        public void WouldStep()
        {
            int toY;
            int toX;
            switch (Direction)
            {
                case Directions.Up:
                    toX = PositionX;
                    toY = PositionY + 1;
                    break;
                case Directions.Down:
                    toX = PositionX;
                    toY = PositionY - 1;
                    break;
                case Directions.Left:
                    toX = PositionX - 1;
                    toY = PositionY;
                    break;
                case Directions.Right:
                    toX = PositionX + 1;
                    toY = PositionY;
                    break;
                default:
                    throw new ApplicationException();
            }
            RangerStepEvent?.Invoke(this, new RangerStepEventArgs(toX, toY));
        }

        public void WouldStepNoEvent()
        {
            int toY;
            int toX;
            switch (Direction)
            {
                case Directions.Up:
                    RtoX = toX = PositionX;
                    RtoY = toY = PositionY + 1;
                    break;
                case Directions.Down:
                    RtoX = toX = PositionX;
                    RtoY = toY = PositionY - 1;
                    break;
                case Directions.Left:
                    RtoX = toX = PositionX - 1;
                    RtoY = toY = PositionY;
                    break;
                case Directions.Right:
                    RtoX = toX = PositionX + 1;
                    RtoY = toY = PositionY;
                    break;
                default:
                    throw new ApplicationException();
            }
        }

        public void ChangeDirection()
        {
            if(Direction == Directions.Up)
                Direction = Directions.Down;
            else if(Direction == Directions.Down)
                Direction = Directions.Up;
            else if (Direction == Directions.Right)
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
