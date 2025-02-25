using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public class MineTable
    {
        private Table[,] _table;
        private int _size;
        private int _player;
        private int _mines;

        public struct Table 
        {
            public int value;
            public bool isOpened;
        }

        public int GetSize { get { return _size; } }

        public int GetMines
        {
            get { return _mines; }
            set { _mines = value; }
        }
        public int GetPlayer 
        { 
            get { return _player; }

            set { _player = value; }
        }

        public Table[,] GetTable { get { return _table; } }

        public int this[int x, int y] { get { return GetValue(x, y); } }
        public MineTable(int size)
        {
            _size = size;
            _player = 1;
            _table = new Table[_size, _size];
            if(size == 6) { _mines = 10; }
            if(size == 10) { _mines = 15; }
            if(size == 16) { _mines = 40; }

            for(int i=0; i<_size; i++)
            {
                for(int j=0; j<_size; j++)
                {
                    _table[i,j].isOpened = false;
                    _table[i,j].value = 0;
                }
            }
            
        }



        public void SetToOpened(int i, int j)
        {
            _table[i, j].isOpened = true;
        }

        public bool IsOpened(int i, int j)
        {
            if (_table[i, j].isOpened) { return true; }
            else
                return false;

        }
        public void SetValue(int x, int y, int value)
        {
            if (x < 0 || x >= _table.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _table.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");
            if (value < -1 || value > _table.GetLength(0) + 1)
                throw new ArgumentOutOfRangeException(nameof(value), "The value is out of range.");

            _table[x, y].value = value;
        }

        public int GetValue(int x, int y)
        {
            if (x >= _size || x < 0) throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y >= _size || y < 0) throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");

            return _table[x, y].value;
        }

        public Boolean IsEmpty(int x, int y)
        {
            if (x < 0 || x >= _size)
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _size)
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _table[x, y].value == 0;
        }

        public void ChangePlayer()
        {
            _player = _player == 1 ? _player = 2 : _player = 1;
        }
 
    }
}
