using KameleonModel.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KameleonModel.Model
{
    public class Figure
    {
        #region Fields
        private Random r = new Random();
        #endregion

        #region Properties
        public event EventHandler<FigureStepEventArgs>? PlayerStepEvent;

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public int RtoX {  get; private set; }
        public int RtoY { get; private set; }

        public int FigureId { get; set; }
        public int Player { get; private set; }

        #endregion

        #region Constructors
        public Figure(int x, int y, int player)
        {
            PositionX = x;
            PositionY = y;
            Player = player;
        }
        #endregion

        #region Public methods
        //public void WouldStep()
        //{
        //    int toY;
        //    int toX;
            
        //    PlayerStepEvent?.Invoke(this, new FigureStepEventArgs(toX, toY));
        //}

        public void StepTo(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }
        #endregion
    }
}
