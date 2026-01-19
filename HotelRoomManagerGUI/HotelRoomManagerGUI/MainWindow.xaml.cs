using System.Text;
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

namespace HotelRoomManagerGUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public Hotel WspolnyHotel { get; set; }
    private string sciezkastart = @"C:\Users\rando\Desktop\C# projekt\HotelRoomManager\HotelRoomManager\bin\Debug\net8.0\stan_hotelu.xml";

    public MainWindow()
    {
        InitializeComponent();
        ZaladujHotel();
    }

    private void GifPlayer_MediaEnded(object sender, RoutedEventArgs e)
    {
        GifPlayer.Position = TimeSpan.FromMilliseconds(1);
        GifPlayer2.Position = TimeSpan.FromMilliseconds(1);

        GifPlayer.Play();
        GifPlayer2.Play();
    }


    private void PlayClickSound()
    {
        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media", "door-1-open.wav");
        if (System.IO.File.Exists(path))
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(path);
            player.Play();
        }
    }


    private void ZaladujHotel()
    {
        try
        {
            if (System.IO.File.Exists(sciezkastart))
            {
                WspolnyHotel = (Hotel)Hotel.DCReadFromXML(sciezkastart);
            }
            else
            {
                WspolnyHotel = new Hotel("Nazwa Twojego Hotelu");
                MessageBox.Show("Nie znaleziono pliku stan_hotelu.xml. Utworzono nowy hotel.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Błąd podczas ładowania: " + ex.Message);
        }
    }

    private void BtnPokoje_Click(object sender, RoutedEventArgs e)
    {
        PlayClickSound();
        MainMenuPanel.Visibility = Visibility.Collapsed;
        MainFrame.Navigate(new HotelRoomMainWindow(WspolnyHotel));
    }

    private void BtnGoscie_Click(object sender, RoutedEventArgs e)
    {
        PlayClickSound();
        MainMenuPanel.Visibility = Visibility.Collapsed;
        MainFrame.Navigate(new GuestMainWindow(WspolnyHotel));
    }

    private void BtnWyjdz_Click(object sender, RoutedEventArgs e)
    {
        PlayClickSound();
        Application.Current.Shutdown();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (Hotel.CzyZmienionoDane)
        {
            var wynik = MessageBox.Show("Wprowadzono zmiany w pokojach lub gościach. Zapisać przed wyjściem?",
                                        "Zapisywanie", MessageBoxButton.YesNoCancel);

            if (wynik == MessageBoxResult.Yes)
            {
                try
                {
                    WspolnyHotel.DCSaveToXML(sciezkastart);

                    Hotel.CzyZmienionoDane = false;
                    MessageBox.Show("Dane zostały pomyślnie zapisane.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas zapisu: " + ex.Message);
                    e.Cancel = true;
                }
            }
            else if (wynik == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }




}