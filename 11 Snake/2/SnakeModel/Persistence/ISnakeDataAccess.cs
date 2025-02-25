using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeModel.Persistence
{
    public interface ISnakeDataAccess
    {

        Table LoadSnakeTable(String path);
    }
}
