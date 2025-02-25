using System;

namespace Bombazo.Model
{
    public class Bomb
    {
        #region Properties
        public int Time { get; private set; }
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        #endregion

        #region Event handlers
        public event EventHandler<EventArgs>? BombDetonateEvent;
        #endregion

        #region Constructors
        public Bomb(int positionX, int positionY) : this(positionX,positionY, 3) { }

        public Bomb(int positionX, int positionY, int time)
        {
            PositionX = positionX;
            PositionY = positionY;
            Time = time;
        }
        #endregion

        #region Public methods
        public void TimeTick()
        {
            --Time;
            if (Time == 0)
            {
                BombDetonateEvent?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
