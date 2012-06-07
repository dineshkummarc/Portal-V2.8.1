using System;
using System.Web.UI;
using System.Configuration;

namespace Portal
{
    /// <summary>
    /// This class contains multiple Utility-Functions.
    /// </summary>
    public sealed class CommonUtilities
    {
        /// <summary>
        /// Helper-Class must not be instantiated, so set the constructor to private.
        /// </summary>
        private CommonUtilities() { }

        /// <summary>
        /// Add a Session-Lifeguard configured in the AppSettings "SessionKeepAlive" in the web.config. This Lifeguard is 
        /// to prevent the connection to run into a session timeout
        /// </summary>
        /// <param name="currentPage">The current Page Object</param>
        public static void AddLifeguard(System.Web.UI.Page currentPage)
        {
            if (null != System.Configuration.ConfigurationManager.AppSettings["SessionKeepAlive"])
            {
                try
                {
                    int timeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SessionKeepAlive"]);
                    AddLifeguard(timeout, currentPage);
                }
                catch (InvalidCastException) { }
            }
        }

        /// <summary>
        /// Add a Session-Lifeguard for a defined time. This Lifeguard is to prevent the connection to run into a  session 
        /// timeout
        /// </summary>
        /// <param name="timeout">Lifeguard time (minimal elapse to timeout) in Minutes, -1 = infinite, 0 = no Lifeguard
        /// </param>
        /// <param name="currentPage">The current Page Object</param>
        public static void AddLifeguard(int timeout, System.Web.UI.Page currentPage)
        {
            int interval = System.Web.HttpContext.Current.Session.Timeout;   // Minutes.

            // Prepare the script-code.
            string scriptCode = "";
            if (-1 == timeout || (timeout > interval))
            {
                scriptCode += "<script language=\"JavaScript\" src=\"CommonUtilities.js\"></script>";
                int reconnectCount = 0;
                if (-1 != timeout)
                {
                    timeout *= 60; // Seconds.
                    interval *= 60; // Seconds.

                    // Calculate the interval and the number of reconnects to keep the session alive until the defined timeout.
                    timeout -= interval;
                    reconnectCount = (int)Math.Ceiling((double)timeout / (interval - 45));
                    interval = timeout / reconnectCount;
                }
                else
                {
                    // Infinite Session-Lifeguard
                    interval = interval * 60 - 45;
                    reconnectCount = 0;
                }
                scriptCode += string.Format("<script language=\"JavaScript\">AddLifeguard({0}, {1});</script>",
                  interval * 1000, reconnectCount);
            }

            currentPage.RegisterClientScriptBlock("AddLifeguard", scriptCode);
        }



    }
}
