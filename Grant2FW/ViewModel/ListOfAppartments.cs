using Grant2FW.DataBaseModels;
using Grant2FW.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Grant2FW.ViewModel
{/// <summary>
 /// Класс для получения данных из edmx и записывание их в ObservableCollection для работы с сущшностями 'квартиры'
 /// getList() получает список посредством  запроса к edmx
 /// </summary>
    internal class ListOfAppartments
    {

        public static ObservableCollection<ElementAppartment> appartmentsCollection
        {
            get; set;
        }
        static public ObservableCollection<ElementAppartment> getList()
        {
            appartmentsCollection = new ObservableCollection<ElementAppartment>();

            var result = from apartment in DataBase.ClassDataBase.dataobj.Appartments
                         join apartmentCopy in DataBase.ClassDataBase.dataobj.ApartmentsCopy on apartment.Id equals apartmentCopy.IdApartment
                         join housing in DataBase.ClassDataBase.dataobj.Housing on apartment.IdHouse equals housing.Id
                         join housingCopies in DataBase.ClassDataBase.dataobj.HousingCopies on housing.Id equals housingCopies.OriginalHousingId
                         join status in DataBase.ClassDataBase.dataobj.StatusesAppartment on apartmentCopy.IdStatus equals status.Id
                         join complex in DataBase.ClassDataBase.dataobj.HousingComplex on housing.IdHousingComplex equals complex.Id
                         where apartmentCopy.IsActual == true && housingCopies.IsActual == true
                         select new ElementAppartment()
                         {
                             IdAppartment = (int)apartmentCopy.IdApartment,
                             IdComplex = complex.Id,
                             IdCopyComplex = (int)housingCopies.IdComplex,
                             IdCopyHouse = housingCopies.Id,
                             IdCopy = apartmentCopy.Id,
                             Address = apartmentCopy.Address,
                             Area = (decimal)apartmentCopy.Area,
                             NumberRooms = apartmentCopy.NumberOfRooms,
                             Section = apartmentCopy.Section,
                             Floor = apartmentCopy.Floor,
                             Status = status.Name,
                             NameComplex = complex.Name,
                             AddedValue = (int)apartmentCopy.Added_value,
                             ExpensesBuildingAnApartment = (int)apartmentCopy.Expenses_Building_An_Apartment,
                             Street = housingCopies.Street,
                             NumberHouse = housingCopies.Number_House,
                             NumberRoom = apartmentCopy.Address
                         };


            foreach (var item in result)
            {
                var tempAddress = item.Address;
                item.Address = $"ул.{item.Street} д.{item.NumberHouse} кв.{tempAddress}";
                appartmentsCollection.Add(item);
            }
            return appartmentsCollection;
        }
    }
    /// <summary>
    /// класс сущности 'квартира'
    /// </summary>
    public class ElementAppartment
    {
        public string NumberRoom {  get; set; } 
        public int IdCopy { get; set; }
        public int IdAppartment {  get; set; }
        public string Address { get; set; }
        public Decimal Area { get; internal set; }
        public Nullable<int> NumberRooms { get; set; }
        public Nullable<int> Section { get; set; }
        public Nullable<int> Floor { get; set; }
        public string Status { get; set; }
        public string NameComplex { get; set; }
        public int AddedValue {  get; set; }    
        public string NumberHouse { get; set; }
        public int IdCopyComplex { get; set; }
        public int IdCopyHouse { get; set; }
        public int IdComplex { get; set; }
        public int ExpensesBuildingAnApartment { get; set; } 
        public Element elementHouse { get; set; }
        public ElementComplex elementComplex { get; set; }
        public string Street { get; internal set; }
    }
}
