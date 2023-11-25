using Microsoft.Win32;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;
namespace Announcement_Platform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loading.Visibility = Visibility.Hidden;
            string gifPath = AppDomain.CurrentDomain.BaseDirectory + "loading.gif";
            var bitmap = new BitmapImage(new Uri(gifPath));
            //string AccPath = AppDomain.CurrentDomain.BaseDirectory + "AccChoose.jpg";
            //var bitmap1 = new BitmapImage(new Uri(AccPath));
            ImageBehavior.SetAnimatedSource(loadingImage, bitmap);
            ImageBehavior.SetRepeatBehavior(loadingImage, System.Windows.Media.Animation.RepeatBehavior.Forever);
            ImageBehavior.SetAnimationSpeedRatio(loadingImage, 0.1); // Ustaw prędkość animacji na 50%
            LoadDataAsync();
            
        }
        private async void LoadDataAsync()
        {
          
            DoubleAnimation progressBarAnimation = new DoubleAnimation
            {
                From = progressBar.Minimum,
                To = progressBar.Maximum,
                Duration = TimeSpan.FromSeconds(10),
                RepeatBehavior = RepeatBehavior.Forever
            };

           
            progressBarAnimation.Completed += (sender, e) =>
            {
                progressBar.Value = progressBar.Minimum;
            };

            
            progressBar.BeginAnimation(ProgressBar.ValueProperty, progressBarAnimation);

          
            await Task.Delay(10000);

            
            Dispatcher.Invoke(() =>
            {
                progressBar.BeginAnimation(ProgressBar.ValueProperty, null);
            loading.Visibility = Visibility.Hidden;
               
                // Tutaj możesz dodać kod do przejścia do głównego ekranu aplikacji lub innych operacji po zakończeniu ładowania
            });
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {

            // Pobierz dane z formularza
            string login = RegisterLoginTextBox.Text;
            string password = RegisterPasswordBox.Password;
            bool isAdmin = (bool)AdminChoose.IsChecked;

            // Sprawdź, czy użytkownik o podanym loginie już istnieje w bazie danych
            var existingUser = App.Database.GetItemsAsync<Account>().Result.FirstOrDefault(u => u.Login == login);

            if (existingUser == null)
            {
                // Dodaj nowego użytkownika do bazy danych
                var newUser = new Account { Login = login, Password = password, IsAdmin = isAdmin };
                App.Database.SaveItemAsync(newUser).Wait(); // Oczekiwanie na zakończenie operacji

                // Komunikat o udanej rejestracji
                MessageBox.Show("Rejestracja udana!");
               

                // Opcjonalnie: Wyczyść pola formularza po udanej rejestracji
                RegisterLoginTextBox.Text = string.Empty;
                RegisterPasswordBox.Password = string.Empty;
                AdminChoose.IsChecked = false;
            }
            else
            {
                // Komunikat o zajętym loginie
                MessageBox.Show("Użytkownik o podanym loginie już istnieje.");
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Pobierz dane z formularza
                string login = LoginTextBox.Text;
                string password = PasswordBox.Password;

                // Sprawdź, czy istnieje użytkownik o podanym loginie
                var user = App.Database.GetItemsAsync<Account>().Result.FirstOrDefault(u => u.Login == login);

                if (user != null)
                {
                    // Sprawdź, czy hasło się zgadza
                    if (user != null && user.Password == password)
                    {
                        // Ustaw flagę na true po pomyślnym zalogowaniu
                        user.IsLoggedIn = true;
                        App.Database.SaveItemAsync(user).Wait();

                        MessageBox.Show("Zalogowano pomyślnie!");
                        LoginBtn.Visibility = Visibility.Hidden;
                        RegisterBtn.Visibility = Visibility.Hidden;
                        ProfileNav.Visibility = Visibility.Visible;
                        LogOutBtn.Visibility = Visibility.Visible;
                        // Opcjonalnie: Przejście do kolejnego widoku lub wykonanie innych działań po zalogowaniu
                    }
                    else
                    {
                        // Komunikat o błędnym haśle
                        MessageBox.Show("Błędne hasło!");
                    }
                }
                else
                {
                    // Komunikat o braku użytkownika o podanym loginie
                    MessageBox.Show("Brak użytkownika o podanym loginie!");
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędów (np. błąd bazy danych)
                MessageBox.Show($"Wystąpił błąd: {ex.Message}");
            }
        }
        private void Logout()
        {
            // Pobierz zalogowanego użytkownika (możesz także użyć innej logiki do określenia zalogowanego użytkownika)
            var loggedInUser = App.Database.GetItemsAsync<Account>().Result.FirstOrDefault(u => u.IsLoggedIn);

            if (loggedInUser != null)
            {
                // Ustaw flagę na false po wylogowaniu
                loggedInUser.IsLoggedIn = false;
                App.Database.SaveItemAsync(loggedInUser).Wait();

                MessageBox.Show("Wylogowano pomyślnie!");
                // Opcjonalnie: Przejście do innego widoku lub wykonanie innych działań po wylogowaniu
                LoginBtn.Visibility = Visibility.Visible;
                RegisterBtn.Visibility = Visibility.Visible;
                ProfileNav.Visibility = Visibility.Hidden;
                LogOutBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                // Komunikat o braku zalogowanego użytkownika
                MessageBox.Show("Nie ma zalogowanego użytkownika.");
            }
        }

        private void AnnouncementNav_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void LogInNav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RegisterNav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConfirmProfileBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProfileNav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {

        }

     
    }
}
