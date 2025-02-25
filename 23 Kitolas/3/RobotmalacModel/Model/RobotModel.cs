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

        public event EventHandler<RobotEventArgs>? GameOver;
        public event EventHandler? Player1Hit;
        public event EventHandler? Player2Hit;

        //IRobotDataAccess

        public bool IsGameOver => _isGameOver;
        public int Length { get { return _length; } set { _length = value; } }
        public int[] P1Would { get { return _p1Would; } set { _p1Would = value; } }
        public int[] P2Would { get { return _p2Would; } set { _p2Would = value; } }

        public string[] P1StandaloneCMD
        {
            get { return _p1StandaloneCMD; }
            set { _p1StandaloneCMD = value;}
        }

        public string[] P2StandaloneCMD
        {
            get { return _p2StandaloneCMD; }
            set { _p2StandaloneCMD = value;}
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


        public void Step()
        {
            if (!_isGameOver)
            {

                _table.ChangePlayer();
            }
        }

        public void CollectCommand(string command, int player)
        {
            string[] commands = command.Split(',');

            if (player == 1)
            {
                _p1StandaloneCMD = commands.Take(5).ToArray();
            }
            else if (player == 2)
            {
                _p2StandaloneCMD = commands.Take(5).ToArray();
            }
            
        }

        public void DoCommand()
        {
            if (!_isGameOver) 
            {
                //p2 hossz nagyobb
                if (_p1StandaloneCMD.Length < _p2StandaloneCMD.Length)
                {
                    Length = _p1StandaloneCMD.Length;
                    //TurnDirection();
                    Move();
                    if (!_isGameOver)
                    {
                        //Length = _p2StandaloneCMD.Length - _p1StandaloneCMD.Length;
                        for (int i = _p1StandaloneCMD.Length; i<_p2StandaloneCMD.Length; i++)
                        {
                            if (_p2StandaloneCMD[i] == "turn left")
                            {
                                switch (_table.P2Direction)
                                {
                                    case "left": _table.P2Direction = "down"; break;
                                    case "right": _table.P2Direction = "up"; break;
                                    case "up": _table.P2Direction = "left"; break;
                                    case "down": _table.P2Direction = "right"; break;
                                }
                            }
                            else if (_p2StandaloneCMD[i] == "turn right")
                            {
                                switch (_table.P2Direction)
                                {
                                    case "left": _table.P2Direction = "up"; break;
                                    case "right": _table.P2Direction = "down"; break;
                                    case "up": _table.P2Direction = "right"; break;
                                    case "down": _table.P2Direction = "left"; break;
                                }
                            }

                            else if (_p2StandaloneCMD[i] == "move up" && _table.P2Current[1] != 0)
                            {
                                P2MoveChanged = false;
                                P2Would[0] = _table.P2Current[0];
                                P2Would[1] = _table.P2Current[1]-1;
                                ChangeCurrent();
                            }
                            else if (_p2StandaloneCMD[i] == "move down" && _table.P2Current[1] != _size-1)
                            {
                                P2MoveChanged = false;
                                P2Would[0] = _table.P2Current[0];
                                P2Would[1] = _table.P2Current[1]+1;
                                ChangeCurrent();
                            }
                            else if (_p2StandaloneCMD[i] == "move left" && _table.P2Current[0] != 0)
                            {
                                P2MoveChanged = false;
                                P2Would[0] = _table.P2Current[0]-1;
                                P2Would[1] = _table.P2Current[1];
                                ChangeCurrent();
                            }
                            else if (_p2StandaloneCMD[i] == "move right" && _table.P2Current[0] != _size-1)
                            {
                                P2MoveChanged = false;
                                P2Would[0] = _table.P2Current[0]+1;
                                P2Would[1] = _table.P2Current[1];
                                ChangeCurrent();
                            }

                            //Shoot
                            if (_p2StandaloneCMD[i] == "shoot" && !_isGameOver)
                            {
                                if (_table.P1Current[0] == _table.P2Current[0]) //azonos i, azonos oszlop
                                {
                                    if ((_table.P1Current[1] > _table.P2Current[1]) && _table.P2Direction == "down")  //melyik j indexe nagyobb | p1 van lentebb
                                    {
                                        _table.P1Health = _table.P1Health-1;
                                        P2HitP1 = true;
                                        if (_table.P1Health <= 0) { _isGameOver = true; OnGameOver(Win.Player2); }
                                    }
                                    else if ((_table.P1Current[1] < _table.P2Current[1]) && _table.P2Direction == "up") //p1 fentebb van
                                    {
                                        _table.P1Health = _table.P1Health-1;
                                        P2HitP1 = true;
                                        if (_table.P1Health <= 0) { _isGameOver = true; OnGameOver(Win.Player2); }
                                    }
                                }
                                else if (_table.P1Current[1] == _table.P2Current[1]) //azonos j, azonos sor
                                {
                                    if ((_table.P1Current[0] > _table.P2Current[0]) && _table.P2Direction == "right") //melyik i indexe nagyobb
                                    {
                                        _table.P1Health = _table.P1Health-1;
                                        P2HitP1 = true;
                                        if (_table.P1Health <= 0) { _isGameOver = true; OnGameOver(Win.Player2); }
                                    }
                                    else if ((_table.P1Current[0] < _table.P2Current[0]) && _table.P2Direction == "left")
                                    {
                                        _table.P1Health = _table.P1Health-1;
                                        P2HitP1 = true;
                                        if (_table.P1Health <= 0) { _isGameOver = true; OnGameOver(Win.Player2); }
                                    }
                                }
                            }

                            //Punch
                            if (_p2StandaloneCMD[i] == "punch" && !_isGameOver)
                            {
                                for (int k = _table.P2Current[1]-1; k<=_table.P2Current[1]+1; k++)
                                {
                                    for (int l = _table.P2Current[0]-1; l<=_table.P2Current[0]+1; l++)
                                    {
                                        if (k>=0 && k<_size && l>=0 && l<_size && _table.P1Current[0] == l && _table.P1Current[1] == k)
                                        {
                                            _table.P1Health = _table.P1Health-1;
                                            P2HitP1 = true;
                                            if (_table.P1Health == 0) { _isGameOver = true; OnGameOver(Win.Player2); }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                //p1 hossz nagyobb
                else if (_p1StandaloneCMD.Length > _p2StandaloneCMD.Length)
                {
                    Length = _p2StandaloneCMD.Length;

                    Move();
                    if (!_isGameOver)
                    {
                        //Length = _p1StandaloneCMD.Length - _p2StandaloneCMD.Length;
                        for (int i = _p2StandaloneCMD.Length; i<_p1StandaloneCMD.Length; i++)
                        {
                            if (_p1StandaloneCMD[i] == "turn left")
                            {
                                switch (_table.P1Direction)
                                {
                                    case "left": _table.P1Direction = "down"; break;
                                    case "right": _table.P1Direction = "up"; break;
                                    case "up": _table.P1Direction = "left"; break;
                                    case "down": _table.P1Direction = "right"; break;
                                }
                            }
                            else if (_p1StandaloneCMD[i] == "turn right")
                            {
                                switch (_table.P1Direction)
                                {
                                    case "left": _table.P1Direction = "up"; break;
                                    case "right": _table.P1Direction = "down"; break;
                                    case "up": _table.P1Direction = "right"; break;
                                    case "down": _table.P1Direction = "left"; break;
                                }
                            }

                            else if (_p1StandaloneCMD[i] == "move up" && _table.P1Current[1] != 0)
                            {
                                P1MoveChanged = false;
                                P1Would[0] = _table.P1Current[0];
                                P1Would[1] = _table.P1Current[1] - 1;
                                ChangeCurrent();
                            }
                            else if (_p1StandaloneCMD[i] == "move down" && _table.P1Current[1] != _size - 1)
                            {
                                P1MoveChanged = false;
                                P1Would[0] = _table.P1Current[0];
                                P1Would[1] = _table.P1Current[1] + 1;
                                ChangeCurrent();
                            }
                            else if (_p1StandaloneCMD[i] == "move left" && _table.P1Current[0] != 0)
                            {
                                P1MoveChanged = false;
                                P1Would[0] = _table.P1Current[0] - 1;
                                P1Would[1] = _table.P1Current[1];
                                ChangeCurrent();
                            }
                            else if (_p1StandaloneCMD[i] == "move right" && _table.P1Current[0] != _size - 1)
                            {
                                P1MoveChanged = false;
                                P1Would[0] = _table.P1Current[0] + 1;
                                P1Would[1] = _table.P1Current[1];
                                ChangeCurrent();
                            }

                            //shoot
                            _p1HitP2 = false;
                            _p2HitP1 = false;
                            if (_p1StandaloneCMD[i] == "shoot" && !_isGameOver)
                            {
                                if (_table.P1Current[0] == _table.P2Current[0]) //azonos i, azonos oszlop
                                {
                                    if ((_table.P1Current[1] > _table.P2Current[1]) && _table.P1Direction == "up")  //melyik j indexe nagyobb | p1 van lentebb
                                    {
                                        _table.P2Health = _table.P2Health-1;
                                        P1HitP2 = true;
                                        if (_table.P2Health <= 0) { _isGameOver = true; OnGameOver(Win.Player1); }
                                    }
                                    else if ((_table.P1Current[1] < _table.P2Current[1]) && _table.P1Direction == "down") //p1 fentebb van
                                    {
                                        _table.P2Health = _table.P2Health-1;
                                        P1HitP2 = true;
                                        if (_table.P2Health <= 0) { _isGameOver = true; OnGameOver(Win.Player1); }
                                    }
                                }
                                else if (_table.P1Current[1] == _table.P2Current[1]) //azonos j, azonos sor
                                {
                                    if ((_table.P1Current[0] > _table.P2Current[0]) && _table.P1Direction == "left") //melyik i indexe nagyobb
                                    {
                                        _table.P2Health = _table.P2Health-1;
                                        P1HitP2 = true;
                                        if (_table.P2Health <= 0) { _isGameOver = true; OnGameOver(Win.Player1); }
                                    }
                                    else if ((_table.P1Current[0] < _table.P2Current[0]) && _table.P1Direction == "right")
                                    {
                                        _table.P2Health = _table.P2Health-1;
                                        P1HitP2 = true;
                                        if (_table.P2Health <= 0) { _isGameOver = true; OnGameOver(Win.Player1); }
                                    }
                                }
                            }

                            //Punch
                            P1HitP2 = false; P2HitP1 = false;
                            if (_p1StandaloneCMD[i] == "punch" && !_isGameOver)
                            {
                                for (int k = _table.P1Current[1]-1; k<=_table.P1Current[1]+1; k++)
                                {
                                    for (int l = _table.P1Current[0]-1; l<=_table.P1Current[0]+1; l++)
                                    {
                                        if (k>=0 && k<_size && l>=0 && l<_size && _table.P2Current[0] == l && _table.P2Current[1] == k)
                                        {
                                            _table.P2Health = _table.P2Health-1;
                                            P1HitP2 = true;
                                            if (_table.P2Health == 0) { _isGameOver = true; OnGameOver(Win.Player1); return; }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                //p1 és p2 hossza =
                else
                {
                    Length = _p1StandaloneCMD.Length;

                    Move();
                }
            }
            

        }

        public void TurnDirection(int i)
        {
            if (!_isGameOver)
            {
                if (_p1StandaloneCMD[i] == "turn left")
                {
                    switch (_table.P1Direction)
                    {
                        case "left": _table.P1Direction = "down"; break;
                        case "right": _table.P1Direction = "up"; break;
                        case "up": _table.P1Direction = "left"; break;
                        case "down": _table.P1Direction = "right"; break;
                    }
                }
                else if (_p1StandaloneCMD[i] == "turn right")
                {
                    switch (_table.P1Direction)
                    {
                        case "left": _table.P1Direction = "up"; break;
                        case "right": _table.P1Direction = "down"; break;
                        case "up": _table.P1Direction = "right"; break;
                        case "down": _table.P1Direction = "left"; break;
                    }
                }

                if (_p2StandaloneCMD[i] == "turn left")
                {
                    switch (_table.P2Direction)
                    {
                        case "left": _table.P2Direction = "down"; break;
                        case "right": _table.P2Direction = "up"; break;
                        case "up": _table.P2Direction = "left"; break;
                        case "down": _table.P2Direction = "right"; break;
                    }
                }
                else if (_p2StandaloneCMD[i] == "turn right")
                {
                    switch (_table.P2Direction)
                    {
                        case "left": _table.P2Direction = "up"; break;
                        case "right": _table.P2Direction = "down"; break;
                        case "up": _table.P2Direction = "right"; break;
                        case "down": _table.P2Direction = "left"; break;
                    }
                }
            }

        }

        public void Move()
        {
            if (!_isGameOver)
            {
                //Length = _p1StandaloneCMD.Length;
                for (int i = 0; i< Length; i++)
                {
                    //mozgatás
                    P1Would[0] = -1; P1Would[1] = -1;
                    P2Would[0] = -1; P2Would[1] = -1;
                    if (_p1StandaloneCMD[i] == "move up" && _table.P1Current[1] != 0)
                    {
                        P1MoveChanged = false;
                        P1Would[0] = _table.P1Current[0];
                        P1Would[1] = _table.P1Current[1]-1;

                        if (_p2StandaloneCMD[i] == "move down" && _table.P2Current[1] != _size-1)
                        {
                            P2Would[0] = _table.P2Current[0]; P2Would[1] = _table.P2Current[1]+1; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move left" && _table.P2Current[0] != 0)
                        {
                            P2Would[0] = _table.P2Current[0]-1; P2Would[1] = _table.P2Current[1]; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move right" && _table.P2Current[0] != _size-1)
                        {
                            P2Would[0] = _table.P2Current[0]+1; P2Would[1] = _table.P2Current[1]; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move up" && _table.P2Current[1] != 0)
                        {
                            P2Would[0] = _table.P2Current[0]; P2Would[1] = _table.P2Current[1]-1; ChangeCurrent();
                        }


                        else
                        {
                            //throw new Exception()
                            ChangeCurrent();
                        }

                    }
                    else if (_p1StandaloneCMD[i] == "move down" && _table.P1Current[1] != _size-1)
                    {
                        P1MoveChanged = false;
                        P1Would[0] = _table.P1Current[0];
                        P1Would[1] = _table.P1Current[1]+1;

                        if (_p2StandaloneCMD[i] == "move up" && _table.P2Current[1] != 0)
                        {
                            P2Would[0] = _table.P2Current[0]; P2Would[1] = _table.P2Current[1]-1; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move left" && _table.P2Current[0] != 0)
                        {
                            P2Would[0] = _table.P2Current[0]-1; P2Would[1] = _table.P2Current[1]; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move right" && _table.P2Current[0] != _size-1)
                        {
                            P2Would[0] = _table.P2Current[0]+1; P2Would[1] = _table.P2Current[1]; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move down" && _table.P2Current[1] != _size-1)
                        {
                            P2Would[0] = _table.P2Current[0]; P2Would[1] = _table.P2Current[1]+1; ChangeCurrent();
                        }

                        else
                        {
                            //throw new Exception()
                            ChangeCurrent();
                        }

                    }
                    else if (_p1StandaloneCMD[i] == "move left" && _table.P1Current[0] != 0)
                    {
                        P1MoveChanged = false;
                        P1Would[0] = _table.P1Current[0]-1;
                        P1Would[1] = _table.P1Current[1];

                        if (_p2StandaloneCMD[i] == "move up" && _table.P2Current[1] != 0)
                        {
                            P2Would[0] = _table.P2Current[0]; P2Would[1] = _table.P2Current[1]-1; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move down" && _table.P2Current[1] != _size-1)
                        {
                            P2Would[0] = _table.P2Current[0]; P2Would[1] = _table.P2Current[1]+1; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move right" && _table.P2Current[0] != _size-1)
                        {
                            P2Would[0] = _table.P2Current[0]+1; P2Would[1] = _table.P2Current[1]; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move left" && _table.P2Current[0] != 0)
                        {
                            P2Would[0] = _table.P2Current[0]-1; P2Would[1] = _table.P2Current[1]; ChangeCurrent();
                        }
                        else
                        {
                            ChangeCurrent();
                        }

                    }
                    //TODO: _p2std-re? v fv-be szervezés és paraméterezés? mi legyen a P1/P2Would sorsa a lefutás után?
                    else if (_p1StandaloneCMD[i] == "move right" && _table.P1Current[0] != _size-1)
                    {
                        P1MoveChanged = false;
                        P1Would[0] = _table.P1Current[0]+1;
                        P1Would[1] = _table.P1Current[1];

                        if (_p2StandaloneCMD[i] == "move up" && _table.P2Current[1] != 0)
                        {
                            P2Would[0] = _table.P2Current[0]; P2Would[1] = _table.P2Current[1]-1; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move down" && _table.P2Current[1] != _size-1)
                        {
                            P2Would[0] = _table.P2Current[0]; P2Would[1] = _table.P2Current[1]+1; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move left" && _table.P2Current[0] != 0)
                        {
                            P2Would[0] = _table.P2Current[0]-1; P2Would[1] = _table.P2Current[1]; ChangeCurrent();
                        }
                        else if (_p2StandaloneCMD[i] == "move right" && _table.P2Current[0] != _size-1)
                        {
                            P2Would[0] = _table.P2Current[0]+1; P2Would[1] = _table.P2Current[1]; ChangeCurrent();
                        }
                        else
                        {
                            ChangeCurrent();
                        }

                    }
                    else if (_p2StandaloneCMD[i] == "move up" || _p2StandaloneCMD[i] == "move down" || _p2StandaloneCMD[i] == "move left" || _p2StandaloneCMD[i] == "move right")
                    {
                        Move2(i);
                    }
                    //ütés és lézer és fordulás is
                    if (!_isGameOver)
                    {
                        TurnDirection(i);
                        Shoot(i);
                        
                    }
                    if (!_isGameOver)
                    {
                        Punch(i);
                    }
                    
                }
            }

        }

        public void Move2(int i)
        {
            P1Would[0] = -1; P1Would[1] = -1;
            P2Would[0] = -1; P2Would[1] = -1;
            if (_p2StandaloneCMD[i] == "move up" && _table.P2Current[1] != 0)
            {
                P2MoveChanged = false;
                P2Would[0] = _table.P2Current[0];
                P2Would[1] = _table.P2Current[1]-1;

                if (_p1StandaloneCMD[i] == "move down" && _table.P1Current[1] != _size-1)
                {
                    P1Would[0] = _table.P1Current[0]; P1Would[1] = _table.P1Current[1]+1; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move left" && _table.P1Current[0] != 0)
                {
                    P1Would[0] = _table.P1Current[0]-1; P1Would[1] = _table.P1Current[1]; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move right" && _table.P1Current[0] != _size-1)
                {
                    P1Would[0] = _table.P1Current[0]+1; P1Would[1] = _table.P1Current[1]; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move up" && _table.P1Current[1] != 0)
                {
                    P1Would[0] = _table.P1Current[0]; P1Would[1] = _table.P1Current[1]-1; ChangeCurrent();
                }
                else
                {
                    ChangeCurrent();
                }

            }
            if (_p2StandaloneCMD[i] == "move down" && _table.P2Current[1] != _size-1)
            {
                P2MoveChanged = false;
                P2Would[0] = _table.P2Current[0];
                P2Would[1] = _table.P2Current[1]+1;

                if (_p1StandaloneCMD[i] == "move up" && _table.P1Current[1] != 0)
                {
                    P1Would[0] = _table.P1Current[0]; P1Would[1] = _table.P1Current[1]-1; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move left" && _table.P1Current[0] != 0)
                {
                    P1Would[0] = _table.P1Current[0]-1; P1Would[1] = _table.P1Current[1]; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move right" && _table.P1Current[0] != _size-1)
                {
                    P1Would[0] = _table.P1Current[0]+1; P1Would[1] = _table.P1Current[1]; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move down" && _table.P1Current[1] != _size-1)
                {
                    P1Would[0] = _table.P1Current[0]; P1Would[1] = _table.P1Current[1]+1; ChangeCurrent();
                }
                else
                {
                    ChangeCurrent();
                }

            }
            if (_p2StandaloneCMD[i] == "move left" && _table.P2Current[0] != 0)
            {
                P2MoveChanged = false;
                P2Would[0] = _table.P2Current[0]-1;
                P2Would[1] = _table.P2Current[1];

                if (_p1StandaloneCMD[i] == "move up" && _table.P1Current[1] != 0)
                {
                    P1Would[0] = _table.P1Current[0]; P1Would[1] = _table.P1Current[1]-1; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move down" && _table.P1Current[1] != _size-1)
                {
                    P1Would[0] = _table.P1Current[0]; P1Would[1] = _table.P1Current[1]+1; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move right" && _table.P1Current[0] != _size-1)
                {
                    P1Would[0] = _table.P1Current[0]+1; P1Would[1] = _table.P1Current[1]; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move left" && _table.P1Current[0] != 0)
                {
                    P1Would[0] = _table.P1Current[0]-1; P1Would[1] = _table.P1Current[1]; ChangeCurrent();
                }
                else
                {
                    ChangeCurrent();
                }

            }
            //régi ötlet: _p2std-re? v fv-be szervezés és paraméterezés? mi legyen a P1/P2Would sorsa a lefutás után?
            if (_p2StandaloneCMD[i] == "move right" && _table.P2Current[0] != _size-1)
            {
                P2MoveChanged = false;
                P2Would[0] = _table.P2Current[0]+1;
                P2Would[1] = _table.P2Current[1];

                if (_p1StandaloneCMD[i] == "move up" && _table.P1Current[1] != 0)
                {
                    P1Would[0] = _table.P1Current[0]; P1Would[1] = _table.P1Current[1]-1; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move down" && _table.P1Current[1] != _size-1)
                {
                    P1Would[0] = _table.P1Current[0]; P1Would[1] = _table.P1Current[1]+1; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move left" && _table.P1Current[0] != 0)
                {
                    P1Would[0] = _table.P1Current[0]-1; P1Would[1] = _table.P1Current[1]; ChangeCurrent();
                }
                else if (_p1StandaloneCMD[i] == "move right" && _table.P1Current[0] != _size-1)
                {
                    P1Would[0] = _table.P1Current[0]+1; P1Would[1] = _table.P1Current[1]; ChangeCurrent();
                }
                else
                {
                    //throw new Exception()
                    ChangeCurrent();
                }

            }

        }

        public void Shoot(int i)
        {
            if (!_isGameOver)
            {
                _p1HitP2 = false;
                _p2HitP1 = false;
                if (_p1StandaloneCMD[i] == "shoot")
                {
                    if (_table.P1Current[0] == _table.P2Current[0]) //azonos i, azonos oszlop
                    {
                        if ((_table.P1Current[1] > _table.P2Current[1]) && _table.P1Direction == "up")  //melyik j indexe nagyobb | p1 van lentebb
                        {
                            _table.P2Health = _table.P2Health-1;
                            P1HitP2 = true;
                            if (_table.P2Health <= 0) { _isGameOver = true; OnGameOver(Win.Player1); return; }
                        }
                        else if ((_table.P1Current[1] < _table.P2Current[1]) && _table.P1Direction == "down") //p1 fentebb van
                        {
                            _table.P2Health = _table.P2Health-1;
                            P1HitP2 = true;
                            if (_table.P2Health <= 0) { _isGameOver = true; OnGameOver(Win.Player1); return; }
                        }
                    }
                    else if (_table.P1Current[1] == _table.P2Current[1]) //azonos j, azonos sor
                    {
                        if ((_table.P1Current[0] > _table.P2Current[0]) && _table.P1Direction == "left") //melyik i indexe nagyobb
                        {
                            _table.P2Health = _table.P2Health-1;
                            P1HitP2 = true;
                            if (_table.P2Health <= 0) { _isGameOver = true; OnGameOver(Win.Player1); return; }
                        }
                        else if ((_table.P1Current[0] < _table.P2Current[0]) && _table.P1Direction == "right")
                        {
                            _table.P2Health = _table.P2Health-1;
                            P1HitP2 = true;
                            if (_table.P2Health <= 0) { _isGameOver = true; OnGameOver(Win.Player1); return; }
                        }
                    }
                }
                if (_p2StandaloneCMD[i] == "shoot")
                {
                    if (_table.P1Current[0] == _table.P2Current[0]) //azonos i, azonos oszlop
                    {
                        if ((_table.P1Current[1] > _table.P2Current[1]) && _table.P2Direction == "down")  //melyik j indexe nagyobb | p1 van lentebb
                        {
                            _table.P1Health = _table.P1Health-1;
                            P2HitP1 = true;
                            if (_table.P1Health <= 0) { _isGameOver = true; OnGameOver(Win.Player2); return; }
                        }
                        else if ((_table.P1Current[1] < _table.P2Current[1]) && _table.P2Direction == "up") //p1 fentebb van
                        {
                            _table.P1Health = _table.P1Health-1;
                            P2HitP1 = true;
                            if (_table.P1Health <= 0) { _isGameOver = true; OnGameOver(Win.Player2); return; }
                        }
                    }
                    else if (_table.P1Current[1] == _table.P2Current[1]) //azonos j, azonos sor
                    {
                        if ((_table.P1Current[0] > _table.P2Current[0]) && _table.P2Direction == "right") //melyik i indexe nagyobb
                        {
                            _table.P1Health = _table.P1Health-1;
                            P2HitP1 = true;
                            if (_table.P1Health <= 0) { _isGameOver = true; OnGameOver(Win.Player2); return; }
                        }
                        else if ((_table.P1Current[0] < _table.P2Current[0]) && _table.P2Direction == "left")
                        {
                            _table.P1Health = _table.P1Health-1;
                            P2HitP1 = true;
                            if (_table.P1Health <= 0) { _isGameOver = true; OnGameOver(Win.Player2); return; }
                        }
                    }
                }
            }

        }

        public void Punch(int i)
        {
            P1HitP2 = false; P2HitP1 = false;
            if (_p1StandaloneCMD[i] == "punch")
            {
                for (int k = _table.P1Current[1]-1; k<=_table.P1Current[1]+1; k++)
                {
                    for (int l = _table.P1Current[0]-1; l<=_table.P1Current[0]+1; l++)
                    {
                        if (k>=0 && k<_size && l>=0 && l<_size && _table.P2Current[0] == l && _table.P2Current[1] == k)
                        {
                            _table.P2Health = _table.P2Health-1;
                            P1HitP2 = true;
                            if (_table.P2Health == 0) { _isGameOver = true; OnGameOver(Win.Player1); return; }
                        }
                    }
                }
            }
            if (_p2StandaloneCMD[i] == "punch")
            {
                for (int k = _table.P2Current[1]-1; k<=_table.P2Current[1]+1; k++)
                {
                    for (int l = _table.P2Current[0]-1; l<=_table.P2Current[0]+1; l++)
                    {
                        if (k>=0 && k<_size && l>=0 && l<_size && _table.P1Current[0] == l && _table.P1Current[1] == k)
                        {
                            _table.P1Health = _table.P1Health-1;
                            P2HitP1 = true;
                            if (_table.P1Health == 0) { _isGameOver = true; OnGameOver(Win.Player2); return; }
                        }
                    }
                }
            }
        }

        public void ChangeCurrent()
        {
            if ((P1Would[0] != _table.P2Current[0] || P1Would[1] != _table.P2Current[1]) && (P2Would[0] == -1 && P2Would[1] == -1))
            {
                _table.P1Current[0] = P1Would[0];
                _table.P1Current[1] = P1Would[1];
                P1MoveChanged = true;
            }
            else if ((P2Would[0] != _table.P1Current[0] || P2Would[1] != _table.P1Current[1]) && (P1Would[0] == -1 && P1Would[1] == -1))
            {
                _table.P2Current[0] = P2Would[0];
                _table.P2Current[1] = P2Would[1];
                P2MoveChanged = true;
            }
            else if ((P1Would[0] != P2Would[0] || P1Would[1] != P2Would[1]) && (P1Would[0] != -1 && P1Would[1] != -1) && (P2Would[0] != -1 && P2Would[1] != -1))
            {
                _table.P1Current[0] = P1Would[0];
                _table.P1Current[1] = P1Would[1];
                _table.P2Current[0] = P2Would[0];
                _table.P2Current[1] = P2Would[1];
                P1MoveChanged = true;
                P2MoveChanged= true;
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
