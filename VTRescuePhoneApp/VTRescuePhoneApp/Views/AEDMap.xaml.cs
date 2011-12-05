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
        AEDFinderServiceReference.AEDServiceInterfaceClient AEDServiceInterfaceClient = new AEDFinderServiceReference.AEDServiceInterfaceClient();
        AEDFinderServiceReference.Coordinate coord = new AEDFinderServiceReference.Coordinate();
        ObservableCollection<AEDFinderServiceReference.building> buildings;
        ObservableCollection<AEDFinderServiceReference.AED> aeds;
        string popupStr;
        MapPolyline routeLines;
        Pushpin selected;

        Popup allAEDs = new Popup();

        public Page1()
        {
            InitializeComponent();
            map1.Mode = new AerialMode();
            
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
           /* if(currentLocation != null)
            {
                map1.Children.Remove(currentLocation);
            }
            */
            currentLocation = new Pushpin();
            currentLocation.Content = "Current Location";
            currentLocation.Location = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
            

            map1.Children.Add(currentLocation);
           
            map1.Center = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
           
            
            AEDServiceInterfaceClient.GetClosestBuildingsCompleted += new EventHandler<AEDFinderServiceReference.GetClosestBuildingsCompletedEventArgs>(AEDFinderService_GetClosestBuildingsCompleted);

            coord.Latitude = e.Position.Location.Latitude;
            coord.Longitude = e.Position.Location.Longitude;
            AEDServiceInterfaceClient.GetClosestBuildingsAsync(coord, 3);
            
        }

        void AEDFinderService_GetWalkingDirectionsCompleted(object sender, AEDFinderServiceReference.GetWalkingDirectionsCompletedEventArgs e)
        {
            ObservableCollection<AEDFinderServiceReference.Coordinate> route = e.Result;
            routeLines = createPath(route);
  
            map1.Children.Add(routeLines); 
        }
 
        void AEDFinderService_GetClosestBuildingsCompleted(object sender, AEDFinderServiceReference.GetClosestBuildingsCompletedEventArgs e)
        {
            buildings = e.Result;
            Pushpin closestBuilding;
            foreach (AEDFinderServiceReference.building building in buildings)
            {
                closestBuilding = new Pushpin();
                closestBuilding.Location = new GeoCoordinate((double)building.latitude, (double)building.@longitude);
                closestBuilding.Content = building.name;
                closestBuilding.Background = new SolidColorBrush(Colors.Red);
                
                closestBuilding.MouseLeftButtonUp += new MouseButtonEventHandler(Pushpin_MouseLeftButtonUp);

                map1.Children.Add(closestBuilding);
            }
        }

        void AEDFinderService_GetAEDsForBuildingCompleted(object sender, AEDFinderServiceReference.GetAEDsForBuildingCompletedEventArgs e)
        {
            aeds = e.Result;
            popupStr = "AEDs:\n\n";
            foreach (AEDFinderServiceReference.AED aed in aeds)
            {
                popupStr += aed.loc_description + "\n\n";
            }
            //Popup allAEDs;
            //allAEDs = new Popup();
            allAEDs.IsOpen = true;
            allAEDs.VerticalOffset = 110;
            allAEDs.HorizontalOffset = 7;

            AllAEDs allaeds = new AllAEDs();

            allaeds.textBlock.Text = popupStr;

            allAEDs.Child = allaeds;

            allAEDs.Closed += (s1, e1) =>
            {
 
            };
            
        }
   

        void Pushpin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Phone.Controls.Maps.Pushpin tempPP = new Microsoft.Phone.Controls.Maps.Pushpin();
            tempPP = (Microsoft.Phone.Controls.Maps.Pushpin)sender;

            map1.Children.Clear();

            map1.Children.Add(tempPP);
            map1.Children.Add(currentLocation);

            AEDServiceInterfaceClient.GetAEDsForBuildingCompleted += new EventHandler<AEDFinderServiceReference.GetAEDsForBuildingCompletedEventArgs>(AEDFinderService_GetAEDsForBuildingCompleted);
            AEDServiceInterfaceClient.GetAEDsForBuildingAsync((string)tempPP.Content);

            selected = tempPP;

            AEDServiceInterfaceClient.GetWalkingDirectionsCompleted += new EventHandler<AEDFinderServiceReference.GetWalkingDirectionsCompletedEventArgs>(AEDFinderService_GetWalkingDirectionsCompleted);
            AEDFinderServiceReference.Coordinate toCoord = new AEDFinderServiceReference.Coordinate();
            toCoord.Latitude = tempPP.Location.Latitude;
            toCoord.Longitude = tempPP.Location.Longitude;
            AEDFinderServiceReference.Coordinate fromCoord = new AEDFinderServiceReference.Coordinate();
            fromCoord.Latitude = watcher.Position.Location.Latitude;
            fromCoord.Longitude = watcher.Position.Location.Longitude;

            AEDServiceInterfaceClient.GetWalkingDirectionsAsync(fromCoord, toCoord);

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
            path.Locations.Add(new GeoCoordinate(selected.Location.Latitude, selected.Location.Longitude));
            return path;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Pulse.xaml", UriKind.Relative));
        }
    }
}