﻿using Grant2FW.DataBaseModels;
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
                         join complex in DataBase.ClassDataBase.dataobj.HousingComplex on housing.IdHousingComplex equals complex.Id
                         join complexCopy in DataBase.ClassDataBase.dataobj.HousingComplexCopies on complex.Id equals complexCopy.IdHousingComplex
                         join status in DataBase.ClassDataBase.dataobj.Status on complexCopy.Status equals status.Id
                         where copies.IsActual == true
                         orderby complex.Name, housing.Street, copies.Number_House
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
                             Street = housing.Street,
                             IdComplex = housing.IdHousingComplex,
                             ComplexName = complex.Name,
                             Status = status.StatusName
                         };

            foreach (var item in result)
            {
                housesCollection.Add(item);
            }



            return housesCollection;
        }
    }

    public class Element
    {
        public int IdCopy { get; set; }
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
