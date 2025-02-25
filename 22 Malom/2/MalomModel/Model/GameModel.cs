using MalomModel.Model;
using MalomModel.Persistence;
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
using static MalomModel.Model.Figure;
using static MalomModel.Persistence.Table;
using static System.Reflection.Metadata.BlobBuilder;

namespace MalomModel.Model
{
    public class GameModel
    {
        private Table _table;
        private readonly IMalomDataAccess? _dataAccess;
        private int _size;
        private bool _isGameOver = false;

        public event EventHandler<MalomEventArgs>? GameAdvanced;
        public event EventHandler<MalomEventArgs>? GameOver;

        public bool IsGameOver { get; set; }

        public int GetSize
        {
            get { return _size; }
        }

        public int CurrentFigureIndex { get; set; }

        public bool CanTakeDown { get; set; }
        public bool IsMill { get; set; }

        public List<Figure> Figures { get; set; }

        public GameModel(IMalomDataAccess? dataAccess, int size)
        {
            _isGameOver = false;
            _size = size;
            _dataAccess = dataAccess;
            _table = new Table(size);
            CurrentFigureIndex = 1;
            CanTakeDown = false;
            IsMill = false;
            Figures = new List<Figure>();
            //GenerateFields();
        }

        public Table Table { get { return _table; } }

        public void GenerateFields()
        {
            int left = 0;
            int right = GetSize - 1;
            int top = 0;
            int bottom = GetSize - 1;
            int color = 2; // Kezdő szín

            while (left <= right && top <= bottom)
            {
                // Felső sor
                for (int j = left; j <= right; j++)
                {
                    Table.SetTableColor(top, j, color);
                    if (top % 2 != 0 && top < 4)
                    {
                        Table.SetTableColor(top, (GetSize-1)/2, 2);
                    }
                }
                top++;

                // Jobb oszlop
                for (int i = top; i <= bottom; i++)
                {
                    Table.SetTableColor(i, right, color);
                    if (right % 2 != 0 && right > 7)
                    {
                        Table.SetTableColor((GetSize-1)/2, right, 2);
                    }
                }
                right--;

                // Alsó sor
                if (top <= bottom)
                {
                    for (int j = right; j >= left; j--)
                    {
                        Table.SetTableColor(bottom, j, color);
                        if (color == 2 && j % 6 == 0)
                        {
                            Table.SetTableColor(bottom, j, 3);
                        }
                    }
                    if (bottom % 2 != 0 && bottom > 7)
                    {
                        Table.SetTableColor(bottom, (GetSize-1)/2, 2);
                    }
                    bottom--;
                }

                // Bal oszlop
                if (left <= right)
                {
                    for (int i = bottom; i >= top; i--)
                    {
                        Table.SetTableColor(i, left, color);
                    }
                    if (left % 2 != 0 && left < 4)
                    {
                        Table.SetTableColor((GetSize-1)/2, left, 2);
                    }
                    left++;
                }

                // Színek váltása
                color = (color == 2) ? 1 : 2; // 2-ről 1-re, majd vissza

                // Középső elem színezése, ha elértük a közepet és GetSize páratlan --- még jó, h nem működik
                if (left == right && top == bottom)
                {
                    Table.SetTableColor(top, left, 1);
                }
            }

            for (int i = 0; i < GetSize; i++)
            {
                for (int j = 0; j < GetSize; j++)
                {
                    if(Table.GetTableColor(i,j) == 2)
                    {
                        //left diagonal
                        if(i == j)
                            Table.SetTableColor(i, j, 3);
                        //right diagonal
                        if(i+j == 12)
                            Table.SetTableColor(i, j, 3);
                        //vertical
                        if (j==(GetSize-1)/2 && i % 2 == 0)
                            Table.SetTableColor(i, j, 3);
                        //horizontal
                        if (i==(GetSize-1)/2 && j % 2 == 0)
                            Table.SetTableColor(i, j, 3);
                    }
                }
            }
            Table.SetTableColor((GetSize-1)/2, (GetSize-1)/2, 1);
        }

        public void SetUpTable()
        {

        }

        public void AdvanceTime()
        {
            if (_isGameOver)
                return;

            if(Table.Player == 1)
                Table.P1Time++;
            if(Table.Player == 2)
                Table.P2Time++;

            OnGameAdvanced();
        }

        public void Step(int x, int y)
        {
            if (!IsGameOver)
            {
                if(Table.Player == 1)
                {
                    if (Table.GetTableColor(x, y) == 3 && Table.P1PlaceDown > 0 && !CanTakeDown)
                    {
                        Table.SetTableColor(x, y, 4);
                        Table.P1PlaceDown--;
                        CheckMill(x, y, 4);
                        //CHeckAllMills(4);
                        if (!CanTakeDown)
                            Table.ChangePlayer();
                    }
                    else if (CanTakeDown && Table.GetTableColor(x, y) == 5 && Table.GetTableCanRemove(x, y) == true)
                    {
                        Table.SetTableColor(x, y, 3);
                        Table.P2Count--;
                        //Table.P2PlaceDown--;
                        CanTakeDown = false;
                        Table.ChangePlayer();
                    }
                    else if (Table.GetTableColor(x, y) == 4 && !CanTakeDown && Table.P1PlaceDown == 0)
                    {
                        Table.P1Current[0] = x; Table.P1Current[1] = y;
                    }
                    else if (Table.GetTableColor(x, y) == 3 && !CanTakeDown && Table.P1PlaceDown == 0)
                    {
                        if (Table.P1Current[0] == 0 || Table.P1Current[0] == 12 || Table.P1Current[1] == 0 || Table.P1Current[1] == 12)
                        {
                            OuterSquare(x, y, Table.P1Current[0], Table.P1Current[1], 4);
                            //CHeckAllMills(4);
                            if (!CanTakeDown)
                                Table.ChangePlayer();
                        }
                        else if(Table.P1Current[0] == 2 || Table.P1Current[0] == 10 || Table.P1Current[1] == 2 || Table.P1Current[1] == 10)
                        {
                            MiddleSquare(x, y, Table.P1Current[0], Table.P1Current[1], 4);
                            //CHeckAllMills(4);
                            if (!CanTakeDown)
                                Table.ChangePlayer();
                        }
                        else if (Table.P1Current[0] == 4 || Table.P1Current[0] == 8 || Table.P1Current[1] == 4 || Table.P1Current[1] == 8)
                        {
                            InnerSquare(x, y, Table.P1Current[0], Table.P1Current[1], 4);
                            //CHeckAllMills(4);
                            if (!CanTakeDown)
                                Table.ChangePlayer();
                        }
                    }
                }
                else if(Table.Player == 2)
                {
                    if (Table.GetTableColor(x, y) == 3 && Table.P2PlaceDown > 0 && !CanTakeDown)
                    {
                        Table.SetTableColor(x, y, 5);
                        Table.P2PlaceDown--;
                        CheckMill(x, y, 5);
                        //CHeckAllMills(5);
                        if (!CanTakeDown)
                            Table.ChangePlayer();
                    }
                    else if (CanTakeDown && Table.GetTableColor(x, y) == 4 && Table.GetTableCanRemove(x, y) == true)
                    {
                        Table.SetTableColor(x, y, 3);
                        Table.P1Count--;
                        //Table.P1PlaceDown--;
                        CanTakeDown = false;
                        Table.ChangePlayer();
                    }
                    else if (Table.GetTableColor(x, y) == 5 && !CanTakeDown && Table.P2PlaceDown == 0)
                    {
                        Table.P2Current[0] = x; Table.P2Current[1] = y;
                    }
                    else if (Table.GetTableColor(x, y) == 3 && !CanTakeDown && Table.P2PlaceDown == 0)
                    {
                        if (Table.P2Current[0] == 0 || Table.P2Current[0] == 12 || Table.P2Current[1] == 0 || Table.P2Current[1] == 12)
                        {
                            OuterSquare(x, y, Table.P2Current[0], Table.P2Current[1], 5);
                            //CHeckAllMills(5);
                            if (!CanTakeDown)
                                Table.ChangePlayer();
                        }
                        else if (Table.P2Current[0] == 2 || Table.P2Current[0] == 10 || Table.P2Current[1] == 2 || Table.P2Current[1] == 10)
                        {
                            MiddleSquare(x, y, Table.P2Current[0], Table.P2Current[1], 5);
                            //CHeckAllMills(5);
                            if (!CanTakeDown)
                                Table.ChangePlayer();
                        }
                        else if (Table.P2Current[0] == 4 || Table.P2Current[0] == 8 || Table.P2Current[1] == 4 || Table.P2Current[1] == 8)
                        {
                            InnerSquare(x, y, Table.P2Current[0], Table.P2Current[1], 5);
                            //CHeckAllMills(5);
                            if (!CanTakeDown)
                                Table.ChangePlayer();
                        }
                    }
                }
            }
        }

        private void OuterSquare(int x, int y, int playerCurrent0, int playerCurrent1, int value)
        {
            if ((x == playerCurrent0-6 && y == playerCurrent1) || (x == playerCurrent0+6 && y == playerCurrent1) || (x == playerCurrent0 && y == playerCurrent1-6) || (x == playerCurrent0 && y == playerCurrent1+6))
            {
                Table.SetTableCanRemove(x, y, true);
                Table.SetTableCanRemove(playerCurrent0, playerCurrent1, true);
                Table.SetTableColor(x, y, value);
                Table.SetTableColor(playerCurrent0, playerCurrent1, 3);

                if(x == playerCurrent0-6)
                {

                }
                //if()

                CheckMill(x, y, value);
            }
            else if ((x == playerCurrent0 && y == playerCurrent1+2) || (x == playerCurrent0 && y == playerCurrent1-2))
            {
                Table.SetTableCanRemove(x, y, true);
                Table.SetTableCanRemove(playerCurrent0, playerCurrent1, true);
                Table.SetTableColor(x, y, value);
                Table.SetTableColor(playerCurrent0, playerCurrent1, 3);

                if(y == playerCurrent1+2)
                {
                    if(Table.GetTableCanRemove(x-6, 6) == true && Table.GetTableCanRemove(x-6, 12) == true)
                    Table.SetTableCanRemove(x-6, playerCurrent1, true);

                }
                else if(y == playerCurrent1-2)
                {

                }

                CheckMill(x, y, value);
            }
            else if ((y == playerCurrent1 && x == playerCurrent0+2) || (y == playerCurrent1 && x == playerCurrent0-2))
            {
                Table.SetTableColor(x, y, value);
                Table.SetTableColor(playerCurrent0, playerCurrent1, 3);

                CheckMill(x, y, value);
            }
        }

        private void MiddleSquare(int x, int y, int playerCurrent0, int playerCurrent1, int value)
        {
            if ((x == playerCurrent0-4 && y == playerCurrent1) || (x == playerCurrent0+4 && y == playerCurrent1) || (x == playerCurrent0 && y == playerCurrent1-4) || (x == playerCurrent0 && y == playerCurrent1+4))
            {
                Table.SetTableColor(x, y, value);
                Table.SetTableColor(playerCurrent0, playerCurrent1, 3);

                CheckMill(x, y, value);
            }
            else if ((x == playerCurrent0 && y == playerCurrent1+2) || (x == playerCurrent0 && y == playerCurrent1-2))
            {
                Table.SetTableColor(x, y, value);
                Table.SetTableColor(playerCurrent0, playerCurrent1, 3);

                CheckMill(x, y, value);
            }
            else if ((y == playerCurrent1 && x == playerCurrent0+2) || (y == playerCurrent1 && x == playerCurrent0-2))
            {
                Table.SetTableColor(x, y, value);
                Table.SetTableColor(playerCurrent0, playerCurrent1, 3);

                CheckMill(x, y, value);
            }
        }

        private void InnerSquare(int x, int y, int playerCurrent0, int playerCurrent1, int value)
        {
            if ((x == playerCurrent0-2 && y == playerCurrent1) || (x == playerCurrent0+2 && y == playerCurrent1) || (x == playerCurrent0 && y == playerCurrent1-2) || (x == playerCurrent0 && y == playerCurrent1+2))
            {
                Table.SetTableColor(x, y, value);
                Table.SetTableColor(playerCurrent0, playerCurrent1, 3);

                CheckMill(x, y, value);
            }
        }

        private void CheckMill(int x, int y, int value)
        {
            if(x == 0)
            {
                if (y == 6 && Table.GetTableColor(x+2, y) == value && Table.GetTableColor(x+4, y) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x+6, y) == value && Table.GetTableColor(x+12, y) == value)
                    CanTakeDown = true;
            }
            if(x == 12)
            {
                if (y == 6 && Table.GetTableColor(x-2, y) == value && Table.GetTableColor(x-4, y) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x-6, y) == value && Table.GetTableColor(x-12, y) == value)
                    CanTakeDown = true;
            }
            if (y == 0)
            {
                if (x == 6 && Table.GetTableColor(x,y+2) == value && Table.GetTableColor(x,y+4) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x,y+6) == value && Table.GetTableColor(x,y+12) == value)
                    CanTakeDown = true;
            }
            if (y == 12)
            {
                if (x == 6 && Table.GetTableColor(x, y-2) == value && Table.GetTableColor(x, y-4) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x, y - 6) == value && Table.GetTableColor(x, y - 12) == value)
                    CanTakeDown = true;
            }

            //middle 
            if (x == 2)
            {
                if (y == 6 && Table.GetTableColor(x-2, y) == value && Table.GetTableColor(x+2, y) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x+4, y) == value && Table.GetTableColor(x+8, y) == value)
                    CanTakeDown = true;
            }
            if (x == 10)
            {
                if (y == 6 && Table.GetTableColor(x-2, y) == value && Table.GetTableColor(x+2, y) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x-4, y) == value && Table.GetTableColor(x-8, y) == value)
                    CanTakeDown = true;
            }

            if (y == 2)
            {
                if (x == 6 && Table.GetTableColor(x, y-2) == value && Table.GetTableColor(x, y+2) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x, y+4) == value && Table.GetTableColor(x, y+8) == value)
                    CanTakeDown = true;
            }
            if (y == 10)
            {
                if (x == 6 && Table.GetTableColor(x, y-2) == value && Table.GetTableColor(x, y+2) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x, y - 4) == value && Table.GetTableColor(x, y - 8) == value)
                    CanTakeDown = true;
            }

            //inner
            if (x == 4)
            {
                if (y == 6 && Table.GetTableColor(x-2, y) == value && Table.GetTableColor(x-4, y) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x+2, y) == value && Table.GetTableColor(x+4, y) == value)
                    CanTakeDown = true;
            }
            if (x == 8)
            {
                if (y == 6 && Table.GetTableColor(x+2, y) == value && Table.GetTableColor(x+4, y) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x-2, y) == value && Table.GetTableColor(x-4, y) == value)
                    CanTakeDown = true;
            }

            if (y == 4)
            {
                if (x == 6 && Table.GetTableColor(x, y-2) == value && Table.GetTableColor(x, y-4) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x, y+2) == value && Table.GetTableColor(x, y+4) == value)
                    CanTakeDown = true;
            }
            if (y == 8)
            {
                if (x == 6 && Table.GetTableColor(x, y+2) == value && Table.GetTableColor(x, y+4) == value)
                    CanTakeDown = true;
                if (Table.GetTableColor(x, y - 2) == value && Table.GetTableColor(x, y - 4) == value)
                    CanTakeDown = true;
            }
        }
        
        private void CHeckAllMills(int value)
        {
            for (int x = 0; x < GetSize; x+=2)
            {
                for (int y = 0; y < GetSize; y+=2)
                {
                    //outer
                    if (x == 0)
                    {
                        if (y == 6 && Table.GetTableColor(x+2, y) == value && Table.GetTableColor(x+4, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x+2, y, false); Table.SetTableCanRemove(x+4, y, false);
                        }
                        else if (y == 6 && Table.GetTableColor(x+2, y) == 3 && Table.GetTableColor(x+4, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x+2, y, true); Table.SetTableCanRemove(x+4, y, true);
                        }
                        if (Table.GetTableColor(x+6, y) == value && Table.GetTableColor(x+12, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x+6, y, false); Table.SetTableCanRemove(x+12, y, false);
                        }
                        else if (Table.GetTableColor(x+6, y) == 3 && Table.GetTableColor(x+12, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x+6, y, true); Table.SetTableCanRemove(x+12, y, true);
                        }
                    }
                    if (x == 12)
                    {
                        if (y == 6 && Table.GetTableColor(x-2, y) == value && Table.GetTableColor(x-4, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x-2, y, false); Table.SetTableCanRemove(x-4, y, false);
                        }
                        else if (y == 6 && Table.GetTableColor(x-2, y) == 3 && Table.GetTableColor(x-4, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x-2, y, true); Table.SetTableCanRemove(x-4, y, true);
                        }
                        if (Table.GetTableColor(x-6, y) == value && Table.GetTableColor(x-12, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x-6, y, false); Table.SetTableCanRemove(x-12, y, false);
                        }
                        else if (Table.GetTableColor(x-6, y) == 3 && Table.GetTableColor(x-12, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x-6, y, true); Table.SetTableCanRemove(x-12, y, true);
                        }
                    }
                    if (y == 0)
                    {
                        if (x == 6 && Table.GetTableColor(x, y+2) == value && Table.GetTableColor(x, y+4) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y+2, false); Table.SetTableCanRemove(x, y+4, false);
                        }
                        else if (x == 6 && Table.GetTableColor(x, y+2) == 3 && Table.GetTableColor(x, y+4) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y+2, true); Table.SetTableCanRemove(x, y+4, true);
                        }
                        if (Table.GetTableColor(x, y+6) == value && Table.GetTableColor(x, y+12) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y+6, false); Table.SetTableCanRemove(x, y+12, false);
                        }
                        else if (Table.GetTableColor(x, y+6) == 3 && Table.GetTableColor(x, y+12) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y+6, true); Table.SetTableCanRemove(x, y+12, true);
                        }
                    }
                    if (y == 12)
                    {
                        if (x == 6 && Table.GetTableColor(x, y-2) == value && Table.GetTableColor(x, y-4) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y-2, false); Table.SetTableCanRemove(x, y-4, false);
                        }
                        else if (x == 6 && Table.GetTableColor(x, y-2) == 3 && Table.GetTableColor(x, y-4) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y-2, true); Table.SetTableCanRemove(x, y-4, true);
                        }
                        if (Table.GetTableColor(x, y - 6) == value && Table.GetTableColor(x, y - 12) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y-6, false); Table.SetTableCanRemove(x, y-12, false);
                        }
                        else if (Table.GetTableColor(x, y - 6) == 3 && Table.GetTableColor(x, y - 12) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y-6, true); Table.SetTableCanRemove(x, y-12, true);
                        }
                    }

                    //middle 
                    if (x == 2)
                    {
                        if (y == 6 && Table.GetTableColor(x-2, y) == value && Table.GetTableColor(x+2, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x-2, y, false); Table.SetTableCanRemove(x+2, y, false);
                        }
                        else if (y == 6 && Table.GetTableColor(x-2, y) == 3 && Table.GetTableColor(x+2, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x-2, y, true); Table.SetTableCanRemove(x+2, y, true);
                        }
                        if (Table.GetTableColor(x+4, y) == value && Table.GetTableColor(x+8, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x+4, y, false); Table.SetTableCanRemove(x+8, y, false);
                        }
                        else if (Table.GetTableColor(x+4, y) == 3 && Table.GetTableColor(x+8, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x+4, y, true); Table.SetTableCanRemove(x+8, y, true);
                        }
                    }
                    if (x == 10)
                    {
                        if (y == 6 && Table.GetTableColor(x-2, y) == value && Table.GetTableColor(x+2, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x-2, y, false); Table.SetTableCanRemove(x+2, y, false);
                        }
                        else if (y == 6 && Table.GetTableColor(x-2, y) == 3 && Table.GetTableColor(x+2, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x-2, y, true); Table.SetTableCanRemove(x+2, y, true);
                        }
                        if (Table.GetTableColor(x-4, y) == value && Table.GetTableColor(x-8, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x-4, y, false); Table.SetTableCanRemove(x-8, y, false);
                        }
                        else if (Table.GetTableColor(x-4, y) == 3 && Table.GetTableColor(x-8, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x-4, y, true); Table.SetTableCanRemove(x-8, y, true);
                        }
                    }

                    if (y == 2)
                    {
                        if (x == 6 && Table.GetTableColor(x, y-2) == value && Table.GetTableColor(x, y+2) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y-2, false); Table.SetTableCanRemove(x, y+2, false);
                        }
                        else if (x == 6 && Table.GetTableColor(x, y-2) == 3 && Table.GetTableColor(x, y+2) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y-2, true); Table.SetTableCanRemove(x, y+2, true);
                        }
                        if (Table.GetTableColor(x, y+4) == value && Table.GetTableColor(x, y+8) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y+4, false); Table.SetTableCanRemove(x, y+8, false);
                        }
                        else if (Table.GetTableColor(x, y+4) == 3 && Table.GetTableColor(x, y+8) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y+4, true); Table.SetTableCanRemove(x, y+8, true);
                        }
                    }
                    if (y == 10)
                    {
                        if (x == 6 && Table.GetTableColor(x, y-2) == value && Table.GetTableColor(x, y+2) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y-2, false); Table.SetTableCanRemove(x, y-2, false);
                        }
                        else if (x == 6 && Table.GetTableColor(x, y-2) == 3 && Table.GetTableColor(x, y+2) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y-2, true); Table.SetTableCanRemove(x, y-2, true);
                        }
                        if (Table.GetTableColor(x, y - 4) == value && Table.GetTableColor(x, y - 8) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y-4, false); Table.SetTableCanRemove(x, y-8, false);
                        }
                        else if (Table.GetTableColor(x, y - 4) == 3 && Table.GetTableColor(x, y - 8) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y-4, true); Table.SetTableCanRemove(x, y-8, true);
                        }
                    }

                    //inner
                    if (x == 4)
                    {
                        if (y == 6 && Table.GetTableColor(x-2, y) == value && Table.GetTableColor(x-4, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x-2, y, false); Table.SetTableCanRemove(x-4, y, false);
                        }
                        else if (y == 6 && Table.GetTableColor(x-2, y) == 3 && Table.GetTableColor(x-4, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x-2, y, true); Table.SetTableCanRemove(x-4, y, true);
                        }
                        if (Table.GetTableColor(x+2, y) == value && Table.GetTableColor(x+4, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x+2, y, false); Table.SetTableCanRemove(x+4, y, false);
                        }
                        else if (Table.GetTableColor(x+2, y) == 3 && Table.GetTableColor(x+4, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x+2, y, true); Table.SetTableCanRemove(x+4, y, true);
                        }
                    }
                    if (x == 8)
                    {
                        if (y == 6 && Table.GetTableColor(x+2, y) == value && Table.GetTableColor(x+4, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x+2, y, false); Table.SetTableCanRemove(x+4, y, false);
                        }
                        else if (y == 6 && Table.GetTableColor(x+2, y) == 3 && Table.GetTableColor(x+4, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x+2, y, true); Table.SetTableCanRemove(x+4, y, true);
                        }
                        if (Table.GetTableColor(x-2, y) == value && Table.GetTableColor(x-4, y) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x-2, y, false); Table.SetTableCanRemove(x-4, y, false);
                        }
                        else if (Table.GetTableColor(x-2, y) == 3 && Table.GetTableColor(x-4, y) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x-2, y, true); Table.SetTableCanRemove(x-4, y, true);
                        }
                    }

                    if (y == 4)
                    {
                        if (x == 6 && Table.GetTableColor(x, y-2) == value && Table.GetTableColor(x, y-4) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y-2, false); Table.SetTableCanRemove(x, y-4, false);
                        }
                        else if (x == 6 && Table.GetTableColor(x, y-2) == 3 && Table.GetTableColor(x, y-4) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y-2, true); Table.SetTableCanRemove(x, y-4, true);
                        }
                        if (Table.GetTableColor(x, y+2) == value && Table.GetTableColor(x, y+4) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y+2, false); Table.SetTableCanRemove(x, y+4, false);
                        }
                        else if (Table.GetTableColor(x, y+2) == 3 && Table.GetTableColor(x, y+4) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y+2, true); Table.SetTableCanRemove(x, y+4, true);
                        }
                    }
                    if (y == 8)
                    {
                        if (x == 6 && Table.GetTableColor(x, y+2) == value && Table.GetTableColor(x, y+4) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y+2, false); Table.SetTableCanRemove(x, y+4, false);
                        }
                        else if (x == 6 && Table.GetTableColor(x, y+2) == 3 && Table.GetTableColor(x, y+4) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y+2, true); Table.SetTableCanRemove(x, y+4, true);
                        }
                        if (Table.GetTableColor(x, y - 2) == value && Table.GetTableColor(x, y - 4) == value)
                        {
                            Table.SetTableCanRemove(x, y, false); Table.SetTableCanRemove(x, y-2, false); Table.SetTableCanRemove(x, y-4, false);
                        }
                        else if (Table.GetTableColor(x, y - 2) == 3 && Table.GetTableColor(x, y - 4) == 3)
                        {
                            Table.SetTableCanRemove(x, y, true); Table.SetTableCanRemove(x, y-2, true); Table.SetTableCanRemove(x, y-4, true);
                        }
                    }
                }
            }
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new MalomEventArgs());
        }

        public virtual void OnGameOver(Win player)
        {
            GameOver?.Invoke(this, new MalomEventArgs(player));
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

            _table = await _dataAccess.LoadAsync(path);
            _size = _table.GetSize;
            _isGameOver = false;
            //SetUpTable();
        }

        //public void LoadGame(String path)
        //{
        //    if (_dataAccess == null)
        //        throw new InvalidOperationException("No data access is provided.");

        //    _table = _dataAccess.LoadMalomTable(path);
        //    _size = _table.GetSize;
        //    //_gameStepCount = 0;
        //    _isGameOver = false;

        //}


    }
}