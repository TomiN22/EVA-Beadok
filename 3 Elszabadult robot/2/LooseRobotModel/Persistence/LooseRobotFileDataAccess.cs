using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LooseRobotModel.Persistence
{
    public class LooseRobotFileDataAccess : ILooseRobotDataAccess
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
                    
                    table.RCurrent[1] = Int32.Parse(numbers[1]);
                    table.RCurrent[0] = Int32.Parse(numbers[2]);

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
                            table.SetWasWall(j, i, Int32.Parse(numbers[i]));
                        }
                    }


                    return table;
                }
            }
            catch
            {
                throw new LooseRobotDataException();
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
                    await writer.WriteLineAsync($"{table.GetSize} {table.RCurrent[1]} {table.RCurrent[0]}");
                    for (Int32 j = 0; j < table.GetSize; j++)
                    {
                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            if (table.GetTableValue(j, i) == 3)
                            {
                                await writer.WriteAsync("3" + " ");
                            }
                            else if (table.RCurrent[0] == j && table.RCurrent[1] == i)
                            {
                                await writer.WriteAsync("2" + " "); // kiírjuk az értékeket
                            }
                            else if ((table.GetSize/2) == j && (table.GetSize/2) == i)
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
                            if (table.WasWall(j, i) == 1)
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
                throw new LooseRobotDataException();
            }
        }


    }
}
