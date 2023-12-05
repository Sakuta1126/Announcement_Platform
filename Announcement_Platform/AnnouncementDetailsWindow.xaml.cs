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
using Announcement_Platform;

namespace Announcement_Platform
{
    /// <summary>
    /// Interaction logic for AnnouncementDetailsWindow.xaml
    /// </summary>
    public partial class AnnouncementDetailsWindow : Window
    {
        public AnnouncementDetailsWindow(Announcement selectedAnnouncement)
        {
            InitializeComponent();

         
            DataContext = selectedAnnouncement;

           
        }

       

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
          
            Announcement selectedAnnouncement = DataContext as Announcement;

            if (selectedAnnouncement != null)
            {

                Applied appliedItem = new Applied
                {
                    PositionName = selectedAnnouncement.PositionName,
                    PositionLevel = selectedAnnouncement.PositionLevel,
                    ContractType = selectedAnnouncement.ContractType,
                    Company = selectedAnnouncement.Company,
                    Localization = selectedAnnouncement.Localization,
                    WorkingDimension = selectedAnnouncement.WorkingDimension,
                    Workingtype = selectedAnnouncement.Workingtype,
                    Sallary = selectedAnnouncement.Sallary,
                    WorkingDays = selectedAnnouncement.WorkingDays,
                    WorkingHours = selectedAnnouncement.WorkingHours,
                    AnnouncementStart = selectedAnnouncement.AnnouncementStart,
                    AnnouncementEnd = selectedAnnouncement.AnnouncementEnd,
                    Category = selectedAnnouncement.Category,
                    Duties = selectedAnnouncement.Duties,
                    Requirements = selectedAnnouncement.Requirements,
                    Benefits = selectedAnnouncement.Benefits,
                    AboutCompany = selectedAnnouncement.AboutCompany,
                    announcement_id = selectedAnnouncement.Id,
                    user_id = UserStore.LoggedInUserId,
                };


               App.Database.SaveItemAsync(appliedItem);

           
                Close();
            }
        }
    }
}
