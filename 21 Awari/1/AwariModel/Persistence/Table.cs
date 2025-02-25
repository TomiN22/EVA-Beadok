using AwariModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwariModel.Persistence
{
    public class Table
    {
        public enum Directions
        {
            Up, Down, Left, Right
        }
        private int _size;
        private int[,] _tableValue;
        private int[,] _tableColor;
        private int[,] _tableNum;
        private int _timer;
        private int[] _p1Current;
        private int[] _p2Current;

        public Directions Direction { get; set; }
        public int GetSize
        {
            get { return _size; }
            //set { _size = value; }
        }

        public int GameTime
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public int Player { get; set;}

        public int[] P1Current { get { return _p1Current; } set { _p1Current = value; } }
        public int[] P2Current { get { return _p2Current; } set { _p2Current = value; } }

        public int FigureId { get; set;}

        public int P1Time { get; set; }
        public int P2Time { get; set; }

        public double P1Count { get; set; }
        public double P2Count { get; set; }


        public int GetTableValue(int x, int y)
        {
            return _tableValue[x, y];
        }

        public void SetTableValue(int x, int y, int value)
        {
            _tableValue[x, y] = value;
        }

        public int GetTableColor(int x, int y)
        {
            return _tableColor[x, y];
        }

        public void SetTableColor(int x, int y, int value)
        {
            _tableColor[x, y] = value;
        }

        public int GetTableNum(int x, int y)
        {
            return _tableNum[x, y];
        }

        public void SetTableNum(int x, int y, int value)
        {
            _tableNum[x, y] = value;
        }

        public Table(int size)
        {
            _size = size;

            _tableValue = new int[2, size];
            _tableColor = new int[2, size];
            _tableNum = new int[2, size];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j<size; j++)
                {
                    _tableValue[i, j] = 0;
                    _tableColor[i, j] = 0;
                    _tableNum[i, j] = 0;
                }
            }
            Player=1;
            _p1Current= new int[2]; _p2Current= new int[2];
            P1Current[0]= -1; P1Current[1]= -1;
            P2Current[0]= -1; P2Current[1]= -1;
            P1Count = 0; P2Count = 0;

            P1Time = 0;
            P2Time = 0;
            _timer = 0;
        }

        public void ChangePlayer()
        {
            Player = Player == 1 ? Player = 2 : Player = 1;
        }
    }
}