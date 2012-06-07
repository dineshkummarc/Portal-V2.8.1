using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;

using Portal.API.Statistics.Service;

namespace Portal.API.Statistics
{
  /// <summary>
  /// This class is used to write the statistics informations.
  /// To improve the performance, writing the statistics is encapsulated 
  /// in separate threads.
  /// </summary>
  public sealed class Statistic
  {
    #region Members

    // Holds already used statistic services.
    private static Dictionary<Type, StatisticService> services = new Dictionary<Type, StatisticService>();

    #endregion

    #region Construction / Destruction

    /// <summary>
    /// Constructor of the statistics class.
    /// It is private, so it cannot be instantiated.
    /// </summary>
    private Statistic()
    {
    }

    #endregion

    /// <summary>
    /// Creates the requested service if not already created and returns it.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static StatisticService GetService(Type type)
    {
      lock (services)
      {
        if (services.ContainsKey(type))
        {
          // Return already created statistic service.
          return services[type];
        }
        else
        {
          // Create and return requested service.
          if (!type.IsSubclassOf(typeof(StatisticService)))
            throw new ArgumentException("Requested service not based on StatisticsService.", "type");

          StatisticService service = (StatisticService)Activator.CreateInstance(type);
          if (null != service)
            services.Add(type, service);

          return service;
        }
      }
    }

    /// <summary>
    /// Returns the path to the statistics directory.
    /// </summary>
    /// <returns></returns>
    public static string GetStatisticsPath(HttpContext context)
    {
      if (null == context)
        throw new ArgumentNullException("context");

      try
      {
        string relativeStatsPath = null;
        try
        {
          relativeStatsPath = System.Configuration.ConfigurationManager.AppSettings["StatisticFilesPath"];

          // Remove leading backslash or slash if necessary.
          if (relativeStatsPath.StartsWith("/") || relativeStatsPath.StartsWith("\\"))
            relativeStatsPath = relativeStatsPath.Remove(0, 1);

          // Add trailing slash if necessary.
          if (!(relativeStatsPath.EndsWith("/") || relativeStatsPath.EndsWith("\\")))
            relativeStatsPath += "/";
        }
        catch (ConfigurationErrorsException)
        {
          relativeStatsPath = "Statistics/";
        }

        string statsPath = context.Server.MapPath("~");
        if (!(statsPath.EndsWith("/") || statsPath.EndsWith("\\")))
          statsPath += "/";
        statsPath += relativeStatsPath;

        if (!Directory.Exists(statsPath))
          Directory.CreateDirectory(statsPath);

        if (!statsPath.EndsWith("\\"))
          statsPath += "\\";
        return statsPath;
      }
      catch (HttpException) { }
      catch (NotSupportedException) { }
      catch (ArgumentNullException) { }
      catch (ArgumentException) { }
      catch (UnauthorizedAccessException) { }
      catch (PathTooLongException) { }
      catch (IOException) { }

      return "";
    }
  }
}
