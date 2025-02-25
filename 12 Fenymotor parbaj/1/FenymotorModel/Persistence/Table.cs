using FenymotorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenymotorModel.Persistence
{
    public class Table
    {
        public enum Directions
        {
            Up, Down, Left, Right
        }
        private int _size;
        private int[,] _tableValue;
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

        public Directions P1Direction { get; set; }
        public Directions P2Direction { get; set; }


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
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j<size; j++)
                {
                    _tableValue[i, j] = 0;
                }
            }
            P1Direction = Directions.Right;
            P2Direction = Directions.Left;
            _timer = 0;
        }


    }
}