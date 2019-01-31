using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using QrScanner.Model;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;


namespace QrScanner.ViewModels
{
    public class ScanCodeViewModel : BaseViewModel
    {
        public Command AcceptTicketCommand { get; set; }
        public Command ScanCodeCommand { get; set; }
        public string ScanResult { get; set; }
        public INavigation Navigation { get; set; }
        public ScanCodeViewModel()
        {
            Title = "Сканировать QR";
            ScanCodeCommand = new Command(ScanCode);
            AcceptTicketCommand = new Command(AcceptTicket);
        }

        public Ticket ScannedTicket { get; set; }
        public bool IsValidTicket { get; set; }
        //bac11b40-b7d9-42cd-9175-c11c38a9f4e7
        private async void ScanCode()
        {
            IsValidTicket = false;
            OnPropertyChanged(nameof(IsValidTicket));
            ScanResult = "";
            OnPropertyChanged(nameof(ScanResult));
            try
            {
                var scanPage = new ZXingScannerPage();
                // Navigate to our scanner page
                await Navigation.PushAsync(scanPage);
                scanPage.OnScanResult += (result) =>
                {
                    // Stop scanning
                    scanPage.IsScanning = false;

                    // Pop the page and show the result
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await GetTicketInfoByCode(result.Text);
                        await Navigation.PopAsync();
                    });
                };
            }
            catch (Exception ex)
            {
                ScanResult = "Ошибка функции ScanCode " + ex;
                OnPropertyChanged(nameof(ScanResult));
            }
        }

        private async Task<Ticket> GetTicketInfoByCode(string text)
        {
            try
            {
                if (text != null)
                {
                    ScanResult = text;
                    OnPropertyChanged(nameof(ScanResult));
                    ScannedTicket = await Requests.GetInstance().GetTicketInfo(ScanResult); // check code
                    IsValidTicket = true;
                    OnPropertyChanged(nameof(IsValidTicket));
                    OnPropertyChanged(nameof(ScannedTicket));
                    return ScannedTicket;
                }
                else
                {
                    ScanResult = "Error";
                    OnPropertyChanged(nameof(ScanResult));
                    return new Ticket();
                }
            }
            catch (Exception e)
            {
                ScanResult = "Ошибка функции GetTicketInfoByCode " + e;
                OnPropertyChanged(nameof(ScanResult));
                return new Ticket();
            }
        }

        //private async void ScanCode()
        //{
        //    ScannedTicket = new Ticket();
        //    OnPropertyChanged(nameof(ScannedTicket));
        //    IsValidTicket = false;
        //    OnPropertyChanged(nameof(IsValidTicket));
        //    try
        //    {
        //        var result = "bac11b40-b7d9-42cd-9175-c11c38a9f4e7";                 // get code
        //        if (result != null)
        //        {
        //            ScanResult = result;
        //            OnPropertyChanged(nameof(ScanResult));
        //            ScannedTicket = await Requests.GetInstance().GetTicketInfo(ScanResult); // check code
        //            IsValidTicket = true;
        //            OnPropertyChanged(nameof(IsValidTicket));
        //            OnPropertyChanged(nameof(ScannedTicket));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public string AcceptResult { get; set; }
        private async void AcceptTicket()
        {
            try
            {
                var res = await Requests.GetInstance().AcceptTicket(ScanResult);
                var mess = JsonConvert.DeserializeObject<Message>(res);

                AcceptResult = mess.MessageText;
                OnPropertyChanged(nameof(AcceptResult));
            }
            catch (Exception e)
            {
                AcceptResult = "Ошибка функции AcceptTicket " + e;
                OnPropertyChanged(nameof(AcceptResult));
            }
        }
    }

    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}