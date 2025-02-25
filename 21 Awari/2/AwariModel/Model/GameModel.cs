using AwariModel.Model;
using AwariModel.Persistence;
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
using static AwariModel.Model.Figure;
using static AwariModel.Persistence.Table;
using static System.Reflection.Metadata.BlobBuilder;

namespace AwariModel.Model
{
    public class GameModel
    {
        private Table _table;
        private readonly IAwariDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;

        public event EventHandler<AwariEventArgs>? GameAdvanced;
        public event EventHandler<AwariEventArgs>? GameOver;

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public int TurnedCounter { get; set; }

        public bool CanTurnAgain { get; set; }

        public List<(int, int)> BlueBowls { get; set; }
        public List<(int, int)> RedBowls { get; set; }

        public GameModel(IAwariDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new Table(size);
            TurnedCounter = 0;
            CanTurnAgain = false;
            BlueBowls = new List<(int, int)>();
            RedBowls = new List<(int, int)>();
            //GenerateFields();
        }

        public Table Table { get { return _table; } }

        public void GenerateFields()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < GetSize; j++)
                {
                    if((i == 1 && j == 0) || (i == 0 && j == GetSize-1))
                    {
                        Table.SetTableValue(i, j, -1);
                    }
                    if(i == 0 && Table.GetTableValue(i, j) != -1)
                    {
                        Table.SetTableValue(i, j, 1);
                        if(j != 0)
                            Table.SetTableNum(i, j, 6);
                    }
                    if(i == 1 && Table.GetTableValue(i, j) != -1)
                    {
                        Table.SetTableValue(i, j, 2);
                        if(j != GetSize-1)
                            Table.SetTableNum(i, j, 6);
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
                    CanTurnAgain = false;
                    if (y != 0 && Table.GetTableValue(x, y) == 1)
                    {
                        int inHand = 0;
                        Table.P1Current[0] = x; Table.P1Current[1] = y;
                        inHand = Table.GetTableNum(x, y);
                        Table.SetTableNum(x, y, 0);
                        for (int i = y-1; i >= 0 && inHand > 0; i--)
                        {
                            if (inHand == 1 && i == 0)
                            {
                                CanTurnAgain = true;
                                TurnedCounter++;
                            }
                            Table.SetTableNum(x, i, Table.GetTableNum(x, i)+1);
                            inHand--;
                        }
                        while (inHand > 0)
                        {
                            for (int i = 1; i < GetSize && inHand > 0; i++)
                            {
                                Table.SetTableNum(x+1, i, Table.GetTableNum(x+1, i)+1);
                                inHand--;
                            }
                            for (int i = GetSize-2; i >= 0 && inHand > 0; i--)
                            {
                                if (inHand == 1 && i == 0)
                                {
                                    CanTurnAgain = true;
                                    TurnedCounter++;
                                }
                                if (i != y)
                                {
                                    Table.SetTableNum(x, i, Table.GetTableNum(x, i)+1);
                                    inHand--;
                                }
                                if(i == y && inHand == 1)
                                {
                                    Table.SetTableNum(0, 0, Table.GetTableNum(x, i)+1);
                                    inHand--;
                                    Table.SetTableNum(0, 0, Table.GetTableNum(0,0)+Table.GetTableNum(x+1, i));
                                    Table.SetTableNum(x+1, i, 0);
                                }
                            }
                        }
                        if (TurnedCounter > 1)
                        {
                            TurnedCounter = 0;
                            CanTurnAgain = false;
                        }
                        if (CanTurnAgain == false)
                            Table.ChangePlayer();
                        
                    }
                }
                else if(Table.Player == 2)
                {
                    CanTurnAgain = false;
                    if (y != GetSize-1 && Table.GetTableValue(x, y) == 2)
                    {
                        int inHand = 0;
                        Table.P2Current[0] = x; Table.P2Current[1] = y;
                        inHand = Table.GetTableNum(x, y);
                        Table.SetTableNum(x, y, 0);
                        for (int i = y+1; i < GetSize && inHand > 0; i++)
                        {
                            if (inHand == 1 && i == GetSize-1)
                            {
                                CanTurnAgain = true;
                                TurnedCounter++;
                            }
                            Table.SetTableNum(x, i, Table.GetTableNum(x, i)+1);
                            inHand--;
                        }
                        while (inHand > 0)
                        {
                            for (int i = GetSize-2; i >= 0 && inHand > 0; i--)
                            {
                                Table.SetTableNum(x-1, i, Table.GetTableNum(x-1, i)+1);
                                inHand--;
                            }
                            for (int i = 1; i < GetSize && inHand > 0; i++)
                            {
                                if (inHand == 1 && i == GetSize-1)
                                {
                                    CanTurnAgain = true;
                                    TurnedCounter++;
                                }
                                if (i != y)
                                {
                                    Table.SetTableNum(x, i, Table.GetTableNum(x, i)+1);
                                    inHand--;
                                }
                                if (i == y && inHand == 1)
                                {
                                    Table.SetTableNum(1, GetSize-1, Table.GetTableNum(x, i)+1);
                                    inHand--;
                                    Table.SetTableNum(1, GetSize-1, Table.GetTableNum(0, 0)+Table.GetTableNum(x-1, i));
                                    Table.SetTableNum(x-1, i, 0);
                                }
                            }
                        }
                        if (TurnedCounter > 1)
                        {
                            TurnedCounter = 0;
                            CanTurnAgain = false;
                        }
                        if (CanTurnAgain == false)
                            Table.ChangePlayer();
                    }
                }
                CheckEpmty();
            }
        }

        private void CheckEpmty()
        {
            int c1 = 0;
            int c2 = 0;
            for (int i = 1; i < GetSize-1; i++)
            {
                if (Table.GetTableNum(0, i) == 0)
                {
                    c1++;
                }
                if (Table.GetTableNum(1, i) == 0)
                {
                    c2++;
                }
            }

            if (c1 == GetSize/2 || c2 == GetSize/2)
            {
                IsGameOver = true;
                if (Table.GetTableNum(0, 0) > Table.GetTableNum(1, GetSize-1))
                    OnGameOver(Win.Player1);
                if (Table.GetTableNum(0, 0) < Table.GetTableNum(1, GetSize-1))
                    OnGameOver(Win.Player2);
                if (Table.GetTableNum(0, 0) == Table.GetTableNum(1, GetSize-1))
                    OnGameOver(Win.Draw);
            }
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new AwariEventArgs());
        }

        public virtual void OnGameOver(Win player)
        {
            GameOver?.Invoke(this, new AwariEventArgs(player));
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

        //    _table = _dataAccess.LoadAwariTable(path);
        //    _size = _table.GetSize;
        //    //_gameStepCount = 0;
        //    _isGameOver = false;

        //}


    }
}