using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManager
{
    /// <summary>
    /// Wyjątek zgłaszany w przypadku niepoprawnych danych pokoju.
    /// </summary>
    public class InvalidRoomDataException : Exception
    {
        /// <summary>
        /// Inicjalizuje nową instancję wyjątku <see cref="InvalidRoomDataException"/>.
        /// </summary>
        /// <param name="message">Komunikat opisujący błąd.</param>
        public InvalidRoomDataException(string? message) : base(message) { }
    }

    /// <summary>
    /// Abstrakcyjna klasa bazowa reprezentująca pokój hotelowy.
    /// </summary>
    public abstract class Room : IEquatable<Room>, IComparable<Room>
    {
        private string roomID;

        static readonly decimal minimalPrice;
        private EnumRoomKind roomKind;
        private string name;
        private decimal price;
        /// <summary>
        /// Data utworzenia pokoju.
        /// </summary>
        protected DateTime creationDate;

        /// <summary>
        /// Inicjalizuje statyczne pola klasy <see cref="Room"/>.
        /// </summary>
        static Room()
        {
            minimalPrice = 100.00m;
        }

        /// <summary>
        /// Unikalny identyfikator pokoju.
        /// </summary>
        [Key]
        public string RoomID { get => roomID; set => roomID = value; }

        /// <summary>
        /// Rodzaj pokoju.
        /// </summary>
        public EnumRoomKind RoomKind { get => roomKind; set => roomKind = value; }
        /// <summary>
        /// Nazwa pokoju.
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// Cena bazowa pokoju.
        /// </summary>
        /// <exception cref="InvalidRoomDataException">
        /// Rzucany, gdy cena jest niższa niż minimalna.
        /// </exception>
        public decimal Price
        {
            get => price;
            set
            {
                if (value < minimalPrice)
                {
                    throw new InvalidRoomDataException("Cena poniżej ceny minimalnej hotelu");
                }
                price = value;
            }
        }

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="Room"/>.
        /// </summary>
        public Room()
        {
            this.creationDate = DateTime.Now;
        }
        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="Room"/> z danymi pokoju.
        /// </summary>
        /// <param name="roomKind">Rodzaj pokoju.</param>
        /// <param name="name">Nazwa pokoju.</param>
        /// <param name="price">Cena bazowa pokoju.</param>
        public Room(EnumRoomKind roomKind, string name, decimal price)
        {
            this.creationDate = DateTime.Now;
            this.roomKind = roomKind;
            this.name = name;
            this.Price = price;
            this.roomID = GenerujNumer();
        }

        /// <summary>
        /// Lista gości aktualnie przypisanych do pokoju.
        /// </summary>
        public List<Guest> AssignedGuests { get; set; } = new List<Guest>();

        /// <summary>
        /// Generuje unikalny numer pokoju.
        /// </summary>
        /// <returns>Wygenerowany identyfikator pokoju.</returns>
        private string GenerujNumer()
        {
            string prefix = name.Length >= 2 ? name.Substring(0, 2).ToUpper() : "RM";
            return $"{prefix}\\{creationDate.Year}\\{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}";
        }
        /// <summary>
        /// Oblicza końcową cenę pokoju, uwzględniając rabaty.
        /// </summary>
        /// <returns>Końcowa cena pokoju.</returns>
        public virtual decimal FinalPrice()
        {
            decimal currentPrice = Price;
            if ((DateTime.Now - creationDate).TotalDays > 30)
            {
                currentPrice = currentPrice * 0.9m;
            }
            return currentPrice;
        }
        /// <summary>
        /// Melduje gościa w pokoju.
        /// </summary>
        /// <param name="g">Gość do zameldowania.</param>
        public void Meldunek(Guest g)
        {
            AssignedGuests.Add(g);
        }
        /// <summary>
        /// Dodaje gościa do pokoju.
        /// </summary>
        /// <param name="g">Gość do dodania.</param>
        public void DodajGoscia(Guest g) => AssignedGuests.Add(g);
        /// <summary>
        /// Usuwa gościa z pokoju na podstawie numeru PESEL.
        /// </summary>
        /// <param name="pesel">Numer PESEL gościa.</param>
        public void UsunGoscia(string pesel) => AssignedGuests.RemoveAll(x => x.Pesel == pesel);
        /// <summary>
        /// Usuwa gości, których pobyt już się zakończył.
        /// </summary>
        public void OdswiezGosci()
        {
            AssignedGuests.RemoveAll(g => g.CheckOutDate < DateTime.Now);
        }
        /// <summary>
        /// Zwraca tekstową reprezentację pokoju wraz z listą gości.
        /// </summary>
        /// <returns>Opis pokoju.</returns>
        public override string ToString()
        {
            OdswiezGosci();
            string info = $"[{RoomID}] {Name} ({Price} zl/doba)";

            foreach (var g in AssignedGuests)
            {
                int dni = (g.CheckOutDate - g.CheckInDate).Days;
                info += $"\n" +
                    $"" +
                    $"" +
                    $"Gość: {g.Imie} {g.Nazwisko}, Pobyt: {dni} dni, Koszt: {dni * FinalPrice()} zl";
            }
            return info;
        }
        /// <summary>
        /// Sprawdza równość dwóch pokoi na podstawie identyfikatora.
        /// </summary>
        /// <param name="other">Inny pokój.</param>
        /// <returns>
        /// <c>true</c>, jeśli pokoje są równe; w przeciwnym razie <c>false</c>.
        /// </returns>
        public bool Equals(Room? other) => other != null && roomID == other.roomID;
        /// <summary>
        /// Porównuje pokoje według ceny.
        /// </summary>
        /// <param name="other">Inny pokój.</param>
        /// <returns>Wynik porównania cen.</returns>
        public int CompareTo(Room? other)
        {
            if (other == null) return -1;
            return other.Price.CompareTo(this.Price);
        }

        /// <summary>
        /// Identyfikator hotelu, do którego należy pokój.
        /// </summary>
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        /// <summary>
        /// Hotel, do którego należy pokój.
        /// </summary>
        public virtual Hotel Hotel { get; set; }
    }
}
