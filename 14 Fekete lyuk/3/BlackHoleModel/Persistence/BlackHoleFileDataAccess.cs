using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace BlackHoleModel.Persistence
{
    public class BlackHoleFileDataAccess : IBlackHoleDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        public async Task<Table> LoadAsync(String path)
        {
            return await LoadAsync(File.OpenRead(path));
        }

        public async Task<Table> LoadAsync(Stream path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla 
                    Table table = new Table(tableSize); // létrehozzuk a táblát

                    table.Player = Int32.Parse(numbers[1]);
                    table.P1ShipsInHole = Int32.Parse(numbers[2]);
                    table.P2ShipsInHole = Int32.Parse(numbers[3]);

                    for (Int32 j = 0; j < table.GetSize; j++)
                    {
                        line = reader.ReadLine() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            table.SetTableValue(j, i, Int32.Parse(numbers[i]));
                        }
                    }
                    return table;
                }
            }
            catch
            {
                throw new BlackHoleDataException();
            }
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String path, Table table)
        {
            await SaveAsync(File.OpenWrite(path), table);
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(Stream path, Table table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    await writer.WriteLineAsync($"{table.GetSize} {table.Player} {table.P1ShipsInHole} {table.P2ShipsInHole}");
                    for (Int32 j = 0; j < table.GetSize; j++)
                    {
                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            if (table.GetTableValue(j, i) == 3)
                            {
                                await writer.WriteAsync("3" + " ");
                            }
                            else if (table.GetTableValue(j, i) == 2)
                            {
                                await writer.WriteAsync("2" + " "); // kiírjuk az értékeket
                            }
                            else if (table.GetTableValue(j, i) == 1)
                            {
                                await writer.WriteAsync("1" + " ");
                            }
                            else
                            {
                                await writer.WriteAsync("0" + " ");
                            }

                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new BlackHoleDataException();
            }
        }


    }
}
