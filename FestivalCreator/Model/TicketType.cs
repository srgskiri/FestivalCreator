using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalCreator.Model
{
    class TicketType
    {
        public string ID { get; set; }
        public string Naam { get; set; }
        public double Prijs { get; set; }
        public int AantalTickets { get; set; }
    }
}
