using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunModel.Model
{
    public class Enemy
    {
        public enum Directions
        {
            Up, Down, Left, Right
        }

        #region Fields
        private Random r = new Random();
        #endregion

        #region Properties
        public event EventHandler<EnemyStepEventArgs>? EnemyStepEvent;

        public int Id { get; set; }
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public int RtoX {  get; private set; }
        public int RtoY { get; private set; }

        public Directions Direction { get; set; }
        #endregion

        #region Constructors
        public Enemy(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }
        #endregion

        #region Public methods
        public void WouldStep()
        {
            
            EnemyStepEvent?.Invoke(this, new EnemyStepEventArgs(PositionX, PositionY));
        }

        public void ChangeDirection()
        {
            Direction = (Directions)r.Next(4);
        }

        public void WouldStepNoEvent()
        {
            int toY;
            int toX;
            switch (Direction)
            {
                case Directions.Up:
                    RtoX = toX = PositionX;
                    RtoY = toY = PositionY - 1;
                    break;
                case Directions.Down:
                    RtoX = toX = PositionX;
                    RtoY = toY = PositionY + 1;
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

        

        public void StepTo(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }
        #endregion
    }
}
