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
    class ContactpersoonFormVM:ObservableObject, IPage
    {
        #region "props"

        private ObservableCollection<Contactpersoon> _contactpersonen;

        public ObservableCollection<Contactpersoon> Contactpersonen
        {
            get { return _contactpersonen; }
            set { _contactpersonen = value; }
        }

        private Contactpersoon _selectedContactpersoon;

        public Contactpersoon SelectedContactpersoon
        {
            get { return _selectedContactpersoon; }
            set
            {
                _selectedContactpersoon = value;
                OnPropertyChanged("SelectedContactpersoon");
            }
        }

        //contactpersoon types info
        private ObservableCollection<ContactpersoonType> _functieTypes;

        public ObservableCollection<ContactpersoonType> FunctieTypes
        {
            get { return _functieTypes; }
            set { _functieTypes = value; }
        }

        //naam van tabblad
        public string Name
        {
            get { return "Contactpersoon"; }
        }

        #endregion

        public ContactpersoonFormVM()
        {
            //ophalen van contactpesoon types
            FunctieTypes = ContactpersoonType.GetContactpersoonTypes();
        }

        public ICommand SaveChangesCommand
        {
            get { return new RelayCommand(SaveChanges); }
        }

        private void SaveChanges()
        {
            Contactpersoon.UpdateContactpersonen(Contactpersonen);
        }
    }
}
