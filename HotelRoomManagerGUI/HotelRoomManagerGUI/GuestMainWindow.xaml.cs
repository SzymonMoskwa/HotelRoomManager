using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using HotelRoomManager;


namespace HotelRoomManagerGUI
{
    /// <summary>
    /// Logika interakcji dla klasy GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Page
    {
        private Hotel h1;
        public GuestMainWindow(Hotel hotel)
        {
            InitializeComponent();
            this.h1 = hotel;


            if (h1 != null)
            {
                TxtNazwaHotelu.Text = h1.Name;
                OdswiezListeGosci();
            }
        }

        private void BtnWroc_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = null;
                mainWindow.MainMenuPanel.Visibility = Visibility.Visible;
            }
        }
        private void MenuOtworzXml_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    Otworz(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd bazy: " + ex.Message);
            }
        }
        private void MenuOtworzDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                h1 = Hotel.ReadHotelFromDb();
                if (h1 != null)
                {
                    TxtNazwaHotelu.Text = h1.Name;
                    var wszyscyGoscie = h1.Rooms.SelectMany(r => r.AssignedGuests).ToList();
                    listGoscie.ItemsSource = new System.Collections.ObjectModel.ObservableCollection<Guest>(wszyscyGoscie);
                    MessageBox.Show("Dane zostały wczytane z domyślnej bazy danych.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd bazy: " + ex.Message);
            }
        }

        private void MenuZapiszDb_Click(object sender, RoutedEventArgs e)
        {
            if (h1 == null) return;

            try
            {
                h1.Name = TxtNazwaHotelu.Text;
                h1.SaveToDb();
                Hotel.CzyZmienionoDane = false;
                MessageBox.Show("Dane zostały zapisane w bazie danych.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd zapisu do bazy: {ex.Message}");
            }
        }
        private void MenuZapiszXml_Click(object sender, RoutedEventArgs e)
        {
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.Filter = "Pliki XML (*.xml)|*.xml";
                if (dlg.ShowDialog() == true)
                {
                    h1.Name = TxtNazwaHotelu.Text;
                    h1.DCSaveToXML(dlg.FileName);
                    Hotel.CzyZmienionoDane = false;
                }
            }
        }


        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Hotel.CzyZmienionoDane = true;
                if (h1 == null) return;
                Guest nowyGosc = new Guest();

                nowyGosc.Imie = "Imię";
                nowyGosc.Nazwisko = "Nazwisko";
                nowyGosc.Pesel = "00000000000";
                nowyGosc.CheckInDate = DateTime.Now;
                nowyGosc.CheckOutDate = DateTime.Now.AddDays(1);

                GuestWindow okno = new GuestWindow(nowyGosc, h1.Rooms);

                if (okno.ShowDialog() == true)
                {
                    Room wybrany = okno.WybranyPokoj;
                    if (wybrany != null)
                    {
                        wybrany.DodajGoscia(nowyGosc);
                        OdswiezListeGosci();
                        AktualizujStatus($"Dodano gościa: {nowyGosc.Nazwisko}");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd inicjalizacji: " + ex.Message);
            }
        }

        private void BtnSortuj_Click(object sender, RoutedEventArgs e)
        {
            if (h1 == null) return;
                var listaGosci = h1.Rooms.SelectMany(r => r.AssignedGuests).ToList();
                var posortowaniGoscie = listaGosci
                    .OrderBy(g => g.CheckOutDate)
                    .ThenBy(g => g.CheckInDate)
                    .ToList();
            listGoscie.ItemsSource = null;
            listGoscie.ItemsSource = new ObservableCollection<Guest>(posortowaniGoscie);
            AktualizujStatus("Posortowano listę według dat zameldowania");
        }
        private void BtnSortPoPesel_Click(object sender, RoutedEventArgs e)
        {
            if (h1 == null) return;
            var listaGosci = h1.Rooms.SelectMany(r => r.AssignedGuests).ToList();
            var posortowaniGoscie = listaGosci
                .OrderBy(g => g.Pesel)
                .ThenBy(g => g.Nazwisko)
                .ToList();

            listGoscie.ItemsSource = null;
            listGoscie.ItemsSource = new ObservableCollection<Guest>(posortowaniGoscie);
            AktualizujStatus("Posortowano listę według numeru PESEL");
        }


        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (listGoscie.SelectedItems.Count > 0)
            {
                int liczba = listGoscie.SelectedItems.Count;
                foreach (Guest gosc in listGoscie.SelectedItems.Cast<Guest>().ToList())
                {
                    var pokoj = h1.Rooms.FirstOrDefault(r => r.AssignedGuests.Contains(gosc));
                    if (pokoj != null)
                    {
                            pokoj.AssignedGuests.Remove(gosc);
                            Hotel.CzyZmienionoDane = true;
                            
                    }
                    
                }

                OdswiezListeGosci();
                AktualizujStatus($"Usunięto zaznaczonych gości ({liczba})");
            }
        }

        private void BtnZmienGoscia_Click(object sender, RoutedEventArgs e)
        {
            if (listGoscie.SelectedItem is Guest wybrany)
            {
                GuestWindow okno = new GuestWindow(wybrany, h1.Rooms);
                okno.WybranyPokoj = h1.Rooms.FirstOrDefault(r => r.AssignedGuests.Contains(wybrany));
                bool? result = okno.ShowDialog();

                OdswiezListeGosci();
                Hotel.CzyZmienionoDane = true;
                AktualizujStatus($"Hotel.CzyZmienionoDane dane gościa: {wybrany.Nazwisko}");
            }
        }





        private void OdswiezListeGosci()
        {
            if (h1 != null)
            {
                var wszyscyGoscie = h1.Rooms.SelectMany(r => r.AssignedGuests).ToList();
                listGoscie.ItemsSource = new ObservableCollection<Guest>(wszyscyGoscie);
            }
        }

        private void Otworz(string sciezka)
        {
            try
            {
                Hotel wczytanyHotel = (Hotel)Hotel.DCReadFromXML(sciezka);

                if (wczytanyHotel != null)
                {
                    this.h1 = wczytanyHotel;
                    var mw = Window.GetWindow(this) as MainWindow;
                    if (mw != null)
                    {
                        mw.WspolnyHotel = wczytanyHotel;
                    }
                    TxtNazwaHotelu.Text = h1.Name;
                    OdswiezListeGosci();
                    Hotel.CzyZmienionoDane = false;
                    MessageBox.Show("Wczytano nowy stan hotelu. Niezapisane zmiany z poprzedniej sesji zostały odrzucone.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odczytu XML: " + ex.Message);
            }
        }

 
        private void AktualizujStatus(string komunikat)
        {
            StatusText.Text = komunikat;
        }




    }
}
