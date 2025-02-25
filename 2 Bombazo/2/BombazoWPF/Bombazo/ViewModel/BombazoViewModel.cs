using BombazoModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace BombazoWPF.ViewModel
{
    public class BombazoViewModel : ViewModelBase
    {
        #region Fields

        private GameModel _model; // modell
        public int _size;
        private int _basterds;
        private int _allBasterds;
        private int _blownUp;
        public bool _startPause;
        private string _bombCooldown = "0";
        private String _textCommand = "";
        private String _rightColumnVisibility = "Collapsed";
        private bool _enableButtons = true;

        #endregion

        #region Properties

        public int Size { get { return _model.GetSize; } }

        public bool EnableButtons
        { 
            get { return _enableButtons; }
            set 
            { 
                _enableButtons = value;
                OnPropertyChanged();
            }
        }

        public int EnemiesBlownUp
        {
            get { return _model.EnemiesBlownUp; }
            set
            {
                _blownUp = value;
                OnPropertyChanged();
            }
        }
        
        public int Basterds
        {
            get { return _model.CountOfEnemies; }
            set
            {
                _basterds = value;
                OnPropertyChanged();
            }
        }
        public int AllBasterds
        {
            get { return _model.AllCountOfEnemies; }
            set
            {
                _allBasterds = value;
                OnPropertyChanged();
            }
        }

        public bool StartPause
        {
            get { return _startPause; }
            set
            {
                _startPause = value;
                OnPropertyChanged();
            }
        }

        public string BombCooldown
        {
            get => _bombCooldown;
            set
            {
                if (value == _bombCooldown) return;

                _bombCooldown = value;
                OnPropertyChanged();
            }
        }

        public String RightColumnVisibility
        {
            get { return _rightColumnVisibility; }
            set { _rightColumnVisibility=value; OnPropertyChanged(); }
        }


        public GameModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
            }
        }

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand Lvl1Command { get; private set; }
        public DelegateCommand Lvl2Command { get; private set; }
        public DelegateCommand Lvl3Command { get; private set; }
        public DelegateCommand PauseStartGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand KeyCommand { get; private set; }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<Field> Fields { get; set; }

        public string PlayerTime { get { return TimeSpan.FromSeconds(_model.Table.GameTime).ToString("g"); } }


        public string Path { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler? NewGame;

        public event EventHandler? Lvl1Game;
        public event EventHandler? Lvl2Game;
        public event EventHandler? Lvl3Game;
        public event EventHandler? PauseStart;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler? LoadGame;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler? SaveGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler? ExitGame;

        #endregion

        #region Constructors

        /// <summary>
        /// Robot nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public BombazoViewModel(GameModel model)
        {
            // játék csatlakoztatása
            _model = model;
            Lvl1Command = new DelegateCommand(param => OnLvl1());
            Lvl2Command = new DelegateCommand(param => OnLvl2());
            Lvl3Command = new DelegateCommand(param => OnLvl3());

            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            PauseStartGameCommand = new DelegateCommand(param => PauseStartGame());
            KeyCommand = new(param => OnKeyCommand(param?.ToString() ?? String.Empty));
            //_model.GameOver += Model_GameOver;
            Path="";
            EnableButtons = true;

            Fields = new ObservableCollection<Field>();

            //RefreshTable();
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Tábla frissítése.
        /// </summary>
        private void RefreshTable()
        {
            //if (!_model.IsGameOver)
            
            foreach (Field field in Fields)
            {
                if (_model.Table.GetTableValue(field.X, field.Y) == 0)
                {
                    field.Colour = "White"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 1)
                {
                    field.Colour = "Black"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 2)
                {
                    field.Colour = "Red"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 3)
                {
                    field.Colour = "Blue"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 4)
                {
                    field.Colour = "Yellow"; OnPropertyChanged("Colour");
                }
                if(_model.Table.GetTableValue(field.X, field.Y) == 0 && _model.Field[field.X,field.Y] == GameModel.FieldValue.BombRange)
                {
                    field.Colour = "Yellow"; OnPropertyChanged("Colour");
                }
                field.Text = String.Empty;
            }

            _model.Bombs.ForEach(b => Fields[b.PositionY + b.PositionX * Size].Text = b.Time.ToString());
            OnPropertyChanged(nameof(PlayerTime));
            OnPropertyChanged(nameof(Text));
            EnemiesBlownUp = _model.EnemiesBlownUp;
            BombCooldown = _model.BombCooldown.ToString();

            //OnPropertyChanged(nameof(Basterds));
            Basterds =_model.CountOfEnemies;
            AllBasterds=_model.AllCountOfEnemies-Basterds;
            
        }

        public void LoadCreateTable()
        {
            for (int i = 0; i < _model.GetSize; i++)
            {
                for (int j = 0; j < _model.GetSize; j++)
                {
                    Fields.Add(new Field()
                    {
                        IsLocked = false,
                        Text = String.Empty,
                        Size = this.Size,
                        X = i,
                        Y = j,
                        Number = i * Size + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                        Colour = "White",
                        //StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });

                }
            }
            BombCooldown = "0";
            RightColumnVisibility = "Visible";
            OnPropertyChanged(nameof(RightColumnVisibility));
            OnPropertyChanged(nameof(Size));
            _model.GameAdvanced += Model_GameAdvanced;
            _model.GameAdvanced += new EventHandler<BombazoEventArgs>(Model_GameAdvanced);
            RefreshTable();
            _model.GameOver += Model_GameOver;
        }
        #endregion

        #region Game event handlers

        private void OnKeyCommand(string key)
        {
            if (EnableButtons)
            {
                int x = _model.Table.PCurrent[0];
                int y = _model.Table.PCurrent[1];
                switch (key)
                {
                    case "w":
                        _model.MovePlayer(x-1, y);
                        break;
                    case "a":
                        _model.MovePlayer(x, y-1);
                        break;
                    case "d":
                        _model.MovePlayer(x, y+1);
                        break;
                    case "s":
                        _model.MovePlayer(x+1, y);
                        break;
                    case "b":
                        _model.PlantBomb(x,y);
                        break;
                    default:
                        break;
                }
                RefreshTable();
            }
            
        }

        private void PauseStartGame()
        {
            if (StartPause || _model.IsGameOver)
            {
                StartPause = false;
                EnableButtons = false;
            }
            else
            {
                StartPause = true;
                EnableButtons = true;
            }
            PauseStart?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(PlayerTime));
        }

        private void Model_GameAdvanced(object? sender, BombazoEventArgs e)
        {
            RefreshTable();
            OnPropertyChanged(nameof(PlayerTime));
        }

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object? sender, BombazoEventArgs e)
        {
            foreach (Field field in Fields)
            {
                field.IsLocked = true; // minden mezőt lezárunk
                field.IsEnabled = false;
                //_model.Step(field.X, field.Y);
            }
            RightColumnVisibility="Collapsed";
        }

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        /*private void Model_GameAdvanced(object? sender, MineEventArgs e)
        {
            OnPropertyChanged(nameof(GameTime));
        }*/


        #endregion

        #region Event methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnLvl1()
        {
            Fields.Clear();
            Lvl1Game?.Invoke(this, EventArgs.Empty);
        }

        private void OnLvl2()
        {
            Fields.Clear();
            Lvl2Game?.Invoke(this, EventArgs.Empty);
        }

        private void OnLvl3()
        {
            Fields.Clear();
            Lvl3Game?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            Fields.Clear();
            LoadGame?.Invoke(this, EventArgs.Empty);
            _model.LoadGame(Path);
            LoadCreateTable();
        }

        /// <summary>
        /// Játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion

    }
}
