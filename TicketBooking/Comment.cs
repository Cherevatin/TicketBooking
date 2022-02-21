namespace TicketBooking
{
    internal class Comment
    {
        public Guid FilmId { get; private set; }

        public string AuthorName { get; private set; }

        public double Rating { get; private set; }

        public string Content { get; private set; }

        public Comment(Guid filmId, string authorName, double rating, string content)
        {
            FilmId = filmId;
            AuthorName = authorName;
            Rating = rating;
            Content = content;
        }
    }
}
