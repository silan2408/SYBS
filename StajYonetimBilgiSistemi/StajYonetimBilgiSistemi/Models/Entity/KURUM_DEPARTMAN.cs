//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StajYonetimBilgiSistemi.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class KURUM_DEPARTMAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KURUM_DEPARTMAN()
        {
            this.Kullanicilar = new HashSet<Kullanicilar>();
            this.KURUM_PERSONEL = new HashSet<KURUM_PERSONEL>();
            this.STAJYER_TANIM = new HashSet<STAJYER_TANIM>();
        }
    
        public int PK_KURUM_DEPARTMAN { get; set; }
        public int FK_KURUM_TANIM { get; set; }
        public string ADI { get; set; }
        public Nullable<int> AKTIF { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kullanicilar> Kullanicilar { get; set; }
        public virtual KURUM_TANIM KURUM_TANIM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KURUM_PERSONEL> KURUM_PERSONEL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STAJYER_TANIM> STAJYER_TANIM { get; set; }
    }
}
