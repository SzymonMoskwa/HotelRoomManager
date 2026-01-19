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
using System.Windows.Shapes;
using HotelRoomManager;

namespace HotelRoomManagerGUI
{
    /// <summary>
    /// Logika interakcji dla klasy TworzeniePokoji.xaml
    /// </summary>
    public partial class TworzeniePokoji : Window
    {
        private Hotel h1;

        private Room edytowanyPokoj;

        public TworzeniePokoji(Hotel h1, Room pokojDoEdycji = null)
        {
            InitializeComponent();
            this.h1 = h1;
            this.edytowanyPokoj = pokojDoEdycji;

            CmbType.ItemsSource = Enum.GetValues(typeof(EnumRoomKind));

            if (edytowanyPokoj != null)
            {
                this.Title = "Edytuj Pokój";
                TxtName.Text = edytowanyPokoj.Name;
                TxtPrice.Text = edytowanyPokoj.Price.ToString();
                CmbType.SelectedItem = edytowanyPokoj.RoomKind;

                RdSuite.IsEnabled = false;
                RdStandard.IsEnabled = false;

                if (edytowanyPokoj is SuiteRoom) RdSuite.IsChecked = true;
                else RdStandard.IsChecked = true;
            }
            else
            {
                CmbType.SelectedIndex = 0;
                RdStandard.IsChecked = true;
            }
        }

        private void BtnZatwierdz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(TxtPrice.Text, out decimal cena)) return;

                if (edytowanyPokoj != null)
                {
                    edytowanyPokoj.Name = TxtName.Text;
                    edytowanyPokoj.Price = cena;
                    edytowanyPokoj.RoomKind = (EnumRoomKind)CmbType.SelectedItem;
                }
                else
                {
                    EnumRoomKind wybranyTyp = (EnumRoomKind)CmbType.SelectedItem;
                    Room nowyPokoj = (RdSuite.IsChecked == true)
                        ? new SuiteRoom(wybranyTyp, TxtName.Text, cena)
                        : new StandardRoom(wybranyTyp, TxtName.Text, cena);

                    h1.DodajPokoj(nowyPokoj);
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (InvalidRoomDataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message);
            }
        }


        private void BtnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
