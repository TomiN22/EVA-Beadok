using FenymotorModel.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenymotorModel.Model
{
    public class Player
    {
        public enum Directions
        {
            Up, Down, Left, Right
        }

        #region Fields
        private Random r = new Random();
        #endregion

        #region Properties
        public event EventHandler<PlayerStepEventArgs>? PlayerStepEvent;

        public int Id {  get; set; }

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public int RtoX {  get; private set; }
        public int RtoY { get; private set; }

        public Directions Direction { get; set; }
        #endregion

        #region Constructors
        public Player(int x, int y, Directions dir, int id)
        {
            PositionX = x;
            PositionY = y;
            Direction = dir;
            Id = id;
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
                    RtoX = toX = PositionX - 1;
                    RtoY = toY = PositionY;
                    break;
                case Directions.Down:
                    RtoX = toX = PositionX + 1;
                    RtoY = toY = PositionY;
                    break;
                case Directions.Left:
                    RtoX = toX = PositionX;
                    RtoY = toY = PositionY - 1;
                    break;
                case Directions.Right:
                    RtoX = toX = PositionX;
                    RtoY = toY = PositionY + 1;
                    break;
                default:
                    throw new ApplicationException();
            }
            PlayerStepEvent?.Invoke(this, new PlayerStepEventArgs(toX, toY));
        }

        public void ChangeDirection(string dir)
        {
            switch (Direction)
            {
                case Directions.Up:
                    if (dir == "right")
                        Direction = Directions.Right;
                    else if (dir == "left")
                        Direction = Directions.Left;
                    break;
                case Directions.Down:
                    if (dir == "right")
                        Direction = Directions.Left;
                    else if (dir == "left")
                        Direction = Directions.Right;
                    break;
                case Directions.Left:
                    if (dir == "right")
                        Direction = Directions.Up;
                    else if (dir == "left")
                        Direction = Directions.Down;
                    break;
                case Directions.Right:
                    if (dir == "right")
                        Direction = Directions.Down;
                    else if (dir == "left")
                        Direction = Directions.Up;
                    break;
                default:
                    throw new ApplicationException();
            }
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
