using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grant2FW.ViewModel
{
    internal class ListOfHousesOfComplex
    {
        public static ObservableCollection<ElementHouseOfComplex> complexesCollection
        {
            get; set;
        }
        static public ObservableCollection<ElementHouseOfComplex> getList(int IdComplex)
        {
            ObservableCollection<ElementHouseOfComplex> houseOfComplexCollection = new ObservableCollection<ElementHouseOfComplex>();

            var result = from complex in DataBase.ClassDataBase.dataobj.HousingComplexCopies
                         join housing in DataBase.ClassDataBase.dataobj.Housing on complex.IdHousingComplex equals housing.IdHousingComplex
                         join housingCopy in DataBase.ClassDataBase.dataobj.HousingCopies on housing.Id equals housingCopy.OriginalHousingId
                         where complex.IsActual == true && housingCopy.IsActual == true && complex.IdHousingComplex == IdComplex
                         orderby housing.Street, housingCopy.Number_House
                         select new ElementHouseOfComplex
                         {
                            Street = housing.Street,
                            NumberHouse = housingCopy.Number_House
                         };


            foreach (var item in result)
            {
                houseOfComplexCollection.Add(item);
            }
            return houseOfComplexCollection;
        }
    }

    public class ElementHouseOfComplex
    {
        public string Street{ get; internal set; }
        public string NumberHouse { get; set; }
    }

}
