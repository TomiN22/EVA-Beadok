using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LopakodoModel.Model
{
    public enum Win
    {
        Player,
        Guard
    }
    public class LopakodoEventArgs : EventArgs
    {
        private Win _winner;
        private bool _hasWon;
        //private int _argTime;

        public Win Winner => _winner;
        public bool HasWon => _hasWon;

        //public int ArgTime { get { return _argTime; } }
        public LopakodoEventArgs()
        {
            //_hasWon = hasWon;
            //_argTime = argTime;
        }

        public LopakodoEventArgs(Win winner)
        {
            _winner = winner;
            //_hasWon = hasWon;
            //_argTime = argTime;
        }
    }
}
