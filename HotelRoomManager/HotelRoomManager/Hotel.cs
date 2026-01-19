using HotelRoomManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace HotelRoomManager
{
    /// <summary>
    /// Wyjątek zgłaszany w przypadku niepoprawnej nazwy hotelu.
    /// </summary>
    public class InvalidNameException : Exception
    {
        /// <summary>
        /// Inicjalizuje nową instancję wyjątku <see cref="InvalidNameException"/>.
        /// </summary>
        /// <param name="message">Komunikat opisujący błąd.</param>
        public InvalidNameException(string? message) : base(message) { }
    }

    /// <summary>
    /// Reprezentuje hotel zawierający pokoje oraz umożliwia operacje na nich,
    /// takie jak serializacja, sortowanie i obliczanie przychodu.
    /// </summary>
    [XmlInclude(typeof(Room))]
    [XmlInclude(typeof(SuiteRoom))]
    [XmlInclude(typeof(Guest))]
    [XmlInclude(typeof(StandardRoom))]
    public class Hotel : IHotel, IComparer<Room>
    {
        private string name;
        private List<Room> rooms;

        /// <summary>
        /// Informuje, czy dane hotelu zostały zmienione.
        /// </summary>
        public static bool CzyZmienionoDane { get; set; } = false;
        /// <summary>
        /// Nazwa hotelu.
        /// </summary>
        /// <exception cref="InvalidNameException">
        /// Rzucany, gdy nazwa jest pusta.
        /// </exception>
        public string Name
        {
            get => name;
            set
            {
                if (value == null) { throw new InvalidNameException("Nazwa jest pusta"); }
                name = value;
            }
        }
        /// <summary>
        /// Lista pokojów hotelowych.
        /// </summary>
        public List<Room> Rooms { get => rooms; set => rooms = value; }

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="Hotel"/>.
        /// </summary>
        public Hotel()
        {
            this.rooms = new List<Room>();
        }

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="Hotel"/> z podaną nazwą.
        /// </summary>
        /// <param name="name">Nazwa hotelu.</param>
        public Hotel(string name)
        {
            this.name = name;
            this.rooms = new List<Room>();
        }

        /// <summary>
        /// Zapisuje obiekt hotelu do pliku XML z użyciem <see cref="XmlSerializer"/>.
        /// </summary>
        /// <param name="fname">Nazwa pliku XML.</param>
        public void DCSaveToXML(string fname)
        {
            foreach (var r in rooms) r.OdswiezGosci();

            XmlSerializer serializer = new XmlSerializer(typeof(Hotel));
            using (var writer = new StreamWriter(fname))
            {
                serializer.Serialize(writer, this);
            }
        }
        /// <summary>
        /// Odczytuje obiekt hotelu z pliku XML.
        /// </summary>
        /// <param name="fname">Nazwa pliku XML.</param>
        /// <returns>Obiekt <see cref="Hotel"/> odczytany z pliku.</returns>
        public static Hotel DCReadFromXML(string fname)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Hotel));
            using (StreamReader reader = new StreamReader(fname))
            {
                return (Hotel)serializer.Deserialize(reader);
            }
        }
        /// <summary>
        /// Dodaje pokój do hotelu, jeśli nie istnieje już na liście.
        /// </summary>
        /// <param name="r">Pokój do dodania.</param>
        public void DodajPokoj(Room r)
        {
            if (!rooms.Any(x => x.Equals(r)))
            {
                rooms.Add(r);
            }
        }
        /// <summary>
        /// Usuwa pokój z hotelu.
        /// </summary>
        /// <param name="r">Pokój do usunięcia.</param>
        public void UsunPokoj(Room r)
        {
            rooms.Remove(r);
        }
        /// <summary>
        /// Oblicza aktualny całkowity przychód hotelu na podstawie pobytów gości.
        /// </summary>
        /// <returns>Całkowity przychód hotelu.</returns>
        public decimal TotalActualIncome()
        {
            decimal suma = 0;
            foreach (var room in rooms)
            {
                foreach (var guest in room.AssignedGuests)
                {
                    // Obliczamy liczbę dni pobytu
                    int dni = (guest.CheckOutDate - guest.CheckInDate).Days;
                    if (dni <= 0) dni = 1; // Zabezpieczenie, by pobyt trwał min. 1 dzień

                    suma += room.FinalPrice() * dni;
                }
            }
            return suma;
        }

        /// <summary>
        /// Wybiera pokoje określonego typu.
        /// </summary>
        /// <param name="typ">Typ pokoju.</param>
        /// <returns>Lista pokojów danego typu.</returns>
        public List<Room> WybierzPokojeTypu(EnumRoomKind typ)
        {
            return rooms.Where(x => x.RoomKind == typ).ToList();
        }

        /// <summary>
        /// Wybiera apartamenty o cenie wyższej niż podana.
        /// </summary>
        /// <param name="minimalnaCena">Minimalna cena pokoju.</param>
        /// <returns>Lista drogich apartamentów.</returns>
        public List<Room> WybierzDrogieApartamenty(decimal minimalnaCena)
        {
            return rooms.Where(x => x is SuiteRoom && x.FinalPrice() > minimalnaCena).ToList();
        }

        /// <summary>
        /// Przydziela gościa do pokoju o podanym identyfikatorze.
        /// </summary>
        /// <param name="rID">Identyfikator pokoju.</param>
        /// <param name="g">Gość do przydzielenia.</param>
        public void PrzydzielGoscia(string rID, Guest g)
        {
            var r = rooms.FirstOrDefault(x => x.RoomID == rID);
            if (r != null)
            {
                r.DodajGoscia(g);
            }
        }

        /// <summary>
        /// Zwraca tekstową reprezentację hotelu.
        /// </summary>
        /// <returns>Opis hotelu wraz z listą pokoi.</returns>
        public override string ToString()
        {
            string wynik = $"Hotel \"{name}\" \n" +
                $"Przychód: {TotalActualIncome()} zl, Pokoje: {rooms.Count} \n" +
                $"------------------------------------------------------------ \n";

            foreach (Room r in rooms)
            {
                wynik += r.ToString() + "\n";
            }

            return wynik;
        }

        /// <summary>
        /// Sortuje pokoje malejąco według ceny.
        /// </summary>
        public void SortRoomsByPriceDescending()
        {
            Rooms.Sort(); // IComparable<Room> w Room decyduje o kolejności
        }

        /// <summary>
        /// Porównuje dwa pokoje według ceny.
        /// </summary>
        /// <param name="x">Pierwszy pokój.</param>
        /// <param name="y">Drugi pokój.</param>
        /// <returns>Wynik porównania cen.</returns>
        public int Compare(Room? x, Room? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            return x.Price.CompareTo(y.Price); // rosnąco
        }
        /// <summary>
        /// Sortuje pokoje rosnąco według ceny.
        /// </summary>
        public void SortRoomsByPriceAscending()
        {
            rooms.Sort(this);
        }

        /// <summary>
        /// Zapisuje hotel do bazy danych.
        /// </summary>
        public void SaveToDb()
        {
            using (var db = new HotelDbContext())
            {
                Console.WriteLine("Zapisywanie hotelu do bazy danych...");
                db.Hotels.Add(this);
                db.SaveChanges();
                Console.WriteLine("Hotel zapisany poprawnie.");
            }
        }

        /// <summary>
        /// Odczytuje ostatnio zapisany hotel z bazy danych.
        /// </summary>
        /// <returns>Obiekt <see cref="Hotel"/> odczytany z bazy danych.</returns>
        public static Hotel ReadHotelFromDb()
        {
            var db = new HotelDbContext();
            Hotel z = new Hotel();
            int hotelId = db.Hotels.Max(z => z.HotelId);
            var zbaza = db.Hotels.Include(h => h.Rooms.Select(r => r.AssignedGuests)).FirstOrDefault(h => h.HotelId == hotelId);
            z.name = zbaza.Name;
            z.rooms = zbaza.Rooms;
            return z;
        }

        /// <summary>
        /// Unikalny identyfikator hotelu.
        /// </summary>
        [Key]
        public int HotelId { get; set; }

        /// <summary>
        /// Delegat filtrujący pokoje.
        /// </summary>
        /// <param name="room">Pokój do sprawdzenia.</param>
        /// <returns>
        /// Wartość <c>true</c>, jeśli pokój spełnia warunek; w przeciwnym razie <c>false</c>.
        /// </returns>
        public delegate bool RoomFilter(Room room);

        /// <summary>
        /// Filtruje pokoje na podstawie przekazanego delegata.
        /// </summary>
        /// <param name="filter">Delegat filtrujący.</param>
        /// <returns>Lista pokoi spełniających warunek.</returns>
        public List<Room> FilterRooms(RoomFilter filter)
        {
            return rooms.Where(room => filter(room)).ToList();
        }
    }
}