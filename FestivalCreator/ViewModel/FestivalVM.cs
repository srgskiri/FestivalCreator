using FestivalCreator.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FestivalCreator.ViewModel
{
    class FestivalVM:ObservableObject, IPage
    {
        #region "props"

        //Festival properties
        private Festival _festival;

        public Festival FestivalObj
        {
            get { return _festival; }
            set { _festival = value; OnPropertyChanged("Festival"); }
        }


        //Podium properties
        private ObservableCollection<Podium> _podia;

        public ObservableCollection<Podium> Podia
        {
            get { return _podia; }
            set { _podia = value; OnPropertyChanged("Podia"); }
        }

        private Podium _selectedPodium;

        public Podium SelectedPodium
        {
            get { return _selectedPodium; }
            set { _selectedPodium = value; OnPropertyChanged("SelectedPodium"); }
        }

        private string _naamNieuwePodium;

        public string NaamNieuwePodium
        {
            get { return _naamNieuwePodium; }
            set { _naamNieuwePodium = value; OnPropertyChanged("NaamNieuwePodium"); }
        }

        //Naam v/d pagina
        public string Name 
        {
            get { return "Festival"; }
        }

        #endregion

        public FestivalVM()
        {
            //festival info invullen
            FestivalObj = Festival.GetFestival();
      
            //podia info invullen
            Podia = Podium.GetPodia();
        }


        //Nieuwe podium toevoegen
        public ICommand AddPodiumCommand
        {
            get { return new RelayCommand(AddPodium); }
        }

        private void AddPodium()
        {
            Podia.Add(new Podium { ID="-1", Naam = NaamNieuwePodium, Beschrijving="Korte beschrijving" });
            SelectedPodium = Podia.Last();
            NaamNieuwePodium = "";
        }

        //alle veranderingen opslaan
        public ICommand SaveChangesCommand
        {
            get { return new RelayCommand(SaveChanges); }
        }

        private void SaveChanges()
        {
            Festival.UpdateFestival(FestivalObj);
            Podium.UpdatePodia(Podia);
        }
    }
}
