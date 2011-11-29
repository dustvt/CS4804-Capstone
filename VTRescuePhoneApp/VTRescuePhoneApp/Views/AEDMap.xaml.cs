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
        Pushpin currentLocation;
        Pushpin closestAED;
        public Page1()
        {
            InitializeComponent();

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
            if(currentLocation != null)
            {
                map1.Children.Remove(currentLocation);
            }
            currentLocation = new Pushpin();
            currentLocation.Content = "Current Location";
            currentLocation.Location = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
            map1.Children.Add(currentLocation);
           
            map1.Center = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

            AEDFinderServiceReference.AEDServiceInterfaceClient AEDServiceInterfaceClient = new AEDFinderServiceReference.AEDServiceInterfaceClient();
            AEDServiceInterfaceClient.GetClosestBuildingCompleted += new EventHandler<AEDFinderServiceReference.GetClosestBuildingCompletedEventArgs>(AEDFinderService_GetClosestBuildingCompleted);

            AEDFinderServiceReference.Coordinate coord = new AEDFinderServiceReference.Coordinate();

            coord.Latitude = e.Position.Location.Latitude;
            coord.Longitude = e.Position.Location.Longitude;
            AEDServiceInterfaceClient.GetClosestBuildingAsync(coord);
        }

        void AEDFinderService_GetClosestBuildingCompleted(object sender, AEDFinderServiceReference.GetClosestBuildingCompletedEventArgs e)
        {
            AEDFinderServiceReference.building building = e.Result;
            
            if (closestAED != null)
            {
                map1.Children.Remove(closestAED);
            }
            closestAED = new Pushpin();
            closestAED.Location = new GeoCoordinate((double)building.latitude, (double)building.@longitude);
            closestAED.Content = "Closest AED Building";
            closestAED.Background = new SolidColorBrush(Colors.Red);
            map1.Children.Add(closestAED);
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Pulse.xaml", UriKind.Relative));
        }
    }
}