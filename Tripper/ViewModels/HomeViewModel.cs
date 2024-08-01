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
        readonly IGpsManager manager;


        public HomeViewModel(BaseServices services, IGpsManager manager) : base(services)
        {
            this.manager = manager;

            var l = this.manager.CurrentListener;
            this.IsUpdating = l != null;

            var mode = l?.BackgroundMode ?? GpsBackgroundMode.None;
            this.UseBackground = mode != GpsBackgroundMode.None;
            this.UseRealtime = mode == GpsBackgroundMode.Realtime;
            this.SelectedAccuracy = (l?.Accuracy ?? GpsAccuracy.Normal).ToString();

            this.GetCurrentPosition = this.CreateOneReading(LocationRetrieve.Current);
            this.GetLastReading = this.CreateOneReading(LocationRetrieve.Last);
            this.GetLastOrCurrent = this.CreateOneReading(LocationRetrieve.LastOrCurrent);
            this.Accuracies = new[]
                {
                GpsAccuracy.Highest.ToString(),
                GpsAccuracy.High.ToString(),
                GpsAccuracy.Normal.ToString(),
                GpsAccuracy.Low.ToString(),
                GpsAccuracy.Lowest.ToString()
            };

            this.ToggleUpdates = new Command(
                async () =>
                {
                    if (this.manager.CurrentListener != null)
                    {
                        await this.manager.StopListener();
                    }
                    else
                    {
                        var access = await this.manager.RequestAccess(new GpsRequest
                        {
                            BackgroundMode = this.GetMode()
                        });
                        this.Access = access.ToString();

                        if (access != AccessState.Available)
                        {
                            await this.Dialogs.DisplayAlertAsync("ERROR", "Insufficient permissions - " + access, "OK");
                            return;
                        }

                        var accuracy = (GpsAccuracy)Enum.Parse(typeof(GpsAccuracy), this.SelectedAccuracy);
                        var request = new GpsRequest
                        {
                            BackgroundMode = this.GetMode(),
                            Accuracy = accuracy
                        };
                        try
                        {
                            await this.manager.StartListener(request);
                        }
                        catch (Exception ex)
                        {
                            await this.Dialogs.DisplayAlertAsync("ERROR", ex.ToString(), "OK");
                        }
                    }
                    this.IsUpdating = this.manager.CurrentListener != null;
                }
            );

            this.RequestAccess = new Command(async () =>
            {
                var request = new GpsRequest { BackgroundMode = this.GetMode() };
                this.Access = (await this.manager.RequestAccess(request)).ToString();
            });
        }
}
