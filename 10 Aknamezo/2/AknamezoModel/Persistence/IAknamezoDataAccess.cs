using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AknamezoModel.Persistence
{
    public interface IAknamezoDataAccess
    {

        Task SaveAsync(String path, Table table);
        Task<Table> LoadAsync(String path);
    }
}
