using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parithon.StreamDeck.Smartthings.Models
{

    public class SceneResults
    {
        [JsonProperty("items")]
        public IEnumerable<Scene> Items { get; set; }
    }

    public class Scene
    {
        [JsonProperty("sceneId")]
        public string SceneId { get; set; }

        [JsonProperty("sceneName")]
        public string SceneName { get; set; }

        [JsonProperty("sceneIcon")]
        public string SceneIcon { get; set; }

        [JsonProperty("sceneColor")]
        public string SceneColor { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDate")]
        public long CreatedDate { get; set; }

        [JsonProperty("lastUpdatedDate")]
        public long LastUpdatedDate { get; set; }

        [JsonProperty("lastExecutedDate")]
        public long? LastExecutedDate { get; set; }

        [JsonProperty("editable")]
        public bool Editable { get; set; }

        [JsonProperty("apiVersion")]
        public string APIVersion { get; set; }
    }

}
