using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Json;
using System.Net;

namespace OrbisSuite
{
    public class TMDB
    {
        private string Internal_TitleID = "";
        public string TitleID {
            get
            {
                return Internal_TitleID;
            }
            set
            {
                Internal_TitleID = value.Contains("_00") ? value : value + "_00";

                //Update the url for the new TitleID
                tmdbUrl = GetUrl();

                //Download the Json document and parse the data.
                ParseJson();
            }
        }
        public string tmdbUrl { get; private set; }
        public JsonValue Data { get; private set; }


        //Json Data
        public int Revision { get; private set; }
        public int PatchRevision { get; private set; }
        public int FormatVersion { get; private set; }
        public string NPTitleID { get; private set; }
        public string Console { get; private set; }
        public string[] Names { get; private set; } = new string[10];
        public string[] Icons { get; private set; } = new string[10];
        public int ParentalLevel { get; private set; }
        public string Pronunciation { get; private set; }
        public string ContentID { get; private set; }
        public string BackgroundImage { get; private set; }
        public string BGM { get; private set; }
        public string Category { get; private set; }
        public int PSVR { get; private set; }
        public int NEOEnable { get; private set; }


        private string GetUrl()
        {
            //TitleID format must follow CUSAXXXXX_00 format.
            if (!Regex.IsMatch(TitleID, @"CUSA\d{5}_00"))
            {
                throw new Exception("TitleID format incorrect!! Format must follow CUSAXXXXX_00.");
            }

            //tmdb Key for PS3 and PS4 credits to https://github.com/Tustin
            byte[] tmdb_key = { 0xF5, 0xDE, 0x66, 0xD2, 0x68, 0x0E, 0x25, 0x5B, 0x2D, 0xF7, 0x9E, 0x74, 0xF8, 0x90, 0xEB, 0xF3, 0x49, 0x26, 0x2F, 0x61, 0x8B, 0xCA, 0xE2, 0xA9,
                                0xAC, 0xCD, 0xEE, 0x51, 0x56, 0xCE, 0x8D, 0xF2, 0xCD, 0xF2, 0xD4, 0x8C, 0x71, 0x17, 0x3C, 0xDC, 0x25, 0x94, 0x46, 0x5B, 0x87, 0x40, 0x5D, 0x19,
                                0x7C, 0xF1, 0xAE, 0xD3, 0xB7, 0xE9, 0x67, 0x1E, 0xEB, 0x56, 0xCA, 0x67, 0x53, 0xC2, 0xE6, 0xB0,
                              };

            //Make a new Hmac sha1 digest with the key tmdb_key
            HMACSHA1 Digest = new HMACSHA1(tmdb_key);

            //Compute new digest using the data of the TitleID
            Digest.ComputeHash(Encoding.UTF8.GetBytes(TitleID));

            //return the url for the json using the TitleID and generated Digest.
            return $"https://tmdb.np.dl.playstation.net/tmdb2/{TitleID}_{BitConverter.ToString(Digest.Hash).Replace("-", "")}/{TitleID}.json";
        }

        private void ParseJson()
        {
            //Download the Json file.
            WebClient webClient = new WebClient();
            Data = JsonValue.Parse(webClient.DownloadString(tmdbUrl));

            //Parse the data.
            if(Data.ContainsKey("revision"))
                Revision = Data["revision"];

            if (Data.ContainsKey("patchRevision"))
                PatchRevision = Data["patchRevision"];

            if (Data.ContainsKey("formatVersion"))
                FormatVersion = Data["formatVersion"];

            if (Data.ContainsKey("npTitleId"))
                NPTitleID = Data["npTitleId"];

            if (Data.ContainsKey("console"))
                Console = Data["console"];

            if (Data.ContainsKey("names"))
            {
                JsonValue jNames = Data["names"];

                for(int i = 0; i < jNames.Count; i++)
                {
                    JsonValue Temp = jNames[i];
                    if (Temp.ContainsKey("name"))
                    {
                        Regex rgx = new Regex(@"[^0-9a-zA-Z +.:']");
                        Names[i] = rgx.Replace(Temp["name"], "");
                    }
                        
                }
            }

            if (Data.ContainsKey("icons"))
            {
                JsonValue jIcons = Data["icons"];

                for (int i = 0; i < jIcons.Count; i++)
                {
                    JsonValue Temp = jIcons[i];

                    if (Temp.ContainsKey("icon") && Temp.ContainsKey("type"))
                    {
                        Icons[i] = Temp["icon"];
                        //Icons[i].Size = Temp["type"];
                    }
                }
            }

            if (Data.ContainsKey("parentalLevel"))
                ParentalLevel = Data["parentalLevel"];

            if (Data.ContainsKey("pronunciation"))
                Pronunciation = Data["pronunciation"];

            if (Data.ContainsKey("contentId"))
                ContentID = Data["contentId"];

            if (Data.ContainsKey("backgroundImage"))
                BackgroundImage = Data["backgroundImage"];

            if (Data.ContainsKey("bgm"))
                BGM = Data["bgm"];

            if (Data.ContainsKey("category"))
                Category = Data["category"];

            if (Data.ContainsKey("psVr"))
                PSVR = Data["psVr"];

            if (Data.ContainsKey("neoEnable"))
                NEOEnable = Data["neoEnable"];
        }

        public TMDB(string TitleID)
        {
            //Store TitleID Locally.
            this.TitleID = TitleID;
        }
    }

    public class Icons
    {
        public string Url { get; set; }
        public string Size { get; set; }
    }
}
