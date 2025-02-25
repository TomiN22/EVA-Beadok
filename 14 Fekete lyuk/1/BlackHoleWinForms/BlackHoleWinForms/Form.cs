using System.Windows.Forms;
using BlackHoleModel.Model;
using BlackHoleModel.Persistence;

namespace BlackHoleWinForms
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private GameModel _model;
        private BlackHoleFileDataAccess _dataAccess;
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

        public string PlayerTime { get { return TimeSpan.FromSeconds(_model.Table.GameTime).ToString("g"); } }

        public Form()
        {
            InitializeComponent();

            saveToolStripMenuItem.Enabled = false;
            _dataAccess = new BlackHoleFileDataAccess();
            _model = new GameModel(_dataAccess, _size);
            x5ToolStripMenuItem.Click += FiveGame_Click;
            x7ToolStripMenuItem.Click += SevenGame_Click;
            x9ToolStripMenuItem.Click += NineGame_Click;
            saveToolStripMenuItem.Click += MenuFileSaveGame_Click;
            loadToolStripMenuItem.Click += MenuFileLoadGame_Click;
            //timer1.Tick += timer1_Tick;
            EnableButtons = true;
            saveToolStripMenuItem.Enabled = false;
            loadToolStripMenuItem.Enabled = true;
            P1ShipsLabel.Visible = true;
            P2ShipsLabel.Visible = true;
            P1ShipsTextLabel.Visible = true;
            P2ShipsTextLabel.Visible = true;
            PlayerLabel.Visible = false;
            P1ShipsLabel.Text = _model.Table.P1ShipsInHole.ToString();
            P2ShipsLabel.Text = _model.Table.P2ShipsInHole.ToString();
            PlayerLabel.Text = "Player"+_model.Table.Player.ToString();
        }

        private void FiveGame_Click(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 5;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
        }

        private void SevenGame_Click(Object? sender, EventArgs e)
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
        }

        private void NineGame_Click(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 9;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
        }

        private void GameOver(Object? sender, BlackHoleEventArgs e)
        {
            MessageBox.Show("Player"+_model.Table.Player+" won!", "Game over!");

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
            P1ShipsLabel.Visible = false;
            P2ShipsLabel.Visible = false;
            P1ShipsTextLabel.Visible = false;
            P2ShipsTextLabel.Visible = false;
            PlayerLabel.Visible = false;
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
                    _buttonGrid[j, i].Location = new Point(0+50*i, 70+50*j); // elhelyezkedéss
                    _buttonGrid[j, i].Size = new Size(50, 50); // méret
                    //_buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttonGrid[j, i].Enabled = true; // kikapcsolt állapot
                    //_buttonGrid[i, j].KeyDown += KeyDownEvent;
                    _buttonGrid[j, i].TabIndex = 100 * i + j; // a gomb számát a TabIndex-ben tároljuk
                    //_buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    //_buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    _buttonGrid[j, i].TabStop = false;
                    Controls.Add(_buttonGrid[j, i]);
                    tableLayoutPanel1.Controls.Add(_buttonGrid[j, i], j, i);
                    _buttonGrid[j, i].Click += OnCellClick;
                    //_buttonGrid[i, j].Click += PlayerTurn;


                }
            }
            P1ShipsLabel.Visible = true;
            P2ShipsLabel.Visible = true;
            P1ShipsTextLabel.Visible = true;
            P2ShipsTextLabel.Visible = true;
            PlayerLabel.Visible = true;
            StartPause=true;
            RefreshTable();
        }

        private void OnCellClick(object? sender, EventArgs e)
        {
            Button? button = sender as Button;
            int x;
            int y;
            if (button != null)
            {
                x = button.TabIndex / 100;
                y = button.TabIndex % 100;


                _model.Step(x, y);

                RefreshTable();
            }
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
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[j, i].BackColor = Color.Black;
                        //_buttonGrid[i, j].Focus();
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[j, i].BackColor  = Color.Red;
                    }
                    if (_model.Table.GetTableValue(i, j) == 3)
                    {
                        _buttonGrid[j, i].BackColor  = Color.Blue;
                    }
                }
            }
            EnableButtons = true;
            StartPause=true;
            P1ShipsLabel.Text = _model.Table.P1ShipsInHole.ToString();
            P2ShipsLabel.Text = _model.Table.P2ShipsInHole.ToString();
            PlayerLabel.Text = "Player"+_model.Table.Player.ToString();
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
            }
            else
            {
                StartPause = true;
                EnableButtons = true;
                saveToolStripMenuItem.Visible = false;
                loadToolStripMenuItem.Visible = false; //timer.Start/Stop
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
                catch (BlackHoleDataException)
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
                        await _model.LoadGameAsync(openFileDialog1.FileName);
                        _model.GameOver += GameOver;
                        _size = _model.Table.GetSize;
                        saveToolStripMenuItem.Enabled = true;
                        //RemoveGrid();
                        //RefreshTable();
                        GenerateTable();
                    }
                    //else
                    //{
                    //    _model.GameOver -= GameOver;
                    //}
                }
                catch (BlackHoleDataException)
                {
                    MessageBox.Show("Game loading error!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //_model.NewGame();
                    //_menuFileSaveGame.Enabled = true;
                }

            }
        }
    }
}
