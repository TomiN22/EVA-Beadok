using LooseRobotModel.Model;
using LooseRobotModel.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static LooseRobotModel.Model.Enemy;
using static System.Reflection.Metadata.BlobBuilder;

namespace LooseRobotModel.Model
{
    public class GameModel
    {
        
        private Table _table;
        private readonly ILooseRobotDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;
        private int _baskets=0;

        public event EventHandler<LooseRobotEventArgs>? GameAdvanced;
        public event EventHandler<LooseRobotEventArgs>? GameOver;

        //IBombazoDataAccess

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        //public FieldValue[,] Field { get; private set; }

        public List<Enemy> Enemies { get; private set; }

        public GameModel(ILooseRobotDataAccess? dataAccess, int size)
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

            _table.SetTableValue((GetSize/2), (GetSize/2), 1);

            do
            {
                x = random.Next(_table.GetSize);
                y = random.Next(_table.GetSize);
            }
            while (x == (GetSize/2) && y == (GetSize/2));

            _table.RCurrent[0] = x; _table.RCurrent[1] = y;
            _table.SetTableValue(x, y, 2);
            Enemy enemy = new Enemy(x, y);
            enemy.EnemyStepEvent += OnEnemyStep;
            Enemies.Add(enemy);
        }

        public void SetUpTable()
        {
            Enemy enemy = new Enemy(_table.RCurrent[0], _table.RCurrent[1]);
            enemy.EnemyStepEvent += OnEnemyStep;
            Enemies.Add(enemy);
            

            //Field = new FieldValue[_size, _size];
            //for (int i = 0; i < _size; i++)
            //{
            //    for (int j = 0; j < _size; j++)
            //    {
            //        Field[i, j] = FieldValue.Empty;
            //    }
            //}

            //for (int i = 0; i < GetSize; i++)
            //{
            //    for (int j = 0; j < GetSize; j++)
            //    {
            //        if(_table.GetTableValue(i,j) == )
            //    }
            //}

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

        public void PlaceWall(int x, int y)
        {
            if (_table.GetTableValue(x, y) == 0 && _table.WasWall(x,y) == 0)
            {
                _table.SetTableValue(x, y, 3);

            }
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

                int toX = e.x;
                int toY = e.y;
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
                    if (_table.GetTableValue(e.x, e.y) == 1)
                    {
                        IsGameOver = true; OnGameOver();
                    }
                    _table.SetTableValue(X, Y, 0);
                    _table.SetTableValue(toX, toY, 2);
                    enemy.StepTo(e.x, e.y);
                    _table.RCurrent[0] = e.x; _table.RCurrent[1] = e.y;
                }

            }
        }


        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new LooseRobotEventArgs(false));
        }

        public virtual void OnGameOver()
        {
            GameOver?.Invoke(this, new LooseRobotEventArgs(true));
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