using HotelRoomManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using TestProject;

namespace TestProject
{
    [TestClass]
    public class HotelTests
    {
        [TestMethod]
        public void TestEmptyList()
        {
            Hotel hotel = new Hotel("TestHotel");
            Assert.IsNotNull(hotel.Rooms);
            Assert.AreEqual(0, hotel.Rooms.Count);
        }

        [TestMethod]
        public void TestDodajPokoj()
        {
            Hotel hotel = new Hotel("TestHotel");
            Room room = new StandardRoom(EnumRoomKind.Single, "Alpha", 200m);
            hotel.DodajPokoj(room);
            hotel.DodajPokoj(room);
            Assert.AreEqual(1, hotel.Rooms.Count);
        }

        [TestMethod]
        public void TestUsunPokoj()
        {
            Hotel hotel = new Hotel("TestHotel");
            Room room = new StandardRoom(EnumRoomKind.Single, "Alpha", 200m);
            hotel.DodajPokoj(room);
            hotel.UsunPokoj(room);
            Assert.AreEqual(0, hotel.Rooms.Count);
        }

        [TestMethod]
        public void TestWybierzPokojeTypu()
        {
            Hotel hotel = new Hotel("TestHotel");
            hotel.DodajPokoj(new StandardRoom(EnumRoomKind.Single, "A", 150m));
            hotel.DodajPokoj(new StandardRoom(EnumRoomKind.Double, "B", 120m));
            var result = hotel.WybierzPokojeTypu(EnumRoomKind.Single);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestPrzydzielGoscia()
        {
            Hotel hotel = new Hotel("TestHotel");
            Room room = new StandardRoom(EnumRoomKind.Single, "Alpha", 200m);
            hotel.DodajPokoj(room);
            Guest guest = new Guest("Jan", "Kowalski", "12345678901", 3);
            hotel.PrzydzielGoscia(room.RoomID, guest);
            Assert.AreEqual(1, room.AssignedGuests.Count);
            Assert.AreSame(guest, room.AssignedGuests[0]);
        }

        [TestMethod]
        public void TestTotalIncome()
        {
            Hotel hotel = new Hotel("TestHotel");
            Room room = new StandardRoom(EnumRoomKind.Single, "Alpha", 100m);
            hotel.DodajPokoj(room);
            Guest guest = new Guest("Jan", "Kowalski", "12345678901", 2);
            room.DodajGoscia(guest);
            decimal income = hotel.TotalActualIncome();
            Assert.AreEqual(200m, income);
        }

        [TestMethod]
        public void TestSortRoomsByPriceDescending()
        {
            Hotel hotel = new Hotel("TestHotel");
            hotel.DodajPokoj(new StandardRoom(EnumRoomKind.Single, "A", 100m));
            hotel.DodajPokoj(new StandardRoom(EnumRoomKind.Single, "B", 300m));
            hotel.DodajPokoj(new StandardRoom(EnumRoomKind.Single, "C", 200m));
            hotel.SortRoomsByPriceDescending();
            Assert.AreEqual(300m, hotel.Rooms[0].Price);
            Assert.AreEqual(200m, hotel.Rooms[1].Price);
            Assert.AreEqual(100m, hotel.Rooms[2].Price);
        }

        [TestMethod]
        public void TestSaveAndReadXML()
        {
            Hotel hotel = new Hotel("TestHotel");
            hotel.DodajPokoj(new StandardRoom(EnumRoomKind.Single, "Alpha", 200m));
            string file = "hotel_test.xml";
            hotel.DCSaveToXML(file);
            Hotel loaded = Hotel.DCReadFromXML(file);
            Assert.AreEqual(1, loaded.Rooms.Count);
            File.Delete(file);
        }
    }
}
