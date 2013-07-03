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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;

using QuickBloxSDK_Silverlight.Geo;

namespace SimpleSample_Chat
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        /// <summary>
        /// Service context
        /// </summary>
        public QuickBloxSDK_Silverlight.QuickBlox QBlox
        {
            get;
            private set;
        }

        private ObservableCollection<Message> commonMessages;
        /// <summary>
        /// Messages for Common Chat
        /// </summary>
        public ObservableCollection<Message> CommonMessages
        {
            get { return commonMessages; }
            set
            {
                if (commonMessages == value) return;
                commonMessages = value;
                RaisePropertyChanged("CommonMessages");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion        

        /// <summary>
        /// Main Page
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            
            CommonMessages = new ObservableCollection<Message>();
            
            //Create context
            var MainContext = App.Current as App;            
            this.QBlox = MainContext.QBlox;

            //Add event handler for GeoService events
            this.QBlox.geoService.GeoServiceEvent += new GeoService.GeoServiceHandler(geoService_GeoServiceEvent);

            //Add event handler for Background events
            this.QBlox.BackgroundEvent += new QuickBloxSDK_Silverlight.QuickBlox.BGR(QBlox_BackgroundEvent);        
        }

        /// <summary>
        /// Event Handler for Background events
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Result"></param>
void QBlox_BackgroundEvent(string Command, object Result)
{
    this.Dispatcher.BeginInvoke(new Action(() =>
    {
        switch (Command)
        {
            case "Online":
                {
                    this.waiter.Visibility = Visibility.Collapsed;
                    //Authenticate sample user
                    this.QBlox.userService.Authenticate("", "");                            
                    break;
                }
            case "Offline":
                {
                    MessageBox.Show("Connect error");
                    break;
                }
            case "geodata":
                {
                    //Get from all geoData all chat Messages
                    var res = MessageManager.GetChatMessages(QBlox.GeoData);
                            
                    //Update chat list
                    if (res != null && CommonMessages != null)
                    {
                        if (res.Count() != CommonMessages.Count())
                            for (int i = CommonMessages.Count(); i < res.Count(); i++)
                                CommonMessages.Add(res[i]);
                    }
                    break;
                }
        }
    }));
}

        /// <summary>
        /// Event handler for GeoService Events
        /// </summary>
        /// <param name="args"></param>
        void geoService_GeoServiceEvent(GeoServiceEventArgs args)
        {
            ((App)App.Current).RootFrame.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (args.currentCommand == GeoServiceCommand.AddGeoLocation)
                {
                    this.waiter.Visibility = Visibility.Collapsed;
                    chatMessages.SelectedIndex = (chatMessages.Items.Count - 1);

                    if (args.status != QuickBloxSDK_Silverlight.Core.Status.OK)
                        MessageBox.Show(args.result.ToString());                       
                }
            }));
        }

        /// <summary>
        /// AppBar Cancel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Click(object sender, System.EventArgs e)
        {
            this.Focus();            
            chatField.Text = String.Empty; 
        }

        /// <summary>
        /// AppBar Send Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void send_Click(object sender, System.EventArgs e)
        {
            //ID of Sample User
            int UserID = 0;
            
            //Post new Message 
            this.QBlox.geoService.AddGeoLocation(new GeoData(UserID, 0, 0, MessageManager.CreateChatMessage(chatField.Text)));        

            this.Focus();                        
            chatField.Text = String.Empty;
            this.waiter.Visibility = Visibility.Visible;
        }
    }
}