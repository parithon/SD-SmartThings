using Newtonsoft.Json;
using Parithon.StreamDeck.Smartthings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parithon.StreamDeck.Smartthings
{
    public class SmartthingsClient : HttpClient
    {
        public SmartthingsClient()
        {
            this.BaseAddress = new Uri("https://api.smartthings.com/v1");
            this.DefaultRequestHeaders.Clear();
        }

        public void SetAuthToken(string authToken)
        {
            this.DefaultRequestHeaders.Remove("Authorization");
            this.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
        }

        public async Task RefreshDevicesAsync()
        {
            var result = await this.GetStringAsync("devices");
            this.Devices = result == null ? null : JsonConvert.DeserializeObject<DeviceResults>(result).Items;
        }

        public IEnumerable<Device> Devices;
    }
}
