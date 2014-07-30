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
    

            int height = 9;
            int width = 9;
            int totalMines = 10;
           

            GamePanel.Children.Clear();
            brd = new Board(height, width, totalMines, this.bcount, this.timer);
            GamePanel.Children.Add(brd);


        }
        private void NewGameEasy(object sender, RoutedEventArgs e)
        {
            brd = null;
            
            bcount.Text = "0";
            int height = 9;
            int width = 9;
            int totalMines = 10;
            object minecount = bcount;



            //Button btn = (Button)sender;
            GamePanel.Children.Clear();
            brd = new Board(height, width, totalMines, this.bcount, this.timer);
            GamePanel.Children.Add(brd);

        }

        private void NewGameMedium(object sender, RoutedEventArgs e)
        {
            brd = null;
            bcount.Text = "0";

            int height = 16;
            int width = 16;
            int totalMines = 40;
            object minecount = bcount;

            

            GamePanel.Children.Clear();
            brd = new Board(height, width, totalMines, this.bcount, this.timer);
            GamePanel.Children.Add(brd);

        }

        private void NewGameHard(object sender, RoutedEventArgs e)
        {
            brd = null;
            bcount.Text = "0";

            int height = 16;
            int width = 30;
            int totalMines = 99;
            object minecount = bcount;
           


            GamePanel.Children.Clear();
            brd = new Board(height, width, totalMines, this.bcount, this.timer);
            GamePanel.Children.Add(brd);

        }

        public void updateminecount(int minecount)
        {
            bcount.Text = minecount.ToString();
        }





    }
}
