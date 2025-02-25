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
        private int[] _p1Act = new int[2];
        private int[] _p2Act = new int[2];
        private int[] _p1Would = new int[2];
        private int[] _p2Would = new int[2];
        private int _toColor;
        private int _toColor2;
        private string[] _p1StandaloneCMD;
        private string[] _p2StandaloneCMD;
        private bool _p1moveChanged;
        private bool _p2moveChanged;
        private int _length = 0;
        private bool _p1HitP2;
        private bool _p2HitP1;
        private int _winner;
        private bool _stop;
        private int _z;
        private int _redCount = 0;
        private int _blueCount = 0;

        public event EventHandler<RobotEventArgs>? GameOver;
        public event EventHandler? Player1Hit;
        public event EventHandler? Player2Hit;

        //IRobotDataAccess

        public bool IsGameOver => _isGameOver;
        public int Length { get { return _length; } set { _length = value; } }

        public int RedCount { get { return _redCount; } set { _redCount = value; } }
        public int BlueCount { get { return _blueCount; } set { _blueCount = value; } }
        public int Z { get { return _z; } set { _z = value; } }
        public bool Stop { get { return _stop; } set { _stop = value; } }
        public int ToColor { get { return _toColor; } set { _toColor = value; } }
        public int ToColor2 { get { return _toColor2; } set { _toColor2 = value; } }
        public int[] P1Act { get { return _p1Act; } set { _p1Act = value; } }
        public int[] P2Act { get { return _p2Act; } set { _p2Act = value; } }
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
            ToColor=-1;
            _p1Act[0] = -1; _p1Act[1] = -1;
            _p2Act[0] = -1; _p2Act[1] = -1;
            _p1Would[0] = -1; _p1Would[1] = -1;
            _p2Would[0] = -1; _p2Would[1] = -1;
            _p1StandaloneCMD = new string[5];
            _p2StandaloneCMD = new string[5];
            Stop=false;
        }

        public RobotTable Table { get { return _table; } }

        public void GenerateFields()
        {
            Random random = new Random();
            int x, y, n=_table.GetSize;

            for (int i = 0; i <_table.GetSize; i++)
            {
                do
                {
                    x = random.Next(_table.GetSize);
                    y = random.Next(_table.GetSize);
                }
                while (_table.GetTableValue(x, y) != 0);

                _table.SetTableValue(x, y, 1);
            }

            while(n>0)
            {
                x = random.Next(_table.GetSize);
                y = random.Next(_table.GetSize);

                if(_table.GetTableValue(x, y) == 0)
                {
                    _table.SetTableValue(x, y, 2);
                    n--;
                }
            }
        }


        public void Step1(int x, int y)
        {
            if (!_isGameOver)
            {
                if (P1Act[0]!= -1 & P1Act[1]!= -1)
                {
                    Step2(x, y);
                }
                else if(_table.GetPlayer == 1 && _table.GetTableValue(x, y) == 1 && P1Act[0]== -1 & P1Act[1]== -1)
                {
                    P1Act[0]= x; P1Act[1]= y;
                    
                    ToColor=1;
                }
                else if( _table.GetPlayer == 2 && _table.GetTableValue(x, y) == 2 && P1Act[0]== -1 & P1Act[1]== -1)
                {
                    P1Act[0]= x; P1Act[1]= y;
                    
                    ToColor=2;
                }
            }
        }

        public void Step2(int x, int y)
        {
            Stop=false;
            P1Would[0] = x; P1Would[1] = y;
            if(P1Would[0]+1 == P1Act[0] && P1Would[1] == P1Act[1]) //balra tolnánk
            {
                _table.SetTableValue(P1Act[0], P1Act[1], 0);
                if (_table.GetTableValue(x, y) == 1)
                {
                    ToColor=1;
                }
                else if (_table.GetTableValue(x, y) == 2)
                {
                    ToColor=2;
                }
                else
                {
                    ToColor=0;
                }

                if (x >= 0 && _table.GetTableValue(x, y) == 0)
                {
                    _table.SetTableValue(x, y, _table.GetPlayer);
                    P1Act[0]= -1; P1Act[1]= -1;
                    _table.Moves--;
                    CheckSum();
                    _table.ChangePlayer();
                    return;
                }
                _table.SetTableValue(x, y, _table.GetPlayer);
                x--;
                ToColor2=-1;
                Z=1; //Z represents which ToColor can be active to modify the cell

                while (x>=0)
                {
                    if (_table.GetTableValue(x, y) == 1 && ToColor2==-1)
                    {
                        ToColor2=1;
                    }
                    else if (_table.GetTableValue(x, y) == 2 && ToColor2==-1)
                    {
                        ToColor2=2;
                    }
                    else if (_table.GetTableValue(x, y) == 0 && ToColor2==-1)
                    {
                        Stop=true;
                        ToColor2=0;
                    }

                    if (_table.GetTableValue(x, y) == 1 && ToColor == -1)
                    {
                        ToColor=1;
                    }
                    else if (_table.GetTableValue(x, y) == 2 && ToColor == -1)
                    {
                        ToColor=2;
                    }
                    else if (_table.GetTableValue(x, y) == 0 && ToColor == -1)
                    {
                        Stop=true;
                        ToColor=0;
                    }

                    if (Z==1)
                    {
                        _table.SetTableValue(x, y, ToColor); ToColor=-1; Z=2;
                    }
                    else if (Z==2)
                    {
                        _table.SetTableValue(x, y, ToColor2); ToColor2=-1; Z=1;
                    }

                    if (Stop)
                    {
                        P1Act[0]= -1; P1Act[1]= -1;
                        _table.Moves--;
                        CheckSum();
                        _table.ChangePlayer();
                        return;
                    }
                    
                    x--;
                }
                    
            }
            else if(P1Would[0] == P1Act[0]+1 && P1Would[1] == P1Act[1]) //jobbra tolnánk
            {
                _table.SetTableValue(P1Act[0], P1Act[1], 0);
                if (_table.GetTableValue(x, y) == 1)
                {
                    ToColor=1;
                }
                else if (_table.GetTableValue(x, y) == 2)
                {
                    ToColor=2;
                }
                else
                {
                    ToColor=0;
                }

                if (x <= _size - 1 && _table.GetTableValue(x, y) == 0)
                {
                    _table.SetTableValue(x, y, _table.GetPlayer);
                    P1Act[0]= -1; P1Act[1]= -1;
                    _table.Moves--;
                    CheckSum();
                    _table.ChangePlayer();
                    return;
                }
                _table.SetTableValue(x, y, _table.GetPlayer);
                x++;
                ToColor2=-1;
                Z=1;

                while (x<=_size-1)
                {
                    if (_table.GetTableValue(x, y) == 1 && ToColor2==-1)
                    {
                        ToColor2=1;
                    }
                    else if (_table.GetTableValue(x, y) == 2 && ToColor2==-1)
                    {
                        ToColor2=2;
                    }
                    else if (_table.GetTableValue(x, y) == 0 && ToColor2==-1)
                    {
                        Stop=true;
                        ToColor2=0;
                    }

                    if (_table.GetTableValue(x, y) == 1 && ToColor == -1)
                    {
                        ToColor=1;
                    }
                    else if (_table.GetTableValue(x, y) == 2 && ToColor == -1)
                    {
                        ToColor=2;
                    }
                    else if (_table.GetTableValue(x, y) == 0 && ToColor == -1)
                    {
                        Stop=true;
                        ToColor=0;
                    }

                    if (Z==1)
                    {
                        _table.SetTableValue(x, y, ToColor); ToColor=-1; Z=2;
                    }
                    else if (Z==2)
                    {
                        _table.SetTableValue(x, y, ToColor2); ToColor2=-1; Z=1;
                    }


                    if (Stop)
                    {
                        P1Act[0]= -1; P1Act[1]= -1;
                        _table.Moves--;
                        CheckSum();
                        _table.ChangePlayer();
                        return;
                    }

                    x++;
                }
            }
            else if(P1Would[0] == P1Act[0] && P1Would[1]+1 == P1Act[1]) //fel tolnánk
            {
                _table.SetTableValue(P1Act[0], P1Act[1], 0);
                if (_table.GetTableValue(x, y) == 1)
                {
                    ToColor=1;
                }
                else if (_table.GetTableValue(x, y) == 2)
                {
                    ToColor=2;
                }
                else
                {
                    ToColor=0;
                }

                if (y >= 0 && _table.GetTableValue(x, y) == 0)
                {
                    _table.SetTableValue(x, y, _table.GetPlayer);
                    P1Act[0]= -1; P1Act[1]= -1;
                    _table.Moves--;
                    CheckSum();
                    _table.ChangePlayer();
                    return;
                }
                _table.SetTableValue(x, y, _table.GetPlayer);
                y--;
                ToColor2=-1;
                Z=1;

                while (y>=0)
                {
                    if (_table.GetTableValue(x, y) == 1 && ToColor2==-1)
                    {
                        ToColor2=1;
                    }
                    else if (_table.GetTableValue(x, y) == 2 && ToColor2==-1)
                    {
                        ToColor2=2;
                    }
                    else if (_table.GetTableValue(x, y) == 0 && ToColor2==-1)
                    {
                        Stop=true;
                        ToColor2=0;
                    }

                    if (_table.GetTableValue(x, y) == 1 && ToColor == -1)
                    {
                        ToColor=1;
                    }
                    else if (_table.GetTableValue(x, y) == 2 && ToColor == -1)
                    {
                        ToColor=2;
                    }
                    else if (_table.GetTableValue(x, y) == 0 && ToColor == -1)
                    {
                        Stop=true;
                        ToColor=0;
                    }

                    if (Z==1)
                    {
                        _table.SetTableValue(x, y, ToColor); ToColor=-1; Z=2;
                    }
                    else if (Z==2)
                    {
                        _table.SetTableValue(x, y, ToColor2); ToColor2=-1; Z=1;
                    }

                    if (Stop)
                    {
                        P1Act[0]= -1; P1Act[1]= -1;
                        _table.Moves--;
                        CheckSum();
                        _table.ChangePlayer();
                        return;
                    }

                    y--;
                }
            }
            else if(P1Would[0] == P1Act[0] && P1Would[1] == P1Act[1]+1) //le tolnánk
            {
                _table.SetTableValue(P1Act[0], P1Act[1], 0);
                if (_table.GetTableValue(x, y) == 1)
                {
                    ToColor=1;
                }
                else if (_table.GetTableValue(x, y) == 2)
                {
                    ToColor=2;
                }
                else
                {
                    ToColor=0;
                }

                if (y <= _size - 1 && _table.GetTableValue(x, y) == 0)
                {
                    _table.SetTableValue(x, y, _table.GetPlayer);
                    P1Act[0]= -1; P1Act[1]= -1;
                    _table.Moves--;
                    CheckSum();
                    _table.ChangePlayer();
                    return;
                }
                _table.SetTableValue(x, y, _table.GetPlayer);
                y++;
                ToColor2=-1;
                Z=1;
                
                while (y <= _size - 1)
                {
                    if (_table.GetTableValue(x, y) == 1 && ToColor2==-1)
                    {
                        ToColor2=1;
                    }
                    else if (_table.GetTableValue(x, y) == 2 && ToColor2==-1)
                    {
                        ToColor2=2;
                    }
                    else if (_table.GetTableValue(x, y) == 0 && ToColor2==-1)
                    {
                        Stop=true;
                        ToColor2=0;
                    }

                    if (_table.GetTableValue(x, y) == 1 && ToColor == -1)
                    {
                        ToColor=1;
                    }
                    else if (_table.GetTableValue(x, y) == 2 && ToColor == -1)
                    {
                        ToColor=2;
                    }
                    else if (_table.GetTableValue(x, y) == 0 && ToColor == -1)
                    {
                        Stop=true;
                        ToColor=0;
                    }

                    if (Z==1)
                    {
                        _table.SetTableValue(x, y, ToColor); ToColor=-1; Z=2;
                    }
                    else if (Z==2)
                    {
                        _table.SetTableValue(x, y, ToColor2); ToColor2=-1; Z=1;
                    }

                    if (Stop)
                    {
                        P1Act[0]= -1; P1Act[1]= -1;
                        _table.Moves--;
                        CheckSum();
                        _table.ChangePlayer();
                        return;
                    }

                    //_table.SetTableValue(x, y, ToColor);

                    //if (_table.GetTableValue(x, y) == 1)
                    //{
                    //    ToColor=1;
                    //}
                    //else if (_table.GetTableValue(x, y) == 2)
                    //{
                    //    ToColor=2;
                    //}
                    //else
                    //{
                    //    P1Act[0]= -1; P1Act[1]= -1;
                    //    _table.ChangePlayer();
                    //    return;
                    //}
                    y++;
                }
            }
            
            P1Act[0]= -1; P1Act[1]= -1;

            _table.Moves--;
            CheckSum();

            _table.ChangePlayer();
        }

        public void CheckSum()
        {
            for(int i=0; i<_table.GetSize; ++i)
            {
                for (int j = 0; j<_table.GetSize; ++j)
                {
                    if(_table.GetTableValue(i, j) == 1)
                    {
                        RedCount++;
                    }
                    if(_table.GetTableValue(i, j) == 2)
                    {
                        BlueCount++;
                    }
                }
            }

            if (_table.Moves == 0)
            {
                if (RedCount>BlueCount)
                {
                    _isGameOver = true; Winner = 1;
                }
                else if (BlueCount>RedCount)
                {
                    _isGameOver = true; Winner = 2;
                }
                else if(RedCount==BlueCount)
                {
                    _isGameOver = true; 
                }
            }
        }

        public void CheckGO()
        {
            if (IsGameOver && Winner == 2)
            {
                OnGameOver(Win.Blue);
            }
            else if(IsGameOver && Winner == 1)
            {
                OnGameOver(Win.Red);
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


//}
//            else if (_table.GetPlayer == 2)
//{
//    P2Would[0] = x; P2Would[1] = y;
//    if (P2Would[0]+1 == P2Act[0] && P2Would[1] == P2Act[1]) //balra tolnánk
//    {

//    }
//    else if (P2Would[0] == P2Act[0]+1 && P2Would[1] == P2Act[1]) //jobbra tolnánk
//    {

//    }
//    else if (P2Would[0] == P2Act[0] && P2Would[1]+1 == P2Act[1]) //le tolnánk
//    {

//    }
//    else if (P2Would[0] == P2Act[0] && P2Would[1] == P2Act[1]+1) //fel tolnánk
//    {

//    }
//}