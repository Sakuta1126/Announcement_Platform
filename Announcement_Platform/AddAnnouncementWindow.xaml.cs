using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Announcement_Platform
{
    /// <summary>
    /// Interaction logic for AddAnnouncementWindow.xaml
    /// </summary>
    public partial class AddAnnouncementWindow : Window
    {
        public AddAnnouncementWindow()
        {
            InitializeComponent();
        }

        private void AddAnnouncement_Click(object sender, RoutedEventArgs e)
        {
        
            if (string.IsNullOrWhiteSpace(PositionName.Text) ||
                string.IsNullOrWhiteSpace(Company.Text) ||
                string.IsNullOrWhiteSpace(WorkingDays.Text) ||
                string.IsNullOrWhiteSpace(Category.Text) ||
                string.IsNullOrWhiteSpace(Duties.Text) ||
                string.IsNullOrWhiteSpace(AboutCompany.Text))
            {
                MessageBox.Show("Wszystkie wymagane pola muszą być wypełnione.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(PositionName.Text, @"^[^\d]+$") ||
                !Regex.IsMatch(Company.Text, @"^[^\d]+$") ||
                !Regex.IsMatch(Category.Text, @"^[^\d]+$") ||
                !Regex.IsMatch(Duties.Text, @"^[^\d]+$") ||
                !Regex.IsMatch(AboutCompany.Text, @"^[^\d]+$"))
            {
                MessageBox.Show("Pola PositionName, Company, Category, Duties i AboutCompany nie mogą zawierać cyfr ani znaków specjalnych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            if (!string.IsNullOrWhiteSpace(Sallary.Text) && !Regex.IsMatch(Sallary.Text, @"^\d+$"))
            {
                MessageBox.Show("Pole Salary może zawierać tylko cyfry.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (AnnouncementEnd.SelectedDate < AnnouncementStart.SelectedDate)
            {
                MessageBox.Show("Data zakończenia ogłoszenia nie może być wcześniejsza niż data rozpoczęcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            var newAnnouncement = new Announcement
            {
                PositionName = PositionName.Text,
                PositionLevel = (PositionLevel.SelectedItem as ComboBoxItem)?.Content.ToString(),
                ContractType = (ContractType.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Company = Company.Text,
                Localization = Localization.Text,
                WorkingDimension = WorkingDimension.Text,
                Workingtype = (Workingtype.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Sallary = string.IsNullOrWhiteSpace(Sallary.Text) ? 0 : decimal.Parse(Sallary.Text),
                WorkingDays = WorkingDays.Text,
                WorkingHours = WorkingHours.Text,
                AnnouncementStart = AnnouncementStart.SelectedDate.Value,
                AnnouncementEnd = AnnouncementEnd.SelectedDate.Value,
                Category = Category.Text,
                Duties = Duties.Text,
                Requirements = Requirements.Text,
                Benefits = Benefits.Text,
                AboutCompany = AboutCompany.Text
            };


            App.Database.SaveItemAsync(newAnnouncement);
         

            string infoMessage = $"Dodano nowe ogłoszenie:\n\n" +
                $"Stanowisko: {newAnnouncement.PositionName}\n" +
                $"Firma: {newAnnouncement.Company}\n" +
                $"Kategoria: {newAnnouncement.Category}\n" +
                $"Obowiązki: {newAnnouncement.Duties}\n" +
                $"O firmie: {newAnnouncement.AboutCompany}";

            MessageBox.Show(infoMessage, "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            MessageBox.Show("Ogłoszenie dodane pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            }

        }
    }
