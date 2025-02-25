#nullable enable
using RobotmalacModel.Model;
using RobotmalacModel.Persistence;
using RobotMalacMaui.ViewModel;
using Microsoft.Maui.Controls;
using RobotMalacMaui.Persistence;

namespace RobotMalacMaui
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private const string SuspendedGameSavePath = "SuspendedGame";

        private readonly AppShell _appShell;
        private readonly RobotMalacViewModel _robotViewModel;
        private RobotModel _robotGameModel;
        private readonly IRobotDataAccess _dataAccess;
        private readonly IStore _robotStore;

        public App()
        {
            InitializeComponent();

            _robotStore = new RobotStore();
            _dataAccess = new IRobotFileDataAccess(FileSystem.AppDataDirectory);

            _robotGameModel = new RobotModel(_dataAccess, 7);
            _robotViewModel = new RobotMalacViewModel(_robotGameModel);

            _appShell = new AppShell(_robotStore, _dataAccess, _robotGameModel, _robotViewModel)
            {
                BindingContext = _robotViewModel
            };
            MainPage = _appShell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);

            // az alkalmazás indításakor
            window.Created += (s, e) =>
            {
                // új játékot indítunk
                _robotGameModel = new RobotModel(_dataAccess, 7);
            };

            window.Activated += (s, e) =>
            {
                if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
                    return;

                Task.Run(async () =>
                {
                    try
                    {
                        await _robotGameModel.LoadGameAsync(SuspendedGameSavePath);
                        //_appShell.StartTimer();
                        //_robotViewModel.LoadCreateTable();
                    }
                    catch
                    {
                    }
                });
            };

            window.Stopped += (s, e) =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        //_appShell.StopTimer();
                        await _robotGameModel.SaveGameAsync(SuspendedGameSavePath);
                    }
                    catch
                    {
                    }
                });
            };

            const int newHeight = 900;

            window.Height = newHeight;

            return window;
        }
    }
}
