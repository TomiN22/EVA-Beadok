using LooseRobotModel.Model;
using LooseRobotModel.Persistence;
using System.Windows.Forms;

namespace LooseRobotWinForms
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private GameModel _model;
        private LooseRobotFileDataAccess _dataAccess;
        private bool _enableButtons = false;
        public bool _startPause;

        public bool EnableButtons
        {
            get { return _enableButtons; }
            set
            {
                _enableButtons = value;
            }
        }

        public bool StartPause
        {
            get { return _startPause; }
            set
            {
                _startPause = value;
            }
        }

        public string PlayerTime { get { return TimeSpan.FromSeconds(_model.Table.GameTime).ToString("g"); } }

        public Form()
        {
            InitializeComponent();

            saveToolStripMenuItem.Enabled = false;
            _dataAccess = new LooseRobotFileDataAccess();
            _model = new GameModel(_dataAccess, _size);
            Menu7x7.Click += MenuTable7_Click; //feliratkozás egy eseményre
            Menu11x11.Click += MenuTable11_Click;
            Menu15x15.Click += MenuTable15_Click;
            saveToolStripMenuItem.Click += MenuFileSaveGame_Click;
            loadToolStripMenuItem.Click += MenuFileLoadGame_Click;
            pauseToolStripMenuItem.Click += PauseStartGame;
            //timer1.Tick += timer1_Tick;
            EnableButtons = true;
        }

        private void MenuTable7_Click(Object sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 7;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
        }

        private void MenuTable11_Click(Object sender, EventArgs e)
        {
            //if (_model != null)
            //{
            //    _model.GameOver -= GameOver;

            //}
            _size = 11;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
        }

        private void MenuTable15_Click(Object sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;

            }
            _size = 15;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
        }

        private void GameOver(Object? sender, LooseRobotEventArgs e)
        {
            timer1.Stop();
            MessageBox.Show(PlayerTime.ToString(), "Finally, you won!");
            
            saveToolStripMenuItem.Enabled = false;
            RemoveGrid();
            
        }

        public void RemoveGrid()
        {
            tableLayoutPanel1.Controls.Clear();
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
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnCount = _model.GetSize;
            tableLayoutPanel1.RowCount = _model.GetSize;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.AutoSize = true;

            saveToolStripMenuItem.Enabled = true;
            TimeLabel.Text = "0:00:00";
            RemoveGrid();

            _buttonGrid = new Button[_size, _size];


            for (int i = 0; i < _size; i++)
            {
                //tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1F / _model.GetSize));
                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 1F / _model.GetSize));
                for (int j = 0; j < _size; j++)
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
                    //_buttonGrid[i, j].BackColor = Color.White;
                    Controls.Add(_buttonGrid[i, j]);
                    tableLayoutPanel1.Controls.Add(_buttonGrid[i, j], i, j);
                    _buttonGrid[i, j].Click += OnCellClick;
                    //_buttonGrid[i, j].Click += PlayerTurn;

                    
                }
            }

            timer1.Start();
            StartPause=true;
            RefreshTable();
        }


        public void RefreshTable()
        {
            for (int i = 0; i < _model.GetSize; i++)
            {
                for (int j = 0; j < _model.GetSize; j++)
                {
                    if (_model.Table.GetTableValue(i, j) == 0)
                    {
                        _buttonGrid[i, j].BackColor = Color.White;
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[i, j].BackColor = Color.Blue;
                    }
                    if (_model.Table.GetTableValue(i, j) == 3 && _model.Table.WasWall(i, j) == 0)
                    {
                        _buttonGrid[i, j].BackColor = Color.Black;
                    }
                    if (_model.Table.WasWall(i, j) == 1)
                    {
                        _buttonGrid[i, j].BackColor  = Color.Green;
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[i, j].BackColor  = Color.Red;
                    }
                }
            }
            EnableButtons = true;
            StartPause=true;
        }

        private void OnCellClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int x;
            int y;
            if (button != null && EnableButtons)
            {
                x = button.TabIndex / 100;
                y = button.TabIndex % 100;


                _model.PlaceWall(x, y);

                RefreshTable();
            }
        }

        private void PauseStartGame(object? sender, EventArgs e)
        {
            if (StartPause || _model.IsGameOver)
            {
                StartPause = false;
                EnableButtons = false;
                saveToolStripMenuItem.Visible = true;
                loadToolStripMenuItem.Visible = true;
                timer1.Stop();
            }
            else
            {
                StartPause = true;
                EnableButtons = true;
                saveToolStripMenuItem.Visible = false;
                loadToolStripMenuItem.Visible = false;
                timer1.Start();
            }
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
                catch (LooseRobotDataException)
                {
                    MessageBox.Show("Saved successfully!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        {
            //RemoveGrid();

            if (openFileDialog1.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    if (_model != null)
                    {
                        _model.GameOver -= GameOver;

                    }
                    await _model.LoadGameAsync(openFileDialog1.FileName);
                    _model.GameOver += GameOver;
                    _size = _model.Table.GetSize;
                    saveToolStripMenuItem.Enabled = true;
                    //RemoveGrid();
                    //RefreshTable();
                    GenerateTable();

                }
                catch (LooseRobotDataException)
                {
                    MessageBox.Show("Game loading error!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //_model.NewGame();
                    //_menuFileSaveGame.Enabled = true;
                }

            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            _model.AdvanceTime();
            TimeLabel.Text = PlayerTime.ToString();
            RefreshTable();
        }
    }
}
