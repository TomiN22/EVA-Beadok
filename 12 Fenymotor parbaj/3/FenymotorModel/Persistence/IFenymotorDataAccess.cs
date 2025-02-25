using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenymotorModel.Persistence
{
    public interface IFenymotorDataAccess
    {
        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="stream">Adatfolyam.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        Task SaveAsync(Stream stream, Table table);
        Task SaveAsync(String path, Table table);

        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="stream">Adatfolyam.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<Table> LoadAsync(Stream stream);
        Task<Table> LoadAsync(String path);
    }
}
