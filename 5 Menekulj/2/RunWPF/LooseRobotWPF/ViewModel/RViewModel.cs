using RunModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static RunModel.Model.GameModel;
using static System.Net.Mime.MediaTypeNames;

namespace RunWPF.ViewModel
{
    public class RViewModel : ViewModelBase
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
        private bool _enableButtons = false;

        #endregion

        #region Properties

        public int Size { get { return _model.GetSize; } }

        public bool EnablePause { get; set; }

        public bool EnableSave { get; set; }
        public bool EnableLoad { get; set; }

        public bool EnableButtons
        { 
            get { return _enableButtons; }
            set 
            { 
                _enableButtons = value;
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
        public DelegateCommand SevenCommand { get; private set; }
        public DelegateCommand ElevenCommand { get; private set; }
        public DelegateCommand FifteenCommand { get; private set; }
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

        public event EventHandler? SevenGame;
        public event EventHandler? ElevenGame;
        public event EventHandler? FifteenGame;
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
        public RViewModel(GameModel model)
        {
            // játék csatlakoztatása
            _model = model;
            SevenCommand = new DelegateCommand(param => OnSeven());
            ElevenCommand = new DelegateCommand(param => OnEleven());
            FifteenCommand = new DelegateCommand(param => OnFifteen());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            PauseStartGameCommand = new DelegateCommand(param => PauseStartGame());
            KeyCommand = new(param => OnKeyCommand(param?.ToString() ?? String.Empty));
            //_model.GameOver += Model_GameOver;
            Path="";
            EnableButtons = true;
            EnablePause = false;
            EnableSave = false;
            EnableLoad = true;

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
                //_model.Field[field.X, field.Y] = FieldValue.WasWall;
                //int i = 0;
                if (_model.Table.GetTableValue(field.X, field.Y) == 0)
                {
                    field.Colour = "White"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 1)
                {
                    field.Colour = "Blue"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 2)
                {
                    field.Colour = "Red"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 3)
                {
                    field.Colour = "Black"; OnPropertyChanged("Colour");
                }

                //field.Text = _model.Table.GetTableValue(field.X, field.Y).ToString();
            }

            //OnPropertyChanged(nameof(PlayerTime)); Itt nem biztos, h kell, mert az AdvancedTime frissíti
            //OnPropertyChanged(nameof(Text));            
        }

        private void ReLoadTable()
        {
            //if (!_model.IsGameOver)

            foreach (Field field in Fields)
            {
                //_model.Field[field.X, field.Y] = FieldValue.WasWall;
                //int i = 0;
                if (_model.Table.GetTableValue(field.X, field.Y) == 0)
                {
                    field.Colour = "White"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 1)
                {
                    field.Colour = "Blue"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 2)
                {
                    field.Colour = "Red"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 3)
                {
                    field.Colour = "Black"; OnPropertyChanged("Colour");
                }
                
                //field.Text = _model.Table.GetTableValue(field.X, field.Y).ToString();
            }


            //OnPropertyChanged(nameof(PlayerTime)); Itt nem biztos, h kell, mert az AdvancedTime frissíti
            //OnPropertyChanged(nameof(Text));            
        }

        public void CreateTable()
        {
            for (int i = 0; i < _model.GetSize; i++)
            {
                for (int j = 0; j < _model.GetSize; j++)
                {
                    Fields.Add(new Field()
                    {
                        IsLocked = false,
                        //Text = _model.Table.GetTableValue(i,j).ToString(),
                        Text = String.Empty,
                        Size = this.Size,
                        X = i,
                        Y = j,
                        Number = i * Size + j, // a gsomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged(nameof(Size));
            //OnPropertyChanged(nameof(Text));
            _model.GenerateFields();
            _model.GameOver += Model_GameOver;
            _model.GameAdvanced += Model_GameAdvanced;
            StartPause=true;
            EnableButtons = true;
            EnablePause = true;
            EnableSave = false;
            EnableLoad = false;
            OnPropertyChanged(nameof(EnableSave));
            OnPropertyChanged(nameof(EnableLoad));
            OnPropertyChanged(nameof(EnablePause));
            RefreshTable();
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
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });

                }
            }

            StartPause=true;
            EnableButtons = true;
            EnablePause = true;
            EnableSave = false;
            EnableLoad = false;
            OnPropertyChanged(nameof(EnableSave));
            OnPropertyChanged(nameof(EnableLoad));
            OnPropertyChanged(nameof(EnablePause));

            RightColumnVisibility = "Visible";
            OnPropertyChanged(nameof(RightColumnVisibility));
            OnPropertyChanged(nameof(Size));
            //_model.GameOver += Model_GameOver;
            _model.GameAdvanced += Model_GameAdvanced;
            //_model.GameAdvanced += new EventHandler<RunEventArgs>(Model_GameAdvanced); // GPT szte u.az (TODO ezt megértani, minek van/volt)


            ReLoadTable();
            _model.GameOver += Model_GameOver;
            PauseStart?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Game event handlers

        /// <summary>
        /// Játék léptetése eseménykiváltása.
        /// </summary>
        /// <param name="index">A lépett mező indexe.</param>
        private void StepGame(Int32 index)
        {
            if (EnableButtons)
            {
                Field field = Fields[index];

                //_model.PlaceWall(field.X, field.Y);

                RefreshTable();
            }
            
        }

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
                    case "s":
                        _model.MovePlayer(x+1, y);
                        break;
                    case "a":
                        _model.MovePlayer(x, y-1);
                        break;
                    case "d":
                        _model.MovePlayer(x, y+1);
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
                EnableSave = true;
                EnableLoad = true;
            }
            else
            {
                StartPause = true;
                EnableButtons = true;
                EnableSave = false;
                EnableLoad = false;
            }
            PauseStart?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(PlayerTime));
            OnPropertyChanged(nameof(EnableSave));
            OnPropertyChanged(nameof(EnableLoad));
        }

        private void Model_GameAdvanced(object? sender, RunEventArgs e)
        {
            RefreshTable();
            OnPropertyChanged(nameof(PlayerTime));
        }

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object? sender, RunEventArgs e)
        {
            EnableLoad = true;
            OnPropertyChanged(nameof(EnableLoad));
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

        private void OnSeven()
        {
            Fields.Clear();
            SevenGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnEleven()
        {
            Fields.Clear();
            ElevenGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnFifteen()
        {
            Fields.Clear();
            FifteenGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            EnableLoad = true;
            OnPropertyChanged(nameof(EnableLoad));
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private async void OnLoadGame()
        {
            _model.GameOver -= Model_GameOver;
            Fields.Clear();
            LoadGame?.Invoke(this, EventArgs.Empty);
            await _model.LoadGameAsync(Path);
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
