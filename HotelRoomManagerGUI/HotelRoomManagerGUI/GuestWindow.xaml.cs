using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;
using HotelRoomManager;

namespace HotelRoomManagerGUI
{
    /// <summary>
    /// Logika interakcji dla klasy GuestWindow.xaml
    /// </summary>
    public partial class GuestWindow : Window
    {
        private Guest gosc;
        public Room WybranyPokoj { get; set; }
        public GuestWindow()
        {
            InitializeComponent();
        }

        public GuestWindow(Guest g, List<Room> wszystkiePokoje) : this()
        {
            this.gosc = g;
            CmbPokoj.ItemsSource = wszystkiePokoje;
            CmbPokoj.DisplayMemberPath = "Name";
            TxtPESEL.Text = gosc.Pesel ?? "";
            TxtImie.Text = gosc.Imie ?? "";
            TxtNazwisko.Text = gosc.Nazwisko ?? "";
            TxtDataZameldowania.Text = gosc.CheckInDate.ToString("dd-MM-yyyy");
            TxtDataWymeldowania.Text = gosc.CheckOutDate.ToString("dd-MM-yyyy");


            CmbPokoj.SelectedIndex = -1;
            this.CmbPokoj.SelectedItem = wszystkiePokoje.FirstOrDefault(r => r.AssignedGuests.Any(guest => guest.Pesel == g.Pesel));
        }



        private void BtnZatwierdz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(TxtPESEL.Text) &&
                    !string.IsNullOrWhiteSpace(TxtImie.Text) &&
                    !string.IsNullOrWhiteSpace(TxtNazwisko.Text))
                {
                    gosc.Imie = TxtImie.Text;
                    gosc.Nazwisko = TxtNazwisko.Text;
                    gosc.Pesel = TxtPESEL.Text;
                    string[] formaty = { "yyyy-MM-dd", "yyyy/MM/dd", "dd-MM-yyyy", "dd.MM.yyyy" };
                    if (DateTime.TryParseExact(TxtDataZameldowania.Text, formaty, null, System.Globalization.DateTimeStyles.None, out DateTime d1))
                    {
                        gosc.CheckInDate = d1;
                    }
                    else
                    {
                        MessageBox.Show("Niepoprawny format daty zameldowania (użyj rrrr-mm-dd)");
                        return;
                    }
                    if (DateTime.TryParseExact(TxtDataWymeldowania.Text, formaty, null, System.Globalization.DateTimeStyles.None, out DateTime d2))
                    {
                        if (d2 <= gosc.CheckInDate)
                        {
                            MessageBox.Show("Data wymeldowania musi być późniejsza niż data zameldowania");
                            return;
                        }
                        gosc.CheckOutDate = d2;
                    }
                    else
                    {
                        MessageBox.Show("Niepoprawny format daty wymeldowania (użyj rrrr-mm-dd)");
                        return;
                    }
                    if (CmbPokoj.SelectedItem is Room wybrany)
                    {
                        WybranyPokoj = wybrany;
                    }
                    else
                    {
                        MessageBox.Show("Proszę wybrać pokój");
                        return;
                    }
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Wszystkie pola (Imię, Nazwisko, PESEL) muszą być wypełnione");
                }
            }
            catch (InvalidImieException ex)
            {
                MessageBox.Show("Błąd Imienia: " + ex.Message);
            }
            catch (InvalidNazwiskoException ex)
            {
                MessageBox.Show("Błąd Nazwiska: " + ex.Message);
            }
            catch (InvalidPeselException ex)
            {
                MessageBox.Show("Błąd PESEL: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił nieoczekiwany błąd: " + ex.Message);
            }
        }

        private void BtnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
