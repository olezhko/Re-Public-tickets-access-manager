using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace QrScanner.Model
{
    public class BaseListResult
    {
        public ObservableCollection<ClubEvent> Items { get; set; }
        public int Total { get; set; }

        public BaseListResult()
        {
            Items = new ObservableCollection<ClubEvent>();
        }
    }

    public class ClubEvent
    {
        [JsonProperty("id")] public long Id { get; set; }
        [JsonProperty("cost")] public double? Cost { get; set; }
        [JsonProperty("costText")] public string CostText { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("startEvent")] public string StartEvent { get; set; }
        [JsonProperty("endEvent")] public string EndEvent { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("coverUri")] public string CoverUri { get; set; }
        [JsonProperty("buyTicketUrl")] public string BuyTicketUrl { get; set; }
        [JsonProperty("recommendation")] public bool? Recommendation { get; set; }
        [JsonProperty("republicPay")] public bool? RepublicPay { get; set; }
        [JsonProperty("costDance")] public double? CostDance { get; set; }
        [JsonProperty("costTablePlace")] public double? CostTablePlace { get; set; }

        public override string ToString()
        {
            return $"{Name}  {StartEvent}";
        }
    }



    public class TicketPlace
    {
        [JsonProperty("place")] public int place { get; set; }
        [JsonProperty("table")] public int table { get; set; }
    }

    public class Ticket
    {
        [JsonProperty("id")] public long Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("surname")] public string Surname { get; set; }
        [JsonProperty("phone")] public string Phone { get; set; }
        [JsonProperty("eventName")] public string EventName { get; set; }
        [JsonProperty("uuid")] public string Uuid { get; set; }
        [JsonProperty("dance")] public int Dance { get; set; }
        [JsonProperty("tables")] public List<TicketPlace> Tables { get; set; }
        [JsonProperty("enterTime")] public string EnterTime { get; set; }
        [JsonProperty("acceptor")] public string Acceptor { get; set; }


        //[JsonProperty("email")] public string Email { get; set; }
        //[JsonProperty("event")] public long? EventId { get; set; }
        //[JsonProperty("people")] public int? People { get; set; }
        //[JsonProperty("arrivalTime")] public string ArrivalTime { get; set; }
        //[JsonProperty("description")] public string Description { get; set; }
        //[JsonProperty("tableNumber")] public int TableNumber { get; set; }
        //[JsonProperty("payStatus")] public string PayStatus { get; set; }
        //[JsonProperty("food")] public int Food { get; set; }
        //[JsonProperty("foodPrice")] public double? FoodPrice { get; set; }
    }

    public class TicketCheck
    {
        [JsonProperty("uuid")] public string Uuid { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("surname")] public string Surname { get; set; }
        [JsonProperty("phone")] public string Phone { get; set; }
        [JsonProperty("enterTime")] public string EnterTime { get; set; }
        [JsonProperty("acceptor")] public string Acceptor { get; set; }
        [JsonProperty("danceFloor")] public int Dance { get; set; }
        [JsonProperty("tables")] public List<TicketPlace> Tables { get; set; }
    }

    public class Message
    {
        [JsonProperty("message")] public string MessageText { get; set; }
    }
}
