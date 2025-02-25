using RunModel.Model;
using RunModel.Persistence;
using System.Windows.Forms;

namespace MenekuljWinForms
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private GameModel _model;
        private RunFileDataAccess _dataAccess;
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

        public string Player { get; set; }

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
            _dataAccess = new RunFileDataAccess();
            _model = new GameModel(_dataAccess, _size);
            x11ToolStripMenuItem.Click += ElevenGame_Click;
            x15ToolStripMenuItem.Click += FifteenGame_Click;
            x21ToolStripMenuItem.Click += TwentyoneGame_Click;
            saveToolStripMenuItem.Click += MenuFileSaveGame_Click;
            loadToolStripMenuItem.Click += MenuFileLoadGame_Click;
            pauseToolStripMenuItem.Click += Pause_Click;
            //timer1.Tick += timer1_Tick;
            EnableButtons = true;
            saveToolStripMenuItem.Enabled = false;
            loadToolStripMenuItem.Enabled = true;
            statusStrip1.Visible = false;
            TimeLabel.Text = _model.Table.GameTime.ToString();
            EnemiesLabel.Text = _model.Enemies.Count.ToString();
        }

        private void ElevenGame_Click(Object sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 11;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
        }

        private void FifteenGame_Click(Object sender, EventArgs e)
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

        private void TwentyoneGame_Click(Object sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 21;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
        }

        private void GameOver(Object? sender, RunEventArgs e)
        {
            timer1.Stop();
            if (e.HasWon)
                MessageBox.Show("Congrats! You won!", "Game over!");
            else
                MessageBox.Show("You lost!", "Game over!");

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
            statusStrip1.Visible = false;
        }

        public void GenerateTable()
        {
            RemoveGrid();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnCount = _model.GetSize;
            tableLayoutPanel1.RowCount = _model.GetSize;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.AutoSize = true;

            saveToolStripMenuItem.Enabled = true;

            _buttonGrid = new Button[_size, _size];


            for (int i = 0; i < _size; i++)
            {
                //tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1F / _model.GetSize));
                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 1F / _model.GetSize));
                for (int j = 0; j < _size; j++)
                {

                    _buttonGrid[j,i] = new Button();
                    _buttonGrid[j,i].Location = new Point(0+50*i, 70+50*j); // elhelyezkedéss
                    if(_model.GetSize != 21)
                        _buttonGrid[j,i].Size = new Size(50, 50); // méret
                    else
                        _buttonGrid[j, i].Size = new Size(35, 35);
                    //_buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttonGrid[j,i].Enabled = true; // kikapcsolt állapot
                    _buttonGrid[j,i].KeyDown += KeyDownEvent;
                    _buttonGrid[j,i].TabIndex = 100 * i + j; // a gomb számát a TabIndex-ben tároljuk
                    //_buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    //_buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    _buttonGrid[j,i].TabStop = false;
                    Controls.Add(_buttonGrid[j,i]);
                    tableLayoutPanel1.Controls.Add(_buttonGrid[j,i], j,i);
                    //_buttonGrid[j, i].Click += OnCellClick;
                    //_buttonGrid[i, j].Click += PlayerTurn;


                }
            }
            statusStrip1.Visible = true;
            TimeLabel.Text = _model.Table.GameTime.ToString();
            EnemiesLabel.Text = _model.Enemies.Count.ToString();
            StartPause=true;
            timer1.Start();
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
                        _buttonGrid[j,i].BackColor = Color.White;
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[j,i].BackColor = Color.Black;
                        _buttonGrid[j,i].Focus();
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[j,i].BackColor  = Color.Red;
                    }
                    if (_model.Table.GetTableValue(i, j) == 3)
                    {
                        _buttonGrid[j,i].BackColor  = Color.Blue;
                    }
                }
            }
            EnableButtons = true;
            StartPause=true;
            TimeLabel.Text = _model.Table.GameTime.ToString();
            EnemiesLabel.Text = _model.Enemies.Count.ToString();
        }



        private void Pause_Click(Object? sender, EventArgs e)
        {
            PauseStartGame();
        }

        private void PauseStartGame()
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

        private void KeyDownEvent(object? sender, KeyEventArgs e)
        {
            if (_enableButtons)
            {
                int x = _model.Table.PCurrent[0];
                int y = _model.Table.PCurrent[1];
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        PauseStartGame();
                        break;
                    case Keys.W:
                        _model.MovePlayer(x-1, y);
                        break;
                    case Keys.S:
                        _model.MovePlayer(x+1, y);
                        break;
                    case Keys.A:
                        _model.MovePlayer(x, y-1);
                        break;
                    case Keys.D:
                        _model.MovePlayer(x, y+1);
                        break;
                    default:
                        break;
                }
                RefreshTable();
            }
            else if (e.KeyCode == Keys.Escape)
                PauseStartGame();
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
                catch (RunDataException)
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
                catch (RunDataException)
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
            RefreshTable();
        }
    }
}
