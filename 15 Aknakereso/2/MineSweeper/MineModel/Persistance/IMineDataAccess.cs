using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public interface IMineDataAccess
    {
        Task SaveAsync(String path, MineTable table);
        Task<MineTable> LoadAsync(String path);
    }
}
