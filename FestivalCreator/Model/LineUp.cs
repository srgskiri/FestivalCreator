using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalCreator.Model
{
    class LineUp
    {
        public string ID { get; set; }
        public DateTime Datum { get; set; }
        public string Van { get; set; }
        public string Tot { get; set; }
        public Podium Podium { get; set; }
        public Band Band { get; set; }
    }
}
