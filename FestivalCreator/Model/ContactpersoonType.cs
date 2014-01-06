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
    class ContactpersoonType
    {
        private string _id;

        public string ID
        {
            get { return _id; }
            set 
            { 
                _id = value;
                if (Naam != null)
                    Naam = GetContactpersoonTypeById(value).Naam;
            }
        }


        private string _naam;

        public string Naam
        {
            get { return _naam; }
            set { _naam = value.Trim(); }
        }


        public static ContactpersoonType GetContactpersoonTypeById(string id)
        {
            ContactpersoonType contactpersoonType = new ContactpersoonType();
            contactpersoonType.ID = id;
            contactpersoonType.Naam = "Fout bij ophalen";

            try 
	        {	        
		        string sql = "SELECT * FROM tblContactpersonType WHERE ID=@ID";
                DbParameter par1 = Database.AddParameter("@ID", id);

                DbDataReader reader = Database.GetData(sql, par1);

                reader.Read();
                contactpersoonType.Naam = reader["Name"].ToString();

                reader.Close();
	        }
	        catch (Exception)
	        {
		        MessageBox.Show("Er is iets mis gegaan bij het ophalen van het CONTACTPERSON TYPE info, excuses.");
	        }  

            return contactpersoonType;
        }

        public static ObservableCollection<ContactpersoonType> GetContactpersoonTypes()
        {
            ObservableCollection<ContactpersoonType> contactpersoonTypes = new ObservableCollection<ContactpersoonType>();

            string sql = "SELECT * FROM tblContactpersonType";

            DbDataReader reader = Database.GetData(sql);

            while (reader.Read())
            {
                ContactpersoonType contactpersoonType = new ContactpersoonType
                {
                    ID = reader["ID"].ToString(),
                    Naam = reader["Name"].ToString()
                };

                contactpersoonTypes.Add(contactpersoonType);
            }

            reader.Close();
            return contactpersoonTypes;
        }

        public static void UpdateContactpersoonTypes(List<ContactpersoonType> omAanTeMaken, List<ContactpersoonType> omTeVerwijderen)
        {
            if(omAanTeMaken.Count>=1)
                foreach (ContactpersoonType contactpersoonType1 in omAanTeMaken)
                    InsertContactpersoonType(contactpersoonType1);
                
            if(omTeVerwijderen.Count>=1)
                foreach (ContactpersoonType contactpersoonType2 in omTeVerwijderen)
                    DeleteContactpersoonType(contactpersoonType2);
            
        }

        private static void DeleteContactpersoonType(ContactpersoonType contactpersoonType)
        {
            

            int rowsaffected = 0;

            try
            {
                string sql = "DELETE FROM tblContactpersonType WHERE id=@id"; 
                DbParameter par1 = Database.AddParameter("@id", contactpersoonType.ID);

                rowsaffected += Database.ModifyData(sql, par1);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het verwijderen van het CONTACTPERSOON-TYPE info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De CONTACTPERSOON-TYPE record is niet verwijderd geweest. (" + contactpersoonType.Naam + ")");
        }

        private static void InsertContactpersoonType(ContactpersoonType contactpersoonType)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "INSERT INTO tblContactpersonType VALUES(@naam)";
                DbParameter par1 = Database.AddParameter("@naam", contactpersoonType.Naam);

                rowsaffected += Database.ModifyData(sql, par1);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het opslaan van het CONTACTPERSOON-TYPE info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De CONTACTPERSOON-TYPE record is niet opgeslagen geweest. (" + contactpersoonType.Naam + ")");
        }

        public override string ToString()
        {
            return this.Naam;
        }
    }
}
