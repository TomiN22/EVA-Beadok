using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotmalacModel.Persistence
{
    public interface IRobotDataAccess
    {

        Task SaveAsync(String path, RobotTable table);
        Task<RobotTable> LoadAsync(String path);
    }
}
