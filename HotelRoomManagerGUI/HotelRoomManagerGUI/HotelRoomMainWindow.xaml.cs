using System;
using System.Collections.Generic;
using System.Linq;
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
using HotelRoomManager;

namespace HotelRoomManagerGUI
{
    /// <summary>
    /// Logika interakcji dla klasy HotelRoomMainWindow.xaml
    /// </summary>
    public partial class HotelRoomMainWindow : Page
    {
        private Hotel h1;

        public HotelRoomMainWindow(Hotel hotel)
        {
            InitializeComponent();
            this.h1 = hotel;



            if (h1 != null)
            {
                TxtNazwaHotelu.Text = h1.Name;
                OdswiezWidokPokoi();
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


        private void RoomButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var data = button.DataContext;

            if (data is string && (string)data == "ADD_BUTTON")
            {
                TworzeniePokoji  oknoDodawania = new TworzeniePokoji(h1);
                if (oknoDodawania.ShowDialog() == true)
                {
                    Hotel.CzyZmienionoDane = true;
                    OdswiezWidokPokoi();
                    AktualizujStatus($"Stworzono nowy hotel");
                }
            }
            else if (data is Room pokoj)
            {

                TworzeniePokoji oknoEdycji = new TworzeniePokoji(h1, pokoj);
                if (oknoEdycji.ShowDialog() == true)
                {
                    Hotel.CzyZmienionoDane = true;
                    OdswiezWidokPokoi();
                    AktualizujStatus($"Edytowano dane hotelu");
                }
            }
        }

        private void MenuOtworzXml_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "Pliki XML (*.xml)|*.xml";
                if (dlg.ShowDialog() == true)
                {
                    h1 = Hotel.DCReadFromXML(dlg.FileName);
                    TxtNazwaHotelu.Text = h1.Name;
                    OdswiezWidokPokoi();
                    AktualizujStatus($"Wczytano z XML: {dlg.SafeFileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odczytu XML: " + ex.Message);
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
                    OdswiezWidokPokoi();
                    AktualizujStatus("Wczytano dane z bazy SQL.");
                    MessageBox.Show("Dane zostały wczytane z bazy danych.");
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
                AktualizujStatus("Zapisano zmiany w bazie SQL.");
                MessageBox.Show("Dane zostały zapisane w bazie danych.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd zapisu do bazy: {ex.Message}");
            }
        }

        private void MenuZapiszXml_Click(object sender, RoutedEventArgs e)
        {
            if (h1 == null) return;

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Pliki XML (*.xml)|*.xml";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    h1.Name = TxtNazwaHotelu.Text;
                    h1.DCSaveToXML(dlg.FileName);
                    Hotel.CzyZmienionoDane = false;
                    AktualizujStatus($"Zapisano do: {dlg.SafeFileName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd zapisu XML: " + ex.Message);
                }
            }
        }


        private void OdswiezWidokPokoi()
        {
            if (h1 == null) return;

            var wyswietlanaLista = h1.Rooms.Cast<object>().ToList();
            wyswietlanaLista.Add("ADD_BUTTON");

            RoomsControl.ItemsSource = wyswietlanaLista;
        }





        private void AktualizujStatus(string komunikat)
        {
            StatusText.Text = komunikat;
        }


    }
}
