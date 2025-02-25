using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AknamezoModel.Model.Meteor;
using static AknamezoModel.Model.Ship;

namespace AknamezoModel.Model
{
    public class Meteor
    {
        public enum BombSizes
        {
            Small, Medium, Big
        };
        #region Properties
        public event EventHandler<MeteorStepEventArgs>? MeteorStepEvent;

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public int RtoX {  get; private set; }
        public int RtoY { get; private set; }

        public BombSizes BombSize { get; set; }

        #endregion

        #region Constructors
        public Meteor(int x, int y)
        {
            PositionX = x;
            PositionY = y;
            Random r = new Random();
            BombSize = (BombSizes)r.Next(3);
        }

        public Meteor(int x, int y, BombSizes bombSizes)
        {
            PositionX = x;
            PositionY = y;
            BombSize = bombSizes;
        }
        #endregion

        #region Public methods
        public void Step()
        {
            int toX = PositionX;
            int toY;

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
