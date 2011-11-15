using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;

namespace VTRescuePhoneApp
{
    public partial class Page1 : PhoneApplicationPage
    {
        GeoCoordinateWatcher watcher;
        public Page1()
        {
            InitializeComponent();
            AEDFinderServiceReference.AEDServiceInterfaceClient AEDServiceInterfaceClient = new AEDFinderServiceReference.AEDServiceInterfaceClient();
            AEDServiceInterfaceClient.GetAllAEDsCompleted += new EventHandler<AEDFinderServiceReference.GetAllAEDsCompletedEventArgs>(AEDFinderService_GetAllAEDsCompleted);

            AEDServiceInterfaceClient.GetAllAEDsAsync();

            if (watcher == null)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High)
                {
                    MovementThreshold = 10
                };

                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);

                watcher.Start();
            }
            map1.ZoomLevel = 15;
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            map1.Center = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
        }
        void AEDFinderService_GetAllAEDsCompleted(object sender, AEDFinderServiceReference.GetAllAEDsCompletedEventArgs e)
        {
            ObservableCollection<AEDFinderServiceReference.aed> aedList = e.Result;
            foreach (AEDFinderServiceReference.aed aed in aedList)
            {
                Pushpin pin = new Pushpin();
                pin.Location = new GeoCoordinate((double)aed.lat, (double)aed.@long);

                map1.Children.Add(pin);
            }
        }
<<<<<<< HEAD

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Pulse.xaml", UriKind.Relative));
        }
=======
>>>>>>> 67040f17cc6f197386b8c6ca3ca863f4afc1e02e
    }
}