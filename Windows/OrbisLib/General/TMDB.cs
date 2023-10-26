using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OrbisLib2.General
{
    public class TMDB
    {
        private static readonly byte[] TmdbKey = { 0xF5, 0xDE, 0x66, 0xD2, 0x68, 0x0E, 0x25, 0x5B, 0x2D, 0xF7, 0x9E, 0x74, 0xF8, 0x90, 0xEB, 0xF3, 0x49, 0x26, 0x2F, 0x61, 0x8B, 0xCA, 0xE2, 0xA9,
            0xAC, 0xCD, 0xEE, 0x51, 0x56, 0xCE, 0x8D, 0xF2, 0xCD, 0xF2, 0xD4, 0x8C, 0x71, 0x17, 0x3C, 0xDC, 0x25, 0x94, 0x46, 0x5B, 0x87, 0x40, 0x5D, 0x19,
            0x7C, 0xF1, 0xAE, 0xD3, 0xB7, 0xE9, 0x67, 0x1E, 0xEB, 0x56, 0xCA, 0x67, 0x53, 0xC2, 0xE6, 0xB0 };

        public string TmdbUrl => GetUrl();

        public Dictionary<string, object> Data { get; } = new Dictionary<string, object>();

        private string _TitleId { get; set; }

        public TMDB(string TitleId)
        {
            _TitleId = TitleId;
            FetchDataAsync().GetAwaiter().GetResult();
        }

        private string GetUrl()
        {
            // Add the _00 if its not there.
            if (Regex.IsMatch(_TitleId, @"CUSA\d{5}") && !_TitleId.Contains("_00"))
                _TitleId += "_00";

            if (!Regex.IsMatch(_TitleId, @"CUSA\d{5}_00"))
            {
                throw new Exception("TitleID format incorrect!! Format must follow CUSAXXXXX_00.");
            }

            using var digest = new HMACSHA1(TmdbKey);
            var hash = digest.ComputeHash(Encoding.UTF8.GetBytes(_TitleId));
            var hashString = BitConverter.ToString(hash).Replace("-", "");
            return $"https://tmdb.np.dl.playstation.net/tmdb2/{_TitleId}_{hashString}/{_TitleId}.json";
        }

        public async Task FetchDataAsync()
        {
            try
            {
                //using var client = new HttpClient();
                //var response = await client.GetStringAsync(TmdbUrl);
                var webClient = new WebClient();
                var response = webClient.DownloadString(TmdbUrl);
                Data.Clear();

                var jsonData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(response);

                if (jsonData is not null)
                {
                    foreach (var (key, value) in jsonData)
                    {
                        if (value.ValueKind == JsonValueKind.Array)
                        {
                            if (key == "names")
                            {
                                var name = value.EnumerateArray().Select(element => element.GetProperty("name").GetString()).ToArray();
                                Data[key] = name;
                            }
                            else if (key == "icons")
                            {
                                var icon = value.EnumerateArray().Select(element => element.GetProperty("icon").GetString()).ToArray();
                                Data[key] = icon;
                            }
                            else
                            {
                                var arrayValues = value.EnumerateArray().Select(element => element.GetRawText()).ToArray();
                                Data[key] = arrayValues;
                            }
                        }
                        else if (value.ValueKind == JsonValueKind.Object)
                        {
                            var objectValues = new Dictionary<string, string>();
                            foreach (var property in value.EnumerateObject())
                            {
                                if (property.Value.ValueKind == JsonValueKind.String)
                                {
                                    objectValues[property.Name] = property.Value.GetString() ?? string.Empty;
                                }
                            }
                            Data[key] = objectValues;
                        }
                        else if (value.ValueKind == JsonValueKind.String)
                        {
                            Data[key] = value.GetString() ?? string.Empty;
                        }
                        else if (value.ValueKind == JsonValueKind.Number)
                        {
                            Data[key] = value.TryGetInt32(out var intValue) ? intValue : (object)value.GetDecimal();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
            
        }
        public string[] GetIcons()
        {
            return Data["icons"] as string[];
        }

        public string[] GetNames()
        {
            return Data["names"] as string[];
        }

        public string GetTitleId()
        {
            return Data["npTitleId"] as string;
        }
    }
}
