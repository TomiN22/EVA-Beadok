using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeModel.Persistence
{
    public class Table
    {
        public enum Directions
        {
            Up, Down, Left, Right
        }
        private int _size;
        private int[,] _tableValue;
        private int[,] _tableWasWall;
        private int[] _hCurrent;
        private int[] _pCurrent;
        private int _timer;

        public Directions Direction { get; set; }
        public int GetSize
        {
            get { return _size; }
            set { _size = value; }
        }

        public int GameTime
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public int[] HCurrent { get { return _hCurrent; } set { _hCurrent = value; } }

        public int EggsEaten { get; set; }

        public bool IsEaten { get; set; }

        public int GetTableValue(int x, int y)
        {
            return _tableValue[x, y];
        }

        public void SetTableValue(int x, int y, int value)
        {
            _tableValue[x, y] = value;
        }

        public Table(int size)
        {
            _size = size;

            _tableValue = new int[size, size];
            _tableWasWall = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j<size; j++)
                {
                    _tableValue[i, j] = 0;
                }
            }
            Direction = Directions.Up;
            EggsEaten = 0;
            IsEaten = true;

            _pCurrent = new int[2];
            _hCurrent = new int[2];
            _timer = 0;
        }


    }
}