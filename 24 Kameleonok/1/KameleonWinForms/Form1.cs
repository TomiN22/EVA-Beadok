using System.Windows.Forms;
using KameleonModel.Model;
using KameleonModel.Persistence;

namespace KameleonWinForms
{
    public partial class Form1 : Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private GameModel _model;
        private KameleonFileDataAccess _dataAccess;
        private bool _enableButtons = false;
        private bool _startPause;
        private Image _greenImg;
        private Image _redImg;
        private Image _bTriangle;
        private Image _wTriangle;

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

            _greenImg = Image.FromFile("./pic/greencircle.PNG");
            _redImg = Image.FromFile("./pic/redcircle.PNG");

            _dataAccess = new KameleonFileDataAccess();
            _model = new GameModel(_dataAccess, _size);

            x3ToolStripMenuItem.Click += Menu_3GameClick; //feliratkozás egy eseményre
            x5ToolStripMenuItem.Click += Menu_5GameClick;
            x7ToolStripMenuItem.Click += Menu_7GameClick;
            saveToolStripMenuItem.Click += MenuFileSaveGame_Click;
            loadToolStripMenuItem.Click += MenuFileLoadGame_Click;
            exitToolStripMenuItem.Click += MenuExit_Click;
            saveToolStripMenuItem.Enabled = false;
            EnableButtons = true;
            statusStrip1.Visible = false;
        }

        private void Menu_3GameClick(Object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
            }
            _size = 3;
            _model = new GameModel(_dataAccess, _size);
            _model.GameOver += GameOver;

            _model.GenerateFields();
            GenerateTable();
            timer1.Start();
        }

        private void Menu_5GameClick(Object? sender, EventArgs e)
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
            timer1.Start();
        }

        private void Menu_7GameClick(Object? sender, EventArgs e)
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

        private void MenuExit_Click(Object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GameOver(Object? sender, KameleonEventArgs e)
        {
            timer1.Stop();
            EnableButtons = false;

            if (e.Winner == Win.Player1)
                MessageBox.Show(e.Winner+" has won!", "Game over!");
            if (e.Winner == Win.Player2)
                MessageBox.Show(e.Winner+" has won!", "Game over!");
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
            statusStrip1.Visible = true;

            _buttonGrid = new Button[_size, _size];

            for (int i = 0; i < _size; i++)
            {
                //tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1F / _model.GetSize));
                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 1F / _model.GetSize));
                for (int j = 0; j < _size; j++)
                {
                    _buttonGrid[j, i] = new Button();
                    _buttonGrid[j, i].Location = new Point(0+50*i, 70+50*j); // elhelyezkedéss
                    _buttonGrid[j, i].Size = new Size(90, 90); // méret
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
                        _buttonGrid[j, i].BackColor = Color.Gray;
                        _buttonGrid[j, i].Image = null;
                    }
                    if (_model.Table.GetTableValue(i, j) == 1)
                    {
                        _buttonGrid[j, i].Image = _greenImg;
                    }
                    if (_model.Table.GetTableValue(i, j) == 2)
                    {
                        _buttonGrid[j, i].Image = _redImg;
                    }
                    if (_model.Table.GetTableColor(i, j) == 1)
                    {
                        _buttonGrid[j, i].BackColor = Color.LightGreen;
                    }
                    if (_model.Table.GetTableColor(i, j) == 2)
                    {
                        _buttonGrid[j, i].BackColor = Color.IndianRed;
                    }
                }
            }
            EnableButtons = true;
            StartPause=true;
            p1TimeLabel.Text = "Time: "+P1Time;
            p2TimeLabel.Text = "Time: "+P2Time;
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

        private async void MenuFileSaveGame_Click(Object? sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await _model.SaveGameAsync(saveFileDialog1.FileName);
                }
                catch (KameleonDataException)
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
                catch (KameleonDataException)
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
