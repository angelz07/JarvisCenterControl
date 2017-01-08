using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JarvisControlCenter
{

    

    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();
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


            updateConsoleLogInfos();
            // consoleLogInfos.showConsoleLogInfos();
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

        
    }

   
}
