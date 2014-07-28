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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;

namespace WpfDemo1
{

    


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Board brd;

        public MainWindow()
        {
            InitializeComponent();

            int height = 16;
            int width = 16;
            int totalMines = 30;
            double Height = 400;
            double Width = 320;

            Board brd = new Board(height, width, totalMines, Width, Height); //Build Board


            
            //Adding to form
            GamePanel.Children.Add(brd);


        }
        private void NewGameEasy(object sender, RoutedEventArgs e)
        {
            //SystemSounds.Beep.Play();

            int height = 16;
            int width = 16;
            int totalMines = 30;
            double Height = 400;
            double Width = 320;


            //Button btn = (Button)sender;
            GamePanel.Children.Clear();
            brd = new Board(height, width, totalMines, Width, Height);
            GamePanel.Children.Add(brd);

        }

        private void NewGameMedium(object sender, RoutedEventArgs e)
        {


            int height = 32;
            int width = 32;
            int totalMines = 50;
            double Height = 600;
            double Width = 520;

            GamePanel.Children.Clear();
            brd = new Board(height, width, totalMines, Width, Height);
            GamePanel.Children.Add(brd);

        }

        private void NewGameHard(object sender, RoutedEventArgs e)
        {


            int height = 48;
            int width = 48;
            int totalMines = 60;
            double Height = 800;
            double Width = 720;


            GamePanel.Children.Clear();
            brd = new Board(height, width, totalMines, Width, Height);
            GamePanel.Children.Add(brd);

        }






    }
}
