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
            _viewModel.ThreeGame += new EventHandler(ViewModel_ThreeGame);
            _viewModel.FourGame += new EventHandler(ViewModel_FourGame);
            _viewModel.SixGame += new EventHandler(ViewModel_SixGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);

            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }

        private void ViewModel_ThreeGame(object? sender, EventArgs e)
        {
            _model = new RobotModel(_dataAccess, 3);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
        }
        private void ViewModel_FourGame(object? sender, EventArgs e)
        {
            _model = new RobotModel(_dataAccess, 4);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            //_model.GenerateFields();
        }

        private void ViewModel_SixGame(object? sender, EventArgs e)
        {
            _model = new RobotModel(_dataAccess, 6);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
        }

        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        private void Model_GameOver(object? sender, RobotEventArgs e)
        {
            if(e.Winner == Win.Draw)
            {
                MessageBox.Show(e.Winner+"!");
                if (_model.GetSize == 3)
                    ViewModel_ThreeGame(sender, e);
                else if (_model.GetSize == 4)
                    ViewModel_FourGame(sender, e);
                else if (_model.GetSize == 6)
                    ViewModel_SixGame(sender, e);
            }
            else // győzelemtől függő üzenet megjelenítése
            {
                MessageBox.Show(e.Winner+" won!");
                if (_model.GetSize == 3)
                    ViewModel_ThreeGame(sender, e);
                else if(_model.GetSize == 4)
                    ViewModel_FourGame(sender, e);
                else if(_model.GetSize == 6)
                    ViewModel_SixGame(sender, e);
            }
        }

        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            //Boolean restartTimer = _timer.IsEnabled;

            //_timer.Stop();

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "Saving Kitolas...";
                saveFileDialog.Filter = "Kitolas table|*.stl";
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
                MessageBox.Show("File save unsuccessful!", "Kitolas", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Loading Kitolas...";
                openFileDialog.Filter = "Kitolas|*.stl";
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
                MessageBox.Show("File load unsuccessful!", "Kitolas", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }

}
