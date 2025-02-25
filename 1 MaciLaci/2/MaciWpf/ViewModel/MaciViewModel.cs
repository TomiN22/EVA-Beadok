using MaciModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MaciWpf.ViewModel
{
    public class MaciViewModel : ViewModelBase
    {
        #region Fields

        private MaciGameModel _model; // modell
        public int _size;
        private int _baskets;
        private int _basketsAcq;
        public bool _startPause;

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
        public int PicnicBaskets
        {
            get { return _model.Baskets; }
            set
            {
                _baskets = value;
                OnPropertyChanged();
            }
        }
        public int PicnicBasketsAcq
        {
            get { return _basketsAcq; }
            set
            {
                _basketsAcq = value;
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

        public String RightColumnVisibility
        {
            get { return _rightColumnVisibility; }
            set { _rightColumnVisibility=value; OnPropertyChanged(); }
        }


        public MaciGameModel Model
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
        public DelegateCommand FourGameCommand { get; private set; }
        public DelegateCommand SixGameCommand { get; private set; }
        public DelegateCommand EightGameCommand { get; private set; }
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
        public ObservableCollection<MaciField> Fields { get; set; }

        public string PlayerTime { get { return TimeSpan.FromSeconds(_model.Table.GameTime).ToString("g"); } }


        public string Path { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler? NewGame;

        public event EventHandler? FourGame;
        public event EventHandler? SixGame;
        public event EventHandler? EightGame;
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
        public MaciViewModel(MaciGameModel model)
        {
            // játék csatlakoztatása
            _model = model;
            FourGameCommand = new DelegateCommand(param => OnFourGame());
            SixGameCommand = new DelegateCommand(param => OnSixGame());
            EightGameCommand = new DelegateCommand(param => OnEightGame());

            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            PauseStartGameCommand = new DelegateCommand(param => PauseStartGame());
            KeyCommand = new(param => OnKeyCommand(param?.ToString() ?? String.Empty));
            //_model.GameOver += Model_GameOver;
            Path="";


            Fields = new ObservableCollection<MaciField>();

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
            //{
                foreach (MaciField field in Fields)
                {
                    if (_model.Table.GetTableValue(field.X, field.Y) == 0)
                    {
                        field.Colour = "White"; OnPropertyChanged("Colour");
                    }
                    if (_model.Table.GetTableValue(field.X, field.Y) == 1)
                    {
                        field.Colour = "Brown"; OnPropertyChanged("Colour");
                    }
                    if (_model.Table.GetTableValue(field.X, field.Y) == 2)
                    {
                        field.Colour = "Yellow"; OnPropertyChanged("Colour");
                    }
                    if (_model.Table.GetTableValue(field.X, field.Y) == 3)
                    {
                        field.Colour = "Green"; OnPropertyChanged("Colour");
                    }
                    if (_model.Table.GetTableValue(field.X, field.Y) == 4)
                    {
                        field.Colour = "Red"; OnPropertyChanged("Colour");
                    }
                }


                OnPropertyChanged(nameof(PlayerTime));
                OnPropertyChanged(nameof(PicnicBaskets));
                PicnicBaskets=_model.Baskets;
                PicnicBasketsAcq=_model.AllBaskets-PicnicBaskets;
            //}

        }

        /// <summary>
        /// Játék léptetése eseménykiváltása.
        /// </summary>
        /// <param name="index">A lépett mező indexe.</param>
        //private void StepGame(Int32 index)
        //{
        //    MaciField field = Fields[index];

        //    //_model.Step1(field.X, field.Y);
            
        //    //field.Text = _model.Table[field.X, field.Y] == -1 ? _model.Table[field.X, field.Y].ToString() : "X"; // visszaírjuk a szöveget
        //    //OnPropertyChanged(nameof(_model.Table.GetPlayer)); // jelezzük a lépésszám változást

        //    //field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
        //    RefreshTable();
        //}


        public void LoadCreateTable()
        {
            for (int i = 0; i < _model.GetSize; i++)
            {
                for (int j = 0; j < _model.GetSize; j++)
                {
                    Fields.Add(new MaciField()
                    {
                        IsLocked = false,
                        //Text = String.Empty,
                        Size = this.Size,
                        X = i,
                        Y = j,
                        Number = i * Size + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                        Colour = "White",
                        //StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });

                }
            }
            RightColumnVisibility = "Visible";
            OnPropertyChanged(nameof(RightColumnVisibility));
            OnPropertyChanged(nameof(Size));
            _model.GameAdvanced += Model_GameAdvanced;
            _model.GameAdvanced += new EventHandler<MaciEventArgs>(Model_GameAdvanced);
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

        private void Model_GameAdvanced(object? sender, MaciEventArgs e)
        {
            RefreshTable();
            OnPropertyChanged(nameof(PlayerTime));
        }

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object? sender, MaciEventArgs e)
        {
            foreach (MaciField field in Fields)
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

        private void OnFourGame()
        {
            Fields.Clear();
            FourGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSixGame()
        {
            Fields.Clear();
            SixGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnEightGame()
        {
            Fields.Clear();
            EightGame?.Invoke(this, EventArgs.Empty);
        }
       

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
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
