using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parithon.StreamDeck.Smartthings.Models
{

    public class DeviceResults
    {
        [JsonProperty("items")]
        public IEnumerable<Device> Items { get; set; }
    }

    public class Device
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("manufacturerName")]
        public string ManufacturerName { get; set; }

        [JsonProperty("presentationId")]
        public string PresentationId { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("roomId")]
        public string RoomId { get; set; }

        [JsonProperty("components")]
        public Component[] Components { get; set; }

        [JsonProperty("createTime")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        [JsonProperty("viper")]
        public Viper Viper { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("restrictionTier")]
        public int RestrictionTier { get; set; }

        [JsonProperty("deviceManufacturerCode")]
        public string DeviceManufacturerCode { get; set; }

        [JsonProperty("deviceTypeId")]
        public string DeviceTypeId { get; set; }

        [JsonProperty("deviceTypeName")]
        public string DeviceTypeName { get; set; }

        [JsonProperty("deviceNetworkType")]
        public string DeviceNetworkType { get; set; }

        [JsonProperty("parentDeviceId")]
        public string ParentDeviceId { get; set; }

        [JsonProperty("dth")]
        public Dth Dth { get; set; }
    }

    public class Profile
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Viper
    {
        [JsonProperty("manufacturerName")]
        public string ManufacturerName { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }
    }

    public class Dth
    {
        [JsonProperty("completedSetup")]
        public bool CompletedSetup { get; set; }

        [JsonProperty("deviceNetworkType")]
        public string DeviceNetworkType { get; set; }

        [JsonProperty("deviceTypeId")]
        public string DeviceTypeId { get; set; }

        [JsonProperty("deviceTypeName")]
        public string DeviceTypeName { get; set; }

        [JsonProperty("executingLocally")]
        public bool ExecutingLocally { get; set; }

        [JsonProperty("hubId")]
        public string HubId { get; set; }

        [JsonProperty("networkSecurityLevel")]
        public string NetworkSecurityLevel { get; set; }
    }

    public class Component
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("capabilities")]
        public Capability[] Capabilities { get; set; }

        [JsonProperty("categories")]
        public Category[] Categories { get; set; }
    }

    public class Capability
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }

    public class Category
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("categoryType")]
        public string CategoryType { get; set; }
    }
}
