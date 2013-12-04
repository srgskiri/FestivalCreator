using FestivalCreator.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalCreator.ViewModel
{
    class FestivalVM:ObservableObject, IPage
    {
        public string NaamFestival { get; set; }
        public DateTime BeginDatum { get; set; }
        public string AantalDagen { get; set; }

        public string Name
        {
            get { return "Festival"; }
        }

        private void GetFestivalInfo()
        {
            //data opvragen met SQL en omzetten naar een RECORD
            DbDataReader reader = Database.GetData("SELECT * FROM tblFestival");
            reader.Read();
            IDataRecord record = reader;

            //properties invullen uit de RECORD
            NaamFestival = record["Name"].ToString();
            AantalDagen = record["Duration"].ToString();

            //de STRING omzetten naar DATETIME
            string[] stukken = record["StartDate"].ToString().Split('-'); //dag-maand-jaar
            BeginDatum = new DateTime(Convert.ToInt16(stukken[2]), Convert.ToInt16(stukken[1]), Convert.ToInt16(stukken[0]));
        }

        public FestivalVM()
        {
            GetFestivalInfo();
        }
    }
}
