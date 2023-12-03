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
    /// Interaction logic for Video.xaml
    /// </summary>
    public partial class Video : Window
    {
        public Video(string videoPath)
        {
            InitializeComponent();
            VideoControl.Source = new Uri(videoPath, UriKind.RelativeOrAbsolute);

            // Odtwórz film po otwarciu okna
            VideoControl.Play();    
        }
        private void VideoControl_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Zamknij okno po zakończeniu filmu
            Close();
        }
    }
}
