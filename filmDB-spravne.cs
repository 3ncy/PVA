public class FilmDB
{
    #region utility metody
    static void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
    {
        Write(message + "\n", color);
    }

    static void Write(string message, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ForegroundColor = ConsoleColor.White;
    }

    static string Ask(string question, string invalidResponse, Func<string, bool> porovnani)
    {
        Write(question);
        string hodnota = Console.ReadLine();
        while (!porovnani(hodnota))
        {
            WriteLine(invalidResponse, ConsoleColor.Red);
            Write(question);
            hodnota = Console.ReadLine();
        }
        return hodnota;
    }
    #endregion

    public static void Main()
    {
        //main loop
        while (true)
        {
            Console.Write("Zadejte volbu: ");
            string volba = Console.ReadLine().Trim().ToLower();

            switch (volba)
            {
                case "pridat":
                    string nazev = Ask("Zadejte nazev: ", "", odpoved => true);
                    string autor = Ask("Zadejte autora filmu: ", "", odpoved => true);
                    string zanr = Ask("Zadejte zanr filmu: ", "", odpoved => true);
                    int rok = int.Parse(Ask("Zadejte rok natoceni: ", "Neplatny rok!", odpoved => int.TryParse(odpoved, out _)));
                    DB.Pridat(new Movie(nazev, autor, zanr, rok));
                    break;

                case "vypsat":
                    DB.Vypsat();
                    break;

                case "najit":
                    string query = Ask("Zadejte hledany nazev: ", "", odpoved => true);
                    DB.Najit(query);
                    break;

                case "odebrat":
                    string remove = Ask("Zadejte nazev filmu na smazani: ", "", odpoved => true);
                    DB.Odebrat(remove);
                    break;

                case "ulozit":
                    string dbName = Ask("zadejte jmeno souboru (prazdne pro defaultni): ", "", odpoved => true);
                    DB.Ulozit(dbName);
                    break;

                case "nacist":
                    string soubor = Ask("zadejte jmeno souboru (nechte prazdne pro defaultni): ", "", odpoved => true);
                    DB.Nacist(soubor);

                    break;

                default:
                    Write("neplatny prikaz. ", ConsoleColor.Red);
                    Write("Prikazy jsou: ");
                    WriteLine("pridat, vypsat, najit, odebrat, ulozit, nacist", ConsoleColor.DarkYellow);
                    break;
            }
        }
    }

    public class DB
    {
        static List<Movie> movies = new List<Movie>();

        public static void Pridat(Movie movie)
        {
            movies.Add(movie);
            WriteLine("Film uspesne pridan!", ConsoleColor.Green);
        }

        public static void Vypsat()
        {
            if (movies.Count == 0) WriteLine("V databazi nejsou zadne filmy! ", ConsoleColor.Yellow);
            foreach (Movie film in movies)
            {
                WriteLine(film.Nazev + " by " + film.Autor + " (" + film.Rok + "); " + film.Zanr, ConsoleColor.Yellow);
            }
        }

        public static void Najit(string query)
        {
            foreach (Movie movie in movies.FindAll(m =>
                         m.Nazev.Contains(query) || m.Autor.Contains(query) || m.Zanr.Contains(query) || m.Rok.ToString().Contains(query)
                         ))//tODO: zvyraznit hledany term
            {
                WriteLine(movie.Nazev + " by " + movie.Autor + " (" + movie.Rok + "); " + movie.Zanr, ConsoleColor.Yellow);
            }
        }

        public static void Odebrat(string remove)
        {
            if (movies.Remove(movies.Find(m => m.Nazev == remove)))
            {
                WriteLine($"Film \"{remove}\" byl smazan z databaze!", ConsoleColor.Green);
            }
            else
            {
                WriteLine($"Film \"{remove}\" nebyl nalezen!", ConsoleColor.Red);
            }
        }

        public static void Ulozit(string filename)
        {
            if (filename == "") filename = "databaze";
            using (StreamWriter sw = new StreamWriter(filename + ".txt"))
            {
                foreach (Movie movie in movies)
                {
                    sw.WriteLine(movie.Nazev);
                    sw.WriteLine(movie.Autor);
                    sw.WriteLine(movie.Zanr);
                    sw.WriteLine(movie.Rok);
                }
            }
            WriteLine("Soubor uspesne ulozen.", ConsoleColor.Green);
        }

        public static void Nacist(string filename)
        {
            if (filename == "") filename = "databaze";

            List<Movie> nacitaneFilmy = new List<Movie>();

            try
            {
                int pocetRadku = File.ReadAllLines(filename + ".txt").Count();
                //if (pocetRadku % 4 != 0)//TODO nepouzivat magic number, 4 je pocet vlastnosti objektu Movie
                //{
                //    throw new Exception("soubor je poskozeny");
                //}
                using (StreamReader sr = new StreamReader(filename + ".txt"))
                {
                    for (int i = 0; i < pocetRadku; i += 4)
                    {
                        string nacitanyNazev = sr.ReadLine();
                        string nacitanyAutor = sr.ReadLine();
                        string nacitanyZanr = sr.ReadLine();
                        int nacitanyRok = int.Parse(sr.ReadLine());

                        nacitaneFilmy.Add(new Movie(nacitanyNazev, nacitanyAutor, nacitanyZanr, nacitanyRok));
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(FileNotFoundException))
                {
                    WriteLine("Soubor nenalezen, nic nenacteno!", ConsoleColor.Red);
                }
                else
                {
                    WriteLine("Zaday soubor je poskozen, nic nebylo nacteno!", ConsoleColor.Red);
                }
            }

            movies.RemoveAll(m => true);
            movies.AddRange(nacitaneFilmy);

            WriteLine("Filmy uspesne nacteny ze souboru!", ConsoleColor.Green);
        }
    }

    public class Movie
    {
        public string Nazev { get; private set; }
        public string Autor { get; private set; }
        public string Zanr { get; private set; }
        public int Rok { get; private set; }

        public Movie(string nazev, string autor, string zanr, int rok)
        {
            Nazev = nazev;
            Autor = autor;
            Zanr = zanr;
            Rok = rok;
        }
    }
}