
namespace TicketBooking
{
    internal class Program
    {
        static void Main()
        {
            string filmsDataPath = @"C:\folder\Work\ABNK\TicketBooking\TicketBooking\films.json";
            string bookingDataPath = @"C:\folder\Work\ABNK\TicketBooking\TicketBooking\boking.json";
            string cancelBookingDataPath = @"C:\folder\Work\ABNK\TicketBooking\TicketBooking\cancelBoking.json";

            bool listAlredyExists = false;

            Cinema cinema = new();

            CinemaScenarious cinemaScenarious = new(cinema, filmsDataPath, bookingDataPath, cancelBookingDataPath);

            while (true)
            {
                if (File.Exists(filmsDataPath))
                {
                    if (!listAlredyExists) 
                        cinemaScenarious.CreateListOfFilms();

                    while (true)
                    {
                        Console.Clear();

                        cinemaScenarious.PrintFilms();

                        Console.WriteLine("\n\n\n  Choose option:\n\n" +
                                            "  1 - Search film\n" +
                                            "  2 - Sort films\n" +
                                            "  3 - Read comments\n" +
                                            "  4 - Book ticket\n" +
                                            "  5 - List of booked films\n" +
                                            "  6 - Unbook ticket\n" +
                                            "  7 - Add new film\n\n" +
                                            "  Esc - Exit");

                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.D1:

                                cinemaScenarious.Search();
                                break;

                            case ConsoleKey.D2:

                                cinemaScenarious.Sort();
                                break;

                            case ConsoleKey.D3:

                                cinemaScenarious.ReadComments();
                                break;

                            case ConsoleKey.D4:

                                cinemaScenarious.BookTicket();
                                break;

                            case ConsoleKey.D5:

                                cinemaScenarious.ListOfBookedFilms();
                                break;

                            case ConsoleKey.D6:

                                cinemaScenarious.UnbookTicket();
                                break;

                            case ConsoleKey.D7:

                                cinemaScenarious.AddNewFilm();
                                break;

                            case ConsoleKey.Escape:
                                return;

                            default:

                                Console.WriteLine("Undefined option, try again.");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("  File not found");

                    Console.WriteLine("\n\n\n  Choose option:\n\n" +
                                        "  1 - Add new film\n\n" +
                                        "  Esc - Exit");
                    bool exit = false;
                    while (!exit)
                    {
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.D1:
                                cinemaScenarious.AddNewFilm();
                                listAlredyExists = true;
                                exit = true;
                                break;
                            case ConsoleKey.Escape:
                                return;

                            default:

                                Console.WriteLine("Undefined option, try again.");
                                break;
                        }
                    }
                }
            }

        }
    }
    
}