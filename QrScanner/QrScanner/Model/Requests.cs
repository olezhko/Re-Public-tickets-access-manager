using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QrScanner.Model;

namespace QrScanner
{
    public class Requests
    {
        #region Singleton
        private static Requests instance;
        public static Requests GetInstance()
        {
            if (instance == null)
                instance = new Requests();
            return instance;
        }
        #endregion

        public Requests()
        {
            var authData = string.Format("{0}:{1}", Settings.Name, Settings.Password);
            var authHeaderValue = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(authData));

            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        HttpClient client;

        private static string domain = @"https://republic-club.by/api";
        private string _getEventsUrl = domain + @"/admin/events?sort=startEvent&order=desc&page=0&size=100&filter=";
        private string _checkLoginPassUrl = domain + @"/loginprocessing?username={0}&password={1}";
        private string _getTicketInfoUrl = domain + @"/public/orders/{0}";
        private string _acceptTicketUrl = domain + @"/tickets/accept?uuid={0}";
        private string _checkCodeUrl = domain + @"/tickets/list?eventId={0}&filter={1}";

        public async Task<BaseListResult> GetEvents()
        {
            var uri = new Uri(string.Format(_getEventsUrl, string.Empty));
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                //content = content.Substring(content.IndexOf('['), content.LastIndexOf(']') - content.IndexOf('[') + 1);
                return JsonConvert.DeserializeObject<BaseListResult>(content);
            }
            else
            {
                var errorxml = await response.Content.ReadAsStringAsync();
                return new BaseListResult();
            }
        }

        public async Task<bool> CheckLoginPass(string name, string pass)
        {
            var uri = new Uri(string.Format(_checkLoginPassUrl, name, pass));
            var content = new StringContent($"username={name}&password={pass}", Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, content);
            var res = await response.Content.ReadAsStringAsync();
            return res.Contains("true"); // fix to check to success"
        }

        public async Task<Ticket> GetTicketInfo(string qrcode)
        {
            var uri = new Uri(string.Format(_getTicketInfoUrl, qrcode));
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Ticket>(content);
                return obj;
            }
            else
            {
                var errorxml = await response.Content.ReadAsStringAsync();
                return new Ticket();
            }
        }

        public async Task<string> AcceptTicket(string qrcode)
        {
            var uri = new Uri(string.Format(_acceptTicketUrl, qrcode));
            var content = new StringContent($"uuid={qrcode}", Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, content);
            var res = await response.Content.ReadAsStringAsync();
            return res; // fix to check to success"
        }

        public async Task<List<TicketCheck>> CheckCode(string uuid, string code)
        {
            var uri = new Uri(string.Format(_checkCodeUrl, uuid, code));
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TicketCheck>>(content);
            }
            else
            {
                var errorxml = await response.Content.ReadAsStringAsync();
                return new List<TicketCheck>();
            }
        }
    }
}
