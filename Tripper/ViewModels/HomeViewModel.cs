using Shiny.Locations;
using System.Runtime.CompilerServices;
using Tripper.Helpers;
using Tripper.Interfaces.Services;

namespace Tripper.ViewModels;


public class HomeViewModel : ViewModelBase
{
    private readonly IGpsManager GpsManager;
    private readonly IDeviceService DeviceService;
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
        LoggingService.Log($"(HomeViewModel) (RefreshingCommand): gps-listener running:{ListenerIsRunning}, gps-enabled:{gpsEnabled}");

        ListenerIsRunning = GpsManager.CurrentListener != null && gpsEnabled;

        SubscribeToLocationChanges();
        if (gpsEnabled && !ListenerIsRunning) 
        {
            LoggingService.Log($"(HomeViewModel) (RefreshingCommand): gps enabled & no listner running -> trying to start listener");

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

    private DateTimeOffset? lastReadingDate;
    public DateTimeOffset? LastReadingDate
    {
        get => lastReadingDate;
        set => SetProperty(ref lastReadingDate, value);
    }

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

    #endregion

    public HomeViewModel(IGpsManager manager, IDeviceService deviceService, ILoggingService loggingService, INavigationService navigationService) 
        : base(loggingService, navigationService) 
    {
        GpsManager = manager;
        DeviceService = deviceService;

        var gpsEnabled = DeviceService.GpsServicesEnabled();
        LoggingService.Log($"(HomeViewModel) (CTOR): current listener is running: {GpsManager.CurrentListener != null}, gps-enabled:{gpsEnabled}");
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
        LoggingService.Log($"(HomeViewModel) (SubscribeToLocationChanges): loc-subscription listening:{subscription != null}");
        if (subscription != null) return;
        subscription = GpsManager.WhenReading().Subscribe(reading => {
            LastReading = reading;
            LastReadingDate = reading.Timestamp;
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
            LoggingService.Log($"(HomeViewModel) (StartListener): ERROR {ex.Message}");
            var t = ex.Message == "There is already a GPS listener running";
            return ex.Message == "There is already a GPS listener running" ;
        }
    }
}
