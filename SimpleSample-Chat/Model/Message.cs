using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using System.ComponentModel;
using QuickBloxSDK_Silverlight.users;

namespace SimpleSample_Chat
{
    public class Message : INotifyPropertyChanged
    {
        #region Properties

        private bool isLoad;
        /// <summary>
        /// The result of parcing procedure (for message response)
        /// </summary>
        public bool IsLoad
        {
            get { return isLoad; }
            set
            {
                if (isLoad == value) return;
                isLoad = value;
                RaisePropertyChanged("IsLoad");
            }
        }      

        private string text;
        /// <summary>
        /// Message text
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value) return;
                text = value;
                RaisePropertyChanged("Text");
            }
        }        

        private User from;
        /// <summary>
        /// The User who has send the message
        /// </summary>
        public User From
        {
            get { return from; }
            set
            {
                if (from == value) return;
                from = value;                
                RaisePropertyChanged("From");
            }
        }

        private DateTime date;
        /// <summary>
        /// Date of message
        /// </summary>
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date == value) return;
                date = value;
                RaisePropertyChanged("Date");
            }
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        /// <summary>
        /// Void constructor
        /// </summary>
        public Message()
        { 
        
        }
        
        /// <summary>
        /// Create Message from Scheme
        /// </summary>
        /// <param name="Scheme"></param>
        public Message(string scheme)
        {
            if (string.IsNullOrEmpty(scheme))
                this.IsLoad = false;
            XElement xmlResult = XElement.Parse(this.FromBase64(scheme.Replace(' ', '+')));
            this.Text = xmlResult.Element("te").Value;
            this.IsLoad = true;
        }        

        /// <summary>
        /// Converter To Base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ToBase64(string str)
        {
            return System.Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Converter From Base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string FromBase64(string str)
        {
            byte[] tr = System.Convert.FromBase64String(str);           
            UTF8Encoding encoder = new UTF8Encoding();
            string result = encoder.GetString(tr, 0, tr.Length);
            return result;
        }       

        /// <summary>
        /// Returns the Message xml as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("<m>");
            result.Append("<t>");
            result.Append("2");
            result.Append("</t>");
            result.Append("<te>");
            result.Append(this.Text);
            result.Append("</te>");
            result.Append("</m>");
            return this.ToBase64(result.ToString());            
        }               
    }
}
