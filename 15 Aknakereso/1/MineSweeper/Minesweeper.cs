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
        Label p2 = new Label();
        int c = 1;

        public Minesweeper()
        {
            InitializeComponent();

            _dataAccess = new IMineFileDataAccess();
            _model = new MineModel(_dataAccess,_size);
            SixSix.Click += MenuTableSix_Click; //feliratkoz�s egy esem�nyre
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
            _size = 6; //10
            _model = new MineModel(_dataAccess, _size);
            GenerateTable();
            _model.GenerateFields();
        }

        private void MenuTableTen_Click(Object sender, EventArgs e)
        {
            _size = 10; //15
            _model = new MineModel(_dataAccess, _size);
            GenerateTable();
            _model.GenerateFields();
        }

        private void MenuTableSixTeen_Click(Object sender, EventArgs e)
        {
            _size = 16; //40
            _model = new MineModel(_dataAccess, _size);
            GenerateTable();
            _model.GenerateFields();
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
            Controls.Remove(p2);
            Controls.Add(p1);
            p1.AutoSize = true;
            p2.AutoSize = true;
            p1.Location = new Point(100,40);
            p2.Location = new Point(100, 40);
            p1.Text = "Player1 on turn";

            RemoveGrid();

            _buttonGrid = new Button[_size, _size];

            
            for (int i=0; i < _size; i++)
            {
                for (int j=0; j < _size; j++)
                {
                    
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(0+50*i, 70+50*j); // elhelyezked�ss
                    _buttonGrid[i, j].Size = new Size(50, 50); // m�ret
                    //_buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // bet�t�pus
                    _buttonGrid[i, j].Enabled = true; // kikapcsolt �llapot
                    _buttonGrid[i, j].TabIndex = 100 * i + j; // a gomb sz�m�t a TabIndex-ben t�roljuk
                    //_buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lap�tott st�pus
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
            
            //_model
            if (button != null)
            {
                // a TabIndex-b�l megkapjuk a sort �s oszlopot
                x = button.TabIndex / 100;
                y = button.TabIndex % 100;

                //step
                
                // mez� friss�t�se
                if (_model.Table.IsEmpty(x, y))
                {
                    //_buttonGrid[x, y].Text = String.Empty;
                    _buttonGrid[x, y].Text = _model.Table[x, y].ToString(" ");
                }
                else
                {
                    if (_model.Table[x, y].ToString() == "-1")
                    {
                        _buttonGrid[x, y].Text = "X";

                        if (c % 2 == 1)
                        {
                            MessageBox.Show("Player1 won!","Game over!");
                        }
                        else if (c % 2 == 0)
                        {
                            MessageBox.Show("Player1 won!","Game over!");
                        }
                        
                        PlayerClear();
                        RemoveGrid();
                        return;
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
                                //_buttonGrid[x, y].Text = String.Empty;
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

                if (_model.IsDraw())
                {
                    MessageBox.Show("Draw!","Game over!");
                    PlayerClear();
                    RemoveGrid();
                    return;
                }

            }
            PlayerTurn(sender, e);
        }

        public void PlayerTurn(Object sender, EventArgs e)
        {
            Controls.Remove(p1);
            if (c % 2 == 1) 
            { 
                Controls.Remove(p1);
                Controls.Add(p2);
                p2.Text = "Player2 on turn";
            }
            else if(c % 2 == 0) 
            {
                Controls.Remove(p2);
                Controls.Add(p1);
                p1.Text = "Player1 on turn";
            }            
            c++;
        }

        public void PlayerClear()
        {
            Controls.Remove(p1);
            Controls.Remove(p2);
        }

        private async void MenuFileSaveGame_Click(Object sender, EventArgs e)
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // j�t� ment�se
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

            if (_openFileDialog1.ShowDialog() == DialogResult.OK) // ha kiv�lasztottunk egy f�jlt
            {
                try
                {
                    // j�t�k bet�lt�se
                    await _model.LoadGameAsync(_openFileDialog1.FileName);
                    _size = _model.Table.GetSize;
                    //_menuFileSaveGame.Enabled = true;
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
                    if (_model.Table.GetTable[i,j].isOpened == true)
                    {
                        if(_model.Table[i, j] != 0)
                        {
                            _buttonGrid[i, j].Text = _model.Table[i,j].ToString();
                        }
                        else
                        {
                            _buttonGrid[i, j].Text = "X";
                        }
                    }
                }
            }

            p1.Text = "Player1";
            p2.Text = "Player2";
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