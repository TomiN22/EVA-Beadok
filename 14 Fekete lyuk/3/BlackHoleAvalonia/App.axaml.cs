using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.ComponentModel;
using BlackHoleModel.Model;
using BlackHoleModel.Persistence;
using BlackHoleAvalonia.ViewModels;
using BlackHoleAvalonia.Views;
using Avalonia.Threading;
using System.IO;
using System;
using Avalonia.Platform.Storage;
using Avalonia.Platform;

namespace BlackHoleAvalonia;

public partial class App : Application
{
    #region Fields

    private GameModel _model = null!;
    private MainViewModel _viewModel = null!;
    private IBlackHoleDataAccess? _dataAccess;
    private DispatcherTimer _timer = null!;

    #endregion

    #region Properites

    private TopLevel? TopLevel
    {
        get
        {
            return ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktop => TopLevel.GetTopLevel(desktop.MainWindow),
                ISingleViewApplicationLifetime singleViewPlatform => TopLevel.GetTopLevel(singleViewPlatform.MainView),
                _ => null
            };
        }
    }

    #endregion

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        // modell létrehozása
        _dataAccess = new BlackHoleFileDataAccess();
        _model = new GameModel(_dataAccess, 0);
        _model.GameOver += new EventHandler<BlackHoleEventArgs>(Model_GameOver);

        // nézemodell létrehozása
        _viewModel = new MainViewModel(_model);
        _viewModel.FiveGame += new EventHandler(ViewModel_Five);
        _viewModel.SevenGame += new EventHandler(ViewModel_Seven);
        _viewModel.NineGame += new EventHandler(ViewModel_Nine);
        //_viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);

        _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
        _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
        _viewModel.PauseStart += ViewModel_PauseStartGame;

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += new EventHandler(Timer_Tick);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // asztali környezethez
            desktop.MainWindow = new MainWindow
            {
                DataContext = _viewModel
            };

            //desktop.Startup += async (s, e) =>
            //{
            //    //_model.NewGame(); // indításkor új játékot kezdünk
            //    _model = new GameModel(_dataAccess, 0);

            //    // betöltjük a felfüggesztett játékot, amennyiben van
            //    try
            //    {
            //        await _model.LoadGameAsync(
            //            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BlackHoleSuspendedGame"));
            //        _viewModel.Model = _model;
            //        _viewModel.LoadCreateTable();
            //        _model.GameOver -= Model_GameOver;
            //        _model.GameOver += Model_GameOver;
            //    }
            //    catch { }
            //};

            //desktop.Exit += async (s, e) =>
            //{
            //    // elmentjük a jelenleg folyó játékot
            //    try
            //    {
            //        await _model.SaveGameAsync(
            //            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BlackHoleSuspendedGame"));
            //        // mentés a felhasználó Documents könyvtárába, oda minden bizonnyal van jogunk írni
            //    }
            //    catch { }
            //};
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            // mobil környezethez
            singleViewPlatform.MainView = new MainView
            {
                DataContext = _viewModel
            };

            if (Application.Current?.TryGetFeature<IActivatableLifetime>() is { } activatableLifetime)
            {
                activatableLifetime.Activated += async (sender, args) =>
                {
                    if (args.Kind == ActivationKind.Background)
                    {
                        // betöltjük a felfüggesztett játékot, amennyiben van
                        try
                        {
                            await _model.LoadGameAsync(
                                Path.Combine(AppContext.BaseDirectory, "SuspendedGame"));
                            _viewModel.Model = _model;
                            _viewModel.LoadCreateTable();
                            _model.GameOver -= Model_GameOver;
                            _model.GameOver += Model_GameOver;
                        }
                        catch
                        {
                        }
                    }
                };
                activatableLifetime.Deactivated += async (sender, args) =>
                {
                    if (args.Kind == ActivationKind.Background)
                    {

                        // elmentjük a jelenleg folyó játékot
                        try
                        {
                            await _model.SaveGameAsync(
                                Path.Combine(AppContext.BaseDirectory, "SuspendedGame"));
                            // Androidon az AppContext.BaseDirectory az alkalmazás adat könyvtára, ahova
                            // akár külön jogosultság nélkül is lehetne írni
                        }
                        catch
                        {
                        }
                    }
                };
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        //_model.AdvanceTime();
    }

    private void ViewModel_Five(object? sender, EventArgs e)
    {
        _model = new GameModel(_dataAccess, 5);
        _viewModel.Model = _model;
        _viewModel.CreateTable();
        _model.GameOver += Model_GameOver;
        _timer.Start();
    }
    private void ViewModel_Seven(object? sender, EventArgs e)
    {
        _model = new GameModel(_dataAccess, 7);
        _viewModel.Model = _model;
        _viewModel.CreateTable();
        _model.GameOver += Model_GameOver;
        _timer.Start();
    }
    private void ViewModel_Nine(object? sender, EventArgs e)
    {
        _model = new GameModel(_dataAccess, 9);
        _viewModel.Model = _model;
        _viewModel.CreateTable();
        _model.GameOver += Model_GameOver;
        _timer.Start();
    }

    //private void ViewModel_ExitGame(object? sender, System.EventArgs e)
    //{
    //    _view.Close(); // ablak bezárása
    //}

    private void ViewModel_PauseStartGame(object? sender, EventArgs e)
    {
        if (_viewModel.StartPause)
        {
            _timer.Start();
        }
        else
        {
            _timer.Stop();
        }
    }

    /// <summary>
    /// Játék betöltésének eseménykezelője.
    /// </summary>
    private async void ViewModel_LoadGame(object? sender, System.EventArgs e)
    {
        if (TopLevel == null)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "BlackHole játék",
                    "A fájlkezelés nem támogatott!",
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
            return;
        }
        /*Boolean restartTimer = !_model.IsGameOver;
        _model.PauseGame();*/
        try
        {
            var files = await TopLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "BlackHole tábla betöltése",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("BlackHole tábla")
                    {
                        Patterns = new[] { "*.stl" }
                    }
                }
            });

            if (files.Count > 0)
            {
                // játék betöltése
                using (var stream = await files[0].OpenReadAsync())
                {
                    await _model.LoadGameAsync(stream);
                    _viewModel.Model = _model;
                    _viewModel.LoadCreateTable();
                    _model.GameOver -= Model_GameOver;
                    _model.GameOver += Model_GameOver;
                }
            }
        }
        catch (BlackHoleDataException)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "BlackHole játék",
                    "A fájl betöltése sikertelen!",
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
        }
        /*if (restartTimer) // ha szükséges, elindítjuk az időzítőt
            _model.ResumeGame();*/
    }

    /// <summary>
    /// Játék mentésének eseménykezelője.
    /// </summary>
    private async void ViewModel_SaveGame(object? sender, EventArgs e)
    {
        if (TopLevel == null)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Black Hole game",
                    "A fájlkezelés nem támogatott!",
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
            return;
        }
        /*Boolean restartTimer = !_model.IsGameOver;
        _model.PauseGame();*/
        try
        {
            var file = await TopLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title = "Balck Hole tábla mentése",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Balck Hole tábla")
                    {
                        Patterns = new[] { "*.stl" }
                    }
                }
            });

            if (file != null)
            {
                // játék mentése
                using (var stream = await file.OpenWriteAsync())
                {
                    await _model.SaveGameAsync(stream);
                }
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Balck Hole game",
                    "A fájl mentése sikertelen!" + ex.Message,
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
        }
        /*if (restartTimer) // ha szükséges, elindítjuk az időzítőt
            _timer.Start();*/
    }

    private async void Model_GameOver(object? sender, BlackHoleEventArgs e)
    {
        _timer.Stop();
        _viewModel.EnableButtons = false;

        await MessageBoxManager.GetMessageBoxStandard("Game over!", _viewModel.Player+" won!", ButtonEnum.Ok, Icon.Success).ShowAsync();
    }
}
