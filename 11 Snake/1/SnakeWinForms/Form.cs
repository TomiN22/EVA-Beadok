using SnakeModel.Model;
using SnakeModel.Persistence;

namespace SnakeWinForms
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private GameModel _model;
        private SnakeFileDataAccess _dataAccess;
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

            _dataAccess = new SnakeFileDataAccess();
            _model = new GameModel(_dataAccess, _size);
            level1ToolStripMenuItem.Click += ElevenGame_Click;
            level2ToolStripMenuItem.Click += FifteenGame_Click;
            level3ToolStripMenuItem.Click += TwentyoneGame_Click;
            pauseToolStripMenuItem.Click += Pause_Click;
            //timer1.Tick += timer1_Tick;
            EnableButtons = true;
            statusStrip1.Visible = false;
            TimeLabel.Text = _model.Table.GameTime.ToString();
            EggsLabel.Text = _model.Table.EggsEaten.ToString();
        }

        private void ElevenGame_Click(Object sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _enableButtons = true;
            _size = 15;
            _model = new GameModel(_dataAccess, _size);
            _model.LoadGame("./save/Level1.txt");
            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
            _model.GameOver += GameOver;
            timer1.Start();
        }

        private void FifteenGame_Click(Object sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _enableButtons = true;
            _size = 11;
            _model = new GameModel(_dataAccess, _size);
            _model.LoadGame("./save/Level2.txt");
            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
            _model.GameOver += GameOver;
            timer1.Start();
        }

        private void TwentyoneGame_Click(Object sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _enableButtons = true;
            _size = 17;
            _model = new GameModel(_dataAccess, _size);
            _model.LoadGame("./save/Level3.txt");
            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
            _model.GameOver += GameOver;
            timer1.Start();
        }

        private void GameOver(Object? sender, SnakeEventArgs e)
        {
            timer1.Stop();
            MessageBox.Show("You've lasted this long.", "Game over!");

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
            TimeLabel.Text = "0:00:00";

            _buttonGrid = new Button[_size, _size];


            for (int i = 0; i < _size; i++)
            {
                //tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1F / _model.GetSize));
                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 1F / _model.GetSize));
                for (int j = 0; j < _size; j++)
                {

                    _buttonGrid[j,i] = new Button();
                    _buttonGrid[j,i].Location = new Point(0+50*i, 70+50*j); // elhelyezked�ss
                    //if(_model.GetSize != 17)
                    //    _buttonGrid[j,i].Size = new Size(40, 50); // m�ret
                    //else
                    _buttonGrid[j, i].Size = new Size(35, 35);
                    //_buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // bet�t�pus
                    _buttonGrid[j,i].Enabled = true; // kikapcsolt �llapot
                    _buttonGrid[j,i].KeyDown += KeyDownEvent;
                    _buttonGrid[j,i].TabIndex = 100 * i + j; // a gomb sz�m�t a TabIndex-ben t�roljuk
                    //_buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lap�tott st�pus
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
            EggsLabel.Text = _model.Table.EggsEaten.ToString();
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
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[j,i].BackColor  = Color.Red;
                    }
                    if (_model.Table.GetTableValue(i, j) == 3)
                    {
                        _buttonGrid[j,i].BackColor  = Color.Red;
                        _buttonGrid[j, i].Focus();
                    }
                    if(_model.Table.GetTableValue(i, j) == 4)
                    {
                        _buttonGrid[j, i].BackColor  = Color.Yellow;
                    }
                }
            }
            EnableButtons = true;
            StartPause=true;
            TimeLabel.Text = _model.Table.GameTime.ToString();
            EggsLabel.Text = _model.Table.EggsEaten.ToString();
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
                timer1.Stop();
            }
            else
            {
                StartPause = true;
                EnableButtons = true;
                timer1.Start();
            }
        }

        private void KeyDownEvent(object? sender, KeyEventArgs e)
        {
            if (EnableButtons)
            {
                //int x = _model.Table.PCurrent[0];
                //int y = _model.Table.PCurrent[1];
                switch (e.KeyCode)
                {
                    case Keys.A:
                        _model.ChangeDirection("left");
                        break;
                    case Keys.D:
                        _model.ChangeDirection("right");
                        break;
                    default:
                        break;
                }
                RefreshTable();
            }

        }


        //private async void MenuFileSaveGame_Click(Object sender, EventArgs e)
        //{
        //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        try
        //        {
        //            // j�t� ment�se
        //            await _model.SaveGameAsync(saveFileDialog1.FileName);
        //        }
        //        catch (SnakeDataException)
        //        {
        //            MessageBox.Show("Saved successfully!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}

        //private async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        //{
        //    //RemoveGrid();

        //    if (openFileDialog1.ShowDialog() == DialogResult.OK) // ha kiv�lasztottunk egy f�jlt
        //    {
        //        try
        //        {
        //            // j�t�k bet�lt�se
        //            if (_model != null)
        //            {
        //                _model.GameOver -= GameOver;

        //            }
        //            await _model.LoadGameAsync(openFileDialog1.FileName);
        //            _model.GameOver += GameOver;
        //            _size = _model.Table.GetSize;
        //            saveToolStripMenuItem.Enabled = true;
        //            //RemoveGrid();
        //            //RefreshTable();
        //            GenerateTable();

        //        }
        //        catch (SnakeDataException)
        //        {
        //            MessageBox.Show("Game loading error!" + Environment.NewLine + "Wrong file path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //            //_model.NewGame();
        //            //_menuFileSaveGame.Enabled = true;
        //        }

        //    }
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            _model.AdvanceTime();
            RefreshTable();
        }
    }
}
