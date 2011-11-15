using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
<<<<<<< HEAD
using System.Windows.Controls.Primitives;
=======
>>>>>>> 67040f17cc6f197386b8c6ca3ca863f4afc1e02e
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace VTRescuePhoneApp.Views
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            Popup yes;
            yes = new Popup();
            yes.Child = new YesResponsive();
            yes.IsOpen = true;
            yes.VerticalOffset = 110;
            yes.HorizontalOffset = 10;
            yes.Closed += (s1, e1) =>
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            };
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Popup no;
            no = new Popup();
            no.Child = new NoResponsive();
            no.IsOpen = true;
            no.VerticalOffset = 320;
            no.HorizontalOffset = 10;
            no.Closed += (s1, e1) =>
            {
                
            };
=======

>>>>>>> 67040f17cc6f197386b8c6ca3ca863f4afc1e02e
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AEDMap.xaml", UriKind.Relative));
        }
<<<<<<< HEAD



=======
>>>>>>> 67040f17cc6f197386b8c6ca3ca863f4afc1e02e
    }
}