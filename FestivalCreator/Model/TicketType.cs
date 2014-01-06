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
    class TicketType
    {
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
            set { _naam = value; }
        }

        private string _prijs;

        public string Prijs
        {
            get { return _prijs; }
            set { _prijs = value; }
        }

        private int _aantalTickets;

        public int AantalTickets
        {
            get { return _aantalTickets; }
            set { _aantalTickets = value; }
        }

        public static ObservableCollection<TicketType> GetTicketTypes()
        {
            ObservableCollection<TicketType> ticketTypes = new ObservableCollection<TicketType>();


            DbDataReader reader = Database.GetData("SELECT * FROM tblTicketType");

            while (reader.Read())
            {
                TicketType ticketType = new TicketType
                                        {
                                            ID = reader["ID"].ToString(),
                                            Naam = reader["Name"].ToString(),
                                            Prijs = reader["Price"].ToString(),
                                            AantalTickets = Convert.ToInt32(reader["AvailableTickets"])
                                        };
                ticketTypes.Add(ticketType);
            }

            reader.Close();
            return ticketTypes;
        }

        public static TicketType GetTicketTypeById(string id)
        {
            TicketType ticketType = new TicketType();

            string sql = "SELECT * FROM tblTicketType WHERE id=@id";
            DbParameter par1 = Database.AddParameter("@id", id);

            DbDataReader reader = Database.GetData(sql, par1);

            if (reader != null)
            {
                reader.Read();
                ticketType.ID = id;
                ticketType.Naam = reader["Name"].ToString();
                ticketType.Prijs = reader["Price"].ToString();
            }

            return ticketType;
        }


        public static void UpdateTicketTypes(ObservableCollection<TicketType> ticketTypes, List<TicketType> omTeVerwijderen)
        {
            if (ticketTypes.Count >= 1)
                foreach (TicketType ticketType in ticketTypes)
                    if (ticketType.ID.Equals("-1"))
                        InsertTicketType(ticketType);
                    else
                        UpdateTicketType(ticketType);

            if (omTeVerwijderen.Count >= 1)
                foreach (TicketType ticketType in omTeVerwijderen)
                    DeleteTicketType(ticketType);
        }

        private static void UpdateTicketType(TicketType ticketType)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "UPDATE tblTicketType SET name=@naam, price=@prijs, availabletickets=@aantal WHERE id=@id";
                DbParameter par1 = Database.AddParameter("@naam", ticketType.Naam);
                DbParameter par2 = Database.AddParameter("@prijs", ticketType.Prijs);
                DbParameter par3 = Database.AddParameter("@aantal", ticketType.AantalTickets.ToString());
                DbParameter par4 = Database.AddParameter("@id", ticketType.ID);

                rowsaffected += Database.ModifyData(sql, par1, par2, par3, par4);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het wijzigen van het Band info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De TICKET-TYPE record is niet gewijzigd geweest. (" + ticketType.Naam + ")");
        }

        private static void DeleteTicketType(TicketType ticketType)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "DELETE FROM tblTicketType WHERE id=@id";
                DbParameter par1 = Database.AddParameter("@id", ticketType.ID);

                rowsaffected += Database.ModifyData(sql, par1);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het verwijderen van het TICKET-TYPE info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De TICKET-TYPE record is niet verwijderd geweest. (" + ticketType.Naam + ")");
        }

        private static void InsertTicketType(TicketType ticketType)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "INSERT INTO tblTicketType VALUES(@naam, @prijs, @aantal)";
                DbParameter par1 = Database.AddParameter("@naam", ticketType.Naam);
                DbParameter par2 = Database.AddParameter("@prijs", ticketType.Prijs);
                DbParameter par3 = Database.AddParameter("@aantal", ticketType.AantalTickets.ToString());

                rowsaffected += Database.ModifyData(sql, par1, par2, par3);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het opslaan van het TICKET-TYPE info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De TICKET-TYPE record is niet opgeslagen geweest. (" + ticketType.Naam + ")");
        }

        public override string ToString()
        {
            return this.Naam;
        }
    }
}
