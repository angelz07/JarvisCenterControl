using System;
using Windows.Media.SpeechSynthesis;
using Windows.UI;
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
     
        public static MainPage Current;

        public string consoleLogInfosTxtBoxText
        {
            get { return consoleLogInfosTxtBox.Text; }
            set { consoleLogInfosTxtBox.Text = value; }
        }

        

        public MainPage()
        {
            this.InitializeComponent();

            /*
            NotifyIcon icon = new NotifyIcon(); // Declaration
            icon.BalloonTipText = "Hello, NotifyIcon!"; // Text of BalloonTip
            icon.Text = "Hello, NotifyIcon!"; // ToolTip of NotifyIcon
            icon.Icon = new System.Drawing.Icon("NotifyIcon.ico"); // Shown Icon
            icon.Visible = true;
            icon.ShowBalloonTip(1000); // Shows BalloonTip
                                       /* NotifyIcon icon = new NotifyIcon();
                                        icon.Icon = System.Drawing.SystemIcons.Application;
                                        icon.Click += delegate { MessageBox.Show("Bye!"); icon.Visible = false; Application.Exit(); };
                                        icon.Visible = true;*/

            
                Current = this;
            variableGlobals.getInfosAppilcation();
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
                 = Visibility.Collapsed;
                //Current.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                consoleLogInfos.addLineToLogs("error:", "jarvisTalkToYou: " + ex.Message);
                throw;
            }
           

            
        }

        private void MainPage1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }

   
}
