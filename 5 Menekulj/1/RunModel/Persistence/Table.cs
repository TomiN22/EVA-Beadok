using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunModel.Persistence
{
    public class Table
    {
        private int _size;
        private int[,] _tableValue;
        private int[,] _tableWasWall;
        private int[] _rCurrent;
        private int[] _pCurrent;
        private int _timer;
        

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

        public int[] RCurrent { get { return _rCurrent; } set { _rCurrent = value; } }

        public int[] PCurrent { get { return _pCurrent; } set { _pCurrent = value; } }
        public int[] E1Current { get; set; }
        public int[] E2Current { get; set; }

        public int GetTableValue(int x, int y)
        {
            return _tableValue[x, y];
        }

        public void SetTableValue(int x, int y, int value)
        {
            _tableValue[x, y] = value;
        }

        public int WasWall(int x, int y)
        {
            return _tableWasWall[x, y];
        }

        public void SetWasWall(int x, int y, int value)
        {
            _tableWasWall[x, y] = value;
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
                    _tableWasWall[i,j] = 0;

                }
            }

            E1Current = new int[2];
            E2Current = new int[2];

            E1Current[0] = -1; E1Current[1] = -1;
            E2Current[0] = -1; E2Current[1] = -1;
            _pCurrent = new int[2];
            PCurrent[0] = 0; PCurrent[1] = 0;
            _rCurrent = new int[2];
            _timer = 0;
        }


    }
}