using BombazoModel.Model;
using BombazoModel.Persistence;
using System.Windows.Forms;

namespace BombazoWinForms
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private GameModel _model;
        private BombazoFileDataAccess _dataAccess;
        private bool _enableButtons = true;
        private bool _startPause = true;
        public Form()
        {
            InitializeComponent();

            _dataAccess = new BombazoFileDataAccess();
            _model = new GameModel(_dataAccess, _size);

            lvl1.Click += MenuLvl1_Click;
            lvl2.Click += MenuLvl2_Click;
            lvl3.Click += MenuLvl3_Click;
            pauseToolStripMenuItem.Click += Pause_Click;
        }

        public void MenuLvl1_Click(object? sender, EventArgs e)
        {
            _enableButtons = true;
            _size = 15;
            _model = new GameModel(_dataAccess, _size);
            _model.LoadGame("./save/Level1.txt");
            SetUpTable();
            timer.Start();
            _model.GameOver += GameOver;
        }

        public void MenuLvl2_Click(object? sender, EventArgs e)
        {
            _enableButtons = true;
            _size = 11;
            _model = new GameModel(_dataAccess, _size);
            _model.LoadGame("./save/Level2.txt");
            SetUpTable();
            timer.Start();
            _model.GameOver += GameOver;
        }

        public void MenuLvl3_Click(object? sender, EventArgs e)
        {
            _enableButtons = true;
            _size = 17;
            _model = new GameModel(_dataAccess, _size);
            _model.LoadGame("./save/Level3.txt");
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
            if (_enableButtons)
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
                    case Keys.B:
                        _model.PlantBomb(x,y);
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
                _enableButtons = false;
                timer.Stop();
            }
            else
            {
                _startPause = true;
                _enableButtons = true;
                timer.Start();
            }
        }

        public void SetUpTable()
        {
            RemoveGrid();

            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.ColumnCount = _model.GetSize;
            tableLayoutPanel.RowCount = _model.GetSize;
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.AutoSize = true;

            pauseToolStripMenuItem.Enabled = true;
            timeLabel.Text = "0:00:00";
            cooldownLabel.Text = "0";
            enemiesBlownLabel.Text = "0";
            
            _buttonGrid = new Button[_size, _size];

            for (int j = 0; j < _size; j++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1F / _model.GetSize));
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 1F / _model.GetSize));

                for (int i = 0; i < _size; i++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Size = new Size(40, 40); // méret
                    //_buttonGrid[i, j].Location = new Point(0+100*i, 70+100*j); // elhelyezkedéss
                    _buttonGrid[i, j].Enabled = true; // kikapcsolt állapot
                    //_buttonGrid[i, j].TabStop = false;
                    _buttonGrid[i, j].Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
                    _buttonGrid[i, j].Dock = DockStyle.Fill;
                    _buttonGrid[i, j].KeyDown += KeyDownEvent;
                    _buttonGrid[i, j].TabIndex = 100 * i + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].BackColor = Color.AntiqueWhite;
                    _buttonGrid[i, j].TabStop = false;
                    _buttonGrid[i, j].Margin = new Padding(0, 0, 0, 0);
                    //Controls.Add(_buttonGrid[i, j]);
                    tableLayoutPanel.Controls.Add(_buttonGrid[i, j], i, j);

                    if (_model.Table.GetTableValue(i, j) == 0)
                    {
                        _buttonGrid[i, j].BackColor = Color.White;
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[i, j].BackColor = Color.Black;
                        _buttonGrid[i, j].Focus();
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[i, j].BackColor = Color.Red;
                    }
                    if (_model.Table.GetTableValue(i, j) == 3)
                    {
                        _buttonGrid[i, j].BackColor = Color.Green;
                    }
                    //if (_model.Table.GetTableValue(i, j) == 4)
                    //{
                    //    _buttonGrid[i, j].BackColor = Color.Red;
                    //}
                }
            }

            RefreshTable();
        }

        public void RemoveGrid()
        {
            tableLayoutPanel.Controls.Clear();
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
            timeLabel.Text = TimeSpan.FromSeconds(_model.Table.GameTime).ToString("g");

            for (int j = 0; j<_size; j++)
            {
                for (int i = 0; i < _size; i++)
                {
                    _buttonGrid[i,j].Text = String.Empty;
                    if (_model.Table.GetTableValue(i, j) == 0)
                    {
                        _buttonGrid[i, j].BackColor = Color.White;
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[i, j].BackColor = Color.Black;
                        _buttonGrid[i, j].Focus();
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[i, j].BackColor = Color.Red;
                    }
                    if (_model.Table.GetTableValue(i, j) == 3)
                    {
                        _buttonGrid[i, j].BackColor = Color.Green;
                    }
                    if (_model.Table.GetTableValue(i, j) == 4)
                    {
                        _buttonGrid[i, j].BackColor = Color.Yellow;
                    }
                    if (_model.Table.GetTableValue(i, j) == 0 && _model.Field[i, j] == GameModel.FieldValue.BombRange)
                    {
                        _buttonGrid[i, j].BackColor = Color.Yellow;
                    }
                }
            }

            _model.Bombs.ForEach(b => _buttonGrid[b.PositionX, b.PositionY].Text = b.Time.ToString());

            cooldownLabel.Text = _model.BombCooldown.ToString();
            enemiesBlownLabel.Text = _model.EnemiesBlownUp.ToString();
            
        }

        private void GameOver(Object? sender, BombazoEventArgs e)
        {
            timer.Stop();
            if (_model.Enemies.Count==0)
            {
                MessageBox.Show("Congrats! You win!", "Game over!");
            }
            else
            {
                MessageBox.Show("You lost!", "Game over!");
            }

            tableLayoutPanel.Enabled = false;
            //RemoveGrid();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _model.AdvanceTime();
            RefreshTable();
        }

    }
}
