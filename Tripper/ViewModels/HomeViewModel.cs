using Microsoft.Maui;
using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripper.ViewModels
{
    public class HomeViewModel
    {
        private readonly IGpsManager manager;
        private IDisposable? subscription;
        private GpsReading lastReading;
        private DateTime lastReadingDate;

        public string lastReadingDateString
        {
            get => lastReadingDate.ToString();
        }

        public string lastReadingString 
        { 
            get => lastReading.ToString();
        }

        public HomeViewModel(IGpsManager manager)
        {
            this.manager = manager;
            SubscribeToLocationChanges();
        }

        public void Dispose() {
            manager?.StopListener();
            subscription?.Dispose();
        }


        public void SubscribeToLocationChanges() 
        {
            this.subscription = this.manager.WhenReading().Subscribe(reading => {
                //TODO log reading
                lastReading = reading;
                lastReadingDate = DateTime.Now;
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
}
