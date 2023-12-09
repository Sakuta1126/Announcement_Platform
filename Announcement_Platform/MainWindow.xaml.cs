using Announcement_Platform;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
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
        public Visibility isvisible { get; set; } = Visibility.Hidden;
        public MainWindow()
        {
            InitializeComponent();
             
              Announcements = new ObservableCollection<Announcement>();
            AppliedAnnouncements = new ObservableCollection<Applied>();
            YourItemsControl.ItemsSource = Announcements;
            AppliedItems.ItemsSource = AppliedAnnouncements;
      
                  timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(11);
            timer.Tick += Timer_Tick;

       
            timer.Start();

           
            loading.Visibility = Visibility.Visible;
            string gifPath = AppDomain.CurrentDomain.BaseDirectory + "loading.gif";
            var bitmap = new BitmapImage(new Uri(gifPath));
           
            ImageBehavior.SetAnimatedSource(loadingImage, bitmap);
            ImageBehavior.SetRepeatBehavior(loadingImage, System.Windows.Media.Animation.RepeatBehavior.Forever);
            ImageBehavior.SetAnimationSpeedRatio(loadingImage, 0.1); 
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

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Login i hasło nie mogą być puste.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (existingUser == null)
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
            
                MessageBox.Show("Użytkownik o podanym loginie już istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        UserStore.SetLoggedInUserId(user.Id);
                        App.Database.SaveItemAsync(user).Wait();
                        MessageBox.Show("Zalogowano pomyślnie!");
                   
                        LoginTextBox.Text = string.Empty;
                        PasswordBox.Password = string.Empty;
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

          
            Announcements.Clear();

           
            foreach (var announcement in announcements)
            {
                Announcements.Add(announcement);
            }
        }
        private async void LoadAppliedAsync()
        {
            
            List<Applied> allApplied = await App.Database.GetItemsAsync<Applied>();

            var loggedInUser = UserStore.LoggedInUserId;



            if (loggedInUser != 0)
            {
             
                List<Applied> userApplied = allApplied.Where(apply => apply.user_id == loggedInUser).ToList();

               
                AppliedAnnouncements.Clear();

                
                foreach (var userApply in userApplied)
                {
                    AppliedAnnouncements.Add(userApply);
                }
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
                Applied.Visibility = Visibility.Hidden;
                AddAnno.Visibility = Visibility.Hidden;
            }
            else
            {
                
                MessageBox.Show("Nie ma zalogowanego użytkownika.");
            }
        }

        private void AnnouncementNav_Click(object sender, RoutedEventArgs e)
        {
            LoadAnnouncementsAsync();
            UpdateAnnoButtonsVisibility();
            announcements.Visibility = Visibility.Visible;
            MainSiteView.Visibility = Visibility.Visible;
            AppliedAnno.Visibility = Visibility.Hidden;
            logo.Visibility = Visibility.Hidden;

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
            // Sprawdź, czy użytkownik już wypełnił profil
            var existingUser = App.Database.GetItemsAsync<User>().Result.FirstOrDefault(u => u.user_id == UserStore.LoggedInUserId);

            if (existingUser != null)
            {
                MessageBox.Show("Profil użytkownika został już wypełniony.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Applied.Visibility = Visibility.Visible;
                AnnouncementNav.Visibility = Visibility.Visible;
                Profile_Informations.Visibility = Visibility.Hidden;
                Applied.Visibility = Visibility.Visible;
                ProfileNav.Visibility = Visibility.Hidden;
                AnnouncementNav.Visibility = Visibility.Visible;
                MainSiteView.Visibility = Visibility.Visible;
                UpdateAddAnnoButtonVisibility();
                return;
            }

       
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
                PFP = Photopath.Text,
                user_id = UserStore.LoggedInUserId
            };

            App.Database.SaveItemAsync(user);

            Lang lang = new Lang
            {
                Language = Lang.Text,
                LanguageLevel = (LangLevel.SelectedItem as ComboBoxItem)?.Content.ToString(),
                user_id = UserStore.LoggedInUserId
            };
            var userProfileInfo = new UserProfileInfo
            {
                User = user,
                Lang = lang
            };

            ShowUserProfileInfo(userProfileInfo);

            App.Database.SaveItemAsync(lang);
            MessageBox.Show("Pomyślnie dodano użytkownika!");
            Name.Text = string.Empty;
            Surname.Text = string.Empty;
            PhoneNumber.Text = string.Empty;
            ResidenceAddress.Text = string.Empty;
            BirthDate.SelectedDate = null;
            Email.Text = string.Empty;
            Summary.Text = string.Empty;
            Lang.Text = string.Empty;
            LangLevel.SelectedIndex = -1;
            Photopath.Text = string.Empty;
            UpdateAddAnnoButtonVisibility();
            Applied.Visibility = Visibility.Visible;
            AnnouncementNav.Visibility = Visibility.Visible;
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
        
        private void UpdateAnnoButtonsVisibility()
        {
            var loggedInUserId = UserStore.LoggedInUserId;

       
            var loggedInUser = App.Database.GetItemsAsync<Account>().Result.FirstOrDefault(u => u.Id == loggedInUserId);

            bool isAdmin = loggedInUser != null && loggedInUser.IsAdmin;
                    MessageBox.Show(isAdmin.ToString());
           
           
            if (loggedInUser != null && loggedInUser.IsAdmin)
            {
                isvisible = Visibility.Visible;
            }
            else
            {

                isvisible = Visibility.Hidden;
            }
            
        }


        private void UpdateAddAnnoButtonVisibility()
        {
            var loggedInUserId = UserStore.LoggedInUserId;

           
            var loggedInUser = App.Database.GetItemsAsync<Account>().Result.FirstOrDefault(u => u.Id == loggedInUserId);

            if (loggedInUser != null && loggedInUser.IsAdmin)
            {
                AddAnno.Visibility = Visibility.Visible;
            }
            else
            {
              
                AddAnno.Visibility = Visibility.Hidden;
            }
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

              
                DeleteRelatedApplied(selectedAnnouncement.Id);
            }
            else
            {
                MessageBox.Show("Błąd podczas uzyskiwania informacji o ogłoszeniu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteRelatedApplied(int announcementId)
        {
         
            List<Applied> relatedApplied = App.Database.GetItemsAsync<Applied>().Result
                .Where(applied => applied.announcement_id == announcementId).ToList();

           
            foreach (var applied in relatedApplied)
            {
                App.Database.DeleteItemAsync(applied);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
           
            string searchText = searchbox.Text.ToLower();

          
            string selectedLevel = GetSelectedComboBoxValue(PositionLevel);
            string selectedContract = GetSelectedComboBoxValue(ContractType);
            string selectedWorkingType = GetSelectedComboBoxValue(Workingtype);
            string selectedWorkingDimension = GetSelectedComboBoxValue(WorkingDimension);

           
            var announcements = Announcements.ToList();

           
            var filteredAnnouncements = announcements.Where(ann =>
                (string.IsNullOrEmpty(searchText) || ann.PositionName.ToLower().Contains(searchText)) &&
                (string.IsNullOrEmpty(selectedLevel) || ann.PositionLevel.ToLower() == selectedLevel) &&
                (string.IsNullOrEmpty(selectedContract) || ann.ContractType.ToLower() == selectedContract) &&
                (string.IsNullOrEmpty(selectedWorkingType) || ann.Workingtype.ToLower() == selectedWorkingType) &&
                (string.IsNullOrEmpty(selectedWorkingDimension) || ann.WorkingDimension.ToLower() == selectedWorkingDimension)
            ).ToList();

          
            YourItemsControl.ItemsSource = filteredAnnouncements;
        }

        private string GetSelectedComboBoxValue(ComboBox comboBox)
        {
          
            var selectedItem = comboBox.SelectedItem as ComboBoxItem;
            return selectedItem?.Content?.ToString()?.ToLower() ?? string.Empty;
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
           
            searchbox.Text = "";

           
            ClearComboBoxes();

          
            YourItemsControl.ItemsSource = Announcements;
        }

        private void ClearComboBoxes()
        {
         
            ClearComboBox(ContractType);
            ClearComboBox(PositionLevel);
            ClearComboBox(Workingtype);
            ClearComboBox(WorkingDimension);
        }

        private void ClearComboBox(ComboBox comboBox)
        {
         
            comboBox.SelectedIndex = -1;
        }


        private void Applied_Click(object sender, RoutedEventArgs e)
        {
            AppliedAnno.Visibility = Visibility.Visible;
            logo.Visibility = Visibility.Hidden;
            announcements.Visibility = Visibility.Hidden;
            LoadAppliedAsync();

        }

       
    }
}

