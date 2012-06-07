using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;

using Portal.API.Statistics.Data;
using Portal.API.Statistics.Service;

namespace Portal.API.Statistics.Worker
{
  /// <summary>
  /// This class writes informations about the current request to the statistics.
  /// </summary>
  internal class RequestWorker : WorkerBase
  {
    // Reference to the calling service.
    RequestStatisticService service;

    // Access to the data must be synchronized.
    static object lockObject = new object();

    public RequestWorker(RequestStatisticService service, HttpContext context)
      : base(context)
    {
      this.service = service;
    }

    public void AddRequest()
    {
      lock (lockObject)
      {
        try
        {
          // Update the request summary statistics.
          UpdateRequestSummary();

          // Update the monthly page request statistics.
          UpdateMonthlyRequests();
        }
        catch (Exception ex)
        {
          HandleException("Unexpected error while adding a request to the request statistics.", ex);
        }
      }
    }

    private void UpdateMonthlyRequests()
    {
      string url = null;
      try
      {
        url = context.Request.Url.ToString();
      }
      catch (NullReferenceException) { }

      if (url != null)
      {
        // Get the data.
        RequestUrlData data = service.GetRequestUrlData(context);
        if (data != null)
        {
          // Find the row for the currently requested URL.
          RequestUrlData.TRequestUrlRow row = null;
          foreach (RequestUrlData.TRequestUrlRow r in data.TRequestUrl.Rows)
          {
            if (string.Compare(r.Url, url, true, CultureInfo.InvariantCulture) == 0)
            {
              row = r;
              row.RequestCount = row.RequestCount + 1;
              break;
            }
          }

          // If the currently requested URL was not found, it must be added to the data.
          if (null == row)
          {
            row = data.TRequestUrl.NewTRequestUrlRow();
            row.Url = url;
            row.RequestCount = 1;
            data.TRequestUrl.Rows.Add(row);
          }

          // Save the data.
          service.SaveRequestUrlData(context, data);
        }
      }
    }

    private void UpdateRequestSummary()
    {
      // Get the data.
      RequestSummaryData data = service.GetRequestSummaryData(context);
      if (data != null)
      {
        // Find the row for the current month.
        RequestSummaryData.TRequestSummaryRow row = null;
        foreach (RequestSummaryData.TRequestSummaryRow r in data.TRequestSummary.Rows)
        {
          DateTime date = r.Month;
          if ((date.Year == context.Timestamp.Year) && (date.Month == context.Timestamp.Month))
          {
            row = r;
            row.RequestCount = row.RequestCount + 1;
            break;
          }
        }

        // If the current month was not found, it must be added to the data.
        if (null == row)
        {
          row = data.TRequestSummary.NewTRequestSummaryRow();
          row.Month = new DateTime(context.Timestamp.Year, context.Timestamp.Month, 1, 0, 0, 0);
          row.RequestCount = 1;
          row.VisitsCount = 0;
          data.TRequestSummary.Rows.Add(row);
        }

        // Save the data.
        service.SaveRequestSummaryData(context, data);
      }
    }

    public void AddVisit()
    {
      lock (lockObject)
      {
        // Get the data.
        RequestSummaryData data = service.GetRequestSummaryData(context);
        if (data != null)
        {
          // Find the row for the current month.
          RequestSummaryData.TRequestSummaryRow row = null;
          foreach (RequestSummaryData.TRequestSummaryRow r in data.TRequestSummary.Rows)
          {
            DateTime date = r.Month;
            if ((date.Year == context.Timestamp.Year) && (date.Month == context.Timestamp.Month))
            {
              row = r;
              row.VisitsCount = row.VisitsCount + 1;
              break;
            }
          }

          // If the current month was not found, it must be added to the data.
          if (null == row)
          {
            row = data.TRequestSummary.NewTRequestSummaryRow();
            row.Month = new DateTime(context.Timestamp.Year, context.Timestamp.Month, 1, 0, 0, 0);
            row.VisitsCount = 1;
            row.RequestCount = 0;
            data.TRequestSummary.Rows.Add(row);
          }

          // Save the data.
          service.SaveRequestSummaryData(context, data);
        }
      }
    }

    protected override void HandleException(string helpMessage, Exception ex)
    {
      base.HandleException(helpMessage, ex);

      if (null == service.LastException)
        service.LastException = LastException;
    }
  }
}
