using StajYonetimBilgiSistemi.Models.Entity;
using System.Collections.Generic;

namespace StajYonetimBilgiSistemi.Roller
{
    public class FirmaBilgileri
    {
        public IEnumerable<KURUM_PERSONEL> kurum_personel { get; set; }
        public IEnumerable<Kullanicilar> kullanicilar { get; set; }
    }
}