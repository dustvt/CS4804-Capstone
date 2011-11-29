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
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Windows.Controls.Primitives;


namespace VTRescuePhoneApp
{
    public partial class Page1 : PhoneApplicationPage
    {
        
        GeoCoordinateWatcher watcher;
        Pushpin currentLocation;
        Pushpin closestBuilding;
        AEDFinderServiceReference.AEDServiceInterfaceClient AEDServiceInterfaceClient = new AEDFinderServiceReference.AEDServiceInterfaceClient();
        AEDFinderServiceReference.Coordinate coord = new AEDFinderServiceReference.Coordinate();
        AEDFinderServiceReference.building building;
        ObservableCollection<AEDFinderServiceReference.AED> aeds;
        string popupStr;

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

            map1.ZoomLevel = 17;
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

           
            AEDServiceInterfaceClient.GetClosestBuildingCompleted += new EventHandler<AEDFinderServiceReference.GetClosestBuildingCompletedEventArgs>(AEDFinderService_GetClosestBuildingCompleted);

            coord.Latitude = e.Position.Location.Latitude;
            coord.Longitude = e.Position.Location.Longitude;
            AEDServiceInterfaceClient.GetClosestBuildingAsync(coord);

            
            
        }

        void AEDFinderService_GetWalkingDirectionsCompleted(object sender, AEDFinderServiceReference.GetWalkingDirectionsCompletedEventArgs e)
        {
            ObservableCollection<AEDFinderServiceReference.Coordinate> coords = e.Result;
            if (coords != null)
            {
                map1.Children.Remove(createPath(coords));
            }
            map1.Children.Add(createPath(coords)); 
        }
        

        void AEDFinderService_GetClosestBuildingCompleted(object sender, AEDFinderServiceReference.GetClosestBuildingCompletedEventArgs e)
        {
            building = e.Result;
            
            if (closestBuilding != null)
            {
                map1.Children.Remove(closestBuilding);
            }
            closestBuilding = new Pushpin();
            closestBuilding.Location = new GeoCoordinate((double)building.latitude, (double)building.@longitude);
            closestBuilding.Content = building.name;
            closestBuilding.Background = new SolidColorBrush(Colors.Red);
            map1.Children.Add(closestBuilding);

            closestBuilding.MouseLeftButtonUp += new MouseButtonEventHandler(Pushpin_MouseLeftButtonUp);

            AEDServiceInterfaceClient.GetWalkingDirectionsCompleted += new EventHandler<AEDFinderServiceReference.GetWalkingDirectionsCompletedEventArgs>(AEDFinderService_GetWalkingDirectionsCompleted);
            AEDServiceInterfaceClient.GetWalkingDirectionsAsync(coord);

            AEDServiceInterfaceClient.GetClosestAEDsCompleted += new EventHandler<AEDFinderServiceReference.GetClosestAEDsCompletedEventArgs>(AEDFinderService_GetClosestAEDsCompleted);
            AEDServiceInterfaceClient.GetClosestAEDsAsync(coord);
            
        }

        void AEDFinderService_GetClosestAEDsCompleted(object sender, AEDFinderServiceReference.GetClosestAEDsCompletedEventArgs e)
        {
            aeds = e.Result;
            popupStr = "AEDs:\n\n";
            foreach (AEDFinderServiceReference.AED aed in aeds)
            {
                popupStr += aed.loc_description + "\n\n";
            }
            
        }

        void Pushpin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Phone.Controls.Maps.Pushpin tempPP = new Microsoft.Phone.Controls.Maps.Pushpin();

            Popup allAEDs;
            allAEDs = new Popup();
            allAEDs.Child = new AllAEDs();
            allAEDs.IsOpen = true;
            allAEDs.VerticalOffset = 110;
            allAEDs.HorizontalOffset = 7;
            
            AllAEDs allaeds = new AllAEDs();

            allaeds.textBlock.Text = popupStr;
           
            allAEDs.Child = allaeds;

            allAEDs.Closed += (s1, e1) =>
            {
               
            };
            tempPP = (Microsoft.Phone.Controls.Maps.Pushpin)sender;


        } 




        MapPolyline createPath(ObservableCollection<AEDFinderServiceReference.Coordinate> route)
        {
            MapPolyline path = new MapPolyline();
            path.Stroke = new SolidColorBrush(Colors.Blue);
            path.StrokeThickness = 5;
            path.Opacity = 0.75;
            path.Locations = new LocationCollection();
            path.Locations.Add(new GeoCoordinate(watcher.Position.Location.Latitude, watcher.Position.Location.Longitude));
            foreach (AEDFinderServiceReference.Coordinate point in route) {
                path.Locations.Add(new GeoCoordinate(point.Latitude, point.Longitude));
            }
            path.Locations.Add(new GeoCoordinate(building.latitude, building.longitude));
            return path;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Pulse.xaml", UriKind.Relative));
        }
    }
}