using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using QrScanner.Model;
using Xamarin.Forms;

namespace QrScanner.ViewModels
{
    public class TicketAcceptorViewModel:BaseViewModel
    {
        public TicketAcceptorViewModel()
        {
            AcceptTicketCommand = new Command(AcceptTicket);
        }

        public TicketCheck Ticket { get; set; }
        public Command AcceptTicketCommand { get; set; }
        public string AcceptResult { get; set; }
        private async void AcceptTicket()
        {
            try
            {
                var res = await Requests.GetInstance().AcceptTicket(Ticket.Uuid);
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
}
