﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegyzetekModel.Model
{
    public enum Win
    {
        Player1,
        Player2,
        Draw
    }
    public class NegyzetekEventArgs : EventArgs
    {
        
        private Win _winner;
        private bool _hasWon;
        //private int _argTime;

        public Win Winner => _winner;
        public bool HasWon => _hasWon;

        //public int ArgTime { get { return _argTime; } }
        public NegyzetekEventArgs()
        {
            //_hasWon = hasWon;
            //_argTime = argTime;
        }

        public NegyzetekEventArgs(Win winner)
        {
            _winner = winner;
            //_hasWon = hasWon;
            //_argTime = argTime;
        }
    }
}
