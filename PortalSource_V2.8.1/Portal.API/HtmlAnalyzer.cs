using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Portal.API
{
  /// <summary>
  /// A Helper Class to analyze Html code.
  /// </summary>
  public static class HtmlAnalyzer
  {
    /// <summary>
    /// Checks if the Html contains any script tags.
    /// </summary>
    /// <param name="html">Html to check</param>
    /// <returns>true, if it contains script tags.</returns>
    public static bool HasScriptTags(string html)
    {
      bool hasScript = false;
      if(!string.IsNullOrEmpty(html))
      {
        Regex regex = new Regex(@"<\s*script\s+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        hasScript = regex.IsMatch(html);
      }
      return hasScript;
    }


    /// <summary>
    /// Retrieves all names of the submit-buttons in the html. Html Comments will not be considered.
    /// </summary>
    /// <param name="html">Html to check</param>
    /// <returns>Array of all the names</returns>
    public static string[] GetSubmitNames(string html)
    {
      string match = "(?<=(\\<\\s*input\\s[^>]*(?<TypeSubmit>type\\s*=\\s*['\"]submit['\"][^>]*)?)name\\s*=\\s*['\"])" // Prefix.
        + "[^'\"<>]*"                                                                                                  // name-value.
        + "(?=(['\"](?(TypeSubmit)|[^>]*type\\s*=\\s*['\"]submit['\"])))";                                             // Postfix.
      Regex regex = new Regex(match, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
                              | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
      MatchCollection mc = regex.Matches(html);

      string[] result = new string[mc.Count];
      int index = 0;
      foreach (Match submitName in mc)
        result[index++] = submitName.ToString();

      return result;
    }


    /// <summary>
    /// Retrieves all names of the input fields in the html. Html Comments will not be considered.
    /// </summary>
    /// <param name="html">Html to check</param>
    /// <returns>Array of all the names</returns>
    public static string[] GetInputNames(string html)
    {
      string match = "(?<=(\\<\\s*(input|select|textarea)\\s[^>]*)name\\s*=\\s*['\"])[^'\"<>]*(?=(['\"]))";
      Regex regex = new Regex(match, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
                              | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
      MatchCollection mc = regex.Matches(html);

      List<string> result = new List<string>(mc.Count);
      foreach(Match inputNameMatch in mc)
      {
        string inputName = inputNameMatch.ToString(); 

        // Does the string allready exist?
        if (!result.Exists(delegate(string comp) { return 0 == string.Compare(comp, inputName.ToString(), true); }))
          result.Add(inputName.ToString());
      }

      return result.ToArray();
    }
  }
}
