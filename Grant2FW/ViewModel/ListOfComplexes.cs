using Grant2FW.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Grant2FW.ViewModel
{

    /// <summary>
    /// Класс для получения данных из edmx и записывание их в ObservableCollection для работы с сущшностями 'жилищные комплексы'
    /// getList() получает список посредством  запроса к edmx
    /// </summary>
    internal class ListOfComplexesVM
    {
        public static ObservableCollection<ElementComplex> complexesCollection
        {
            get; set;
        }
        static public ObservableCollection<ElementComplex> getList()
        {
            complexesCollection = new ObservableCollection<ElementComplex>();

            var result = from complex in DataBase.ClassDataBase.dataobj.HousingComplexCopies
                         join status in DataBase.ClassDataBase.dataobj.Status on complex.Status equals status.Id
                         where complex.IsActual == true
                         orderby complex.City, status.StatusName
                         select new ElementComplex
                         {
                             IdCopy = complex.Id,
                             OriginalComplexId = complex.IdHousingComplex,
                             Cost_House_Construction = complex.Cost_Complex_Construction,
                             Additional_Cost_Apartament_House = complex.Additional_Cost_Complex,
                             IsActual = complex.IsActual,
                             ComplexName = complex.Name,
                             Status = status.StatusName,
                             City = complex.City
                         };

            foreach (var item in result)
            {
                complexesCollection.Add(item);
            }

            var resultHouses = from housingCopy in DataBase.ClassDataBase.dataobj.HousingCopies
                               join housing in DataBase.ClassDataBase.dataobj.Housing on housingCopy.OriginalHousingId equals housing.Id
                               where housingCopy.IsActual == true
                               select new
                               {
                                   HousingCopy = housingCopy,
                                   Housing = housing
                               };

            foreach (var complex in complexesCollection)
            {
                int numberOfHouses = resultHouses.Count(item => item.Housing.IdHousingComplex == complex.OriginalComplexId);
                complex.NumberHouse = numberOfHouses;
            }

            return complexesCollection;
        }
    }
    /// <summary>
    /// класс сущности 'жилищный комлекс'
    /// </summary>
    public class ElementComplex
    {
        public int IdCopy { get; set; }
        public Nullable<int> OriginalComplexId { get; set; }
        public Nullable<int> Cost_House_Construction { get; set; }
        public Nullable<int> Additional_Cost_Apartament_House { get; set; }
        public Nullable<bool> IsActual { get; set; }
        public string ComplexName { get; internal set; }
        public string Status { get; internal set; }
        public string City { get; internal set; }
        public int NumberHouse { get; set; }
    }
}
