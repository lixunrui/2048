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
    /// Interaction logic for NumberItem.xaml
    /// </summary>
    /// //////////////////////////////////////////////////////////////////////////
    /// This class use to represent a number item that displayed in the coordinate system
    public partial class NumberItem : UserControl
    {
        public class Coordicate 
        {
            internal Int32 _x;
            public Int32 X
            {
                get { return _x; }
                set { _x = value; }
            }

            internal Int32 _y;
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
            set { _number = value; }
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

        void InitNumber()
        {
            Random rnd = new Random();
            _number = rnd.Next(1, 3);
            _number *= 2; // only display 2 or 4
            Position = new Coordicate(0, 0);
            Number_ViewBox.DataContext = this;
        }
    }
}
