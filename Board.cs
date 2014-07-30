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
    class Board : Grid
    {

        public Button[,] boardarray;    //Main Board
        public int[,] markedLocations; //Used for 3Bv Method


        public const string mine = "99";
        public const string field = "0";

        public int height=0;
        public int width = 0;
        public int minecount = 0;
        public int totalMines;



        public TextBlock scoreboardBcount;
        public TextBlock timer;
        public System.Windows.Threading.DispatcherTimer clock;
        public int difficulty = 0;
        public int clicks = 1;



        //Board Constructor
        public Board(int h, int w, int tMines, TextBlock txtBcount, TextBlock txtTimer)
        {
            height = h;
            width = w;
            totalMines = tMines;
            minecount = totalMines;
            scoreboardBcount = txtBcount;
            scoreboardBcount.Text = minecount.ToString();
            timer = txtTimer;
            StartTimer();



            
            
  

            //Declare 2D array for main board of buttons
            boardarray = new Button[height, width];

            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            //ShowGridLines = true;

            //Create Columns
            for (int col = 0; col < width; col++)
            {
                ColumnDefinition column = new ColumnDefinition();
                //col.Width = GridLength.Auto;
                ColumnDefinitions.Add(column);
            }

            //Create
            for (int row = 0; row < height; row++)
            {
                RowDefinition rows = new RowDefinition();
                RowDefinitions.Add(rows);
            }
                        
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {

                    Button button = new Button();
                    button.Content = " ";
                    button.Height = 25;
                    button.Width = 25;
                    button.Tag = "0";
                  
                    button.Click += LeftClick;
                    button.MouseRightButtonUp += RightClick;

                    boardarray[row, col] = button;


                }

            }



            Random rnd = new Random();

            //Build Mine Field with 
            for (int i = 0; i < totalMines; i++)
            {
                int row = rnd.Next(0, height);	//choose random value for x
                int col = rnd.Next(0, width);		//choose random value for y

                i = checkIfCellEmpty(row, col, i);

            }

            calcNeighborRows(height, width);
            difficulty = calc3bvValue();
            iterateRows(height, width);  //Output Buttons


        }

        //Place Mines
        private int checkIfCellEmpty(int row, int col, int i)
        {

            if (boardarray[row, col].Tag == mine)
            {
                i--;
                return i;
            }

           
            boardarray[row, col].Tag = mine;
            //boardarray[0, 0].Tag = mine;
            return i;

        }


        //Iterate 2D Array of Buttons
        public void iterateRows(int height, int width)
        {

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    
                    SetColumn(boardarray[i, j], j);
                    SetRow(boardarray[i, j], i);
                    this.Children.Add(boardarray[i, j]);
                }
                Console.WriteLine("\n");
            }
        }



        //Calculate Neighbor Values
        public void calcNeighborRows(int h, int w)
        {
            //row=i, col=j
            for (int row = 0; row < h; row++)
            {
                for (int col = 0; col < w; col++)
                {
                    if (boardarray[row, col].Tag != mine)
                    {
                        calcNighborValue(row, col, h, w);
                    }

                }

            }


        }


        //Calculate Neighbor Values
        public void blowmines()
        {
            clock.Stop();
            //row=i, col=j
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (boardarray[row, col].Tag == mine)
                    {
                        boardarray[row, col].Content = "\u1360";
                    }

                    boardarray[row, col].IsEnabled = false;
                }

            }


        }



        //Calculate Nieghbor Values Method
        private void calcNighborValue(int row, int col, int height, int width)
        {

            int neighbors = 0;

            //If Right Column Contains Adjacent Mines
            if (col + 1 < width && boardarray[row, col + 1].Tag == mine)
            {
                neighbors++;
            }
            if (row - 1 >= 0 && col + 1 < width && boardarray[row - 1, col + 1].Tag == mine)
            {
                neighbors++;
            }
            if (row + 1 < height && col + 1 < width && boardarray[row + 1, col + 1].Tag == mine)
            {
                neighbors++;
            }

            //If Mine Above or Below
            if (row - 1 >= 0 && boardarray[row - 1, col].Tag == mine)
            {
                neighbors++;
            }
            if (row + 1 < height && boardarray[row + 1, col].Tag == mine)
            {
                neighbors++;
            }

            //If Left Column Contains Adjacent Mines
            if (col - 1 >= 0 && boardarray[row, col - 1].Tag == mine)
            {
                neighbors++;
            }
            if (row - 1 >= 0 && col - 1 >= 0 && boardarray[row - 1, col - 1].Tag == mine)
            {
                neighbors++;
            }
            if (row + 1 < height && col - 1 >= 0 && boardarray[row + 1, col - 1].Tag == mine)
            {
                neighbors++;
            }


            if (neighbors > 0)
            {
                //boardarray[row, col].Content = neighbors.ToString();
                boardarray[row, col].Tag = neighbors.ToString();
              

            }




        }





        //every non-marked neighbor N of C (diagonal and orthogonal):
        private void floodFill2(int row, int col)
        {
            int boardwidth = width;
            int boardheight = height;

            
            //If Neighbors Right are Zeros
            if (col + 1 < boardwidth)
            {

                if (boardarray[row, col + 1].IsEnabled == true && boardarray[row, col + 1].Tag == "0")
                {

                    //boardarray[row, col + 1].Content = boardarray[row, col + 1].Tag;
                    boardarray[row, col + 1].IsEnabled = false;
                    floodFill2(row, col + 1);

                    //Console.WriteLine("Flood: {0} x {1}", row, col+1);
                    //Console.WriteLine(boardarray[row, col + 1].Tag);
                }
                if (boardarray[row, col + 1].IsEnabled == true && Int32.Parse((boardarray[row, col + 1].Tag).ToString()) > 0 && Int32.Parse((boardarray[row, col + 1].Tag).ToString()) < 99)
                {
                    boardarray[row, col + 1].Content = boardarray[row, col + 1].Tag;
                    boardarray[row, col + 1].IsEnabled = false;
                }

            }

            //If Neighbors Left are Zeros
            if (col - 1 >= 0)
            {

                if (boardarray[row, col - 1].IsEnabled == true && boardarray[row, col - 1].Tag == "0")
                {

                    //boardarray[row, col - 1].Content = boardarray[row, col - 1].Tag;
                    boardarray[row, col - 1].IsEnabled = false;
                    floodFill2(row, col - 1);

                    //Console.WriteLine("Flood: {0} x {1}", row, col - 1);
                    //Console.WriteLine(boardarray[row, col - 1].Tag);

                }
                if (boardarray[row, col - 1].IsEnabled == true && Int32.Parse((boardarray[row, col - 1].Tag).ToString()) > 0 && Int32.Parse((boardarray[row, col - 1].Tag).ToString()) < 99)
                {
                    boardarray[row, col - 1].Content = boardarray[row, col - 1].Tag;
                    boardarray[row, col - 1].IsEnabled = false;
                    
                }

            }


            //If Neighbors below are Zeros
            if (row + 1 < boardheight)
            {
                if (boardarray[row + 1, col].IsEnabled == true && boardarray[row + 1, col].Tag == "0")
                {

                    //boardarray[row + 1, col].Content = boardarray[row + 1, col].Tag;
                    boardarray[row + 1, col].IsEnabled = false;
                    floodFill2(row + 1, col);

                    //Console.WriteLine("Flood: {0} x {1}", row+1, col);
                    //Console.WriteLine(boardarray[row + 1, col].Tag);

                }
                if (boardarray[row + 1, col].IsEnabled == true && Int32.Parse((boardarray[row + 1, col].Tag).ToString()) > 0 && Int32.Parse((boardarray[row + 1, col].Tag).ToString()) < 99)
                {
                    boardarray[row + 1, col].Content = boardarray[row + 1, col].Tag;
                    boardarray[row + 1, col].IsEnabled = false;
                }

            }


            //If Neighbors Above are Zeros  
            if (row - 1 >= 0)
            {
                if (boardarray[row - 1, col].IsEnabled == true && boardarray[row - 1, col].Tag == "0")
                {

                    //boardarray[row - 1, col].Content = boardarray[row - 1, col].Tag;
                    boardarray[row - 1, col].IsEnabled = false;
                    floodFill2(row - 1, col);

                    //Console.WriteLine("Flood: {0} x {1}", row - 1, col);
                    //Console.WriteLine(boardarray[row - 1, col].Tag);

                }
                if (boardarray[row - 1, col].IsEnabled == true && Int32.Parse((boardarray[row - 1, col].Tag).ToString()) > 0 && Int32.Parse((boardarray[row - 1, col].Tag).ToString()) < 99)
                {
                    boardarray[row - 1, col].Content = boardarray[row - 1, col].Tag;
                    boardarray[row - 1, col].IsEnabled = false;
                }

            }


        }






        //Left Click Button
        private void LeftClick(object sender, RoutedEventArgs e)
        {


            Button button = (Button)sender;
            int col = Grid.GetColumn(button);
            Console.WriteLine("col" + col);
            
            int row = Grid.GetRow(button);
            Console.WriteLine("row" + row);
            

            var btn = Int32.Parse(boardarray[row, col].Tag.ToString());
            //boardarray[col, row].Content = boardarray[col, row].Tag.ToString();

            string c = "\u16A9";

            if (boardarray[row, col].Content.ToString() == c.ToString())
            {
                
            }
            else if (btn == 0)
            {
                boardarray[row, col].Content = " ";
                floodFill2(row,col);
                clicks++;
                //floodfill(row, col);
               
                //Console.WriteLine("Space");
                

            }
            else if (btn == 99)
            {
                blowmines();
                boardarray[row, col].Content = "\u1360";
                SystemSounds.Beep.Play();
                MessageBox.Show("Game Over\n\nTime: " + timer.Text +" sec   Clicks: "+clicks +"   Difficulty: "+difficulty);
                
                //Console.WriteLine("Mine");
            }
            else
            {
                boardarray[row, col].Content = btn;
                clicks++;
                if (btn == 1)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.Red);
                }
                else if (btn == 2)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.Green );
                }
                else if (btn == 3)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.RoyalBlue);
                }
                else if (btn == 4)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.Purple);
                }
                else if (btn == 5)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.Yellow);
                }
                else if (btn == 6)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.Orange);
                }
                else if (btn == 7)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.LawnGreen);
                }
                else if (btn == 8)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                }  
                Console.WriteLine(btn);
            }
            
            //Console.WriteLine("L: {0} x {1}", row, col);


        }

        //Right Click Button
        private void RightClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            //MainWindow scoreboard = (MainWindow)sender;//Access Scoreboard XML Textblocks
            int col = Grid.GetColumn(button);
            int row = Grid.GetRow(button);
            string c = "\u16A9";

                if (boardarray[row, col].Content.ToString() == c.ToString())
                {
                    boardarray[row, col].Content = " ";
                    minecount++;
                    scoreboardBcount.Text = (minecount).ToString();
                    
                }
                else if (boardarray[row, col].Content == " ")
                {
                    boardarray[row, col].Content = c;
                    minecount--;
                    scoreboardBcount.Text = (minecount).ToString();
                    
                }

                if (Int32.Parse(scoreboardBcount.Text.ToString()) == 0)
                {
                    int wincount = checkwin();
                    
                    if(wincount ==0){
                        blowmines();
                        SystemSounds.Beep.Play();
                       
                        MessageBox.Show("!!!!!! You Win !!!!!!\n\nTime: " + timer.Text + " sec   Clicks: " + clicks + "   Difficulty: " + difficulty);

                    }
                }


            

            //Console.WriteLine("R: {0} x {1}", row, col);


        }

        private int checkwin()
        {
            int wincount = totalMines;
            string c = "\u16A9";

            //row=i, col=j
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {

                    if (boardarray[row, col].Tag == mine && boardarray[row, col].Content.ToString() == c.ToString())
                    {
                        wincount--;
                       
                    }

                    
                }

            }
            return wincount;

        }

        public void StartTimer()
        {
            clock = new System.Windows.Threading.DispatcherTimer();
            clock.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 1000=Seconds 
            clock.Tick += new EventHandler(Each_Tick);
            clock.Start();
        }

        // A variable to count with.
        int i = 0;

        // Raised every 100 miliseconds while the DispatcherTimer is active.
        public void Each_Tick(object o, EventArgs sender)
        {
            timer.Text = i++.ToString();
        }






        public int calc3bvValue()
        {

            int bvcount = 0;

            markedLocations = new int[height, width];

            //For Each Empty Cell, Non Marked
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {

                    if (markedLocations[row, col] == 0 && Int32.Parse((boardarray[row, col].Tag).ToString()) == 0)
                    {

                        markedLocations[row, col] = 1;
                        //Console.WriteLine(i+","+j);
                        bvcount++;


                        bvfloodFill(row, col);

                    }


                }

            }

            //For Each Non Marked, Non Mine Cell
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {

                    if (markedLocations[row, col] != 1 && Int32.Parse((boardarray[row, col].Tag).ToString()) > 0 && Int32.Parse((boardarray[row, col].Tag).ToString()) < 99)
                    {

                        markedLocations[row, col] = 1;
                        bvcount++;


                    }

                }

            }



            return bvcount;
        }


        //every non-marked neighbor N of C (diagonal and orthogonal):
        private void bvfloodFill(int row, int col)
        {



            //If Neighbors Right are Zeros
            if (col + 1 < width)
            {

                if (markedLocations[row, col + 1] == 0 && boardarray[row, col + 1].Tag == "0")
                {
                    markedLocations[row, col + 1] = 1;
                    bvfloodFill(row, col + 1);
                }


            }

            //If Neighbors Left are Zeros
            if (col - 1 >= 0)
            {
                if (markedLocations[row, col - 1] == 0 && boardarray[row, col - 1].Tag == "0")
                {
                    markedLocations[row, col - 1] = 1;
                    bvfloodFill(row, col - 1);

                }

            }



            if (row + 1 < height)
            {
                if (markedLocations[row + 1, col] == 0 && boardarray[row + 1, col].Tag == "0")
                {
                    markedLocations[row + 1, col] = 1;
                    bvfloodFill(row + 1, col);

                }

            }

            //If Neighbors Above or Below are Zeros  
            if (row - 1 >= 0)
            {
                if (markedLocations[row - 1, col] == 0 && boardarray[row - 1, col].Tag == "0")
                {
                    markedLocations[row - 1, col] = 1;
                    bvfloodFill(row - 1, col);

                }

            }


        }









    }//End Class
}//End Namespace



/*

        private void floodfill(int row, int col)
        {

            int boardwidth = 16;
            int boardheight = 16;
            //int y = row;
            //int x = col;


            for (int i = row-1; i < row+1; i++)
            {
                for (int j = col-1; j < row+1; j++)
                {
                    Console.WriteLine(i + "," + j);
                    if (i >= 0 && i < boardheight && j >= 0 && j < boardwidth && boardarray[i, j].Tag == "0")
                    {
                        //if(boardarray[i, j].IsEnabled == true && boardarray[i, j].Tag == "0"){
                            boardarray[i, j].Content = boardarray[i, j].Tag;
                            boardarray[i, j].IsEnabled = false;
                            floodfill(i, j);
                        //}
                    }
                    
                    j++;


                }
                i++; 
            }
        } 
 
 
 
 
 
 
 
 */

