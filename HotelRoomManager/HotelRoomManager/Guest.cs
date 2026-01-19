using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelRoomManager
{
    /// <summary>
    /// Wyjątek zgłaszany w przypadku niepoprawnego numeru PESEL.
    /// </summary>
    public class InvalidPeselException : Exception
    {
        /// <summary>
        /// Inicjalizuje nową instancję wyjątku <see cref="InvalidPeselException"/>.
        /// </summary>
        /// <param name="message">Komunikat opisujący błąd.</param>
        public InvalidPeselException(string? message) : base(message) { }
    }
    /// <summary>
    /// Wyjątek zgłaszany w przypadku niepoprawnego imienia.
    /// </summary>
    public class InvalidImieException : Exception
    {
        /// <summary>
        /// Inicjalizuje nową instancję wyjątku <see cref="InvalidImieException"/>.
        /// </summary>
        /// <param name="message">Komunikat opisujący błąd.</param>
        public InvalidImieException(string? message) : base(message) { }
    }
    /// <summary>
    /// Wyjątek zgłaszany w przypadku niepoprawnego nazwiska.
    /// </summary>
    public class InvalidNazwiskoException : Exception
    {
        /// <summary>
        /// Inicjalizuje nową instancję wyjątku <see cref="InvalidNazwiskoException"/>.
        /// </summary>
        /// <param name="message">Komunikat opisujący błąd.</param>
        public InvalidNazwiskoException(string? message) : base(message) { }
    }

    /// <summary>
    /// Reprezentuje gościa hotelowego wraz z danymi osobowymi i terminem pobytu.
    /// </summary>
    public class Guest : ICloneable
    {
        private string imie;
        private string nazwisko;
        private string pesel;

        /// <summary>
        /// Imię gościa.
        /// </summary>
        /// <exception cref="InvalidImieException">
        /// Rzucany, gdy imię jest puste, za krótkie lub nie zaczyna się od wielkiej litery.
        /// </exception>
        public string Imie
        {
            get => imie;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) { throw new InvalidImieException("Imie ma puste miejsca"); }
                if (value.Length < 3) { throw new InvalidImieException("Imie jest za krotkie"); }
                if (!char.IsUpper(value[0])) { throw new InvalidImieException("Imie musi zaczynać się z dużej litery"); }
                imie = value;
            }
        }

        /// <summary>
        /// Nazwisko gościa.
        /// </summary>
        /// <exception cref="InvalidNazwiskoException">
        /// Rzucany, gdy nazwisko jest puste, za krótkie lub nie zaczyna się od wielkiej litery.
        /// </exception>
        public string Nazwisko
        {
            get => nazwisko;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) { throw new InvalidNazwiskoException("Nazwisko ma puste miejsca"); }
                if (value.Length < 3) { throw new InvalidNazwiskoException("Nazwisko jest za krotkie"); }
                if (!char.IsUpper(value[0])) { throw new InvalidNazwiskoException("Nazwisko musi zaczynać się z dużej litery"); }
                nazwisko = value;
            }
        }

        /// <summary>
        /// Numer PESEL gościa.
        /// </summary>
        /// <exception cref="InvalidPeselException">
        /// Rzucany, gdy PESEL jest pusty, ma niepoprawną długość lub zawiera znaki inne niż cyfry.
        /// </exception>
        public string Pesel
        {
            get => pesel;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) { throw new InvalidPeselException("Pesel ma puste miejsca"); }
                if (value.Length != 11) { throw new InvalidPeselException("Zla długość Peselu"); }
                if (!value.All(char.IsDigit)) { throw new InvalidPeselException("Pesel musi składać się tylko z cyfr"); }
                pesel = value;
            }
        }

        /// <summary>
        /// Data zameldowania gościa.
        /// </summary>
        public DateTime CheckInDate { get; set; }
        /// <summary>
        /// Data wymeldowania gościa.
        /// </summary>
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// Inicjalizuje nową, pustą instancję klasy <see cref="Guest"/>.
        /// </summary>
        public Guest() { }

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="Guest"/> z danymi gościa i czasem pobytu.
        /// </summary>
        /// <param name="imie">Imię gościa.</param>
        /// <param name="nazwisko">Nazwisko gościa.</param>
        /// <param name="pesel">Numer PESEL gościa.</param>
        /// <param name="daysCount">Liczba dni pobytu.</param>
        public Guest(string imie, string nazwisko, string pesel, int daysCount)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Pesel = pesel;
            CheckInDate = DateTime.Now;
            CheckOutDate = DateTime.Now.AddDays(daysCount);
        }

        /// <summary>
        /// Zwraca tekstową reprezentację gościa.
        /// </summary>
        /// <returns>
        /// Ciąg znaków zawierający imię, nazwisko oraz datę wymeldowania.
        /// </returns>
        public override string ToString()
        {
            return $"{Imie} {Nazwisko} (do: {CheckOutDate:yyyy-MM-dd})";
        }

        /// <summary>
        /// Tworzy płytką kopię obiektu gościa.
        /// </summary>
        /// <returns>Klon obiektu <see cref="Guest"/>.</returns>
        public object Clone()
        {
            Guest g = this.MemberwiseClone() as Guest;
            return g!;
        }

        /// <summary>
        /// Unikalny identyfikator gościa.
        /// </summary>
        [Key]
        public int GuestId { get; set; }
    }
}
