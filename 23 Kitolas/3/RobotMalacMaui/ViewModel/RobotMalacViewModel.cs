using RobotmalacModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotMalacMaui.ViewModel
{
    public class RobotMalacViewModel : ViewModelBase
    {
        #region Fields

        private RobotModel _model;
        public int _size;
        public int _progressBar1Value = 100;
        public int _progressBar2Value = 100;
        public int _p1Hitpoints = 3;
        public int _p2Hitpoints = 3;
        private String _playerTurnText = "Player 1 on turn";
        private TableSizeViewModel? _tableSizeViewModel;
        //private BitmapImage _playerTurnImage = new BitmapImage(new Uri("./pic/Peppa_malac_szemcsi_fekete.png", UriKind.Relative));
        private ImageSource _playerTurnImage = new FileImageSource { File = "peppa_malac_szemcsi_fekete.png" };

        private String _textCommand = "";
        private bool _rightColumnVisibility = false;

        #endregion

        #region Properties

        public int Size { get { return _size; } }

        public ImageSource PlayerTurnImage
        {
            get { return _playerTurnImage; }
            set
            {
                _playerTurnImage = value;
                OnPropertyChanged();
            }
        }

        public String PlayerTurnText
        {
            get { return _playerTurnText; }
            set
            {
                _playerTurnText = value;
                OnPropertyChanged();
            }
        }
        public String TextCommand
        {
            get { return _textCommand; }
            set
            {
                _textCommand = value;
                OnPropertyChanged();
            }
        }
        public bool RightColumnVisibility
        {
            get { return _rightColumnVisibility; }
            set { _rightColumnVisibility=value; OnPropertyChanged(); }
        }
        public int ProgressBar1Value
        {
            get { return _progressBar1Value; }
            set { _progressBar1Value = value; OnPropertyChanged(); }
        }
        public int ProgressBar2Value
        {
            get { return _progressBar2Value; }
            set { _progressBar2Value = value; OnPropertyChanged(); }
        }
        public int P1Hitpoints
        {
            get { return _p1Hitpoints; }
            set { _p1Hitpoints = value; OnPropertyChanged(); }
        }
        public int P2Hitpoints
        {
            get { return _p2Hitpoints; }
            set { _p2Hitpoints = value; OnPropertyChanged(); }
        }


        public RobotModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
            }
        }

        public TableSizeViewModel? TableSizeViewModel
        {
            get => _tableSizeViewModel;
            set
            {
                _tableSizeViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand FourGameCommand { get; private set; }
        public DelegateCommand SixGameCommand { get; private set; }
        public DelegateCommand EightGameCommand { get; private set; }
        public DelegateCommand OkClickedCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// How to play parancs lekérdezése.
        /// </summary>
        public DelegateCommand HowToCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<RobotMalacField> Fields { get; set; }

        public ObservableCollection<TableSizeViewModel> TableSizes
        {
            get;
            set;
        }
        public RowDefinitionCollection RowSize
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Auto), _model.Table.GetSize).ToArray());
        }

        public ColumnDefinitionCollection ColSize
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Auto), _model.Table.GetSize).ToArray());
        }


        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler<int>? NewGame;

        public event EventHandler? FourGame;
        public event EventHandler? SixGame;
        public event EventHandler? EightGame;
        public event EventHandler? OkClicked;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler? LoadGame;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler? SaveGame;

        /// <summary>
        /// How to play eseménye.
        /// </summary>
        public event EventHandler? HowToGame;

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
        public RobotMalacViewModel(RobotModel model)
        {
            // játék csatlakoztatása
            _model = model;
            _size=_model.GetSize;
            _model.GameOver += new EventHandler<RobotEventArgs>(Model_GameOver);


            NewGameCommand = new DelegateCommand(param => OnNewGame(Int32.Parse(param!.ToString()!)));
            FourGameCommand = new DelegateCommand(param => OnFourGame());
            SixGameCommand = new DelegateCommand(param => OnSixGame());
            EightGameCommand = new DelegateCommand(param => OnEightGame());
            HowToCommand = new DelegateCommand(ParamArrayAttribute => OnHowToGame());
            OkClickedCommand = new DelegateCommand(param => OnOkClicked());

            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            TableSizes = new ObservableCollection<TableSizeViewModel>
            {
                new TableSizeViewModel { TableSize = 4 },
                new TableSizeViewModel { TableSize = 6 },
                new TableSizeViewModel { TableSize = 8 }
            };
            TableSizeViewModel = TableSizes[1];

            Fields = new ObservableCollection<RobotMalacField>();



            RefreshTable();
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Tábla frissítése.
        /// </summary>
        private void RefreshTable()
        {
            MoveToPosition();
            ChangeHealth();
        }

        private void ReloadTable()
        {
            for (int i = 0; i<Fields.Count(); ++i)
            {
                Fields[i].PImage = new FileImageSource { File = "" };
            }

            for (int i = 0; i<Fields.Count(); ++i)
            {
                if (Fields[i].X == _model.Table.P1Current[1] && Fields[i].Y == _model.Table.P1Current[0])
                {
                    switch (_model.Table.P1Direction)
                    {
                        case "up": Fields[i].PImage = new FileImageSource { File = "peppa_fekete_fel.png" }; break;
                        case "down": Fields[i].PImage = new FileImageSource { File = "peppa_fekete_le.png" }; break;
                        case "right": Fields[i].PImage = new FileImageSource { File = "peppa_fekete_jobb.png" }; break;
                        case "left": Fields[i].PImage = new FileImageSource { File = "peppa_malac_szemcsi_fekete.png" }; break;
                    }
                }
                if (Fields[i].X == _model.Table.P2Current[1] && Fields[i].Y == _model.Table.P2Current[0])
                {
                    switch (_model.Table.P2Direction)
                    {
                        case "up": Fields[i].PImage = new FileImageSource { File = "peppa_piros_fel.png" }; break;
                        case "down": Fields[i].PImage = new FileImageSource { File = "peppa_piros_le.png" }; break;
                        case "right": Fields[i].PImage = new FileImageSource { File = "peppa_piros_jobb.png" }; break;
                        case "left": Fields[i].PImage = new FileImageSource { File = "peppa_malac_szemcsi_piros.png" }; break;
                    }
                }

            }

            ChangeHealth();
        }

        public void MoveToPosition()
        {
            if (_model.P1MoveChanged == true || _model.P2MoveChanged)
            {
                for (int i = 0; i<Fields.Count(); ++i)
                {
                    Fields[i].PImage = new FileImageSource { File = "" };
                }
            }
            for (int i = 0; i<Fields.Count(); ++i)
            {
                if (Fields[i].X == _model.Table.P1Current[1] && Fields[i].Y == _model.Table.P1Current[0])
                {
                    switch (_model.Table.P1Direction)
                    {
                        case "up": Fields[i].PImage = new FileImageSource { File = "peppa_fekete_fel.png" }; break;
                        case "down": Fields[i].PImage = new FileImageSource { File = "peppa_fekete_le.png" }; break;
                        case "right": Fields[i].PImage = new FileImageSource { File = "peppa_fekete_jobb.png" }; break;
                        case "left": Fields[i].PImage = new FileImageSource { File = "peppa_malac_szemcsi_fekete.png" }; break;
                    }
                }
                if (Fields[i].X == _model.Table.P2Current[1] && Fields[i].Y == _model.Table.P2Current[0])
                {
                    switch (_model.Table.P2Direction)
                    {
                        case "up": Fields[i].PImage = new FileImageSource { File = "peppa_piros_fel.png" }; break;
                        case "down": Fields[i].PImage = new FileImageSource { File = "peppa_piros_le.png" }; break;
                        case "right": Fields[i].PImage = new FileImageSource { File = "peppa_piros_jobb.png" }; break;
                        case "left": Fields[i].PImage = new FileImageSource { File = "peppa_malac_szemcsi_piros.png" }; break;
                    }
                }

            }

        }

        public void ChangeHealth()
        {
            if (_model.Table.P1Health == 3)
            {
                ProgressBar1Value = 100;
                P1Hitpoints = 3;
            }
            else if (_model.Table.P1Health == 2)
            {
                ProgressBar1Value = 66;
                P1Hitpoints = 2;
            }
            else if (_model.Table.P1Health == 1)
            {
                ProgressBar1Value = 33;
                P1Hitpoints = 1;
            }
            else
            {
                ProgressBar1Value = 0;
                P1Hitpoints = 0;
            }

            if (_model.Table.P2Health == 3)
            {
                ProgressBar2Value= 100;
                P2Hitpoints = 3;
            }
            else if (_model.Table.P2Health == 2)
            {
                ProgressBar2Value = 66;
                P2Hitpoints = 2;
            }
            else if (_model.Table.P2Health == 1)
            {
                ProgressBar2Value = 33;
                P2Hitpoints = 1;
            }
            else
            {
                ProgressBar2Value = 0;
                P2Hitpoints = 0;
            }
        }

        /// <summary>
        /// Játék léptetése eseménykiváltása.
        /// </summary>
        /// <param name="index">A lépett mező indexe.</param>
        private void StepGame(Int32 index)
        {
            RobotMalacField field = Fields[index];

            //_model.Step(field.X, field.Y);
            //_player = _model.Table.GetPlayer;

            //if (!_model.Table.IsOpened(field.X, field.Y))
            //{
            //    _model.ShowAll(field.X, field.Y);
            //}

            //foreach (var field2 in Fields)
            //{
            //    if (_model.Table.IsOpened(field2.X, field2.Y))
            //    {
            //        field2.IsLocked = true;
            //    }
            //}


            //field.Text = _model.Table[field.X, field.Y] == -1 ? _model.Table[field.X, field.Y].ToString() : "X"; // visszaírjuk a szöveget
            OnPropertyChanged(nameof(_model.Table.GetPlayer)); // jelezzük a lépésszám változást

            //field.Text = !_model.Table.IsEmpty(field.X, field.Y) ? _model.Table[field.X, field.Y].ToString() : String.Empty;
            RefreshTable();
        }

        public void RemoveTable()
        {
            Fields.Clear();
        }
        public void CreateTable()
        {
            for (int i = 0; i < _model.GetSize; i++)
            {
                for (int j = 0; j < _model.GetSize; j++)
                {
                    //_txt = i * Size + j;
                    Fields.Add(new RobotMalacField()
                    {
                        IsLocked = false,
                        Size = this.Size,
                        X = i,
                        Y = j,
                        Number = i * Size + j, // a gsomb sorszáma, amelyet felhasználunk az azonosításhoz
                        //StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                });

                    if (j == (_model.GetSize/2)-1 && i == 0)
                    {
                        Fields.Last().PImage = new FileImageSource { File = "peppa_fekete_le.png" };
                        OnPropertyChanged("PImage");
                    }
                    if (j == _model.GetSize/2 && i == _model.GetSize-1)
                    {
                        Fields.Last().PImage = new FileImageSource { File = "peppa_piros_fel.png" };
                        OnPropertyChanged("PImage");
                    }
                }
            }

            RightColumnVisibility = true;
            OnPropertyChanged(nameof(RightColumnVisibility));
            OnPropertyChanged(nameof(Size));
            RefreshTable();
            //_model.GenerateFields();
            _model.GameOver += Model_GameOver;
        }

        public void LoadCreateTable()
        {
            for (int i = 0; i < _model.GetSize; i++)
            {
                for (int j = 0; j < _model.GetSize; j++)
                {
                    Fields.Add(new RobotMalacField()
                    {
                        IsLocked = false,
                        //Text = String.Empty,
                        Size = this.Size,
                        X = i,
                        Y = j,
                        Number = i * Size + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                        //StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            RightColumnVisibility = true;
            OnPropertyChanged(nameof(RightColumnVisibility));
            OnPropertyChanged(nameof(Size));
            ReloadTable();
            _model.GameOver += Model_GameOver;
        }
        #endregion

        #region Game event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object? sender, RobotEventArgs e)
        {
            RightColumnVisibility = false;
            //foreach (RobotMalacField field in Fields)
            //{
            //    field.IsLocked = true; // minden mezőt lezárunk
            //    field.IsEnabled = false;
            //    //_model.Step(field.X, field.Y);
            //}
        }

        /// <summary>
        /// Játék létrehozásának eseménykezelője.
        /// </summary>
        private void Model_GameCreated(object? sender, RobotEventArgs e)
        {
            RefreshTable();
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnNewGame(int size)
        {
            NewGame?.Invoke(this, size);
        }

        private void OnFourGame()
        {
            Fields.Clear();
            //PlayerTurnImage = new BitmapImage(new Uri("./pic/Peppa_malac_szemcsi_fekete.png", UriKind.Relative));
            //OnPropertyChanged(nameof(PlayerTurnImage));
            PlayerTurnText = "Player 1 on turn";
            OnPropertyChanged(nameof(PlayerTurnText));
            FourGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSixGame()
        {
            Fields.Clear();
            //PlayerTurnImage = new BitmapImage(new Uri("./pic/Peppa_malac_szemcsi_fekete.png", UriKind.Relative));
            //OnPropertyChanged(nameof(PlayerTurnImage));
            PlayerTurnText = "Player 1 on turn";
            OnPropertyChanged(nameof(PlayerTurnText));
            SixGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnEightGame()
        {
            Fields.Clear();
            //PlayerTurnImage = new BitmapImage(new Uri("./pic/Peppa_malac_szemcsi_fekete.png", UriKind.Relative));
            //OnPropertyChanged(nameof(PlayerTurnImage));
            PlayerTurnText = "Player 1 on turn";
            OnPropertyChanged(nameof(PlayerTurnText));
            EightGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnOkClicked()
        {
            if (_model.Table.GetPlayer == 1)
            {
                PlayerTurnImage = new FileImageSource { File = "peppa_malac_szemcsi_fekete.png" };
                OnPropertyChanged(nameof(PlayerTurnImage));
                PlayerTurnText = "Player 1 on turn";
                OnPropertyChanged(nameof(PlayerTurnText));
                _model.CollectCommand(TextCommand, _model.Table.GetPlayer);
                TextCommand = "";
                _model.Step();
                if (_model.Table.GetPlayer == 2)
                {
                    PlayerTurnImage = new FileImageSource { File = "peppa_malac_szemcsi_piros.png" };
                    OnPropertyChanged(nameof(PlayerTurnImage));
                    PlayerTurnText = "Player 2 on turn";
                    OnPropertyChanged(nameof(PlayerTurnText));
                }
            }
            else if (_model.Table.GetPlayer == 2)
            {
                PlayerTurnImage = new FileImageSource { File = "peppa_malac_szemcsi_piros.png" };
                OnPropertyChanged(nameof(PlayerTurnImage));
                PlayerTurnText = "Player 2 on turn";
                OnPropertyChanged(nameof(PlayerTurnText));
                _model.CollectCommand(TextCommand, _model.Table.GetPlayer);
                TextCommand = "";
                _model.Step();
                if (_model.Table.GetPlayer == 1)
                {
                    PlayerTurnImage = new FileImageSource { File = "peppa_malac_szemcsi_fekete.png" };
                    OnPropertyChanged(nameof(PlayerTurnImage));
                    PlayerTurnText = "Player 1 on turn";
                    OnPropertyChanged(nameof(PlayerTurnText));
                }
                _model.DoCommand();
                RefreshTable();
            }
            OkClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            Fields.Clear();
            LoadGame?.Invoke(this, EventArgs.Empty);
            //await _model.LoadGameAsync(Path);
            //LoadCreateTable();
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// How to play eseménykiváltása.
        /// </summary>
        private void OnHowToGame()
        {
            HowToGame?.Invoke(this, EventArgs.Empty);
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

/*
 * NewGameCommand = new DelegateCommand(param =>
            {
                if (param != null)
                {
                    // Assuming param.ToString() won't return null, but you can also check it if needed
                    int value;
                    if (int.TryParse(param.ToString(), out value))
                    {
                        OnNewGame(value);
                    }
                    else
                    {
                        // Handle the case where parsing fails (e.g., show an error message)
                        Console.WriteLine($"Failed to parse '{param}' as an integer.");
                    }
                }
                else
                {
                    // Handle the case where param is null (e.g., show an error message)
                    Console.WriteLine("Parameter is null.");
                }
            });
*/