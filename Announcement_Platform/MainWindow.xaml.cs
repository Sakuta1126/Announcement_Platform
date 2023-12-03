using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;
using WpfAnimatedGif;
namespace Announcement_Platform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        public ObservableCollection<Announcement> Announcements { get; set; } 
        public ObservableCollection<Applied> AppliedAnnouncements { get; set; }

        public MainWindow()
        {
            InitializeComponent();
           App.Database.ClearDatabaseAsync();

            Announcements = new ObservableCollection<Announcement>();
            AppliedAnnouncements = new ObservableCollection<Applied>();
            YourItemsControl.ItemsSource = Announcements;
            AppliedItems.ItemsSource = AppliedAnnouncements;
      
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(11);
            timer.Tick += Timer_Tick;

       
            timer.Start();

           
            loading.Visibility = Visibility.Hidden;
            string gifPath = AppDomain.CurrentDomain.BaseDirectory + "loading.gif";
            var bitmap = new BitmapImage(new Uri(gifPath));
           
            ImageBehavior.SetAnimatedSource(loadingImage, bitmap);
            ImageBehavior.SetRepeatBehavior(loadingImage, System.Windows.Media.Animation.RepeatBehavior.Forever);
            ImageBehavior.SetAnimationSpeedRatio(loadingImage, 0.1); // Ustaw prędkość animacji na 50%
            LoadDataAsync();
            
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
    
            timer.Stop();

  
            var videoWindow = new Video("Projekt.mp4");
            videoWindow.ShowDialog();
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
                MainSiteView.Visibility=Visibility.Visible;

               
            });
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {

       
            string login = RegisterLoginTextBox.Text;
            string password = RegisterPasswordBox.Password;
            bool isAdmin = (bool)AdminChoose.IsChecked;

           
            var existingUser = App.Database.GetItemsAsync<Account>().Result.FirstOrDefault(u => u.Login == login);

            if (existingUser == null && RegisterLoginTextBox.Text != "" || RegisterPasswordBox.Password != "")
            {
              
                var newUser = new Account { Login = login, Password = password, IsAdmin = isAdmin };
               
                App.Database.SaveItemAsync(newUser).Wait();

                MessageBox.Show("Rejestracja udana!");
                RegisterView.Visibility = Visibility.Hidden;
                MainSiteView.Visibility = Visibility.Visible;
               

         
                RegisterLoginTextBox.Text = string.Empty;
                RegisterPasswordBox.Password = string.Empty;
                AdminChoose.IsChecked = false;
            }
            else 
            {
            
                MessageBox.Show("Uzytkownik o podanym loginie istnieje.");
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
           
                string login = LoginTextBox.Text;
                string password = PasswordBox.Password;

                var user = App.Database.GetItemsAsync<Account>().Result.FirstOrDefault(u => u.Login == login);

                if (user != null)
                {
                   
                    if (user != null && user.Password == password)
                    {
                  
                        user.IsLoggedIn = true;
                        App.Database.SaveItemAsync(user).Wait();
                        
                        MessageBox.Show("Zalogowano pomyślnie!");
                        LoginView.Visibility = Visibility.Hidden;
                        MainSiteView.Visibility = Visibility.Visible;
                        RegisterNav.Visibility = Visibility.Hidden;
                        LogInNav.Visibility = Visibility.Hidden;
                        ProfileNav.Visibility = Visibility.Visible;
                        LogOutBtn.Visibility = Visibility.Visible;
                       
                   
                    }
                    else
                    {
                        
                        MessageBox.Show("Błędne hasło!");
                    }
                }
                else
                {
               
                    MessageBox.Show("Brak użytkownika o podanym loginie!");
                }
            }
            catch (Exception ex)
            {
            
                MessageBox.Show($"Wystąpił błąd: {ex.Message}");
            }
        }
        private async void LoadAnnouncementsAsync()
        {
            List<Announcement> announcements = await App.Database.GetItemsAsync<Announcement>();

            // Wyczyść kolekcję przed dodaniem nowych ogłoszeń
            Announcements.Clear();

            // Dodaj ogłoszenia do kolekcji
            foreach (var announcement in announcements)
            {
                Announcements.Add(announcement);
            }
        }
        private async void LoadAppliedAsync()
        {
            List<Applied> applied = await App.Database.GetItemsAsync<Applied>();

            AppliedAnnouncements.Clear();

            // Dodaj ogłoszenia w zakresie aplikacji do kolekcji
            foreach (var applys in applied)
            {
                AppliedAnnouncements.Add(applys);
            }
        }
        private void Logout()
        {
            
            var loggedInUser = App.Database.GetItemsAsync<Account>().Result.FirstOrDefault(u => u.IsLoggedIn);

            if (loggedInUser != null)
            {
            
                loggedInUser.IsLoggedIn = false;
                App.Database.SaveItemAsync(loggedInUser).Wait();

                MessageBox.Show("Wylogowano pomyślnie!");
              
                LogInNav.Visibility = Visibility.Visible;
                RegisterNav.Visibility = Visibility.Visible;
                ProfileNav.Visibility = Visibility.Hidden;
                LogOutBtn.Visibility = Visibility.Hidden;
                AnnouncementNav.Visibility= Visibility.Hidden;
                announcements.Visibility = Visibility.Hidden;
                MainSiteView.Visibility=Visibility.Visible;
            }
            else
            {
                
                MessageBox.Show("Nie ma zalogowanego użytkownika.");
            }
        }

        private void AnnouncementNav_Click(object sender, RoutedEventArgs e)
        {
            LoadAnnouncementsAsync();
            announcements.Visibility = Visibility.Visible;
            MainSiteView.Visibility = Visibility.Visible;
            AppliedAnno.Visibility = Visibility.Hidden;
            UpdateAnnoButtonsVisibility();


        }

        private void LogInNav_Click(object sender, RoutedEventArgs e)
        {
            LoginView.Visibility = Visibility.Visible;
            MainSiteView.Visibility = Visibility.Hidden;
        }

        private void RegisterNav_Click(object sender, RoutedEventArgs e)
        {
            RegisterView.Visibility = Visibility.Visible;
            MainSiteView.Visibility = Visibility.Hidden;
        }

        private void ConfirmProfileBtn_Click(object sender, RoutedEventArgs e)
        {
        
            if (string.IsNullOrWhiteSpace(Name.Text) ||
                string.IsNullOrWhiteSpace(Surname.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(ResidenceAddress.Text) ||
                BirthDate.SelectedDate == null ||
                string.IsNullOrWhiteSpace(Email.Text) ||
                string.IsNullOrWhiteSpace(Summary.Text) ||
                string.IsNullOrWhiteSpace(Lang.Text) ||
                LangLevel.SelectedItem == null)
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

           
            if (!Regex.IsMatch(Email.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Nieprawidłowy format adresu email.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            if (!Regex.IsMatch(PhoneNumber.Text, @"^[0-9]{9}$"))
            {
                MessageBox.Show("Nieprawidłowy format numeru telefonu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (BirthDate.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Data urodzenia nie może być późniejsza niż dzisiejsza data.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = new User
            {
                Name = Name.Text,
                Surname = Surname.Text,
                BirthDate = BirthDate.SelectedDate.Value,
                Email = Email.Text,
                PhoneNumber = int.Parse(PhoneNumber.Text),
                ResidenceAddress = ResidenceAddress.Text,
                Summary = Summary.Text,
                PFP = Photopath.Text
            };

            App.Database.SaveItemAsync(user);
            Lang lang = new Lang
            {
                Language = Lang.Text,
                LanguageLevel = (LangLevel.SelectedItem as ComboBoxItem)?.Content.ToString()
            };
            var userProfileInfo = new UserProfileInfo
            {
                User = user,
                Lang = lang
            };

       
            ShowUserProfileInfo(userProfileInfo);

    
            App.Database.SaveItemAsync(lang);
            MessageBox.Show("Pomyślnie dodano użytkownika!");
            DisplayUserData();
            UpdateAddAnnoButtonVisibility();
            Profile_Informations.Visibility = Visibility.Hidden;
            Applied.Visibility = Visibility.Visible;
            ProfileNav.Visibility = Visibility.Hidden;
            AnnouncementNav.Visibility = Visibility.Visible;
            MainSiteView.Visibility = Visibility.Visible;
        }
        private void ShowUserProfileInfo(UserProfileInfo userProfileInfo)
        {
        
            UserProfileInfoWindow userProfileInfoWindow = new UserProfileInfoWindow(userProfileInfo);

       
            userProfileInfoWindow.Owner = Window.GetWindow(this);
            
            userProfileInfoWindow.ShowDialog();
        }

        private void ProfileNav_Click(object sender, RoutedEventArgs e)
        {
            MainSiteView.Visibility = Visibility.Hidden;
            Profile_Informations.Visibility = Visibility.Visible;
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            Logout();
        }

        private void ChooseImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki graficzne (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png|Wszystkie pliki (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
             
                Photopath.Text = openFileDialog.FileName;
                ProfileImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        private async void DisplayUserData()
        {
            var users = await App.Database.GetItemsAsync<User>();
            var langs = await App.Database.GetItemsAsync<Lang>();

            foreach (var user in users)
            {
                StringBuilder userMessage = new StringBuilder();
                userMessage.AppendLine($"ID: {user.Id}, Name: {user.Name}, Surname: {user.Surname}, Email: {user.Email}, PhoneNumber: {user.PhoneNumber}, PFP: {user.PFP}, ResidenceAddress: {user.ResidenceAddress}");

            
                var userLangs = langs.Where(lang => lang.Id == user.Id).ToList();
                if (userLangs.Any())
                {
                    userMessage.AppendLine("\nLanguages:");
                    foreach (var lang in userLangs)
                    {
                        userMessage.AppendLine($"- Language: {lang.Language}, Level: {lang.LanguageLevel}");
                    }
                }

                
                MessageBox.Show(userMessage.ToString(), $"Dane użytkownika ID: {user.Id}", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void UpdateAnnoButtonsVisibility()
        {

          
            var loggedInUser = App.Database.GetItemsAsync<Account>().Result.FirstOrDefault(u => u.IsLoggedIn);


            bool isAdmin = loggedInUser != null && loggedInUser.IsAdmin;

            foreach (var item in YourItemsControl.Items)
            {
                if (item is FrameworkElement element)
                {
                    var viewDetailsButton = element.FindName("ViewDetails") as Button;
                    var deleteAnnButton = element.FindName("DeleteAnn") as Button;


                    if (deleteAnnButton != null)
                    {
                        deleteAnnButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Hidden;
                    }
                }
            }


        }
        
        private void UpdateAddAnnoButtonVisibility()
        {
         
          

                var loggedInUser = App.Database.GetItemsAsync<Account>().Result.FirstOrDefault(u => u.IsLoggedIn);

                bool isAdmin = loggedInUser != null && loggedInUser.IsAdmin;

            
                AddAnno.Visibility = isAdmin ? Visibility.Visible : Visibility.Hidden;
         
            
        }
        private void AddAnno_Click(object sender, RoutedEventArgs e)
        {
            AddAnnouncementWindow addAnnouncementWindow = new AddAnnouncementWindow();

            addAnnouncementWindow.Owner = this;


            addAnnouncementWindow.ShowDialog();
        }

      

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {

            Button clickedButton = sender as Button;


            Announcement selectedAnnouncement = clickedButton.DataContext as Announcement;

            if (selectedAnnouncement != null)
            {

                AnnouncementDetailsWindow detailsWindow = new AnnouncementDetailsWindow(selectedAnnouncement);
                detailsWindow.Show();
            }
            else
            {
                MessageBox.Show("Błąd podczas uzyskiwania informacji o ogłoszeniu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteAnn_Click(object sender, RoutedEventArgs e)
        {

            Button clickedButton = sender as Button;

            
            Announcement selectedAnnouncement = clickedButton.DataContext as Announcement;

            if (selectedAnnouncement != null)
            {
                
                App.Database.DeleteItemAsync(selectedAnnouncement);


                Announcements.Remove(selectedAnnouncement);
            }
            else
            {
                MessageBox.Show("Błąd podczas uzyskiwania informacji o ogłoszeniu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        

     

        private void Applied_Click(object sender, RoutedEventArgs e)
        {
            AppliedAnno.Visibility = Visibility.Visible;
            announcements.Visibility = Visibility.Hidden;
            LoadAppliedAsync();

        }

        private void ViewApplied_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

