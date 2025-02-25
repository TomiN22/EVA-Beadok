using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AknamezoModel.Persistence
{
    public class Table
    {
        private int _size;
        private int[,] _tableValue;
        private int[] _pCurrent;
        private int _timer;
        private int _interval = 1000;
        private int _currentGas;
        private int _gasLimit;


        public int GasLimit
        {
            get { return _gasLimit; }
            set { _gasLimit = value; }
        }

        public int CurrentGas
        {
            get { return _currentGas; }
            set { _currentGas = value; }
        }
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

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

        public int StdTime { get; set; }

        public int FastTime { get; set; }

        public int[] PCurrent { get { return _pCurrent; } set { _pCurrent = value; } }

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
            CurrentGas = 20;
            GasLimit = 1;

            _tableValue = new int[size, size];
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j<size; j++)
                {
                    _tableValue[i, j] = 0;
                }
            }

            
            
            _pCurrent= new int[2];
            _timer = 0;
        }


    }
}