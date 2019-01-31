using System;
using QrScanner.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QrScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckCodePage : ContentPage
    {
        private CheckCodeViewModel viewModel;
        public CheckCodePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CheckCodeViewModel() { Navigation = Navigation };
        }
    }
}