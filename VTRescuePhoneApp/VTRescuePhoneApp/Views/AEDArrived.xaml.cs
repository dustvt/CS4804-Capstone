using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;


namespace VTRescuePhoneApp
{
    public partial class AEDArrived : UserControl
    {

        public AEDArrived()
        {
            InitializeComponent();
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            ClosePopup();

        }
        
        private void ClosePopup()
        {
            Popup buyPop = this.Parent as Popup;
            buyPop.IsOpen = false;
        }


    }
}
