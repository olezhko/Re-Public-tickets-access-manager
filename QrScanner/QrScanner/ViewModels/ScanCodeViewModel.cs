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
        public string ScanCodeString;
        private async void ScanCode()
        {
            IsVisibleAcceptButton = true;
            OnPropertyChanged(nameof(IsVisibleAcceptButton));
            AcceptResult = "";
            OnPropertyChanged(nameof(AcceptResult));
            IsValidTicket = false;
            OnPropertyChanged(nameof(IsValidTicket));
            ScanResult = "";
            OnPropertyChanged(nameof(ScanResult));

            ScannedTicket = null;
            OnPropertyChanged(nameof(ScannedTicket));


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
                    ScanCodeString = text;
                    ScannedTicket = await Requests.GetInstance().GetTicketInfo(text); // check code
                    if (ScannedTicket == null)
                    {
                        ScanResult = $"Билет не найден!";
                        OnPropertyChanged(nameof(ScanResult));
                    }
                    else
                    {
                        IsValidTicket = true;
                        OnPropertyChanged(nameof(IsValidTicket));
                        OnPropertyChanged(nameof(ScannedTicket));
                        ScanResult = ScannedTicket.Acceptor == null ? "Билет найден! Билет не активирован!" : $"Билет найден! Билет активирован! Принял: {ScannedTicket.Acceptor}";
                        OnPropertyChanged(nameof(ScanResult));

                    }

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
                return null;
            }
        }

        public bool IsVisibleAcceptButton { get; set; }
        public string AcceptResult { get; set; }
        private async void AcceptTicket()
        {
            try
            {
                var res = await Requests.GetInstance().AcceptTicket(ScanCodeString);
                var mess = JsonConvert.DeserializeObject<Message>(res);

                AcceptResult = mess.MessageText;
                OnPropertyChanged(nameof(AcceptResult));

                IsVisibleAcceptButton = false;
                OnPropertyChanged(nameof(IsVisibleAcceptButton));

                await GetTicketInfoByCode(ScanCodeString);
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