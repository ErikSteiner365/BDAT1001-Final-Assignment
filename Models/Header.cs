using System;
using System.Text;
using System.Text.Json;


namespace IdentityExample.Models
{
    /// <summary>
    /// Used to format the Header part of a Json Web Token (JWT)
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Algorithm used to encryptpy the JWT
        /// </summary>
        public string alg { get; set; } = "HS256";
        /// <summary>
        /// The type of Token, JWT by default
        /// </summary>
        public string typ { get; set; } = "JWT";
        /// <summary>
        /// Convert the JWT Header to Json
        /// </summary>
        /// <returns>Json Header</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        /// <summary>
        /// Convert the JWT Header to Base64 used in the generation of a JWT
        /// Converts the Json output to Bytes then to Base64
        /// </summary>
        /// <returns>Base64 Encoded Header</returns>
        public string ToBase64String()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(ToJson()));
        }
    }
}
