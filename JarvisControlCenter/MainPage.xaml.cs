using System;
using Windows.ApplicationModel.Core;
using Windows.Media.SpeechSynthesis;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
//using System.Windows.Forms;
/*using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

    http://www.geekchamp.com/icon-explorer/action-icons

*/

namespace JarvisControlCenter
{

    

    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
        private static VariableGlobals variableGlobals = new VariableGlobals();
        private static GestionFileConfig gestionFileConfig = new GestionFileConfig();
        private static ConfigApplication configApplication = new ConfigApplication();

        public static MainPage Current;

        public string consoleLogInfosTxtBoxText
        {
            get { return consoleLogInfosTxtBox.Text; }
            set { consoleLogInfosTxtBox.Text = value; }
        }

        

        public MainPage()
        {
           
            this.InitializeComponent();

            Current = this;
            initialisationVariable();
            //variableGlobals.getInfosAppilcation();
            updateConsoleLogInfos();
            formatConsoleLogInfosTxtBox();

        }
        public static implicit operator string(MainPage v)
        {
            throw new NotImplementedException();
        }

        public static void updateConsoleLogInfos()
        {
            Current.refreshConsoleLog_Click(null, null);
        }

        private void consoleLogInfosTxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            formatConsoleLogInfosTxtBox();
        }

       

        public void formatConsoleLogInfosTxtBox() {
            consoleLogInfosTxtBox.Foreground = new SolidColorBrush(Colors.Green);
        }

        public void  refreshConsoleLog_Click(object sender, RoutedEventArgs e)
        {
            
            string infos = consoleLogInfos.showConsoleLogInfos();
            consoleLogInfosTxtBox.Text = infos;
            formatConsoleLogInfosTxtBox();

            double pos = this.scrollBarLogs.ExtentHeight;
            scrollBarLogs.ChangeView(null, pos, null);
        }

        
        public static void jarvisTalk(SpeechSynthesisStream ttsStream)
        {
            //consoleLogInfos.addLineToLogs("debug", "test tts MainPage");
            jarvisTalkToYou(ttsStream);
        }

        private static void jarvisTalkToYou(SpeechSynthesisStream ttsStream)
        {
            try
            {
                MediaElement tts = new MediaElement();
                tts.SetSource(ttsStream, "");
                // Current.Visibility = Visibility.Collapsed;
                // (AppBar as AppBarButton).Visibility = Visibility.Collapsed;
                // = Visibility.Collapsed;
                //Current.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                consoleLogInfos.addLineToLogs("error:", "jarvisTalkToYou: " + ex.Message);
                
            }
           

            
        }

        private void MainPage1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        

       
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            variableGlobals.getInfosAppilcation();
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void ConfigBP_Click(object sender, RoutedEventArgs e)
        {
            //variableGlobals.getInfosAppilcation();
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
            Frame frame = new Frame();
            frame.Navigate(typeof(ConfigApplication), null);
            Window.Current.Content = frame;
            // You have to activate the window in order to show it later.
            Window.Current.Activate();
            

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private void HomeBP_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void initialisationVariable()
        {
            string result = await gestionFileConfig.initializeFileJson();
            if (result == "true")
            {

                consoleLogInfos.addLineToLogs("debug:", "initialisationVariable: OK" );
                variableGlobals.getInfosAppilcation();
              //  consoleLogInfos.addLineToLogs("debug:", "result_isConfigFilePresent: " + variableGlobals.configFileIsPresent);
                
            }

            variableGlobals.getInfosAppilcation();

            /*
            if (result == "true") {
                await variableGlobals.getInfosAppilcation();
            }
            consoleLogInfos.addLineToLogs("debug:", "initialisationVariable: " + result);*/
        }

       
    }

   
}
