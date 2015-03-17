﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SSW.TimeProAPI.Extension;
using SSW.TimeProAPI.Models;

namespace SSW.TimeProAPI
{
    public class ClientInvoicesApiHelper
    {
        private readonly string _apiKey;
        private readonly string _timeProUrlId;

        public ClientInvoicesApiHelper (string timeProUrlId, string apiKey)
        {
            _timeProUrlId = timeProUrlId;
            _apiKey = apiKey;
        }

        private string BaseRequestUri
        {
            get { return "https://" + _timeProUrlId + ".sswtimepro.com/api/ClientInvoices/"; }
        }

        public async Task<IEnumerable<ClientInvoiceModel>> GetInvoicesAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = HelperMethods.CreateAuthorizationHeader(_apiKey);

            var response = await client.GetAsync(BaseRequestUri);
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<IEnumerable<ClientInvoiceModel>>(await response.Content.ReadAsStringAsync());
            return result;

        }
        

        public async Task<ClientInvoiceModel> GetInvoiceByIdAsync(int id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = HelperMethods.CreateAuthorizationHeader(_apiKey);

            var response = await client.GetAsync(BaseRequestUri + id);
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<ClientInvoiceModel>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<IEnumerable<ClientInvoiceModel>> GetInvoicesWithinRangeAsync(int startRecord, int numberOfRecords)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = HelperMethods.CreateAuthorizationHeader(_apiKey);

            var response = await client.GetAsync(BaseRequestUri + "?startRecord=" + startRecord + "&numberOfRecords=" + numberOfRecords);
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<IEnumerable<ClientInvoiceModel>>(await response.Content.ReadAsStringAsync());
            return result;

        }

        public async Task<ClientInvoiceModel> CreateInvoiceAsync(ClientInvoiceModel clientInvoice)
        {

            using (var client = new HttpClient())
            {

                var values = HelperMethods.CreateKeyValuePairsFromReflection(clientInvoice);


                client.DefaultRequestHeaders.Authorization = HelperMethods.CreateAuthorizationHeader(_apiKey);

                var content = new FormUrlEncodedContent(values);

                HttpResponseMessage response =
                    await client.PostAsync(BaseRequestUri, content);

                string responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ClientInvoiceModel>(responseString);
            }
        }

        public async Task<ClientInvoiceModel> EditInvoiceAsync(ClientInvoiceModel clientInvoice)
        {

            using (var client = new HttpClient())
            {
                var values = HelperMethods.CreateKeyValuePairsFromReflection(clientInvoice);

                client.DefaultRequestHeaders.Authorization = HelperMethods.CreateAuthorizationHeader(_apiKey);


                var content = new FormUrlEncodedContent(values);

                HttpResponseMessage response =
                    await client.PutAsync(BaseRequestUri, content);
               
                return await GetInvoiceByIdAsync(clientInvoice.InvoiceID);

            }
        }

        public async void DeleteInvoiceAsync(int id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = HelperMethods.CreateAuthorizationHeader(_apiKey); 

            var response = await client.DeleteAsync(BaseRequestUri + id);
            response.EnsureSuccessStatusCode();


        }
    }
}