using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabirintusModel.Persistence
{
    public interface ILabirintusDataAccess
    {

        LabirintusTable LoadLabirintusTable(String path);
    }
}
