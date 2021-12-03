using Parithon.StreamDeck.SDK;
using Parithon.StreamDeck.SDK.Messages;
using Parithon.StreamDeck.SDK.Models;
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

        public DeviceAction(SmartthingsClient client)
        {
            this.client = client;
            this.States.Add(new StreamDeckActionState()
            {
                Image = "Images/deviceAction_off",
                TitleAlignment = Alignment.Bottom,
                TitleColor = "#dadada"
            });
            this.States.Add(new StreamDeckActionState() { Image = "Images/deviceAction_on" });
        }
        public override string Icon => "Images/deviceAction";

        public override string Name => "On/Off";

        public override void Initialize(AppearPayload payload)
        {
            base.Initialize(payload);
        }

        public override void OnKeyDown(KeyPayload payload)
        {
        }

        public override void OnKeyUp(KeyPayload payload)
        {
        }
    }
}
