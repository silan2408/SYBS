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
    
    public partial class GUNLUK_CALISMA
    {
        public int PK_GUNLUK_CALISMA { get; set; }
        public int FK_STAJYER_TANIM { get; set; }
        public System.DateTime TARIH { get; set; }
        public string ACIKLAMA { get; set; }
        public Nullable<int> kullaniciadi { get; set; }
    
        public virtual Kullanicilar Kullanicilar { get; set; }
        public virtual STAJYER_TANIM STAJYER_TANIM { get; set; }
    }
}