using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManager
{
    /// <summary>
    /// Reprezentuje standardowy pokój hotelowy.
    /// </summary>
    public class StandardRoom : Room
    {
        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="StandardRoom"/>.
        /// </summary>
        public StandardRoom() : base() { }
        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="StandardRoom"/> z danymi pokoju.
        /// </summary>
        /// <param name="roomKind">Rodzaj pokoju.</param>
        /// <param name="name">Nazwa pokoju.</param>
        /// <param name="price">Cena bazowa pokoju.</param>
        public StandardRoom(EnumRoomKind roomKind, string name, decimal price) : base(roomKind, name, price) { }
    }
}
