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

namespace Announcement_Platform
{
    /// <summary>
    /// Interaction logic for UserProfileInfoWindow.xaml
    /// </summary>
    public partial class UserProfileInfoWindow : Window
    {
        public UserProfileInfo UserProfileInfo { get; set; }

        public UserProfileInfoWindow(UserProfileInfo userProfileInfo)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            UserProfileInfo = userProfileInfo;
            DataContext = UserProfileInfo;
        }
    }
}
