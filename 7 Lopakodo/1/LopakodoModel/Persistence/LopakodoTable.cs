using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LopakodoModel.Persistence
{
    public class LopakodoTable
    {
        private int _size;
        private int[,] _tableValue;
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

        public int[] PCurrent { get { return _pCurrent; } set { _pCurrent = value; } }

        public int GetTableValue(int x, int y)
        {
            return _tableValue[x, y];
        }

        public void SetTableValue(int x, int y, int value)
        {
            _tableValue[x, y] = value;
        }

        public LopakodoTable(int size)
        {
            _size = size;
            _tableValue = new int[size, size];
            
            _pCurrent= new int[2] { GetSize-1, 0 };
            _timer = 0;
        }


    }
}