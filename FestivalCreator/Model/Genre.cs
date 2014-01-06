using FestivalCreator.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FestivalCreator.Model
{
    class Genre:ObservableObject
    {
        #region "props"
        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _naam;

        public string Naam
        {
            get { return _naam; }
            set { _naam = value; }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; OnPropertyChanged("IsChecked"); }
        }


        #endregion

        public static ObservableCollection<Genre> GetGenres()
        {
            ObservableCollection<Genre> genres = new ObservableCollection<Genre>();

            DbDataReader reader = Database.GetData("SELECT * FROM tblGenre");

            while (reader.Read())
            {
                Genre genre = new Genre
                {
                    ID = reader["ID"].ToString(),
                    Naam = reader["Name"].ToString(),
                };
                genres.Add(genre);
            }

            reader.Close();
            return genres;
        }

        public static ObservableCollection<Genre> GetGenresById(string genresCSV)
        {
            ObservableCollection<Genre> genres = new ObservableCollection<Genre>();

            Genre genre;

            if (genresCSV.Length < 3)//als er maar 1 genre is
            {
                genre = GetGenreById(genresCSV);
                if(genre!=null)
                    genres.Add(genre);
            }
            else
            {
                string[] stukken = genresCSV.Split(';');

                for (int i = 0; i < stukken.Length; i++)
                {
                    genre = GetGenreById(stukken[i]);
                    if(genre!=null)
                        genres.Add(genre);
                }
                    
            }

            return genres;
        }

        private static Genre GetGenreById(string id)
        {
            Genre genre = new Genre { ID=id, Naam = "Fout bij ophalen" };

            try 
	        {	        
		        string sql = "SELECT * FROM tblGenre WHERE ID=@ID";
                DbParameter par1 = Database.AddParameter("@ID", id);

                DbDataReader reader = Database.GetData(sql, par1);

                reader.Read();
                genre.Naam = reader["Name"].ToString();

                reader.Close();
	        }
	        catch (Exception)
	        {
		        MessageBox.Show("Er is iets mis gegaan bij het ophalen van het GENRE info, excuses. ID: " +id);
                return null;
	        }  

            return genre;
        }

        //////////////////////////////////////////////////////
        public static void UpdateGenres(List<Genre> omAanTeMaken, List<Genre> omTeVerwijderen)
        {
            if (omAanTeMaken.Count >= 1)
                foreach (Genre genre in omAanTeMaken)
                    InsertGenre(genre);

            if (omTeVerwijderen.Count >= 1)
                foreach (Genre genre in omTeVerwijderen)
                    DeleteGenre(genre);

        }

        private static void DeleteGenre(Genre genre)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "DELETE FROM tblGenre WHERE id=@id";
                DbParameter par1 = Database.AddParameter("@id", genre.ID);

                rowsaffected += Database.ModifyData(sql, par1);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het verwijderen van het GENRE info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De GENRE record is niet verwijderd geweest. (" + genre.Naam + ")");
        }

        private static void InsertGenre(Genre genre)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "INSERT INTO tblGenre VALUES(@naam)";
                DbParameter par1 = Database.AddParameter("@naam", genre.Naam);

                rowsaffected += Database.ModifyData(sql, par1);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het opslaan van het GENRE info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De GENRE record is niet opgeslagen geweest. (" + genre.Naam + ")");
        }///////////////////////////////////////////////

        public static string GenresToCSV(ObservableCollection<Genre> genres)
        {
            string genresCSV = "";

            foreach (Genre genre in genres)
            {
                if (genresCSV.Equals(""))
                    genresCSV += genre.ID;
                else
                    genresCSV += ";" + genre.ID;
            }

            return genresCSV;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Genre;

            if (item == null)
            {
                return false;
            }

            return this.Naam.Equals(item.Naam);
        }

        public override string ToString()
        {
            return this.Naam;
        }
    }
}
