using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Portal
{
  /// <summary>
  /// Zusammenfassungsbeschreibung für IDownloadProcessor
  /// </summary>
  public interface IDownloadProcessor
  {
    /// <summary>
    /// Führt den Download aus (Schreibt die Daten in die Response). Im Fehlerfall wird eine Exception geworfen.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    void Process(HttpRequest request, HttpResponse response, string moduleType, string ModuleReference);
  }
}