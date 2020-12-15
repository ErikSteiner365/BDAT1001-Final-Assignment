using System;
using System.Security.Cryptography;
using System.Text;


namespace IdentityExample.Models
{
    public class Signature
    {
        private readonly Header _header;
        private readonly PayLoad _payload;
        private readonly string _securityKey;
        public Signature(Header header, PayLoad payLoad, string securityKey)
        {
            _header = header;
            _payload = payLoad;
            _securityKey = securityKey;
        }
        public string GenerateToken()
        {
            if (_header == null || _payload == null || String.IsNullOrEmpty(_securityKey))
            {
                throw new Exception("Confirm that the Header, Payload and Security Key are valid");
            }
            //Convert the Header and Payload to Base64
            string headerBase64 = _header.ToBase64String();
            string payloadBase64 = _payload.ToBase64String();
            var algorithm = new HMACSHA256();
            algorithm.Key = Encoding.ASCII.GetBytes(_securityKey);
            //Compute the Hash value of the Security Key
            var computedHash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(headerBase64 + "." + payloadBase64));
            var signature = Convert.ToBase64String(computedHash).Split('=')[0].Replace('+', '-').Replace('/', '_');
            var token = $"{headerBase64}.{payloadBase64}.{signature}";
            return token;
        }
    }
}

