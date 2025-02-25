using PotyogosAmobaModel.Model;
using PotyogosAmobaModel.Persistence;
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
using static PotyogosAmobaModel.Model.Figure;
using static PotyogosAmobaModel.Persistence.Table;
using static System.Reflection.Metadata.BlobBuilder;

namespace PotyogosAmobaModel.Model
{
    public class GameModel
    {
        private Table _table;
        private readonly IPotyogosAmobaDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;

        public event EventHandler<PotyogosAmobaEventArgs>? GameAdvanced;
        public event EventHandler<PotyogosAmobaEventArgs>? GameOver;

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public int CurrentFigureIndex { get; set; }

        public bool HasTakenDown { get; set; }

        public List<Figure> Figures { get; set; }

        public GameModel(IPotyogosAmobaDataAccess? dataAccess, int size)
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
                    Table.SetTableValue(i, j, 0);
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

        public void Step(int y)
        {
            if (!IsGameOver)
            {
                if(Table.Player == 1)
                {
                    PlaceFigure(y, 1);
                }
                else if(Table.Player == 2)
                {
                    PlaceFigure(y, 2);
                }
            }
        }

        private void PlaceFigure(int y, int tableValue)
        {
            int x = Table.GetSize-1;
            while (Table.GetTableValue(x, y)!=0 && x>0)
            {
                x--;
            }
            if(Table.GetTableValue(x, y) == 0)
            {
                Table.SetTableValue(x, y, tableValue);
                Table.ChangePlayer();
                CheckDirections(x,y,tableValue);
            }
                
        }

        private void CheckDirections(int x, int y, int tableValue)
        {
            int counter = 0;
            int i = y-1;
            int j;
            if (!IsGameOver)
            {
                while (i > 0 && Table.GetTableValue(x, i)==tableValue)
                {
                    counter++;
                    i--;
                }
                if(counter == 2 && y+1 < Table.GetSize && Table.GetTableValue(x, y+1)==tableValue)
                {
                    Table.SetHasColor(x, y+1, true);
                    counter++;
                }
                if (counter == 3)
                {
                    i = y-1;
                    Table.SetHasColor(x, y, true);
                    while (i > 0 && Table.GetTableValue(x, i)==tableValue)
                    {
                        Table.SetHasColor(x, i, true);
                        i--;
                    }
                    SetGameOver();
                }
                CheckFull();
            }
            if (!IsGameOver)
            {
                i = y+1; counter = 0;
                while (i < Table.GetSize && Table.GetTableValue(x, i)==tableValue)
                {
                    counter++;
                    i++;
                }
                if (counter == 2 && y-1 > 0 && Table.GetTableValue(x, y-1)==tableValue)
                {
                    Table.SetHasColor(x, y-1, true);
                    counter++;
                }
                if (counter == 3)
                {
                    i = y+1;
                    Table.SetHasColor(x, y, true);
                    while (i < Table.GetSize && Table.GetTableValue(x, i)==tableValue)
                    {
                        Table.SetHasColor(x, i, true);
                        i++;
                    }
                    SetGameOver();
                }
                CheckFull();
            }
            if (!IsGameOver)
            {
                //i = x-1; counter = 0;
                //while (i > 0 && Table.GetTableValue(i, y)==tableValue)
                //{
                //    counter++;
                //    i--;
                //}
                //if (counter == 3)
                //{
                //    i = x-1;
                //    Table.SetHasColor(x, y, true);
                //    while (i > 0 && Table.GetTableValue(i, y)==tableValue)
                //    {
                //        Table.SetHasColor(i, y, true);
                //        i--;
                //    }
                //    SetGameOver();
                //}
            }
            if (!IsGameOver)
            {
                i = x+1; counter = 0;
                while (i < Table.GetSize && Table.GetTableValue(i, y)==tableValue)
                {
                    counter++;
                    i++;
                }
                if(counter == 3)
                {
                    i = x+1;
                    Table.SetHasColor(x, y, true);
                    while (i < Table.GetSize && Table.GetTableValue(i, y)==tableValue)
                    {
                        Table.SetHasColor(i, y, true);
                        i++;
                    }
                    SetGameOver();
                }
                CheckFull();
            }
            if (!IsGameOver)
            {
                i = x-1; j = y-1; counter = 0;
                while (i > 0 && j > 0 && Table.GetTableValue(i, j)==tableValue)
                {
                    counter++;
                    i--; j--;
                }
                if (counter == 2 && x+1 < Table.GetSize && y+1 < Table.GetSize && Table.GetTableValue(x+1, y+1)==tableValue)
                {
                    Table.SetHasColor(x+1, y+1, true);
                    counter++;
                }
                if (counter == 3)
                {
                    i = x-1; j = y-1;
                    Table.SetHasColor(x, y, true);
                    while (i > 0 && j > 0 && Table.GetTableValue(i, j)==tableValue)
                    {
                        Table.SetHasColor(i, j, true);
                        i--; j--;
                    }
                    SetGameOver();
                }
                CheckFull();
            }
            if (!IsGameOver)
            {
                i = x-1; j = y+1; counter = 0;
                while (i > 0 && j < Table.GetSize && Table.GetTableValue(i, j)==tableValue)
                {
                    counter++;
                    i--; j++;
                }
                if (counter == 2 && x+1 < Table.GetSize && y-1 > 0 && Table.GetTableValue(x+1, y-1)==tableValue)
                {
                    Table.SetHasColor(x+1, y-1, true);
                    counter++;
                }
                if (counter == 3)
                {
                    i = x-1; j = y+1;
                    Table.SetHasColor(x, y, true);
                    while (i > 0 && j < Table.GetSize && Table.GetTableValue(i, j)==tableValue)
                    {
                        Table.SetHasColor(i, j, true);
                        i--; j++;
                    }
                    SetGameOver();
                }
                CheckFull();
            }
            if (!IsGameOver)
            {
                i = x+1; j = y-1; counter = 0;
                while (i < Table.GetSize && j > 0 && Table.GetTableValue(i, j)==tableValue)
                {
                    counter++;
                    i++; j--;
                }
                if (counter == 2 && x-1 > 0 && y+1 < Table.GetSize && Table.GetTableValue(x-1, y+1)==tableValue)
                {
                    Table.SetHasColor(x-1, y+1, true);
                    counter++;
                }
                if (counter == 3)
                {
                    i = x+1; j = y-1;
                    Table.SetHasColor(x, y, true);
                    while (i < Table.GetSize && j > 0 && Table.GetTableValue(i, j)==tableValue)
                    {
                        Table.SetHasColor(i, j, true);
                        i++; j--;
                    }
                    SetGameOver();
                }
                CheckFull();
            }
            if (!IsGameOver)
            {
                i = x+1; j = y+1; counter = 0;
                while (i < Table.GetSize && j < Table.GetSize && Table.GetTableValue(i, j)==tableValue)
                {
                    counter++;
                    i++; j++;
                }
                if (counter == 2 && x-1 >= 0 && y-1 >= 0 && Table.GetTableValue(x-1, y-1)==tableValue)
                {
                    Table.SetHasColor(x-1, y-1, true);
                    counter++;
                }
                if (counter == 3)
                {
                    i = x+1; j = y+1;
                    Table.SetHasColor(x, y, true);
                    while (i < Table.GetSize && j < Table.GetSize && Table.GetTableValue(i, j)==tableValue)
                    {
                        Table.SetHasColor(i, j, true);
                        i++; j++;
                    }
                    SetGameOver();
                }
                CheckFull();
            }
        }

        private void CheckFull()
        {
            int fullCounter = 0;
            if (!IsGameOver)
            {
                for (int i = 0; i < Table.GetSize; i++)
                {
                    for (int j = 0; j < Table.GetSize; j++)
                    {
                        if (Table.GetTableValue(i, j)!=0)
                            fullCounter++;
                    }
                }
            }
            if (fullCounter == Table.GetSize*Table.GetSize)
            {
                IsGameOver = true; OnGameOver(Win.Draw);
            }
        }

        private void CheckSetVHPatterns()
        {

        }

        private void CheckSetDiagonalPatterns()
        {

        }

        private void SetGameOver()
        {
            IsGameOver = true;
            if (Table.Player == 1)
                OnGameOver(Win.Player1);
            else if (Table.Player == 2)
                OnGameOver(Win.Player2);
        }

        private void SetWinnerCells()
        {

        }

        private void SetP1HitWithCircle(int x, int y)
        {
            Table.P2Count--;
            Table.SetTableValue(Table.P1Current[0], Table.P1Current[1], 1);
            Table.SetTableValue(x, y, 3);
            Table.P1Current[0] = -2; Table.P1Current[1] = -2;
            Table.ChangePlayer();
        }

        private void SetP2HitWithCircle(int x, int y)
        {
            Table.P1Count--;
            Table.SetTableValue(Table.P2Current[0], Table.P2Current[1], 1);
            Table.SetTableValue(x, y, 4);
            Table.P2Current[0] = -2; Table.P2Current[1] = -2;
            Table.ChangePlayer();
        }

        private void SetP1HitWithTriangle(int x, int y)
        {
            Table.P2Count--;
            Table.SetTableValue(Table.P1Current[0], Table.P1Current[1], 1);
            Table.SetTableValue(x, y, 5);
            Table.P1Current[0] = -2; Table.P1Current[1] = -2;
            Table.ChangePlayer();
        }

        private void SetP2HitWithTriangle(int x, int y)
        {
            Table.P1Count--;
            Table.SetTableValue(Table.P2Current[0], Table.P2Current[1], 1);
            Table.SetTableValue(x, y, 6);
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
            GameAdvanced?.Invoke(this, new PotyogosAmobaEventArgs());
        }

        public virtual void OnGameOver(Win player)
        {
            GameOver?.Invoke(this, new PotyogosAmobaEventArgs(player));
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

        //public void LoadGame(String path) //async Taks ????????? or void? but cannot wait void...........
        //{
        //    if (_dataAccess == null)
        //        throw new InvalidOperationException("No data access is provided.");

        //    _table = _dataAccess.LoadPotyogosAmobaTable(path);
        //    _size = _table.GetSize;
        //    //_gameStepCount = 0;
        //    _isGameOver = false;

        //}


    }
}