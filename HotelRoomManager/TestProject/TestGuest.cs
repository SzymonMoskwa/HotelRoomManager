using System.Text.RegularExpressions;
using HotelRoomManager;

namespace TestProject
{
    [TestClass]
    public class GuestTests
    {
        [TestMethod]
        public void TestDataGuest()
        {
            Guest guest = new Guest("Jan", "Kowalski", "12345678901", 3);
            Assert.AreEqual("Jan", guest.Imie);
            Assert.AreEqual("Kowalski", guest.Nazwisko);
            Assert.AreEqual("12345678901", guest.Pesel);
            Assert.IsTrue(guest.CheckOutDate > guest.CheckInDate);
        }

        [TestMethod]
        public void TestImieEmpty()
        {
            Assert.ThrowsException<InvalidImieException>(() =>
            {
                Guest guest = new Guest("", "Kowalski", "12345678901", 3);
            });
        }

        [TestMethod]
        public void TestImieSmallLetter()
        {
            Assert.ThrowsException<InvalidImieException>(() =>
            {
                Guest guest = new Guest("jan", "Kowalski", "12345678901", 3);
            });
        }

        [TestMethod]
        public void TestPeselShort()
        {
            Assert.ThrowsException<InvalidPeselException>(() =>
            {
                Guest guest = new Guest("Jan", "Kowalski", "1234567890", 3);
            });
        }

        [TestMethod]
        public void TestPeselContainsLetter()
        {
            Assert.ThrowsException<InvalidPeselException>(() =>
            {
                Guest guest = new Guest("Jan", "Kowalski", "1234567890A", 3);
            });
        }

        [TestMethod]
        public void Clone_CreatesSeparateObject()
        {
            Guest guest = new Guest("Jan", "Kowalski", "12345678901", 2);
            Guest clone = (Guest)guest.Clone();

            Assert.AreNotSame(guest, clone);           
            Assert.AreEqual(guest.Imie, clone.Imie);   
            Assert.AreEqual(guest.Nazwisko, clone.Nazwisko);
            Assert.AreEqual(guest.Pesel, clone.Pesel);
        }
    }
}
