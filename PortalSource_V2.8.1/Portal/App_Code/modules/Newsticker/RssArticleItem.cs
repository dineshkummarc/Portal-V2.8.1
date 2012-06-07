using System;

namespace Portal.Modules.Newsticker
{
	/// <summary>
	/// Zusammenfassung für RssArticleItem.
	/// </summary>
	public class RssArticleItem
	{
		string m_szTitle;
		string m_szUrl;
		string m_szDescription;

		public RssArticleItem(string szTitle, string szUrl, string szDescription)
		{
			m_szTitle = szTitle;
			m_szUrl = szUrl;
			m_szDescription = szDescription;
		}

		public string Title
		{
			get { return m_szTitle;	}
		}

		public string Url
		{
			get { return m_szUrl;	}
		}

		public string Description
		{
			get	{ return m_szDescription;	}
		}
	}
}
