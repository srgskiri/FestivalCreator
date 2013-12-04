using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace FestivalCreator.Model
{
    class Band
    {
        public string ID { get; set; }
        public string Naam { get; set; }
        public string FotoPath { get; set; }
        public string Beschrijving { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public ObservableCollection<Genre> Genres { get; set; }
    }
}
