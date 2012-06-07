using System;
using System.Collections;
using System.Web.UI;
using System.Web;
using System.Web.Caching;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Sloppycode.net;

namespace Portal.Modules.Newsticker
{
	/// <summary>
	/// Zusammenfassung für RssFeedItem.
	/// </summary>
	public class RssFeedItem
	{
		private string m_szTitle;
    private string m_szUrl;
    private RssFeed m_Feed;
    private int m_nMaxItemCount;

		public RssFeedItem(string szTitle, string szUrl, RssFeed Feed, int nMaxCount)
		{
      m_szTitle = szTitle;
      m_szUrl = szUrl;
			m_Feed = Feed;
			m_nMaxItemCount = nMaxCount;
		}

   public RssFeedItem(string szTitle, string szUrl, int nMaxCount, bool bOnlyCache)
     : this(szTitle, szUrl, null, nMaxCount)
    {
      // Als Key für den Cache verwenden wir die URL.
      string szKey = szUrl;

      // Das CacheItem wird aus dem Cache geholt. Ist nichts vorhanden, wird null zurückgegeben.
      object objCached = HttpContext.Current.Cache.Get(szKey);

      // Falls ein Cache-Objekt gefunden wurde wird dieses übernommen.
      if (null != objCached)
        m_Feed = (RssFeed)objCached;

      // Nichts im Cache: Feed neu holen, wenn dies erlaubt ist.
      if ((null == m_Feed) && !bOnlyCache)
      {
        // Laden des Feeds als RSS.
        m_Feed = RssReader.GetFeed(szUrl, false);
      
        // Ist das Feed-Onjekt immer noch leer, versuchen wir den Feed
        // als RDF-Feed zu laden..
        if (m_Feed.Items.Count == 0)
          m_Feed = RssReader.GetFeed(szUrl, true);
      
        // Feed-Objekt im Cache ablegen.
        HttpContext.Current.Cache.Insert(szKey, m_Feed, null, DateTime.Now.AddHours(1.0), Cache.NoSlidingExpiration);
      }
    }

		public string Title
		{
			get { return m_szTitle; }
		}

    public string Url
    {
      get { return m_szUrl; }
    }

		public RssItems ArticleList
		{
			get { return m_Feed.Items; }
		}

    public int MaxNofItems
    {
      get { return m_nMaxItemCount; }
    }

    public bool DataExist
    {
      get {return m_Feed != null; }
    }

    public RssFeed Feed
    {
      get { return m_Feed; }
    }


    /// <summary>
    /// Hängt die Representation dieses Newsfeeds der übergebenen Control-Collection an.
    /// </summary>
    /// <param name="CtrlCollection"></param>
    public void AddFeedRepresentation(System.Web.UI.ControlCollection CtrlCollection)
    {
      if(!DataExist)
        throw new Exception();

      // Titel einfügen.
      Label TitleLbl = new Label();
      TitleLbl.Text = Title;
      TitleLbl.CssClass = "NewsTitle";
      CtrlCollection.Add(TitleLbl);
      CtrlCollection.Add(new LiteralControl("<br>"));

      // Limitieren der max. Anzahl der Artikel.
      int nCount = m_Feed.Items.Count;
      if (nCount > MaxNofItems)
        nCount = MaxNofItems;

      // Schlagzeilen einfügen.
      for(int nzIndex = 0; nzIndex < nCount; nzIndex++)
      {
        HyperLink HeadLineLink = new HyperLink();
        HeadLineLink.Text = ArticleList[nzIndex].Title;
        HeadLineLink.NavigateUrl = ArticleList[nzIndex].Link;
        HeadLineLink.ToolTip = ArticleList[nzIndex].Description;
        HeadLineLink.Target = "_blank";
        HeadLineLink.CssClass = "NewsItem";

        // Die Daten an das NewsData-Objekt anhängen.
        CtrlCollection.Add(HeadLineLink);
        CtrlCollection.Add(new LiteralControl("<br>"));
      }
    }  
	}
}
