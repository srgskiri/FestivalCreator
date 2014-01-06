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
    class ContactpersoonTypeFormVM:ObservableObject, IPage
    {
        #region "props"

        private ObservableCollection<ContactpersoonType> _contactpersoonTypes;

        public ObservableCollection<ContactpersoonType> ContactpersoonTypes
        {
            get { return _contactpersoonTypes; }
            set { _contactpersoonTypes = value; OnPropertyChanged("ContactpersoonTypes"); }
        }

        private ContactpersoonType _selectedContactpersoonType;

        public ContactpersoonType SelectedContactpersoonType
        {
            get { return _selectedContactpersoonType; }
            set { _selectedContactpersoonType = value; OnPropertyChanged("SelectedContactpersoonType"); }
        }


        private string _naamNieuweContactpersoonType;

        public string NaamNieuweContactpersoonType
        {
            get { return _naamNieuweContactpersoonType; }
            set { _naamNieuweContactpersoonType = value; OnPropertyChanged("NaamNieuweContactpersoonType"); }
        }

        public List<ContactpersoonType> NetAangemaakt { get; set; }
        public List<ContactpersoonType> OmTeVerwijderen { get; set; }

        public string Name
        {
            get { return "Functies"; }
        }

        #endregion

        public ContactpersoonTypeFormVM()
        {
            //lijsten aanmaken
            NetAangemaakt = new List<ContactpersoonType>();
            OmTeVerwijderen = new List<ContactpersoonType>();

            //contactpersoontypes opvullen in lijst
            ContactpersoonTypes = ContactpersoonType.GetContactpersoonTypes();
        }

        public ICommand AddContactpersonTypeCommand
        {
            get { return new RelayCommand(AddContactpersonType); }
        }


        private void AddContactpersonType()
        {
            //de nieuwe functie toevoegen
            SelectedContactpersoonType = new ContactpersoonType { ID = "-1", Naam = NaamNieuweContactpersoonType };
            
            //de functie-object in de 2 lijsten steken, 
            //1) de zichtbare lijst          
            //2) de lijst dat bijhoudt wat hij in de Database moet steken
            ContactpersoonTypes.Add(SelectedContactpersoonType);
            NetAangemaakt.Add(SelectedContactpersoonType);

            //textbox met de naam leegmaken
            NaamNieuweContactpersoonType = "";
        }

        public ICommand DeleteContactpersonTypeCommand
        {
            get { return new RelayCommand(DeleteContactpersonType); }
        }


        private void DeleteContactpersonType()
        {
            //verwijderen van het zichtbare lijst
            ContactpersoonType selectedContactpersoonType = SelectedContactpersoonType;
            ContactpersoonTypes.Remove(selectedContactpersoonType);

            //als deze net aangemaakt werd, verwijder deze uit NETAANGEMAAKT-lijst, 
            //omdat ze nog niet uit database moet verwijderd worden
            if(NetAangemaakt.Contains(selectedContactpersoonType))
            {
                NetAangemaakt.Remove(selectedContactpersoonType);
                return;
            }

            //als deze al in database zit, zal deze dan verwijderd worden bij het opslaan
            OmTeVerwijderen.Add(selectedContactpersoonType);
            
        }

        public ICommand SaveChangesCommand
        {
            get { return new RelayCommand(SaveChanges); }
        }


        private void SaveChanges()
        {
            ContactpersoonType.UpdateContactpersoonTypes(NetAangemaakt, OmTeVerwijderen);
        }
    }
}
