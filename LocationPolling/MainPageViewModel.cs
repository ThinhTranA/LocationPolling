using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Essentials;
using MvvmHelpers;
using System.Threading.Tasks;
using System.Linq;

namespace LocationPolling
{
    public class MainPageViewModel : BaseViewModel
    {
        Location _location;
        public Location Location 
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }

        int _distanceInMeters;
        public int DistanceInMeters
        {
            get { return _distanceInMeters; }
            set { SetProperty(ref _distanceInMeters, value); }
        }

        public int MinDistance { get; set; }
        public int MaxDistance { get; set; }

        public Command GetLocationCommand { get; set; }


        Location office140William = new Location {Latitude = -31.9510955810547, Longitude = 115.858526164014, Altitude = 14.9981451034546 };

        public MainPageViewModel()
        {
            MinDistance = 100; //m
            MaxDistance = 120;

            GetLocationCommand = new Command(async () => await ExecuteGetLocationCommandAsync());
            GetLocationCommand.Execute(null);
        }

        async Task ExecuteGetLocationCommandAsync()
        {
            //Location = await GetLocation();
            //DistanceInMeters = Location.CalculateDistance(Location, office140William, DistanceUnits.Kilometers) * 1000;
            await UpdateLocationAsyncInfiniteLoop();
        }

        async Task UpdateLocationAsyncInfiniteLoop()
        {
            while (true)
            {
                Location = await GetLocation();
                var distanceInMeters = Location.CalculateDistance(Location, office140William, DistanceUnits.Kilometers) * 1000;
                DistanceInMeters = Convert.ToInt32(distanceInMeters);
                if(Enumerable.Range(MinDistance,MaxDistance).Contains(DistanceInMeters))
                {
                    AlertPhone();
                }

                await Task.Delay(1000);
            }
        }


        async Task<Location> GetLocation()
        {
            try
            {
              //  var location = await Geolocation.GetLastKnownLocationAsync();
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }

                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return null;
        }

        void AlertPhone()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                // Use default vibration length
                Vibration.Vibrate();

                // Or use specified time
                var duration = TimeSpan.FromSeconds(5);
                Vibration.Vibrate(duration);
                
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

            IsBusy = false;
        }
    }
}
