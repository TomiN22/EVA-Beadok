using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwariModel.Persistence
{
    public class AwariFileDataAccess : IAwariDataAccess
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

                    for (Int32 j = 0; j < 2; j++)
                    {
                        line = reader.ReadLine() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            table.SetTableValue(j, i, Int32.Parse(numbers[i]));
                        }
                    }

                    for (Int32 i = 0; i < 2; i++)
                    {
                        line = reader.ReadLine() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            table.SetTableNum(i, j, Int32.Parse(numbers[j]));
                        }
                    }
                    return table;
                }
            }
            catch
            {
                throw new AwariDataException();
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
                    await writer.WriteLineAsync($"{table.GetSize} {table.Player} {table.P1Time} {table.P2Time}");
                    for (Int32 j = 0; j < 2; j++)
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
                                await writer.WriteAsync("-1" + " ");
                            }
                        }
                        await writer.WriteLineAsync();
                    }

                    for (Int32 i = 0; i < 2; i++)
                    {
                        for (Int32 j = 0; j < table.GetSize; j++)
                        {
                            await writer.WriteAsync(table.GetTableNum(i, j).ToString()+" ");
                        }
                        await writer.WriteLineAsync();
                    }

                }
            }
            catch
            {
                throw new AwariDataException();
            }
        }


    }
}
