using CommercialGateAPI.Models;
using Common;
using System.Linq;
using System.Net.Http;

namespace CommercialGateAPI.Helpers
{
    public class APIHelper
    {
        public string GetSSOTokenFromHeaders(HttpRequestMessage req)
        {
            string SSOToken = "";
            if (req.Headers != null && req.Headers.Contains("cgate_api"))
            {
                var cgate_api = req.Headers.GetValues("cgate_api")?.FirstOrDefault();
                if (!string.IsNullOrEmpty(req.Headers.GetValues("cgate_api")?.FirstOrDefault()))
                {
                    var decrypt = Encryption.Encrypt_Decrypt(cgate_api, "AFINITIGENIECODE", "AFINITIGENIECODE", 1, 128, false);//always decrypt
                    if (!string.IsNullOrEmpty(decrypt))
                    {
                        string[] list = decrypt.Split('|');
                        SSOToken = list[0]?.ToString();
                    }
                }
            }
            return SSOToken;
        }

        public UserModel GetUserModelInfo(HttpRequestMessage req)
        {
            UserModel model = new UserModel();
            if (req.Headers != null && req.Headers.Contains("cgate_api"))
            {
                var cgate_api = req.Headers.GetValues("cgate_api")?.FirstOrDefault();
                if (!string.IsNullOrEmpty(req.Headers.GetValues("cgate_api")?.FirstOrDefault()) && req.Headers.GetValues("cgate_api")?.FirstOrDefault() != "undefined")
                {
                    var decrypt = Encryption.Encrypt_Decrypt(cgate_api, "AFINITIGENIECODE", "AFINITIGENIECODE", 1, 128, false);//always decrypt
                    if (!string.IsNullOrEmpty(decrypt))
                    {
                        string[] list = decrypt?.Split('|');
                        model.userKey = list[2]?.ToString();
                        model.userName = list[3]?.ToString();
                    }
                }
            }
            return model;
        }

    }
}