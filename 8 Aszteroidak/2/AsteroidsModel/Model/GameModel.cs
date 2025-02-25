using AsteroidsModel.Model;
using AsteroidsModel.Persistence;
using Castle.Components.DictionaryAdapter.Xml;
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
using static AsteroidsModel.Model.Meteor;
using static System.Reflection.Metadata.BlobBuilder;

namespace AsteroidsModel.Model
{
    public class GameModel
    {

        private Table _table;
        private readonly IAsteroidsDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;
        private int _minRand;

        public event EventHandler<AsteroidsEventArgs>? GameAdvanced;
        public event EventHandler<AsteroidsEventArgs>? GameOver;


        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public List<Meteor> Meteors { get; private set; }

        public GameModel(IAsteroidsDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new Table(size);
            Meteors = new List<Meteor>();
            _minRand = Table.GetSize/2;
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
            _table.SetTableValue((GetSize-1), (GetSize/2), 1);
        }

        public void SetUpTable()
        {
            for (int i = 0; i < GetSize; i++)
            {
                for (int j = 0; j < GetSize; j++)
                {
                    if(_table.GetTableValue(i, j)==2)
                    {
                        Meteor meteor = new Meteor(i, j);
                        meteor.MeteorStepEvent += OnMeteorStep;
                        Meteors.Add(meteor);
                    }
                }
            }
            
            

        }

        public void GenerateMeteors()
        {
            Random random = new Random();
            var positions = Enumerable.Range(0, Table.GetSize).ToList();
            int numberOfEnemies;

            if (Table.GameTime % 2 == 0)
            {
                _minRand++;
            }

            if (_table.GameTime >= 10)
            {
                if (_minRand >= Table.GetSize-1)
                {
                    numberOfEnemies = Table.GetSize;
                }
                else
                {
                    numberOfEnemies = random.Next(_minRand, Table.GetSize-1);
                }

                for (int i = 0; i < numberOfEnemies; i++)
                {
                    int positionIndex = random.Next(0, positions.Count);
                    int position = positions[positionIndex];
                    positions.RemoveAt(positionIndex);

                    Meteor meteor = new Meteor(0, position);
                    meteor.MeteorStepEvent += OnMeteorStep;
                    Meteors.Add(meteor);
                    Table.SetTableValue(0, position, 2);
                }
            }
            else
            {
                numberOfEnemies = random.Next(0, Table.GetSize / 2);

                for (int i = 0; i < numberOfEnemies; i++)
                {
                    int positionIndex = random.Next(0, positions.Count);
                    int position = positions[positionIndex];
                    positions.RemoveAt(positionIndex);

                    Meteor meteor = new Meteor(0, position);
                    meteor.MeteorStepEvent += OnMeteorStep;
                    Meteors.Add(meteor);
                    Table.SetTableValue(0, position, 2);
                }
            }
        }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            TakeDownAsteroids();

            _table.GameTime++;

           

            for (int i = Meteors.Count - 1; i >= 0; i--)
            {
                Meteors[i].Step();

                if (Meteors[i].PositionX == GetSize - 1)
                {
                    Meteors.RemoveAt(i);
                    _table.SetTableValue(Meteors[i].PositionX, Meteors[i].PositionY, 0);
                }
            }

            if (_table.GameTime % 6 == 0)
            {
                GenerateMeteors();
            }



            OnGameAdvanced();
        }

        public void TakeDownAsteroids()
        {
            for (int i = 0; i < GetSize; i++)
            {
                if (_table.GetTableValue(GetSize-1,i) == 2)
                {
                    _table.SetTableValue(GetSize-1,i, 0);
                }
            }
        }

        public void MovePlayer(int x, int y)
        {
            if (x >= 0 && x < GetSize && y >= 0 && y < GetSize)
            {
                if (_table.GetTableValue(x, y) == 2)
                {
                    IsGameOver = true; OnGameOver();
                }

                _table.SetTableValue(_table.PCurrent[0], _table.PCurrent[1], 0);
                _table.SetTableValue(x, y, 1);
                _table.PCurrent[0] = x; _table.PCurrent[1] = y;
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

                if(toX == GetSize-1)
                {
                    if (_table.GetTableValue(toX, toY) == 1)
                    {
                        IsGameOver = true; OnGameOver();
                    }
                    Meteors.Remove(meteor);
                    _table.SetTableValue(toX, toY, 2);
                    _table.SetTableValue(X, Y, 0);
                    return;
                }
                

                if (toX < GetSize-1)
                {
                    meteor.StepTo(toX, toY);
                    _table.SetTableValue(X, Y, 0);
                    _table.SetTableValue(toX, toY, 2);
                }

            }
        }


        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new AsteroidsEventArgs(false));
        }

        public virtual void OnGameOver()
        {
            GameOver?.Invoke(this, new AsteroidsEventArgs(true));
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