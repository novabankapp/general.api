using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NovaPay.Integrator.General.Application.Resources.Responses
{
    public class ValidationResponse
    {
        [JsonPropertyName("customer_name")]
        [JsonProperty("customer_name")]
        public string? CustomerName { get; set; }
        [JsonPropertyName("success")]
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonPropertyName("payload")]
        [JsonProperty("payload")]
        public object? Payload { get; set; }
    }
}
