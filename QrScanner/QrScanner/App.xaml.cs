using System;
using System.Threading;
using QrScanner.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QrScanner.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace QrScanner
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
