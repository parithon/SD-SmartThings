using Parithon.StreamDeck.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parithon.StreamDeck.Smartthings.Actions
{
    public class DeviceAction : StreamDeckAction
    {
        public DeviceAction()
        {
            this.States.Add(new StreamDeckActionState() { Image = "Images/deviceAction_off" });
            this.States.Add(new StreamDeckActionState() { Image = "Images/deviceAction_on" });
        }
        public override string Icon => "Images/deviceAction";

        public override string Name => "Device On/Off";

        public override void OnKeyDown(KeyPayload payload)
        {
        }

        public override void OnKeyUp(KeyPayload payload)
        {
        }
    }
}
