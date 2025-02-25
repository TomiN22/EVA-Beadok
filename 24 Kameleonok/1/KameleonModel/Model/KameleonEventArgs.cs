using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KameleonModel.Model
{
    public enum Win
    {
        Player1,
        Player2
    }
    public class KameleonEventArgs : EventArgs
    {
        
        private Win _winner;
        private bool _hasWon;
        //private int _argTime;

        public Win Winner => _winner;
        public bool HasWon => _hasWon;

        //public int ArgTime { get { return _argTime; } }
        public KameleonEventArgs()
        {
            //_hasWon = hasWon;
            //_argTime = argTime;
        }

        public KameleonEventArgs(Win winner)
        {
            _winner = winner;
            //_hasWon = hasWon;
            //_argTime = argTime;
        }
    }
}
