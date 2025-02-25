using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LooseRobotModel.Persistence
{
    public class Table
    {
        private int _size;
        private int[,] _tableValue;
        private int[,] _tableWasWall;
        private int[] _rCurrent;
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

            
            
            _rCurrent= new int[2];
            _timer = 0;
        }


    }
}