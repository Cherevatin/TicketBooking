using System.Text.Json;

namespace TicketBooking
{
    internal class CinemaScenarious
    {

        private Cinema _cinema;
        private string _filmListPath;
        private string _bookingListPath;
        private string _cancelBookingListPath;
        public CinemaScenarious(Cinema cinema, string filmsPath, string bookingPath, string cancelBookingPath)
        {
            _cinema = cinema;
            _filmListPath = filmsPath;
            _bookingListPath = bookingPath;
            _cancelBookingListPath = cancelBookingPath;
        }

        public void CreateListOfFilms()
        {


                using (StreamReader r = new StreamReader(_filmListPath))
                {
                    string jsonString = r.ReadToEnd();
                    try
                    {
                        _cinema.AddFilms(JsonSerializer.Deserialize<List<Film>>(jsonString));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }

        }

        public void PrintFilms(List <Film> films = null)
        {
            if (films == null)
            {
                films = _cinema.Films;
            }

            int lengthOfLongestTitle = Convert.ToInt32((-1.3) * films.Max(f => f.Title.Length));
            int lengthOfLongestGengre = Convert.ToInt32((-1.3) * films.Max(f => f.Genre.Length));

            Console.WriteLine("  {0,-5} {1," + lengthOfLongestGengre + "} {2," + lengthOfLongestTitle + "} {3,-8} {4,0}",
                "#",
                "Genre",
                "Film title",
                "Rating",
                "Number of available seats");
            Console.WriteLine();

            foreach (var film in films)
            {
                Console.WriteLine("  {0,-5} {1," + lengthOfLongestGengre + "} {2," + lengthOfLongestTitle + "} {3,-8} {4,0}",
                    films.IndexOf(film) + 1,
                    film.Genre,
                    film.Title,
                    film.Rating,
                    film.FreeSeats);
            }
        }

        public void PrintBookedFilms(List<BookingItem> bookings = null)
        {

            if (bookings == null)
            {
                bookings = _cinema.BookingList;
            }

            var filmsTitles = _cinema.Films.Where(f => bookings.Any(b => b.FilmId == f.Id)).Select(f => f.Title).ToList();

            int lengthOfLongestTitle = filmsTitles.Max(f => f.Length);
            lengthOfLongestTitle = "Film title".Length < lengthOfLongestTitle ? lengthOfLongestTitle : "Film title".Length;
            lengthOfLongestTitle = Convert.ToInt32(-1.3 * lengthOfLongestTitle);

            int lengthOfLongestSurname = Convert.ToInt32((-1.3) * bookings.Max(f => f.Surname.Length));
            lengthOfLongestSurname = "Surname".Length < Math.Abs(lengthOfLongestSurname) ? lengthOfLongestSurname : "Surname".Length;
            lengthOfLongestSurname = Convert.ToInt32(-1.3 * lengthOfLongestSurname);

            Console.WriteLine("  {0,-5} {1," + lengthOfLongestTitle + "} {2," + lengthOfLongestSurname + "} {3,0}",
                "#",
                "Film title",
                "Surname",
                "Number of booked seats");
            Console.WriteLine();

            foreach (var (ticket, index) in bookings.Select((ticket, index) => (ticket, index)))
            {
                var title = _cinema.Films.First(f => f.Id == ticket.FilmId).Title;

                Console.WriteLine("  {0,-5} {1," + lengthOfLongestTitle + "} {2," + lengthOfLongestSurname + "} {3,0}",
                    index + 1,
                    title,
                    ticket.Surname,
                    ticket.NumberOfSeats);

            }
        }

        public void WriteToFile<T>(T obj, string path, bool append)
        {

            string jsonString = JsonSerializer.Serialize(obj);

            using (StreamWriter w = new StreamWriter(path, append: append))
            {
                w.WriteLine(jsonString);
            }
        }

        public void Search()
        {
            Console.Clear();
            Console.WriteLine("### SEARCHING TYPE ####\n\n");
            List<Film> sortedFilms = null;

            Console.WriteLine("\n  Choose option:\n\n" +
                                     "  1 - Search by name\n" +
                                     "  2 - Search by genre\n" +
                                     "  3 - Search by rating\n" +
                                     "  Backspace - Back to main menu");

            bool exit = false;
            while (!exit)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:

                        SearchByName();
                        exit = true;
                        break;

                    case ConsoleKey.D2:

                        SearchByGenre();
                        exit = true;
                        break;

                    case ConsoleKey.D3:

                        SearchByRating();
                        exit = true;
                        break;

                    case ConsoleKey.Backspace:

                        return;

                    default:

                        Console.WriteLine("Undefined option, try again.");
                        break;
                }
            }

            if (sortedFilms != null)
            {
            }
        }
        public void SearchByName()
        {
            Console.Clear();
            Console.WriteLine("### SEARCHING FILM BY NAME ####\n\n");

            Console.Write("\n  Enter film title: ");
            string searchedFilm = Console.ReadLine().ToLower();

            if (!String.IsNullOrEmpty(searchedFilm))
            {
                List<Film> foundFilms = _cinema.Films.Where(f => f.Title.ToLower().Contains(searchedFilm)).ToList();

                if (foundFilms.Any())
                {
                    Console.WriteLine("\n  Searching results:\n");
                    PrintFilms(foundFilms);

                    SearchOkMenu(foundFilms);
                }
                else
                {
                    SearchNotFoundMenu();
                    return;
                }
            }

        }

        public void SearchByGenre()
        {
            Console.Clear();
            Console.WriteLine("### SEARCHING FILM BY GENRE ####");

            Console.Write("\n  Enter genre: ");
            string searchedGenre = Console.ReadLine().ToLower();

            if (!String.IsNullOrEmpty(searchedGenre))
            {
                List<Film> foundFilms = _cinema.Films.Where(f => f.Genre.ToLower().Contains(searchedGenre)).ToList();

                if (foundFilms.Any())
                {
                    Console.WriteLine("\n  Searching results:\n");
                    PrintFilms(foundFilms);

                    SearchOkMenu(foundFilms);
                }
                {
                    SearchNotFoundMenu();
                    return;
                }
            }

        }

        public void SearchByRating()
        {
            Console.Clear();
            Console.WriteLine("### SEARCHING FILM BY RATING ####");

            Console.Write("\n  Enter rating: ");

            int searchedRating;
            while (true) {
                try
                {
                    searchedRating = Convert.ToInt32(Console.ReadLine().ToLower());
                    break;
                }
                catch
                {
                    Console.WriteLine("  Invalid number");
                }
            }

            List<Film> foundFilms = _cinema.Films.Where(f => f.Rating == searchedRating).ToList();

            if (foundFilms.Any())
            {
                Console.WriteLine("\n  Searching results:\n");
                PrintFilms(foundFilms);

                SearchOkMenu(foundFilms);
            }
            else
            {
                SearchNotFoundMenu();
                return;
            }
            
        }

        public void Sort()
        {
            
            Console.Clear();
            Console.WriteLine("#### SORT TYPE ####\n\n");
            List<Film> sortedFilms = null ;

            Console.WriteLine("\n  Choose option:\n\n" +
                                     "  1 - Sort by name\n" +
                                     "  2 - Sort by genre\n" +
                                     "  3 - Sort by rating\n" +
                                     "  4 - Sort by free seats\n" +
                                     "  Backspace - Back to main menu");

            bool exit = false;
            while (!exit)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:

                        sortedFilms = _cinema.Films.OrderBy(f => f.Title).ToList();
                        exit = true;
                        break;

                    case ConsoleKey.D2:

                        sortedFilms = _cinema.Films.OrderBy(f => f.Genre).ToList();
                        exit = true;
                        break;

                    case ConsoleKey.D3:

                        sortedFilms = _cinema.Films.OrderByDescending(f => f.Rating).ToList();
                        exit = true;
                        break;

                    case ConsoleKey.D4:

                        sortedFilms = _cinema.Films.OrderByDescending(f => f.FreeSeats).ToList();
                        exit = true;
                        break;

                    case ConsoleKey.Backspace:

                        return;

                    default:

                        Console.WriteLine("Undefined option, try again.");
                        break;
                }
            }

            if (sortedFilms != null)
            {
                _cinema.UpdateList(sortedFilms);

                WriteToFile(_cinema.Films, _filmListPath, false);
            }
            return;
        }

        public void AddNewFilm()
        {
            Console.Clear();

            Console.WriteLine("### ADDING A FILM ####\n\n");
            Console.Write("\n  Enter film title: ");
            string title = Console.ReadLine();

            Console.Write("\n  Enter film genre: ");
            string genre = Console.ReadLine();

            while (true)
            {
                Console.Write("\n  Enter amount of free seats: ");
                try
                {
                    int freeSeats = Convert.ToInt32(Console.ReadLine());

                    _cinema.AddFilm(title, freeSeats, genre);

                    WriteToFile(_cinema.Films, _filmListPath, true);

                    Console.WriteLine("  New film added successfully!");

                    Console.WriteLine("\n  Press Backspace to back to the main menu\n");
                    while (Console.ReadKey().Key != ConsoleKey.Backspace) { }
                    return;

                }
                catch
                {
                    Console.WriteLine("  Number must be int. Try again.");
                }
            }
        }

        public void BookTicket(List<Film> films = null)
        {

            Film bookedFilm = ChooseFilm(films);

            Console.Clear();
            Console.WriteLine("### TICKET BOOKING ####\n\n");

            Console.WriteLine("  Selected film: " + bookedFilm.Title);

            Console.Write("  Enter your name: ");
            string name = Console.ReadLine();

            Console.Write("  Enter your surname: ");
            string surname = Console.ReadLine();

            Console.Write("  Enter your phone number: ");
            string phone = Console.ReadLine();

            Console.Write("  Enter the number of places you need: ");
            while (true)
            {
                try
                {
                    int numberOfseats = Convert.ToInt32(Console.ReadLine());
                    if (bookedFilm.FreeSeats < numberOfseats)
                    {
                        Console.WriteLine("  Not enough free places");
                    }
                    else
                    {
                        bookedFilm.BookSeats(numberOfseats);
                        WriteToFile(_cinema.Films, _filmListPath, false);

                        _cinema.MakeBooking(name, surname, phone, bookedFilm.Id, numberOfseats);
                        WriteToFile(_cinema.BookingList, _bookingListPath, false);

                        Console.WriteLine("\n  Booking successfully!\n");
                    }
                    Console.WriteLine("\n  Press Backspace to back to the main menu\n");
                    while (Console.ReadKey().Key != ConsoleKey.Backspace) { }
                    return;
                }
                catch
                {
                    Console.Write("  Invalid number");
                }
            }
        }

        public void UnbookTicket(List<BookingItem> films = null)
        {


            if (_cinema.BookingList.Any())
            {
                if (films == null)
                {
                    Console.Clear();
                    PrintBookedFilms();
                }

                BookingItem itemToUnbook = ChooseBooking(films);

                Console.Clear();

                Console.WriteLine("### TICKET UNBOOKING ####\n\n");

                Console.WriteLine("\n\n  Selected film: " + _cinema.GetFilmById(itemToUnbook.FilmId).Title);

                while (true)
                {
                    Console.Write("\n  Enter the phone number on which the booking was made: ");

                    string phone = Console.ReadLine();

                    if (phone == "exit")
                    {
                        return;
                    }
                    else if (phone.Equals(itemToUnbook.Phone))
                    {
                        Console.Write("  Are you sure to cancel your booking? Y/n");
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Y:

                                _cinema.CancelBooking(itemToUnbook.FilmId, itemToUnbook.NumberOfSeats, itemToUnbook.Phone);

                                WriteToFile(_cinema.BookingList, _bookingListPath, false);
                                WriteToFile(_cinema.UnbookingList, _cancelBookingListPath, false);

                                Console.WriteLine("\n  Booking canceled successfully!");
                                break;

                            case ConsoleKey.N:

                                UnbookTicket();
                                return;

                            default:

                                Console.WriteLine("  Ivalid input. Try again.");
                                break;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("  Phone number does not match.\n  Try again or write \'exit\' to main page ");
                    }
                }

            }
            else
            {
                Console.Clear();

                Console.WriteLine("### TICKET UNBOOKING ####\n\n");

                Console.WriteLine("\n  No booked tickets!");
            }


            Console.WriteLine("\n  Press Backspace to back to the main menu\n");
            while (Console.ReadKey().Key != ConsoleKey.Backspace) { }
            return;


        }
        public void ReadComments(List<Film> films = null)
        {

            Film film = ChooseFilm(films);

            Console.Clear();
            Console.WriteLine("### COMMENTS ####\n\n");
            Console.WriteLine("  " + film.Title.ToUpper()+"\n\n");

            if (film.GetComments().Any())
            {
                foreach (var (comment, index) in film.GetComments().Select((comment, index) => (comment, index)))
                {
                    Console.WriteLine("  #" + index + 1 + "\n" +
                                      "  Author: " + comment.AuthorName + "\n" +
                                      "  Rate: " + comment.Rating + "\n" +
                                      "  Content: " + comment.Content + "\n");

                }
            }
            else
            {
                Console.WriteLine("  No comments");
            }

            Console.WriteLine("\n\n\n  Choose option:\n\n" +
                                    "  1 - Add comment\n" +
                                    "  Backspace - Back to main menu");

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:

                        AddingComment(film);
                        return;

                    case ConsoleKey.Backspace:

                        return;

                    default:

                        Console.WriteLine("\n  Undefined option, try again.");
                        break;
                }
            }

            
        }

        public Film ChooseFilm(List<Film> films)
        {

            if (films == null)
            {
                return ChooseByNumber(_cinema.Films);
            }
            else if (films.Count > 1)
            {
                return ChooseByNumber(films);
            }
            else
            {
                return films.First();
            }
        }

        public BookingItem ChooseBooking(List<BookingItem> bookings)
        {

            if (bookings == null)
            {
                return ChooseByNumber(_cinema.BookingList);
            }
            else if (bookings.Count > 1)
            {
                return ChooseByNumber(bookings);
            }
            else
            {
                return bookings.First();
            }
        }
        public T ChooseByNumber<T>(List<T> List)
        {
            T item;
            while (true)
            {
                Console.Write("\n\n  Choose film by number: ");
                try
                {
                    item = List[(Convert.ToInt32(Console.ReadLine())) - 1];
                    return item;
                }
                catch
                {
                    Console.Write("  Invalid number. Try again.");
                }
            }
        }


        public void FilmTicketList(List<Film> films)
        {
            Film film = ChooseFilm(films);

            Console.Clear();
            Console.WriteLine("### FILM BOOKINGS LIST ####\n\n");

            List<BookingItem> bookingsList = _cinema.BookingList.Where(b => _cinema.Films.Any(f => f.Id == b.FilmId)).ToList();

            if (bookingsList.Any())
            {
                PrintBookedFilms(bookingsList);
            }
            else
            {
                Console.WriteLine("  There are no bookings for this film");
            }
            BookUnbookMenu(bookingsList, films);
        }
        
        public void ListOfBookedFilms()
        {

            Console.Clear();

            Console.WriteLine("  ### LIST OF BOOKED FILMS ####\n\n");

            if (_cinema.BookingList.Any())
            {
                PrintBookedFilms();

                BookUnbookMenu(_cinema.BookingList,_cinema.Films);
                return;
            }
            else
            {
                Console.WriteLine("  No films booked!");
            }

            Console.WriteLine("\n  Press Backspace to back to the main menu\n");
            while (Console.ReadKey().Key != ConsoleKey.Backspace) { }
            return;
        }


        public void AddingComment(Film film)
        {
            Console.Clear();

            Console.WriteLine("  ### ADDING COMMENT ####\n\n");

            Console.Write("  Enter your name: ");
            string authorName = Console.ReadLine();

            Console.Write("  Enter your rating: ");
            double rating = Convert.ToDouble(Console.ReadLine());

            Console.Write("  Enter your comment: ");
            string content = Console.ReadLine();

            film.AddComment(film.Id, authorName, rating, content);

            Console.WriteLine("\n\n  Comment added successfully!");

            Console.WriteLine("\n  Press Backspace to back to the main menu\n");
            while (Console.ReadKey().Key != ConsoleKey.Backspace) { }
            return;
        }

        public void SearchOkMenu(List<Film> films)
        {
            Console.WriteLine("\n\n\n  Choose option:\n\n" +
                                      "  1 - Book tickets\n" +
                                      "  2 - Read comments\n" +
                                      "  3 - List of bookings \n\n" +
                                      "  Backspace - Back to main menu");

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:

                        BookTicket(films);
                        return;

                    case ConsoleKey.D2:

                        ReadComments(films);
                        return;

                    case ConsoleKey.D3:

                        FilmTicketList(films);
                        return;

                    case ConsoleKey.Backspace:

                        return;

                    default:
                        Console.WriteLine("\n  Undefined option, try again.");
                        break;
                }
            }
        }

        public void SearchNotFoundMenu()
        {
            Console.WriteLine("\n  Film not found");
            Console.WriteLine("\n\n\n  Choose option:\n\n" +
                              "  1 - Add the film\n\n" +
                              "  Backspace - Back to main menu");

            while (true)
            {

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        AddNewFilm();
                        return;
                    case ConsoleKey.Backspace:
                        return;
                    default:
                        Console.WriteLine("  Undefined option, try again.");
                        break;
                }
            }
        }

        public void BookUnbookMenu(List<BookingItem> bookings = null, List<Film> films = null)
        {
            Console.WriteLine("\n\n\n  Choose option:\n\n" +
                                      "  1 - Unbook ticket\n" +
                                      "  2 - Book ticket\n" +
                                      "  Backspace - Back to main menu");

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:

                        UnbookTicket(bookings);
                        return;

                    case ConsoleKey.D2:

                        BookTicket(films);
                        return;

                    case ConsoleKey.Backspace:

                        return;

                    default:

                        Console.WriteLine("\n  Undefined option, try again.");
                        break;
                }
            }
        }
    }


}

