using MaciModel.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MaciModel.Model
{
    public class MaciGameModel
    {
        private MaciTable _table;
        private readonly IMaciDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;
        private int _baskets=0;

        public event EventHandler<MaciEventArgs>? GameAdvanced;
        public event EventHandler<MaciEventArgs>? GameOver;

        //IMaciDataAccess

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public int Baskets
        {
            get { return _baskets; }
            set { _baskets = value; }
        }

        public int AllBaskets { get; set; }



        public List<Ranger> Rangers { get; private set; }

        public MaciGameModel(IMaciDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new MaciTable(size);
            Rangers = new List<Ranger>();
            //GenerateFields();
        }

        public MaciTable Table { get { return _table; } }

        

        public void AddRanger()
        {
            Random random = new Random();
            int direction = random.Next(4);

            for (int i = 0; i <_table.GetSize; i++)
            {
                for (int j = 0; j < _table.GetSize; j++)
                {
                    if (_table.GetTableValue(i, j) == 4)
                    {
                        Ranger r = new(i, j);
                        r.RangerStepEvent += OnRangerStep;
                        Rangers.Add(r);
                    }
                }
            }

        }

        public void CountBaskets()
        {
            for (Int32 i = 0; i < GetSize; i++)
            {
                for (Int32 j = 0; j < GetSize; j++)
                {
                    if(_table.GetTableValue(i, j) == 2)
                    {
                        Baskets++;
                    }
                }
            }
        }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            _table.GameTime++;
            foreach (Ranger ranger in Rangers)
            {
                ranger.WouldStep();
            }

            OnGameAdvanced();
        }

        public void MovePlayer(int x, int y)
        {
            if (x >= 0 && x < GetSize && y >= 0 && y < GetSize && _table.GetTableValue(x, y) != 3)
            {
                if (_table.GetTableValue(x, y) == 2)
                {
                    Baskets--;
                    if (Baskets==0)
                    {
                        IsGameOver = true; OnGameOver();
                    }
                    
                }
                _table.SetTableValue(_table.PCurrent[0], _table.PCurrent[1], 0);
                _table.SetTableValue(x, y, 1);
                _table.PCurrent[0] = x; _table.PCurrent[1] = y;
            }
        }

        public void OnRangerStep(object? sender, RangerStepEventArgs e)
        {
            if (sender is Ranger ranger)
            {
                int X = ranger.PositionX;
                int Y = ranger.PositionY;

                for (int by = (ranger.PositionY - 1 >= 0 ? ranger.PositionY - 1 : 0); by <= (ranger.PositionY + 1 < GetSize ? ranger.PositionY + 1 : GetSize - 1); by++)
                {
                    for (int bx = (ranger.PositionX - 1 >= 0 ? ranger.PositionX - 1 : 0); bx <= (ranger.PositionX + 1 < GetSize ? ranger.PositionX + 1 : GetSize - 1); bx++)
                    {
                        if (_table.GetTableValue(bx, by) == 1)
                        {
                            IsGameOver = true; OnGameOver();
                        }
                    }
                }


                int toX = e.x;
                int toY = e.y;
                if (toX < 0 || toX >= GetSize || toY < 0 || toY >= GetSize || _table.GetTableValue(toX, toY) == 1 || _table.GetTableValue(toX, toY) == 2 || _table.GetTableValue(toX, toY) == 3)
                {
                    ranger.ChangeDirection();
                    //ranger.WouldStep();
                    ranger.WouldStepNoEvent();
                    _table.SetTableValue(X, Y, 0);
                    _table.SetTableValue(ranger.RtoX, ranger.RtoY, 4);
                    ranger.StepTo(ranger.RtoX, ranger.RtoY);
                }
                else
                {
                    _table.SetTableValue(X, Y, 0);
                    _table.SetTableValue(toX, toY, 4);
                    ranger.StepTo(e.x, e.y);
                }

            }
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new MaciEventArgs(false));
        }

        public virtual void OnGameOver()
        {
            GameOver?.Invoke(this, new MaciEventArgs(true));
        }

        public void LoadGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _table = _dataAccess.LoadMaciTable(path);
            _size = _table.GetSize;
            //_gameStepCount = 0;
            _isGameOver = false;
            AddRanger();
            CountBaskets();
            AllBaskets=Baskets;
        }


    }
}