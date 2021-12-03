using Parithon.StreamDeck.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parithon.StreamDeck.Smartthings.Actions
{
    public class SceneAction : StreamDeckAction
    {
        public SceneAction()
        {
            this.States.Add(new StreamDeckActionState() { Image = "Images/sceneAction_off" });
            this.States.Add(new StreamDeckActionState() { Image = "Images/sceneAction_on" });
        }

        public override string Icon => "Images/sceneAction";

        public override string Name => "Scene";

        public override string PropertyInspectorPath => "PI/sceneAction.html";

        public override void OnKeyDown(KeyPayload payload)
        {
        }

        public override void OnKeyUp(KeyPayload payload)
        {
        }
    }
}
