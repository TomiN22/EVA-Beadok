using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwariModel.Persistence
{
    public interface IAwariDataAccess
    {
        Task SaveAsync(String path, Table table);
        Task<Table> LoadAsync(String path);
    }
}
