using QrScanner.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QrScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
		    if (Settings.IsLogin)
		    {
		        NameEntry.Text = Settings.Name;
		        PassEntry.Text = Settings.Password;
            }
		}

	    private async void Login_Clicked(object sender, EventArgs e)
	    {
	        ResultErrorLabel.Text = "";

            string name = NameEntry.Text;
	        string pass = PassEntry.Text;

	        try
	        {
	            var res = await Requests.GetInstance().CheckLoginPass(name, pass);
	            if (res)
	            {
	                Settings.Name = name;
	                Settings.Password = pass;
	                Settings.IsLogin = true;
	                Application.Current.MainPage = new MainPage();
	            }
	            else
	            {
	                ResultErrorLabel.Text = "Ошибка авторизации";
	            }
            }
	        catch (Exception exception)
	        {
	            ResultErrorLabel.Text = "Ошибка авторизации. Проверьте подключение к интернету.";
            }
	    }
	}
}