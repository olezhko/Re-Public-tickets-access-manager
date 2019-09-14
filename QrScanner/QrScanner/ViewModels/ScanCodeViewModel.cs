using QrScanner.Model;
using QrScanner.Views;
using System;
using System.Threading.Tasks;
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
        public Ticket ScannedTicket { get; set; }

        public ScanCodeViewModel()
        {
            Title = "Сканировать QR";
            ScanCodeCommand = new Command(ScanCode);
        }

        private void ClearResult()
        {
            ScanResult = "";
            OnPropertyChanged(nameof(ScanResult));
            ScannedTicket = null;
            OnPropertyChanged(nameof(ScannedTicket));
        }

        private bool isScanCamera = true;
        private async void ScanCode()
        {
            ClearResult();
            if (isScanCamera)
            {
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
            else
            {
                await GetTicketInfoByCode("e2279741-22dd-4353-8fa5-b82f5fafe98e");
            }
        }

        private async Task<Ticket> GetTicketInfoByCode(string text)
        {
            try
            {
                if (text != null)
                {
                    ScannedTicket = await Requests.GetInstance().GetTicketInfo(text); // check code
                    if (ScannedTicket == null)
                    {
                        ScanResult = $"Билет не найден!";
                        OnPropertyChanged(nameof(ScanResult));
                    }
                    else
                    {
                        TicketSelected(new TicketViewModel()
                        {
                            Acceptor = ScannedTicket.Acceptor,
                            Dance = ScannedTicket.Dance,
                            EnterTime = ScannedTicket.EnterTime,
                            Name = ScannedTicket.Name,
                            Phone = ScannedTicket.Phone,
                            Surname = ScannedTicket.Surname,
                            Tables = ScannedTicket.Tables,
                            Title = ScannedTicket.EventName,
                            PayStatus = ScannedTicket.PayStatus
                        });
                    }

                    return ScannedTicket;
                }
                else
                {
                    ScanResult = "Билет не найден!";
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

        private async void TicketSelected(TicketViewModel selectedTicket)
        {
            // здесь делать проверку, если билет уже был принят
            TicketAcceptorViewModel viewModel = new TicketAcceptorViewModel();
            viewModel.Ticket = selectedTicket;
            TicketAcceptorPage page = new TicketAcceptorPage()
            {
                BindingContext = viewModel,
            };
            await Navigation.PushAsync(page);
        }
    }
}