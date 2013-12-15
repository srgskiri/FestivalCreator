using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FestivalCreator.Model
{
    class Festival
    {
        private string _naam;

        public string Naam
        {
            get { return _naam; }
            set { _naam = value.Trim(); }
        }

        private DateTime _beginDatum;

        public DateTime BeginDatum
        {
            get { return _beginDatum; }
            set { _beginDatum = value; }
        }

        private string _aantalDagen;

        public string AantalDagen
        {
            get { return _aantalDagen; }
            set { _aantalDagen = value.Trim(); }
        }


        public static Festival GetFestival()
        {
            Festival festival = new Festival();

            //data opvragen met SQL en omzetten naar een RECORD
            DbDataReader reader = Database.GetData("SELECT * FROM tblFestival");
            reader.Read();

            //properties invullen uit de RECORD
            festival.Naam = reader["Name"].ToString();
            festival.AantalDagen = reader["Duration"].ToString();

            //de STRING omzetten naar DATETIME
            string[] stukken = reader["StartDate"].ToString().Split('-'); //dag-maand-jaar
            festival.BeginDatum = new DateTime(Convert.ToInt16(stukken[2]), Convert.ToInt16(stukken[1]), Convert.ToInt16(stukken[0]));

            reader.Close();
            return festival;
        }

        public static void UpdateFestival(Festival festival)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "UPDATE tblfestival SET StartDate=@BeginDatum, Duration=@AantalDagen, Name=@NaamFestival";
                DbParameter par1 = Database.AddParameter("@BeginDatum", festival.BeginDatum.Day + "-" + festival.BeginDatum.Month + "-" + festival.BeginDatum.Year);
                DbParameter par2 = Database.AddParameter("@AantalDagen", festival.AantalDagen);
                DbParameter par3 = Database.AddParameter("@NaamFestival", festival.Naam);

                rowsaffected += Database.ModifyData(sql, par1, par2, par3);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het opslaan van het FESTIVAL info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De FESTIVAL record is niet toegevoegd geweest.");
        }
    }
}
