using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using RobotmalacModel.Model;
using RobotmalacModel.Persistence;
//using RobotmalacModel.Persistence;

namespace RobotmalacWinForms
{
    public partial class RobotMalacView : Form
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

        public RobotMalacView()
        {
            InitializeComponent();

            startupPath = Application.StartupPath;
            startupPath = Application.StartupPath;
            _peppaFekete = Image.FromFile(Path.Combine(startupPath, "./pic/Peppa_malac_szemcsi_fekete.png"));
            _peppaPiros = Image.FromFile(Path.Combine(startupPath, "./pic/Peppa_malac_szemcsi_piros.png"));
            _peppaFeketeLe = Image.FromFile(Path.Combine(startupPath, "./pic/Peppa_fekete_le.png"));
            _peppaFeketeFel = Image.FromFile(Path.Combine(startupPath, "./pic/Peppa_fekete_fel.png"));
            _peppaFeketeJobb = Image.FromFile(Path.Combine(startupPath, "./pic/Peppa_fekete_jobb.png"));
            _peppaPirosLe = Image.FromFile(Path.Combine(startupPath, "./pic/Peppa_piros_le.png"));
            _peppaPirosFel = Image.FromFile(Path.Combine(startupPath, "./pic/Peppa_piros_fel.png"));
            _peppaPirosJobb = Image.FromFile(Path.Combine(startupPath, "./pic/Peppa_piros_jobb.png"));

            SaveMenuItem.Enabled = false;
            _dataAccess = new IRobotFileDataAccess();
            _model = new RobotModel(_dataAccess, _size);
            FourFour.Click += MenuTableFour_Click;
            SixSix.Click += MenuTableSix_Click;
            EightEight.Click += MenuTableEight_Click;
            buttonOK.Click += ButtonOK_Click;
            infoStripMenuItem.Click += InfoMenu;

            SaveMenuItem.Click += MenuFileSaveGame_Click;
            LoadMenuItem.Click += MenuFileLoadGame_Click;
        }

        private void MenuTableFour_Click(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 4;
            _model = new RobotModel(_dataAccess, _size);
            GenerateTable();
            _model.GameOver += GameOver;
        }
        private void MenuTableSix_Click(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 6;
            _model = new RobotModel(_dataAccess, _size);
            GenerateTable();
            _model.GameOver += GameOver;
        }
        private void MenuTableEight_Click(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 8;
            _model = new RobotModel(_dataAccess, _size);
            GenerateTable();
            _model.GameOver += GameOver;
        }

        public void GenerateTable()
        {
            RemoveGrid();
            _model.Player1Hit += HandlePlayer1Hit;
            _model.Player2Hit += HandlePlayer2Hit;
            _buttonGrid = new Button[_size, _size];
            commandLabel.Visible = true;
            commandTextBox.Visible = true;
            buttonOK.Visible = true;
            buttonPTurn.Visible = true;
            buttonPTurn.Image = _peppaFekete;
            playerLabel.ForeColor = Color.Black;
            playerLabel.Visible = true;
            SaveMenuItem.Enabled = true;
            p1HealthBar.Value = 100;
            p2HealthBar.Value = 100;
            p1HealthLabel.Visible = true;
            p2HealthLabel.Visible = true;
            p1HealthBar.Visible = true;
            p2HealthBar.Visible = true;
            p1Hlabel.Text = "3";
            p2Hlabel.Text = "3";
            p1Hlabel.Visible = true;
            p2Hlabel.Visible = true;
            

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
                    _buttonGrid[i, j].BackColor = Color.AntiqueWhite;
                    _buttonGrid[i, j].TabStop = false;
                    Controls.Add(_buttonGrid[i, j]);
                    
                    if(i == (_size/2)-1 && j == 0)
                    {
                        _buttonGrid[i, j].BackgroundImageLayout = ImageLayout.Center;
                        _buttonGrid[i, j].Image = _peppaFeketeLe;
                    }
                    if (i == _size/2 && j == _size-1){
                        _buttonGrid[i, j].BackgroundImageLayout = ImageLayout.Center;
                        _buttonGrid[i, j].Image = _peppaPirosFel;
                    }
                }
            }

        }

        public void SetUpTable()
        {
            _buttonGrid = new Button[_size, _size];

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
                    Controls.Add(_buttonGrid[i, j]);

                    if (_model.Table.P1Current[0] == i && _model.Table.P1Current[1] == j)
                    {
                        switch(_model.Table.P1Direction)
                        {
                            case "up": _buttonGrid[i, j].Image = _peppaFeketeFel; break;
                            case "down": _buttonGrid[i, j].Image = _peppaFeketeLe; break;
                            case "left": _buttonGrid[i, j].Image = _peppaFekete; break;
                            case "right": _buttonGrid[i, j].Image = _peppaFeketeJobb; break;
                        }
                    }
                    else if (_model.Table.P2Current[0] == i && _model.Table.P2Current[1] == j)
                    {
                        switch (_model.Table.P2Direction)
                        {
                            case "up": _buttonGrid[i, j].Image = _peppaPirosFel; break;
                            case "down": _buttonGrid[i, j].Image = _peppaPirosLe; break;
                            case "left": _buttonGrid[i, j].Image = _peppaPiros; break;
                            case "right": _buttonGrid[i, j].Image = _peppaPirosJobb; break;
                        }
                    }
                }
            }
            _model.Player1Hit += HandlePlayer1Hit;
            _model.Player2Hit += HandlePlayer2Hit;
            commandLabel.Visible = true;
            commandTextBox.Visible = true;
            buttonOK.Visible = true;
            buttonPTurn.Visible = true;
            buttonPTurn.Image = _peppaFekete;
            playerLabel.ForeColor = Color.Black;
            playerLabel.Visible = true;
            SaveMenuItem.Enabled = true;
            if(_model.Table.P1Health == 3) { p1HealthBar.Value = 100; }
            if (_model.Table.P1Health == 2) { p1HealthBar.Value = 66; }
            if (_model.Table.P1Health == 1) { p1HealthBar.Value = 33; }
            if (_model.Table.P1Health == 0) { p1HealthBar.Value = 0; }
            if (_model.Table.P2Health == 3) { p2HealthBar.Value = 100; }
            if (_model.Table.P2Health == 2) { p2HealthBar.Value = 66; }
            if (_model.Table.P2Health == 1) { p2HealthBar.Value = 33; }
            if (_model.Table.P2Health == 0) { p2HealthBar.Value = 0; }
            p1HealthLabel.Visible = true;
            p2HealthLabel.Visible = true;
            p1HealthBar.Visible = true;
            p2HealthBar.Visible = true;
            p1Hlabel.Text = _model.Table.P1Health.ToString();
            p2Hlabel.Text = _model.Table.P2Health.ToString();
            p1Hlabel.Visible = true;
            p2Hlabel.Visible = true;
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

        public void PlayerTurn()
        {
            playerLabel.Text = (_model.Table.GetPlayer == 1 ? "Player 1" : "Player 2") + " on turn";
            if(_model.Table.GetPlayer == 1)
            {
                playerLabel.ForeColor = Color.Black;
                buttonPTurn.Image = _peppaFekete;
            }
            else if(_model.Table.GetPlayer == 2)
            {
                playerLabel.ForeColor = Color.Red;
                buttonPTurn.Image = _peppaPiros;
            }
        }

        public void HandlePlayer1Hit(object? sender, EventArgs e)
        {
            p1Hlabel.Text = _model.Table.P1Health.ToString();

            if(_model.Table.P1Health == 2) { p1HealthBar.Value = 66; }
            if(_model.Table.P1Health == 1) { p1HealthBar.Value = 33; }
            if (_model.Table.P1Health == 0) { p1HealthBar.Value = 0; }
        }

        public void HandlePlayer2Hit(object? sender, EventArgs e)
        {
            p2Hlabel.Text = _model.Table.P2Health.ToString();

            if (_model.Table.P2Health == 2) { p2HealthBar.Value = 66; }
            if (_model.Table.P2Health == 1) { p2HealthBar.Value = 33; }
            if (_model.Table.P2Health == 0) { p2HealthBar.Value = 0; }
        }

        private void GameOver(Object? sender, RobotEventArgs e)
        {
            MessageBox.Show((e.Winner) + " won! " , "Game over!");
            
            //Controls.Remove(p1);
            SaveMenuItem.Enabled = false;
            RemoveGrid();
        }

        private void ButtonOK_Click(Object? sender, EventArgs e)
        {
            

            if (_model.Table.GetPlayer == 1) 
            {
                _model.CollectCommand(commandTextBox.Text, _model.Table.GetPlayer);
                
                _model.Step();
                PlayerTurn();
                SaveMenuItem.Enabled = false;
                commandTextBox.Text = null;
            }
            else if (_model.Table.GetPlayer == 2)
            {
                SaveMenuItem.Enabled = true;
                _model.CollectCommand(commandTextBox.Text, _model.Table.GetPlayer);
                //PlayerTurn();
                _model.DoCommand();
                commandTextBox.Text = null;
                if (_model.Table.P1Direction == "up")
                {
                    for(int i=0; i<_size; ++i)
                    {
                        for(int j=0; j< _size; ++j)
                        {
                            if(_model.Table.P1Current[0] == i && _model.Table.P1Current[1] == j)
                            {
                                _buttonGrid[i, j].Image = _peppaFeketeFel;
                            }
                        }
                    }
                }
                else if(_model.Table.P1Direction == "down")
                {
                    for (int i = 0; i<_size; ++i)
                    {
                        for (int j = 0; j< _size; ++j)
                        {
                            if (_model.Table.P1Current[0] == i && _model.Table.P1Current[1] == j)
                            {
                                _buttonGrid[i, j].Image = _peppaFeketeLe;
                            }
                        }
                    }
                }
                else if(_model.Table.P1Direction == "right")
                {
                    for (int i = 0; i<_size; ++i)
                    {
                        for (int j = 0; j< _size; ++j)
                        {
                            if (_model.Table.P1Current[0] == i && _model.Table.P1Current[1] == j)
                            {
                                _buttonGrid[i, j].Image = _peppaFeketeJobb;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i<_size; ++i)
                    {
                        for (int j = 0; j< _size; ++j)
                        {
                            if (_model.Table.P1Current[0] == i && _model.Table.P1Current[1] == j)
                            {
                                _buttonGrid[i, j].Image = _peppaFekete;
                            }
                        }
                    }
                }

                //P2Direction
                if (_model.Table.P2Direction == "up")
                {
                    for (int i = 0; i<_size; ++i)
                    {
                        for (int j = 0; j< _size; ++j)
                        {
                            if (_model.Table.P2Current[0] == i && _model.Table.P2Current[1] == j)
                            {
                                _buttonGrid[i, j].Image = _peppaPirosFel;
                            }
                        }
                    }
                }
                else if (_model.Table.P2Direction == "down")
                {
                    for (int i = 0; i<_size; ++i)
                    {
                        for (int j = 0; j< _size; ++j)
                        {
                            if (_model.Table.P2Current[0] == i && _model.Table.P2Current[1] == j)
                            {
                                _buttonGrid[i, j].Image = _peppaPirosLe;
                            }
                        }
                    }
                }
                else if (_model.Table.P2Direction == "right")
                {
                    for (int i = 0; i<_size; ++i)
                    {
                        for (int j = 0; j< _size; ++j)
                        {
                            if (_model.Table.P2Current[0] == i && _model.Table.P2Current[1] == j)
                            {
                                _buttonGrid[i, j].Image = _peppaPirosJobb;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i<_size; ++i)
                    {
                        for (int j = 0; j< _size; ++j)
                        {
                            if (_model.Table.P2Current[0] == i && _model.Table.P2Current[1] == j)
                            {
                                _buttonGrid[i, j].Image = _peppaPiros;
                            }
                        }
                    }
                }

                if(_model.P1MoveChanged == true || _model.P2MoveChanged)
                {
                    for(int i=0; i<_size; ++i)
                    {
                        for(int j=0; j<_size; ++j)
                        {
                            _buttonGrid[i, j].Image = null;
                        }
                    }

                    switch (_model.Table.P1Direction)
                    {
                        case "up": _buttonGrid[_model.Table.P1Current[0], _model.Table.P1Current[1]].Image = _peppaFeketeFel; break;
                        case "down": _buttonGrid[_model.Table.P1Current[0], _model.Table.P1Current[1]].Image = _peppaFeketeLe; break;
                        case "right": _buttonGrid[_model.Table.P1Current[0], _model.Table.P1Current[1]].Image = _peppaFeketeJobb; break;
                        case "left": _buttonGrid[_model.Table.P1Current[0], _model.Table.P1Current[1]].Image = _peppaFekete; break;
                    }

                    switch (_model.Table.P2Direction)
                    {
                        case "up": _buttonGrid[_model.Table.P2Current[0], _model.Table.P2Current[1]].Image = _peppaPirosFel; break;
                        case "down": _buttonGrid[_model.Table.P2Current[0], _model.Table.P2Current[1]].Image = _peppaPirosLe; break;
                        case "right": _buttonGrid[_model.Table.P2Current[0], _model.Table.P2Current[1]].Image = _peppaPirosJobb; break;
                        case "left": _buttonGrid[_model.Table.P2Current[0], _model.Table.P2Current[1]].Image = _peppaPiros; break;
                    }
                            
                }
                //Thread.Sleep(1000);
                _model.Step();
                PlayerTurn();
                
            }
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