﻿using Prism.Common;
using Shiny.Locations;
using Tripper.Helpers;
using Tripper.Interfaces.Services;
using Tripper.Services;

namespace Tripper.ViewModels;


public class HomeViewModel : ViewModelBase
{
    private readonly IGpsManager manager;
    private IDisposable? subscription;

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

    public ICommand RefreshingCommand => new Command(() =>
    {
        if(IsRefreshing) return;

        IsRefreshing = true;
        //TODO not sure what can be ideal here but maybe rechecking the prerequisits -> permissions given? features on?!
        LoggingService?.Log("HomeView:RefreshingCommand refreshing view");
        IsRefreshing = false;
    });

    #endregion

    #region binding props

    private DateTime? lastReadingDate;
    public DateTime? LastReadingDate
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

    #endregion

    public HomeViewModel(IGpsManager manager, ILoggingService loggingService, INavigationService navigationService) 
        : base(loggingService, navigationService) 
    {
        this.manager = manager;
        SubscribeToLocationChanges();
        Task.Run(StartListener);
    }

    public void Dispose() {
        subscription?.Dispose();
    }


    public void SubscribeToLocationChanges() 
    {
        this.subscription = this.manager.WhenReading().Subscribe(reading => {
            LastReading = reading;
            LastReadingDate = DateTime.Now;
        });
    }

    public async Task StartListener()
    {
        await manager.StartListener(new GpsRequest
        {
            Accuracy = GpsAccuracy.High,
            BackgroundMode = GpsBackgroundMode.Realtime,
            DistanceFilterMeters = 5
        });
    }
}
