using PotyogosAmobaModel.Model;
using System.Windows.Forms;
using PotyogosAmobaModel.Persistence;

namespace PotyogosAmobaWinForms
{
    public partial class Form1 : Form
    {
        private Button[,] _buttonGrid = null!;
        private Button[] _columnHeaders = null!;
        private int _size;
        private GameModel _model;
        private PotyogosAmobaFileDataAccess _dataAccess;
        private bool _enableButtons = false;
        private bool _startPause;

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

        public string P1Time { get { return TimeSpan.FromSeconds(_model.Table.P1Time).ToString("g"); } }
        public string P2Time { get { return TimeSpan.FromSeconds(_model.Table.P2Time).ToString("g"); } }

        public Form1()
        {
            InitializeComponent();

            _dataAccess = new PotyogosAmobaFileDataAccess();
            _model = new GameModel(_dataAccess, _size);
            x10ToolStripMenuItem.Click += Menu_10GameClick;
            x20ToolStripMenuItem.Click += Menu_20GameClick;
            x30ToolStripMenuItem.Click += Menu_30GameClick;

            stopToolStripMenuItem.Click += StartStop_Click;

            saveToolStripMenuItem.Click += MenuFileSaveGame_Click;
            loadToolStripMenuItem.Click += MenuFileLoadGame_Click;
            exitToolStripMenuItem.Click += MenuExit_Click;

            EnableButtons = true;
            saveToolStripMenuItem.Enabled = false;
            loadToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = false;
            statusStrip1.Visible = false;
        }

        private void Menu_10GameClick(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 10;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
        }

        private void Menu_20GameClick(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 20;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
        }

        private void Menu_30GameClick(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 30;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
        }

        private void MenuExit_Click(Object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GameOver(Object? sender, PotyogosAmobaEventArgs e)
        {
            timer1.Stop();
            EnableButtons = false;
            saveToolStripMenuItem.Enabled = false;

            if (e.Winner == Win.Player1)
                MessageBox.Show(e.Winner+" has won!", "Game over!");
            if (e.Winner == Win.Player2)
                MessageBox.Show(e.Winner+" has won!", "Game over!");
            if (e.Winner == Win.Draw)
                MessageBox.Show("It's a "+e.Winner+"!", "Game over!");

            //DialogResult result = MessageBox.Show("Would you like to start a new game?", "Game over", MessageBoxButtons.YesNo);

            //if (result == DialogResult.Yes)
            //{
            //    if (_model.GetSize == 4)
            //        Menu_4GameClick(sender, EventArgs.Empty);
            //    if (_model.GetSize == 6)
            //        Menu_6GameClick(sender, EventArgs.Empty);
            //    if (_model.GetSize == 8)
            //        Menu_8GameClick(sender, EventArgs.Empty);
            //}
            //else
            //{
            //    this.Close(); // Ha nem akar új játékot, bezárjuk az alkalmazást
            //}
        }

        public void RemoveGrid()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel2.Controls.Clear();
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
            if(_columnHeaders != null )
            {
                for (int i = 0; i < _columnHeaders.GetLength(0); i++)
                {
                    Controls.Remove(_columnHeaders[i]);
                }
            }
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

            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.ColumnCount = _model.GetSize;
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.ColumnStyles.Clear();
            tableLayoutPanel2.RowStyles.Clear();
            tableLayoutPanel2.AutoSize = true;

            saveToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = true;
            statusStrip1.Visible = true;

            _buttonGrid = new Button[_size, _size];
            _columnHeaders = new Button[_size];

            for (int i = 0; i < _size; i++)
            {
                //tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1F / _model.GetSize));
                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 1F / _model.GetSize));
                for (int j = 0; j < _size; j++)
                {
                    _buttonGrid[j, i] = new Button();
                    _buttonGrid[j, i].Location = new Point(0+50*i, 70+50*j); // elhelyezkedéss
                    if(_size == 10)
                        _buttonGrid[j, i].Size = new Size(60, 60); // méret
                    if (_size == 20)
                        _buttonGrid[j, i].Size = new Size(30, 30);
                    if (_size == 30)
                        _buttonGrid[j, i].Size = new Size(20, 20);
                    //_buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttonGrid[j, i].Enabled = true; // kikapcsolt állapot
                    //_buttonGrid[i, j].KeyDown += KeyDownEvent;
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

            for (int i = 0; i < _size; i++)
            {
                _columnHeaders[i] = new Button();
                _columnHeaders[i].Location = new Point(0+50*i); // elhelyezkedéss
                if (_size == 10)
                    _columnHeaders[i].Size = new Size(60, 60); // méret
                if (_size == 20)
                    _columnHeaders[i].Size = new Size(30, 30);
                if (_size == 30)
                    _columnHeaders[i].Size = new Size(20, 20);
                //_columnHeaders[j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                _columnHeaders[i].Enabled = true; // kikapcsolt állapot
                //_columnHeaders[j].KeyDown += KeyDownEvent;
                _columnHeaders[i].TabIndex = 400 * i; // a gomb számát a TabIndex-ben tároljuk
                //_columnHeaders[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                //_columnHeaders[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                _columnHeaders[i].TabStop = false;
                Controls.Add(_columnHeaders[i]);
                tableLayoutPanel2.Controls.Add(_columnHeaders[i], i, 0);
                _columnHeaders[i].Click += OnCellClick;
                //_columnHeaders[i, j].Click += PlayerTurn;
            }

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
                        _buttonGrid[j, i].BackColor = Color.White;
                        _buttonGrid[j, i].Text=String.Empty;
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[j, i].BackColor = Color.White;
                        _buttonGrid[j, i].Text = "O";
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[j, i].BackColor = Color.White;
                        _buttonGrid[j, i].Text = "X";
                    }
                    if (_model.Table.HasColor(i, j) == true)
                    {
                        _buttonGrid[j, i].BackColor = Color.Red;
                    }
                }
            }
            EnableButtons = true;
            //StartPause=true;
            p1timeLabel.Text = "P1 Time: "+P1Time;
            p2timeLabel.Text = "P2 Time: "+P2Time;
        }

        private void OnCellClick(object? sender, EventArgs e)
        {
            Button? button = sender as Button;
            int x;
            int y;
            if (button != null)
            {
                x = button.TabIndex / 100;
                y = button.TabIndex / 400;

                if (StartPause)
                    _model.Step(y);

                RefreshTable();
            }
        }

        private void StartStop_Click(Object? sender, EventArgs e)
        {
            PauseStartGame();
        }

        private void PauseStartGame()
        {
            if (StartPause || _model.IsGameOver)
            {
                StartPause = false;
                EnableButtons = true;
                stopToolStripMenuItem.Text="Start";
                //saveToolStripMenuItem.Visible = true;
                //loadToolStripMenuItem.Visible = true;
                timer1.Stop();
            }
            else
            {
                StartPause = true;
                EnableButtons = false;
                stopToolStripMenuItem.Text="Stop";
                //saveToolStripMenuItem.Visible = false;
                //loadToolStripMenuItem.Visible = false;
                //_model.StartGame();
                timer1.Start();
            }
        }

        private async void MenuFileSaveGame_Click(Object? sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await _model.SaveGameAsync(saveFileDialog1.FileName);
                }
                catch (PotyogosAmobaDataException)
                {
                    MessageBox.Show("Saved successfully!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void MenuFileLoadGame_Click(Object? sender, EventArgs e)
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
                    timer1.Start();
                }
                catch (PotyogosAmobaDataException)
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
