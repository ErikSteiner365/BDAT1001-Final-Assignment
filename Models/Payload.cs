using System;
using System.Text;
using System.Text.Json;
using IdentityExample.Controllers;


namespace IdentityExample.Models
{
    /// Used to format the Payload part of a Json Web Token (JWT)
    public class PayLoad
    {
        public string issuer { get; set; } = "StocksApplicationServer";/// Issuer where was the JWT issed from
        public string audience { get; set; } = "StocksApplicationUser";/// Audience for which the JWT was intended for
        public string username { get; set; } = "UserName"; /// UserName
        public string email { get; set; } = "Email"; /// Email address related to the User
        public string uniqueId { get; set; } = "1"; /// A Guid representing a unique id : <example> var nameid = Guid.NewGuid().ToString();
        public long nbf { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds(); /// Not Before Timestamp
        public long iat { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds(); /// Initiated At Timestamp
                                                                                                      /// Convert the JWT Payload to Json <returns>Json Payload</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        /// Convert the JWT Payload to Base64 used in the generation of a JWT Converts the Json output to Bytes then to Base64
        /// <returns>Base64 Encoded Payload</returns>
        public string ToBase64String()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(ToJson()));
        }
    }
}

