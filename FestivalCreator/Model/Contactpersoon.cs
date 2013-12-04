using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalCreator.Model
{
    class Contactpersoon
    {
        public string ID { get; set; }
        public string Naam { get; set; }
        public string Bedrijf { get; set; }
        public ContactpersoonType Functie { get; set; }
        public string Stad { get; set; }
        public string Email { get; set; }
        public string Telefoon { get; set; }
        public string GSM { get; set; }
    }
}
