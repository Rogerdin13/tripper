using Shiny.Locations;
using Tripper.Helpers;
using Tripper.Interfaces.Services;

namespace Tripper.ViewModels;


public class HomeViewModel : ViewModelBase
{
    private readonly IGpsManager GpsManager;
    private readonly IDeviceService DeviceService;
    private readonly IDistanceService DistanceService;
    private IDisposable? subscription = null;
    private bool refreshInProgress;
    

    #region refresh binding props

    private bool isRefreshing;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set => SetProperty(ref isRefreshing, value);
    }

    private Color platformRefreshColor = DeviceInfo.Platform == DevicePlatform.iOS ? new Color(255,255,255) : new Color(0,0,0);
    public Color PlatformRefreshColor
    {
        get => platformRefreshColor;
        set => SetProperty(ref platformRefreshColor, value);
    }

    public ICommand RefreshingCommand => new Command(async () =>
    {
        if (refreshInProgress) return;
        refreshInProgress = true;

        var gpsEnabled = DeviceService.GpsServicesEnabled();
        LoggingService.Log($"gps-listener running:{ListenerIsRunning}, gps-enabled:{gpsEnabled}");

        ListenerIsRunning = GpsManager.CurrentListener != null && gpsEnabled;

        SubscribeToLocationChanges();
        if (gpsEnabled && !ListenerIsRunning) 
        {
            LoggingService.Log($"gps enabled & no listner running -> trying to start listener");

            ListenerIsRunning = await StartListener();

            IsRefreshing = false;
            refreshInProgress = false;
            return;
        }

        IsRefreshing = false;
        refreshInProgress = false;
    });

    #endregion


    #region binding props

    private GpsReading? lastReading;
    public GpsReading? LastReading
    { 
        get => lastReading;
        set => SetProperty(ref lastReading, value);
    }

    private bool listenerIsRunning;
    public bool ListenerIsRunning
    {
        get => listenerIsRunning;
        set => SetProperty(ref listenerIsRunning, value);
    }

    private double totalDistance;
    public double TotalDistance
    {
        get => totalDistance;
        set => SetProperty(ref totalDistance, value);
    }

    private double partialDistance;
    public double PartialDistance
    {
        get => partialDistance;
        set => SetProperty(ref partialDistance, value);
    }

    #endregion


    #region button commands & binding props

    private bool resetInProgress;
    public bool ResetInProgress
    {
        get => resetInProgress;
        set => SetProperty(ref resetInProgress, value);
    }

    public ICommand TotalResetCommand => new Command(() => {
        if (!ListenerIsRunning) return;

        DistanceService.ResetTrip();
        TotalDistance = DistanceService.TotalDistance();
        PartialDistance = DistanceService.PartialDistance();
    });

    public ICommand PartialResetCommand => new Command(async () => {
        if (ResetInProgress || !ListenerIsRunning) return;
        ResetInProgress = true;

        _ = await DistanceService.ResetPartialDistance();
        PartialDistance = DistanceService.PartialDistance();

        ResetInProgress = false;
    });

    #endregion


    public HomeViewModel(IGpsManager manager, IDeviceService deviceService, ILoggingService loggingService, INavigationService navigationService, IDistanceService distanceService) 
        : base(loggingService, navigationService) 
    {
        GpsManager = manager;
        DeviceService = deviceService;
        DistanceService = distanceService;

        var gpsEnabled = DeviceService.GpsServicesEnabled();
        LoggingService.Log($"current listener is running: {GpsManager.CurrentListener != null}, gps-enabled:{gpsEnabled}");
        ListenerIsRunning = GpsManager.CurrentListener != null && gpsEnabled;

        SubscribeToLocationChanges();
        if (gpsEnabled && !ListenerIsRunning) 
        {
            Task.Run(async () => ListenerIsRunning = await StartListener());
            return;
        }
    }

    public void Dispose() {
        subscription?.Dispose();
    }


    public void SubscribeToLocationChanges() 
    {
        LoggingService.Log($"loc-subscription listening:{subscription != null}");
        if (subscription != null) return;
        subscription = GpsManager.WhenReading().Subscribe(reading => {
            LastReading = reading;
            DistanceService.AddPosition(reading.Position);
            TotalDistance = DistanceService.TotalDistance();
            PartialDistance = DistanceService.PartialDistance();
        });
    }

    public async Task<bool> StartListener()
    {
        try
        {
            await GpsManager.StartListener(new GpsRequest
            {
                Accuracy = GpsAccuracy.High,
                BackgroundMode = GpsBackgroundMode.Realtime,
                DistanceFilterMeters = 5
            });

            return true;
        }
        catch (Exception ex) 
        {
            LoggingService.Log($"ERROR {ex.Message}");
            var t = ex.Message == "There is already a GPS listener running";
            return ex.Message == "There is already a GPS listener running" ;
        }
    }
}
