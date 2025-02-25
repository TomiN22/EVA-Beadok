using AsteroidsModel.Model;
using AsteroidsModel.Persistence;
using AsteroidsWPF.ViewModel;
using Microsoft.Win32;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace GyorsulasWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GameModel _model = null!;
        private AViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private IAsteroidsDataAccess? _dataAccess;
        private DispatcherTimer _timer;
        private DispatcherTimer _normTimer;
        private string _path;

        public int DispTime {  get; set; }

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _dataAccess = new AsteroidsFileDataAccess();
            _model = new GameModel(_dataAccess, 0);
            _model.GameOver += new EventHandler<AsteroidsEventArgs>(Model_GameOver);

            // nézemodell létrehozása
            _viewModel = new AViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);

            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            _viewModel.PauseStart += ViewModel_PauseStartGame;

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);

            _normTimer = new DispatcherTimer();
            _normTimer.Interval = TimeSpan.FromSeconds(1);
            _normTimer.Tick += new EventHandler(NormTimer_Tick);

            //DispTime=_timer.ToString();
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }

        private void NormTimer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceNormTime();
            //_timer.Interval = TimeSpan.FromMilliseconds(Math.Max(30, _timer.Interval.TotalMilliseconds - 50));
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceTime();
            _timer.Interval = TimeSpan.FromMilliseconds(_model.TimeInterval);
        }

        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _model = new GameModel(_dataAccess, 15);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();
            _normTimer.Start();
        }

        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        private void ViewModel_PauseStartGame(object? sender, EventArgs e)
        {
            if (_viewModel.StartPause)
            {
                _timer.Start();
                _normTimer.Start();
            }
            else
            {
                _timer.Stop();
                _normTimer.Stop();
            }
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            //Boolean restartTimer = _timer.IsEnabled;

            //_timer.Stop();

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Loading Loose Robot";
                openFileDialog.Filter = "LR table|*.stl";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    //await _model.LoadGameAsync(openFileDialog.FileName);
                    _viewModel.Path = openFileDialog.FileName;
                    //_timer.Start();
                }
            }
            catch (AsteroidsDataException)
            {
                MessageBox.Show("File load unsuccessful!", "Loose Robot", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /*if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                _timer.Start();*/
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            //Boolean restartTimer = _timer.IsEnabled;

            //_timer.Stop();

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "Loading Loose Robot";
                saveFileDialog.Filter = "LR table|*.stl";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (AsteroidsDataException)
                    {
                        MessageBox.Show("File save unsuccessful!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("File save unsuccessful!", "Loose Robot", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //if (restartTimer) // ha szükséges, elindítjuk az időzítőt
            //    _timer.Start();
        }

        private void Model_GameOver(object? sender, AsteroidsEventArgs e)
        {
            _timer.Stop();
            _normTimer.Stop();
            _viewModel.EnableButtons = false;

            MessageBox.Show(_viewModel.StdGameTime.ToString(), "Game over! You're out of fuel!");

        }
    }

}
