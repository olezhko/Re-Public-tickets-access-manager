using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using QrScanner.Model;
using QrScanner.Views;
using Xamarin.Forms;

namespace QrScanner.ViewModels
{
    public class CheckCodeViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public CheckCodeViewModel()
        {
            Title = "Проверка Кода";
            Events = new ObservableCollection<ClubEvent>();
            GetEvents();
            CheckCodeCommand = new Command(()=>CheckCode(CodeString));
            ResultTickets = new List<TicketCheck>();
            EventItemSelectedCommand = new Command(EventItemSelected);
            TicketSelectedCommand = new Command(TicketSelected);
        }

        public ObservableCollection<ClubEvent> Events { get; set; }
        private async void GetEvents()
        {
            try
            {
                Events.Clear();
                var result = await Requests.GetInstance().GetEvents();
                foreach (var item in result.Items)
                {
                    Events.Add(item);
                }
                OnPropertyChanged(nameof(Events));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public ClubEvent SelectedEvent { get; set; }


        public bool IsViewInfoTicket { get; set; }
        public bool IsSearchCodeAvailable { get; set; }
        public ICommand EventItemSelectedCommand { get; set; }
        public void EventItemSelected()
        {
            ClearResult();
            IsSearchCodeAvailable = SelectedEvent != null;
            OnPropertyChanged(nameof(IsSearchCodeAvailable));

            IsViewInfoTicket = false;
            OnPropertyChanged(nameof(IsViewInfoTicket));
        }

        public string CodeString { get; set; }
        public ICommand CheckCodeCommand { get; }
        private async void CheckCode(string codeString)
        {
            try
            {
                ClearResult();
                ResultTickets = await Requests.GetInstance().CheckCode(SelectedEvent.Id.ToString(), codeString);
                if (ResultTickets != null && ResultTickets.Count != 0)
                {
                    CheckingResult = "Билет найден";
                    OnPropertyChanged(nameof(CheckingResult));

                    IsViewInfoTicket = true;
                    OnPropertyChanged(nameof(IsViewInfoTicket));
                    OnPropertyChanged(nameof(ResultTickets));
                }
                else
                {
                    CheckingResult = "Нет Билетов";
                    OnPropertyChanged(nameof(CheckingResult));
                }
            }
            catch (Exception e)
            {
                CheckingResult = "Ошибка функции CheckCode " + e;
                OnPropertyChanged(nameof(CheckingResult));
            }
        }

        public string CheckingResult { get; set; }
        public List<TicketCheck> ResultTickets { get; set; }


        public TicketCheck SelectedTicket { get; set; }
        public ICommand TicketSelectedCommand { get; set; }

        // если человек уже проходил, то показывать аларму, что он уже был
        // выводить информацию, провведена ли оплата
        private async void TicketSelected()
        {
            // здесь делать проверку, если билет уже был принят

            TicketAcceptorViewModel viewModel = new TicketAcceptorViewModel();
            viewModel.Ticket = SelectedTicket;
            TicketAccetptorPage page = new TicketAccetptorPage()
            {
                BindingContext = viewModel,
            };
            await Navigation.PushAsync(page);
        }
        
        private void ClearResult()
        {
            ResultTickets = new List<TicketCheck>();
            OnPropertyChanged(nameof(ResultTickets));
            CheckingResult = "";
            OnPropertyChanged(nameof(CheckingResult));
        }
    }
}