﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunModel.Model
{
    public class RunEventArgs : EventArgs
    {
        private bool _hasWon;
        //private int _argTime;

        public bool HasWon => _hasWon;

        //public int ArgTime { get { return _argTime; } }

        public RunEventArgs(bool hasWon)
        {
            _hasWon = hasWon;
            //_argTime = argTime;
        }
    }
}
