using FenymotorModel.Model;
using FenymotorModel.Persistence;
using System;
using FenymotorAvalonia.ViewModels;

namespace FenymotorAvalonia.Views
{
    public static class DesignData
    {
        public static MainViewModel ViewModel
        {
            get
            {
                var model = new GameModel(new FenymotorFileDataAccess(),7);
                model.GenerateFields();
                // egy elindított játékot rakunk be a nézetmodellbe, így a tervezőfelületen sem csak üres cellák lesznek
                return new MainViewModel(model);
            }
        }
    }
}
