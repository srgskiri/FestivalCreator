using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Windows;

namespace FestivalCreator.Model
{
    class Band
    {
        #region
        public string ID { get; set; }

        private string _naam;

        public string Naam
        {
            get { return _naam; }
            set { _naam = value.Trim(); }
        }


        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value.Trim(); }
        }

        private string _beschrijving;

        public string Beschrijving
        {
            get { return _beschrijving; }
            set { _beschrijving = value.Trim(); }
        }

        private string _twitter;

        public string Twitter
        {
            get { return _twitter; }
            set { _twitter = value.Trim(); }
        }

        private string _facebook;

        public string Facebook
        {
            get { return _facebook; }
            set { _facebook = value.Trim(); }
        }

        private ObservableCollection<Genre> _genres;
    
        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; }
        }
        #endregion

        public static ObservableCollection<Band> GetBands()
        {
            ObservableCollection<Band> bands = new ObservableCollection<Band>();

            DbDataReader reader = Database.GetData("SELECT * FROM tblBand");

            while (reader.Read())
            {
                Band band = new Band
                {
                    ID = reader["ID"].ToString(),
                    Naam = reader["Name"].ToString(),
                    ImagePath = reader["Picture"].ToString(),
                    Beschrijving = reader["Description"].ToString(),
                    Twitter = reader["Twitter"].ToString(),
                    Facebook = reader["Facebook"].ToString(),
                    Genres = Genre.GetGenresById(reader["Genres"].ToString().Trim())
                };
             
                bands.Add(band);
            }

            reader.Close();
            return bands;
        }

        public static void UpdateBands(ObservableCollection<Band> bands)
        {
            foreach (Band band in bands)
            {
                if (band.ID.Equals("-1"))
                    InsertBand(band);
                else
                    UpdateBand(band);
            }
        }

        private static void UpdateBand(Band band)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "UPDATE tblBand Set name=@naam, picture=@image, description=@beschrijving, twitter=@twitter, facebook=@facebook, genres=@genres WHERE id=@id";
                DbParameter par1 = Database.AddParameter("@naam", band.Naam);
                DbParameter par2 = Database.AddParameter("@image", band.ImagePath);
                DbParameter par3 = Database.AddParameter("@beschrijving", band.Beschrijving);
                DbParameter par4 = Database.AddParameter("@twitter", band.Twitter);
                DbParameter par5 = Database.AddParameter("@facebook", band.Facebook);
                DbParameter par6 = Database.AddParameter("@genres", Genre.GenresToCSV(band.Genres));
                DbParameter par7 = Database.AddParameter("@id", band.ID);

                rowsaffected += Database.ModifyData(sql, par1, par2, par3, par4, par5, par6, par7);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het wijzigen van het Band info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De CONTACTPERSOON record is niet gewijzigd geweest. (" + band.Naam + ")");
        }

        private static void InsertBand(Band band)
        {
            int rowsaffected = 0;

            try
            {
                string sql = "INSERT INTO tblBand VALUES( @naam, @image, @beschrijving, @twitter, @facebook, @genres )";
                DbParameter par1 = Database.AddParameter("@naam", band.Naam);
                DbParameter par2 = Database.AddParameter("@image", band.ImagePath);
                DbParameter par3 = Database.AddParameter("@beschrijving", band.Beschrijving);
                DbParameter par4 = Database.AddParameter("@twitter", band.Twitter);
                DbParameter par5 = Database.AddParameter("@facebook", band.Facebook);
                DbParameter par6 = Database.AddParameter("@genres", Genre.GenresToCSV(band.Genres));

                rowsaffected += Database.ModifyData(sql, par1, par2, par3, par4, par5, par6);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is iets mis gegaan bij het opslaan van het CONTACTPERSOON info, excuses.");
            }

            if (rowsaffected == 0)
                MessageBox.Show("De CONTACTPERSOON record is niet toegevoegd geweest. (" + band.Naam + ")");
        }
    }
}
