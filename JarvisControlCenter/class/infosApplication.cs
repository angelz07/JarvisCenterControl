using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisControlCenter
{
    class InfosApplication
    {
        private static ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();

        public string infosAppilcation(string item) {
            string retour = "false";

            try
            {
                string jsonFile = File.ReadAllText("infosApplication.json").ToString();
                JObject dataJson = JObject.Parse(jsonFile);
                string devices = dataJson[item].ToString();

                retour = devices;
            }
            catch (Exception ex)
            {
                consoleLogInfos.addLineToLogs("error", "infosAppilcation : " + ex.Message);
                throw;
            }

            return retour;
        }
    }
}
