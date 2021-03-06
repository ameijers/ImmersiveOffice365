﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Office365DataHub
{
    public class Manager : Singleton<Manager>
    {
        public delegate void OnGetSomeInformationCompleted(string result);

        public void GetSomeInformation(string url, OnGetSomeInformationCompleted onGetSomeInformationCompleted)
        {
            System.Threading.Tasks.Task.Run(
                () => GetSomeInformationAsync(url, onGetSomeInformationCompleted) );
        }

        public async Task GetSomeInformationAsync(string url, OnGetSomeInformationCompleted onGetSomeInformationCompleted)
        {
            string result = "";

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }

            onGetSomeInformationCompleted(result);
        }
    }
}
