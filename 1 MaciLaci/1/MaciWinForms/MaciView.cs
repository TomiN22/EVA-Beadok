using MaciModel.Model;
using MaciModel.Persistence;

namespace MaciWinForms
{
    public partial class MaciView : Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private MaciGameModel _model;
        private MaciFileDataAccess _dataAccess;
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
        public MaciView()
        {
            InitializeComponent();

            _dataAccess = new MaciFileDataAccess();
            _model = new MaciGameModel(_dataAccess, _size);

            MenuItem4x4.Click += MenuFour_Click;
            MenuItem6x6.Click += MenuSix_Click;
            MenuItem8x8.Click += MenuEight_Click;
            pauseMenuItem.Click += Pause_Click;
        }

        private void MenuFour_Click(Object? sender, EventArgs e)
        {
            EnableButtons = true;
            _size = 4;
            _model = new MaciGameModel(_dataAccess, _size);
            _model.LoadGame("./save/4x4.txt");
            SetUpTable();
            timer.Start();
            _model.GameOver += GameOver;
        }

        private void MenuSix_Click(Object? sender, EventArgs e)
        {
            EnableButtons = true;
            _size = 6;
            _model = new MaciGameModel(_dataAccess, _size);
            _model.LoadGame("./save/6x6.txt");
            SetUpTable();
            timer.Start();
            _model.GameOver += GameOver;
        }

        private void MenuEight_Click(Object? sender, EventArgs e)
        {
            EnableButtons = true;
            _size = 8;
            _model = new MaciGameModel(_dataAccess, _size);
            _model.LoadGame("./save/8x8.txt");
            SetUpTable();
            timer.Start();
            _model.GameOver += GameOver;
        }

        private void Pause_Click(Object? sender, EventArgs e)
        {
            PauseStartGame();
        }

        private void KeyDownEvent(object? sender, KeyEventArgs e)
        {
            if (EnableButtons)
            {
                int x = _model.Table.PCurrent[0];
                int y = _model.Table.PCurrent[1];
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        PauseStartGame();
                        break;
                    case Keys.A:
                        _model.MovePlayer(x-1, y);
                        break;
                    case Keys.W:
                        _model.MovePlayer(x, y-1);
                        break;
                    case Keys.D:
                        _model.MovePlayer(x+1, y);
                        break;
                    case Keys.S:
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
            if (_startPause || _model.IsGameOver)
            {
                _startPause = false;
                EnableButtons = false;
                timer.Stop();
            }
            else
            {
                _startPause = true;
                EnableButtons = true;
                timer.Start();
            }
            //PauseStart?.Invoke(this, EventArgs.Empty);
            //OnPropertyChanged(nameof(PlayerTime));
        }

        public void SetUpTable()
        {
            RemoveGrid();
            timeLabel.Visible = true;
            timeTextLabel.Visible = true;
            picnicTextLabel.Visible = true;
            remBasketsLabel.Visible = true;
            acqBasketsLabel.Visible = true;

            remBasketsLabel.Text = _model.Baskets.ToString();
            acqBasketsLabel.Text = (_model.AllBaskets-_model.Baskets).ToString();
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
                    _buttonGrid[i, j].KeyDown += KeyDownEvent;
                    _buttonGrid[i, j].TabIndex = 100 * i + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].BackColor = Color.AntiqueWhite;
                    _buttonGrid[i, j].TabStop = false;
                    Controls.Add(_buttonGrid[i, j]);

                    if (_model.Table.GetTableValue(i, j) == 0)
                    {
                        _buttonGrid[i, j].BackColor = Color.White;
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[i, j].BackColor = Color.Brown;
                        _buttonGrid[i, j].Focus();
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[i, j].BackColor = Color.Yellow;
                    }
                    if (_model.Table.GetTableValue(i, j) == 3)
                    {
                        _buttonGrid[i, j].BackColor = Color.Green;
                    }
                    if (_model.Table.GetTableValue(i, j) == 4)
                    {
                        _buttonGrid[i, j].BackColor = Color.Red;
                    }
                }
            }

            RefreshTable();
        }

        public void RemoveGrid()
        {
            //timeLabel.Text = "0:00:00";
            timeLabel.Visible = false;
            timeTextLabel.Visible = false;
            picnicTextLabel.Visible = false;
            remBasketsLabel.Visible = false;
            acqBasketsLabel.Visible = false;

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
            timeLabel.Text = TimeSpan.FromSeconds(_model.Table.GameTime).ToString("g");

            for (int j=0; j<_size; j++)
            {
                for (int i = 0; i < _size; i++)
                {
                    if (_model.Table.GetTableValue(i, j) == 0)
                    {
                        _buttonGrid[i, j].BackColor = Color.White;
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[i, j].BackColor = Color.Brown;
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[i, j].BackColor = Color.Yellow;
                    }
                    if (_model.Table.GetTableValue(i, j) == 3)
                    {
                        _buttonGrid[i, j].BackColor = Color.Green;
                    }
                    if (_model.Table.GetTableValue(i, j) == 4)
                    {
                        _buttonGrid[i, j].BackColor = Color.Red;
                    }
                }
            }

            remBasketsLabel.Text = _model.Baskets.ToString();
            acqBasketsLabel.Text = (_model.AllBaskets-_model.Baskets).ToString();
        }

        private void GameOver(Object? sender, MaciEventArgs e)
        {
            timer.Stop();
            if (_model.Baskets == 0)
            {
                MessageBox.Show("Congrats! You've collected all the baskets!", "Game over!");
            }
            else
            {
                MessageBox.Show("You've been spotted! You lost!", "Game over!");
            }
            

            RemoveGrid();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _model.AdvanceTime();
            RefreshTable();
        }
    }
}
