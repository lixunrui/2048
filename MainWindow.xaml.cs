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

namespace LXR_2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NumberItem[,] Numbers;
        public MainWindow()
        {
            InitializeComponent();
            InitNumberComponents();
        }

        void InitNumberComponents()
        {
            Numbers = new NumberItem[4, 4];
            for (int i = 0; i < 4; i++ )
            {
                for (int j = 0; j < 4; j++ )
                {
                    Numbers[i,j] = new NumberItem();
                    
                    NumberPanel.Children.Add(Numbers[i,j]);

                    Grid.SetRow(Numbers[i, j], i);
                    Grid.SetColumn(Numbers[i, j], j);
                    
                }
            }
        }

        private void StokedKey(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    Console.WriteLine("Up");
                    break;
                case Key.Down:
                    Console.WriteLine("Down");
                    break;
                case Key.Left:
                    Console.WriteLine("Left");
                    break;
                case Key.Right:
                    Console.WriteLine("Right");
                    break;
            }
        }


        void StokedUp()
        {

        }

        void StokedDown()
        {

        }

        void StokedLeft()
        {
            for (int i = 0; i < 4; i++ ) // Numbers[i,x]
            {
                // only check first row here
                for (int j = 0; j < 4; j++ )
                {
                    // compare with the left one
                    if (j - 1 < 0)
                        continue;
                    else // has a item on left
                    {
                        if (Numbers[i,j-1].Number == 0 )// this is a 0 item
                        {    
                            // move the current item to left
                            Numbers[i, j - 1].Number = Numbers[i, j].Number;
                            // reset the current one
                            Numbers[i, j].Number = 0;
                        }
                        else // left side has a value item
                        {
                            // compare the value
                            if (Numbers[i,j-1].Number == Numbers[i,j].Number)
                            {
                                // add the value to the left one
                                Numbers[i, j - 1].Number *= 2;
                                // reset the current item
                                Numbers[i, j].Number = 0;
                            }
                            // if different, do nothing    
                        }
                    }
                } // end for j


            }// end for i
        }

        void StokedRight()
        {

        }

        void NestMove(int loop)
        {
            if (loop < 0)
                return;
            else
            {

            }
            
        }
    }
}
