using RunModel.Model;
using RunModel.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static RunModel.Model.Enemy;
using static System.Reflection.Metadata.BlobBuilder;

namespace RunModel.Model
{
    public class GameModel
    {
        
        private Table _table;
        private readonly IRunDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;
        private int _baskets=0;

        public event EventHandler<RunEventArgs>? GameAdvanced;
        public event EventHandler<RunEventArgs>? GameOver;

        //IBombazoDataAccess

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public int Bombs {  get; set; }

        //public FieldValue[,] Field { get; private set; }

        public List<Enemy> Enemies { get; private set; }

        public GameModel(IRunDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new Table(size);
            Enemies = new List<Enemy>();

            //GenerateFields();
        }

        public Table Table { get { return _table; } }

        public void GenerateFields()
        {
            Random random = new Random();
            int x, y, n = _table.GetSize;

            _table.SetTableValue(0, (GetSize/2), 1);
            Table.PCurrent[0] = 0; Table.PCurrent[1] = GetSize/2;

            _table.SetTableValue(GetSize-1, 0, 2);
            _table.SetTableValue(GetSize-1, GetSize-1, 2);

            Enemy enemy1 = new Enemy(GetSize-1, 0);
            enemy1.Id = 1;
            enemy1.EnemyStepEvent += OnEnemyStep;
            Enemies.Add(enemy1);
            Table.E1Current[0] = GetSize-1; Table.E1Current[1] = 0;

            Enemy enemy2 = new Enemy(GetSize-1, GetSize-1);
            enemy2.Id = 2;
            enemy2.EnemyStepEvent += OnEnemyStep;
            Enemies.Add(enemy2);
            Table.E2Current[0] = GetSize-1; Table.E2Current[1] = GetSize-1;

            if (GetSize == 11)
            {
                Bombs = 5;
            }
            else if(GetSize == 15)
            {
                Bombs = 10;
            }
            else if(GetSize == 21)
            {
                Bombs = 25;
            }

            while (Bombs != 0)
            {
                x = random.Next(0, GetSize);
                y = random.Next(0, GetSize);

                if (_table.GetTableValue(x, y) == 0)
                {
                    _table.SetTableValue(x,y,3);
                    Bombs--;
                }
            }
        }

        public void SetUpTable()
        {
            if (Table.E1Current[0] != -1 && Table.E1Current[1] != -1)
            {
                Enemy enemy1 = new Enemy(Table.E1Current[0], Table.E1Current[1]);
                enemy1.EnemyStepEvent += OnEnemyStep;
                Enemies.Add(enemy1);
            }

            if (Table.E2Current[0] != -1 && Table.E2Current[1] != -1)
            {
                Enemy enemy2 = new Enemy(Table.E2Current[0], Table.E2Current[1]);
                enemy2.EnemyStepEvent += OnEnemyStep;
                Enemies.Add(enemy2);
            }
            
        }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            _table.GameTime++;
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                Enemies[i].WouldStep();
            }
            //foreach (Enemy enemy in Enemies)
            //{
            //    enemy.WouldStep();
            //}

            CountEnemies();

            OnGameAdvanced();
        }

        public void MovePlayer(int x, int y)
        {
            if (x >= 0 && x < GetSize && y >= 0 && y < GetSize)
            {
                if (_table.GetTableValue(x, y) == 2 || _table.GetTableValue(x, y) == 3)
                {
                    IsGameOver = true; OnGameOver(false);
                }

                _table.SetTableValue(_table.PCurrent[0], _table.PCurrent[1], 0);
                _table.SetTableValue(x, y, 1);
                _table.PCurrent[0] = x; _table.PCurrent[1] = y;
            }
        }

        public void CountEnemies()
        {
            if (Enemies.Count == 0)
            {
                IsGameOver = true; OnGameOver(true);
            }                
        }

        public void SetECurrent(Enemy enemy)
        {
            if (enemy.Id == 1)
            {
                _table.E1Current[0] = enemy.PositionX; _table.E1Current[1] = enemy.PositionY;
            }
            else
            {
                _table.E2Current[0] = enemy.PositionX; _table.E2Current[1] = enemy.PositionY;
            }
        }

        public void OnEnemyStep(object? sender, EnemyStepEventArgs e)
        {
            if (sender is Enemy enemy)
            {
                int X = enemy.PositionX;
                int Y = enemy.PositionY;
                
                if (Table.PCurrent[0] < enemy.PositionX && Table.PCurrent[1] > enemy.PositionY)
                {
                    if((enemy.PositionX-Table.PCurrent[0]) >= (Table.PCurrent[1]-enemy.PositionY))
                    {
                        _table.SetTableValue(X, Y, 0);
                        if (_table.GetTableValue(enemy.PositionX-1, enemy.PositionY) == 3)
                        {
                            Enemies.Remove(enemy);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                            SetECurrent(enemy); return;
                        }
                        else
                        {
                            enemy.StepTo(e.x-1, e.y);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                        }

                        SetECurrent(enemy);
                    }
                    else if((enemy.PositionX-Table.PCurrent[0]) < (Table.PCurrent[1]-enemy.PositionY))
                    {
                        _table.SetTableValue(X, Y, 0);
                        if (_table.GetTableValue(enemy.PositionX, enemy.PositionY+1) == 3)
                        {
                            Enemies.Remove(enemy);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                            SetECurrent(enemy); return;
                        }
                        else
                        {
                            enemy.StepTo(e.x, e.y+1);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                        }

                        SetECurrent(enemy);
                    }
                    if (_table.GetTableValue(e.x, e.y) == 1)
                    {
                        IsGameOver = true; OnGameOver(false);
                    }
                    
                }
                else if (Table.PCurrent[0] < enemy.PositionX && Table.PCurrent[1] < enemy.PositionY)
                {
                    if ((enemy.PositionX-Table.PCurrent[0]) >= (enemy.PositionY-Table.PCurrent[1]))
                    {
                        _table.SetTableValue(X, Y, 0);
                        if (_table.GetTableValue(enemy.PositionX-1, enemy.PositionY) == 3)
                        {
                            Enemies.Remove(enemy);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                            SetECurrent(enemy); return;
                        }
                        else
                        {
                            enemy.StepTo(e.x-1, e.y);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                        }

                        SetECurrent(enemy);
                    }
                    else if((enemy.PositionX-Table.PCurrent[0]) < (enemy.PositionY-Table.PCurrent[1]))
                    {
                        _table.SetTableValue(X, Y, 0);
                        if (_table.GetTableValue(enemy.PositionX, enemy.PositionY-1) == 3)
                        {
                            Enemies.Remove(enemy);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                            SetECurrent(enemy); return;
                        }
                        else
                        {
                            enemy.StepTo(e.x, e.y-1);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                        }

                        SetECurrent(enemy);
                    }
                    if (_table.GetTableValue(e.x, e.y) == 1)
                    {
                        IsGameOver = true; OnGameOver(false);
                    }
                    
                }
                else if(Table.PCurrent[0] > enemy.PositionX && Table.PCurrent[1] > enemy.PositionY)
                {
                    if ((Table.PCurrent[0]-enemy.PositionX) >= (Table.PCurrent[1]-enemy.PositionY))
                    {
                        _table.SetTableValue(X, Y, 0);
                        if (_table.GetTableValue(enemy.PositionX+1, enemy.PositionY) == 3)
                        {
                            Enemies.Remove(enemy);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                            SetECurrent(enemy); return;
                        }
                        else
                        {
                            enemy.StepTo(e.x+1, e.y);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                        }

                        SetECurrent(enemy);
                    }
                    else if ((Table.PCurrent[0]-enemy.PositionX) < (Table.PCurrent[1]-enemy.PositionY))
                    {
                        _table.SetTableValue(X, Y, 0);
                        if (_table.GetTableValue(enemy.PositionX, enemy.PositionY+1) == 3)
                        {
                            Enemies.Remove(enemy);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                            SetECurrent(enemy); return;
                        }
                        else
                        {
                            enemy.StepTo(e.x, e.y+1);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                        }

                        SetECurrent(enemy);
                    }
                    if (_table.GetTableValue(e.x, e.y) == 1)
                    {
                        IsGameOver = true; OnGameOver(false);
                    }
                    
                }
                else if(Table.PCurrent[0] > enemy.PositionX && Table.PCurrent[1] < enemy.PositionY)
                {
                    if ((Table.PCurrent[0]-enemy.PositionX) >= (enemy.PositionY-Table.PCurrent[1]))
                    {
                        _table.SetTableValue(X, Y, 0);
                        if (_table.GetTableValue(enemy.PositionX+1, enemy.PositionY) == 3)
                        {
                            Enemies.Remove(enemy);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                            SetECurrent(enemy); return;
                        }
                        else
                        {
                            enemy.StepTo(e.x+1, e.y);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                        }

                        SetECurrent(enemy);
                    }
                    else if ((Table.PCurrent[0]-enemy.PositionX) < (enemy.PositionY-Table.PCurrent[1]))
                    {
                        _table.SetTableValue(X, Y, 0);
                        if (_table.GetTableValue(enemy.PositionX, enemy.PositionY-1) == 3)
                        {
                            Enemies.Remove(enemy);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                            SetECurrent(enemy); return;
                        }
                        else
                        {
                            enemy.StepTo(e.x, e.y-1);
                            _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                        }

                        SetECurrent(enemy);
                    }
                    if (_table.GetTableValue(e.x, e.y) == 1)
                    {
                        IsGameOver = true; OnGameOver(false);
                    }
                    
                }
                else if(Table.PCurrent[0] == enemy.PositionX && Table.PCurrent[1] < enemy.PositionY)
                {
                    _table.SetTableValue(X, Y, 0);
                    if (_table.GetTableValue(enemy.PositionX, enemy.PositionY-1) == 3)
                    {
                        Enemies.Remove(enemy);
                        _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                        SetECurrent(enemy); return;
                    }
                    else if (_table.GetTableValue(enemy.PositionX, enemy.PositionY-1) == 1)
                    {
                        IsGameOver = true; OnGameOver(false);
                    }
                    else
                    {
                        enemy.StepTo(e.x, e.y-1);
                        _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                    }

                    SetECurrent(enemy);
                }
                else if(Table.PCurrent[0] == enemy.PositionX && Table.PCurrent[1] > enemy.PositionY)
                {
                    _table.SetTableValue(X, Y, 0);
                    if (_table.GetTableValue(enemy.PositionX, enemy.PositionY+1) == 3)
                    {
                        Enemies.Remove(enemy);
                        _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                        SetECurrent(enemy); return;
                    }
                    else if (_table.GetTableValue(enemy.PositionX, enemy.PositionY+1) == 1)
                    {
                        IsGameOver = true; OnGameOver(false);
                    }
                    else
                    {
                        enemy.StepTo(e.x, e.y+1);
                        _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                    }

                    SetECurrent(enemy);
                }
                else if(Table.PCurrent[1] == enemy.PositionY && Table.PCurrent[0] < enemy.PositionX)
                {
                    _table.SetTableValue(X, Y, 0);
                    if (_table.GetTableValue(enemy.PositionX-1, enemy.PositionY) == 3)
                    {
                        Enemies.Remove(enemy);
                        _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                        SetECurrent(enemy); return;
                    }
                    else if (_table.GetTableValue(enemy.PositionX-1, enemy.PositionY) == 1)
                    {
                        IsGameOver = true; OnGameOver(false);
                    }
                    else
                    {
                        enemy.StepTo(e.x-1, e.y);
                        _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                    }

                    SetECurrent(enemy);
                }
                else if (Table.PCurrent[1] == enemy.PositionY && Table.PCurrent[0] > enemy.PositionX)
                {
                    _table.SetTableValue(X, Y, 0);
                    if (_table.GetTableValue(enemy.PositionX+1, enemy.PositionY) == 3)
                    {
                        Enemies.Remove(enemy);
                        _table.SetTableValue(enemy.PositionX, enemy.PositionY, 0);
                        SetECurrent(enemy); return;
                    }
                    else if (_table.GetTableValue(enemy.PositionX+1, enemy.PositionY) == 1)
                    {
                        IsGameOver = true; OnGameOver(false);
                    }
                    else
                    {
                        enemy.StepTo(e.x+1, e.y);
                        _table.SetTableValue(enemy.PositionX, enemy.PositionY, 2);
                    }

                    SetECurrent(enemy);
                }

            }
        }


        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new RunEventArgs(false));
        }

        public virtual void OnGameOver(bool hasWon)
        {
            GameOver?.Invoke(this, new RunEventArgs(hasWon));
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

            Enemies.RemoveAll(e => true);

            _table = await _dataAccess.LoadAsync(path);
            _size = _table.GetSize;
            _isGameOver = false;
            SetUpTable();
        }

        //public void LoadGame(String path)
        //{
        //    if (_dataAccess == null)
        //        throw new InvalidOperationException("No data access is provided.");

        //    _table = _dataAccess.LoadBombazoTable(path);
        //    _size = _table.GetSize;
        //    //_gameStepCount = 0;
        //    _isGameOver = false;
        //    AddEnemy();
        //    CountEnemies();
        //    AllCountOfEnemies=CountOfEnemies;
        //}


    }
}