using FestivalCreator.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FestivalCreator.ViewModel
{
    class TicketsVM:ObservableObject, IPage
    {
        #region "props"
        private ObservableCollection<TicketType> _ticketTypes;

        public ObservableCollection<TicketType> TicketTypes
        {
            get { return _ticketTypes; }
            set { _ticketTypes = value; OnPropertyChanged("TicketTypes"); }
        }

        private TicketType _selectedTicketType;

        public TicketType SelectedTicketType
        {
            get { return _selectedTicketType; }
            set { _selectedTicketType = value; OnPropertyChanged("SelectedTicketType"); }
        }

        public List<TicketType> OmTeVerwijderen { get; set; }
        
        
        //props voor tickets
        private ObservableCollection<Ticket> _tickets;

        public ObservableCollection<Ticket> Tickets
        {
            get { return _tickets; }
            set { _tickets = value; OnPropertyChanged("Tickets"); }
        }

        private ObservableCollection<Ticket> _zichtbareTickets;

        public ObservableCollection<Ticket> ZichtbareTickets
        {
            get { return _zichtbareTickets; }
            set { _zichtbareTickets = value; OnPropertyChanged("ZichtbareTickets"); }
        }

        private string _zoekString;

        public string ZoekString
        {
            get { return _zoekString; }
            set 
            { 
                _zoekString = value;

                if (_zoekString.Equals(""))
                    ZichtbareTickets = new ObservableCollection<Ticket>(Tickets);
                else
                    ZichtbareTickets = GetTicketsVolgensZoekopdracht(_zoekString);
                OnPropertyChanged("Zoekstring"); 
            }
        }

        
        private Ticket _selectedTicket;

        public Ticket SelectedTicket
        {
            get { return _selectedTicket; }
            set { _selectedTicket = value; OnPropertyChanged("SelectedTicket"); }
        }

        

        public string Name
        {
            get { return "Tickets"; }
        }

        #endregion

        public TicketsVM()
        {
            TicketTypes = TicketType.GetTicketTypes();

            //lijst aanmaken
            OmTeVerwijderen = new List<TicketType>();

            //tickets lijst aanmaken
            Tickets = Ticket.GetTickets();

            //zoekstring initialiseren
            ZoekString = "";
        }

        private ObservableCollection<Ticket> GetTicketsVolgensZoekopdracht(string zoekString)
        {
            ObservableCollection<Ticket> zichtbareTickets = new ObservableCollection<Ticket>();

            foreach (Ticket ticket in Tickets)
            {
                if (ticket.PersoonInBezitEmail.ToLower().Contains(zoekString.ToLower()) || ticket.TicketType.Naam.ToLower().Contains(zoekString.ToLower()))
                    zichtbareTickets.Add(ticket);
            }

            return zichtbareTickets;
        }

        public ICommand AddTicketTypeCommand
        {
            get { return new RelayCommand(AddTicketType); }
        }


        private void AddTicketType()
        {
            //de nieuwe functie toevoegen
            SelectedTicketType = new TicketType { ID = "-1", Naam = "naam" };

            //de functie-object in de 2 lijsten steken, 
            //1) de zichtbare lijst          
            //2) de lijst dat bijhoudt wat hij in de Database moet steken
            TicketTypes.Add(SelectedTicketType);
        }

        public ICommand DeleteTicketTypeCommand
        {
            get { return new RelayCommand(DeleteTicketType); }
        }

        private void DeleteTicketType()
        {
            //verwijderen van het zichtbare lijst
            TicketType selectedTicketType = SelectedTicketType;
            TicketTypes.Remove(selectedTicketType);

            //als deze al in database zit, zal deze dan verwijderd worden bij het opslaan
            OmTeVerwijderen.Add(selectedTicketType);

        }

        public ICommand SaveTicketTypeChangesCommand
        {
            get { return new RelayCommand(SaveTicketTypeChanges); }
        }


        private void SaveTicketTypeChanges()
        {
            TicketType.UpdateTicketTypes(TicketTypes, OmTeVerwijderen);
        }

        public ICommand AddTicketCommand
        {
            get { return new RelayCommand(AddTicket); }
        }

        private void AddTicket()
        {
            Ticket ticket = new Ticket { ID = "-1", PersoonInBezit = "naam", TicketType = TicketTypes[0] };
            Tickets.Add(ticket);

            if (ZoekString.Equals(""))
                ZichtbareTickets.Add(ticket);
        }

        public ICommand PrintTicketCommand
        {
            get { return new RelayCommand(PrintTicket); } 
        }

        private void PrintTicket()
        {
            if (SelectedTicket != null)
                Ticket.PrintTicket(SelectedTicket);
        }

        public ICommand SaveTicketChangesCommand
        {
            get { return new RelayCommand(SaveTicketChanges); }
        }

        private void SaveTicketChanges()
        {
            Ticket.UpdateTickets(Tickets);
        }
    }
}
