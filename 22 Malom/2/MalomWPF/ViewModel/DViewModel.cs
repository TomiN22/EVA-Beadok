using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MalomModel.Model;
using static System.Net.Mime.MediaTypeNames;

namespace MalomWPF.ViewModel
{
    public class DViewModel : ViewModelBase
    {
        #region Fields

        private GameModel _model; // modell
        public int _size;
        private int _player;
        private bool _enableButtons = true;
        public bool _startPause;
        private String _rightColumnVisibility = "Collapsed";
        private BitmapImage _greenImage = new BitmapImage(new Uri("./pic/greencircle.PNG", UriKind.Relative));
        private BitmapImage _gGreenImage = new BitmapImage(new Uri("./pic/greygreencircle.PNG", UriKind.Relative));
        private BitmapImage _redImage = new BitmapImage(new Uri("./pic/redcircle.PNG", UriKind.Relative));
        private BitmapImage _gRedImage = new BitmapImage(new Uri("./pic/greyredcircle.PNG", UriKind.Relative));
        private int _pTurnNumber;
        private string _pTurnColor;

        #endregion

        #region Properties

        public int Size { get { return _model.Table.GetSize; } }
        public GameModel Model 
        { 
            get { return _model; }
            set
            {
                _model = value;
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

        public bool EnableButtons
        {
            get { return _enableButtons; }
            set
            {
                _enableButtons = value;
                OnPropertyChanged();
            }
        }

        public int Points {  get; set; }

        public bool EnablePause { get; set; }
        public bool EnableSave { get; set; }
        public bool EnableLoad { get; set; }

        public bool WasPausedBySystem {  get; set; }

        public string StatusBarVisibility {  get; set; }

        public int P1Placedown {  get; set; }
        public int P2Placedown { get; set; }

        public int PTurnNumber { get { return _pTurnNumber; } set { _pTurnNumber = value; OnPropertyChanged(); } }
        public string PTurnColor { get { return _pTurnColor; } set { _pTurnColor = value; OnPropertyChanged(); } }

        public String RightColumnVisibility
        {
            get { return _rightColumnVisibility; }
            set { _rightColumnVisibility=value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand ThreeGameCommand { get; private set; }
        public DelegateCommand FiveGameCommand { get; private set; }
        public DelegateCommand SevenGameCommand { get; private set; }

        public DelegateCommand KeyCommand { get; private set; }

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

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<Field> Fields { get; set; }

        public string P1Time { get { return TimeSpan.FromSeconds(_model.Table.P1Time).ToString("g"); } }
        public string P2Time { get { return TimeSpan.FromSeconds(_model.Table.P2Time).ToString("g"); } }

        public string Path { get ; set; } 

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler? NewGame;
        public event EventHandler? ThreeGame;
        public event EventHandler? FiveGame;
        public event EventHandler? SevenGame;

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
        /// Sudoku nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public DViewModel(GameModel model)
        {
            _model = model;
            NewGameCommand = new DelegateCommand(param => OnNewGame());

            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            //KeyCommand = new(param => OnKeyCommand(param?.ToString() ?? String.Empty));
            //_model.GameOver += Model_GameOver;
            Path="";
            EnableButtons = true;
            EnablePause = false;
            EnableSave = false;
            EnableLoad = true;
            StatusBarVisibility = "Collapsed";
            //Points = _model.Table.Points;
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
                    field.Colour = "gray"; OnPropertyChanged("Colour");
                    field.Image = null;
                }

                if (_model.Table.GetTableValue(field.X, field.Y) == 1)
                {
                    field.Image = _greenImage;
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 2)
                {
                    field.Image = _redImage;
                }
                if(_model.Table.GetTableColor(field.X, field.Y) == 1)
                {
                    field.Colour = "White"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableColor(field.X, field.Y) == 2)
                {
                    field.Colour = "Black"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableColor(field.X, field.Y) == 3)
                {
                    field.Colour = "Gray"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableColor(field.X, field.Y) == 4)
                {
                    field.Colour = "Red"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableColor(field.X, field.Y) == 5)
                {
                    field.Colour = "Blue"; OnPropertyChanged("Colour");
                }

                //field.Text = _model.Table.GetTableValue(field.X,field.Y).ToString();
            }
            P1Placedown = _model.Table.P1PlaceDown;
            P2Placedown = _model.Table.P2PlaceDown;

            OnPropertyChanged(nameof(P1Placedown));
            OnPropertyChanged(nameof(P2Placedown));
            OnPropertyChanged(nameof(PTurnNumber));
            OnPropertyChanged(nameof(PTurnColor));
            //_model.Bombs.ForEach(b => Fields[b.PositionY + b.PositionX * Size].Text = b.Time.ToString());
            OnPropertyChanged(nameof(P1Time));
            OnPropertyChanged(nameof(P2Time));
            //OnPropertyChanged(nameof(Text));
        }

        public void CreateTable()
        {
            for (int i = 0; i < _model.Table.GetSize; i++)
            {
                for (int j = 0; j < _model.Table.GetSize; j++)
                {
                    Fields.Add(new Field()
                    {
                        IsLocked = false,
                        Text = String.Empty,
                        Size = this.Size,
                        X = i,
                        Y = j,
                        Number = i * Size + j, // a gsomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            StartPause =true;
            EnableButtons = true;
            EnablePause = true;
            EnableSave = true;
            EnableLoad = true;
            StatusBarVisibility = "Visible";
            OnPropertyChanged(nameof(StatusBarVisibility));
            OnPropertyChanged(nameof(EnableSave));
            OnPropertyChanged(nameof(EnableLoad));
            OnPropertyChanged(nameof(EnablePause));
            OnPropertyChanged(nameof(Size));
            _model.GenerateFields();
            _model.GameOver += Model_GameOver;
            _model.GameAdvanced += Model_GameAdvanced;
            RefreshTable();
        }

        //public void SetUp()
        //{
        //    if (_model.GetSize == 4)
        //        OnTwoGame();
        //    if (_model.GetSize == 6)
        //        OnSixGame();
        //    if (_model.GetSize == 8)
        //        OnEightGame();
        //}

        public void LoadCreateTable()
        {
            for (int i = 0; i < _model.Table.GetSize; i++)
            {
                for (int j = 0; j < _model.Table.GetSize; j++)
                {
                    Fields.Add(new Field()
                    {
                        IsLocked = false,
                        Text = String.Empty,
                        Size = this.Size,
                        X = i,
                        Y = j,
                        Number = i * Size + j, // a gsomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            _model.SetUpTable();
            StartPause=false;
            EnableButtons = true;
            EnablePause = true;
            EnableSave = true;
            EnableLoad = true;
            StatusBarVisibility = "Visible";
            OnPropertyChanged(nameof(StatusBarVisibility));
            OnPropertyChanged(nameof(EnableSave));
            OnPropertyChanged(nameof(EnableLoad));
            OnPropertyChanged(nameof(EnablePause));
            OnPropertyChanged(nameof(Size));
            _model.GameOver += Model_GameOver;
            _model.GameAdvanced += Model_GameAdvanced;
            RefreshTable();
        }

        /// <summary>
        /// Játék léptetése eseménykiváltása.
        /// </summary>
        /// <param name="index">A lépett mező indexe.</param>
        private void StepGame(Int32 index)
        {
            Field field = Fields[index];

            _model.Step(field.X, field.Y);

            //field.Text = _model.Table[field.X, field.Y] == -1 ? _model.Table[field.X, field.Y].ToString() : "X"; // visszaírjuk a szöveget
            //field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
            RefreshTable();
        }
        #endregion

        #region Game event handlers

        public void PauseStartGame()
        {
            if (StartPause || _model.IsGameOver)
            {
                StartPause = false;
                EnableButtons = false;
                EnableSave = true;
                //StatusBarVisibility = "Visible";
                EnableLoad = true;
            }
            else
            {
                StartPause = true;
                EnableButtons = true;
                EnableSave = false;
                //StatusBarVisibility = "Collapsed";
                EnableLoad = false;
            }
            PauseStart?.Invoke(this, EventArgs.Empty);
            //OnPropertyChanged(nameof(StatusBarVisibility));
            OnPropertyChanged(nameof(EnableSave));
            OnPropertyChanged(nameof(EnableLoad));
            OnPropertyChanged(nameof(P1Time));
            OnPropertyChanged(nameof(P2Time));
        }

        public void OnAppDeactivated()
        {
            if (StartPause) // Csak akkor, ha a játék nem volt már szünetelve
            {
                WasPausedBySystem = true;
                PauseStartGame();
            }
        }

        public void OnAppActivated()
        {
            if (WasPausedBySystem)
            {
                WasPausedBySystem = false;
                PauseStartGame(); // Visszatéréskor csak akkor folytatjuk, ha a rendszer miatt állt meg
            }
        }

        //public async Task SaveScores(string path)
        //{
        //    await _model.SaveHighScoresAsync(path);
        //}

        //public async Task LoadScores(string path)
        //{
        //    await _model.LoadHighScoresAsync(path);
        //    HighScores = new ObservableCollection<int>(_model.HighScores);
        //}

        private void Model_GameAdvanced(object? sender, MalomEventArgs e)
        {
            //RefreshTable();
            OnPropertyChanged(nameof(P1Time));
            OnPropertyChanged(nameof(P2Time));
        }

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object? sender, MalomEventArgs e)
        {
            foreach (Field field in Fields)
            {
                field.IsLocked = true; // minden mezőt lezárunk
                field.IsEnabled = false;
                //_model.Step(field.X, field.Y);
            }
            RightColumnVisibility="Collapsed";
            EnableSave = false;

            //_model.AddScore(Points); // Pontszám hozzáadása
            //SaveScores("./save/records.txt"); // Pontszámok mentése
        }

        /// <summary>
        /// Játék létrehozásának eseménykezelője.
        /// </summary>
        private void Model_GameCreated(object? sender, MalomEventArgs e)
        {
            RefreshTable();
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnNewGame()
        {
            Fields.Clear();
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private async void OnLoadGame()
        {
            Fields.Clear();
            LoadGame?.Invoke(this, EventArgs.Empty);
            await _model.LoadGameAsync(Path);
            LoadCreateTable();
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
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
