using FestivalCreator.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FestivalCreator.ViewModel
{
    class ContactpersonenVM:ObservableObject, IPage
    {
        #region "props"

        //contactpersonen info
        private ObservableCollection<Contactpersoon> _contactpersonen;

        public ObservableCollection<Contactpersoon> Contactpersonen
        {
            get { return _contactpersonen; }
            set 
            { 
                _contactpersonen = value;
                ContactpersoonFormVMObject.Contactpersonen = value;
                OnPropertyChanged("Contactpersonen");
            }
        }

        private ObservableCollection<Contactpersoon> _zichtbareContactpersonen;

        public ObservableCollection<Contactpersoon> ZichtbareContactpersonen
        {
            get { return _zichtbareContactpersonen; }
            set { _zichtbareContactpersonen = value; OnPropertyChanged("ZichtbareContactpersonen"); }
        }

        private Contactpersoon _selectedContactpersoon;

        public Contactpersoon SelectedContactpersoon
        {
            get { return _selectedContactpersoon; }
            set 
            { 
                _selectedContactpersoon = value;
                if (value != null)
                { 
                    _selectedContactpersoon.Weergave = "(" + _selectedContactpersoon.Functie.Naam + ")  " + _selectedContactpersoon.Naam + " " + _selectedContactpersoon.Voornaam;
                    ContactpersoonFormVMObject.SelectedContactpersoon = value;
                }
                
                OnPropertyChanged("SelectedContactpersoon");
            }
        }

        private string _zoekString;

        public string ZoekString
        {
            get { return _zoekString; }
            set 
            { 
                _zoekString = value;

                if (_zoekString.Equals(""))
                {
                    if(ContactpersoonTypesFilter!=null && ContactpersoonTypesFilter.Count != 0)
                        ZichtbareContactpersonen = FilterContactpersonen(new ObservableCollection<Contactpersoon>(Contactpersonen), ContactpersoonTypesFilter);
                    else
                        ZichtbareContactpersonen = new ObservableCollection<Contactpersoon>(Contactpersonen);
                }
                else
                {
                    ZichtbareContactpersonen.Clear();
                    ZichtbareContactpersonen = GetContactpersonenVolgensZoekopdracht(_zoekString);
                }

                OnPropertyChanged("ZoekString");
            }
        }

        //contactpersoon TYPES
        private ObservableCollection<ContactpersoonType> _contactpersoonTypes;

        public ObservableCollection<ContactpersoonType> ContactpersoonTypes
        {
            get { return _contactpersoonTypes; }
            set { _contactpersoonTypes = value; }
        }

        private List<string> _contactpersoonTypesFilter;

        public List<string> ContactpersoonTypesFilter
        {
            get { return _contactpersoonTypesFilter; }
            set { _contactpersoonTypesFilter = value; }
        }

        


        #region "mvvm props"

        //de viewmodel van de contactpersoon formulier (niet de listbox)
        private ContactpersoonFormVM _contactpersoonFormVM;

        public ContactpersoonFormVM ContactpersoonFormVMObject
        {
            get { return _contactpersoonFormVM; }
            set { _contactpersoonFormVM = value; }
        }

        //navigatie props
        private IPage _currentPage;

        public IPage CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        private List<IPage> _pages;

        public List<IPage> Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new List<IPage>();
                return _pages;
            }
        }

        #endregion

        //naam v/d pagina
        public string Name
        {
            get { return "Contactpersonen"; }
        }
        #endregion

        public ContactpersonenVM()
        {
            //mvvm props opvullen
            Pages.Add(new ContactpersoonFormVM());
            Pages.Add(new ContactpersoonTypeFormVM());

            CurrentPage = Pages[0];
            ContactpersoonFormVMObject = (ContactpersoonFormVM) Pages[0];

            //ophalen van contactpersonen
            Contactpersonen = Contactpersoon.GetContactpersonen();

            //zoekstring initialiseren
            ZoekString = "";

            //contactpersoonTypes
            ContactpersoonTypes = ContactpersoonType.GetContactpersoonTypes();
            ContactpersoonTypesFilter = new List<string>();
        }

        private ObservableCollection<Contactpersoon> GetContactpersonenVolgensZoekopdracht(string zoekString)
        {
            ObservableCollection<Contactpersoon> zichtbareContactpersonen = new ObservableCollection<Contactpersoon>();

            foreach (Contactpersoon contactpersoon in Contactpersonen)
            {
                if (contactpersoon.Naam.ToLower().Contains(zoekString.ToLower()) || contactpersoon.Voornaam.ToLower().Contains(zoekString.ToLower()))
                    zichtbareContactpersonen.Add(contactpersoon);
            }

            return zichtbareContactpersonen;
        }

        public ICommand AddContactpersonCommand
        {
            get { return new RelayCommand(AddContactperson); }
        }

        private void AddContactperson()
        {
            Contactpersoon contactpersoon = new Contactpersoon { ID = "-1", Naam = "naam", Voornaam = "voornaam", Functie = ContactpersoonType.GetContactpersoonTypeById("1") };
            Contactpersonen.Add(contactpersoon);
            SelectedContactpersoon = Contactpersonen.Last();

            if (ZoekString.Equals(""))
                ZichtbareContactpersonen.Add(contactpersoon);
        }

        public ICommand CheckBoxChangedCommand
        {
            get { return new RelayCommand<string>(CheckBoxChanged); }
        }

        private void CheckBoxChanged(string naam)
        {
            ToggleFilter(naam);
        }

        private void ToggleFilter(string naam)
        {
            if (ContactpersoonTypesFilter.Contains(naam))
                ContactpersoonTypesFilter.Remove(naam);
            else
                ContactpersoonTypesFilter.Add(naam);

            if (ContactpersoonTypesFilter.Count == 0)
                ZichtbareContactpersonen = GetContactpersonenVolgensZoekopdracht(ZoekString);
            else
                ZichtbareContactpersonen = FilterContactpersonen(GetContactpersonenVolgensZoekopdracht(ZoekString), ContactpersoonTypesFilter);
        }

        private ObservableCollection<Contactpersoon> FilterContactpersonen(ObservableCollection<Contactpersoon> zichtbareContactpersonen, List<string> contactpersoonTypesFilter)
        {
            ObservableCollection<Contactpersoon> gefilterdeContactpersonen = new ObservableCollection<Contactpersoon>();

            foreach (Contactpersoon contactpersoon in zichtbareContactpersonen)
                if (contactpersoonTypesFilter.Contains(contactpersoon.Functie.Naam))
                    gefilterdeContactpersonen.Add(contactpersoon);

            return gefilterdeContactpersonen;
        }

        public ICommand ChangePageItemCommand
        {
            get { return new RelayCommand<IPage>(ChangePageItem); }
        }

        private void ChangePageItem(IPage page)
        {
            CurrentPage = page;
        }
    }
}
