using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FestivalCreator.ViewModel
{
    class ApplicationVM:ObservableObject
    {
        #region "properties"
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

        public ApplicationVM()
        {
            Pages.Add(new FestivalVM());
            Pages.Add(new ContactpersonenVM());
            Pages.Add(new BandsVM());
            Pages.Add(new LineUpVM());
            Pages.Add(new TicketsVM());

            CurrentPage = Pages[0];
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        private void ChangePage(IPage page)
        {
            CurrentPage = page;
        }  
    }
}
