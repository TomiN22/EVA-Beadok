using RobotmalacModel.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Persistence;

namespace RobotmalacModel.Model
{
    public class RobotModel
    {
        private RobotTable _table;
        private readonly IRobotDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;
        private int[] _p1Would = new int[2];
        private int[] _p2Would = new int[2];
        private string[] _p1StandaloneCMD;
        private string[] _p2StandaloneCMD;
        private bool _p1moveChanged;
        private bool _p2moveChanged;
        private int _length = 0;
        private bool _p1HitP2;
        private bool _p2HitP1;
        private int _winner;

        public event EventHandler<RobotEventArgs>? GameOver;
        public event EventHandler? Player1Hit;
        public event EventHandler? Player2Hit;

        //IRobotDataAccess

        public bool IsGameOver => _isGameOver;
        public int Length { get { return _length; } set { _length = value; } }
        public int[] P1Would { get { return _p1Would; } set { _p1Would = value; } }
        public int[] P2Would { get { return _p2Would; } set { _p2Would = value; } }

        public int Winner
        {
            get { return _winner; }
            set { _winner = value; }
        }


        public string[] P1StandaloneCMD
        {
            get { return _p1StandaloneCMD; }
            set { _p1StandaloneCMD = value; }
        }

        public string[] P2StandaloneCMD
        {
            get { return _p2StandaloneCMD; }
            set { _p2StandaloneCMD = value; }
        }

        public bool P1MoveChanged { get { return _p1moveChanged; } set { _p1moveChanged = value; } }
        public bool P2MoveChanged { get { return _p2moveChanged; } set { _p2moveChanged = value; } }
        public bool P1HitP2 { get { return _p1HitP2; } set { _p1HitP2 = value; if (_p1HitP2) { OnPlayer2Hit(); } } }
        public bool P2HitP1 { get { return _p2HitP1; } set { _p2HitP1 = value; if (_p2HitP1) { OnPlayer1Hit(); } } }

        public int GetSize
        {
            get { return _size; }
        }

        protected virtual void OnPlayer1Hit()
        {
            Player1Hit?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPlayer2Hit()
        {
            Player2Hit?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnGameOver(Win winner)
        {
            GameOver?.Invoke(this, new RobotEventArgs(true, winner, _table.GetPlayer));
        }

        public RobotModel(IRobotDataAccess? dataAccess, int size)
        {
            _size = size;
            _dataAccess = dataAccess;
            _table = new RobotTable(size);
            _p1Would[0] = -1; _p1Would[1] = -1;
            _p2Would[0] = -1; _p2Would[1] = -1;
            _p1StandaloneCMD = new string[5];
            _p2StandaloneCMD = new string[5];
        }

        public RobotTable Table { get { return _table; } }


        public void Step(int x, int y)
        {
            if (!_isGameOver)
            {
                if (_table.GetPlayer == 1)
                {
                    if (_table.GetTableValue(x, y) == 0)
                    {
                        if ((x==_table.Prey[0]-1 && y==_table.Prey[1]) || (x==_table.Prey[0]+1 && y==_table.Prey[1]) || (x==_table.Prey[0] && y==_table.Prey[1]-1) || (x==_table.Prey[0] && y==_table.Prey[1]+1))
                        {
                            _table.SetTableValue(_table.Prey[0], _table.Prey[1], 0);
                            _table.Prey[0] = x;
                            _table.Prey[1] = y;
                            _table.SetTableValue(x, y, 1);
                            _table.ChangePlayer();
                        }

                        //if (Moves == 0) { _isGameOver = true; OnGameOver(Win.Player2); }
                        //if(x==0 && x!=_size-1) { }
                        //if (x==_size-1) { }

                    }

                }
                else if (_table.GetPlayer == 2)
                {
                    if (_table.GetTableValue(x, y) == 2)
                    {
                        P2Would[0] = x; P2Would[1] = y;
                        //if (x == 0 && x != _size-1) { } 
                    }
                    if (_table.GetTableValue(x, y) == 0 && P2Would[0] != -1 && P2Would[1] != -1 && ((x==P2Would[0]-1 && y==P2Would[1]) || (x==P2Would[0]+1 && y==P2Would[1]) || (x==P2Would[0] && y==P2Would[1]-1) || (x==P2Would[0] && y==P2Would[1]+1)))
                    {
                        _table.SetTableValue(P2Would[0], P2Would[1], 0);
                        _table.SetTableValue(x, y, 2);
                        //_table.SetTableValue(x, y, 2);
                        P2Would[0] = -1; P2Would[1] = -1;
                        _table.Moves--;

                        if (_table.Moves == 0) { _isGameOver = true; Winner = 1;  }
                        //sarkok
                        if (_table.Prey[0]==0 && _table.Prey[1]==0 && (_table.GetTableValue(_table.Prey[0]+1, _table.Prey[1])==2 && _table.GetTableValue(_table.Prey[0], _table.Prey[1]+1)==2))
                        {
                            _isGameOver = true; Winner = 2;
                        }
                        else if (_table.Prey[0]==_size-1 && _table.Prey[1]==0 && (_table.GetTableValue(_table.Prey[0]-1, _table.Prey[1])==2 && _table.GetTableValue(_table.Prey[0], _table.Prey[1]+1)==2))
                        {
                            _isGameOver = true; Winner = 2;
                        }
                        else if ((_table.Prey[0]==0 && _table.Prey[1]==_size-1) && (_table.GetTableValue(_table.Prey[0], _table.Prey[1]-1)==2 && _table.GetTableValue(_table.Prey[0]+1, _table.Prey[1])==2))
                        {
                            _isGameOver = true; Winner = 2;
                        }
                        else if ((_table.Prey[0]==_size-1 && _table.Prey[1]==_size-1) && (_table.GetTableValue(_table.Prey[0]-1, _table.Prey[1])==2 && _table.GetTableValue(_table.Prey[0], _table.Prey[1]-1)==2))
                        {
                            _isGameOver = true; Winner = 2;
                        }
                        //felső sor
                        else if ((_table.Prey[0]!=0 && _table.Prey[0]!=_size-1 && _table.Prey[1]==0) && (_table.GetTableValue(_table.Prey[0]-1, _table.Prey[1])==2 && _table.GetTableValue(_table.Prey[0]+1, _table.Prey[1])==2 && _table.GetTableValue(_table.Prey[0], _table.Prey[1]+1)==2))
                        {
                            _isGameOver = true; Winner = 2;
                        }
                        //alsó sor
                        else if ((_table.Prey[0]!=0 && _table.Prey[0]!=_size-1 && _table.Prey[1]==_size-1) && (_table.GetTableValue(_table.Prey[0]-1, _table.Prey[1])==2 && _table.GetTableValue(_table.Prey[0]+1, _table.Prey[1])==2 && _table.GetTableValue(_table.Prey[0], _table.Prey[1]-1)==2))
                        {
                            _isGameOver = true; Winner = 2;
                        }
                        //bal oldal
                        else if ((_table.Prey[0]==0 && _table.Prey[1]!=_size-1 && _table.Prey[1]!=0) && (_table.GetTableValue(_table.Prey[0], _table.Prey[1]-1)==2 && _table.GetTableValue(_table.Prey[0], _table.Prey[1]+1)==2 && _table.GetTableValue(_table.Prey[0]+1, _table.Prey[1])==2))
                        {
                            _isGameOver = true; Winner = 2;
                        }
                        //jobb oldal
                        else if ((_table.Prey[0]==_size-1 && _table.Prey[1]!=_size-1 && _table.Prey[1]!=0) && (_table.GetTableValue(_table.Prey[0], _table.Prey[1]-1)==2 && _table.GetTableValue(_table.Prey[0], _table.Prey[1]+1)==2 && _table.GetTableValue(_table.Prey[0]-1, _table.Prey[1])==2))
                        {
                            _isGameOver = true; Winner = 2;
                        }
                        else if (((_table.Prey[0]>0 && _table.Prey[1]>0) && (_table.Prey[0]<_size-1 && _table.Prey[1]<_size-1)) && ((_table.GetTableValue(_table.Prey[0]-1, _table.Prey[1])==2 && _table.GetTableValue(_table.Prey[0]+1, _table.Prey[1])==2 && _table.GetTableValue(_table.Prey[0], _table.Prey[1]-1)==2 && _table.GetTableValue(_table.Prey[0], _table.Prey[1]+1)==2)))
                        {
                            _isGameOver = true; Winner = 2;
                        }

                        //if (IsGameOver)
                        //{
                        //    OnGameOver(Win.Hunter);
                        //}
                        _table.ChangePlayer();
                    }
                }
            }
        }

        public void CheckGO()
        {
            if (IsGameOver && Winner == 2)
            {
                OnGameOver(Win.Hunter);
            }
            else if (IsGameOver && Winner == 1)
            {
                OnGameOver(Win.Prey);
            }

        }



        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _table);
        }

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _table = await _dataAccess.LoadAsync(path);
            _size = _table.GetSize;
            //_gameStepCount = 0;
            _isGameOver = false;

        }


    }
}
