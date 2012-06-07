using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Web;
using System.Web.Caching;
using Portal.API.Statistics.Data;
using Portal.API.Statistics.Worker;

namespace Portal.API.Statistics.Service
{
  public class LoginStatisticService : StatisticService
  {
    // Access to the XML-file must be synchronized.
    private object lockObject = new object();

    /// <summary>
    /// Constructor.
    /// </summary>
    public LoginStatisticService()
    {
    }

    /// <summary>
    /// Starts a new thread that adds a login to the statistics file.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userId"></param>
    public void AddLogin(HttpContext context, Guid userId)
    {
      if (IsStatisticEnabled)
      {
        // Bots will be ignored.
        if (!StatisticHelper.IsBot(context.Request))
        {
          LoginWorker worker = new LoginWorker(this, context, userId);
          Thread t = new Thread(new ThreadStart(worker.AddLogin));
          t.Start();
        }
      }
    }

    public LoginStatisticData GetData(HttpContext context)
    {
      if (null == context)
        throw new ArgumentNullException("context");

      LoginStatisticData data = null;

      lock (lockObject)
      {
        string fileName = Statistic.GetStatisticsPath(context) + "Logins.xml";

        // Try to get the login statistics data from the cache.
        // If the data is not in the cache, try to load the file.
        // At last, if the file doesn't exist, create a new data object.
        object obj = context.Cache.Get("LoginStatisticData");
        if ((null != obj) && obj.GetType().Equals(typeof(LoginStatisticData)))
        {
          data = (LoginStatisticData)obj;
        }
        else if (File.Exists(fileName))
        {
          data = new LoginStatisticData();
          data.ReadXml(fileName);
        }
        else
        {
          data = new LoginStatisticData();
        }
      }

      return data;
    }

    internal void SaveData(HttpContext context, LoginStatisticData data)
    {
      if (null == context)
        throw new ArgumentNullException("context");
      if (null == data)
        throw new ArgumentNullException("data");


      lock (lockObject)
      {
        string fileName = Statistic.GetStatisticsPath(context) + "Logins.xml";

        // At last, write the data back to the login statistics file and
        // put the data object back to the cache.
        try
        {
          data.WriteXml(fileName);
        }
        catch (UnauthorizedAccessException ex)
        {
          HandleNoAccessException(Statistic.GetStatisticsPath(context), ex);
        }
        catch (SecurityException ex)
        {
          HandleNoAccessException(Statistic.GetStatisticsPath(context), ex);
        }
        catch (Exception ex)
        {
          HandleException(Statistic.GetStatisticsPath(context), ex);
        }

        context.Cache.Insert("LoginStatisticData", data, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
      }
    }

    private void HandleException(string path, Exception ex)
    {
      // We cannot show a message to the user, because we are in a background thread.
      string text = string.Format("Unexpected failure while accessing the directory {0}. The statistic functionality is disabled.", path);
      ApplicationException specEx = new ApplicationException(text, ex);
      LastException = specEx;
    }

    private void HandleNoAccessException(string path, Exception ex)
    {
      // We cannot show a message to the user, because we are in a background thread.
      string text = string.Format("No access to the directory {0}. The statistic functionality is disabled.", path);
      ApplicationException specEx = new ApplicationException(text, ex);
      LastException = specEx;
    }
  }
}
