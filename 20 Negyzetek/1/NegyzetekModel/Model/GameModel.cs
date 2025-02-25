using NegyzetekModel.Model;
using NegyzetekModel.Persistence;
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
using static NegyzetekModel.Model.Figure;
using static NegyzetekModel.Persistence.Table;
using static System.Reflection.Metadata.BlobBuilder;

namespace NegyzetekModel.Model
{
    public class GameModel
    {
        private Table _table;
        private readonly INegyzetekDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;

        public event EventHandler<NegyzetekEventArgs>? GameAdvanced;
        public event EventHandler<NegyzetekEventArgs>? GameOver;

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public int CurrentFigureIndex { get; set; }

        public bool HasColored { get; set; }

        public List<Figure> Figures { get; set; }

        public GameModel(INegyzetekDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new Table(size);
            CurrentFigureIndex = 1;
            HasColored = false;
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
                    if(i % 2 == 0 && j % 2 == 0)
                    {
                        Table.SetTableValue(i, j, 4);
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
                    HasColored = false;
                    if (Table.GetTableValue(x, y) == 4 && Table.P1Current[0] != -1 && Table.P1Current[1] != -1)
                    {
                        CheckDraw(x, y, Table.P1Current[0], Table.P1Current[1]);
                        Table.P1Current[0] = -1; Table.P1Current[1] = -1;
                        CheckCanMove();
                    }
                    else if (Table.GetTableValue(x, y) == 4)
                    {
                        Table.P1Current[0] = x; Table.P1Current[1] = y;
                    }
                }
                else if(Table.Player == 2)
                {
                    HasColored = false;
                    if (Table.GetTableValue(x, y) == 4 && Table.P2Current[0] != -1 && Table.P2Current[1] != -1)
                    {
                        CheckDraw(x, y, Table.P2Current[0], Table.P2Current[1]);
                        Table.P2Current[0] = -1; Table.P2Current[1] = -1;
                        CheckCanMove();
                    }
                    else if (Table.GetTableValue(x, y) == 4)
                    {
                        Table.P2Current[0] = x; Table.P2Current[1] = y;
                    }
                }
            }
        }

        private void CheckDraw(int x, int y, int pCurrent0, int pCurrent1)
        {
            if (x == pCurrent0 && y == pCurrent1-2 && Table.GetTableValue(pCurrent0, pCurrent1-1) == 0)
            {
                Table.SetTableValue(pCurrent0, pCurrent1-1, 3);
                CheckNeighbours(pCurrent0, pCurrent1-1, 2);
            }
            else if (x == pCurrent0 && y == pCurrent1+2 && Table.GetTableValue(pCurrent0, pCurrent1+1) == 0)
            {
                Table.SetTableValue(pCurrent0, pCurrent1+1, 3);
                CheckNeighbours(pCurrent0, pCurrent1+1, 2);
            }
            else if (x == pCurrent0-2 && y == pCurrent1 && Table.GetTableValue(pCurrent0-1, pCurrent1) == 0)
            {
                Table.SetTableValue(pCurrent0-1, pCurrent1, 3);
                CheckNeighbours(pCurrent0-1, pCurrent1, 1);
            }
            else if (x == pCurrent0+2 && y == pCurrent1 && Table.GetTableValue(pCurrent0+1, pCurrent1) == 0)
            {
                Table.SetTableValue(pCurrent0+1, pCurrent1, 3);
                CheckNeighbours(pCurrent0+1, pCurrent1, 1);
            }
        }

        private void CheckNeighbours(int x, int y, int dir)
        {
            if(dir == 1)
            {
                if (y-1 >= 0)
                {
                    CheckSurrounding(x, y-1);
                }
                if (y+1 < GetSize)
                {
                    CheckSurrounding(x ,y+1);
                }
            }
            else if(dir == 2)
            {
                if (x-1 >= 0)
                {
                    CheckSurrounding(x-1, y);
                }
                if (x+1 < GetSize)
                {
                    CheckSurrounding(x+1, y);
                }
            }
            if (!HasColored)
                Table.ChangePlayer();
        }

        private void CheckSurrounding(int x, int y)
        {
            int c = 0;
            
            if (Table.GetTableValue(x-1, y) == 3)
                c++;

            if (Table.GetTableValue(x+1, y) == 3)
                c++;
            
            if (Table.GetTableValue(x, y-1) == 3)
                c++;

            if (Table.GetTableValue(x, y+1) == 3)
                c++;

            if (c == 4)
            {
                Table.SetTableValue(x, y, Table.Player);
                HasColored = true;
            }
        }

        private void CheckCanMove()
        {
            bool hasFilled = true;
            Table.P1Count = 0; Table.P2Count = 0;
            for (int i = 0; i < GetSize; i++)
            {
                for (int j = 0; j < GetSize; j++)
                {
                    if(i % 2 != 0 && j % 2 != 0)
                    {
                        if(Table.GetTableValue(i, j) == 0)
                        {
                            hasFilled = false;
                        }
                        else if(Table.GetTableValue(i, j) == 1)
                        {
                            Table.P1Count++;
                        }
                        else if (Table.GetTableValue(i, j) == 2)
                        {
                            Table.P2Count++;
                        }
                    }
                }
            }

            if (hasFilled)
            {
                IsGameOver = true;
                if (Table.P1Count > Table.P2Count)
                    OnGameOver(Win.Player1);
                else if (Table.P1Count < Table.P2Count)
                    OnGameOver(Win.Player2);
                else
                    OnGameOver(Win.Draw);
            }
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new NegyzetekEventArgs());
        }

        public virtual void OnGameOver(Win player)
        {
            GameOver?.Invoke(this, new NegyzetekEventArgs(player));
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

        //    _table = _dataAccess.LoadNegyzetekTable(path);
        //    _size = _table.GetSize;
        //    //_gameStepCount = 0;
        //    _isGameOver = false;

        //}


    }
}