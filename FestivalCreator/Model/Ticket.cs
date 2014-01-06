using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalCreator.Model
{
    class Ticket
    {
        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value.Trim(); }
        }

        private string _persoonInBezit;

        public string PersoonInBezit
        {
            get { return _persoonInBezit; }
            set { _persoonInBezit = value.Trim(); }
        }

        private string _persoonInBezitEmail;

        public string PersoonInBezitEmail
        {
            get { return _persoonInBezitEmail; }
            set { _persoonInBezitEmail = value.Trim(); }
        }

        private TicketType _ticketType;

        public TicketType TicketType
        {
            get { return _ticketType; }
            set { _ticketType = value; }
        }

        private int _aantal;

        public int Aantal
        {
            get { return _aantal; }
            set { _aantal = value; }
        }


        public static ObservableCollection<Ticket> GetTickets()
        {
            ObservableCollection<Ticket> tickets = new ObservableCollection<Ticket>();

            DbDataReader reader = Database.GetData("SELECT * FROM tblTicket");

            while (reader.Read())
            {
                Ticket ticket = new Ticket
                                {
                                    ID = reader["ID"].ToString(),
                                    PersoonInBezit = reader["TicketHolder"].ToString(),
                                    PersoonInBezitEmail = reader["TicketHolderEmail"].ToString(),
                                    TicketType = TicketType.GetTicketTypeById(reader["TicketType"].ToString()),
                                    Aantal = Convert.ToInt32(reader["Amount"])
                                };

                tickets.Add(ticket);
            }

            reader.Close();
            return tickets;
        }

        public static void UpdateTickets(ObservableCollection<Ticket> tickets)
        {
            foreach (Ticket ticket in tickets)
            {
                if (ticket.ID.Equals("-1"))
                    InsertTicket(ticket);
                else
                    UpdateTicket(ticket);
            }
        }

        private static void InsertTicket(Ticket ticket)
        {
            try
            {
                string sql = "INSERT INTO tblTicket VALUES( @persooninbezit, @persooninbezitemail, @tickettype, @aantal )";
                DbParameter par1 = Database.AddParameter("@persooninbezit", ticket.PersoonInBezit);
                DbParameter par2 = Database.AddParameter("@persooninbezitemail", ticket.PersoonInBezitEmail);
                DbParameter par3 = Database.AddParameter("@tickettype", ticket.TicketType.ID);
                DbParameter par4 = Database.AddParameter("@aantal", ticket.Aantal.ToString());

                Database.ModifyData(sql, par1, par2, par3, par4);
            }
            catch (Exception)
            {
                Console.WriteLine("Toevoegen niet gelukt " + ticket.PersoonInBezit);
            }
        }

        private static void UpdateTicket(Ticket ticket)
        {
            try
            {
                string sql = "UPDATE tblTicket SET ticketholder=@persooninbezit, ticketholderemail=@persooninbezitemail, tickettype=@tickettype, amount=@aantal WHERE id=@id";
                DbParameter par1 = Database.AddParameter("@persooninbezit", ticket.PersoonInBezit);
                DbParameter par2 = Database.AddParameter("@persooninbezitemail", ticket.PersoonInBezitEmail);
                DbParameter par3 = Database.AddParameter("@tickettype", ticket.TicketType.ID);
                DbParameter par4 = Database.AddParameter("@aantal", ticket.Aantal.ToString());
                DbParameter par5 = Database.AddParameter("@id", ticket.ID);

                Database.ModifyData(sql, par1, par2, par3, par4, par5);
            }
            catch (Exception)
            {
                Console.WriteLine("Wijzigen iet gelukt "+ticket.PersoonInBezit);
            }
                
        }

        public static void PrintTicket(Ticket ticket)
        {
            string file = ticket.PersoonInBezit + "-ticket.docx";
            File.Copy("temp.docx", file, true);

            WordprocessingDocument doc = WordprocessingDocument.Open(file, true);
            IDictionary<string, BookmarkStart> bookmarks = new Dictionary<string, BookmarkStart>();

            foreach (BookmarkStart bms in doc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
            {
                bookmarks[bms.Name] = bms;
            }

            bookmarks["TicketHolder"].Parent.InsertAfter<Run>(new Run(new Text(ticket.PersoonInBezit)), bookmarks["TicketHolder"]);
            bookmarks["TicketHolderEmail"].Parent.InsertAfter<Run>(new Run(new Text(ticket.PersoonInBezitEmail)), bookmarks["TicketHolderEmail"]);
            bookmarks["TicketType"].Parent.InsertAfter<Run>(new Run(new Text(ticket.TicketType.Naam)), bookmarks["TicketType"]);
            bookmarks["Amount"].Parent.InsertAfter<Run>(new Run(new Text(ticket.Aantal.ToString())), bookmarks["Amount"]);

            doc.Close();
        }
    }
}
