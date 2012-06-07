using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace Portal.API.Statistics.Worker
{
  /// <summary>
  /// Base class of the statistics worker thread classes.
  /// </summary>
  class WorkerBase
  {
    // Holds the current users HttpContext.
    protected HttpContext context;

    /// <summary>
    /// Constructor of the WorkerBase class.
    /// </summary>
    /// <param name="context"></param>
    public WorkerBase(HttpContext context)
    {
      this.context = context;
    }

    /// <summary>
    /// Returns the path to the statistics directory.
    /// </summary>
    /// <returns></returns>
    protected string StatisticsPath
    {
      get
      {
        return Statistic.GetStatisticsPath(context);
      }
    }

    protected virtual void HandleException(string helpMessage, Exception ex)
    {
      // We cannot show a message to the user, because we are in a background thread.
      ApplicationException specEx = new ApplicationException(helpMessage, ex);
      LastException = specEx;
    }

    protected Exception LastException
    {
      get;
      set;
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
  }
}
