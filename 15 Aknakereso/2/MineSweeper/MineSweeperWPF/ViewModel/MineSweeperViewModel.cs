using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace MineSweeperWPF.ViewModel
{
    public class MineSweeperViewModel : ViewModelBase
    {
        #region Fields

        private MineModel _model; // modell
        public int _size;
        private int _player;

        #endregion

        #region Properties

        public int Size { get { return _model.GetSize; } }
        public MineModel Model 
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
        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand SixGameCommand { get; private set; }
        public DelegateCommand TenGameCommand { get; private set; }
        public DelegateCommand SixteenGameCommand { get; private set; }
        public DelegateCommand IsLockedCommand { get; private set; } 

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
        public ObservableCollection<MineSweeperField> Fields { get; set; }

        /// <summary>
        /// Lépések számának lekérdezése.
        /// </summary>
        public Int32 GameStepCount 
        { 
            get
            { return _model.GameStepCount; } 
        }
        public string Player
        {
            get
            { return "Player"+_player; }
        } 

        public string Path { get ; set; } 

        

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler? NewGame;

        public event EventHandler? SixGame;
        public event EventHandler? TenGame;
        public event EventHandler? SixteenGame;
        public event EventHandler? IsLocked;

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
        public MineSweeperViewModel(MineModel model)
        {
            // játék csatlakoztatása
            _model = model;
            //_model.GameAdvanced += new EventHandler<MineEventArgs>(Model_GameAdvanced);
            //_model.GameCreated += new EventHandler<MineEventArgs>(Model_GameCreated);

            // parancsok kezelése
            //NewGameCommand = new DelegateCommand(param => OnNewGame());
            SixGameCommand = new DelegateCommand(param => OnSixGame());
            TenGameCommand = new DelegateCommand(param => OnTenGame());
            SixteenGameCommand = new DelegateCommand(param => OnSixteenGame());
            IsLockedCommand = new DelegateCommand(param => OnIsLocked());

            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            _player = 1;

            // játéktábla létrehozása
            Fields = new ObservableCollection<MineSweeperField>();
            /*for (Int32 i = 0; i < Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < Size; j++)
                {
                    Fields.Add(new MineSweeperField
                    {
                        IsLocked = true,
                        Text = String.Empty,
                        Size = this.Size,
                        X = i,
                        Y = j,
                        Number = i * Size + j, // a gsomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                        // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                    });
                }
            }*/
            RefreshTable();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Tábla frissítése.
        /// </summary>
        private void RefreshTable()
        {
            foreach (MineSweeperField field in Fields) // inicializálni kell a mezőket is
            {
                if (_model.Table.IsOpened(field.X, field.Y))
                {
                    //field.Text = _model.Table[field.X, field.Y].ToString();
                    if (_model.Table[field.X, field.Y] == -1)
                    {
                        field.Text = "X";
                        field.IsLocked = true;
                    }
                    else if (_model.Table[field.X, field.Y] != -1 && _model.Table[field.X, field.Y] != 0)
                    {
                        field.Text = _model.Table[field.X, field.Y].ToString();
                        field.IsLocked = true;
                    }
                    else
                    {
                        field.Text = " ";
                        field.IsLocked = true;
                    }
                }
                _player = _model.Table.GetPlayer;
                //field.IsLocked = _model.Table.IsLocked(field.X, field.Y);

            }

            //OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(Player));
            
        }

        /// <summary>
        /// Játék léptetése eseménykiváltása.
        /// </summary>
        /// <param name="index">A lépett mező indexe.</param>
        private void StepGame(Int32 index)
        {
            MineSweeperField field = Fields[index];

            _model.Step(field.X, field.Y);
            _player = _model.Table.GetPlayer;

            if (!_model.Table.IsOpened(field.X, field.Y))
            {
                _model.ShowAll(field.X, field.Y);
            }

            foreach (var field2 in Fields)
            {
                if(_model.Table.IsOpened(field2.X, field2.Y))
                {
                    field2.IsLocked = true;
                }
            }
            

            //field.Text = _model.Table[field.X, field.Y] == -1 ? _model.Table[field.X, field.Y].ToString() : "X"; // visszaírjuk a szöveget
            OnPropertyChanged(nameof(Player)); // jelezzük a lépésszám változást

            //field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
            RefreshTable();
        }

        public void CreateTable()
        {
            for (int i = 0; i < _model.GetSize; i++)
            {
                for (int j = 0; j < _model.GetSize; j++)
                {
                    Fields.Add(new MineSweeperField()
                    {
                        IsLocked = false,
                        Text = _model.Table.GetValue(i,j).ToString(),
                        Size = this.Size,
                        X = i,
                        Y = j,
                        Number = i * Size + j, // a gsomb sorszáma, amelyet felhasználunk az azonosításhoz
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged(nameof(Size));
            RefreshTable();
            _model.GenerateFields();
            _model.GameOver += Model_GameOver;
        }

        public void LoadCreateTable()
        {
            for (int i = 0; i < _model.GetSize; i++)
            {
                for (int j = 0; j < _model.GetSize; j++)
                {
                    Fields.Add(new MineSweeperField()
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
            OnPropertyChanged(nameof(Size));
            RefreshTable();
            _model.GameOver += Model_GameOver;
        }
        #endregion

        #region Game event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object? sender, MineEventArgs e)
        {
            foreach (MineSweeperField field in Fields)
            {
                field.IsLocked = true; // minden mezőt lezárunk
                field.IsEnabled = false;
                //_model.Step(field.X, field.Y);
            }
        }

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        /*private void Model_GameAdvanced(object? sender, MineEventArgs e)
        {
            OnPropertyChanged(nameof(GameTime));
        }*/

        /// <summary>
        /// Játék létrehozásának eseménykezelője.
        /// </summary>
        private void Model_GameCreated(object? sender, MineEventArgs e)
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
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSixGame()
        {
            Fields.Clear();
            SixGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnTenGame()
        {
            Fields.Clear();
            TenGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSixteenGame()
        {
            Fields.Clear();
            SixteenGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnIsLocked()
        {
            IsLocked?.Invoke(this, EventArgs.Empty);
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
