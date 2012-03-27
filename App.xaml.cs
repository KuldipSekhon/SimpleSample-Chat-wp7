using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Info;

namespace SimpleSample_Chat
{
    public partial class App : Application
    {
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Simple application ID
        /// </summary>
        public int AppID = 335;
        /// <summary>
        /// Simple owner ID
        /// </summary>
        public int OwnerID = 4331;
        /// <summary>
        /// Simple Authorization Key
        /// </summary>
        public string AuthKey = "WfJmNOEunR7wX23";
        /// <summary>
        /// Simple Authorization Secret
        /// </summary>
        public string AuthSecret = "gtGRdOKjEaurz46";

        /// <summary>
        /// Get Device ID
        /// </summary>
        /// <returns></returns>
        public string GetDeviceUniqueID()
        {
            try
            {
                byte[] result = null;
                object uniqueId;
                if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                    result = (byte[])uniqueId;

                return Convert.ToBase64String(result);
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Service Context
        /// </summary>
        public QuickBloxSDK_Silverlight.QuickBlox QBlox
        { get; set; }

        public App()
        {            
            UnhandledException += Application_UnhandledException;         
            InitializeComponent();
            InitializePhoneApplication();

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Application.Current.Host.Settings.EnableFrameRateCounter = true;
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            //Start to use QuickBlox Service
            this.QBlox = new QuickBloxSDK_Silverlight.QuickBlox(AppID, OwnerID, this.AuthKey, this.AuthSecret, null, true, this.GetDeviceUniqueID());
            //Set the background events update rate - 3 sec
            this.QBlox.PingInterval = 3;
            //Start to work in background mode
            this.QBlox.BackgroundUpdateStart();
        }
       
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }


        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {

                System.Diagnostics.Debugger.Break();
            }
        }

        private bool phoneApplicationInitialized = false;

        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            RootFrame.NavigationFailed += RootFrame_NavigationFailed;
            phoneApplicationInitialized = true;
        }

        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }
    }
}