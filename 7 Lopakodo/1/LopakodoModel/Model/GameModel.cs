using LopakodoModel.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static LopakodoModel.Model.Enemy;
using static System.Reflection.Metadata.BlobBuilder;

namespace LopakodoModel.Model
{
    public class GameModel
    {
        public enum FieldValue
        {
            BombRange, Empty
        }
        private LopakodoTable _table;
        private readonly ILopakodoDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;
        private int _baskets=0;

        public event EventHandler<LopakodoEventArgs>? GameAdvanced;
        public event EventHandler<LopakodoEventArgs>? GameOver;

        //ILopakodoDataAccess

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

        public FieldValue[,] Field { get; private set; }
        public bool[,] IsVisible { get; private set; }

        private Dictionary<Enemy, HashSet<(int, int)>> enemyVisibility = new();

        public List<Enemy> Enemies { get; private set; }

        public GameModel(ILopakodoDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new LopakodoTable(size);
            Enemies = new List<Enemy>();

            Field = new FieldValue[size, size];
            IsVisible = new bool[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Field[i, j] = FieldValue.Empty;
                    IsVisible[i,j] = false;
                }
            }
            
            //GenerateFields();
        }

        public LopakodoTable Table { get { return _table; } }

        

        public void AddEnemy()
        {
            Random random = new Random();
            int direction = random.Next(4);

            for (int i = 0; i <_table.GetSize; i++)
            {
                for (int j = 0; j < _table.GetSize; j++)
                {
                    if (_table.GetTableValue(i, j) == 4)
                    {
                        Enemy e = new(i, j);
                        e.EnemyStepEvent += OnEnemyStep;
                        Enemies.Add(e);
                        IterateVicinity(e,i,j);
                    }
                }
            }

        }

        public void CountEnemies()
        {
            for (Int32 i = 0; i < GetSize; i++)
            {
                for (Int32 j = 0; j < GetSize; j++)
                {
                    if(_table.GetTableValue(i, j) == 4)
                    {
                        CountOfEnemies++;
                    }
                }
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

        public void MovePlayer(int x, int y)
        {
            if (x >= 0 && x < GetSize && y >= 0 && y < GetSize && _table.GetTableValue(x, y) != 1)
            {
                if (_table.GetTableValue(x, y) == 3)
                {
                    IsGameOver = true; OnGameOver(Win.Player);
                }

                _table.SetTableValue(_table.PCurrent[0], _table.PCurrent[1], 0);
                _table.SetTableValue(x, y, 2);
                _table.PCurrent[0] = x; _table.PCurrent[1] = y;
            }
        }

        public void IterateVicinity(Enemy e, int ePosX, int ePosY)
        {
            if (enemyVisibility.ContainsKey(e))
            {
                foreach (var pos in enemyVisibility[e])
                {
                    bool isVisibleByOther = Enemies
                        .Where(otherEnemy => otherEnemy != e && enemyVisibility.ContainsKey(otherEnemy))
                        .Any(otherEnemy => enemyVisibility[otherEnemy].Contains(pos));

                    // Csak akkor állítjuk láthatatlanná, ha egyetlen másik ellenség sem látja
                    if (!isVisibleByOther)
                    {
                        IsVisible[pos.Item1, pos.Item2] = false;
                    }
                }
                enemyVisibility[e].Clear();
            }
            else
            {
                enemyVisibility[e] = new HashSet<(int, int)>();
            }


            SetVisibilityAndTrack(e, ePosX, ePosY);
            for (int x = (ePosX - 1 >= 0 ? ePosX - 1 : ePosX); x <= (ePosX + 1 < GetSize ? ePosX + 1 : GetSize - 1); x++)
            {
                for (int y = (ePosY - 1 >= 0 ? ePosY - 1 : ePosY); y <= (ePosY + 1 < GetSize ? ePosY + 1 : GetSize - 1); y++)
                {
                    if (Table.GetTableValue(x, y) != 1)
                    {
                        SetVisibilityAndTrack(e, x, y);
                        //sarkok
                        if (ePosX - 2 >= 0 && ePosY - 2 >= 0 && x == ePosX - 1 && y == ePosY - 1)
                            SetVisibilityAndTrack(e, ePosX - 2, ePosY - 2);
                        if (ePosX - 2 >= 0 && ePosY + 2 < GetSize &&
                            x == ePosX - 1 && y == ePosY + 1)
                            SetVisibilityAndTrack(e, ePosX - 2, ePosY + 2);
                        if (ePosX + 2 < GetSize && ePosY - 2 >= 0 &&
                            x == ePosX + 1 && y == ePosY - 1)
                            SetVisibilityAndTrack(e, ePosX + 2, ePosY - 2);
                        if (ePosX + 2 < GetSize && ePosY + 2 < GetSize &&
                            x == ePosX + 1 && y == ePosY + 1)
                            SetVisibilityAndTrack(e, ePosX + 2, ePosY + 2);
                        //kereszt
                        if (ePosX - 2 >= 0 && x == ePosX - 1 && y == ePosY)
                            SetVisibilityAndTrack(e, ePosX - 2, ePosY);
                        if (ePosX + 2 < GetSize && x == ePosX + 1 && y == ePosY)
                            SetVisibilityAndTrack(e, ePosX + 2, ePosY);
                        if (ePosY - 2 >= 0 && x == ePosX && y == ePosY - 1)
                            SetVisibilityAndTrack(e, ePosX, ePosY - 2);
                        if (ePosY + 2 < GetSize && x == ePosX && y == ePosY + 1)
                            SetVisibilityAndTrack(e, ePosX, ePosY + 2);

                        //8 spéci
                        //fent
                        if (ePosX - 2 >= 0 && ePosY - 1 >= 0 && Table.GetTableValue(ePosX - 1, ePosY - 1) != 1 && Table.GetTableValue(ePosX - 1, ePosY) != 1)
                            SetVisibilityAndTrack(e, ePosX - 2, ePosY - 1);
                        if (ePosX - 2 >= 0 && ePosY + 1 < GetSize && Table.GetTableValue(ePosX - 1, ePosY + 1) != 1 && Table.GetTableValue(ePosX - 1, ePosY) != 1)
                            SetVisibilityAndTrack(e, ePosX - 2, ePosY + 1);

                        //lent
                        if (ePosX + 2 < GetSize && ePosY - 1 >= 0 && Table.GetTableValue(ePosX + 1, ePosY - 1) != 1 && Table.GetTableValue(ePosX + 1, ePosY) != 1)
                            SetVisibilityAndTrack(e, ePosX + 2, ePosY - 1);
                        if (ePosX + 2 < GetSize && ePosY + 1 < GetSize && Table.GetTableValue(ePosX + 1, ePosY + 1) != 1 && Table.GetTableValue(ePosX + 1, ePosY) != 1)
                            SetVisibilityAndTrack(e, ePosX + 2, ePosY + 1);

                        //bal
                        if (ePosX - 1 >= 0 && ePosY - 2 >= 0 && Table.GetTableValue(ePosX - 1, ePosY - 1) != 1 && Table.GetTableValue(ePosX, ePosY - 1) != 1)
                            SetVisibilityAndTrack(e, ePosX - 1, ePosY - 2);
                        if (ePosX + 1 < GetSize && ePosY - 2 >= 0 && Table.GetTableValue(ePosX + 1, ePosY - 1) != 1 && Table.GetTableValue(ePosX, ePosY - 1) != 1)
                            SetVisibilityAndTrack(e, ePosX + 1, ePosY - 2);

                        //jobb
                        if (ePosX - 1 >= 0 && ePosY + 2 < GetSize && Table.GetTableValue(ePosX - 1, ePosY + 1) != 1 && Table.GetTableValue(ePosX, ePosY + 1) != 1)
                            SetVisibilityAndTrack(e, ePosX - 1, ePosY + 2);
                        if (ePosX + 1 < GetSize && ePosY + 2 < GetSize && Table.GetTableValue(ePosX + 1, ePosY + 1) != 1 && Table.GetTableValue(ePosX, ePosY + 1) != 1)
                            SetVisibilityAndTrack(e, ePosX + 1, ePosY + 2);
                    }
                }
            }
        }

        private void SetVisibilityAndTrack(Enemy e, int x, int y)
        {
            IsVisible[x, y] = true;
            enemyVisibility[e].Add((x, y));
        }

        public void ChangeDirection(Enemy e)
        {
            Random r = new Random();
            // Az érvényes irányok listája, amit majd az aktuális pozíció alapján szűkítünk
            List<Directions> allowedDirections = new List<Directions>
            {
                Directions.Up,
                Directions.Right,
                Directions.Down,
                Directions.Left
            };

            allowedDirections.Remove(e.Direction);

            // Az aktuális pozíció alapján eltávolítjuk a tiltott irányt
            if (e.PositionX == 0)
            {
                allowedDirections.Remove(Directions.Up); // Nem mehet balra, ha a bal szélen van
            }
            else if (e.PositionX == GetSize-1)
            {
                allowedDirections.Remove(Directions.Down); // Nem mehet jobbra, ha a jobb szélen van
            }

            if (e.PositionY == 0)
            {
                allowedDirections.Remove(Directions.Left); // Nem mehet felfelé, ha a felső szélen van
            }
            else if (e.PositionY == GetSize-1)
            {
                allowedDirections.Remove(Directions.Right); // Nem mehet lefelé, ha az alsó szélen van
            }

            if (e.PositionX - 1 >= 0 && Table.GetTableValue(e.PositionX - 1, e.PositionY) != 0)
                allowedDirections.Remove(Directions.Up);
            if (e.PositionX + 1 < GetSize && Table.GetTableValue(e.PositionX + 1, e.PositionY) != 0)
                allowedDirections.Remove(Directions.Down);
            if (e.PositionY - 1 >= 0 && Table.GetTableValue(e.PositionX, e.PositionY - 1) != 0)
                allowedDirections.Remove(Directions.Left);
            if (e.PositionY + 1 < GetSize && Table.GetTableValue(e.PositionX, e.PositionY + 1) != 0)
                allowedDirections.Remove(Directions.Right);

            // Véletlenszerűen választunk egy irányt a megengedett irányok közül
            e.Direction = allowedDirections[r.Next(allowedDirections.Count)];
        }

        public void OnEnemyStep(object? sender, EnemyStepEventArgs e)
        {
            if (sender is Enemy enemy)
            {
                int X = enemy.PositionX;
                int Y = enemy.PositionY;
                                
                int toX = e.x;
                int toY = e.y;
                if (toX < 0 || toX >= GetSize || toY < 0 || toY >= GetSize || _table.GetTableValue(toX, toY) == 1 || _table.GetTableValue(toX, toY) == 3 || _table.GetTableValue(toX, toY) == 4)
                {
                    //enemy.ChangeDirection();
                    ChangeDirection(enemy);
                    //enemy.WouldStep();

                    //if (enemy.RtoX >= 0 && enemy.RtoX < GetSize && enemy.RtoY >= 0 && enemy.RtoY < GetSize)
                    //{
                    //    if (_table.GetTableValue(enemy.RtoX, enemy.RtoY) == 1 || _table.GetTableValue(toX, toY) == 3 || _table.GetTableValue(toX, toY) == 4)
                    //    {
                    //        ChangeDirection(enemy);
                    //        //enemy.WouldStepNoEvent();
                    //    }
                    //}
                    //    //enemy.WouldStepNoEvent();
                    //    //_table.SetTableValue(X, Y, 0);
                    //    //_table.SetTableValue(enemy.RtoX, enemy.RtoY, 2);
                    //    //enemy.StepTo(enemy.RtoX, enemy.RtoY);
                    //}
                }
                else
                {
                    //if (_table.GetTableValue(e.x, e.y) == 3)
                    //{
                    //    IsGameOver = true; OnGameOver();
                    //}
                    _table.SetTableValue(X, Y, 0);
                    _table.SetTableValue(toX, toY, 4);
                    enemy.StepTo(e.x, e.y);
                    IterateVicinity(enemy, e.x,e.y);
                }

                if (IsVisible[Table.PCurrent[0], Table.PCurrent[1]])
                {
                    IsGameOver = true; OnGameOver(Win.Guard); return;
                }
            }
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new LopakodoEventArgs());
        }

        public virtual void OnGameOver(Win winner)
        {
            GameOver?.Invoke(this, new LopakodoEventArgs(winner));
        }

        public void LoadGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _table = _dataAccess.LoadLopakodoTable(path);
            _size = _table.GetSize;
            //_gameStepCount = 0;
            _isGameOver = false;
            AddEnemy();
            CountEnemies();
            AllCountOfEnemies=CountOfEnemies;
        }


    }
}