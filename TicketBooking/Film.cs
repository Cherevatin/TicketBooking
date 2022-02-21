using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking
{
    internal class Film
    {
        public Guid Id { get; private set; }

        public string Title { get; private set; }

        public string Genre { get; private set; }

        public double Rating { get; private set; }

        public int FreeSeats { get; private set; }

        public List<Comment> Comments { get; private set; }

        public Film(Guid id, string title, int freeSeats, string genre, double rating = 0)
        {
            Id = id;
            Title = title;
            FreeSeats = freeSeats;
            Genre = genre;
            Rating = rating;

            Comments = new();

        }

        public void BookSeats(int seats)
        {
            FreeSeats -= seats;
        }

        public void UnbookSeats(int seats)
        {
            FreeSeats += seats;
        }

        public void AddComment(Guid filmId, string authorName, double rating, string content)
        {
            Comment comment = new(filmId,authorName, rating, content);
            Comments.Add(comment);

            RefreshRating();
        }

        public void RefreshRating()
        {
            if (Comments.Any())
            {
                Rating = Comments.Sum(f => f.Rating) / Comments.Count;
            }
        }

        public List<Comment> GetComments()
        {
            return Comments;
        }
    }
}
