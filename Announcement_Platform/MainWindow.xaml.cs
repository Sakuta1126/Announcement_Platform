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
                AccChoose.Visibility = Visibility.Visible;
                // Tutaj możesz dodać kod do przejścia do głównego ekranu aplikacji lub innych operacji po zakończeniu ładowania
            });
        }

    }
}
