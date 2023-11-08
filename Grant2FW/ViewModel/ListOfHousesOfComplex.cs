using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grant2FW.ViewModel
{

    /// <summary>
    /// Класс для получения данных из edmx и записывание их в ObservableCollection для работы с сущшностями 'дома жилищных комплексов'
    /// getList() получает список посредством запроса к edmx
    /// </summary>
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
                         join housingCopy in DataBase.ClassDataBase.dataobj.HousingCopies on complex.Id equals housingCopy.IdComplex
                         where complex.IsActual == true && housingCopy.IsActual == true && complex.IdHousingComplex == IdComplex
                         orderby housingCopy.Street, housingCopy.Number_House
                         select new ElementHouseOfComplex
                         {
                            Street = housingCopy.Street,
                            NumberHouse = housingCopy.Number_House
                         };


            foreach (var item in result)
            {
                houseOfComplexCollection.Add(item);
            }
            return houseOfComplexCollection;
        }
    }
    /// <summary>
    /// класс сущности 'дом ЖК'
    /// </summary>
    public class ElementHouseOfComplex
    {
        public string Street{ get; internal set; }
        public string NumberHouse { get; set; }
    }

}
