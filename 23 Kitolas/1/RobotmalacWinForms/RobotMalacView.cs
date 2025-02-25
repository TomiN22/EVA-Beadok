using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using RobotmalacModel.Model;
using RobotmalacModel.Persistence;
//using RobotmalacModel.Persistence;

namespace RobotmalacWinForms
{
    public partial class BekeritesView : Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private RobotModel _model;
        private IRobotFileDataAccess _dataAccess;

        private string startupPath;

        private Image _peppaFekete;
        private Image _peppaPiros;
        private Image _peppaFeketeLe;
        private Image _peppaFeketeFel;
        private Image _peppaFeketeJobb;
        private Image _peppaPirosLe;
        private Image _peppaPirosFel;
        private Image _peppaPirosJobb;


        //TODO: relatív útvonal -kész
        //Image _peppaFekete = Image.FromFile("E:\\ELTE\\2023-24 õszi\\EVA\\Gyak\\Bead\\1\\pic\\Peppa_malac_szemcsi_fekete.png");
        //Image _peppaPiros = Image.FromFile("E:\\ELTE\\2023-24 õszi\\EVA\\Gyak\\Bead\\1\\pic\\Peppa_malac_szemcsi_piros.png");
        //Image _peppaFeketeLe = Image.FromFile("E:\\ELTE\\2023-24 õszi\\EVA\\Gyak\\Bead\\1\\pic\\Peppa_fekete_le.png");
        //Image _peppaFeketeFel = Image.FromFile("E:\\ELTE\\2023-24 õszi\\EVA\\Gyak\\Bead\\1\\pic\\Peppa_fekete_fel.png");
        //Image _peppaFeketeJobb = Image.FromFile("E:\\ELTE\\2023-24 õszi\\EVA\\Gyak\\Bead\\1\\pic\\Peppa_fekete_jobb.png");
        //Image _peppaPirosLe = Image.FromFile("E:\\ELTE\\2023-24 õszi\\EVA\\Gyak\\Bead\\1\\pic\\Peppa_piros_le.png");
        //Image _peppaPirosFel = Image.FromFile("E:\\ELTE\\2023-24 õszi\\EVA\\Gyak\\Bead\\1\\pic\\Peppa_piros_fel.png");
        //Image _peppaPirosJobb = Image.FromFile("E:\\ELTE\\2023-24 õszi\\EVA\\Gyak\\Bead\\1\\pic\\Peppa_piros_jobb.png");

        public BekeritesView()
        {
            InitializeComponent();

            startupPath = Application.StartupPath;
            startupPath = Application.StartupPath;
            

            SaveMenuItem.Enabled = false;
            _dataAccess = new IRobotFileDataAccess();
            _model = new RobotModel(_dataAccess, _size);
            ThreeThree.Click += MenuTableFour_Click;
            FiveFive.Click += MenuTableSix_Click;
            SevenSeven.Click += MenuTableEight_Click;
            //buttonOK.Click += ButtonOK_Click;
            //infoStripMenuItem.Click += InfoMenu;

            SaveMenuItem.Click += MenuFileSaveGame_Click;
            LoadMenuItem.Click += MenuFileLoadGame_Click;
        }

        private void MenuTableFour_Click(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 3;
            _model = new RobotModel(_dataAccess, _size);
            //_model.Table.Moves = 12;
            remainingCountLabel.Text = _model.Table.Moves.ToString();
            _model.GenerateFields();
            GenerateTable();
            _model.GameOver += GameOver;

        }
        private void MenuTableSix_Click(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 4;
            _model = new RobotModel(_dataAccess, _size);
            //_model.Table.Moves = 20;
            remainingCountLabel.Text = _model.Table.Moves.ToString();
            _model.GenerateFields();
            GenerateTable();
            _model.GameOver += GameOver;
        }
        private void MenuTableEight_Click(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 6;
            _model = new RobotModel(_dataAccess, _size);
            //_model.Table.Moves = 28;
            remainingCountLabel.Text = _model.Table.Moves.ToString();
            _model.GenerateFields();
            GenerateTable();
            _model.GameOver += GameOver;
        }

        public void GenerateTable()
        {
            RemoveGrid();
            _buttonGrid = new Button[_size, _size];
            playerLabel.Visible = true;
            playerLabel.Text = (_model.Table.GetPlayer == 1 ? "Red" : "Blue") + " on turn";
            remainingLabel.Visible=true;
            remainingCountLabel.Visible=true;
            SaveMenuItem.Enabled = true;

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Size = new Size(100, 100); // méret
                    _buttonGrid[i, j].Location = new Point(0+100*i, 70+100*j); // elhelyezkedéss
                    _buttonGrid[i, j].Enabled = true; // kikapcsolt állapot
                    _buttonGrid[i, j].TabStop = false;
                    _buttonGrid[i, j].TabIndex = 100 * i + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].BackColor = Color.Transparent;
                    _buttonGrid[i, j].TabStop = false;
                    _buttonGrid[i, j].Click += CellClick;
                    Controls.Add(_buttonGrid[i, j]);
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        //_buttonGrid[i, j].Text="B";
                        _buttonGrid[i, j].BackColor = Color.Blue;
                    }
                    else if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        //_buttonGrid[i, j].Text="W";
                        _buttonGrid[i, j].BackColor = Color.Red;
                    }
                    //_buttonGrid[i, j].Text = _buttonGrid[i, j].TabIndex.ToString();
                }
            }
            RefreshTable();
        }


        public void RefreshTable()
        {
            remainingCountLabel.Text = _model.Table.Moves.ToString();
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _buttonGrid[i, j].BackColor = Color.Transparent;
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[i, j].BackColor = Color.Blue;
                    }
                    else if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[i, j].BackColor = Color.Red;
                    }
                }
            }
            _model.CheckGO();
            
        }

        public void SetUpTable()
        {
            _buttonGrid = new Button[_size, _size];
            remainingLabel.Visible=true;
            remainingCountLabel.Visible=true;
            SaveMenuItem.Enabled = true;
            remainingCountLabel.Text = _model.Table.Moves.ToString();

            for (int j = 0; j < _size; j++)
            {
                for (int i = 0; i < _size; i++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Size = new Size(100, 100); // méret
                    _buttonGrid[i, j].Location = new Point(0+100*i, 70+100*j); // elhelyezkedéss
                    _buttonGrid[i, j].Enabled = true; // kikapcsolt állapot
                    _buttonGrid[i, j].TabStop = false;
                    _buttonGrid[i, j].TabIndex = 100 * i + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].BackColor = Color.AntiqueWhite;
                    _buttonGrid[i, j].TabStop = false;
                    _buttonGrid[i, j].Click += CellClick;
                    Controls.Add(_buttonGrid[i, j]);

                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[i, j].BackColor = Color.Blue;
                    }
                    else if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[i, j].BackColor = Color.Red;
                    }

                    
                }
            }
            
            //RefreshTable();
            playerLabel.Visible = true;
            SaveMenuItem.Enabled = true;
            PlayerTurn();
        }

        public void RemoveGrid()
        {
            commandLabel.Visible = false;
            commandTextBox.Visible = false;
            buttonOK.Visible = false;
            playerLabel.Visible = false;
            SaveMenuItem.Enabled = false;
            p1HealthLabel.Visible = false;
            p2HealthLabel.Visible = false;
            p1HealthBar.Visible = false;
            p2HealthBar.Visible = false;
            p1Hlabel.Visible= false;
            p2Hlabel.Visible= false;
            buttonPTurn.Visible = false;
            remainingCountLabel.Visible = false;
            remainingLabel.Visible = false;
            
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
            if (_model.IsGameOver)
            {
                if(_size == 3)
                {
                    if (_model != null)
                    {
                        _model.GameOver -= GameOver;
                    }
                    _size = 3;
                    _model = new RobotModel(_dataAccess, _size);
                    //_model.Table.Moves = 12;
                    remainingCountLabel.Text = _model.Table.Moves.ToString();
                    GenerateTable();
                    _model.GameOver += GameOver;
                }
                else if(_size == 5)
                {
                    if (_model != null)
                    {
                        _model.GameOver -= GameOver;
                    }
                    _size = 5;
                    _model = new RobotModel(_dataAccess, _size);
                    //_model.Table.Moves = 20;
                    remainingCountLabel.Text = _model.Table.Moves.ToString();
                    GenerateTable();
                    _model.GameOver += GameOver;
                }
                else if(_size == 7)
                {
                    if (_model != null)
                    {
                        _model.GameOver -= GameOver;
                    }
                    _size = 7;
                    _model = new RobotModel(_dataAccess, _size);
                    //_model.Table.Moves = 28;
                    remainingCountLabel.Text = _model.Table.Moves.ToString();
                    GenerateTable();
                    _model.GameOver += GameOver;
                }
            }
        }

        public void PlayerTurn()
        {
            playerLabel.Text = (_model.Table.GetPlayer == 1 ? "Red" : "Blue") + " on turn";
        }

        

        private void GameOver(Object? sender, RobotEventArgs e)
        {
            if(e.Winner == Win.Draw)
                MessageBox.Show("It's a draw'! ", "Game over!");
            else
                MessageBox.Show((e.Winner) + " won! ", "Game over!");

            //Controls.Remove(p1);
            SaveMenuItem.Enabled = false;
            RemoveGrid();
            switch (_model.Table.GetSize)
            {
                case 3:
                    if (_model != null)
                    {
                        _model.GameOver -= GameOver;
                    }
                    _size = 3;
                    _model = new RobotModel(_dataAccess, _size);
                    remainingCountLabel.Text = _model.Table.Moves.ToString();
                    _model.GenerateFields();
                    GenerateTable();
                    _model.GameOver += GameOver;
                    break;
                case 4:
                    if (_model != null)
                    {
                        _model.GameOver -= GameOver;
                    }
                    _size = 4;
                    _model = new RobotModel(_dataAccess, _size);
                    remainingCountLabel.Text = _model.Table.Moves.ToString();
                    _model.GenerateFields();
                    GenerateTable();
                    _model.GameOver += GameOver;
                    break;
                case 6:
                    if (_model != null)
                    {
                        _model.GameOver -= GameOver;
                    }
                    _size = 6;
                    _model = new RobotModel(_dataAccess, _size);
                    remainingCountLabel.Text = _model.Table.Moves.ToString();
                    _model.GenerateFields();
                    GenerateTable();
                    _model.GameOver += GameOver;
                    break;
            }
        }

        private void CellClick(Object sender, EventArgs e)
        {
            Button button = sender as Button;
            int x;
            int y;
            if (button != null)
            {
                x = button.TabIndex / 100;
                y = button.TabIndex % 100;

                _model.Step1(x, y);
                //_model.Step2(x, y);
                
                PlayerTurn();
            }
            RefreshTable();
        }

        private void InfoMenu(Object? sender, EventArgs e)
        {
            //infoTextBox.Font = new Font(FontFamily.GenericSansSerif, 15);
            MessageBox.Show("Move commands: 'move left', 'move right', 'move up' and 'move down'.\r\nTurn with: 'turn left' or 'turn right'.\r\nAttack using the 'shoot' or 'punch'. The 'shoot' has a greater precedency.\r\n", "|How to play|");
        }

        private async void MenuFileSaveGame_Click(Object? sender, EventArgs e)
        {

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await _model.SaveGameAsync(saveFileDialog.FileName);
                }
                catch (RobotDataException)
                {
                    MessageBox.Show("Saved successfully!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void MenuFileLoadGame_Click(Object? sender, EventArgs e)
        {

            if (openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {

                    _model.GameOver -= GameOver;
                    await _model.LoadGameAsync(openFileDialog.FileName);
                    _model.GameOver += GameOver;
                    _size = _model.Table.GetSize;
                    SaveMenuItem.Enabled = true;
                    RemoveGrid();
                    SetUpTable();

                }
                catch (RobotDataException)
                {
                    MessageBox.Show("Game loading error!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //_model.NewGame();
                    //_menuFileSaveGame.Enabled = true;
                }

            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            commandTextBox.Visible= true;
        }

        //private void label1_Click(object sender, EventArgs e)
        //{

        //}

        //private void label1_Click(object sender, EventArgs e)
        //{

        //}
    }
}