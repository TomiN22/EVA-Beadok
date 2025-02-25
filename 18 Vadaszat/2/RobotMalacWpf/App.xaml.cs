using Microsoft.Win32;
using RobotmalacModel.Model;
using RobotmalacModel.Persistence;
using RobotMalacWpf.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace RobotMalacWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private RobotModel _model = null!;
        private RobotMalacViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private IRobotDataAccess? _dataAccess;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _dataAccess = new IRobotFileDataAccess();
            _model = new RobotModel(_dataAccess, 0);
            _model.GameOver += new EventHandler<RobotEventArgs>(Model_GameOver);

            // nézemodell létrehozása
            _viewModel = new RobotMalacViewModel(_model);
            _viewModel.FourGame += new EventHandler(ViewModel_FourGame);
            _viewModel.SixGame += new EventHandler(ViewModel_SixGame);
            _viewModel.EightGame += new EventHandler(ViewModel_EightGame);
            //_viewModel.HowToGame += new EventHandler(ViewModel_HowToGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);

            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }

        private void ViewModel_FourGame(object? sender, EventArgs e)
        {
            _model = new RobotModel(_dataAccess, 3);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
        }
        private void ViewModel_SixGame(object? sender, EventArgs e)
        {
            _model = new RobotModel(_dataAccess, 5);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            //_model.GenerateFields();
        }

        private void ViewModel_EightGame(object? sender, EventArgs e)
        {
            _model = new RobotModel(_dataAccess, 7);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
        }

        //private void ViewModel_HowToGame(object? sender, System.EventArgs e)
        //{
        //    MessageBox.Show("Move commands: 'move left', 'move right', 'move up' and 'move down'.\r\n" +
        //        "Turn with: 'turn left' or 'turn right'.\r\nAttack using the 'shoot' or 'punch'." +
        //        " The 'shoot' has a greater precedency.\r\n", "|How to play|"
        //                        );
        //}
        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        private void Model_GameOver(object? sender, RobotEventArgs e)
        {
            if (e.IsWon) // győzelemtől függő üzenet megjelenítése
            {
                MessageBox.Show(e.Winner+" won!");
            }
            if (_model.IsGameOver)
            {
                if (_model.Table.GetSize == 3)
                {
                    if (_model != null)
                    {
                        _model.GameOver -= Model_GameOver;
                    }
                    //_size = 3;
                    _model = new RobotModel(_dataAccess, _model.Table.GetSize);

                    _model = new RobotModel(_dataAccess, 3);
                    _viewModel.Model = _model;
                    _viewModel.CreateTable();
                    _model.GameOver += Model_GameOver;
                    //remainingCountLabel.Text = _model.Table.Moves.ToString();
                    //GenerateTable();
                }
                else if (_model.Table.GetSize == 5)
                {
                    if (_model != null)
                    {
                        _model.GameOver -= Model_GameOver;
                    }
                    //_size = 5;
                    _model = new RobotModel(_dataAccess, _model.Table.GetSize);
                    
                    _model = new RobotModel(_dataAccess, 5);
                    _viewModel.Model = _model;
                    _viewModel.CreateTable();
                    _model.GameOver += Model_GameOver;
                    //remainingCountLabel.Text = _model.Table.Moves.ToString();
                    //GenerateTable();
                }
                else if (_model.Table.GetSize == 7)
                {
                    if (_model != null)
                    {
                        _model.GameOver -= Model_GameOver;
                    }
                    //_size = 7;
                    _model = new RobotModel(_dataAccess, _model.Table.GetSize);

                    _model = new RobotModel(_dataAccess, 5);
                    _viewModel.Model = _model;
                    _viewModel.CreateTable();
                    _model.GameOver += Model_GameOver;
                    //remainingCountLabel.Text = _model.Table.Moves.ToString();
                    //GenerateTable();
                }

            }
        }

        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            //Boolean restartTimer = _timer.IsEnabled;

            //_timer.Stop();

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "Saving Hunt...";
                saveFileDialog.Filter = "Hunt|*.stl";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (RobotDataException)
                    {
                        MessageBox.Show("File save unsuccessful!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("File save unsuccessful!", "Hunt", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Loading Hunt...";
                openFileDialog.Filter = "Hunt|*.stl";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    //await _model.LoadGameAsync(openFileDialog.FileName);
                    _viewModel.Path = openFileDialog.FileName;
                    //_timer.Start();
                }
            }
            catch (RobotDataException)
            {
                MessageBox.Show("File load unsuccessful!", "Hunt", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }

}
