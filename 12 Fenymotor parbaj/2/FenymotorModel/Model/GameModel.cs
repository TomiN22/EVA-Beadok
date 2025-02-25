using FenymotorModel.Model;
using FenymotorModel.Persistence;
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
using static FenymotorModel.Model.Player;
using static FenymotorModel.Persistence.Table;
using static System.Reflection.Metadata.BlobBuilder;

namespace FenymotorModel.Model
{
    public class GameModel
    {
        
        private Table _table;
        private readonly IFenymotorDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;

        public event EventHandler<FenymotorEventArgs>? GameAdvanced;
        public event EventHandler<FenymotorEventArgs>? GameOver;

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public List<Player> Players { get; private set; }

        public GameModel(IFenymotorDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new Table(size);
            Players = new List<Player>();
            //GenerateFields();
        }

        public Table Table { get { return _table; } }

        public void GenerateFields()
        {
            Table.SetTableValue(GetSize/2, 0, 1);
            Player p1 = new Player(GetSize/2,0,Player.Directions.Right,1);
            p1.PlayerStepEvent += OnPlayerStep;
            Players.Add(p1);

            Table.SetTableValue(GetSize/2, GetSize-1, 2);
            Player p2 = new Player(GetSize/2, GetSize-1, Player.Directions.Left,2);
            p2.PlayerStepEvent += OnPlayerStep;
            Players.Add(p2);
        }

        public void SetUpTable()
        {
            for (int i = 0; i < GetSize; i++)
            {
                for(int j = 0; j < GetSize; j++)
                {
                    if (Table.GetTableValue(i, j)==1)
                    {
                        Player p1 = new Player(i, j, (Player.Directions)Table.P1Direction, 1);
                        p1.PlayerStepEvent += OnPlayerStep;
                        Players.Add(p1);
                    }
                    if(Table.GetTableValue(i, j)==2)
                    {
                        Player p2 = new Player(i, j, (Player.Directions)Table.P2Direction, 2);
                        p2.PlayerStepEvent += OnPlayerStep;
                        Players.Add(p2);
                    }
                }
            }
            
        }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            Table.GameTime++;
            foreach (Player player in Players)
            {
                if (!IsGameOver)
                    player.WouldStep();
            }

            OnGameAdvanced();
        }

        public void KeyCommand(string dir, int id)
        {
            if(Players[0].Id == id)
            {
                Players[0].ChangeDirection(dir);
                Table.P1Direction = (Table.Directions)Players[0].Direction;
            }
            else if(Players[1].Id == id)
            {
                Players[1].ChangeDirection(dir);
                Table.P2Direction = (Table.Directions)Players[1].Direction;
            }            
        }

        public void OnPlayerStep(object? sender, PlayerStepEventArgs e)
        {
            if (sender is Player player)
            {
                int X = player.PositionX;
                int Y = player.PositionY;

                int toX = e.x;
                int toY = e.y;
                if (toX < 0 || toX >= GetSize || toY < 0 || toY >= GetSize || _table.GetTableValue(toX, toY) == 11 || _table.GetTableValue(toX, toY) == 22 ||
                    (_table.GetTableValue(toX, toY) == 1 || _table.GetTableValue(toX, toY) == 2 &&
                    !(Players[0].Direction == Player.Directions.Right && Players[1].Direction == Player.Directions.Left) && !(Players[0].Direction == Player.Directions.Left && Players[1].Direction == Player.Directions.Right)
                    && !(Players[0].Direction == Player.Directions.Up && Players[1].Direction == Player.Directions.Down) && !(Players[0].Direction == Player.Directions.Down && Players[1].Direction == Player.Directions.Up)))
                {
                    IsGameOver = true;

                    if (player.Id == 1)
                        OnGameOver(Win.Player2);
                    else
                        OnGameOver(Win.Player1);

                    return;
                }
                else if ((_table.GetTableValue(toX, toY) == 1 || _table.GetTableValue(toX, toY) == 2) && IsGameOver == false)
                {
                    IsGameOver = true; OnGameOver(Win.Draw); return;
                }
                else
                {
                    if(player.Id == 1)
                    {
                        _table.SetTableValue(X, Y, 11);
                        _table.SetTableValue(toX, toY, 1);
                    }
                    else
                    {
                        _table.SetTableValue(X, Y, 22);
                        _table.SetTableValue(toX, toY, 2);
                    }

                    player.StepTo(e.x, e.y);
                }

            }
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new FenymotorEventArgs());
        }

        public virtual void OnGameOver(Win winner)
        {
            GameOver?.Invoke(this, new FenymotorEventArgs(winner));
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

            Players.RemoveAll(e => true);

            _table = await _dataAccess.LoadAsync(path);
            _size = _table.GetSize;
            _isGameOver = false;
            //SetUpTable();
        }

        //public void LoadGame(String path) //async Taks ????????? or void? but cannot wait void...........
        //{
        //    if (_dataAccess == null)
        //        throw new InvalidOperationException("No data access is provided.");

        //    _table = _dataAccess.LoadFenymotorTable(path);
        //    _size = _table.GetSize;
        //    //_gameStepCount = 0;
        //    _isGameOver = false;

        //}


    }
}