using FestivalCreator.ViewModel;
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
    class Contactpersoon:ObservableObject
    {
        #region "props"

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _naam;

        public string Naam
        {
            get { return _naam; }
            set 
            { 
                _naam = value;
                if(Weergave != null && Functie != null && Voornaam != null)
                    Weergave = "(" + Functie.Naam + ")  " + value + " " + Voornaam; 
            }
        }

        private string _voornaam;

        public string Voornaam
        {
            get { return _voornaam; }
            set
            {
                _voornaam = value;
                if (Weergave != null && Functie != null)
                    Weergave = "(" + Functie.Naam + ")  " + Naam + " " + value;
            }
        }

        private string _bedrijf;

        public string Bedrijf
        {
            get { return _bedrijf; }
            set { _bedrijf = value.Trim(); }
        }

        private ContactpersoonType _functie;

        public ContactpersoonType Functie
        {
            get { return _functie; }
            set
            {
                _functie = value;
                if (Weergave != null)
                    Weergave = "(" + value.Naam + ")  " + Naam + " " + Voornaam;
            }
        }

        private string _stad;

        public string Stad
        {
            get { return _stad; }
            set { _stad = value.Trim(); }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value.Trim(); }
        }

        private string _telefoon;

        public string Telefoon
        {
            get { return _telefoon; }
            set { _telefoon = value.Trim(); }
        }

        private string _gsm;

        public string GSM
        {
            get { return _gsm; }
            set { _gsm = value.Trim(); }
        }

        private string _weergave;

        public string Weergave
        {
            get { return _weergave; }
            set { _weergave = value; OnPropertyChanged("Weergave"); }
        }


        #endregion

        public static ObservableCollection<Contactpersoon> GetContactpersonen()
        {
            ObservableCollection<Contactpersoon> contactpersonen = new ObservableCollection<Contactpersoon>();

            //data opvragen
            DbDataReader reader = Database.GetData("SELECT * FROM tblPerson");

            while (reader.Read())
            {
                Contactpersoon contactpersoon = new Contactpersoon
                {
                    ID = reader["ID"].ToString(),
                    Naam = GetNaam(reader["Name"].ToString(), 0),
                    Voornaam = GetNaam(reader["Name"].ToString(), 1),
                    Bedrijf = reader["Company"].ToString(),
                    Functie = ContactpersoonType.GetContactpersoonTypeById(reader["JobRole"].ToString()),
                    Stad = reader["City"].ToString(),
                    Email = reader["Email"].ToString(),
                    Telefoon = reader["Phone"].ToString(),
                    GSM = reader["Cellphone"].ToString()
                };
                contactpersoon.Weergave = "(" + contactpersoon.Functie.Naam + ")  " + contactpersoon.Naam + " " + contactpersoon.Voornaam;
                contactpersonen.Add(contactpersoon);
            }

            reader.Close();
            return contactpersonen;
        }

        public static void UpdateContactpersonen(ObservableCollection<Contactpersoon> contactpersonen)
        {
            foreach (Contactpersoon contactpersoon in contactpersonen)
            {
                if (contactpersoon.ID.Equals("-1"))
                    InsertContactpersoon(contactpersoon);
                else
                    UpdateContactpersoon(contactpersoon);
            }
        }

        private static void UpdateContactpersoon(Contactpersoon contactpersoon)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "UPDATE tblPerson SET name=@naam, company=@bedrijf, jobrole=@functie, city=@stad, email=@email, phone=@telefoon, cellphone=@gsm WHERE id=@id";
                DbParameter par1 = Database.AddParameter("@id", contactpersoon.ID);
                DbParameter par2 = Database.AddParameter("@naam", contactpersoon.Naam+";"+contactpersoon.Voornaam);
                DbParameter par3 = Database.AddParameter("@bedrijf", contactpersoon.Bedrijf);
                DbParameter par4 = Database.AddParameter("@functie", contactpersoon.Functie.ID);
                DbParameter par5 = Database.AddParameter("@stad", contactpersoon.Stad);
                DbParameter par6 = Database.AddParameter("@email", contactpersoon.Email);
                DbParameter par7 = Database.AddParameter("@telefoon", contactpersoon.Telefoon);
                DbParameter par8 = Database.AddParameter("@gsm", contactpersoon.GSM);

                rowsaffected += Database.ModifyData(sql, par1, par2, par3, par4, par5, par6, par7, par8);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het opslaan van het CONTACTPERSOON info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De CONTACTPERSOON record is niet gewijzigd geweest. (" + contactpersoon.Naam + " "+ contactpersoon.Voornaam +")");
        }

        private static void InsertContactpersoon(Contactpersoon contactpersoon)
        {
            int rowsaffected = 0; 

            try
            {
                string sql = "INSERT INTO tblPerson VALUES(@naam, @bedrijf, @functie, @stad, @email, @telefoon, @gsm )";
                DbParameter par1 = Database.AddParameter("@naam", contactpersoon.Naam+";"+contactpersoon.Voornaam);
                DbParameter par2 = Database.AddParameter("@bedrijf", contactpersoon.Bedrijf);
                DbParameter par3 = Database.AddParameter("@functie", contactpersoon.Functie.ID);
                DbParameter par4 = Database.AddParameter("@stad", contactpersoon.Stad);
                DbParameter par5 = Database.AddParameter("@email", contactpersoon.Email);
                DbParameter par6 = Database.AddParameter("@telefoon", contactpersoon.Telefoon);
                DbParameter par7 = Database.AddParameter("@gsm", contactpersoon.GSM);

                rowsaffected += Database.ModifyData(sql, par1, par2, par3, par4, par5, par6, par7);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het opslaan van het CONTACTPERSOON info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De CONTACTPERSOON record is niet toegevoegd geweest. (" + contactpersoon.Naam + " "+ contactpersoon.Voornaam +")");
        }

        private static string GetNaam(string volledigeNaam, int index)
        {
            string[] stukken = volledigeNaam.Split(';');

            if (index == 0)
                return stukken[index];

            return stukken[index];
        }
    }
}
