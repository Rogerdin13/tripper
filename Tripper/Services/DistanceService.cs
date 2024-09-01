using Shiny.Locations;
using Tripper.Interfaces.Services;

namespace Tripper.Services;

public class DistanceService : IDistanceService
{
    private readonly ILoggingService LoggingService;

    // TODO add DB to persist this for exporting the route as a gpx route -> bery low prio milestone
    // ordered list (newest always first)
    private List<Position> PositionList = [];
    private double TotalDistanceCounter = .0;
    private double PartialDistanceCounter = .0;


    public DistanceService(ILoggingService loggingService)
    {
        LoggingService = loggingService;

        TotalDistanceCounter = Preferences.Default.Get(Constants.PrefKEY_TOTAL_DISTANCE, .0);
        PartialDistanceCounter = Preferences.Default.Get(Constants.PrefKEY_PARTI_DISTANCE, .0);
    }

    #region position managment
    public List<Position> GetPositions()
    {
        return PositionList;
    }

    public bool AddPosition(Position position)
    {
        try
        {

            PositionList.Insert(0, position);
            if(PositionList.Count > 1) CalculateNewDistances();
            return true;
        }
        catch (Exception ex)
        {
            LoggingService.Log($"ERROR {ex.Message}");
            return false;
        }
    }

    public bool ResetTrip()
    {
        try
        {
            PositionList = new List<Position>();
            TotalDistanceCounter = 0.0;
            PartialDistanceCounter = 0.0;

            Preferences.Default.Set(Constants.PrefKEY_TOTAL_DISTANCE, .0);
            Preferences.Default.Set(Constants.PrefKEY_PARTI_DISTANCE, .0);

            return true;
        }
        catch (Exception ex)
        {
            LoggingService.Log($"ERROR {ex.Message}");
            return false;
        }
    }

    #endregion


    #region distance managment

    public async Task<bool> ResetPartialDistance()
    {
        try
        {
            var success = await AddCurrentPosition();
            if (success) { 
                PartialDistanceCounter = 0.0;
            }
            return success;
        }
        catch (Exception ex)
        {
            LoggingService.Log($"ERROR {ex.Message}");
            return false;
        }
    }

    public double PartialDistance()
    {
        return PartialDistanceCounter;
    }

    public double TotalDistance()
    {
        return TotalDistanceCounter;
    }

    #endregion


    #region private

    /// <summary>
    ///     rounds to 3 dezimal points after converting from km to m
    /// </summary>
    private void CalculateNewDistances()
    {
        var newPos = PositionList[0];
        var lastPos = PositionList[1];

        // in meters with max 3 decimal characters
        var distance = Math.Round(Location.CalculateDistance(lastPos.Latitude, lastPos.Longitude, newPos.Latitude, newPos.Longitude, DistanceUnits.Kilometers), 2);

        TotalDistanceCounter += distance;
        PartialDistanceCounter += distance;

        Preferences.Default.Set(Constants.PrefKEY_TOTAL_DISTANCE, TotalDistanceCounter);
        Preferences.Default.Set(Constants.PrefKEY_PARTI_DISTANCE, PartialDistanceCounter);
    }

    private async Task<bool> AddCurrentPosition()
    {
        try
        {
            var loc = await GetDeviceCurrentLocation();
            if(loc == null) return false;
            var pos = new Position(loc.Latitude, loc.Longitude);
            AddPosition(pos);
            return true;
        }
        catch (Exception ex) 
        {
            LoggingService.Log($"ERROR {ex.Message}");
            return false;
        }
    }

    private async Task<Location?> GetDeviceCurrentLocation()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(1));
            var location = await Geolocation.Default.GetLocationAsync(request); //TODO add cancelation token for comfort and better request managment
            return location;
        }
        catch (Exception ex)
        {
            LoggingService.Log($"ERROR {ex.Message}");
            return null;
        }
    }

    #endregion
}
