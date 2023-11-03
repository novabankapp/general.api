using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NovaPay.Integrator.General.Application.Resources.Responses
{
    public class AuthResponse
    {
        [JsonProperty("access_token")]
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonProperty("refresh_token")]
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
