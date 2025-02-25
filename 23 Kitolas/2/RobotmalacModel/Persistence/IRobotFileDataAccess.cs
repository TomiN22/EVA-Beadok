using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotmalacModel.Persistence
{
    public class IRobotFileDataAccess : IRobotDataAccess
    {
        public async Task<RobotTable> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla 
                    RobotTable table = new RobotTable(tableSize); // létrehozzuk a táblát
                    table.GetPlayer = Int32.Parse(numbers[1]);
                    table.Moves = Int32.Parse(numbers[2]);
                    table.Prey[0] = Int32.Parse(numbers[3]);
                    table.Prey[1] = Int32.Parse(numbers[4]);
                    for (Int32 j = 0; j < table.GetSize; j++)
                    {

                        line = await reader.ReadLineAsync() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            table.SetTableValue(i, j, Int32.Parse(numbers[i]));
                        }
                    }

                    return table;
                }
            }
            catch
            {
                throw new RobotDataException();
            }
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String path, RobotTable table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    await writer.WriteLineAsync($"{table.GetSize} {table.GetPlayer} {table.Moves} {table.Prey[0]} {table.Prey[1]}");
                    for (Int32 j = 0; j < table.GetSize; j++)
                    {
                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            if (table.GetTableValue(i, j) == 0)
                            {
                                await writer.WriteAsync("0" + " "); // kiírjuk az értékeket
                            }
                            else if (table.GetTableValue(i, j) == 1)
                            {
                                await writer.WriteAsync("1" + " ");
                            }
                            else if (table.GetTableValue(i, j) == 2)
                            {
                                await writer.WriteAsync("2" + " ");
                            }

                        }
                        await writer.WriteLineAsync();
                    }

                }
            }
            catch
            {
                throw new RobotDataException();
            }
        }
    }
}
