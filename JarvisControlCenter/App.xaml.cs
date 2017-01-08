using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Media.SpeechRecognition;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;

using System.Diagnostics;
using Windows.UI.Popups;

using System.Threading;
//using System.Diagnostics;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Specialized;



namespace JarvisControlCenter
{

    

    struct objJson
    {
        public string test { get; set; }
    }

    /// <summary>
    /// Fournit un comportement spécifique à l'application afin de compléter la classe Application par défaut.
    /// </summary>
    sealed partial class App : Application
    {
        private static string ipFhem = "192.168.1.25";
        private static string portFhem = "8083";
        private static string loginFhem = "cuesmes";
        private static string passFhem = "cuesmes";

        

        public string thisVar { get; internal set; }


        /// <summary>
        /// Initialise l'objet d'application de singleton.  Il s'agit de la première ligne du code créé
        /// à être exécutée. Elle correspond donc à l'équivalent logique de main() ou WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoqué lorsque l'application est lancée normalement par l'utilisateur final.  D'autres points d'entrée
        /// seront utilisés par exemple au moment du lancement de l'application pour l'ouverture d'un fichier spécifique.
        /// </summary>
        /// <param name="e">Détails concernant la requête et le processus de lancement.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            

               // string[] infos = decryptJsonDevices("le lustre de la salle à mangé");

               // string[] sendCmdFhemResult = sendCmdFhem(infos, "éteint");

            Frame rootFrame = Window.Current.Content as Frame;

            // Ne répétez pas l'initialisation de l'application lorsque la fenêtre comporte déjà du contenu,
            // assurez-vous juste que la fenêtre est active
            if (rootFrame == null)
            {
                // Créez un Frame utilisable comme contexte de navigation et naviguez jusqu'à la première page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: chargez l'état de l'application précédemment suspendue
                }

                // Placez le frame dans la fenêtre active
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Quand la pile de navigation n'est pas restaurée, accédez à la première page,
                    // puis configurez la nouvelle page en transmettant les informations requises en tant que
                    // paramètre
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Vérifiez que la fenêtre actuelle est active
                Window.Current.Activate();

                try {
                    StorageFile vcd = await Package.Current.InstalledLocation.GetFileAsync("VoiceCommandDefinition.xml");
                    await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcd);
                } catch(Exception ex) {
                    ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
                    consoleLogInfos.addLineToLogs("error", ex.Message);
                }
            }
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            if (args.Kind == ActivationKind.VoiceCommand)
            {
                var commandArgs = args as VoiceCommandActivatedEventArgs;
                Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = commandArgs.Result;



                // Get the name of the voice command and the text spoken. 
                // See VoiceCommands.xml for supported voice commands.
                string voiceCommandName = speechRecognitionResult.RulePath[0];
                string textSpoken = speechRecognitionResult.Text;

                // commandMode indicates whether the command was entered using speech or text.
                // Apps should respect text mode by providing silent (text) feedback.
                string commandMode = this.SemanticInterpretation("commandMode", speechRecognitionResult);

                switch (voiceCommandName)
                {
                    case "appelFhem":
                        // Access the value of {destination} in the voice command.
                        string action = this.SemanticInterpretation("actionDomo", speechRecognitionResult);
                        string device = this.SemanticInterpretation("deviceFhem", speechRecognitionResult);

                        string[] infos = decryptJsonDevices(device);

                        string[] sendCmdFhemResult = sendCmdFhem(infos, action);


                        break;
                    default:
                        // If we can't determine what page to launch, go to the default entry point.
                        ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
                        consoleLogInfos.addLineToLogs("error", "Impossible de trouver la commande");
                        break;
                }

                //await dialog.ShowAsync();
            }
        }


        static string[] decryptJsonDevices(string device_texte)
        {

            string[] retour = new string[4];
            try
            {
                string jsonFile = File.ReadAllText("devices.json").ToString();
                JObject dataJson = JObject.Parse(jsonFile);
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

                return retour;

            }
            catch (Exception ex)
            {
                ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
                consoleLogInfos.addLineToLogs("error", ex.Message);
                return retour;
                throw;

            }

            

        }

        static string[] sendCmdFhem(string[] device_infos, string action)
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

            /*
             string ipFhem = "192.168.1.25";
            string portFhem = "8083";
            string loginFhem = "cuesmes";
            string passFhem = "cuesmes";


            http://192.168.1.25:8083/fhem?cmd.lustre_salle_a_mangee=set%20lustre_salle_a_mangee%20off&XHR=1

             */
           

            string url = "http://" + ipFhem + ":" + portFhem + "/fhem?cmd." + id + "=set%20" + id + "%20" + actionDecrypt + "&XHR=1";

            try
            {

                UriBuilder uriB = new UriBuilder();
                uriB.Host = ipFhem;
                uriB.Port = int.Parse(portFhem);
                uriB.Path = "fhem";
                uriB.Query = "cmd." + id + "=set%20" + id + "%20" + actionDecrypt + "&XHR=1";

                string urlFhem = uriB.ToString();
                
                ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
                consoleLogInfos.addLineToLogs("requete", urlFhem);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(urlFhem));
                request.Credentials = new NetworkCredential(loginFhem, passFhem);
                request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);



            }
            catch (Exception ex)
            {
                ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
                consoleLogInfos.addLineToLogs("error", ex.Message);
                throw;
            }
            




            return retour;
        }

        private static void ReadWebRequestCallback(IAsyncResult callbackResult)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;

                using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult))
                {
                    using (StreamReader httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
                    {
                        string results = httpwebStreamReader.ReadToEnd();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
                consoleLogInfos.addLineToLogs("error", ex.Message);
                throw;
            }
        }

        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }

        /// <summary>
        /// Appelé lorsque la navigation vers une page donnée échoue
        /// </summary>
        /// <param name="sender">Frame à l'origine de l'échec de navigation.</param>
        /// <param name="e">Détails relatifs à l'échec de navigation</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Appelé lorsque l'exécution de l'application est suspendue.  L'état de l'application est enregistré
        /// sans savoir si l'application pourra se fermer ou reprendre sans endommager
        /// le contenu de la mémoire.
        /// </summary>
        /// <param name="sender">Source de la requête de suspension.</param>
        /// <param name="e">Détails de la requête de suspension.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: enregistrez l'état de l'application et arrêtez toute activité en arrière-plan
            deferral.Complete();
        }


    }


}
