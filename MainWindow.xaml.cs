using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Threading;

namespace LXR_2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NumberItem[,] Numbers;
        Collection<NumberItem> components;
        const Int32 START_NUMBER = 2;
        const Int32 MAX_NUMBER = 16;
        const Int32 NUMBER_A_LINE = 4;

        Boolean _moved = false;

        AutoResetEvent ReadyToAdd = new AutoResetEvent(true);

        delegate void MovedElement(Int32 elementNumber);

        event MovedElement OnMoved;

        public MainWindow()
        {
            InitializeComponent();
            InitNumberComponents();
            this.OnMoved = new MovedElement(UpdateScore);
        }

        private void UpdateScore(int elementNumber=0)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                try
                {
                    Int32 currentScore = Convert.ToInt32(txt_Score.Text);
                    txt_Score.Text = (currentScore + elementNumber).ToString();
                }
                catch (System.Exception)
                {
                    txt_Score.Text = "0";
                }

                try
                {
                    if (!_moved)
                    {
                        Int32 moved = Convert.ToInt32(txt_Moves.Text);
                        txt_Moves.Text = (moved + 1).ToString();
                    }

                }
                catch (System.Exception)
                {
                    txt_Moves.Text = "0";
                }

                _moved = true;
                
                ReadyToAdd.Set();
            }));
        }



        // Only create two components when start the game
        void InitNumberComponents()
        {
            components = new Collection<NumberItem>();

            Numbers = new NumberItem[NUMBER_A_LINE,NUMBER_A_LINE];

            txt_Moves.Text = txt_Score.Text = "0";

            CreateComponents(START_NUMBER);
        }

        void CreateComponents(int num)
        {
            Random random = new Random();
            int start = 1;
            while (start <= num)
            {
                // creating two components
                int x = random.Next(0, 4); // range from 0 - 3
                int y = random.Next(0, 4);

                //// make sure this is no duplicate components
                if (components.Count >= MAX_NUMBER)
                {
                    break;
                }

                if (Numbers[x,y] != null)
                {
                    continue; ;
                }

                NumberItem item = new NumberItem(x, y);
                if (num == 1)
                {
                    item.SetBackGroundColor(true);
                }
                NumberPanel.Children.Add(item);
                Grid.SetRow(item, x);
                Grid.SetColumn(item, y);
                
                Numbers[x, y] = item;
                components.Add(Numbers[x, y]);
                start++;
            }
        }

        Boolean GameOver()
        {
            if (components.Count >= MAX_NUMBER)
            {
                // check if there is any two components have the same number
                for (int i = 0; i < NUMBER_A_LINE - 1; i++)
                {
                    for (int j = 0; j < NUMBER_A_LINE - 1 ;j++ )
                    {
                        Console.WriteLine("Current Position $7B{0}:{1}$7D",i,j);
                        if (Numbers[i, j] == null || Numbers[i, j+1] == null || Numbers[j, i] == null || Numbers[j+1, i] == null)
                        {
                            continue;
                        }

                        if (Numbers[i, j].Number == Numbers[i, j + 1].Number)
                        {
                            Console.WriteLine("Comparing {{{0},{1}}} with{{{0},{2}}}", i, j, j + 1);
                            return false;
                        }

                        if (Numbers[j, i].Number == Numbers[j+1, i].Number)
                        {
                            Console.WriteLine("Comparing {{{0},{1}}} with{{{2},{1}}}", j, i, j + 1);
                            return false;
                        }
                    }
                }
                return true;
            }
            else
                return false;
        }

        void ExitGame()
        {
            this.Close();
        }

        private void StokedKey(object sender, KeyEventArgs e)
        {
            _moved = false;
            switch (e.Key)
            {
                case Key.Up:
                    StokedUp();
                    Console.WriteLine("Up");
                    break;
                case Key.Down:
                    StokedDown();
                    Console.WriteLine("Down");
                    break;
                case Key.Left:
                    StokedLeft();
                    Console.WriteLine("Left");
                    break;
                case Key.Right:
                    StokedRight();
                    Console.WriteLine("Right");
                    break;
            }



            if (!GameOver())
            {
                if (ReadyToAdd.WaitOne(300))
                {
                    UpdateComponentsColor();
                    CreateComponents(1);
                }
            }
            else
            {
                Console.WriteLine("Game Over");
               // ExitGame();
            }
        }

        private void UpdateComponentsColor()
        {
            foreach (UIElement element in NumberPanel.Children)
            {
                if (element is NumberItem)
                {
                    NumberItem item = element as NumberItem;
                    item.SetBackGroundColor(false);
                }
            }
        }

        void StokedUp()
        {
            for (int column = 0; column < NUMBER_A_LINE; column ++ )
            {
                for (int row = 1; row < NUMBER_A_LINE; )
                {
                    if (row == 0)
                    {
                        row++;
                    }
                    // current is null, then move to the next
                    if (Numbers[row, column] == null)
                    {
                        row++;
                        continue;
                    }
                    else if (Numbers[row - 1, column] == null)
                    {
                        // move current to the left 
                        UpdatePosition(Numbers[row, column], row - 1, column);
                        Numbers[row - 1, column] = Numbers[row, column];

                        Numbers[row, column] = null;
                        UpdateScore();
                        row--;

                    }
                    else // if the left is not null
                    {
                        if (Numbers[row - 1, column].Number == Numbers[row, column].Number)
                        {
                            // add them together and destroy the second element
                            Numbers[row - 1, column].Number += Numbers[row, column].Number;
                            Console.WriteLine("Update {{{0}:{1}}} to {2}", row - 1, column, Numbers[row - 1, column].Number);
                            UpdateScore(Numbers[row, column].Number);
                            RemoveElement(Numbers[row, column]);
                            Numbers[row, column] = null;
                            
                        }
                        else
                            row++;
                    }
                }// end for row
            }// end for column
        }

        void StokedDown()
        {
            for (int column = 0; column < NUMBER_A_LINE; column++)
            {
                for (int row = NUMBER_A_LINE - 1; row >= 0; )
                {
                    if (row == NUMBER_A_LINE - 1)
                    {
                        row--;
                    }
                    // current is null, then move to the next
                    if (Numbers[row, column] == null)
                    {
                        row--;
                        continue;
                    }
                    else if (Numbers[row + 1, column] == null)
                    {
                        // move current to the left 
                        UpdatePosition(Numbers[row, column], row + 1, column);
                        Numbers[row + 1, column] = Numbers[row, column];

                        Numbers[row, column] = null;
                        UpdateScore();
                        row++;

                    }
                    else // if the left is not null
                    {
                        if (Numbers[row + 1, column].Number == Numbers[row, column].Number)
                        {
                            // add them together and destroy the second element
                            Numbers[row + 1, column].Number += Numbers[row, column].Number;
                            Console.WriteLine("Update {{{0}:{1}}} to {2}", row + 1, column, Numbers[row + 1, column].Number);
                            UpdateScore(Numbers[row, column].Number);
                            RemoveElement(Numbers[row, column]);
                            Numbers[row, column] = null;
                            
                        }
                        else
                            row--;
                    }
                }// end for row
            }// end for column
        }

        void StokedLeft()
        {
            for (int row = 0; row < NUMBER_A_LINE; row++ )
            {// check from the second column
                for (int column = 1; column < NUMBER_A_LINE; )
                {
                    if (column ==0 )
                    {
                        column++;
                    }
                    // current is null, then move to the next
                    if (Numbers[row, column] == null)
                    {
                        column++;
                        continue;
                    }
                    else if(Numbers[row, column - 1] == null)
                    {
                        // move current to the left 
                        UpdatePosition(Numbers[row, column], row, column - 1);
                        Numbers[row, column - 1] = Numbers[row, column];
                                            
                        Numbers[row, column] = null;
                        UpdateScore();
                        column--;
                        
                    }
                    else // if the left is not null
                    {
                        if (Numbers[row, column - 1].Number == Numbers[row, column].Number)
                        {
                            // add them together and destroy the second element
                            Numbers[row, column - 1].Number += Numbers[row, column].Number;
                            Console.WriteLine("Update {{{0}:{1}}} to {2}", row, column - 1, Numbers[row, column - 1].Number);
                            UpdateScore(Numbers[row, column].Number);
                            RemoveElement(Numbers[row, column]);
                            Numbers[row, column] = null;
                         
                        }
                        else
                            column++;
                    }
                }// end for column
            }// end for row
        }

        void StokedRight()
        {
            for (int row = 0; row < NUMBER_A_LINE; row++)
            {// check from the second column
                for (int column = NUMBER_A_LINE - 1; column >= 0; )
                {
                    if (column == NUMBER_A_LINE - 1)
                    {
                        column--;
                    }
                    // current is null, then move to the next
                    if (Numbers[row, column] == null)
                    {
                        column--;
                        continue;
                    }
                    else if (Numbers[row, column + 1] == null)
                    {
                        // move current to the left 
                        UpdatePosition(Numbers[row, column], row, column + 1);
                        Numbers[row, column + 1] = Numbers[row, column];

                        Numbers[row, column] = null;
                        UpdateScore();
                        column++;

                    }
                    else // if the right is not null
                    {
                        if (Numbers[row, column + 1].Number == Numbers[row, column].Number)
                        {
                            // add them together and destroy the second element
                            Numbers[row, column + 1].Number += Numbers[row, column].Number;
                            Console.WriteLine("Update {{{0}:{1}}} to {2}", row, column + 1, Numbers[row, column + 1].Number);
                            UpdateScore(Numbers[row, column].Number);
                            RemoveElement(Numbers[row, column]);
                            Numbers[row, column] = null;
              
                        }
                        else
                            column--;
                    }
                }// end for column
            }// end for row
        }

        void NestMove(ref int row, ref int column)
        {

            
        }

        void UpdatePosition(NumberItem item, int row, int column)
        {
            this.Dispatcher.BeginInvoke((Action)(
                ()=>{
                    Grid.SetColumn(item, column);
                    Grid.SetRow(item, row);
                    item.Position = new NumberItem.Coordicate(row, column);
                }
                ));
        }

        void RemoveElement(NumberItem item)
        {
            components.Remove(item);
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                NumberPanel.Children.Remove(item);
            }
    ));
        }



        private void Exit_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void New_Game_Clicked(object sender, RoutedEventArgs e)
        {
            List<UIElement> elements = new List<UIElement>();

            foreach (UIElement element in NumberPanel.Children)
            {
                if (element is NumberItem)
                {
                    elements.Add(element);
                }
            }

            foreach (UIElement element in elements)
            {
                NumberPanel.Children.Remove(element);
            }
            InitNumberComponents();
            
        }
    }
}
