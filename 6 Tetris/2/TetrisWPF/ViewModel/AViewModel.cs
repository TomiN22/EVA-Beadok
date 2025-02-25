using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisModel.Model;
using TetrisModel.Persistence;
using static System.Net.Mime.MediaTypeNames;

namespace TetrisWPF.ViewModel
{
    public class AViewModel : ViewModelBase
    {
        #region Fields

        private GameModel _model; // modell
        public int _size;
        private int _player;
        private bool _enableButtons = true;
        public bool _startPause;
        private String _rightColumnVisibility = "Collapsed";

        #endregion

        #region Properties

        public int SizeX { get { return _model.GetSizeX; } }
        public int SizeY { get { return _model.GetSizeY; } }
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

        public bool EnablePause { get; set; }
        public bool EnableSave { get; set; }
        public bool EnableLoad { get; set; }

        public String RightColumnVisibility
        {
            get { return _rightColumnVisibility; }
            set { _rightColumnVisibility=value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand Lvl1GameCommand { get; private set; }
        public DelegateCommand Lvl2GameCommand { get; private set; }
        public DelegateCommand Lvl3GameCommand { get; private set; }

        public DelegateCommand PauseStartGameCommand { get; private set; }

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

        public string GameTime { get { return TimeSpan.FromSeconds(_model.Table.GameTime).ToString("g"); } }
        public string StdGameTime { get { return TimeSpan.FromSeconds(_model.Table.StdTime).ToString("g"); } }
        public string FastestGameTime { get { return TimeSpan.FromSeconds(_model.Table.FastTime).ToString("g"); } }
        public string Path { get ; set; } 

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
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
        /// Sudoku nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public AViewModel(GameModel model)
        {
            _model = model;
            Lvl1GameCommand = new DelegateCommand(param => OnLvl1Game());
            Lvl2GameCommand = new DelegateCommand(param => OnLvl2Game());
            Lvl3GameCommand = new DelegateCommand(param => OnLvl3Game());

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
            OnPropertyChanged(nameof(EnablePause));
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
                if (_model.Table.GetTableValue(field.X, field.Y) == 2)
                {
                    field.Colour = "Green"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 1)
                {
                    field.Colour = "Blue"; OnPropertyChanged("Colour");
                }
                if(_model.Table.GetTableValue(field.X, field.Y) == 3)
                {
                    field.Colour = "Black"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 4)
                {
                    field.Colour = "Yellow"; OnPropertyChanged("Colour");
                }
                if (_model.Table.GetTableValue(field.X, field.Y) == 5)
                {
                    field.Colour = "Red"; OnPropertyChanged("Colour");
                }

                field.Text = String.Empty;
            }

            //_model.Bombs.ForEach(b => Fields[b.PositionY + b.PositionX * Size].Text = b.Time.ToString());
            OnPropertyChanged(nameof(GameTime));
            //OnPropertyChanged(nameof(StdGameTime));

        }

        public void CreateTable()
        {
            for (int i = 0; i < _model.GetSizeX; i++)
            {
                for (int j = 0; j < _model.GetSizeY; j++)
                {
                    Fields.Add(new Field()
                    {
                        IsLocked = false,
                        Text = String.Empty,
                        SizeX = this.SizeX,
                        SizeY = this.SizeY,
                        X = i,
                        Y = j,
                        Number = i * SizeX + j, // a gsomb sorszáma, amelyet felhasználunk az azonosításhoz
                        //StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
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
            OnPropertyChanged(nameof(SizeX));
            OnPropertyChanged(nameof(SizeY));
            //_model.GenerateFields();
            _model.GameOver += Model_GameOver;
            _model.GameAdvanced += Model_GameAdvanced;
            _model.STDGameAdvanced += Model_STDGameAdvanced;
            _model.FastGameAdvanced += Model_FastestGameAdvanced;
            RefreshTable();
        }

        public void LoadCreateTable()
        {
            for (int i = 0; i < _model.GetSizeX; i++)
            {
                for (int j = 0; j < _model.GetSizeY; j++)
                {
                    Fields.Add(new Field()
                    {
                        IsLocked = false,
                        Text = String.Empty,
                        SizeX = this.SizeX,
                        SizeY = this.SizeY,
                        X = i,
                        Y = j,
                        Number = i * SizeX + j, // a gsomb sorszáma, amelyet felhasználunk az azonosításhoz
                        //StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
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
            OnPropertyChanged(nameof(StdGameTime));
            OnPropertyChanged(nameof(FastestGameTime));
            OnPropertyChanged(nameof(SizeX));
            OnPropertyChanged(nameof(SizeY));
            RefreshTable();
            _model.GameOver += Model_GameOver;
            _model.GameAdvanced += Model_GameAdvanced;
            _model.STDGameAdvanced += Model_STDGameAdvanced;
            _model.FastGameAdvanced += Model_FastestGameAdvanced;
            PauseStart?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Game event handlers

        private void OnKeyCommand(string key)
        {
            if (EnableButtons && _model.ShapeList.Any())
            {
                var activeShape = _model.ShapeList.Last();

                switch (key)
                {
                    case "w":
                        _model.RotateShape(activeShape);
                        break;
                    case "a":
                        _model.MoveShape(activeShape, 0, -1);
                        break;
                    case "d":
                        _model.MoveShape(activeShape, 0, 1);
                        break;
                    case "s":
                        _model.MoveShape(activeShape, 1, 0);
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
            OnPropertyChanged(nameof(EnableSave));
            OnPropertyChanged(nameof(EnableLoad));
            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(StdGameTime));
            OnPropertyChanged(nameof(FastestGameTime));
        }

        private void Model_FastestGameAdvanced(object? sender, TetrisEventArgs e)
        {
            OnPropertyChanged(nameof(FastestGameTime));
            RefreshTable();
        }

        private void Model_STDGameAdvanced(object? sender, TetrisEventArgs e)
        {
            RefreshTable();
            OnPropertyChanged(nameof(StdGameTime));
        }

        private void Model_GameAdvanced(object? sender, TetrisEventArgs e)
        {
            RefreshTable();
            OnPropertyChanged(nameof(GameTime));
            //OnPropertyChanged(nameof(StdGameTime));
        }

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object? sender, TetrisEventArgs e)
        {
            EnablePause = false;
            EnableSave = false;
            EnableLoad = true;
            OnPropertyChanged(nameof(EnablePause));
            OnPropertyChanged(nameof(EnableSave));
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
        /// Játék létrehozásának eseménykezelője.
        /// </summary>
        private void Model_GameCreated(object? sender, TetrisEventArgs e)
        {
            RefreshTable();
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnLvl1Game()
        {
            Fields.Clear();
            Lvl1Game?.Invoke(this, EventArgs.Empty);
        }

        private void OnLvl2Game()
        {
            Fields.Clear();
            Lvl2Game?.Invoke(this, EventArgs.Empty);
        }

        private void OnLvl3Game()
        {
            Fields.Clear();
            Lvl3Game?.Invoke(this, EventArgs.Empty);
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
