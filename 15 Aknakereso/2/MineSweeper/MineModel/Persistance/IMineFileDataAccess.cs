using Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public class IMineFileDataAccess : IMineDataAccess
    {
        public async Task<MineTable> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla 
                    MineTable table = new MineTable(tableSize); // létrehozzuk a táblát
                    table.GetPlayer = Int32.Parse(numbers[1]);
                    table.GetMines = Int32.Parse(numbers[2]);

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            table.SetValue(i, j, Int32.Parse(numbers[j]));
                        }
                    }

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        String[] locks = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            if (locks[j] == "1")
                            {
                                table.SetToOpened(i, j);
                            }
                        }
                    }

                    return table;
                }
            }
            catch
            {
                throw new MineDataException();
            }
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String path, MineTable table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    await writer.WriteLineAsync($"{table.GetSize} {table.GetPlayer} {table.GetMines}");
                    for (Int32 i = 0; i < table.GetSize; i++)
                    {
                        for (Int32 j = 0; j < table.GetSize; j++)
                        {
                            await writer.WriteAsync(table[i, j] + " "); // kiírjuk az értékeket
                        }
                        await writer.WriteLineAsync();
                    }

                    for (Int32 i = 0; i < table.GetSize; i++)
                    {
                        for (Int32 j = 0; j < table.GetSize; j++)
                        {
                            await writer.WriteAsync((table.IsOpened(i, j) ? "1" : "0") + " "); // kiírjuk a zárolásokat
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new MineDataException();
            }
        }
    }
}
