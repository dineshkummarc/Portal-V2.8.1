using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Portal.API.Statistics
{
  public static class StatisticHelper
  {
    static string[] _botKeywords = new string[] {"spider", "google", "yahoo", "search", "crawl", "slurp", "msn",
      "teoma", "ask.com", "alexa", "froogle", "inktomi", "looksmart", "url_spider_sql", "firefly", 
      "nationaldirectory", "ask jeeves", "tecnoseek", "infoseek", "webfindbot", "girafabot", "crawler", 
      "www.galaxy.com", "scooter", "appie", "fast", "webbug", "spade", "zyborg", "rabaz"};

    /// <summary>
    /// Determine if the source of the request is a bot.
    /// </summary>
    /// <remarks>This method will only detect the "friendly" bots with a declaration in the Agent.</remarks>
    /// <param name="req"></param>
    /// <returns></returns>
    public static bool IsBot(HttpRequest req)
    {
      string userAgent = req.UserAgent.ToLower();
      foreach (string keyword in _botKeywords)
      {
        if (userAgent.Contains(keyword))
          return true;
      }
      return false;
    }
  }
}
