using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunModel.Persistence
{
    public interface IRunDataAccess
    {

        Task SaveAsync(String path, Table table);
        Task<Table> LoadAsync(String path);
    }
}
