using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManager
{
    /// <summary>
    /// Określa rodzaj pokoju hotelowego.
    /// </summary>
    public enum EnumRoomKind {
        /// <summary>
        /// Pokój jednoosobowy.
        /// </summary>
        Single,
        /// <summary>
        /// Pokój dwuosobowy.
        /// </summary>
        Double,
        /// <summary>
        /// Apartament typu suite.
        /// </summary>
        Suite,
        /// <summary>
        /// Pokój typu studio.
        /// </summary>
        Studio,
        /// <summary>
        /// Inny typ pokoju.
        /// </summary>
        Other
    }

    /// <summary>
    /// Interfejs definiujący podstawowe operacje na hotelu.
    /// </summary>
    public interface IHotel
    {
        /// <summary>
        /// Nazwa hotelu.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Lista pokojów należących do hotelu.
        /// </summary>
        List<Room> Rooms { get; set; }

        /// <summary>
        /// Dodaje pokój do hotelu.
        /// </summary>
        /// <param name="r">Pokój do dodania.</param>
        void DodajPokoj(Room r);
        /// <summary>
        /// Usuwa pokój z hotelu.
        /// </summary>
        /// <param name="r">Pokój do usunięcia.</param>
        void UsunPokoj(Room r);
        /// <summary>
        /// Przydziela gościa do pokoju o podanym identyfikatorze.
        /// </summary>
        /// <param name="rID">Identyfikator pokoju.</param>
        /// <param name="g">Gość do przydzielenia.</param>
        void PrzydzielGoscia(string rID, Guest g);

    }
}
