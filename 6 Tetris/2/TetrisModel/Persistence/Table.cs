using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisModel.Persistence
{
    public class Table
    {
        private int _sizeX;
        private int _sizeY;
        private int[,] _tableValue;
        private bool[,] _tableOccupied;
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

        public int GetSizeX
        {
            get { return _sizeX; }
            set { _sizeX = value; }
        }

        public int GetSizeY
        {
            get { return _sizeY; }
            set { _sizeY = value; }
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

        public bool GetOccupied(int x, int y)
        {
            return _tableOccupied[x, y];
        }

        public void SetOccupied(int x, int y, bool value)
        {
            _tableOccupied[x,y] = value;
        }

        public Table(int sizeX, int sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            CurrentGas = 20;
            GasLimit = 1;

            _tableValue = new int[sizeX, sizeY];
            _tableOccupied = new bool[sizeX, sizeY];


            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j<sizeY; j++)
                {
                    _tableValue[i, j] = 0;
                    _tableOccupied[i, j] = false;
                }
            }

            
            
            _pCurrent= new int[2];
            _timer = 0;
        }


    }
}