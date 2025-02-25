using Bombazo.Model;
using BombazoModel.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static BombazoModel.Model.Enemy;
using static System.Reflection.Metadata.BlobBuilder;

namespace BombazoModel.Model
{
    public class GameModel
    {
        public enum FieldValue
        {
            BombRange, Empty
        }
        private BombazoTable _table;
        private readonly IBombazoDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;
        private int _baskets=0;

        public event EventHandler<BombazoEventArgs>? GameAdvanced;
        public event EventHandler<BombazoEventArgs>? GameOver;

        //IBombazoDataAccess

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


        public List<Bomb> Bombs { get; private set; }
        public List<Enemy> Enemies { get; private set; }

        public GameModel(IBombazoDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new BombazoTable(size);
            Enemies = new List<Enemy>();
            Bombs = new List<Bomb>();

            Field = new FieldValue[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Field[i, j] = FieldValue.Empty;
                }
            }
            
            EnemiesBlownUp = 0;
            BombCooldown = 0;
            //GenerateFields();
        }

        public BombazoTable Table { get { return _table; } }

        

        public void AddEnemy()
        {
            Random random = new Random();
            int direction = random.Next(4);

            for (int i = 0; i <_table.GetSize; i++)
            {
                for (int j = 0; j < _table.GetSize; j++)
                {
                    if (_table.GetTableValue(i, j) == 2)
                    {
                        Enemy e = new(i, j);
                        e.EnemyStepEvent += OnEnemyStep;
                        Enemies.Add(e);
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
                    if(_table.GetTableValue(i, j) == 2)
                    {
                        CountOfEnemies++;
                    }
                }
            }
        }

        public void PlantBomb(int x, int y)
        {
            if (BombCooldown != 0) return;
            if (GetSize==15)
                BombCooldown = 1;
            if (GetSize==11)
                BombCooldown = 4;
            if (GetSize==17)
                BombCooldown = 4;

            Bomb b = new(x, y);
            Field[x, y] = FieldValue.BombRange;
            b.BombDetonateEvent += OnBombDetonate;
            Bombs.Add(b);
            SetBombRange(b);
        }

        public void BombTick()
        {
            if (Bombs.Count != 0)
            {
                for (int i = 0; i < Bombs.Count; i++)
                {
                    Bombs[i].TimeTick();
                }
            }
            
        }

        public void SetBombRange(Bomb b)
        {
            for (int by = (b.PositionY - 3 >= 0 ? b.PositionY - 3 : 0); by <= (b.PositionY + 3 < GetSize ? b.PositionY + 3 : GetSize - 1); by++)
            {
                for (int bx = (b.PositionX - 3 >= 0 ? b.PositionX - 3 : 0); bx <= (b.PositionX + 3 < GetSize ? b.PositionX + 3 : GetSize - 1); bx++)
                {
                    if (Field[bx, by] == FieldValue.Empty) Field[bx, by] = FieldValue.BombRange;
                }
            }
        }

        public void BombCooldownTick()
        {
            if (BombCooldown == 0) return;
            --BombCooldown;
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

            BombTick();
            BombCooldownTick();
            AllBombRange();

            OnGameAdvanced();
        }

        public void MovePlayer(int x, int y)
        {
            if (x >= 0 && x < GetSize && y >= 0 && y < GetSize && _table.GetTableValue(x, y) != 1)
            {
                if (_table.GetTableValue(x, y) == 2)
                {
                    IsGameOver = true; OnGameOver();
                }

                _table.SetTableValue(_table.PCurrent[0], _table.PCurrent[1], 0);
                _table.SetTableValue(x, y, 3);
                _table.PCurrent[0] = x; _table.PCurrent[1] = y;
            }
        }

        public void AllBombRange()
        {
            if (Bombs.Count == 0) return;
            for (int i = 0; i < Bombs.Count; i++)
            {
                SetBombRange(Bombs[i]);
            }
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

            // Az aktuális pozíció alapján eltávolítjuk a tiltott irányt
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



            // Véletlenszerűen választunk egy irányt a megengedett irányok közül
            e.Direction = allowedDirections[r.Next(allowedDirections.Count)];
        }

        public void OnEnemyStep(object? sender, EnemyStepEventArgs e)
        {
            if (sender is Enemy enemy)
            {
                int X = enemy.PositionX;
                int Y = enemy.PositionY;

                //for (int by = (enemy.PositionY - 1 >= 0 ? enemy.PositionY - 1 : 0); by <= (enemy.PositionY + 1 < GetSize ? enemy.PositionY + 1 : GetSize - 1); by++)
                //{
                //    for (int bx = (enemy.PositionX - 1 >= 0 ? enemy.PositionX - 1 : 0); bx <= (enemy.PositionX + 1 < GetSize ? enemy.PositionX + 1 : GetSize - 1); bx++)
                //    {
                //        if (_table.GetTableValue(bx, by) == 1)
                //        {
                //            IsGameOver = true; OnGameOver();
                //        }
                //    }
                //}

                

                int toX = e.x;
                int toY = e.y;
                if (toX < 0 || toX >= GetSize || toY < 0 || toY >= GetSize || _table.GetTableValue(toX, toY) == 1 || _table.GetTableValue(toX, toY) == 2)
                {

                    //enemy.ChangeDirection();
                    ChangeDirection(enemy);
                    //enemy.WouldStep();

                    if (enemy.RtoX >= 0 && enemy.RtoX < GetSize && enemy.RtoY >= 0 && enemy.RtoY < GetSize)
                    {
                        if (_table.GetTableValue(enemy.RtoX, enemy.RtoY) == 1 || _table.GetTableValue(enemy.RtoX, enemy.RtoY) == 2)
                        {
                            ChangeDirection(enemy);
                            //enemy.WouldStepNoEvent();
                        }
                    }
                    //    //enemy.WouldStepNoEvent();
                    //    //_table.SetTableValue(X, Y, 0);
                    //    //_table.SetTableValue(enemy.RtoX, enemy.RtoY, 2);
                    //    //enemy.StepTo(enemy.RtoX, enemy.RtoY);
                    //}
                }
                else
                {
                    if (_table.GetTableValue(e.x, e.y) == 3)
                    {
                        IsGameOver = true; OnGameOver();
                    }
                    _table.SetTableValue(X, Y, 0);
                    _table.SetTableValue(toX, toY, 2);
                    enemy.StepTo(e.x, e.y);
                    
                }

            }
        }

        private void OnBombDetonate(object? sender, EventArgs e)
        {
            if (sender is Bomb bomb)
            {
                Bombs.Remove(bomb);
                for (int y = (bomb.PositionY - 3 >= 0 ? bomb.PositionY - 3 : 0); y <= (bomb.PositionY + 3 < GetSize ? bomb.PositionY + 3 : GetSize - 1); y++)
                {
                    for (int x = (bomb.PositionX - 3 >= 0 ? bomb.PositionX - 3 : 0); x <= (bomb.PositionX + 3 < GetSize ? bomb.PositionX + 3 : GetSize - 1); x++)
                    {
                        if(_table.GetTableValue(x,y) == 2)
                        {
                            EnemiesBlownUp++;
                            
                            _table.SetTableValue(x,y, 0);
                            Enemies.Remove(Enemies.First(e => e.PositionX == x && e.PositionY == y));

                            if (Enemies.Count == 0)
                            {
                                IsGameOver = true; OnGameOver();
                            }

                        }
                        if(_table.GetTableValue(x,y) == 3)
                        {
                            IsGameOver = true; OnGameOver();
                        }
                        Field[x, y] = FieldValue.Empty;
                    }
                }
            }
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new BombazoEventArgs(false));
        }

        public virtual void OnGameOver()
        {
            GameOver?.Invoke(this, new BombazoEventArgs(true));
        }

        public void LoadGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _table = _dataAccess.LoadBombazoTable(path);
            _size = _table.GetSize;
            //_gameStepCount = 0;
            _isGameOver = false;
            AddEnemy();
            CountEnemies();
            AllCountOfEnemies=CountOfEnemies;
        }


    }
}