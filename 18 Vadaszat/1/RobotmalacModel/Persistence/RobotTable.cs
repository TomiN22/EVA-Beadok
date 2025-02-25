using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotmalacModel.Persistence
{
    public class RobotTable
    {
        private int _size;
        private int _player;
        private int[,] _tableValue;
        private int[] _p1Current;
        private int[] _p2Current;
        private int _p1Health = 3;
        private int _p2Health = 3;
        private string _p1Direction = "down";
        private string _p2Direction = "up";
        private int[] _prey;
        private int _moves;

        public int Moves
        {
            get { return _moves; }
            set { _moves = value; }
        }
        public int GetPlayer
        {
            get { return _player; }

            set { _player = value; }
        }

        public int GetSize
        {
            get { return _size; }
            set { _size = value; }
        }


        public int[] Prey { get { return _prey; } set { _prey = value; } }
        public int[] P1Current { get { return _p1Current; } set { _p1Current = value; } }
        public int[] P2Current { get { return _p2Current; } set { _p2Current = value; } }

        public int P1Health { get { return _p1Health; } set { _p1Health = value; } }
        public int P2Health { get { return _p2Health; } set { _p2Health = value; } }

        public string P1Direction { get { return _p1Direction; } set { _p1Direction = value; } }
        public string P2Direction { get { return _p2Direction; } set { _p2Direction = value; } }

        public int GetTableValue(int x, int y)
        {
            return _tableValue[x, y]; 
        }

        public void SetTableValue(int x, int y, int value)
        {
            _tableValue[x, y] = value;
        }

        public RobotTable(int size)
        {
            _size = size;
            if(size == 3)
            {
                Moves=12;
            }
            else if(size == 5)
            {
                Moves=20;
            }
            else if(size == 7)
            {
                Moves=28;
            }
            _tableValue = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j<size; j++)
                {
                    _tableValue[i, j] = 0;
                    if((i==0 && j==0) || (i==0 && j==size-1) || (i==size-1 && j==0) || (i==size-1 && j==size-1))
                    {
                        _tableValue[i, j] = 2;
                    }
                    else if(i==size/2 && j==size/2)
                    {
                        _tableValue[i, j] = 1;
                    }
                }
            }
            _player = 2;
            P1Health = 3;
            P2Health = 3;
            _prey = new int[2] { _size/2, _size/2 };
            _p1Current= new int[2] { (_size/2)-1, 0 };
            _p2Current= new int[2] { _size/2, _size-1 };
        }

        

        public void ChangePlayer()
        {
            _player = _player == 1 ? _player = 2 : _player = 1;
        }
    }
}

//private readonly Table[,]? _table;
//public struct Table
//{
//    public int value;
//    //public bool isOpened;
//}
//public Table[,]? GetTable { get { return _table; } }

//public int this[int x, int y] { get { return GetValue(x, y); } }