using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for NumberItem.xaml
    /// </summary>
    /// //////////////////////////////////////////////////////////////////////////
    /// This class use to represent a number item that displayed in the coordinate system
    public partial class NumberItem : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public class Coordicate 
        {
            Int32 _x;
            public Int32 X
            {
                get { return _x; }
                set { _x = value; }
            }

            Int32 _y;
            public Int32 Y
            {
                get { return _y; }
                set { _y = value; }
            }

            internal Coordicate(Int32 x, Int32 y)
            {
                _x = x;
                _y = y;
            }
        }

        Int32 _number;
        public Int32 Number
        {
            get { return _number; }
            set {
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    _number = value;
                    Console.WriteLine("Update number to {0}", _number);
                    // this following line has issue:
                    // http://www.codeproject.com/Articles/29054/WPF-Data-Binding-Part
                    //this.txt_Num.GetBindingExpression(TextBlock.TextProperty).UpdateSource();
                    OnPropertyChanged("Number");
                }
    ));
            }
        }

        void OnPropertyChanged(String element)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("Number"));
            }
        }

        Coordicate _position;
        public Coordicate Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public NumberItem()
        {
            InitializeComponent();
            InitNumber();
        }

        /// <summary>
        /// Init a component with specific position 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public NumberItem(int x, int y)
        {
            InitializeComponent();
            InitNumber(x,y);
        }
       
        void InitNumber(int x, int y, bool newAdd)
        {
            this.InitNumber(x, y);
            BackGroundColor.Background = Brushes.LightSkyBlue;
        }

        void InitNumber(int x, int y)
        {
            Random rnd = new Random();
            _number = rnd.Next(1, 3);
            _number *= 2; // only display 2 or 4
            Position = new Coordicate(x, y);
            Number_ViewBox.DataContext = this;
        }

        void InitNumber()
        {
            Random rnd = new Random();
            _number = rnd.Next(1, 3);
            _number *= 2; // only display 2 or 4
            Position = new Coordicate(0, 0);
            Number_ViewBox.DataContext = this;
        }

        internal void SetBackGroundColor(Boolean setColor)
        {
            if (setColor)
            {
                BackGroundColor.Background = Brushes.LightSkyBlue;
            }
            else
                BackGroundColor.Background = Brushes.WhiteSmoke;
        }
    }
}
