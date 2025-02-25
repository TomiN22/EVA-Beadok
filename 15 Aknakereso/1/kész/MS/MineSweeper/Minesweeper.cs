using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Model;
using Persistance;

namespace MineSweeper
{
    public partial class Minesweeper : Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private MineModel _model;
        private IMineFileDataAccess _dataAccess;

        Label p1 = new Label();


        public Minesweeper()
        {
            InitializeComponent();
            SaveGame.Enabled = false;
            _dataAccess = new IMineFileDataAccess();
            _model = new MineModel(_dataAccess, _size);
            SixSix.Click += MenuTableSix_Click; //feliratkozás egy eseményre
            TenTen.Click += MenuTableTen_Click;
            SixTeenSixTeen.Click += MenuTableSixTeen_Click;
            SaveGame.Click += MenuFileSaveGame_Click;
            LoadGame.Click += MenuFileLoadGame_Click;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void MenuTableSix_Click(Object sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 6; //10
            _model = new MineModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            GenerateTable();
            _model.GenerateFields();
        }

        private void MenuTableTen_Click(Object sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;

            }
            _size = 10; //15
            _model = new MineModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            GenerateTable();
            _model.GenerateFields();
        }

        private void MenuTableSixTeen_Click(Object sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;

            }
            _size = 16; //40
            _model = new MineModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            GenerateTable();
            _model.GenerateFields();
        }

        private void GameOver(Object? sender, MineEventArgs e)
        {

            if (e.Winner != Win.Draw)
            {
                MessageBox.Show((_model.Table.GetPlayer == 1 ? "Player 1" : "Player 2") + " won","Game over!");
            }
            else
            {
                MessageBox.Show("It's a draw!","Game over!");

            }
            Controls.Remove(p1);
            SaveGame.Enabled = false;
            RemoveGrid();
        }

        public void RemoveGrid()
        {
            if (_buttonGrid != null)
            {
                for (int i = 0; i < _buttonGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < _buttonGrid.GetLength(1); j++)
                    {
                        Controls.Remove(_buttonGrid[i, j]);
                    }
                }
            }
        }

        public void GenerateTable()
        {
            

            Controls.Add(p1);
            p1.AutoSize = true;
            p1.Location = new Point(100,40);
            p1.Text = "Player1 on turn";

            RemoveGrid();

            _buttonGrid = new Button[_size, _size];

            
            for (int i=0; i < _size; i++)
            {
                for (int j=0; j < _size; j++)
                {
                    
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(0+50*i, 70+50*j); // elhelyezkedéss
                    _buttonGrid[i, j].Size = new Size(50, 50); // méret
                    //_buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttonGrid[i, j].Enabled = true; // kikapcsolt állapot
                    _buttonGrid[i, j].TabIndex = 100 * i + j; // a gomb számát a TabIndex-ben tároljuk
                    //_buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    //_buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    _buttonGrid[i, j].TabStop = false;
                    Controls.Add(_buttonGrid[i, j]);

                    _buttonGrid[i, j].Click += OpenCell;
                    //_buttonGrid[i, j].Click += PlayerTurn;
                }
            }
            
        }

        private void OpenCell(Object sender, EventArgs e)
        {
            Button button = sender as Button;
            int x;
            int y;
            if (button != null)
            {
                x = button.TabIndex / 100;
                y = button.TabIndex % 100;

                
                if (_model.Table.IsEmpty(x, y))
                {
                    _buttonGrid[x, y].Text = _model.Table[x, y].ToString(" ");
                }
                else
                {
                    if (_model.Table[x, y].ToString() == "-1")
                    {
                        _buttonGrid[x, y].Text = "X";
                    }
                    else
                    {
                        _buttonGrid[x, y].Text = _model.Table[x, y].ToString();
                    }
                }
                _buttonGrid[x, y].Enabled = false;

                _model.ShowAll(x, y);

                for(int i=0; i<_size; i++)
                {
                    for(int j=0; j<_size; j++)
                    {
                        if(_model.Table.IsOpened(i,j))
                        {
                            if (_model.Table.IsEmpty(i, j))
                            {
                                _buttonGrid[i, j].Text = _model.Table[i, j].ToString(" ");
                            }
                            else
                            {
                                if (_model.Table[i, j].ToString() == "-1")
                                {
                                    _buttonGrid[i, j].Text = "X";
                                }
                                else
                                {
                                    _buttonGrid[i, j].Text = _model.Table[i, j].ToString();
                                }
                            }
                            _buttonGrid[i, j].Enabled = false;
                        }
                    }
                }
                _model.Step(x, y);
                
                PlayerTurn();
            }
        }

        public void PlayerTurn()
        {
            p1.Text = (_model.Table.GetPlayer == 1 ? "Player 1" : "Player 2") + " on turn";
        }

        private async void MenuFileSaveGame_Click(Object sender, EventArgs e)
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await _model.SaveGameAsync(saveFileDialog1.FileName);
                }
                catch (MineDataException)
                {
                    MessageBox.Show("Saved successfully!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        {

            if (_openFileDialog1.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    if (_model != null)
                    {
                        _model.GameOver -= GameOver;

                    }
                    await _model.LoadGameAsync(_openFileDialog1.FileName);
                    _model.GameOver += GameOver;
                    _size = _model.Table.GetSize;
                    SaveGame.Enabled = true;
                    RemoveGrid();
                    SetUpTable();


                }
                catch (MineDataException)
                {
                    MessageBox.Show("Game loading error!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //_model.NewGame();
                    //_menuFileSaveGame.Enabled = true;
                }

            }
        }

        public void SetUpTable()
        {
            _buttonGrid = new Button[_size, _size];
            GenerateTable();
            for(int i=0; i<_size; i++)
            {
                for(int j=0; j<_size; j++)
                {
                    if (_model.Table.GetTable[i,j].isOpened)
                    {
                            if (_model.Table.IsEmpty(i, j))
                            {
                                _buttonGrid[i, j].Text = _model.Table[i, j].ToString(" ");
                            }
                            else
                            {
                                if (_model.Table[i, j].ToString() == "-1")
                                {
                                    _buttonGrid[i, j].Text = "X";
                                }
                                else
                                {
                                    _buttonGrid[i, j].Text = _model.Table[i, j].ToString();
                                }
                            }
                            _buttonGrid[i, j].Enabled = false;
                    }
                }
            }

            PlayerTurn();
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        /*private void GameWon(object sender, EventArgs e)
        {
            switch (e.PlayerWon)
            {
                case FieldType.PlayerA:
                    MessageBox.Show("The first player won the game.", "Game over", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    break;
                case FieldType.PlayerB:
                    MessageBox.Show("The second player won the game.", "Game over", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    break;
            }
        }*/
    }
}