using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace JarvisControlCenter
{
    class GestionFileConfig
    {
        private static VariableGlobals variableGlobals = new VariableGlobals();

        public StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

        public async Task<string> initializeFileJson()
        {
            string retour = "";
            string devicesJson = "devices.json";
            string infosApplicationJson = "infosApplication.json";
            string logsJson = "logs.json";

            try
            {
                //devices.json
                string isDeviceJson = await isFilePresent(devicesJson);
                if (isDeviceJson == "true")
                {
                    string isCorrect = await testIfDeviceJsonCorrect(devicesJson);
                    if (isCorrect != "true")
                    {
                        string result = await copyFileConfig(devicesJson);
                        retour = retour + " Result copy devicesJson: " + result;
                    }
                }
                else
                {
                    string result = await copyFileConfig(devicesJson);
                    retour = retour + " Result copy devicesJson: " + result;
                }// isDeviceJson

                // infosApplication.json
                string isInfosApplicationJson = await isFilePresent(infosApplicationJson);
                if (isInfosApplicationJson == "true")
                {
                    string isCorrect = await testIfInfosApplicationCorrect(infosApplicationJson);
                    if (isCorrect != "true")
                    {
                        string result = await copyFileConfig(infosApplicationJson);
                        retour = retour + " Result copy infosApplicationJson: " + result;
                    }
                }
                else
                {
                    string result = await copyFileConfig(infosApplicationJson);
                    retour = retour + " Result copy infosApplicationJson: " + result;
                }// Fin infosApplication.json


                //logs.json
                string isLogsJson = await isFilePresent(logsJson);
                if (isLogsJson == "true")
                {
                    string isCorrect = await testIfLogsCorrect(logsJson);
                    if (isCorrect != "true")
                    {
                        string result = await copyFileConfig(logsJson);
                        retour = retour + " Result copy logsJson: " + result;
                    }
                }
                else
                {
                    string result = await copyFileConfig(logsJson);
                    retour = retour + " Result copy logsJson: " + result;
                }// logs.Json

                variableGlobals.configFileIsPresent = "true";

                retour = "true";
            }
            catch (Exception ex)
            {
                retour = ex.Message;
            }

            return retour;
        }

        public async Task<string> isFilePresent(string fileName)
        {
            string retour = "false";
            try
            {
                var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
                if (item != null)
                {
                    retour = "true";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return retour;
        }




        //test si devices.json est correct
        public async Task<string> testIfDeviceJsonCorrect(string file)
        {
            string retour = "false";
            try
            {
                // StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile path = await storageFolder.GetFileAsync(file);
                string jsonFile = await Windows.Storage.FileIO.ReadTextAsync(path);

                JObject dataJson = JObject.Parse(jsonFile);
                var devices = dataJson["devices"];

                if (devices.ToString() != null && devices.ToString() != "")
                {
                    retour = "true";
                }
            }
            catch (Exception ex)
            {
                retour = ex.Message;
            }



            return retour;
        }

        //test si infosApplication.json est correct
        public async Task<string> testIfInfosApplicationCorrect(string file)
        {
            string retour = "false";
            try
            {
                //  StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile path = await storageFolder.GetFileAsync(file);
                string jsonFile = await Windows.Storage.FileIO.ReadTextAsync(path);

                JObject dataJson = JObject.Parse(jsonFile);

                string ipFhem = dataJson["ipFhem"].ToString();
                string portFhem = dataJson["portFhem"].ToString();
                string loginFhem = dataJson["loginFhem"].ToString();
                string passFhem = dataJson["passFhem"].ToString();

                string isIpFhem = "false";
                if (ipFhem != "false" && ipFhem != "")
                {
                    isIpFhem = "true";
                }
                string isPortFhem = "false";
                if (portFhem != "false" && portFhem != "")
                {
                    portFhem = "true";
                }
                string isLoginFhem = "false";
                if (loginFhem != "false" && loginFhem != "")
                {
                    loginFhem = "true";
                }
                string isPassFhem = "false";
                if (passFhem != "false" && passFhem != "")
                {
                    passFhem = "true";
                }

                if (isIpFhem == "true" && isPortFhem == "true" && isLoginFhem == "true" && isPassFhem == "true")
                {
                    retour = "true";
                }
            }
            catch (Exception ex)
            {

                retour = ex.Message;
            }

            return retour;
        }


        //test si logs.json est correct
        public async Task<string> testIfLogsCorrect(string file)
        {
            string retour = "false";
            try
            {
                //  StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile path = await storageFolder.GetFileAsync(file);
                string jsonFile = await Windows.Storage.FileIO.ReadTextAsync(path);

                JObject dataJson = JObject.Parse(jsonFile);
                var logs = dataJson["logs"];

                if (logs.ToString() != null && logs.ToString() != "")
                {
                    retour = "true";
                }
            }
            catch (Exception ex)
            {
                retour = ex.Message;
            }
            return retour;
        }


        //Copy des fichiers
        public async Task<string> copyFileConfig(string fileName)
        {
            string retour = "false";
            try
            {
                string jsonFile = File.ReadAllText(fileName).ToString();
                JObject dataJson = JObject.Parse(jsonFile);

                string data = JsonConvert.SerializeObject(dataJson);

                var localFolder = ApplicationData.Current.LocalFolder;
                var localFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                var fileBytes = System.Text.Encoding.UTF8.GetBytes(data);
                using (var s = await localFile.OpenStreamForWriteAsync())
                {
                    s.Write(fileBytes, 0, fileBytes.Length);
                }

                retour = "true";
            }
            catch (Exception ex)
            {
                retour = ex.Message;
            }

            return retour;
        }

    }
}
