namespace TicketBooking
{
    internal class Genre
    {
        public string Type { get; set; }

        public string Description { get; set; }

        public Genre(string type, string description)
        {
            Type = type;
            Description = description;
        }
    }
}
