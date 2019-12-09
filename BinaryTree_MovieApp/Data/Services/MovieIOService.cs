using BinaryTree_MovieApp.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryTree_MovieApp.Data.Services
{
    public class MovieIOService
    {
        public List<Movie> LoadMoviesFromMemoryStream(MemoryStream ms)
        {
            List<Movie> movies = new List<Movie>();

            try
            {
                List<string> movieStrings = new List<string>();

                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    while (!sr.EndOfStream)
                    {
                        movieStrings.Add(sr.ReadLine());
                    }
                }

                foreach(string movieString in movieStrings)
                {
                    Movie movie = PaseStringIntoMovie(movieString);

                    if (movie != null) movies.Add(movie);
                }

            }
            catch { }

            return movies;
        }

        public Movie PaseStringIntoMovie(string movieString)
        {
            try
            {
                string[] movieProps = movieString.Split(';');

                if (movieProps.Length < 5) return null;

                string title;
                DateTime releaseDate;
                int runTime;
                string director;
                float rating;

                title = movieProps[0];
                if (!DateTime.TryParse(movieProps[1], out releaseDate)) return null;
                if (!int.TryParse(movieProps[2], out runTime)) return null;
                director = movieProps[3];
                if (!float.TryParse(movieProps[4], out rating)) return null;

                return new Movie(title,releaseDate,runTime,director,rating);

            }
            catch { return null; }
        }
    }
}
