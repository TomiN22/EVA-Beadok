using SnakeModel.Model;
using SnakeModel.Persistence;
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
using static SnakeModel.Persistence.Table;
using static System.Reflection.Metadata.BlobBuilder;

namespace SnakeModel.Model
{
    public class GameModel
    {
        
        private Table _table;
        private readonly ISnakeDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;
        private List<(int X, int Y)> _snake = new List<(int, int)>();

        public event EventHandler<SnakeEventArgs>? GameAdvanced;
        public event EventHandler<SnakeEventArgs>? GameOver;

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public int Bombs {  get; set; }

        public List<(int X, int Y)> Snake
        {
            get => _snake;
            private set => _snake = value; // A setter privát, hogy kívülről ne lehessen módosítani
        }

        public GameModel(ISnakeDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new Table(size);
            Snake = new List<(int, int)>();
            //GenerateFields();
        }

        public Table Table { get { return _table; } }

        public void GenerateFields()
        {
            Snake = new List<(int, int)>()
            {
                (GetSize/2, GetSize/2),
                (GetSize/2+1, GetSize/2),
                (GetSize/2+2, GetSize/2),
                (GetSize/2+3, GetSize/2),
                (GetSize/2+4, GetSize/2)
            };
            //Snake[0] = (GetSize/2, GetSize/2);
            Table.HCurrent[0] = GetSize/2; Table.HCurrent[1] = GetSize/2;
            //Snake[1] = (GetSize/2, GetSize/2+1);
            //Snake[2] = (GetSize/2, GetSize/2+2);
            //Snake[3] = (GetSize/2, GetSize/2+3);
            //Snake[4] = (GetSize/2, GetSize/2+4);
        }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            _table.GameTime++;
            PlaceEgg();
            SnakeStep();

            OnGameAdvanced();
        }

        public void ChangeDirection(string dir)
        {
            switch(Table.Direction)
            {
                case Directions.Up:
                    if(dir == "right")
                        Table.Direction = Directions.Right;
                    else if(dir == "left")
                        Table.Direction = Directions.Left;
                    break;
                case Directions.Down:
                    if (dir == "right")
                        Table.Direction = Directions.Left;
                    else if (dir == "left")
                        Table.Direction = Directions.Right;
                    break;
                case Directions.Left:
                    if (dir == "right")
                        Table.Direction = Directions.Up;
                    else if (dir == "left")
                        Table.Direction = Directions.Down;
                    break;
                case Directions.Right:
                    if (dir == "right")
                        Table.Direction = Directions.Down;
                    else if (dir == "left")
                        Table.Direction = Directions.Up;
                    break;
                default: 
                    throw new ApplicationException();
            }
        }

        public void SnakeStep()
        {
            switch (Table.Direction)
            {
                case Directions.Up:
                    Table.HCurrent[0] = Table.HCurrent[0]-1;
                    break;
                case Directions.Down:
                    Table.HCurrent[0] = Table.HCurrent[0]+1;
                    break;
                case Directions.Left:
                    Table.HCurrent[1] = Table.HCurrent[1]-1;
                    break;
                case Directions.Right:
                    Table.HCurrent[1] = Table.HCurrent[1]+1;
                    break;
                default:
                    throw new ApplicationException();
            }
            SnakeMove(Table.HCurrent[0], Table.HCurrent[1]);
        }

        public void SnakeMove(int x, int y)
        {
            if(Table.HCurrent[0] < 0 || Table.HCurrent[0] >= GetSize || Table.HCurrent[1] < 0 || Table.HCurrent[1] >= GetSize
                || Table.GetTableValue(Table.HCurrent[0], Table.HCurrent[1]) == 1 || Table.GetTableValue(Table.HCurrent[0], Table.HCurrent[1]) == 2)
            {
                IsGameOver = true; OnGameOver(); return;
            }
            else
            {
                Snake.Insert(0, (Table.HCurrent[0], Table.HCurrent[1]));
                if (Table.GetTableValue(Table.HCurrent[0], Table.HCurrent[1]) == 4)
                {
                    Table.EggsEaten++; Table.IsEaten = true;
                }
                Table.SetTableValue(Table.HCurrent[0], Table.HCurrent[1], 3);

                if(!Table.IsEaten)
                {
                    Table.SetTableValue(Snake[Snake.Count-1].X, Snake[Snake.Count-1].Y, 0);
                    Snake.RemoveAt(Snake.Count - 1);
                }
                
                for (int i = 1; i < Snake.Count; i++)
                {
                    Table.SetTableValue(Snake[i].X, Snake[i].Y, 2);
                }

            }

            
        }

        public void PlaceEgg()
        {
            Random random = new Random();
            int x, y;

            if (Table.IsEaten)
            {
                do
                {
                    x = random.Next(0, GetSize);
                    y = random.Next(0, GetSize);
                }
                while (Table.GetTableValue(x,y) != 0);
                Table.SetTableValue(x, y, 4);
                Table.IsEaten = false;
            }
        }


        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new SnakeEventArgs());
        }

        public virtual void OnGameOver()
        {
            GameOver?.Invoke(this, new SnakeEventArgs());
        }


        //public async Task SaveGameAsync(String path)
        //{
        //    if (_dataAccess == null)
        //        throw new InvalidOperationException("No data access is provided.");

        //    await _dataAccess.SaveAsync(path, _table);
        //}

        //public async Task LoadGameAsync(String path)
        //{
        //    if (_dataAccess == null)
        //        throw new InvalidOperationException("No data access is provided.");

        //    Enemies.RemoveAll(e => true);

        //    _table = await _dataAccess.LoadAsync(path);
        //    _size = _table.GetSize;
        //    _isGameOver = false;
        //    SetUpTable();
        //}

        public void LoadGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _table = _dataAccess.LoadSnakeTable(path);
            _size = _table.GetSize;
            //_gameStepCount = 0;
            _isGameOver = false;

        }


    }
}