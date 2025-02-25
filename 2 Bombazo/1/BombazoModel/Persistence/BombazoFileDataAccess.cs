using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombazoModel.Persistence
{
    public class BombazoFileDataAccess : IBombazoDataAccess
    {
        public BombazoTable LoadBombazoTable(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = reader.ReadLine() ?? String.Empty;
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla 
                    BombazoTable table = new BombazoTable(tableSize); // létrehozzuk a táblát
                    for (Int32 i = 0; i < table.GetSize; i++)
                    {
                        line = reader.ReadLine() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < table.GetSize; j++)
                        {
                            table.SetTableValue(j, i, Int32.Parse(numbers[j]));
                        }
                    }

                    return table;
                }
            }
            catch
            {
                throw new BombazoDataException();
            }
        }

       
    }
}
