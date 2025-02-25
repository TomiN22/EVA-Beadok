using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombazoModel.Persistence
{
    public interface IBombazoDataAccess
    {

        BombazoTable LoadBombazoTable(String path);
    }
}
