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
using System.Linq;
using QuickBloxSDK_Silverlight.Geo;
using System.Collections.Generic;

namespace SimpleSample_Chat
{
    /// <summary>
    /// Helper Class to operate with Chat Messages
    /// </summary>
    public class MessageManager
    {
        /// <summary>
        /// Get Chat Message object for text
        /// </summary>
        /// <param name="message">text</param>
        /// <returns>new Message object</returns>
        public static string CreateChatMessage(string message)
        {
            return (new Message() { Text = message }).ToString();
        }

        /// <summary>
        /// Get Chat Messages from array of GeoData objects
        /// </summary>
        /// <param name="geoData">array of geoData</param>
        /// <returns>array of Messages</returns>
        public static Message[] GetChatMessages(GeoData[] geoData)
        {
            if (geoData == null || geoData.Length < 1)
                return null;
            else
            {
                List<Message> result = new List<Message>();
                foreach (var t in geoData)
                {
                    if (t == null || string.IsNullOrEmpty(t.Status))
                        continue;
                    try
                    {
                        Message mes = new Message(t.Status);
                        if (!mes.IsLoad)
                            continue;
                        mes.Date = t.CreatedDate;
                        mes.From = t.user;
                        result.Add(mes);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return result.Where(t => t != null).OrderBy(t => t.Date).ToArray();            
            }            
        }       
    }
}
