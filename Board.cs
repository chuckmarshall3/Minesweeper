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

        int height = 0;
        int width = 0;

        //Board Constructor
        public Board(int height, int width, int totalMines, double windowwidth, double windowheight)
        {

            //Declare 2D array
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

            //boardarray[row, col].Content = mine;
            boardarray[row, col].Tag = mine;
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
        public void blowbombs()
        {
            //row=i, col=j
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
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


        //every non-marked neighbor N of C (diagonal and orthogonal):
        private void floodFill2(int row, int col)
        {
            int boardwidth = 16;
            int boardheight = 16;

            
            //If Neighbors Right are Zeros
            if (col + 1 < boardwidth)
            {

                if (boardarray[row, col + 1].IsEnabled == true && boardarray[row, col + 1].Tag == "0")
                {

                    //boardarray[row, col + 1].Content = boardarray[row, col + 1].Tag;
                    boardarray[row, col + 1].IsEnabled = false;
                    floodFill2(row, col + 1);

                    Console.WriteLine("Flood: {0} x {1}", row, col+1);
                    Console.WriteLine(boardarray[row, col + 1].Tag);
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

                    Console.WriteLine("Flood: {0} x {1}", row, col - 1);
                    Console.WriteLine(boardarray[row, col - 1].Tag);

                }
                if (boardarray[row, col - 1].IsEnabled == true && Int32.Parse((boardarray[row, col - 1].Tag).ToString()) > 0 && Int32.Parse((boardarray[row, col - 1].Tag).ToString()) < 99)
                {
                    boardarray[row, col - 1].Content = boardarray[row, col - 1].Tag;
                    boardarray[row, col - 1].IsEnabled = false;
                }

            }



            if (row + 1 < boardheight)
            {
                if (boardarray[row + 1, col].IsEnabled == true && boardarray[row + 1, col].Tag == "0")
                {

                    //boardarray[row + 1, col].Content = boardarray[row + 1, col].Tag;
                    boardarray[row + 1, col].IsEnabled = false;
                    floodFill2(row + 1, col);

                    Console.WriteLine("Flood: {0} x {1}", row+1, col);
                    Console.WriteLine(boardarray[row + 1, col].Tag);

                }
                if (boardarray[row + 1, col].IsEnabled == true && Int32.Parse((boardarray[row + 1, col].Tag).ToString()) > 0 && Int32.Parse((boardarray[row + 1, col].Tag).ToString()) < 99)
                {
                    boardarray[row + 1, col].Content = boardarray[row + 1, col].Tag;
                    boardarray[row + 1, col].IsEnabled = false;
                }

            }


            //If Neighbors Above or Below are Zeros  
            if (row - 1 >= 0)
            {
                if (boardarray[row - 1, col].IsEnabled == true && boardarray[row - 1, col].Tag == "0")
                {

                    //boardarray[row - 1, col].Content = boardarray[row - 1, col].Tag;
                    boardarray[row - 1, col].IsEnabled = false;
                    floodFill2(row - 1, col);

                    Console.WriteLine("Flood: {0} x {1}", row - 1, col);
                    Console.WriteLine(boardarray[row - 1, col].Tag);

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
            if (btn == 0)
            {
                boardarray[row, col].Content = " ";
                floodFill2(row,col);
                //floodfill(row, col);
               
                //Console.WriteLine("Space");
                

            }
            else if (btn == 99)
            {
                blowbombs();
                boardarray[row, col].Content = "\u1360";
                MessageBox.Show("Game Over");
                


                //Console.WriteLine("Mine");
            }
            else
            {
                boardarray[row, col].Content = btn;
                if (btn == 1)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.Yellow);
                }
                else if (btn == 2)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.Red );
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
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (btn == 6)
                {
                    boardarray[row, col].Foreground = new SolidColorBrush(Colors.Orange);
                }               
                Console.WriteLine(btn);
            }
            
            Console.WriteLine("L: {0} x {1}", row, col);


        }

        //Right Click Button
        private void RightClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int col = Grid.GetColumn(button);
            int row = Grid.GetRow(button);
            string c = "\u16A9";

                if (boardarray[row, col].Content.ToString() == c.ToString())
                {
                    boardarray[row, col].Content = " ";
                }
                else if (boardarray[row, col].Content == " ")
                {
                    boardarray[row, col].Content = c;
                }

            

            Console.WriteLine("R: {0} x {1}", row, col);


        }


    }//End Class
}//End Namespace


/*   
   public int calc3bvValue(int height, int width)
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


                   floodFill(row, col);

               }


           }

       }

       //For Each Non Marked, Non Mine Cell
       for (int row = 0; row < height; row++)
       {
           for (int col = 0; col < width; col++)
           {

               if (markedLocations[row, col] != 1 && Int32.Parse((boardarray[row, col].Tag).ToString()) > 0 && Int32.Parse((boardarray[row, col].Tag).ToString())  < 99)
               {

                   markedLocations[row, col] = 1;
                   bvcount++;


               }

           }

       }



       return bvcount;
   }
        

   //every non-marked neighbor N of C (diagonal and orthogonal):
   private void floodFill(int row, int col)
   {
       int boardwidth = 16;
       int boardheight = 16;

            
       //If Neighbors Right are Zeros
       if (col + 1 < boardwidth)
       {

           if (markedLocations[row,col+1] == 0 && boardarray[row, col + 1].Tag == "0")
           {
               markedLocations[row, col + 1] = 1;
               floodFill(row, col + 1);
           }


       }

       //If Neighbors Left are Zeros
       if (markedLocations[row, col - 1] == 0 && col - 1 >= 0)
       {
           if (markedLocations[row, col - 1] == 0 && boardarray[row, col - 1].Tag == "0")
           {
               markedLocations[row, col - 1] = 1;
               floodFill(row, col - 1);

           }

       }



       if (row + 1 < boardheight)
       {
           if (markedLocations[row + 1, col] == 0 && boardarray[row + 1, col].Tag == "0")
           {
               markedLocations[row + 1, col] = 1;
               floodFill(row + 1, col);

           }

       }

       //If Neighbors Above or Below are Zeros  
       if (row - 1 >= 0)
       {
           if (markedLocations[row - 1, col] == 0 && boardarray[row - 1, col].Tag == "0")
           {
               markedLocations[row - 1, col] = 1;
               floodFill(row - 1, col);

           }

       }


   }*/

