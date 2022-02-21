namespace TicketBooking
{
    internal class BookingItem
    {
        static int count = 0;
        public string Name { get; private set; }

        public string Surname { get; private set; }

        public string Phone { get; private set; }

        public int NumberOfSeats{ get; private set; }

        public Guid FilmId { get; private set; }

        public BookingItem(string name, string surname, string phone, Guid filmId, int numberOfseats)
        {
            count++;

            Name = name;
            Surname = surname;
            Phone = phone;
            NumberOfSeats = numberOfseats;
            FilmId = filmId;
            
        }



    }
}
