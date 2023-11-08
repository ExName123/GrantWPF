using Grant2FW.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Grant2FW.ViewModel
{
    /// <summary>
    ///   Класс для получения данных из edmx и записывание их в ObservableCollection для работы с сущшностями 'дома'
    /// getList() получает список посредством  запроса к edmx
    /// </summary>
    public class ListOfHousesVM
    {
        public static ObservableCollection<Element> housesCollection
        {
            get; set;
        }
        static public ObservableCollection<Element> getList()
        {
            housesCollection = new ObservableCollection<Element>();

            var result = from housing in DataBase.ClassDataBase.dataobj.Housing
                         join copies in DataBase.ClassDataBase.dataobj.HousingCopies on housing.Id equals copies.OriginalHousingId
                         join complex in DataBase.ClassDataBase.dataobj.HousingComplex on copies.IdComplex equals complex.Id
                         join complexCopy in DataBase.ClassDataBase.dataobj.HousingComplexCopies on complex.Id equals complexCopy.IdHousingComplex
                         join status in DataBase.ClassDataBase.dataobj.Status on complexCopy.Status equals status.Id
                         where copies.IsActual == true
                         orderby complex.Name, copies.Street, copies.Number_House
                         select new Element
                         {
                             IdCopy = copies.Id,
                             OriginalHousingId = copies.OriginalHousingId,
                             Number_House = copies.Number_House,
                             Cost_House_Construction = copies.Cost_House_Construction,
                             Additional_Cost_Apartament_House = copies.Additional_Cost_Apartament_House,
                             Added_Value = copies.Added_Value,
                             Building_Costs = copies.Building_Costs,
                             IsActual = copies.IsActual,
                             Street = copies.Street,
                             IdComplex = copies.IdComplex,
                             ComplexName = complex.Name,
                             Status = status.StatusName
                         };

            foreach (var item in result)
            {
        //        item.CountAppartmentsSold = result
        //.Count(element => element.Status == "sold" && element.OriginalHousingId == item.OriginalHousingId);

        //        item.CountAppartmentsReady = result
        //            .Count(element => element.Status == "ready" && element.OriginalHousingId == item.OriginalHousingId);

                housesCollection.Add(item);
            }

            return housesCollection;
        }
    }
    /// <summary>
    /// класс сущности 'дом'
    /// </summary>
    public class Element
    {
        public int IdCopy { get; set; }
        public int CountAppartmentsSold { get; set; }
        public int CountAppartmentsReady { get; set; }
        public Nullable<int> OriginalHousingId { get; set; }
        public string Number_House { get; set; }
        public Nullable<int> Cost_House_Construction { get; set; }
        public Nullable<int> Additional_Cost_Apartament_House { get; set; }
        public Nullable<int> Added_Value { get; set; }
        public Nullable<int> Building_Costs { get; set; }
        public Nullable<bool> IsActual { get; set; }
        public string Street { get; set; }
        public Nullable<int> IdComplex { get; set; }
        public string ComplexName { get; internal set; }
        public string Status { get; internal set; }
    }
}
