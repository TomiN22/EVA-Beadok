using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaciModel.Persistence
{
    public class MaciFileDataAccess : IMaciDataAccess
    {
        public MaciTable LoadMaciTable(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = reader.ReadLine() ?? String.Empty;
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla 
                    MaciTable table = new MaciTable(tableSize); // létrehozzuk a táblát
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
                throw new MaciDataException();
            }
        }

       
    }
}
