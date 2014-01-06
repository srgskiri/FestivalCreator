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
    class BandsVM:ObservableObject, IPage
    {
        #region "props"

        private ObservableCollection<Band> _bands;

        public ObservableCollection<Band> Bands
        {
            get { return _bands; }
            set 
            { 
                _bands = value;
                BandFormVMObject.Bands = value;
                OnPropertyChanged("Bands"); 
            }
        }

        private ObservableCollection<Band> _zichtbareBands;

        public ObservableCollection<Band> ZichtbareBands
        {
            get { return _zichtbareBands; }
            set { _zichtbareBands = value; OnPropertyChanged("ZichtbareBands"); }
        }


        private Band _selectedBand;

        public Band SelectedBand
        {
            get { return _selectedBand; }
            set 
            { 
                _selectedBand = value;

                if(value != null)
                    BandFormVMObject.SelectedBand = value;

                OnPropertyChanged("SelectedBand"); }
        }

        private string _zoekstring;

        public string ZoekString
        {
            get { return _zoekstring; }
            set 
            { 
                _zoekstring = value;

                if (_zoekstring.Equals(""))
                    ZichtbareBands = new ObservableCollection<Band>(Bands);
                else
                {
                    ZichtbareBands.Clear();
                    ZichtbareBands = GetBandsVolgensZoekopdracht(_zoekstring);
                }

                OnPropertyChanged("ZoekString"); 
            }
        }


        #region "mvvm props"
        //bandinfo VM
        private BandFormVM _bandFormVMObject;

        public BandFormVM BandFormVMObject
        {
            get { return _bandFormVMObject; }
            set { _bandFormVMObject = value; }
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
            get { return "Bands"; }
        }

        #endregion

        public BandsVM()
        {
            //mvvm props opvullen
            Pages.Add(new BandFormVM());
            Pages.Add(new GenreFormVM());

            CurrentPage = Pages[0];
            BandFormVMObject = (BandFormVM) Pages[0];

            //bands opvullen
            Bands = Band.GetBands();

            //zoekstring initialiseren
            ZoekString = "";
        }

        private ObservableCollection<Band> GetBandsVolgensZoekopdracht(string zoekString)
        {
            ObservableCollection<Band> zichtbareBands = new ObservableCollection<Band>();

            foreach (Band band in Bands)
            {
                if (band.Naam.ToLower().Contains(zoekString.ToLower()))
                    zichtbareBands.Add(band);
            }

            return zichtbareBands;
        }

        public ICommand AddBandCommand
        {
            get { return new RelayCommand(AddBand); }
        }

        private void AddBand()
        {
            Band band = new Band 
                        { ID = "-1",
                            Naam = "Naam",
                            ImagePath = ".jpg",
                            Twitter = "twitter.com/",
                            Facebook = "facebook.com/",
                            Genres = new ObservableCollection<Genre>()
                        };

            Bands.Add(band);
            SelectedBand = Bands.Last();

            if (ZoekString.Equals(""))
                ZichtbareBands.Add(band);
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
