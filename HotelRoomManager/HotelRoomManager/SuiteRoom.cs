using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManager
{
    /// <summary>
    /// Reprezentuje apartament typu suite z dodatkową opłatą.
    /// </summary>
    public class SuiteRoom : Room
    {
        /// <summary>
        /// Dodatkowa opłata za apartament typu suite.
        /// </summary>
        static decimal suiteBonus;

        /// <summary>
        /// Inicjalizuje statyczne pola klasy <see cref="SuiteRoom"/>.
        /// </summary>
        static SuiteRoom()
        {
            suiteBonus = 250.00m;
        }

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="SuiteRoom"/>.
        /// </summary>
        public SuiteRoom() : base() { }

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="SuiteRoom"/> z danymi pokoju.
        /// </summary>
        /// <param name="roomKind">Rodzaj pokoju.</param>
        /// <param name="name">Nazwa pokoju.</param>
        /// <param name="price">Cena bazowa pokoju.</param>
        public SuiteRoom(EnumRoomKind roomKind, string name, decimal price) : base(roomKind, name, price)
        {
            this.Price += suiteBonus;
        }

        /// <summary>
        /// Oblicza końcową cenę apartamentu typu suite, uwzględniając dodatkową opłatę.
        /// </summary>
        /// <returns>Końcowa cena apartamentu.</returns>
        public override decimal FinalPrice()
        {
            decimal currentPrice = base.FinalPrice();
            return currentPrice + suiteBonus;
        }
    }
}
