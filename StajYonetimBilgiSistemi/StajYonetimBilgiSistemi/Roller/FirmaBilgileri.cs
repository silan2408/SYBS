using StajYonetimBilgiSistemi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StajYonetimBilgiSistemi.Roller
{
    public class FirmaBilgileri
    {
        public IEnumerable<KURUM_PERSONEL> kurum_personel { get; set; }
        public IEnumerable<KURUM_TANIM> kurum_tanım { get; set; }
    }
}