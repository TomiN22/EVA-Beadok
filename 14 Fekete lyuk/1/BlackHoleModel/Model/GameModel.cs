using BlackHoleModel.Model;
using BlackHoleModel.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static BlackHoleModel.Model.Enemy;
using static System.Reflection.Metadata.BlobBuilder;

namespace BlackHoleModel.Model
{
    public class GameModel
    {
        private Table _table;
        private readonly IBlackHoleDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;

        public event EventHandler<BlackHoleEventArgs>? GameAdvanced;
        public event EventHandler<BlackHoleEventArgs>? GameOver;

        //IBombazoDataAccess

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        //public FieldValue[,] Field { get; private set; }

        public List<Enemy> Enemies { get; private set; }

        public GameModel(IBlackHoleDataAccess? dataAccess, int size)
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
            for (int i = 0; i < GetSize; i++)
            {
                for (int j = 0; j < GetSize; j++)
                {
                    _table.SetTableValue(i, j, 0);
                }
            }

            _table.SetTableValue((GetSize/2), (GetSize/2), 1);

            //player 1 default
            _table.SetTableValue(0, 0, 2);
            _table.SetTableValue(1, 1, 2);
            _table.SetTableValue(0, GetSize-1, 2);
            _table.SetTableValue(1, GetSize-2, 2);

            //player 2 default
            _table.SetTableValue(GetSize-1, 0, 3);
            _table.SetTableValue(GetSize-2, 1, 3);
            _table.SetTableValue(GetSize-1, GetSize-1, 3);
            _table.SetTableValue(GetSize-2, GetSize-2, 3);

            if (GetSize == 7 || GetSize == 9)
            {
                //player 1 default
                _table.SetTableValue(2, 2, 2);
                _table.SetTableValue(2, GetSize-3, 2);

                //player 2 default
                _table.SetTableValue(GetSize-3, 2, 3);
                _table.SetTableValue(GetSize-3, GetSize-3, 3);
            }
            if (GetSize == 9)
            {
                //player 1
                _table.SetTableValue(3, 3, 2);
                _table.SetTableValue(3, GetSize-4, 2);

                //player 2
                _table.SetTableValue(GetSize-4, 3, 3);
                _table.SetTableValue(GetSize-4, GetSize-4, 3);
            }
            
        }

        public void Step(int x, int y)
        {
            if (!IsGameOver)
            {
                if (Table.Player == 1)
                {
                    if (_table.GetTableValue(x, y) == 2)
                    {
                        Table.P1Current[0] = x; Table.P1Current[1] = y;
                    }
                    if((_table.GetTableValue(x,y) == 0 || _table.GetTableValue(x, y) == 1) && Table.P1Current[0] != -1 && Table.P1Current[1] != -1 &&
                        ((x==Table.P1Current[0]-1 && y==Table.P1Current[1]) || (x==Table.P1Current[0]+1 && y==Table.P1Current[1]) || (x==Table.P1Current[0] && y==Table.P1Current[1]-1) || (x==Table.P1Current[0] && y==Table.P1Current[1]+1)))
                    {
                        if (Table.P1Current[0] > x)
                        {
                            if(_table.GetTableValue(x, y) == 0)
                            {
                                //Table.SetTableValue(x, y, 0);
                                while (x>=0 && Table.GetTableValue(x, y) == 0)
                                {
                                    Table.SetTableValue(x+1, y, 0);
                                    Table.SetTableValue(x, y, 2); x--;
                                }
                                
                            }
                            if (x>0 && Table.GetTableValue(x, y) == 1)
                            {
                                Table.SetTableValue(x, y, 1);
                                Table.SetTableValue(x+1, y, 0); Table.P1ShipsInHole++; CheckShips();
                            }
                        }
                        else if (Table.P1Current[0] < x)
                        {
                            if (_table.GetTableValue(x, y) == 0)
                            {
                                while (x<=GetSize-1 && Table.GetTableValue(x, y) == 0)
                                {
                                    Table.SetTableValue(x-1, y, 0);
                                    Table.SetTableValue(x, y, 2); x++;
                                }
                            }
                            if (x<GetSize-1 && Table.GetTableValue(x, y) == 1)
                            {
                                Table.SetTableValue(x, y, 1);
                                Table.SetTableValue(x-1, y, 0); Table.P1ShipsInHole++; CheckShips();
                            }
                        }

                        if (Table.P1Current[1] > y)
                        {
                            if (_table.GetTableValue(x, y) == 0)
                            {
                                while (y>=0 && Table.GetTableValue(x, y) == 0)
                                {
                                    Table.SetTableValue(x, y+1, 0);
                                    Table.SetTableValue(x, y, 2); y--;
                                }
                            }
                            if (y>0 && Table.GetTableValue(x, y) == 1)
                            {
                                Table.SetTableValue(x, y, 1);
                                Table.SetTableValue(x, y+1, 0); Table.P1ShipsInHole++; CheckShips();
                            }
                        }
                        else if (Table.P1Current[1] < y)
                        {
                            if (_table.GetTableValue(x, y) == 0)
                            {
                                while (y<=GetSize-1 && Table.GetTableValue(x, y) == 0)
                                {
                                    Table.SetTableValue(x, y-1, 0);
                                    Table.SetTableValue(x, y, 2); y++;
                                }
                            }
                            if (y<GetSize-1 && Table.GetTableValue(x, y) == 1)
                            {
                                Table.SetTableValue(x, y, 1);
                                Table.SetTableValue(x, y-1, 0); Table.P1ShipsInHole++; CheckShips();
                            }
                        }
                        Table.P1Current[0] = -1; Table.P1Current[1] = -1;
                        Table.ChangePlayer();
                    }
                    //if (_table.GetTableValue(x, y) == 1)
                    //{

                    //}
                }
                else if (Table.Player == 2)
                {
                    if (_table.GetTableValue(x, y) == 3)
                    {
                        Table.P2Current[0] = x; Table.P2Current[1] = y;
                    }
                    if((_table.GetTableValue(x, y) == 0 || _table.GetTableValue(x, y) == 1) && Table.P2Current[0] != -1 && Table.P2Current[1] != -1 && ((x==Table.P2Current[0]-1 && y==Table.P2Current[1]) || (x==Table.P2Current[0]+1 && y==Table.P2Current[1]) || (x==Table.P2Current[0] && y==Table.P2Current[1]-1) || (x==Table.P2Current[0] && y==Table.P2Current[1]+1)))
                    {
                        if (Table.P2Current[0] > x)
                        {
                            if (_table.GetTableValue(x, y) == 0)
                            {
                                while (x>=0 && Table.GetTableValue(x, y) == 0)
                                {
                                    Table.SetTableValue(x+1, y, 0);
                                    Table.SetTableValue(x, y, 3); x--;
                                }
                            }
                            if (x>0 && Table.GetTableValue(x, y) == 1)
                            {
                                Table.SetTableValue(x, y, 1);
                                Table.SetTableValue(x+1, y, 0); Table.P2ShipsInHole++; CheckShips();
                            }
                        }
                        else if(Table.P2Current[0] < x)
                        {
                            if (_table.GetTableValue(x, y) == 0)
                            {
                                while (x<=GetSize-1 && Table.GetTableValue(x, y) == 0)
                                {
                                    Table.SetTableValue(x-1, y, 0);
                                    Table.SetTableValue(x, y, 3); x++;
                                }
                            }
                            if (x<GetSize-1 && Table.GetTableValue(x, y) == 1)
                            {
                                Table.SetTableValue(x, y, 1);
                                Table.SetTableValue(x-1, y, 0); Table.P2ShipsInHole++; CheckShips();
                            }
                        }

                        if (Table.P2Current[1] > y)
                        {
                            if (_table.GetTableValue(x, y) == 0)
                            {
                                while (y>=0 && Table.GetTableValue(x, y) == 0)
                                {
                                    Table.SetTableValue(x, y+1, 0);
                                    Table.SetTableValue(x, y, 3); y--;
                                }
                            }
                            if (y>0 && Table.GetTableValue(x, y) == 1)
                            {
                                Table.SetTableValue(x, y, 1);
                                Table.SetTableValue(x, y+1, 0); Table.P2ShipsInHole++; CheckShips();
                            }
                        }
                        else if (Table.P2Current[1] < y)
                        {
                            if (_table.GetTableValue(x, y) == 0)
                            {
                                while (y<=GetSize-1 && Table.GetTableValue(x, y) == 0)
                                {
                                    Table.SetTableValue(x, y-1, 0);
                                    Table.SetTableValue(x, y, 3); y++;
                                }
                            }
                            if (y<GetSize-1 && Table.GetTableValue(x, y) == 1)
                            {
                                Table.SetTableValue(x, y, 1);
                                Table.SetTableValue(x, y-1, 0); Table.P2ShipsInHole++; CheckShips();
                            }
                        }
                        Table.P2Current[0] = -1; Table.P2Current[1] = -1;
                        Table.ChangePlayer();
                    }
                }
            }
        }

        public void CheckShips()
        {
            if(Table.P1ShipsInHole == (GetSize-1)/2)
            {
                IsGameOver = true; OnGameOver();
            }
            if(Table.P2ShipsInHole == (GetSize-1)/2)
            {
                IsGameOver = true; OnGameOver();
            }
        }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            _table.GameTime++;
            foreach (Enemy enemy in Enemies)
            {
                enemy.WouldStep();
            }

           

            OnGameAdvanced();
        }

        public void ChangeDirection(Enemy e)
        {
            Random r = new Random();
            List<Directions> allowedDirections = new List<Directions>
            {
                Directions.Up,
                Directions.Right,
                Directions.Down,
                Directions.Left
            };

            allowedDirections.Remove(e.Direction);

            if (e.PositionX == 0)
            {
                allowedDirections.Remove(Directions.Left); // Nem mehet balra, ha a bal szélen van
            }
            else if (e.PositionX == GetSize-1)
            {
                allowedDirections.Remove(Directions.Right); // Nem mehet jobbra, ha a jobb szélen van
            }

            if (e.PositionY == 0)
            {
                allowedDirections.Remove(Directions.Up); // Nem mehet felfelé, ha a felső szélen van
            }
            else if (e.PositionY == GetSize-1)
            {
                allowedDirections.Remove(Directions.Down); // Nem mehet lefelé, ha az alsó szélen van
            }



            e.Direction = allowedDirections[r.Next(allowedDirections.Count)];
        }

        public void OnEnemyStep(object? sender, EnemyStepEventArgs e)
        {
            if (sender is Enemy enemy)
            {

                int X = enemy.PositionX;
                int Y = enemy.PositionY;

                //megkergülés (ha nem fix időnként kergül meg, akkor az 5mp helyett egy rand kell és ahhoz vizsgáljuk
                if(_table.GameTime % 5 == 0)
                {
                    ChangeDirection(enemy);
                }

                int toX = e.X;
                int toY = e.Y;
                if (toX < 0 || toX >= GetSize || toY < 0 || toY >= GetSize || _table.GetTableValue(toX, toY) == 3)
                {
                    if (toX >= 0 && toX < GetSize && toY >= 0 && toY < GetSize && (_table.GetTableValue(toX, toY) == 3))
                    {
                        _table.SetWasWall(toX, toY, 1);
                        //Field[toX, toY] = FieldValue.WasWall;
                        _table.SetTableValue(toX, toY, 0);
                        
                    }
                    
                    ChangeDirection(enemy);


                    //enemy.WouldStep();

                    //enemy.WouldStepNoEvent();
                    //_table.SetTableValue(X, Y, 0);
                    //_table.SetTableValue(enemy.RtoX, enemy.RtoY, 2);
                    //enemy.StepTo(enemy.RtoX, enemy.RtoY);
                }
                else
                {
                    if (_table.GetTableValue(e.X, e.Y) == 1)
                    {
                        IsGameOver = true; OnGameOver();
                    }
                    _table.SetTableValue(X, Y, 0);
                    _table.SetTableValue(toX, toY, 2);
                    enemy.StepTo(e.X, e.Y);
                    
                }

            }
        }


        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new BlackHoleEventArgs(false));
        }

        public virtual void OnGameOver()
        {
            GameOver?.Invoke(this, new BlackHoleEventArgs(true));
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