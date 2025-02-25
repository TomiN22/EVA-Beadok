using System.Windows.Forms;
using FenymotorModel.Model;
using FenymotorModel.Persistence;

namespace FenymotorWinForms
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private GameModel _model;
        private FenymotorFileDataAccess _dataAccess;
        private bool _enableButtons = true;
        private bool _startPause = true;

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

        public Form()
        {
            InitializeComponent();

            _dataAccess = new FenymotorFileDataAccess();
            _model = new GameModel(_dataAccess, _size);

            x12ToolStripMenuItem.Click += Menu12_Click;
            x24ToolStripMenuItem.Click += Menu24_Click;
            x36ToolStripMenuItem.Click += Menu36_Click;
            pauseToolStripMenuItem.Click += Pause_Click;

            saveToolStripMenuItem.Click += MenuFileSaveGame_Click;
            loadToolStripMenuItem.Click += MenuFileLoadGame_Click;

            EnableButtons = true;
            saveToolStripMenuItem.Enabled = false;
            loadToolStripMenuItem.Enabled = true;
            statusStrip1.Visible = false;
            TimeLabel.Text = _model.Table.GameTime.ToString();
        }

        public void Menu12_Click(object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 12;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer.Start();
        }

        public void Menu24_Click(object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 24;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer.Start();
        }

        public void Menu36_Click(object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 36;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer.Start();
        }

        private void Pause_Click(Object? sender, EventArgs e)
        {
            PauseStartGame();
        }

        private void KeyDownEvent(object? sender, KeyEventArgs e)
        {
            if (_enableButtons)
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        PauseStartGame();
                        break;
                    case Keys.A:
                        _model.KeyCommand("left", 1);
                        break;
                    case Keys.D:
                        _model.KeyCommand("right", 1);
                        break;
                    case Keys.J:
                        _model.KeyCommand("left", 2);
                        break;
                    case Keys.L:
                        _model.KeyCommand("right", 2);
                        break;
                    default:
                        break;
                }
                RefreshTable();
            }
            else if (e.KeyCode == Keys.Escape)
                PauseStartGame();
        }

        private void PauseStartGame()
        {
            if (_startPause || _model.IsGameOver)
            {
                StartPause = false;
                EnableButtons = false;
                saveToolStripMenuItem.Visible = true;
                loadToolStripMenuItem.Visible = true;
                saveToolStripMenuItem.Enabled = true;
                loadToolStripMenuItem.Enabled = true;
                timer.Stop();
            }
            else
            {
                StartPause = true;
                EnableButtons = true;
                saveToolStripMenuItem.Visible = false;
                loadToolStripMenuItem.Visible = false;
                saveToolStripMenuItem.Enabled = false;
                loadToolStripMenuItem.Enabled = false;
                timer.Start();
            }
        }

        public void GenerateTable()
        {
            RemoveGrid();
            tableLayoutPanel1.Enabled = true;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnCount = _model.GetSize;
            tableLayoutPanel1.RowCount = _model.GetSize;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.AutoSize = true;

            saveToolStripMenuItem.Enabled = false;
            loadToolStripMenuItem.Enabled = false;

            _buttonGrid = new Button[_size, _size];


            for (int i = 0; i < _size; i++)
            {
                //tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1F / _model.GetSize));
                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 1F / _model.GetSize));
                for (int j = 0; j < _size; j++)
                {

                    _buttonGrid[j, i] = new Button();
                    _buttonGrid[j, i].Location = new Point(0+50*i, 70+50*j); // elhelyezkedéss
                    if (_model.GetSize == 12)
                        _buttonGrid[j, i].Size = new Size(50, 50); // méret
                    else if(_model.GetSize == 24)
                        _buttonGrid[j, i].Size = new Size(25, 25);
                    else if(_model.GetSize == 36)
                        _buttonGrid[j, i].Size = new Size(15, 15);
                    //_buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttonGrid[j, i].Enabled = true; // kikapcsolt állapot
                    _buttonGrid[j, i].KeyDown += KeyDownEvent;
                    _buttonGrid[j, i].TabIndex = 100 * i + j; // a gomb számát a TabIndex-ben tároljuk
                    //_buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    //_buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    _buttonGrid[j, i].TabStop = false;
                    Controls.Add(_buttonGrid[j, i]);
                    tableLayoutPanel1.Controls.Add(_buttonGrid[j, i], j, i);
                    //_buttonGrid[j, i].Click += OnCellClick;
                    //_buttonGrid[i, j].Click += PlayerTurn;


                }
            }
            EnableButtons = true;
            statusStrip1.Visible = true;
            TimeLabel.Text = _model.Table.GameTime.ToString();
            StartPause=true;
            pauseToolStripMenuItem.Enabled = true;
            timer.Start();
            RefreshTable();
        }

        public void SetUpTable()
        {
            RemoveGrid();

            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnCount = _model.GetSize;
            tableLayoutPanel1.RowCount = _model.GetSize;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.AutoSize = true;

            pauseToolStripMenuItem.Enabled = true;
            TimeLabel.Text = "0:00:00";

            _buttonGrid = new Button[_size, _size];

            for (int j = 0; j < _size; j++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1F / _model.GetSize));
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 1F / _model.GetSize));

                for (int i = 0; i < _size; i++)
                {
                    _buttonGrid[j, i] = new Button();
                    _buttonGrid[j, i].Size = new Size(40, 40); // méret
                    //_buttonGrid[i, j].Location = new Point(0+100*i, 70+100*j); // elhelyezkedéss
                    _buttonGrid[j, i].Enabled = true; // kikapcsolt állapot
                    //_buttonGrid[i, j].TabStop = false;
                    _buttonGrid[j, i].Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
                    _buttonGrid[j, i].Dock = DockStyle.Fill;
                    _buttonGrid[j, i].KeyDown += KeyDownEvent;
                    _buttonGrid[j, i].TabIndex = 100 * i + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[j, i].BackColor = Color.AntiqueWhite;
                    _buttonGrid[j, i].TabStop = false;
                    _buttonGrid[j, i].Margin = new Padding(0, 0, 0, 0);
                    //Controls.Add(_buttonGrid[i, j]);
                    tableLayoutPanel1.Controls.Add(_buttonGrid[j, i], j, i);

                    if (_model.Table.GetTableValue(i, j) == 0)
                    {
                        _buttonGrid[j, i].BackColor = Color.White;
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[j, i].BackColor = Color.Blue;
                        _buttonGrid[j, i].Focus();
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[j, i].BackColor = Color.Red;
                    }
                    if (_model.Table.GetTableValue(i, j) == 11)
                    {
                        _buttonGrid[j, i].BackColor = Color.Red;
                    }
                    if (_model.Table.GetTableValue(i, j) == 22)
                    {
                        _buttonGrid[j, i].BackColor = Color.Red;
                    }
                }
            }

            RefreshTable();
        }

        public void RemoveGrid()
        {
            tableLayoutPanel1.Controls.Clear();
            pauseToolStripMenuItem.Enabled = false;

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

        public void RefreshTable()
        {
            TimeLabel.Text = TimeSpan.FromSeconds(_model.Table.GameTime).ToString("g");

            for (int j = 0; j<_size; j++)
            {
                for (int i = 0; i < _size; i++)
                {
                    //_buttonGrid[i, j].Text = String.Empty;
                    if (_model.Table.GetTableValue(i, j) == 0)
                    {
                        _buttonGrid[j, i].BackColor = Color.White;
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[j, i].BackColor = Color.Blue;
                        _buttonGrid[j, i].Focus();
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[j, i].BackColor = Color.Red;
                    }
                    if (_model.Table.GetTableValue(i, j) == 11)
                    {
                        _buttonGrid[j, i].BackColor = Color.SkyBlue;
                    }
                    if (_model.Table.GetTableValue(i, j) == 22)
                    {
                        _buttonGrid[j, i].BackColor = Color.Salmon;
                    }
                }
            }


        }

        private void GameOver(Object? sender, FenymotorEventArgs e)
        {
            timer.Stop();
            EnableButtons = false;
            if (e.Winner == Win.Player1)
            {
                MessageBox.Show("Player1 won!", "Game over!");
            }
            else if (e.Winner == Win.Player2)
            {
                MessageBox.Show("Player2 won!", "Game over!");
            }
            else
            {
                MessageBox.Show("It's a draw!", "Game over!");
            }

            tableLayoutPanel1.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            loadToolStripMenuItem.Visible = true;
            loadToolStripMenuItem.Enabled = true;
            //RemoveGrid();
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
                catch (FenymotorDataException)
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
                catch (FenymotorDataException)
                {
                    MessageBox.Show("Game loading error!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //_model.NewGame();
                    //_menuFileSaveGame.Enabled = true;
                }

            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _model.AdvanceTime();
            RefreshTable();
        }
    }
}
