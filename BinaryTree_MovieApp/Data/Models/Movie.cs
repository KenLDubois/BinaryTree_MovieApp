using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryTree_MovieApp.Data.Models
{
    public class Movie : IBinaryNode<Movie>, IValidatableObject
    {
        private string title;
        private DateTime dateTime = DateTime.Now;
        private int runtime;
        private string director;
        private float rating;

        public Movie Left { get; set; }
        public Movie Right { get; set; }

        public Movie() { }

        public Movie(string _title, DateTime _releaseDate, int _runtime, string _director, float _rating)
        {
            Title = _title;
            ReleaseDate = _releaseDate;
            Runtime = _runtime;
            Director = _director;
            Rating = _rating;
        }

        [Required]
        [Display(Name = "Title")]
        [StringLength(100,ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [Required]
        public DateTime ReleaseDate
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        [Required]
        public int Runtime
        {
            get { return runtime; }
            set { runtime = value; }
        }

        public string Director
        {
            get { return director; }
            set { director = value; }
        }

        [Required]
        [Range(0, 10.0)]
        public float Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        public int CompareTo([AllowNull] Movie other)
        {
            if (ReleaseDate.Date < other.ReleaseDate.Date) return -1;
            if (ReleaseDate.Date > other.ReleaseDate.Date) return 1;
            return Title.ToLower().CompareTo(other.Title.ToLower());
        }

        public bool Equals([AllowNull] Movie other)
        {
            if (other == null) return false;

            return (other.Title == Title
                && other.ReleaseDate.Date == ReleaseDate.Date
                && other.Runtime == Runtime
                && other.Director == Director
                && other.Rating == Rating);
        }

        public override string ToString()
        {
            return $"{Title} ({ReleaseDate.Year})";
        }

        public bool MeetsSearchCriteria(Movie searchTerm)
        {
            if (searchTerm == null) return false;
            return searchTerm.Title.ToLower() == Title.ToLower() && searchTerm.ReleaseDate.Year == ReleaseDate.Year;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReleaseDate > DateTime.Now)
            {
                yield return new ValidationResult(
                    "Release dates cannot be in the future.",
                    new[] { nameof(ReleaseDate) });
            }

            if(Runtime <= 0)
            {
                yield return new ValidationResult(
                    "Runtime must be greater than 0 minues.",
                    new[] { nameof(Runtime) } );
            }
        }
    }
}
