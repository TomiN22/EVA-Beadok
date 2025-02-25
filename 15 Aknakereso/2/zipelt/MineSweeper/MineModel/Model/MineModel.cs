using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistance;

namespace Model
{
    public class MineModel
    {
        private MineTable _table;
        private int _gameStepCount;
        private int _size;
        private bool _isGameOver = false;
        private IMineDataAccess _dataAccess;

        public event EventHandler<MineEventArgs>? GameOver;

        public MineModel(IMineDataAccess dataAccess, int size)
        {
            _size = size;
            _dataAccess = dataAccess;
            _table = new MineTable(size);
        }


        public int GetSize { get { return _size; } }

        public bool IsGameOver => _isGameOver;

        public MineTable Table { get { return _table; } }
        public int GameStepCount { get { return _gameStepCount; } }



        public void NextMine()
        {
            for(int i=0; i<_table.GetSize; i++)
            {
                for(int j=0; j<_table.GetSize; j++)
                {
                    if (_table.GetValue(i, j) == -1)
                    {
                        for(int k=i-1; k<=i+1; k++)
                        {
                            for(int l=j-1; l<=j+1; l++)
                            {
                                if(k>=0 && k<_table.GetSize && l>=0 && l<_table.GetSize && _table.GetValue(k,l) < 9 && _table.GetValue(k,l) != -1)
                                {
                                    _table.SetValue(k, l, _table.GetValue(k,l)+1);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ShowAll(int x, int y)
        {
            if (!(_table.IsOpened(x, y)))
            {
                _table.SetToOpened(x, y);

                if (_table.GetValue(x, y) == 0)
                {
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            if (i >= 0 && i < _table.GetSize && j >= 0 && j < _table.GetSize)
                            {
                                ShowAll(i, j);
                            }

                        }
                    }
                }
            }
            
        }

        public void Step(int x, int y)
        {
            if (!_isGameOver)
            {
                _table.ChangePlayer();
            }

            if (IsDraw())
            {
                _isGameOver = true;

                OnGameOver(Win.Draw);
            }

            else if (_table.GetValue(x, y) == -1)
            {
                _isGameOver = true;
                
                OnGameOver(_table.GetPlayer == 1 ? Win.Player1 : Win.Player2);
            }


        }
        public void GenerateFields()
        {
            Random random = new Random();

            for (int i = 0; i <_table.GetMines; i++)
            {
                int x, y;

                do
                {
                    x = random.Next(_table.GetSize);
                    y = random.Next(_table.GetSize);
                }
                while (_table.GetValue(x,y) != 0);

                _table.SetValue(x, y, -1);
            }
            NextMine();
        }

        public bool IsDraw()
        {
            int _counter = 0;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_table.IsOpened(i, j))
                    {
                        _counter++;
                    }
                }
            }

            if (_counter == ((_size*_size)-_table.GetMines))
                return true;
            else
                return false;
        }

        public void OnGameOver(Win winner)
        {
            GameOver?.Invoke(this, new MineEventArgs(true, winner, _table.GetPlayer));
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
            _size = _table.GetSize; //
            _gameStepCount = 0;
            _isGameOver = false;
        }


    }
}


/*
 * for (int k = 0; k < _table.GetSize; k++)
            {
                for (int l = 0; l < _table.GetSize; l++)
                {
                    if (_table.GetValue(x, y) == -1)
                    {
                        if (k > 0 && k<_table.GetSize-1 && y>0 && y<_table.GetSize-1 && _table.GetValue(x,y) == -1)
                        {
                            _table.SetValue(k-1, y-1, _table.GetValue(x,y)+1);
                            _table.SetValue(k-1, y, _table.GetValue(x, y) + 1);
                            _table.SetValue(k-1, y+1, _table.GetValue(x, y) + 1);

                            _table.SetValue(k, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(k, y+1, _table.GetValue(x, y) + 1);

                            _table.SetValue(k+1, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(k+1, y, _table.GetValue(x, y) + 1);
                            _table.SetValue(k+1, y+1, _table.GetValue(x, y) + 1);
                        }
                        else if (k==0 && y==0 && _table.GetValue(x, y) == -1)
                        { 
                            _table.SetValue(x, y+1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x+1, y, _table.GetValue(x, y) + 1);
                            _table.SetValue(x+1, y+1, _table.GetValue(x, y) + 1);
                        }
                        else if(k==0 && y==_table.GetSize-1 && _table.GetValue(x, y) == -1)
                        {
                            _table.SetValue(x, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x+1, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x+1, y, _table.GetValue(x, y) + 1);
                        }
                        else if(k==_table.GetSize-1 && y==0 && _table.GetValue(x, y) == -1)
                        {
                            _table.SetValue(x-1, y, _table.GetValue(x,y) + 1);
                            _table.SetValue(x-1, y+1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x, y+1, _table.GetValue(x, y) + 1);
                        }
                        else if(k==_table.GetSize-1 && y==_table.GetSize-1 && _table.GetValue(x, y) == -1)
                        {
                            _table.SetValue(x-1, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x-1, y, _table.GetValue(x, y) + 1);
                            _table.SetValue(x, y-1, _table.GetValue(x, y) + 1);
                        }
                        else if(k>0 && k<_table.GetSize-1 && y==0 && _table.GetValue(x, y) == -1)
                        {
                            _table.SetValue(x-1, y, _table.GetValue(x, y) + 1);
                            _table.SetValue(x-1, y+1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x, y+1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x+1, y, _table.GetValue(x, y) + 1);
                            _table.SetValue(x+1, y+1, _table.GetValue(x, y) + 1);
                        }
                        else if(k>0 && k<_table.GetSize-1 && y==_table.GetSize-1 && _table.GetValue(x, y) == -1)
                        {
                            _table.SetValue(x-1, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x-1, y, _table.GetValue(x, y) + 1);
                            _table.SetValue(x, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x+1, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x-1, y, _table.GetValue(x, y) + 1);
                        }
                        else if(k==0 && y>0 && y<_table.GetSize-1 && _table.GetValue(x,y) == -1)
                        {
                            _table.SetValue(x, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x, y+1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x+1, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x+1, y, _table.GetValue(x, y) + 1);
                            _table.SetValue(x+1, y, _table.GetValue(x, y) + 1);
                        }
                        else if(k==_table.GetSize-1 && y>0 && y<_table.GetSize-1 && _table.GetValue(x, y) == -1)
                        {
                            _table.SetValue(x, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x, y+1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x-1, y-1, _table.GetValue(x, y) + 1);
                            _table.SetValue(x-1, y, _table.GetValue(x, y) + 1);
                            _table.SetValue(x-1, y+1, _table.GetValue(x, y) + 1);
                        }
                    }
                }
            }*/