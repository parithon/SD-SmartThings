using Newtonsoft.Json;
using Parithon.StreamDeck.SDK;
using Parithon.StreamDeck.SDK.Messages;
using Parithon.StreamDeck.SDK.Models;
using Parithon.StreamDeck.Smartthings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parithon.StreamDeck.Smartthings.Actions
{
    public class DeviceAction : StreamDeckAction
    {
        private readonly SmartthingsClient client;
        private readonly ActionSettings settings = new();

        public DeviceAction(SmartthingsClient client)
        {
            this.client = client;
            this.States.Add(new StreamDeckActionState() { Image = "Images/deviceAction_off" });
            this.States.Add(new StreamDeckActionState() { Image = "Images/deviceAction_on" });
        }

        public override string Icon => "Images/deviceAction";

        public override string Name => "On/Off";

        public override string PropertyInspectorPath => "PI/deviceAction.html";

        public override async void Initialize(AppearPayload payload)
        {
            if (payload.Settings.ContainsKey("Id"))
            {
                settings.Id = payload.Settings.Id;
                this.client.RegisterDevice(settings.Id, UpdateActionStatus);
            }
            if (payload.Settings.ContainsKey("Label"))
            {
                settings.Label = payload.Settings.Label;
                await this.SendAsync(new SetTitleMessage(this.Context, settings.Label));
            }
            this.client.OnDevicesRefreshed += (s, e) => GetDevices();
            base.Initialize(payload);
        }

        public override void PropertyInspector(bool isVisible)
        {
            if (isVisible)
            {
                GetDevices();
            }
            else
            {
                SaveSettings();
            }
            base.PropertyInspector(isVisible);
        }

        public override async void SendToPlugin(dynamic payload)
        {
            if (payload.command == "refresh")
            {
                await this.client.RefreshDevicesAsync();
            }
            else if (payload.command == "select" && payload.deviceId != null)
            {
                settings.Id = payload["deviceId"];
                settings.Label = this.client.Devices.Single(d => d.DeviceId == settings.Id).Label;
                this.client.RegisterDevice(settings.Id, UpdateActionStatus);
                await this.SendAsync(new SetTitleMessage(this.Context, settings.Label));
                SaveSettings();
            }
        }

        public override void OnKeyDown(KeyPayload payload)
        {
        }

        public override async void OnKeyUp(KeyPayload payload)
        {
            var result = await this.client.ExecuteDeviceAsync(settings.Id, payload.State == 1);
            if (result)
            {
                await this.SendAsync(new ShowOKMessage(this.Context));
            }
            else
            {
                await this.SendAsync(new ShowAlertMessage(this.Context));
                await this.SendAsync(new SetStateMessage(this.Context, 0));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !string.IsNullOrEmpty(settings.Id))
            {
                this.client.UnregisterDevice(settings.Id);
            }
            base.Dispose(disposing);
        }

        private async void GetDevices()
        {
            var devices = this.client.Devices;
            if (devices != null && devices.Any())
            {
                await this.SendAsync(new SendToPropertyInspectorMessage(typeof(DeviceAction).FullName, this.Context, new { 
                    devices = devices.Select(d => new { d.DeviceId, d.Label, Selected = (d.DeviceId == settings.Id) })
                }));
            }
        }

        private async void UpdateActionStatus(bool isOn)
        {
            if (isOn)
            {
                await this.SendAsync(new SetStateMessage(this.Context, 1));
            }
            else
            {
                await this.SendAsync(new SetStateMessage(this.Context, 0));
            }
        }

        private async void SaveSettings()
        {
            var settingsJSON = JsonConvert.SerializeObject(settings);
            await this.SendAsync(new SetSettingsMessage(this.Context, this.settings));
        }
    }
}
