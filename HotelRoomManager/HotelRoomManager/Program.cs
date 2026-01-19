using HotelRoomManager;
using System;
using System.Data.Entity;

namespace HotelRoomManager
{
    /// <summary>
    /// Zawiera klasy i interfejsy odpowiedzialne za zarządzanie hotelem, pokojami oraz gośćmi.
    /// </summary>
    class NamespaceDoc { }

    internal class Program
    {

        private static void Main(string[] args)
        {
            Console.WriteLine("=== SYSTEM ZARZĄDZANIA HOTELEM ===\n");
            try
            { 
                Hotel mojHotel = new Hotel("Grand Hotel");

                Room r1 = new StandardRoom(EnumRoomKind.Double, "Błękitny", 200m);
                Room r2 = new StandardRoom(EnumRoomKind.Double, "Błękitny", 200m);
                Room r3 = r1;
                SuiteRoom s1 = new SuiteRoom(EnumRoomKind.Suite, "Królewski", 500m);

                mojHotel.DodajPokoj(r1);
                mojHotel.DodajPokoj(s1);

                Guest g1 = new Guest("Jan", "Kowalski", "90101030111", 5);

                Guest g2 = new Guest("Maciej", "Kowal", "80020200222", 12);

                mojHotel.PrzydzielGoscia(r1.RoomID, g1);
                mojHotel.PrzydzielGoscia(s1.RoomID, g2);

                mojHotel.SortRoomsByPriceDescending(); 
                Console.WriteLine("Sortowanie według Ceny malejąco - IComparable");
                Console.WriteLine(mojHotel.ToString());

                mojHotel.SortRoomsByPriceAscending();
                Console.WriteLine("Sortowanie według Ceny rosnąco - IComparer");
                Console.WriteLine(mojHotel.ToString());

                Console.WriteLine(r1.Equals(r2));
                Console.WriteLine(r1.Equals(r3));
                Console.WriteLine("\n");

                mojHotel.DCSaveToXML("stan_hotelu.xml");
                Console.WriteLine("Zapisano stan hotelu do pliku XML.");

                Hotel wczytanyHotel = Hotel.DCReadFromXML("stan_hotelu.xml");
                Console.WriteLine("\n--- DANE WCZYTANE Z PLIKU XML ---");
                Console.WriteLine(wczytanyHotel.ToString());

                mojHotel.SaveToDb();
                Hotel test = Hotel.ReadHotelFromDb();
                Console.WriteLine("\n--- DANE WCZYTANE Z BAZY DANYCH ---");
                Console.WriteLine(test.ToString());

                Console.WriteLine("\nFiltrowanie pokojów\nPokoje za mniej niż 500 zł za dobę:");
                Hotel.RoomFilter filtrCeny = (room => room.Price < 500);
                List<Room> pokojePowyzej500zl = mojHotel.FilterRooms(filtrCeny);
                pokojePowyzej500zl.ForEach(r => Console.WriteLine(r.ToString()));
                
            }
            catch (InvalidRoomDataException ex)
            {
                Console.WriteLine($"Blad danych pokoju: {ex.Message}");
            }
            catch (InvalidImieException ex)
            {
                Console.WriteLine($"Blad w Imieniu: {ex.Message}");
            }
            catch (InvalidNazwiskoException ex)
            {
                Console.WriteLine($"Blad w Nazwisku: {ex.Message}");
            }
            catch (InvalidPeselException ex)
            {
                Console.WriteLine($"Blad w Peselu: {ex.Message}");
            }
            catch (InvalidNameException ex)
            {
                Console.WriteLine($"Blad w Nazwie Pokoju: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystapil nieoczekiwany blad: {ex.Message}");
            }

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wyjść...");
            Console.ReadKey();
        }
    }
}