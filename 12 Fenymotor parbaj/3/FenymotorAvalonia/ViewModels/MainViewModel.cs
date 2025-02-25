using FenymotorAvalonia.ViewModels;
using CommunityToolkit.Mvvm.Input;
using FenymotorModel.Model;
using System.Collections.ObjectModel;
using System;
using Avalonia.Input;
using System.Windows.Input;

namespace FenymotorAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    #region Fields

    private GameModel _model; // modell
    private int _size;
    private bool _startPause;
    private String _rightColumnVisibility = "Collapsed";
    private bool _enableButtons = false;

    #endregion

    #region Properties

    public int Size { get { return _size; } }

    public string Player { get; set; }
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
    public RelayCommand FiveCommand { get; private set; }
    public RelayCommand SevenCommand { get; private set; }
    public RelayCommand NineCommand { get; private set; }
    public RelayCommand PauseStartGameCommand { get; private set; }

    /// <summary>
    /// Játék betöltése parancs lekérdezése.
    /// </summary>
    public RelayCommand LoadGameCommand { get; private set; }

    /// <summary>
    /// Játék mentése parancs lekérdezése.
    /// </summary>
    public RelayCommand SaveGameCommand { get; private set; }

    /// <summary>
    /// Kilépés parancs lekérdezése.
    /// </summary>
    public RelayCommand ExitCommand { get; private set; }

    public ICommand KeyCommand { get; }

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

    public event EventHandler? FiveGame;
    public event EventHandler? SevenGame;
    public event EventHandler? NineGame;
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
    public MainViewModel(GameModel model)
    {
        // játék csatlakoztatása
        _model = model;
        _size = _model.Table.GetSize;
        FiveCommand = new RelayCommand(OnFive);
        SevenCommand = new RelayCommand(OnSeven);
        NineCommand = new RelayCommand(OnNine);
        SaveGameCommand = new RelayCommand(OnSaveGame);
        LoadGameCommand = new RelayCommand(OnLoadGame);
        ExitCommand = new RelayCommand(OnExitGame);
        PauseStartGameCommand = new RelayCommand(PauseStartGame);
        KeyCommand = new ParameterRelayCommand(OnKeyCommand);
        //_model.GameOver += Model_GameOver;
        EnableButtons = true;
        EnablePause = false;
        EnableSave = false;
        EnableLoad = true;
        OnPropertyChanged(nameof(EnableSave));
        OnPropertyChanged(nameof(EnableLoad));

        Path="";

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

            //field.Text = _model.Table.GetTableValue(field.X, field.Y).ToString();
        }

        OnPropertyChanged(nameof(Player));
        //OnPropertyChanged(nameof(Text));            
    }

    public void CreateTable()
    {
        _size = _model.GetSize;
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
                    //Number = i * Size + j, // a gsomb sorszáma, amelyet felhasználunk az azonosításhoz
                    StepCommand = new RelayCommand<Tuple<Int32, Int32>>(position =>
                    {
                        if (position != null)
                            StepGame(position.Item1, position.Item2);
                    })
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
        EnableSave = true;
        EnableLoad = true;
        OnPropertyChanged(nameof(EnableSave));
        OnPropertyChanged(nameof(EnableLoad));
        OnPropertyChanged(nameof(EnablePause));
        RefreshTable();
    }

    public void LoadCreateTable()
    {
        Fields.Clear();
        _size = _model.Table.GetSize;
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
                    //Number = i * Size + j, // a gomb sorszáma, amelyet felhasználunk az azonosításhoz
                    StepCommand = new RelayCommand<Tuple<Int32, Int32>>(position =>
                    {
                        if (position != null)
                            StepGame(position.Item1, position.Item2);
                    })
                });

            }
        }

        StartPause=true;
        EnableButtons = true;
        EnablePause = true;
        if (_size != 0)
        {
            EnableSave = true;
            EnableLoad = true;
        }

        OnPropertyChanged(nameof(EnableSave));
        OnPropertyChanged(nameof(EnableLoad));
        OnPropertyChanged(nameof(EnablePause));

        RightColumnVisibility = "Visible";
        OnPropertyChanged(nameof(RightColumnVisibility));
        OnPropertyChanged(nameof(Size));
        _model.GameAdvanced += Model_GameAdvanced;
        _model.GameOver += Model_GameOver;
        //_model.GameAdvanced += new EventHandler<FenymotorEventArgs>(Model_GameAdvanced);

        RefreshTable();

        //PauseStart?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Game event handlers

    /// <summary>
    /// Játék léptetése eseménykiváltása.
    /// </summary>
    /// <param name="index">A lépett mező indexe.</param>
    private void StepGame(Int32 x, Int32 y)
    {
        //_model.Step(x, y);

        //OnPropertyChanged(nameof(_model.Table.Player));
        RefreshTable();
    }

    private void OnKeyCommand(object? parameter)
    {
        if (EnableButtons && (parameter is string key))
        {
            switch (key)
            {
                case "a":
                    _model.KeyCommand("left", 1);
                    break;
                case "d":
                    _model.KeyCommand("right", 1);
                    break;
                case "j":
                    _model.KeyCommand("left", 2);
                    break;
                case "l":
                    _model.KeyCommand("right", 2);
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

    private void Model_GameAdvanced(object? sender, FenymotorEventArgs e)
    {
        RefreshTable();
        OnPropertyChanged(nameof(PlayerTime));
    }

    /// <summary>
    /// Játék végének eseménykezelője.
    /// </summary>
    private void Model_GameOver(object? sender, FenymotorEventArgs e)
    {
        foreach (Field field in Fields)
        {
            field.IsLocked = true; // minden mezőt lezárunk
            field.IsEnabled = false;
            //_model.Step(field.X, field.Y);
        }
        Fields.Clear();
        EnableSave = false;
        OnPropertyChanged(nameof(EnableSave));
        RightColumnVisibility="Collapsed";
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

    private void OnFive()
    {
        Fields.Clear();
        FiveGame?.Invoke(this, EventArgs.Empty);
    }

    private void OnSeven()
    {
        Fields.Clear();
        SevenGame?.Invoke(this, EventArgs.Empty);
    }

    private void OnNine()
    {
        Fields.Clear();
        NineGame?.Invoke(this, EventArgs.Empty);
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
        //await _model.LoadGameAsync(Path);
        //LoadCreateTable();
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
