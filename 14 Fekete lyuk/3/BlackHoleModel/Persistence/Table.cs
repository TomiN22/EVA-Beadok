using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHoleModel.Persistence
{
    public class Table
    {
        public enum Players
        {
            Player1, Player2
        }

        private int _size;
        private int[,] _tableValue;
        //private int[,] _tableWasWall;
        private int[] _p1Current;
        private int[] _p2Current;
        private int _timer;
        private int _player;



        public int GetSize
        {
            get { return _size; }
            set { _size = value; }
        }

        public int Player
        {
            get { return _player; }

            set { _player = value; }
        }

        public int GameTime
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public int P1ShipsInHole { get; set; }
        public int P2ShipsInHole { get; set; }

        public int[] P1Current { get { return _p1Current; } set { _p1Current = value; } }
        public int[] P2Current { get { return _p2Current; } set { _p2Current = value; } }

        public int GetTableValue(int x, int y)
        {
            return _tableValue[x, y];
        }

        public void SetTableValue(int x, int y, int value)
        {
            _tableValue[x, y] = value;
        }

        //public void SetWasWall(int x, int y, int value)
        //{
        //    _tableWasWall[x, y] = value;
        //}

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
            P1ShipsInHole = 0; P2ShipsInHole = 0;
            _p1Current= new int[2]; _p2Current= new int[2];
            P1Current[0]= -1; P1Current[1]= -1;
            P2Current[0]= -1; P2Current[1]= -1;
            Player = 1;
            _timer = 0;
        }

        public void ChangePlayer()
        {
            _player = _player == 1 ? _player = 2 : _player = 1;
        }
    }
}