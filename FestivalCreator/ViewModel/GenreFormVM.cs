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
    class GenreFormVM:ObservableObject, IPage
    {
        #region "props"

        private ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; }
        }

        private Genre _selectedGenre;

        public Genre SelectedGenre
        {
            get { return _selectedGenre; }
            set { _selectedGenre = value; }
        }

        private string _naamNieuweGenre;

        public string NaamNieuweGenre
        {
            get { return _naamNieuweGenre; }
            set { _naamNieuweGenre = value; OnPropertyChanged("NaamNieuweGenre"); }
        }

        public List<Genre> NetAangemaakt { get; set; }
        public List<Genre> OmTeVerwijderen { get; set; }
 
        public string Name
        {
            get { return "Genres"; }
        }

        #endregion

        public GenreFormVM()
        {
            //lijsten aanmaken
            NetAangemaakt = new List<Genre>();
            OmTeVerwijderen = new List<Genre>();

            //contactpersoontypes opvullen in lijst
            Genres = Genre.GetGenres();
        }

        public ICommand AddGenreCommand
        {
            get { return new RelayCommand(AddGenre); }
        }


        private void AddGenre()
        {
            //de nieuwe functie toevoegen
            SelectedGenre = new Genre { ID = "-1", Naam = NaamNieuweGenre };
            
            //de functie-object in de 2 lijsten steken, 
            //1) de zichtbare lijst          
            //2) de lijst dat bijhoudt wat hij in de Database moet steken
            Genres.Add(SelectedGenre);
            NetAangemaakt.Add(SelectedGenre);

            //textbox met de naam leegmaken
            NaamNieuweGenre = "";
        }

        public ICommand DeleteGenreCommand
        {
            get { return new RelayCommand(DeleteGenre); }
        }


        private void DeleteGenre()
        {
            //verwijderen van het zichtbare lijst
            Genre selectedGenre = SelectedGenre;
            Genres.Remove(selectedGenre);

            //als deze net aangemaakt werd, verwijder deze uit NETAANGEMAAKT-lijst, 
            //omdat ze nog niet uit database moet verwijderd worden
            if(NetAangemaakt.Contains(selectedGenre))
            {
                NetAangemaakt.Remove(selectedGenre);
                return;
            }

            //als deze al in database zit, zal deze dan verwijderd worden bij het opslaan
            OmTeVerwijderen.Add(selectedGenre);
            
        }

        public ICommand SaveChangesCommand
        {
            get { return new RelayCommand(SaveChanges); }
        }


        private void SaveChanges()
        {
            Genre.UpdateGenres(NetAangemaakt, OmTeVerwijderen);
        }
    }
}
