using Model;

namespace MineSweeper
{
    public partial class Minesweeper : Form
    {
        private Button[,] _buttonGrid = null!;
        private int _size;
        private Action<int, int> clickAction;
         
        public Minesweeper()
        {
            InitializeComponent();
            SixSix.Click += MenuTableSix_Click; //feliratkozás egy eseményre
            TenTen.Click += MenuTableTen_Click;
            SixTeenSixTeen.Click += MenuTableSixTeen_Click;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void MenuTableSix_Click(Object sender, EventArgs e)
        {
            _size = 6;
            _buttonGrid = new Button[6,6];
            GenerateTable();
        }

        private void MenuTableTen_Click(Object sender, EventArgs e)
        {
            _size = 10;
            _buttonGrid = new Button[10,10];
            GenerateTable();
        }

        private void MenuTableSixTeen_Click(Object sender, EventArgs e)
        {
            _size = 16;
            _buttonGrid = new Button[16,16];
            GenerateTable();
        }

        public void GenerateTable()
        {
            for(int i=0; i < _size; i++)
            {
                for(int j=0; j < _size; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(0+50*i, 50+50*j); // elhelyezkedéss
                    _buttonGrid[i, j].Size = new Size(50, 50); // méret
                    //_buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttonGrid[i, j].Enabled = false; // kikapcsolt állapot
                    //_buttonGrid[i, j].TabIndex = 100 + i * _model.Table.Size + j; // a gomb számát a TabIndex-ben tároljuk
                    //_buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    //_buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    //_buttonGrid[i, j].TabStop = false;
                    Controls.Add(_buttonGrid[i, j]);

                    _buttonGrid[i, j].Click += OpenCell;
                }
            }
            
        }

        public void OnCellClick(Action<int, int> method)
        {
            clickAction = method;
        }

        private void OpenCell(Object sender, EventArgs e)
        {
            if(clickAction != null)
            {
                int x = (sender as Button).Left / (sender as Button).Width;
                int y = (sender as Button).Top / (sender as Button).Height;
                System.Diagnostics.Debug.WriteLine($"CLICK {x},{y}");

                clickAction(x, y);
            }
            
        }
    }
}