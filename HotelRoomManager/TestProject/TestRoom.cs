using HotelRoomManager;
using System.Text.RegularExpressions;

namespace TestProject
{
    [TestClass]
    public class RoomTests
    {
        [TestMethod]
        public void TestDataRoom()
        {
            Room room = new StandardRoom(EnumRoomKind.Single, "Alpha", 200m);
            Assert.AreEqual("Alpha", room.Name);
            Assert.AreEqual(EnumRoomKind.Single, room.RoomKind);
            Assert.AreEqual(200m, room.Price);
            Assert.IsNotNull(room.RoomID);
            Assert.AreEqual(0, room.AssignedGuests.Count);
        }

        [TestMethod]
        public void TestPriceBelowMin()
        {
            Assert.ThrowsException<InvalidRoomDataException>(() =>
            {
                Room room = new StandardRoom(EnumRoomKind.Single, "Alpha", 50m);
            });
        }

        [TestMethod]
        public void TestAddRemoveGuest()
        {
            Room room = new StandardRoom(EnumRoomKind.Single, "Alpha", 200m);
            Guest guest = new Guest("John", "Smith", "12345678901", 3);
            room.DodajGoscia(guest);
            Assert.AreEqual(1, room.AssignedGuests.Count);
            room.UsunGoscia("12345678901");
            Assert.AreEqual(0, room.AssignedGuests.Count);
        }

        [TestMethod]
        public void TestOdswiezGosci()
        {
            Room room = new StandardRoom(EnumRoomKind.Single, "Alpha", 200m);
            Guest pastGuest = new Guest("John", "Smith", "12345678901", -1); 
            Guest futureGuest = new Guest("Jane", "Doe", "98765432109", 5);  
            room.DodajGoscia(pastGuest);
            room.DodajGoscia(futureGuest);
            room.OdswiezGosci();
            Assert.AreEqual(1, room.AssignedGuests.Count);
        }

        [TestMethod]
        public void TestEquals()
        {
            Room room1 = new StandardRoom(EnumRoomKind.Single, "Alpha", 200m);
            Room room2 = new StandardRoom(EnumRoomKind.Single, "Beta", 300m);
            Assert.IsFalse(room1.Equals(room2));
        }

        [TestMethod]
        public void TestCompareTo()
        {
            Room room1 = new StandardRoom(EnumRoomKind.Single, "Alpha", 200m);
            Room room2 = new StandardRoom(EnumRoomKind.Single, "Beta", 300m);
            Assert.IsTrue(room2.CompareTo(room1) < 0);
            Assert.IsTrue(room1.CompareTo(room2) > 0);
        }
    }
}
