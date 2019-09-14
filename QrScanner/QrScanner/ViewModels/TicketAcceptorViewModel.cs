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

        public TicketViewModel Ticket { get; set; }
        public Command AcceptTicketCommand { get; set; }
        public string AcceptResult { get; set; }
        private async void AcceptTicket()
        {
            AcceptResult = "";
            OnPropertyChanged(nameof(AcceptResult));
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


    public class TicketViewModel : BaseViewModel
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string EnterTime { get; set; }
        public string Acceptor { get; set; }
        public int Dance { get; set; }
        public string PayStatus { get; set; }
        public List<TicketPlace> Tables { get; set; }
    }
}
