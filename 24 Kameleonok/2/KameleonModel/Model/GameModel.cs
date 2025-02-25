using KameleonModel.Model;
using KameleonModel.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static KameleonModel.Model.Figure;
using static KameleonModel.Persistence.Table;
using static System.Reflection.Metadata.BlobBuilder;

namespace KameleonModel.Model
{
    public class GameModel
    {
        private Table _table;
        private readonly IKameleonDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;

        public event EventHandler<KameleonEventArgs>? GameAdvanced;
        public event EventHandler<KameleonEventArgs>? GameOver;

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public int CurrentFigureIndex { get; set; }

        public bool HasTakenDown { get; set; }

        public List<Figure> Figures { get; set; }

        public GameModel(IKameleonDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new Table(size);
            CurrentFigureIndex = 1;
            HasTakenDown = false;
            Figures = new List<Figure>();
            //GenerateFields();
        }

        public Table Table { get { return _table; } }

        public void GenerateFields()
        {
            for (int i = 0; i < GetSize; i++)
            {
                for (int j = 0; j < GetSize; j++)
                {
                    if(i == 0)
                    {
                        Table.SetTableValue(i, j, 2);
                        Table.SetTableColor(i, j, 2);
                    }
                    if (i == GetSize-1)
                    {
                        Table.SetTableValue(i, j, 1);
                        Table.SetTableColor(i, j, 1);
                    }
                    if(i < GetSize-1 && j == 0)
                    {
                        Table.SetTableValue(i, j, 2);
                        Table.SetTableColor(i, j, 2);
                    }
                    if (i > 0 && j == GetSize-1)
                    {
                        Table.SetTableValue(i, j, 1);
                        Table.SetTableColor(i, j, 1);
                    }
                    if(i == GetSize-2 && j > 0 && j < GetSize-1)
                    {
                        Table.SetTableValue(i, j, 2);
                        Table.SetTableColor(i, j, 2);
                    }
                    if (i == 1 && j > 0 && j < GetSize-1)
                    {
                        Table.SetTableValue(i, j, 1);
                        Table.SetTableColor(i, j, 1);
                    }
                    if (i == (GetSize-1)/2 && j == 1)
                    {
                        Table.SetTableValue(i, j, 1);
                        Table.SetTableColor(i, j, 1);
                    }
                    if (i == (GetSize-1)/2 && j == GetSize-2)
                    {
                        Table.SetTableValue(i, j, 2);
                        Table.SetTableColor(i, j, 2);
                    }
                    if( i == (GetSize-1)/2 && j == (GetSize-1)/2)
                    {
                        Table.SetTableValue(i, j, 0);
                        Table.SetTableColor(i, j, 0);
                    }

                    if (GetSize>5)
                    {
                        if(i > 1 && i < GetSize-2 && j == GetSize-2)
                        {
                            Table.SetTableValue(i, j, 2);
                            Table.SetTableColor(i, j, 2);
                        }
                        if(i > 1 && i < GetSize-2 && j == 1)
                        {
                            Table.SetTableValue(i, j, 1);
                            Table.SetTableColor(i, j, 1);
                        }
                        if (i == 2 && j >= 2 && j <= GetSize-3)
                        {
                            Table.SetTableValue(i, j, 2);
                            Table.SetTableColor(i, j, 2);
                        }
                        if (i == GetSize-3 && j >= 2 && j <= GetSize-3)
                        {
                            Table.SetTableValue(i, j, 1);
                            Table.SetTableColor(i, j, 1);
                        }
                        if(i == 3 && j == 2)
                        {
                            Table.SetTableValue(i, j, 2);
                            Table.SetTableColor(i, j, 2);
                        }
                        if (i == 3 && j == GetSize-3)
                        {
                            Table.SetTableValue(i, j, 1);
                            Table.SetTableColor(i, j, 1);
                        }
                    }
                }
            }
        }

        public void SetUpTable()
        {

        }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            if(Table.Player == 1)
                Table.P1Time++;
            if(Table.Player == 2)
                Table.P2Time++;

            OnGameAdvanced();
        }

        public void Step(int x, int y)
        {
            if (!IsGameOver)
            {
                if(Table.Player == 1)
                {
                    if(Table.GetTableValue(x, y) == 1)
                    {
                        Table.P1Current[0] = x; Table.P1Current[1] = y;
                    }
                    else if(Table.GetTableValue(x, y) == 0)
                    {
                        bool boo = false;
                        if ((x-1 == Table.P1Current[0] && y == Table.P1Current[1]) || (x+1 == Table.P1Current[0] && y == Table.P1Current[1])
                            || (x == Table.P1Current[0] && y-1 == Table.P1Current[1]) || (x == Table.P1Current[0] && y+1 == Table.P1Current[1]))
                        {
                            Table.SetTableValue(Table.P1Current[0], Table.P1Current[1], 0);
                            Table.SetTableValue(x, y, 1);
                            Table.ChangePlayer();
                            boo = true;
                        }
                        //jump
                        else if (x-2 == Table.P1Current[0] && y == Table.P1Current[1])
                        {
                            SetP1Jump(x-1, y);
                            Table.SetTableValue(x, y, 1);
                            boo = true;
                        }
                        else if (x+2 == Table.P1Current[0] && y == Table.P1Current[1])
                        {
                            SetP1Jump(x+1, y);
                            Table.SetTableValue(x, y, 1);
                            boo = true;
                        }
                        else if (x == Table.P1Current[0] && y-2 == Table.P1Current[1])
                        {
                            SetP1Jump(x, y-1);
                            Table.SetTableValue(x, y, 1);
                            boo = true;
                        }
                        else if (x == Table.P1Current[0] && y+2 == Table.P1Current[1])
                        {
                            SetP1Jump(x, y+1);
                            Table.SetTableValue(x, y, 1);
                            boo = true;
                        }

                        if (boo)
                        {
                            for (int i = 0; i < GetSize; i++)
                            {
                                for (int j = 0; j < GetSize; j++)
                                {
                                    if (Table.GetTableColor(i, j)==1 && Table.GetTableValue(i, j)==2)
                                    {
                                        Table.SetTableValue(i, j, 1);
                                        Table.P2Count--;
                                        Table.P1Count++;
                                    }
                                }
                            }
                        }
                    }
                }
                else if(Table.Player == 2)
                {
                    if (Table.GetTableValue(x, y) == 2)
                    {
                        Table.P2Current[0] = x; Table.P2Current[1] = y;
                    }
                    else if (Table.GetTableValue(x, y) == 0)
                    {
                        bool boo = false;
                        if ((x-1 == Table.P2Current[0] && y == Table.P2Current[1]) || (x+1 == Table.P2Current[0] && y == Table.P2Current[1])
                            || (x == Table.P2Current[0] && y-1 == Table.P2Current[1]) || (x == Table.P2Current[0] && y+1 == Table.P2Current[1]))
                        {
                            Table.SetTableValue(Table.P2Current[0], Table.P2Current[1], 0);
                            Table.SetTableValue(x, y, 2);
                            Table.ChangePlayer();
                            boo = true;
                        }
                        //jump
                        else if (x-2 == Table.P2Current[0] && y == Table.P2Current[1])
                        {
                            SetP2Jump(x-1, y);
                            Table.SetTableValue(x, y, 2);
                            boo = true;
                        }
                        else if (x+2 == Table.P2Current[0] && y == Table.P2Current[1])
                        {
                            SetP2Jump(x+1, y);
                            Table.SetTableValue(x, y, 2);
                            boo = true;
                        }
                        else if (x == Table.P2Current[0] && y-2 == Table.P2Current[1])
                        {
                            SetP2Jump(x, y-1);
                            Table.SetTableValue(x, y, 2);
                            boo = true;
                        }
                        else if (x == Table.P2Current[0] && y+2 == Table.P2Current[1])
                        {
                            SetP2Jump(x, y+1);
                            Table.SetTableValue(x, y, 2);
                            boo = true;
                        }

                        if (boo)
                        {
                            for (int i = 0; i < GetSize; i++)
                            {
                                for (int j = 0; j < GetSize; j++)
                                {
                                    if(Table.GetTableColor(i,j)==2 && Table.GetTableValue(i, j)==1)
                                    {
                                        Table.SetTableValue(i, j, 2);
                                        Table.P1Count--;
                                        Table.P2Count++;
                                    }
                                }
                            }
                        }
                    }
                }
                CHeckCounts();
            }
        }

        private void SetP1Jump(int x, int y)
        {
            Table.P2Count--;
            Table.SetTableValue(Table.P1Current[0], Table.P1Current[1], 0);
            Table.SetTableValue(x, y, 0);
            Table.P1Current[0] = -2; Table.P1Current[1] = -2;
            Table.ChangePlayer();
        }

        private void SetP2Jump(int x, int y)
        {
            Table.P1Count--;
            Table.SetTableValue(Table.P2Current[0], Table.P2Current[1], 0);
            Table.SetTableValue(x, y, 0);
            Table.P2Current[0] = -2; Table.P2Current[1] = -2;
            Table.ChangePlayer();
        }

        private void CHeckCounts()
        {
            if(Table.P1Count == 0)
            {
                IsGameOver = false; OnGameOver(Win.Player2);
            }
            else if(Table.P2Count == 0)
            {
                IsGameOver = false; OnGameOver(Win.Player1);
            }
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new KameleonEventArgs());
        }

        public virtual void OnGameOver(Win player)
        {
            GameOver?.Invoke(this, new KameleonEventArgs(player));
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
            _isGameOver = false;
            //SetUpTable();
        }

        //public void LoadGame(String path)
        //{
        //    if (_dataAccess == null)
        //        throw new InvalidOperationException("No data access is provided.");

        //    _table = _dataAccess.LoadKameleonTable(path);
        //    _size = _table.GetSize;
        //    //_gameStepCount = 0;
        //    _isGameOver = false;

        //}


    }
}