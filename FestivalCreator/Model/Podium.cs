using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FestivalCreator.Model
{
    class Podium
    {
        public string ID { get; set; }

        private string _naam;

        public string Naam
        {
            get { return _naam; }
            set { _naam = value.Trim(); }
        }

        private string _beschrijving;

        public string Beschrijving
        {
            get { return _beschrijving; }
            set { _beschrijving = value.Trim(); }
        }

        public static ObservableCollection<Podium> GetPodia()
        {
            ObservableCollection<Podium> podia = new ObservableCollection<Podium>();

            //data opvragen
            DbDataReader reader = Database.GetData("SELECT * FROM tblStage");

            while (reader.Read())
            {
                Podium podium = new Podium
                {
                    ID = reader["ID"].ToString(),
                    Naam = reader["Name"].ToString(),
                    Beschrijving = reader["Description"].ToString()
                };
                podia.Add(podium);
            }

            reader.Close();
            return podia;
        }

        public static void UpdatePodia(ObservableCollection<Podium> podia)
        {
            foreach (Podium podium in podia)
            { 
                if(podium.ID.Equals("-1"))
                    InsertPodium(podium);
                else
                    UpdatePodium(podium);
            }
        }

        private static void UpdatePodium(Podium podium)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "UPDATE tblStage SET name=@naam, description=@beschrijving WHERE id=@id";
                DbParameter par1 = Database.AddParameter("@naam",podium.Naam);
                DbParameter par2 = Database.AddParameter("@beschrijving", podium.Beschrijving);
                DbParameter par3 = Database.AddParameter("@id", podium.ID);

                rowsaffected += Database.ModifyData(sql, par1, par2, par3);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het opslaan van het PODIA info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De PODIUM record is niet toegevoegd geweest. ("+ podium.Naam +")");
        }

        private static void InsertPodium(Podium podium)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "INSERT INTO tblStage VALUES(@naam,@beschrijving)";
                DbParameter par1 = Database.AddParameter("@naam", podium.Naam);
                DbParameter par2 = Database.AddParameter("@beschrijving", podium.Beschrijving);

                rowsaffected += Database.ModifyData(sql, par1, par2);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het opslaan van het PODIA info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De PODIUM record is niet toegevoegd geweest. (" + podium.Naam + ")");
        }

    }
}
