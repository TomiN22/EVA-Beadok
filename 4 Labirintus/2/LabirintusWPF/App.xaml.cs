using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using LabirintusModel.Model;
using LabirintusModel.Persistence;
using LabirintusWPF.ViewModel;

namespace LabirintusWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GameModel _model = null!;
        private LabirintusViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private ILabirintusDataAccess? _dataAccess;
        private DispatcherTimer _timer;
        private string _path;


        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _dataAccess = new LabirintusFileDataAccess();
            _model = new GameModel(_dataAccess, 0);
            _model.GameOver += new EventHandler<LabirintusEventArgs>(Model_GameOver);

            // nézemodell létrehozása
            _viewModel = new LabirintusViewModel(_model);
            _viewModel.Lvl1Game += new EventHandler(ViewModel_Lvl1);
            _viewModel.Lvl2Game += new EventHandler(ViewModel_Lvl2);
            _viewModel.Lvl3Game += new EventHandler(ViewModel_Lvl3);
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

        private void ViewModel_Lvl1(object? sender, EventArgs e)
        {
            _model = new GameModel(_dataAccess, 10);
            _viewModel.Path="./save/Level1.txt";
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
        private void ViewModel_Lvl2(object? sender, EventArgs e)
        {
            _model = new GameModel(_dataAccess, 15);
            _viewModel.Path="./save/Level2.txt";
            _model.LoadGame(_viewModel.Path);
            _viewModel.Model = _model;
            _viewModel.EnableButtons = true;
            _viewModel.LoadCreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();

        }

        private void ViewModel_Lvl3(object? sender, EventArgs e)
        {
            _model = new GameModel(_dataAccess, 17);
            _viewModel.Path="./save/Level3.txt";
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

        private void Model_GameOver(object? sender, LabirintusEventArgs e)
        {
            _timer.Stop();
            _viewModel.EnableButtons = false;
            
            MessageBox.Show("You've found the way out!", "You won!");

        }
    }

}
