using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalomModel.Persistence
{
    public class MalomFileDataAccess : IMalomDataAccess
    {
        public async Task<Table> LoadAsync(String path)
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
                    table.P1Time = Int32.Parse(numbers[2]);
                    table.P2Time = Int32.Parse(numbers[3]);
                    table.P1Count = Int32.Parse(numbers[4]);
                    table.P2Count = Int32.Parse(numbers[5]);

                    for (Int32 j = 0; j < table.GetSize; j++)
                    {
                        line = reader.ReadLine() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            table.SetTableValue(j, i, Int32.Parse(numbers[i]));
                        }
                    }

                    for (Int32 j = 0; j < table.GetSize; j++)
                    {
                        line = reader.ReadLine() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            table.SetTableColor(j, i, Int32.Parse(numbers[i]));
                        }
                    }
                    return table;
                }
            }
            catch
            {
                throw new MalomDataException();
            }
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String path, Table table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    await writer.WriteLineAsync($"{table.GetSize} {table.Player} {table.P1Time} {table.P2Time} {table.P1Count} {table.P2Count}");
                    for (Int32 j = 0; j < table.GetSize; j++)
                    {
                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            if (table.GetTableValue(j, i) == 2)
                            {
                                await writer.WriteAsync("2" + " ");
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

                    for (Int32 j = 0; j < table.GetSize; j++)
                    {
                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            if (table.GetTableColor(j, i) == 2)
                            {
                                await writer.WriteAsync("2" + " ");
                            }
                            else if (table.GetTableColor(j, i) == 1)
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
                throw new MalomDataException();
            }
        }


    }
}
