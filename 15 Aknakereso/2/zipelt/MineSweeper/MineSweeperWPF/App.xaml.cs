using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MineSweeperWPF.ViewModel;
using Model;
using Persistance;


namespace MineSweeperWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
        public partial class App : Application
        {
            #region Fields

            private MineModel _model = null!;
            private MineSweeperViewModel _viewModel = null!;
            private MainWindow _view = null!;
            private DispatcherTimer _timer = null!;
            private IMineDataAccess _dataAccess;
            

            #endregion

            #region Constructors

            /// <summary>
            /// Alkalmazás példányosítása.
            /// </summary>
            public App()
            {
                Startup += new StartupEventHandler(App_Startup);
            }

            #endregion

            #region Application event handlers

            private void App_Startup(object? sender, StartupEventArgs e)
            {
                // modell létrehozása
                _dataAccess = new IMineFileDataAccess();
                _model = new MineModel(_dataAccess,0);
                _model.GameOver += new EventHandler<MineEventArgs>(Model_GameOver);
                //_model.NewGame();

                // nézemodell létrehozása
                _viewModel = new MineSweeperViewModel(_model);
                //_viewModel.NewGame += new EventHandler(ViewModel_NewGame);
                _viewModel.SixGame += new EventHandler(ViewModel_SixGame);
                _viewModel.TenGame += new EventHandler(ViewModel_TenGame);
                _viewModel.SixteenGame += new EventHandler(ViewModel_SixteenGame);
                _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
                _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
                _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

                // nézet létrehozása
                _view = new MainWindow();
                _view.DataContext = _viewModel;
                //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
                _view.Show();
            }

            #endregion

            #region View event handlers

            /// <summary>
            /// Nézet bezárásának eseménykezelője.
            /// </summary>
            /*private void View_Closing(object? sender, CancelEventArgs e)
            {
                Boolean restartTimer = _timer.IsEnabled;

                _timer.Stop();

                if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Sudoku", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true; // töröljük a bezárást

                    if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                        _timer.Start();
                }
            }*/

            #endregion

            #region ViewModel event handlers

            /// <summary>
            /// Új játék indításának eseménykezelője.
            /// </summary>
            /*private void ViewModel_NewGame(object? sender, EventArgs e)
            {
                //_model.NewGame();
                _timer.Start();
            }*/

        private void ViewModel_SixGame(object? sender, EventArgs e)
        {
            //_viewModel.Model = new MineModel(_dataAccess, 6);
            //_viewModel = new MineSweeperViewModel(_model);
            
            _model = new MineModel(_dataAccess, 6);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            //_model.GenerateFields();
        }

        private void ViewModel_TenGame(object? sender, EventArgs e)
        {
            _model = new MineModel(_dataAccess, 10);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            //_model.GenerateFields();
        }

        private void ViewModel_SixteenGame(object? sender, EventArgs e)
        {
            _model = new MineModel(_dataAccess, 16);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            //_model.GenerateFields();
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
                    openFileDialog.Title = "Loading MineSweeper";
                    openFileDialog.Filter = "Mine table|*.stl";
                    if (openFileDialog.ShowDialog() == true)
                    {
                    // játék betöltése
                    //await _model.LoadGameAsync(openFileDialog.FileName);
                    _viewModel.Path = openFileDialog.FileName;
                        //_timer.Start();
                    }
                }
                catch (MineDataException)
                {
                    MessageBox.Show("File load unsuccessful!", "MineSweeper", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    saveFileDialog.Title = "Loading MineSweeper";
                    saveFileDialog.Filter = "Mine table|*.stl";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        try
                        {
                            // játéktábla mentése
                            await _model.SaveGameAsync(saveFileDialog.FileName);
                        }
                        catch (MineDataException)
                        {
                            MessageBox.Show("File save unsuccessful!");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("File save unsuccessful!", "MineSweeper", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                //    _timer.Start();
            }

            /// <summary>
            /// Játékból való kilépés eseménykezelője.
            /// </summary>
            private void ViewModel_ExitGame(object? sender, System.EventArgs e)
            {
                _view.Close(); // ablak bezárása
            }

            #endregion

            #region Model event handlers

            /// <summary>
            /// Játék végének eseménykezelője.
            /// </summary>
            private void Model_GameOver(object? sender, MineEventArgs e)
            {
                //_timer.Stop();

                if (e.IsWon) // győzelemtől függő üzenet megjelenítése
                {
                    MessageBox.Show(e.Winner+"nyert!"
                                    //"Összesen " + e.GameStepCount + " lépést tettél meg és " +
                                    //TimeSpan.FromSeconds(e.GameTime).ToString("g") + " ideig játszottál.",
                                    );
                }
                else
                {
                    
                }
            }

            #endregion
        }
    
}
