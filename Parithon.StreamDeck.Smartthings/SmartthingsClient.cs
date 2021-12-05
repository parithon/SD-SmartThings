using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private bool authenticationSet = false;
        public event EventHandler<EventArgs> OnScenesRefreshed;
        public event EventHandler<EventArgs> OnDevicesRefreshed;

        public SmartthingsClient()
        {
            this.BaseAddress = new Uri("https://api.smartthings.com/v1");
            this.DefaultRequestHeaders.Clear();
        }

        public void SetAuthToken(string authToken)
        {
            this.DefaultRequestHeaders.Remove("Authorization");
            this.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            this.authenticationSet = true;
        }

        public async Task RefreshDevicesAsync()
        {
            var result = await this.GetStringAsync("devices?capability=switch");
            this.Devices = result == null ? null : JsonConvert.DeserializeObject<DeviceResults>(result).Items;
            this.OnDevicesRefreshed?.Invoke(this, EventArgs.Empty);
        }

        public async Task RefreshScenesAsync()
        {
            var result = await this.GetStringAsync("scenes");
            this.Scenes = result == null ? null : JsonConvert.DeserializeObject<SceneResults>(result).Items;
            this.OnScenesRefreshed?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerable<Device> Devices;
        public IEnumerable<Scene> Scenes;

        public async Task<bool> ExecuteSceneAsync(string sceneId)
        {
            var response = await this.PostAsync($"scenes/{sceneId}/execute", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ExecuteDeviceAsync(string deviceId, bool on)
        {
            var content = new[]
            {
                new {
                    capability = "switch",
                    command = !on ? "on" : "off"
                }
            };
            var json = JsonConvert.SerializeObject(content);
            var response = await this.PostAsync($"devices/{deviceId}/commands", new StringContent(json, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }

        private Dictionary<string, Action<bool>> _devicesToCheck = new();

        public void RegisterDevice(string deviceId, Action<bool> DeviceStatusUpdateCallback)
        {
            if (this._devicesToCheck.ContainsKey(deviceId))
            {
                this._devicesToCheck.Remove(deviceId);
            }
            this._devicesToCheck.Add(deviceId, DeviceStatusUpdateCallback);
            if (this._checkDevices == null)
            {
                this._checkDevices = new Timer(CheckDevices, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            }
        }

        public async void UnregisterDevice(string deviceId)
        {
            this._devicesToCheck.Remove(deviceId);
            if (!this._devicesToCheck.Any())
            {
                await this._checkDevices.DisposeAsync();
                this._checkDevices = null;
            }
        }

        private Timer _checkDevices;

        private void CheckDevices(object state)
        {
            if (!this.authenticationSet) return;
            System.Diagnostics.Debug.WriteLine($"Checking state of devices: {this._devicesToCheck.Count()}");
           _ = Parallel.ForEach(this._devicesToCheck, async device =>
            {
                var result = await this.GetStringAsync($"devices/{device.Key}/components/main/capabilities/switch/status");
                var data = JsonConvert.DeserializeObject<dynamic>(result);
                var value = data["switch"]["value"];
                device.Value?.Invoke(value == "on");
            });
        }
    }
}
