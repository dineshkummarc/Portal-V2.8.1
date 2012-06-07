using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;

namespace Portal.Modules.FormBuilder
{
  /// <summary>
  /// Dient zum ersetzen von Platzhaltern in Strings.
  /// </summary>
  public class PlaceholderReplacer
  {
    // Die Regular Expression welche zum Finden der Platzahlter eingesetzt wird.
    private Regex _placeHolderRe;

    /// <summary>
    /// Delegate welches zum ersetzen der Plathalter verwendet wird.
    /// </summary>
    /// <param name="source">Name des Platzhalters</param>
    /// <returns>Der neue Text</returns>
    public delegate string ReplacePlaceholder(string source);

    private ReplacePlaceholder _replaceDelegate;

    /// <summary>
    /// Erzeugt eine Instanz des PlaceholderReplacer.
    /// </summary>
    /// <param name="replaceDelegate">Delegate welches die Ersetzung eines einzelnen Platzhalters vornimmt.</param>
    public PlaceholderReplacer(ReplacePlaceholder replaceDelegate)
    {
      if (replaceDelegate == null)
        throw new ArgumentNullException("replaceDelegate");
      _replaceDelegate = replaceDelegate;

      // Die Regular Expression zum Suchen der Platzhalter zusammenstellen.
      _placeHolderRe = new Regex("(?!\\\\)\\{[^}]+\\}", RegexOptions.Multiline | RegexOptions.CultureInvariant
                                                      | RegexOptions.Compiled);
    }

    /// <summary>
    /// Ergänzt die Platzhalter des übergebenen Strings. 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public string Replace(string source)
    {
      if (string.IsNullOrEmpty(source))
        return "";

      StringBuilder result = new StringBuilder();

      int currentPos = 0;

      // Alle Platzhalter (mit Klammern!) ermitteln.
      MatchCollection placeHolders = _placeHolderRe.Matches(source);
      foreach (Match foundPh in placeHolders)
      {
        // Teil vor dem Platzhalter hinzufügen.
        if(currentPos != foundPh.Index)
          result.Append(source.Substring(currentPos, foundPh.Index - currentPos));

        // Den ersetzten Platzhalter hinzufügen.
        string placeholder = Unwrap(foundPh.ToString());
        result.Append(_replaceDelegate.Invoke(placeholder));

        // Den Cursor innerhalb der Quelle verschieben.
        currentPos = foundPh.Index + foundPh.Length;
      }

      // Den restlichen Teil nach dem letzten Platzhalter hinzufügen.
      if(currentPos < source.Length)
        result.Append(source.Substring(currentPos));

      return result.ToString();
    }


    /// <summary>
    /// Entfernt die Platzhalter kennzeichen vom String, so dass nur noch der Name zurückbleibt.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    protected string Unwrap(string source)
    {
      return source.Trim("{} ".ToCharArray());
    }
  }
}
