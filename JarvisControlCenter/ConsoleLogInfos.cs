using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using Windows.UI.Popups;
using System.Text.RegularExpressions;



namespace JarvisControlCenter
{
    internal class ConsoleLogInfos
    {
        
        static string logsVar = "{'logs':[{'type':'infos','text':'--------------------------'},{'type':'infos','text':'Démarage de Jarvis Control'},{'type':'infos','text':'--------------------------'},{'type':'','text':''},]}";
        


        static string logsVarGlobal()
        {
            
            return logsVar;
        }

        public string showConsoleLogInfos()
        {

            return decryptJsonLogs();
                
        }

        

        public string addLineToLogs(string type, string text)
        {
            string retour = "";
 
            try
            {
                var obj = JObject.Parse(logsVar);
                var array = obj.GetValue("logs") as JArray;

                var newLog = "{ 'type': '" + type + "', 'text': '" + text + "'}";
                var add = JObject.Parse(newLog);
                array.Add(add);
                logsVar = JsonConvert.SerializeObject(obj, Formatting.Indented);

                MainPage.updateConsoleLogInfos();
            }
            catch (Exception ex)
            {
                ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
                consoleLogInfos.addLineToLogs("error", ex.Message);
                throw;
            }

            return retour;
        }

        static string decryptJsonLogs()
        {
            try
            {
                string retour = "";
                string jsonLogs = logsVar;

                JObject dataJson = JObject.Parse(jsonLogs);
                var logs = dataJson["logs"];


                foreach (var line in logs)
                {
                    JObject dataLine = JObject.Parse(line.ToString());
                    string type = dataLine["type"].ToString();
                    string text = dataLine["text"].ToString();
                    string lineLogs;
                    if (type != "")
                    {
                        lineLogs = "'" + type + "' : " + "'" + text + "'" + Environment.NewLine;
                    }
                    else
                    {
                        lineLogs = Environment.NewLine;
                    }

                    retour = retour + lineLogs;
                    
                }

                return retour;

            }
            catch (Exception ex)
            {
                ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
                consoleLogInfos.addLineToLogs("error", ex.Message);
                string retour = "erreur dans le parsing json device" + ex.Message;
                return retour;
                throw;

            }
        }

    }
}