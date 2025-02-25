using LabirintusModel.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace LabirintusModel.Model
{
    public class GameModel
    {
        public enum FieldValue
        {
            BombRange, Empty
        }
        private LabirintusTable _table;
        private readonly ILabirintusDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;
        private int _baskets=0;

        public event EventHandler<LabirintusEventArgs>? GameAdvanced;
        public event EventHandler<LabirintusEventArgs>? GameOver;

        //ILabirintusDataAccess

        public bool IsGameOver { get; set; }

        public int BombCooldown { get; private set; }

        public int EnemiesBlownUp { get; private set; }

        public int GetSize
        {
            get { return _size; }
        }

        public int CountOfEnemies
        {
            get { return _baskets; }
            set { _baskets = value; }
        }

        public int AllCountOfEnemies { get; set; }

        public bool[,] IsVisible { get; private set; }

        public GameModel(ILabirintusDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new LabirintusTable(size);

            IsVisible = new bool[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    IsVisible[i, j] = false;
                }
            }

            
            //GenerateFields();
        }

        public LabirintusTable Table { get { return _table; } }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            _table.GameTime++;
            


            OnGameAdvanced();
        }

        public void MovePlayer(int x, int y)
        {
            if (x >= 0 && x < GetSize && y >= 0 && y < GetSize && _table.GetTableValue(x, y) != 1)
            {
                if (_table.GetTableValue(x, y) == 3)
                {
                    IsGameOver = true; OnGameOver();
                }

                _table.SetTableValue(_table.PCurrent[0], _table.PCurrent[1], 0);
                _table.SetTableValue(x, y, 2);
                _table.PCurrent[0] = x; _table.PCurrent[1] = y;
                IterateVicinity();
            }
        }

        public void IterateVicinity()
        {
            for (int i = 0; i < GetSize; i++)
            {
                for (int j = 0; j < GetSize; j++)
                {
                    IsVisible[i, j] = false;
                }
            }

            IsVisible[Table.PCurrent[0], Table.PCurrent[1]] = true;
            for (int x = (Table.PCurrent[0] - 1 >= 0 ? Table.PCurrent[0] - 1 : Table.PCurrent[0]); x <= (Table.PCurrent[0] + 1 < GetSize ? Table.PCurrent[0] + 1 : GetSize - 1); x++)
            { 
                for (int y = (Table.PCurrent[1] - 1 >= 0 ? Table.PCurrent[1] - 1 : Table.PCurrent[1]); y <= (Table.PCurrent[1] + 1 < GetSize ? Table.PCurrent[1] + 1 : GetSize - 1); y++)
                {
                    if (Table.GetTableValue(x,y) != 1)
                    {
                        IsVisible[x,y] = true;
                        //sarkok
                        if(Table.PCurrent[0] - 2 >= 0 && Table.PCurrent[1] - 2 >= 0 && x == Table.PCurrent[0] - 1 && y == Table.PCurrent[1] - 1)
                            IsVisible[Table.PCurrent[0] - 2, Table.PCurrent[1] - 2] = true;
                        if(Table.PCurrent[0] - 2 >= 0 && Table.PCurrent[1] + 2 < GetSize &&
                            x == Table.PCurrent[0] - 1 && y == Table.PCurrent[1] + 1)
                            IsVisible[Table.PCurrent[0] - 2, Table.PCurrent[1] + 2] = true;
                        if(Table.PCurrent[0] + 2 < GetSize && Table.PCurrent[1] - 2 >= 0 &&
                            x == Table.PCurrent[0] + 1 && y == Table.PCurrent[1] - 1)
                            IsVisible[Table.PCurrent[0] + 2, Table.PCurrent[1] - 2] = true;
                        if(Table.PCurrent[0] + 2 < GetSize && Table.PCurrent[1] + 2 < GetSize &&
                            x == Table.PCurrent[0] + 1 && y == Table.PCurrent[1] + 1)
                            IsVisible[Table.PCurrent[0] + 2, Table.PCurrent[1] + 2] = true;
                        //kereszt
                        if(Table.PCurrent[0] - 2 >= 0 && x == Table.PCurrent[0] - 1 && y == Table.PCurrent[1])
                            IsVisible[Table.PCurrent[0] - 2, Table.PCurrent[1]] = true;
                        if (Table.PCurrent[0] + 2 < GetSize && x == Table.PCurrent[0] + 1 && y == Table.PCurrent[1])
                            IsVisible[Table.PCurrent[0] + 2, Table.PCurrent[1]] = true;
                        if (Table.PCurrent[1] - 2 >= 0 && x == Table.PCurrent[0] && y == Table.PCurrent[1] - 1)
                            IsVisible[Table.PCurrent[0], Table.PCurrent[1] - 2] = true;
                        if (Table.PCurrent[1] + 2 < GetSize && x == Table.PCurrent[0] && y == Table.PCurrent[1] + 1)
                            IsVisible[Table.PCurrent[0], Table.PCurrent[1] + 2] = true;

                        //8 spéci
                        //fent
                        if(Table.PCurrent[0] - 2 >= 0 && Table.PCurrent[1] - 1 >= 0 && Table.GetTableValue(Table.PCurrent[0] - 1, Table.PCurrent[1] - 1) != 1 && Table.GetTableValue(Table.PCurrent[0] - 1, Table.PCurrent[1]) != 1)
                            IsVisible[Table.PCurrent[0] - 2, Table.PCurrent[1] - 1] = true;
                        if (Table.PCurrent[0] - 2 >= 0 && Table.PCurrent[1] + 1 < GetSize && Table.GetTableValue(Table.PCurrent[0] - 1, Table.PCurrent[1] + 1) != 1 && Table.GetTableValue(Table.PCurrent[0] - 1, Table.PCurrent[1]) != 1)
                            IsVisible[Table.PCurrent[0] - 2, Table.PCurrent[1] + 1] = true;

                        //lent
                        if (Table.PCurrent[0] + 2 < GetSize && Table.PCurrent[1] - 1 >= 0 && Table.GetTableValue(Table.PCurrent[0] + 1, Table.PCurrent[1] - 1) != 1 && Table.GetTableValue(Table.PCurrent[0] + 1, Table.PCurrent[1]) != 1)
                            IsVisible[Table.PCurrent[0] + 2, Table.PCurrent[1] - 1] = true;
                        if (Table.PCurrent[0] + 2 < GetSize && Table.PCurrent[1] + 1 < GetSize && Table.GetTableValue(Table.PCurrent[0] + 1, Table.PCurrent[1] + 1) != 1 && Table.GetTableValue(Table.PCurrent[0] + 1, Table.PCurrent[1]) != 1)
                            IsVisible[Table.PCurrent[0] + 2, Table.PCurrent[1] + 1] = true;

                        //bal
                        if (Table.PCurrent[0] - 1 >= 0 && Table.PCurrent[1] - 2 >= 0 && Table.GetTableValue(Table.PCurrent[0] - 1, Table.PCurrent[1] - 1) != 1 && Table.GetTableValue(Table.PCurrent[0], Table.PCurrent[1] - 1) != 1)
                            IsVisible[Table.PCurrent[0] - 1, Table.PCurrent[1] - 2] = true;
                        if (Table.PCurrent[0] + 1 < GetSize && Table.PCurrent[1] - 2 >= 0 && Table.GetTableValue(Table.PCurrent[0] + 1, Table.PCurrent[1] - 1) != 1 && Table.GetTableValue(Table.PCurrent[0], Table.PCurrent[1] - 1) != 1)
                            IsVisible[Table.PCurrent[0] + 1, Table.PCurrent[1] - 2] = true;

                        //jobb
                        if (Table.PCurrent[0] - 1 >= 0 && Table.PCurrent[1] + 2 < GetSize && Table.GetTableValue(Table.PCurrent[0] - 1, Table.PCurrent[1] + 1) != 1 && Table.GetTableValue(Table.PCurrent[0], Table.PCurrent[1] + 1) != 1)
                            IsVisible[Table.PCurrent[0] - 1, Table.PCurrent[1] + 2] = true;
                        if (Table.PCurrent[0] + 1 < GetSize && Table.PCurrent[1] + 2 < GetSize && Table.GetTableValue(Table.PCurrent[0] + 1, Table.PCurrent[1] + 1) != 1 && Table.GetTableValue(Table.PCurrent[0], Table.PCurrent[1] + 1) != 1)
                            IsVisible[Table.PCurrent[0] + 1, Table.PCurrent[1] + 2] = true;
                    }
                }
            }
        }

        //public void SetLights()
        //{
        //    for (int y = (Table.PCurrent[1] - 2 >= 0 ? Table.PCurrent[1] - 2 : 0); y <= (Table.PCurrent[1] + 2 < GetSize ? Table.PCurrent[1] + 2 : GetSize - 1); y++)
        //    {
        //        for (int x = (Table.PCurrent[0] - 2 >= 0 ? Table.PCurrent[0] - 2 : 0); x <= (Table.PCurrent[0] + 2 < GetSize ? Table.PCurrent[0] + 2 : GetSize - 1); x++)
        //        {
        //            if (Field[x, y] == FieldValue.Empty) Field[x, y] = FieldValue.BombRange;
        //        }
        //    }
        //}

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new LabirintusEventArgs(false));
        }

        public virtual void OnGameOver()
        {
            GameOver?.Invoke(this, new LabirintusEventArgs(true));
        }

        public void LoadGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _table = _dataAccess.LoadLabirintusTable(path);
            _size = _table.GetSize;
            _isGameOver = false;
            IterateVicinity();
        }


    }
}