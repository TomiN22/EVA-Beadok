using System;
using System.Windows.Forms;
using AsteroidsModel.Model;
using AsteroidsModel.Persistence;

namespace AsteroidsWinForms
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private GameModel _model;
        private AsteroidsFileDataAccess _dataAccess;
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
            _dataAccess = new AsteroidsFileDataAccess();
            _model = new GameModel(_dataAccess, _size);
            newGameToolStripMenuItem.Click += MenuNewGame_Click; //feliratkozás egy eseményre
            saveToolStripMenuItem.Click += MenuFileSaveGame_Click;
            loadToolStripMenuItem.Click += MenuFileLoadGame_Click;
            pauseToolStripMenuItem.Click += Pause_Click;
            //timer1.Tick += timer1_Tick;
            EnableButtons = true;
        }

        private void MenuNewGame_Click(Object sender, EventArgs e)
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


        private void GameOver(Object? sender, AsteroidsEventArgs e)
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
            RemoveGrid();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnCount = _model.GetSize;
            tableLayoutPanel1.RowCount = _model.GetSize;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.AutoSize = true;

            saveToolStripMenuItem.Enabled = true;
            timeLabel.Text = "0:00:00";
            

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
                    _buttonGrid[i, j].KeyDown += KeyDownEvent;
                    _buttonGrid[i, j].TabIndex = 100 * i + j; // a gomb számát a TabIndex-ben tároljuk
                    //_buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    //_buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    _buttonGrid[i, j].TabStop = false;
                    Controls.Add(_buttonGrid[i, j]);
                    tableLayoutPanel1.Controls.Add(_buttonGrid[i, j], i, j);
                    //_buttonGrid[i, j].Click += OnCellClick;
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
                        _buttonGrid[i, j].Focus();
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[i, j].BackColor  = Color.Black;
                    }
                }
            }
            EnableButtons = true;
            StartPause=true;
        }

        private void Pause_Click(Object? sender, EventArgs e)
        {
            PauseStartGame();
        }

        private void KeyDownEvent(object? sender, KeyEventArgs e)
        {
            if (_enableButtons)
            {
                int x = _model.Table.PCurrent[1];
                int y = _model.Table.PCurrent[0];
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        PauseStartGame();
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


        private async void MenuFileSaveGame_Click(Object sender, EventArgs e)
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await _model.SaveGameAsync(saveFileDialog1.FileName);
                }
                catch (AsteroidsDataException)
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
                catch (AsteroidsDataException)
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
            timeLabel.Text = PlayerTime.ToString();
            RefreshTable();
        }
    }
}
