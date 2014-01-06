using FestivalCreator.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FestivalCreator.ViewModel
{
    class BandFormVM:ObservableObject, IPage
    {
        #region "props"
        private ObservableCollection<Band> _bands;

        public ObservableCollection<Band> Bands
        {
            get { return _bands; }
            set { _bands = value; }
        }


        private Band _selectedBand;

        public Band SelectedBand
        {
            get { return _selectedBand; }
            set 
            { 
                _selectedBand = value;
                ImagePath = value.ImagePath;
                ToggleCheckBoxes(value.Genres);
                OnPropertyChanged("SelectedBand"); 
            }
        }


        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set 
            { 
                _imagePath = value;
                SelectedBand.ImagePath = value;
                SetImage(value);
                OnPropertyChanged("ImagePath");
            }
        }

        private BitmapImage _image;

        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("Image"); }
        }

        //genres
        private ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; }
        }

        
        //naam v/d form
        public string Name
        {
            get { return "Band info"; }
        }

        #endregion 

        public BandFormVM()
        {
            //lijst Genres opvullen
            Genres = Genre.GetGenres();
        }

        public void SetImage(string imagePath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory+"../../Images/";
            Uri uri = new Uri(basePath+imagePath+"");

            //kijken of de gegeven foto bestaat
            if (File.Exists(uri.OriginalString))
                Image = new BitmapImage(uri);
            //indien niet wordt de 'no image' foto gebruikt
            else
                Image = new BitmapImage(new Uri(basePath+"GeenAfb.jpg"));
        }


        private void ToggleCheckBoxes(ObservableCollection<Genre> genresVanBand)
        {
            if (genresVanBand != null)
            {
                if (genresVanBand.Count != 0)
                {
                    foreach (Genre genre in Genres)
                    {

                        foreach (Genre genreVanBand in genresVanBand)
                            if (genre.Equals(genreVanBand))
                            {
                                genre.IsChecked = true;
                                break;
                            }
                            else
                                genre.IsChecked = false;
                    }
                }
                else
                {
                    foreach (Genre genre in Genres)
                        genre.IsChecked = false;
                }
                    
                        
            }
            

        }

        public ICommand SaveChangesCommand
        {
            get { return new RelayCommand(SaveChanges); }
        }

        private void SaveChanges()
        {
            Band.UpdateBands(Bands);
        }

        public ICommand CheckBoxChangedCommand
        {
            get { return new RelayCommand<string>(CheckBoxChanged); }
        }

        private void CheckBoxChanged(string idGenre)
        {
            ToggleGenreInBand(idGenre);
        }

        private void ToggleGenreInBand(string idGenre)
        {
            if(SelectedBand!=null)
            {
                Genre genre = Genre.GetGenresById(idGenre)[0];

                if (SelectedBand.Genres.Contains(genre))
                    SelectedBand.Genres.Remove(genre);
                else
                    SelectedBand.Genres.Add(genre);
            }
        }
    }
}
