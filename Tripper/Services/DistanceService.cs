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
            LoggingService.Log($"{ex.Message}");
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
            LoggingService.Log($"{ex.Message}");
            return false;
        }
    }

    #endregion


    #region distance managment

    public bool ResetPartialDistance()
    {
        try
        {
            PartialDistanceCounter = 0.0;
            return true;
        }
        catch (Exception ex)
        {
            LoggingService.Log($"{ex.Message}");
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

        // in meters
        var distance = Math.Round(Location.CalculateDistance(lastPos.Latitude, lastPos.Longitude, newPos.Latitude, newPos.Longitude, DistanceUnits.Kilometers)*1000, 3);

        LoggingService.Log($"Distance calculated: {distance}m");

        TotalDistanceCounter += distance;
        PartialDistanceCounter += distance;
    }

    #endregion
}
