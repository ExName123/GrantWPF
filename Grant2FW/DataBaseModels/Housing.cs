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
    
    public partial class Housing
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Housing()
        {
            this.HousingCopies = new HashSet<HousingCopies>();
        }
    
        public int Id { get; set; }
        public string Street { get; set; }
        public Nullable<int> IdHousingComplex { get; set; }
    
        public virtual HousingComplex HousingComplex { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HousingCopies> HousingCopies { get; set; }
    }
}
