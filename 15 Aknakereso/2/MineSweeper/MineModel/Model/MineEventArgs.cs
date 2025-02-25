using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum Win
    {
        Player1,
        Player2,
        Draw,
        Yet
    }
    public class MineEventArgs : EventArgs
    {

        private Win _winner;
        private bool _isWon;
        private int _currentPlayer;

        public bool IsWon => _isWon;
        public Win Winner => _winner;

        public int CurrenPlayer => _currentPlayer;

        public MineEventArgs(bool isWon, Win winner, int currentPlayer)
        {
            _winner = winner;
            _isWon = isWon;
            _currentPlayer = currentPlayer;
        }

    }
}
