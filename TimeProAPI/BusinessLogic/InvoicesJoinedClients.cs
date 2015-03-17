﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SSW.TimeProAPI.Extension;
using SSW.TimeProAPI.Models;

namespace SSW.TimeProAPI
{
    public class InvoicesJoinedClientsApiHelper
    {
        private readonly string _apiKey;
        private readonly string _timeProUrlId;

        public InvoicesJoinedClientsApiHelper(string timeProUrlId, string apiKey)
        {
            _timeProUrlId = timeProUrlId;
            _apiKey = apiKey;
        }

        private string BaseRequestUri
        {
            get { return "https://" + _timeProUrlId + ".sswtimepro.com/api/InvoicesJoinedClients/"; }
        }

        public async Task<IEnumerable<InvoiceJoinedClientModel>> GetInvoicesAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = HelperMethods.CreateAuthorizationHeader(_apiKey);

            var response = await client.GetAsync(BaseRequestUri);
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<IEnumerable<InvoiceJoinedClientModel>>(await response.Content.ReadAsStringAsync());
            return result;

        }
        
        public async Task<IEnumerable<InvoiceJoinedClientModel>> GetInvoicesByClientIdAsync(string id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = HelperMethods.CreateAuthorizationHeader(_apiKey);

            var response = await client.GetAsync(BaseRequestUri + "?ClientID=" + id);
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<IEnumerable<InvoiceJoinedClientModel>>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<string> Range(string clientFilter, string request)
        {
            using (var client = new HttpClient())
            {
                var values = new List<KeyValuePair<string, string>>();

                client.DefaultRequestHeaders.Authorization = HelperMethods.CreateAuthorizationHeader(_apiKey);

                var content = new FormUrlEncodedContent(values);
                 
                HttpResponseMessage response =
                    await client.PostAsync(BaseRequestUri + "?clientFilter=" + clientFilter + "&request=" + request, content);

                string responseString = await response.Content.ReadAsStringAsync();

                return responseString;

            }
        }

    }
}