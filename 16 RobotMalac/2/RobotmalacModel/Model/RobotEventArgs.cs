using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotmalacModel.Model
{
    public enum Win
    {
        Player1,
        Player2,
    }
    public class RobotEventArgs : EventArgs
    {
        private Win _winner;
        private bool _isWon;
        private int _currentPlayer;

        public bool IsWon => _isWon;
        public Win Winner { get { return _winner; } set { _winner = value; } }

        public int CurrenPlayer => _currentPlayer;

        public RobotEventArgs(bool isWon, Win winner, int currentPlayer)
        {
            _winner = winner;
            _isWon = isWon;
            _currentPlayer = currentPlayer;
        }
    }
}
