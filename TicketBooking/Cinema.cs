namespace TicketBooking
{
    internal class Cinema 
    {
        public List<Film> Films { get; private set; }

        public List<BookingItem> BookingList { get; private set; }

        public List<BookingItem> UnbookingList { get; private set; }

        public Cinema()
        {
            Films = new();
            BookingList = new();
            UnbookingList = new();
        }
        public void AddFilm(string title, int freeSeats, string genre = "not set")
        {
            Film film = new(Guid.NewGuid(), title, freeSeats, genre);
            Films.Add(film);
        }

        public void AddFilms(List<Film> films)
        {
            Films = films;
        }

        public void AddBookings(List<BookingItem> bookings)
        {
            BookingList = bookings;
        }

        public void AddUnbookings(List<BookingItem> unbookings)
        {
            UnbookingList = unbookings;
        }

        public void UpdateList(List<Film> newList)
        {
            Films = newList;
        }

        public void MakeBooking(string name, string surname, string phone, Guid idFilm, int numberOfseats)
        {
            BookingItem item = new(name, surname, phone, idFilm, numberOfseats);
            BookingList.Add(item);
        }

        public void CancelBooking(Guid id, int numberOfseats, string phone)
        {
            GetFilmById(id).UnbookSeats(numberOfseats);

            UnbookingList.Add(GetBookingItemById(id));

            BookingList.RemoveAll(b => b.FilmId == id && b.Phone == phone);
        }

        public Film GetFilmById(Guid id)
        {
            return Films.Find(f => f.Id == id);
        }

        public BookingItem GetBookingItemById(Guid id)
        {
            return BookingList.Find(b => b.FilmId == id);
        }

    }
}
