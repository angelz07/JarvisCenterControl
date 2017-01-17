using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace JarvisControlCenter
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class ConfigApplication : Page
    {
       
        private static ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
        private static VariableGlobals variableGlobals = new VariableGlobals();

        public static ConfigApplication CurrentConfigApp;

        public ConfigApplication()
        {
            this.InitializeComponent();
            CurrentConfigApp = this;
            //variableGlobals.getInfosAppilcation();
            updateInfosBP();
        }

        public static void updateInfosBP()
        {
            
            CurrentConfigApp.refrechInfosBP_Copy_Click(null, null);
           
        }

        public static void updateInfosAppTextbox()
        {
            variableGlobals.getInfosAppilcation();

            if (variableGlobals.ipFhem != "" && variableGlobals.portFhem != "" && variableGlobals.loginFhem != "" && variableGlobals.passFhem != "")
            {
                CurrentConfigApp.txtBoxIpFhem.Text = variableGlobals.ipFhem;
                CurrentConfigApp.txtBoxPortFhem.Text = variableGlobals.portFhem;
                CurrentConfigApp.txtBoxLoginFhem.Text = variableGlobals.loginFhem;
                CurrentConfigApp.txtBoxPassFhem.Text = variableGlobals.passFhem;
            }
            else
            {
                variableGlobals.getInfosAppilcation();
                CurrentConfigApp.txtBoxIpFhem.Text = variableGlobals.ipFhem;
                CurrentConfigApp.txtBoxPortFhem.Text = variableGlobals.portFhem;
                CurrentConfigApp.txtBoxLoginFhem.Text = variableGlobals.loginFhem;
                CurrentConfigApp.txtBoxPassFhem.Text = variableGlobals.passFhem;
            }
          
        }

       

        private void QuitterBP_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Close();
        }

        private void HamburgerButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            updateInfosBP();
        }




        private async void sauvegardeInfosBP_Click(object sender, RoutedEventArgs e)
        {
            string newIpFhem = txtBoxIpFhem.Text;
            string newPortFhem = txtBoxPortFhem.Text;
            string newLoginFhem = txtBoxLoginFhem.Text;
            string newIpPassFhem = txtBoxPassFhem.Text;

            string result = await variableGlobals.recordNewInfosApp(newIpFhem, newPortFhem, newLoginFhem, newIpPassFhem);
            string message = "";
            if (result == "true")
            {
                message = "Infos Enregistrée";
                Window.Current.Close();
            }
            else
            {
                message = result;
            }
           var Msg = new MessageDialog(message);
            await Msg.ShowAsync();
        }

        private void refrechInfosBP_Copy_Click(object sender, RoutedEventArgs e)
        {
            updateInfosAppTextbox(); 
        }

        private async void recordLogs_Click(object sender, RoutedEventArgs e)
        {
            string retour = await consoleLogInfos.copyLogsFile();
            if (retour == "true") {
                var Msg = new MessageDialog("Fichier log Enregistré.");
                await Msg.ShowAsync();
            }
        }
    }
}
