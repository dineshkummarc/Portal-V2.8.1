using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;

using Portal.API.Statistics.Data;
using Portal.API.Statistics.Worker;
using System.Globalization;
using System.Security;

namespace Portal.API.Statistics.Service
{
    public class RequestStatisticService : StatisticService
    {
        // Access to the XML-file must be synchronized.
        private object lockObject = new object();

        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestStatisticService()
        {
        }

        /// <summary>
        /// Starts a new thread that writes informations about the current request to the statistics.
        /// </summary>
        /// <param name="context">Current context object containing the necessary informations.</param>
        public void AddRequest(HttpContext context)
        {
            if (IsStatisticEnabled)
            {
              // Bots will be ignored.
              if (!StatisticHelper.IsBot(context.Request))
              {
                RequestWorker worker = new RequestWorker(this, context);
                Thread t = new Thread(new ThreadStart(worker.AddRequest));
                t.Start();
              }
            }
        }

        /// <summary>
        /// Starts a new thread that writes informations about the current visit to the statistics.
        /// </summary>
        /// <param name="context"></param>
        public void AddVisit(HttpContext context)
        {
            if (IsStatisticEnabled)
            {
              // Bots will be ignored.
              if (!StatisticHelper.IsBot(context.Request))
              {
                RequestWorker worker = new RequestWorker(this, context);
                Thread t = new Thread(new ThreadStart(worker.AddVisit));
                t.Start();
              }
            }
        }

        public RequestSummaryData GetRequestSummaryData(HttpContext context)
        {
            if (null == context)
                throw new ArgumentNullException("context");

            RequestSummaryData data = null;

            try
            {
              // Create the directory if it doesn't already exist.
              if (!Directory.Exists(Path.Combine(Statistic.GetStatisticsPath(context), "Requests")))
                Directory.CreateDirectory(Path.Combine(Statistic.GetStatisticsPath(context), "Requests"));

              lock (lockObject)
              {
                string fileName = Path.Combine(Statistic.GetStatisticsPath(context), @"Requests\Summary.xml");

                // Try to get the login statistics data from the cache.
                // If the data is not in the cache, try to load the file.
                // At last, if the file doesn't exist, create a new data object.
                object obj = context.Cache.Get("RequestSummaryData");
                if ((null != obj) && obj.GetType().Equals(typeof(RequestSummaryData)))
                {
                  data = (RequestSummaryData)obj;
                }
                else if (File.Exists(fileName))
                {
                  data = new RequestSummaryData();
                  data.ReadXml(fileName);
                }
                else
                {
                  data = new RequestSummaryData();
                }
              }
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

            return data;
        }

        internal void SaveRequestSummaryData(HttpContext context, RequestSummaryData data)
        {
          if (null == context)
            throw new ArgumentNullException("context");
          if (null == data)
            throw new ArgumentNullException("data");

          lock (lockObject)
          {
            string fileName = Path.Combine(Statistic.GetStatisticsPath(context), @"Requests\Summary.xml");

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

            context.Cache.Insert("RequestSummaryData", data, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
          }
        }

        public RequestUrlData GetRequestUrlData(HttpContext context)
        {
            if (null == context)
                throw new ArgumentNullException("context");

            RequestUrlData data = null;

            try
            {
              // Create the directory if it doesn't already exist.
              if (!Directory.Exists(Path.Combine(Statistic.GetStatisticsPath(context), "Requests")))
                Directory.CreateDirectory(Path.Combine(Statistic.GetStatisticsPath(context), "Requests"));



              lock (lockObject)
              {
                string fileName = Path.Combine(Statistic.GetStatisticsPath(context), @"Requests\Requests_{0}_{1}.xml");
                fileName = string.Format(CultureInfo.InvariantCulture, fileName, context.Timestamp.Year, context.Timestamp.Month);

                // Try to get the login statistics data from the cache.
                // If the data is not in the cache, try to load the file.
                // At last, if the file doesn't exist, create a new data object.
                object obj = context.Cache.Get(fileName);
                if ((null != obj) && obj.GetType().Equals(typeof(RequestUrlData)))
                {
                  data = (RequestUrlData)obj;
                }
                else if (File.Exists(fileName))
                {
                  data = new RequestUrlData();
                  data.ReadXml(fileName);
                }
                else
                {
                  data = new RequestUrlData();
                }
              }
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

            return data;
        }

        internal void SaveRequestUrlData(HttpContext context, RequestUrlData data)
        {
          if (null == context)
            throw new ArgumentNullException("context");
          if (null == data)
            throw new ArgumentNullException("data");

          lock (lockObject)
          {
            string fileName = Path.Combine(Statistic.GetStatisticsPath(context), @"Requests\Requests_{0}_{1}.xml");
            fileName = string.Format(CultureInfo.InvariantCulture, fileName, context.Timestamp.Year, context.Timestamp.Month);

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
            catch(SecurityException ex)
            {
              HandleNoAccessException(Statistic.GetStatisticsPath(context), ex);
            }
            catch (Exception ex)
            {
              HandleException(Statistic.GetStatisticsPath(context), ex);
            }

            context.Cache.Insert(fileName, data, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
          }
        }


        public RequestUrlData GetRequestUrlData(HttpContext context, DateTime dateTime)
        {
            if (null == context)
                throw new ArgumentNullException("context");

            RequestUrlData data = null;

            try
            {
              // Create the directory if it doesn't already exist.
              if (!Directory.Exists(Path.Combine(Statistic.GetStatisticsPath(context), "Requests")))
                Directory.CreateDirectory(Path.Combine(Statistic.GetStatisticsPath(context), "Requests"));


              lock (lockObject)
              {
                string fileName = Path.Combine(Statistic.GetStatisticsPath(context), @"Requests\Requests_{0}_{1}.xml");
                fileName = string.Format(CultureInfo.InvariantCulture, fileName, dateTime.Year, dateTime.Month);

                // Try to get the login statistics data from the cache.
                // If the data is not in the cache, try to load the file.
                // At last, if the file doesn't exist, create a new data object.
                object obj = context.Cache.Get(fileName);
                if ((null != obj) && obj.GetType().Equals(typeof(RequestUrlData)))
                {
                  data = (RequestUrlData)obj;
                }
                else if (File.Exists(fileName))
                {
                  data = new RequestUrlData();
                  data.ReadXml(fileName);
                }
                else
                {
                  data = new RequestUrlData();
                }
              }
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

            return data;
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
