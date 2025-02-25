using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AknamezoModel.Persistence
{
    public class AknamezoFileDataAccess : IAknamezoDataAccess
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
                    
                    table.PCurrent[1] = Int32.Parse(numbers[1]);
                    table.PCurrent[0] = Int32.Parse(numbers[2]);
                    table.GameTime = Int32.Parse(numbers[3]);
                    table.StdTime = Int32.Parse(numbers[4]);
                    table.FastTime = Int32.Parse(numbers[5]);

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
                throw new AknamezoDataException();
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
                    await writer.WriteLineAsync($"{table.GetSize} {table.PCurrent[1]} {table.PCurrent[0]} {table.GameTime} {table.StdTime} {table.FastTime}");
                    for (Int32 j = 0; j < table.GetSize; j++)
                    {
                        for (Int32 i = 0; i < table.GetSize; i++)
                        {
                            if (table.GetTableValue(j, i) == 5)
                            {
                                await writer.WriteAsync("5" + " ");
                            }
                            else if(table.GetTableValue(j, i) == 4)
                            {
                                await writer.WriteAsync("4" + " ");
                            }
                            else if (table.GetTableValue(j, i) == 3)
                            {
                                await writer.WriteAsync("3" + " ");
                            }
                            else if (table.GetTableValue(j, i) == 2)
                            {
                                await writer.WriteAsync("2" + " ");
                            }
                            else if (table.PCurrent[0] == j && table.PCurrent[1] == i)
                            {
                                await writer.WriteAsync("1" + " "); // kiírjuk az értékeket
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
                throw new AknamezoDataException();
            }
        }


    }
}
