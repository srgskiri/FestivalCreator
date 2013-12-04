using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalCreator.Model
{
    class Ticket
    {
        public string ID { get; set; }
        public string PersoonInBezit { get; set; }
        public string PersoonInBezitEmail { get; set; }
        public TicketType TicketType { get; set; }
        public int Aantal { get; set; }
    }
}
