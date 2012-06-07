using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.API.Statistics.Service
{
  public class StatisticService
  {
    // This object is used for synchronized access to the exception.
    private object _lockObject = new object();

    public bool IsStatisticEnabled
    {
      get
      {
        bool isStatisticEnabled = false;
        bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["EnableStatistics"], out isStatisticEnabled);
        return isStatisticEnabled;
      }
    }

    /// <summary>
    /// Most of the functionality of the statistic service work in a background thread with no possibility to show
    /// exception messages. This function retrieves and consumes the last important exception.
    /// </summary>
    /// <returns></returns>
    public Exception ConsumeLastException()
    {
      Exception lastExc = LastException;
      LastException = null;
      return lastExc;
    }

    private Exception _lastException;

    internal Exception LastException
    {
      get
      {
        lock (_lockObject)
        {
          return _lastException;
        }
      }
      set
      {
        lock (_lockObject)
        {
          _lastException = value;
        }
      }
    }
  }
}
