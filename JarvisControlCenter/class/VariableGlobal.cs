using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace JarvisControlCenter
{
    class  VariableGlobals
    {
        private static GestionFileConfig gestionFileConfig = new GestionFileConfig();
        private static ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();

        public StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

        public string ipFhem ="";
        public string portFhem = "";
        public string loginFhem = "";
        public string passFhem = "";

        public string configFileIsPresent = "false";
       
      

        public class InfosAppJson
        {
            public string ipFhem { get; set; }
            public string portFhem { get; set; }
            public string loginFhem { get; set; }
            public string passFhem { get; set; }
        }

       

        public async Task<string> infosAppilcation(string item)
        {
            string retour = "false";
            if (configFileIsPresent == "false")
            {
                await gestionFileConfig.initializeFileJson();
            }
            //await Task.Factory.StartNew(() => { });
           
                try
                {
                    StorageFile path = await storageFolder.GetFileAsync("infosApplication.json");
                    string jsonFile = await FileIO.ReadTextAsync(path);

                    JObject dataJson = JObject.Parse(jsonFile);
                    string dataItem = dataJson[item].ToString();
                    //consoleLogInfos.addLineToLogs("debug", "dataItem : " + dataItem);
                    
                    retour = dataItem;
                }
                catch (Exception ex)
                {
                    retour =  ex.Message;
                }
           
            

            return retour;
        }

        public async void getInfosAppilcation()
        {
            ipFhem = await infosAppilcation("ipFhem");
            portFhem = await infosAppilcation("portFhem");
            loginFhem = await infosAppilcation("loginFhem");
            passFhem = await infosAppilcation("passFhem");


            
        }



        public string recordNewInfosApp(string newIpFhem, string newPortFhem, string newLoginFhem, string newIpPassFhem)
        {
            string retour = "false";

            InfosAppJson infosAooJson = new InfosAppJson
            {
                ipFhem = newIpFhem,
                portFhem = newPortFhem,
                loginFhem = newLoginFhem,
                passFhem = newIpPassFhem
            };




            try
            {
                /*
                 *  string output = JsonConvert.SerializeObject(infosAooJson);
                  string fileName = "infosApplication.json";
                  var localFolder = ApplicationData.Current.LocalFolder;
                  var localFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                  var fileBytes = System.Text.Encoding.UTF8.GetBytes(output);
                  using (var s = await localFile.OpenStreamForWriteAsync())
                  {
                      s.Write(fileBytes, 0, fileBytes.Length);
                  }
                  */

                retour = "output= ";
            }
            catch (Exception e)
            {
                retour = "error: " + e.Message;
            }

            getInfosAppilcation();
            return retour;

        }

        

       
    }
}
