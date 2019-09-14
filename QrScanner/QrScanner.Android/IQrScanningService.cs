using QrScanner.ViewModels;
using System.Threading.Tasks;
using QrScanner.Model;
using ZXing.Mobile;

[assembly: Xamarin.Forms.Dependency(typeof(QrScanner.Droid.QrScanningService))]
namespace QrScanner.Droid
{
    public class QrScanningService : IQrScanningService
    {
        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Сканирование QR кода",
                BottomText = "Пожалуйста подождите",
            };

            var scanResult = await scanner.Scan(optionsCustom);
            return scanResult.Text;
        }
    }
}