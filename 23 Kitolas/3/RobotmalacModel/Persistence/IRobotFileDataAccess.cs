using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotmalacModel.Persistence
{
    public class IRobotFileDataAccess : IRobotDataAccess
    {
        /// <summary>
        /// Könyvtár.
        /// </summary>
        private String? _directory = String.Empty;

        public IRobotFileDataAccess(String? saveDirectory = null)
        {
            _directory = saveDirectory;
        }
        public async Task<RobotTable> LoadAsync(String path)
        {
            if (!String.IsNullOrEmpty(_directory))
                path = Path.Combine(_directory, path);
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla 
                    RobotTable table = new RobotTable(tableSize); // létrehozzuk a táblát
                    table.GetPlayer = Int32.Parse(numbers[1]);
                    table.P1Current[0] = Int32.Parse(numbers[2]);
                    table.P1Current[1] = Int32.Parse(numbers[3]);
                    table.P1Direction = (numbers[4]);
                    table.P2Current[0] = Int32.Parse(numbers[5]);
                    table.P2Current[1] = Int32.Parse(numbers[6]);
                    table.P2Direction = (numbers[7]);
                    table.P1Health = Int32.Parse(numbers[8]);
                    table.P2Health = Int32.Parse(numbers[9]);


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
                    await writer.WriteLineAsync($"{table.GetSize} {table.GetPlayer} {table.P1Current[0]} {table.P1Current[1]} {table.P1Direction} {table.P2Current[0]} {table.P2Current[1]} {table.P2Direction} {table.P1Health} {table.P2Health}");
                    for (Int32 j = 0; j < table.GetSize; j++)
                    {
                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            if (table.P1Current[0] == i && table.P1Current[1] == j)
                            {
                                await writer.WriteAsync("1" + " "); // kiírjuk az értékeket
                            }
                            else if(table.P2Current[0] == i && table.P2Current[1] == j)
                            {
                                await writer.WriteAsync("2" + " ");
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
                throw new RobotDataException();
            }
        }
    }
}
