using BlackHoleModel.Model;
using BlackHoleModel.Persistence;
using System;
using BlackHoleAvalonia.ViewModels;

namespace BlackHoleAvalonia.Views
{
    public static class DesignData
    {
        public static MainViewModel ViewModel
        {
            get
            {
                var model = new GameModel(new BlackHoleFileDataAccess(),7);
                model.GenerateFields();
                // egy elindított játékot rakunk be a nézetmodellbe, így a tervezőfelületen sem csak üres cellák lesznek
                return new MainViewModel(model);
            }
        }
    }
}
