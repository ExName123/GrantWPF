//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Grant2FW.DataBaseModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class HousingComplexCopies
    {
        public int Id { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> IdHousingComplex { get; set; }
        public Nullable<bool> IsActual { get; set; }
    
        public virtual HousingComplex HousingComplex { get; set; }
        public virtual Status Status1 { get; set; }
    }
}
