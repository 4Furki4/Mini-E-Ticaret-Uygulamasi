using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.Facebook
{
    public class FacebookAccessTokenValidationResponse
    {
        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }

    public class FacebookAccessValidationData
    {
        [JsonPropertyName("data")]
        public FacebookAccessTokenValidationResponse Data { get; set; }
    }
}

//{ "data":{ "app_id":"748961220181075","type":"USER","application":"MiniETicaret","data_access_expires_at":1685575506,"expires_at":1677805200,"is_valid":true,"scopes":["email","public_profile"],"user_id":"3572351953039097"}