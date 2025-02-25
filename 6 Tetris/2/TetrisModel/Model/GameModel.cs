using TetrisModel.Model;
using TetrisModel.Persistence;
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
using static TetrisModel.Model.Shape;
using static TetrisModel.Model.Ship;
using static System.Reflection.Metadata.BlobBuilder;
using System.Diagnostics.Metrics;

namespace TetrisModel.Model
{
    public class GameModel
    {
        public enum Shapes
        {
            Square,
            Line,
            L,
            J,
            Roof,
            S,
            Z
        };

        private Table _table;
        private readonly ITetrisDataAccess? _dataAccess;
        private int _sizeX;
        private int _sizeY;
        private bool _isGameOver = false;
        private int _bombRate = 4;
        private Shapes _shape;

        public event EventHandler<TetrisEventArgs>? FastGameAdvanced;
        public event EventHandler<TetrisEventArgs>? STDGameAdvanced;
        public event EventHandler<TetrisEventArgs>? GameAdvanced;
        public event EventHandler<TetrisEventArgs>? GameOver;


        public bool IsGameOver { get; set; }

        public int GetSizeX
        {
            get { return _sizeX; }
        }

        public int GetSizeY
        {
            get { return _sizeY; }
        }

        public int BombRate { get { return _bombRate; } set { _bombRate = value; } }

        public int TimeInterval { get; set; }
        public bool CanStartFalling { get; set; }

        public List<(int, int)> ShapeElements { get; private set; }
        public List<Shape> ShapeList { get; private set; }

        public GameModel(ITetrisDataAccess? dataAccess, int sizeX, int sizeY)
        {
            _isGameOver = false;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _dataAccess = dataAccess;
            _table = new Table(sizeX, sizeY);
            ShapeElements = new List<(int, int)>();
            ShapeList = new List<Shape>();
            _table.GameTime = 0;
            BombRate = 4;
            CanStartFalling = true;

            //GenerateFields();
        }

        public Table Table { get { return _table; } }

        public void GenerateFields()
        {
            Random random = new Random();

            for (int i = 0; i < GetSizeX; i++)
            {
                for (int j = 0; j < GetSizeY; j++)
                {
                    _table.SetTableValue(i, j, 0);
                }
            }
        }

        public void SetUpTable()
        {


        }

        public void GenerateShape()
        {
            Random random = new Random();
            _shape = (Shapes)random.Next(7);
            int id = 0;

            if (_shape == Shapes.Square)
            {
                ShapeElements.AddRange(new List<(int, int)>
                {
                    (0, GetSizeY/2),
                    (0, (GetSizeY/2)-1),
                    (1, GetSizeY/2),
                    (1, (GetSizeY/2)-1)
                });
                id=1;
                //Shape shape = new Shape(ShapeElements);
            }
            else if (_shape == Shapes.Line)
            {
                ShapeElements.AddRange(new List<(int, int)>
                {
                    (0, (GetSizeY/2)+1),
                    (0, GetSizeY/2),
                    (0, (GetSizeY/2)-1),
                    (0, (GetSizeY/2)-2)
                });
                id=2;
            }
            else if (_shape == Shapes.L)
            {
                ShapeElements.AddRange(new List<(int, int)>
                {
                    (0, (GetSizeY/2)-1),
                    (1, (GetSizeY/2)-1),
                    (2, (GetSizeY/2)-1),
                    (2, GetSizeY/2)
                });
                id=3;
            }
            else if (_shape == Shapes.J)
            {
                ShapeElements.AddRange(new List<(int, int)>
                {
                    (0, (GetSizeY/2)),
                    (1, (GetSizeY/2)),
                    (2, (GetSizeY/2)),
                    (2, GetSizeY/2-1)
                });
                id=4;
            }
            else if (_shape == Shapes.Roof)
            {
                ShapeElements.AddRange(new List<(int, int)>
                {
                    (0, (GetSizeY/2)-1),
                    (1, GetSizeY/2),
                    (1, (GetSizeY/2)-1),
                    (1, (GetSizeY/2)-2)
                });
                id=5;
            }
            else if (_shape == Shapes.S)
            {
                ShapeElements.AddRange(new List<(int, int)>
                {
                    (0, (GetSizeY/2)),
                    (0, (GetSizeY/2)-1),
                    (1, (GetSizeY/2)-1),
                    (1, (GetSizeY/2)-2)
                });
                id=6;
            }
            else if (_shape == Shapes.Z)
            {
                ShapeElements.AddRange(new List<(int, int)>
                {
                    (0, (GetSizeY/2)-1),
                    (0, (GetSizeY/2)),
                    (1, (GetSizeY/2)),
                    (1, (GetSizeY/2)+1)
                });
                id=7;
            }

            Shape shape = new Shape(ShapeElements,id,1);
            shape.MeteorStepEvent += OnShapeStep;
            ShapeList.Add(shape);

            for (int i = 0; i < ShapeElements.Count; i++)
            {
                if (Table.GetTableValue(ShapeElements[i].Item1, ShapeElements[i].Item2) != 0)
                {
                    IsGameOver = true; OnGameOver(); return;
                }
            }

            for (int i = 0; i < ShapeElements.Count; i++)
            {
                Table.SetTableValue(ShapeElements[i].Item1, ShapeElements[i].Item2, 1);
            }
        }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            _table.GameTime++;

            if (CanStartFalling)
            {
                CanStartFalling = false;
                GenerateShape();
            }
            else
            {
                ShapeList[0].StepShape();
            }

            OnGameAdvanced();
        }

        public void OnShapeStep(object? sender, ShapeStepEventArgs e)
        {
            if (sender is Shape shape)
            {
                int X = shape.PositionX;
                int Y = shape.PositionY;

                int toX = e.x;
                int toY = e.y;
                bool reachedEnd = false;
                int counter = 0;

                for (int i = 0; i < shape.ShapeElements.Count; i++)
                {
                    if (shape.ShapeElements[i].Item1 + 1 < GetSizeX && !Table.GetOccupied(shape.ShapeElements[i].Item1 + 1, shape.ShapeElements[i].Item2))
                    {
                        counter++;
                    }
                }

                if (counter != shape.ShapeElements.Count)
                {
                    for (int j = 0; j < shape.ShapeElements.Count; j++)
                    {
                        Table.SetOccupied(shape.ShapeElements[j].Item1, shape.ShapeElements[j].Item2, true);
                    }
                    ShapeElements.Clear();
                    ShapeList.Remove(shape);
                    CanStartFalling = true;
                    ClearFull();
                }
                else if (counter == shape.ShapeElements.Count)
                {
                    for (int i = 0; i < shape.ShapeElements.Count; i++)
                    {
                        Table.SetTableValue(ShapeElements[i].Item1, ShapeElements[i].Item2, 0);
                    }
                    shape.StepTo();
                    for (int i = 0; i < shape.ShapeElements.Count; i++)
                    {
                        Table.SetTableValue(ShapeElements[i].Item1, ShapeElements[i].Item2, 1);
                    }

                }
            }
        }

        public void MoveShape(Shape shape, int x, int y)
        {
            bool canMove = true;
            foreach (var element in shape.ShapeElements)
            {
                int newX = element.Item1 + x;
                int newY = element.Item2 + y;

                if (newX < 0 || newX >= GetSizeX || newY < 0 || newY >= GetSizeY || Table.GetOccupied(newX, newY))
                {
                    canMove = false;
                    break;
                }
            }

            if (canMove)
            {
                foreach (var element in shape.ShapeElements)
                {
                    Table.SetTableValue(element.Item1, element.Item2, 0);
                }

                //alakzat elemeinek pozíciójának frissítése
                for (int i = 0; i < shape.ShapeElements.Count; i++)
                {
                    shape.ShapeElements[i] = (shape.ShapeElements[i].Item1 + x, shape.ShapeElements[i].Item2 + y);
                }

                foreach (var element in shape.ShapeElements)
                {
                    Table.SetTableValue(element.Item1, element.Item2, 1);
                }
            }
        }

        public void RotateShape(Shape shape)
        {
            if(shape.Id == 1)
            {
                IBlock(shape);
            }
            else if (shape.Id == 2)
            {

            }
            else if (shape.Id == 3)
            {

            }
            else if (shape.Id == 4)
            {

            }
            else if (shape.Id == 5)
            {

            }
            else if (shape.Id == 6)
            {

            }
            else if (shape.Id == 7)
            {

            }
        }

        private void IBlock(Shape shape)
        {
            if (shape.State == 0)
            {
                
            }
            else if(shape.State == 1)
            {

            }
            else if (shape.State == 2)
            {

            }
            else if (shape.State == 3)
            {

            }
            shape.ChangeState();
        }

        private bool IsRowEmpty(int r)
        {
            for (int j = 0; j < GetSizeY; j++)
            {
                if (Table.GetTableValue(r, j) != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsRowFull(int r)
        {
            for (int j = 0; j < GetSizeY; j++)
            {
                if (Table.GetTableValue(r, j) != 1)
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearRow(int r)
        {
            for (int j = 0; j < GetSizeY; j++)
            {
                Table.SetTableValue(r, j, 0);
                Table.SetOccupied(r, j, false);
            }
        }

        private void MoveDown(int r, int numRows)
        {
            for (int j = 0; j < GetSizeY; j++)
            {
                if(Table.GetTableValue(r, j) == 1)
                {
                    Table.SetTableValue(r, j, 0);
                    Table.SetOccupied(r, j, false);
                    Table.SetTableValue(r+numRows, j, 1);
                    Table.SetOccupied(r+numRows, j, true);
                }
            }
        }

        private void ClearFull()
        {
            int cleared = 0;
            for (int i = GetSizeX-1; i >= 0; i--)
            {
                if (IsRowFull(i))
                {
                    ClearRow(i);
                    cleared++;
                }
                if(cleared > 0 && !IsRowEmpty(i))
                {
                    MoveDown(i, cleared);
                }
            }
        }

        private void OnFastGameAdvanced()
        {
            FastGameAdvanced?.Invoke(this, new TetrisEventArgs(false));
        }

        private void OnSTDGameAdvanced()
        {
            STDGameAdvanced?.Invoke(this, new TetrisEventArgs(false));
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new TetrisEventArgs(false));
        }

        public virtual void OnGameOver()
        {
            GameOver?.Invoke(this, new TetrisEventArgs(true));
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

            //Shape.RemoveAll(e => true);

            _table = await _dataAccess.LoadAsync(path);
            _sizeX = _table.GetSizeX;
            _sizeY = _table.GetSizeY;
            _isGameOver = false;
            SetUpTable();
        }

    }
}