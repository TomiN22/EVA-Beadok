using AknamezoModel.Model;
using AknamezoModel.Persistence;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static AknamezoModel.Model.Meteor;
using static AknamezoModel.Model.Ship;
using static System.Reflection.Metadata.BlobBuilder;

namespace AknamezoModel.Model
{
    public class GameModel
    {

        private Table _table;
        private readonly IAknamezoDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;
        private int _bombRate = 4;

        public event EventHandler<AknamezoEventArgs>? FastGameAdvanced;
        public event EventHandler<AknamezoEventArgs>? STDGameAdvanced;
        public event EventHandler<AknamezoEventArgs>? GameAdvanced;
        public event EventHandler<AknamezoEventArgs>? GameOver;


        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public int BombRate {  get { return _bombRate; } set { _bombRate = value; } }

        public int TimeInterval { get; set; }

        public List<Meteor> Meteors { get; private set; }
        public List<Ship> Ships { get; private set; }

        public GameModel(IAknamezoDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new Table(size);
            Meteors = new List<Meteor>();
            Ships = new List<Ship>();
            _table.GameTime = 0;
            BombRate = 4;

            //GenerateFields();
        }

        public Table Table { get { return _table; } }

        public void GenerateFields()
        {
            Random random = new Random();
            int x, y, n = _table.GetSize;

            for (int i = 0; i < GetSize; i++)
            {
                for (int j = 0; j < GetSize; j++)
                {
                    _table.SetTableValue(i, j, 0);
                }
            }

            _table.PCurrent[0] = (GetSize-1); _table.PCurrent[1] = (GetSize/2);
            _table.SetTableValue((GetSize/2), (GetSize-1), 1);
            GenerateShips();
        }

        public void SetUpTable()
        {
            for (int i = 0; i < GetSize; i++)
            {
                for (int j = 0; j < GetSize; j++)
                {
                    if( _table.GetTableValue(i,j) == 5)
                    {
                        Meteor meteor = new Meteor(i, j,BombSizes.Big);
                        meteor.MeteorStepEvent += OnMeteorStep;
                        Meteors.Add(meteor);
                    }
                    if (_table.GetTableValue(i, j) == 4)
                    {
                        Meteor meteor = new Meteor(i, j, BombSizes.Medium);
                        meteor.MeteorStepEvent += OnMeteorStep;
                        Meteors.Add(meteor);
                    }
                    if (_table.GetTableValue(i, j)==3)
                    {
                        Ship ship = new Ship(i, j);
                        ship.ShipStepEvent += OnShipStep;
                        Ships.Add(ship);
                    }
                    if (_table.GetTableValue(i, j)==2)
                    {
                        Meteor meteor = new Meteor(i, j, BombSizes.Small);
                        meteor.MeteorStepEvent += OnMeteorStep;
                        Meteors.Add(meteor);
                    }
                }
            }

            _table.SetTableValue(_table.PCurrent[1], _table.PCurrent[0], 1);
            
        }

        public void GenerateShips()
        {
            Random random = new Random();
            int shipCount = random.Next(5, GetSize-5);
            var positions = Enumerable.Range(0, Table.GetSize).ToList();

            for (int i = 0; i < shipCount; i++)
            {
                int positionIndex = random.Next(0, positions.Count);
                int position = positions[positionIndex];
                positions.RemoveAt(positionIndex);

                Ship ship = new Ship(position, 0);
                ship.ShipStepEvent += OnShipStep;
                Ships.Add(ship);
                Table.SetTableValue(position, 0, 3);
            }
        }

        public void GenerateMeteors()
        {
            for (int i = 0; i < GetSize; i++)
            {
                if(_table.GetTableValue(i,0) == 3)
                {
                    Meteor meteor = new Meteor(i, 1);
                    meteor.MeteorStepEvent += OnMeteorStep;
                    Meteors.Add(meteor);
                    if(meteor.BombSize == BombSizes.Small)
                    {
                        Table.SetTableValue(i, 1, 2);
                    }
                    else if (meteor.BombSize == BombSizes.Medium)
                    {
                        Table.SetTableValue(i, 1, 4);
                    }
                    else if (meteor.BombSize == BombSizes.Big)
                    {
                        Table.SetTableValue(i, 1, 5);
                    }
                }
            }
        }

        public void AdvanceFastestTime()
        {
            _table.FastTime++;

            for (int i = Meteors.Count - 1; i >= 0; i--)
            {
                if (Meteors[i].BombSize == BombSizes.Big)
                {
                    if (Meteors[i].PositionY == GetSize - 1)
                    {
                        Meteors.RemoveAt(i);
                        _table.SetTableValue(Meteors[i].PositionX, Meteors[i].PositionY, 0);
                    }
                    else
                    {
                        Meteors[i].Step();
                    }
                }

            }

            OnFastGameAdvanced();
        }

        public void AdvanceNormTime()
        {
            

            for (int i = Ships.Count - 1; i >= 0; i--)
            {
                Ships[i].Step();
            }

            for (int i = Meteors.Count - 1; i >= 0; i--)
            {
                if(Meteors[i].BombSize == BombSizes.Small)
                {
                    if (Meteors[i].PositionY == GetSize - 1)
                    {
                        Meteors.RemoveAt(i);
                        _table.SetTableValue(Meteors[i].PositionX, Meteors[i].PositionY, 0);
                    }
                    else
                    {
                        Meteors[i].Step();
                    }
                }
                
            }

            

            if (_table.StdTime % 4 == 0 && _table.StdTime < 30)
            {
                GenerateMeteors();
            }
            else if (_table.StdTime >= 30 && _table.StdTime % Math.Max(1, BombRate) == 0)
            {
                GenerateMeteors();
            }
            if(_table.StdTime % 2 == 0)
                BombRate--;

            _table.StdTime++;
            OnSTDGameAdvanced();
        }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            TakeDownAsteroids();

            _table.GameTime++;

            Table.Interval-=50;
            TimeInterval = Math.Max(100, Table.Interval);

            for (int i = Meteors.Count - 1; i >= 0; i--)
            {
                if (Meteors[i].BombSize == BombSizes.Medium)
                {
                    if (Meteors[i].PositionY == GetSize - 1)
                    {
                        Meteors.RemoveAt(i);
                        _table.SetTableValue(Meteors[i].PositionX, Meteors[i].PositionY, 0);
                    }
                    else
                    {
                        Meteors[i].Step();
                    }
                }

            }

            OnGameAdvanced();
        }

        public void TakeDownAsteroids()
        {
            for (int i = 0; i < GetSize; i++)
            {
                if (_table.GetTableValue(i, GetSize-1) == 2 || _table.GetTableValue(i, GetSize-1) == 4 || _table.GetTableValue(i, GetSize-1) == 5)
                {
                    _table.SetTableValue(i, GetSize-1, 0);
                }
            }
        }

        public void MovePlayer(int x, int y)
        {
            if (x >= 0 && x < GetSize && y >= 1 && y < GetSize)
            {
                if (_table.GetTableValue(x, y) == 2 || _table.GetTableValue(x, y) == 4 || _table.GetTableValue(x, y) == 5)
                {
                    IsGameOver = true; OnGameOver();
                }

                _table.SetTableValue(_table.PCurrent[1], _table.PCurrent[0], 0);
                _table.SetTableValue(x, y, 1);
                _table.PCurrent[1] = x; _table.PCurrent[0] = y;
            }
        }

        public void OnShipStep(object? sender, ShipStepEventArgs s)
        {
            if (sender is Ship ship)
            {
                int X = ship.PositionX;
                int Y = ship.PositionY;

                int toX = s.x;
                int toY = s.y;

                if (toX < 0 || toX >= GetSize || _table.GetTableValue(toX,toY) != 0)
                {
                    ship.ChangeDirection();
                }
                else
                {
                    _table.SetTableValue(X, Y, 0);
                    _table.SetTableValue(toX, toY, 3);
                    ship.StepTo(s.x, s.y);
                } 
            }
        }

        public void OnMeteorStep(object? sender, MeteorStepEventArgs e)
        {
            if (sender is Meteor meteor)
            {
                int X = meteor.PositionX;
                int Y = meteor.PositionY;

                int toX = e.x;
                int toY = e.y;

                if(toY == GetSize-1)
                {
                    if (_table.GetTableValue(toX, toY) == 1)
                    {
                        _table.SetTableValue(X, Y, 0);
                        _table.SetTableValue(toX, toY, 1);
                        
                        IsGameOver = true; OnGameOver();
                    }
                    else
                    {
                        _table.SetTableValue(X, Y, 0);

                        if (meteor.BombSize == BombSizes.Small)
                        {
                            _table.SetTableValue(toX, toY, 2);
                        }
                        else if(meteor.BombSize == BombSizes.Medium)
                        {
                            _table.SetTableValue(toX, toY, 4);
                        }
                        else if(meteor.BombSize == BombSizes.Big)
                        {
                            _table.SetTableValue(toX, toY, 5);
                        }
                    }
                    Meteors.Remove(meteor);
                }

                //if (toX > GetSize-1)
                //{
                //    _table.SetTableValue(X, Y, 0);
                //}

                if (toY < GetSize-1)
                {
                    //meteor.StepTo(toX, toY);

                    _table.SetTableValue(X, Y, 0);
                    if (_table.GetTableValue(toX, toY) == 1)
                    {
                        _table.SetTableValue(X, Y, 0);
                        _table.SetTableValue(toX, toY, 1);

                        IsGameOver = true; OnGameOver();
                    }

                    if (meteor.BombSize == BombSizes.Small)
                    {
                        _table.SetTableValue(toX, toY, 2);
                    }
                    else if (meteor.BombSize == BombSizes.Medium)
                    {
                        _table.SetTableValue(toX, toY, 4);
                    }
                    else if (meteor.BombSize == BombSizes.Big)
                    {
                        _table.SetTableValue(toX, toY, 5);
                    }

                    
                    meteor.StepTo(toX, toY);
                }

            }
        }

        private void OnFastGameAdvanced()
        {
            FastGameAdvanced?.Invoke(this, new AknamezoEventArgs(false));
        }

        private void OnSTDGameAdvanced()
        {
            STDGameAdvanced?.Invoke(this, new AknamezoEventArgs(false));
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new AknamezoEventArgs(false));
        }

        public virtual void OnGameOver()
        {
            GameOver?.Invoke(this, new AknamezoEventArgs(true));
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

            Meteors.RemoveAll(e => true);

            _table = await _dataAccess.LoadAsync(path);
            _size = _table.GetSize;
            _isGameOver = false;
            SetUpTable();
        }

    }
}