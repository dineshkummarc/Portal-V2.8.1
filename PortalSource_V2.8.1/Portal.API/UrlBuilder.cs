using System;
using System.Web;
using System.Collections.Specialized;
using System.Text;

namespace Portal.API
{
  /// <summary>
  /// Helper Class for the handling of a URL.
  /// </summary>
  public class UrlBuilder : UriBuilder
  {
    #region Member Variables

    NameValueCollection _queryParams;

    #endregion

    #region Properties

    /// <summary>
    /// Returns the QueryString.
    /// </summary>
    public NameValueCollection QueryString
    {
      get
      {
        if (_queryParams == null)
        {
          _queryParams = new NameValueCollection();
        }

        return _queryParams;
      }
    }

    /// <summary>
    /// The name of the page.
    /// </summary>
    public string PageName
    {
      get
      {
        string path = base.Path;
        return path.Substring(path.LastIndexOf("/") + 1);
      }
      set
      {
        string path = base.Path;
        path = path.Substring(0, path.LastIndexOf("/"));
        base.Path = string.Concat(path, "/", value);
      }
    }
    #endregion

    #region Construction / Destruction

    /// <summary>
    /// Initializes a new instance of the UrlBuilder class. 
    /// </summary>
    public UrlBuilder()
      : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the UrlBuilder class with settings of the request.
    /// </summary>
    public UrlBuilder(System.Web.HttpRequest request)
      : base(request.Url.AbsoluteUri)
    {
      UpdateQueryParams();
    }


    /// <summary>
    /// Initializes a new instance of the UrlBuilder class with the specified URI. 
    /// </summary>
    /// <param name="uri"></param>
    public UrlBuilder(string uri)
      : base(uri)
    {
      UpdateQueryParams();
    }


    /// <summary>
    /// Initializes a new instance of the UrlBuilder class with the specified Uri instance.
    /// </summary>
    /// <param name="uri"></param>
    public UrlBuilder(Uri uri)
      : base(uri)
    {
      UpdateQueryParams();
    }

    /// <summary>
    /// Initializes a new instance of the UrlBuilder class with the specified scheme and host. 
    /// </summary>
    /// <param name="schemeName"></param>
    /// <param name="hostName"></param>
    public UrlBuilder(string schemeName, string hostName)
      : base(schemeName, hostName)
    {
    }


    /// <summary>
    /// Initializes a new instance of the UrlBuilder class with the specified scheme, host, and port. 
    /// </summary>
    /// <param name="scheme"></param>
    /// <param name="host"></param>
    /// <param name="portNumber"></param>
    public UrlBuilder(string scheme, string host, int portNumber)
      : base(scheme, host, portNumber)
    {
    }


    /// <summary>
    /// Initializes a new instance of the UrlBuilder class with the specified scheme, host, port number, and path. 
    /// </summary>
    /// <param name="scheme"></param>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="pathValue"></param>
    public UrlBuilder(string scheme, string host, int port, string pathValue)
      : base(scheme, host, port, pathValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the UrlBuilder class with the specified scheme, host, port number, path and query string or fragment identifier.
    /// </summary>
    /// <param name="scheme"></param>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="path"></param>
    /// <param name="extraValue"></param>
    public UrlBuilder(string scheme, string host, int port, string path, string extraValue)
      : base(scheme, host, port, path, extraValue)
    {
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Returns the Url.
    /// </summary>
    /// <returns></returns>
    public new string ToString()
    {
      UpdateQueryString();

      return base.Uri.AbsoluteUri;
    }

    /// <summary>
    /// Terminates the current execution and navigates to the url with a redirect.
    /// </summary>
    public void RedirectTo()
    {
      RedirectTo(true);
    }

    /// <summary>
    /// Navigates with a redirect to the url.
    /// </summary>
    /// <param name="endResponse">Should the current execution terminate?</param>
    public void RedirectTo(bool endResponse)
    {
      HttpContext.Current.Response.Redirect(this.ToString(), endResponse);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Updates the params based on the query.
    /// </summary>
    private void UpdateQueryParams()
    {
      string query = base.Query;

      if (_queryParams == null)
        _queryParams = new NameValueCollection();
      else
        _queryParams.Clear();

      if (string.IsNullOrEmpty(query))
        return;

      // Start after the '?'.
      query = query.Substring(1); 

      string[] pairs = query.Split(new char[] { '&' });
      foreach (string s in pairs)
      {
        string[] pair = s.Split(new char[] { '=' });
        string key = HttpContext.Current.Server.UrlDecode(pair[0]);
        string value = HttpContext.Current.Server.UrlDecode((pair.Length > 1) ? pair[1] : string.Empty);
        _queryParams.Add(key, value);
      }
    }

    /// <summary>
    /// Updates the Query string based on the parameters.
    /// </summary>
    private void UpdateQueryString()
    {
      int count = _queryParams.Count;

      if (count == 0)
      {
        base.Query = string.Empty;
      }
      else
      {
        StringBuilder query = new StringBuilder();
        for (int keyIndex = 0; keyIndex < count; keyIndex++)
        {
          if (keyIndex > 0)
            query.Append('&');
          string key = HttpContext.Current.Server.UrlEncode(_queryParams.GetKey(keyIndex));
          query.Append(key);

          // There are 0..n values per key possible.
          string[] values = _queryParams.GetValues(keyIndex);
          int currValueIndex = 0;
          foreach (string value in values)
          {
            query.Append('=');
            query.Append(HttpContext.Current.Server.UrlEncode(value));
            if (++currValueIndex < values.Length)
            {
              query.Append('&');
              query.Append(key);
            }
          }
        }
        base.Query = query.ToString();
      }
    }
    #endregion
  }
}
