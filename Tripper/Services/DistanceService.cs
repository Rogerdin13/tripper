using Shiny.Locations;
using System;
using Tripper.Interfaces.Services;

namespace Tripper.Services;

public class DistanceService : IDistanceService
{
    private readonly ILoggingService LoggingService;

    // temporary storage here
    // ordered list (newest always first)
    private List<Position> PositionList = [];
    private double TotalDistanceCounter = 0.0;
    private double PartialDistanceCounter = 0.0;


    public DistanceService(ILoggingService loggingService)
    {
        LoggingService = loggingService;
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
            CalculateNewDistances();
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
                LoggingService.Log("Added Current Pos and reset Partial");
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

        LoggingService.Log($"Calculating new distance: new: {newPos} | old: {lastPos}");

        // in meters with max 3 decimal characters
        var distance = Math.Round(Location.CalculateDistance(lastPos.Latitude, lastPos.Longitude, newPos.Latitude, newPos.Longitude, DistanceUnits.Kilometers)*1000, 3);

        LoggingService.Log($"Distance calculated: {distance}m");

        TotalDistanceCounter += distance;
        PartialDistanceCounter += distance;
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
