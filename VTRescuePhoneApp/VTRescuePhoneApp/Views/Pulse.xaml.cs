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
    public partial class Pulse : PhoneApplicationPage
    {
        public Pulse()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Popup yes;
            yes = new Popup();
            yes.Child = new YesPulse();
            yes.IsOpen = true;
            yes.VerticalOffset = 110;
            yes.HorizontalOffset = 10;
            yes.Closed += (s1, e1) =>
            {
                //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Popup no;
            no = new Popup();
            no.Child = new NoPulse();
            no.IsOpen = true;
            no.VerticalOffset = 300;
            no.HorizontalOffset = 5;
            no.Closed += (s1, e1) =>
            {

            };
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Popup AEDArrived;
            AEDArrived = new Popup();
            AEDArrived.Child = new AEDArrived();
            AEDArrived.IsOpen = true;
            AEDArrived.VerticalOffset = 450;
            AEDArrived.HorizontalOffset = 5;
            AEDArrived.Closed += (s1, e1) =>
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            };
            
        }
    }
}