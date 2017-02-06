using JarvisControlCenter;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Windows.Storage;


/*
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;*/
namespace JarvisControlCenter
{
    internal class FhemClass
    {
        private static ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
        private static FeedbackClass feedBack = new FeedbackClass();
        private static VariableGlobals variableGlobals = new VariableGlobals();
        public StorageFolder storageFolder = ApplicationData.Current.LocalFolder;


        private static string ipFhem = "192.168.1.25";
          private static string portFhem = "8083";
          private static string loginFhem = "cuesmes";
          private static string passFhem = "cuesmes";



        public async System.Threading.Tasks.Task<string[]> decryptJsonDevicesFhem(string device_texte, string action)
        {
            string[] retour = new string[4];
            try
            {
                StorageFile path = await storageFolder.GetFileAsync("devices.json");
                string jsonFile = await FileIO.ReadTextAsync(path);

                JObject dataJson = JObject.Parse(jsonFile);

              //  string jsonFile = File.ReadAllText("devices.json").ToString();
              //  JObject dataJson = JObject.Parse(jsonFile);
                var devices = dataJson["devices"];

                foreach (var line in devices)
                {
                    JObject dataLine = JObject.Parse(line.ToString());
                    string name = dataLine["name"].ToString();
                    string id = dataLine["id"].ToString();
                    string type = dataLine["type"].ToString();
                    string timer = dataLine["timer"].ToString();

                    if (device_texte == name)
                    {
                        retour[0] = name;
                        retour[1] = id;
                        retour[2] = type;
                        retour[3] = timer;
                    }

                }
                string[] sendCmdFhemResult = sendCmdFhem(retour, action);
                return retour;
            }
            catch (Exception ex)
            {
                consoleLogInfos.addLineToLogs("error", "decryptJsonDevicesFhem : " + ex.Message);
                return retour;

            }
        } // Fin decryptJsonDevicesFhem

        public string[] sendCmdFhem(string[] device_infos, string action)
        {
           

            string[] retour = new string[4];
            string name = device_infos[0];
            string id = device_infos[1];
            string type = device_infos[2];
            string timer = device_infos[3];

            string actionDecrypt = "";
            if (action == "allume" || action == "Allume" || action == "ouvre" || action == "Ouvre")
            {
                actionDecrypt = "on";
            }

            if (action == "éteint" || action == "Eteint" || action == "ferme" || action == "Ferme")
            {
                actionDecrypt = "off";
            }

         /*   string url = "";
            if (type == "switch")
            {
                //http://192.168.1.25:8083/fhem?cmd.lustre_salle_a_mangee=set%20lustre_salle_a_mangee%20off&XHR=1
                url = "http://" + variableGlobals.ipFhem + ":" + variableGlobals.portFhem + "/fhem?cmd." + id + "=set%20" + id + "%20" + actionDecrypt + "&XHR=1";

            }
           */

            try
            {
               variableGlobals.getInfosAppilcation();

                //variableGlobals.getInfosAppilcation(variableGlobals.ipFhem == "" && variableGlobals.portFhem == "" && variableGlobals.loginFhem == "" && variableGlobals.passFhem == "");
                if (variableGlobals.ipFhem != "" && variableGlobals.portFhem != "" && variableGlobals.loginFhem != "" && variableGlobals.passFhem != "")
                {
                    ipFhem = variableGlobals.ipFhem;
                    portFhem = variableGlobals.portFhem;
                    loginFhem = variableGlobals.loginFhem;
                    passFhem = variableGlobals.passFhem;

                    // consoleLogInfos.addLineToLogs("debug", "on est dans le IF : ");
                }
                UriBuilder uriB = new UriBuilder();
                uriB.Host = ipFhem;
                uriB.Port = int.Parse(portFhem);
                uriB.Path = "fhem";

                if (type == "switch")
                {
                    uriB.Query = "cmd." + id + "=set%20" + id + "%20" + actionDecrypt + "&XHR=1";
                }

                string urlFhem = uriB.ToString();

                consoleLogInfos.addLineToLogs("requete", urlFhem);
               // sendHttpRequestFhem(urlFhem, loginFhem, passFhem);
                
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(urlFhem));
                    request.Credentials = new NetworkCredential(loginFhem, passFhem);
                    var reponse = request.GetResponseAsync();
                   consoleLogInfos.addLineToLogs("Debug", "reponse : " + reponse);
                   // request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallbackFhem), request);
                    tempFeedback(name, actionDecrypt);
                    //await feedBack.feedback("ceci est un test");
                }
                catch (Exception ex) {
                    consoleLogInfos.addLineToLogs("error", "HttpWebRequest request : " + ex.Message);
                }
                
            }
            catch (Exception ex)
            {
                consoleLogInfos.addLineToLogs("error", "HttpWebRequest request : " + ex.Message);
                
            }

            return retour;
        }

        protected async void tempFeedback(string name,string action)
        {
            await sendToFeedback(name, action);
        }

        private async System.Threading.Tasks.Task<string> sendToFeedback(string name, string action)
        {
            string retour = "";

            await feedBack.feedback(name, action);
            return retour;
        }

        


        private static void ReadWebRequestCallbackFhem(IAsyncResult callbackResult)
        {
           /* try
            {
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            
                using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult))
                {
                    using (StreamReader httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
                    {
                      //  string results = httpwebStreamReader.ReadToEnd();
                       // consoleLogInfos.addLineToLogs("debug", "results = " + results);

                    }
                }

            }
            catch (Exception ex)
            {
                consoleLogInfos.addLineToLogs("error", " ReadWebRequestCallbackFhem : " + ex.Message);
                
            }
            */
        }

    }// Fin internal class Fhem
}
