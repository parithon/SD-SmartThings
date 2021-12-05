using Newtonsoft.Json;
using Parithon.StreamDeck.SDK;
using Parithon.StreamDeck.SDK.Messages;
using Parithon.StreamDeck.SDK.Models;
using Parithon.StreamDeck.Smartthings.Models;

namespace Parithon.StreamDeck.Smartthings.Actions
{
    public class SceneAction : StreamDeckAction
    {
        private readonly SmartthingsClient client;
        private readonly ActionSettings settings = new();

        public SceneAction(SmartthingsClient client)
        {
            this.client = client;
            this.States.Add(new StreamDeckActionState() { Image = "Images/sceneActionState" });
        }

        public override string Icon => "Images/sceneAction";

        public override string Name => "Scene";

        public override string PropertyInspectorPath => "PI/sceneAction.html";

        public override void Initialize(AppearPayload payload)
        {
            if (payload.Settings.ContainsKey("Id"))
            {
                settings.Id = payload.Settings.Id;
            }
            this.client.OnScenesRefreshed += (s, e) => GetScenes();
            base.Initialize(payload);
        }

        public override async void SendToPlugin(dynamic payload)
        {
            if (payload.command != null)
            {
                if (payload.command == "refresh")
                {
                    await this.client.RefreshScenesAsync();
                }
                else if (payload.command == "select" && payload.sceneId != null)
                {
                    settings.Id = payload["sceneId"];
                    SaveSettings();
                }
            }
        }

        public override void PropertyInspector(bool isVisible)
        {
            if (isVisible)
            {
                GetScenes();
            }
            else
            {
                SaveSettings();
            }
            base.PropertyInspector(isVisible);
        }

        public override void OnKeyDown(KeyPayload payload)
        {
        }

        public override async void OnKeyUp(KeyPayload payload)
        {
            var result = await this.client.ExecuteSceneAsync(settings.Id);
            if (result)
            {
                await this.SendAsync(new ShowOKMessage(this.Context));
            }
            else
            {
                await this.SendAsync(new ShowAlertMessage(this.Context));
            }
        }

        private async void GetScenes()
        {
            var scenes = this.client.Scenes;
            if (scenes != null && scenes.Any())
            {
                await this.SendAsync(new SendToPropertyInspectorMessage(typeof(SceneAction).FullName, this.Context, new
                {
                    scenes = scenes.Select(s => new { s.SceneId, s.SceneName, Selected = (s.SceneId == settings.Id) })
                }));
            }
        }

        private async void SaveSettings()
        {
            var settingsJSON = JsonConvert.SerializeObject(settings);
            await this.SendAsync(new SetSettingsMessage(this.Context, this.settings));
        }
    }
}
