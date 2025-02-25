using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotMalacMaui.ViewModel
{
    public class TableSizeViewModel : ViewModelBase
    {
        private int _tablesize;

        public int TableSize
        {
            get
            {
                return _tablesize;
            }

            set
            {
                _tablesize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TableSizeText));
            }
        }

        public string TableSizeText => _tablesize.ToString();
    }
}
