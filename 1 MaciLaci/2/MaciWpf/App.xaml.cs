using Microsoft.Win32;
using MaciModel.Model;
using MaciModel.Persistence;
using MaciWpf.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media.Imaging;

namespace MaciWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MaciGameModel _model = null!;
        private MaciViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private IMaciDataAccess? _dataAccess;
        private DispatcherTimer _timer;
        private string _path;


        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _dataAccess = new MaciFileDataAccess();
            _model = new MaciGameModel(_dataAccess, 0);
            _model.GameOver += new EventHandler<MaciEventArgs>(Model_GameOver);

            // nézemodell létrehozása
            _viewModel = new MaciViewModel(_model);
            _viewModel.FourGame += new EventHandler(ViewModel_FourGame);
            _viewModel.SixGame += new EventHandler(ViewModel_SixGame);
            _viewModel.EightGame += new EventHandler(ViewModel_EightGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);

            _viewModel.PauseStart += ViewModel_PauseStartGame;

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        private void ViewModel_FourGame(object? sender, EventArgs e)
        {
            _model = new MaciGameModel(_dataAccess, 4);
            _viewModel.Path="./save/4x4.txt";
            _model.LoadGame(_viewModel.Path);
            _viewModel.Model = _model;
            _viewModel.EnableButtons = true;
            _viewModel.LoadCreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();

            //LoadGame?.Invoke(this, EventArgs.Empty);
            //await _model.LoadGame(Path);
            //LoadCreateTable();
        }
        private void ViewModel_SixGame(object? sender, EventArgs e)
        {
            _model = new MaciGameModel(_dataAccess, 6);
            _viewModel.Path="./save/6x6.txt";
            _model.LoadGame(_viewModel.Path);
            _viewModel.Model = _model;
            _viewModel.EnableButtons = true;
            _viewModel.LoadCreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();

        }

        private void ViewModel_EightGame(object? sender, EventArgs e)
        {
            _model = new MaciGameModel(_dataAccess, 8);
            _viewModel.Path="./save/8x8.txt";
            _model.LoadGame(_viewModel.Path);
            _viewModel.Model = _model;
            _viewModel.EnableButtons = true;
            _viewModel.LoadCreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();
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
            }
            else
            {
                _timer.Stop();
            }
        }

        private void Model_GameOver(object? sender, MaciEventArgs e)
        {
            _viewModel.EnableButtons = false;
            _timer.Stop();
            if (_model.Baskets==0)
            {
                MessageBox.Show("Congrats! You've collected all the baskets!");
                
            }
            else // győzelemtől függő üzenet megjelenítése
            {
                MessageBox.Show("You've been spotted! You lost!");
                
            }
        }


    }

}
