using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using QrScanner.Views;
using QrScanner.ViewModels;

namespace QrScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanCodePage : ContentPage
    {
        ScanCodeViewModel viewModel;

        public ScanCodePage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ScanCodeViewModel(){Navigation = Navigation};
        }
    }
}