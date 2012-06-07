using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Web.UI;

namespace Portal.API
{
	/// <summary>
	/// Summary description for Language.
	/// </summary>
	[XmlRoot("language"), Serializable]
	public sealed class Language
	{
		private static XmlSerializer xmlLanguage = new XmlSerializer(typeof(Language));		

		private Language()
		{
		}

		public static string GetText(string reference)
		{
			Language l = Load(System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
			string w = (string)l.wordsTbl[reference];
			return w==null?"":w;
		}

		public static string GetText(Module module, string reference)
		{
			if(module == null)
			{
				return GetText(reference);
			}
			else
			{
				Language l = Load(module, System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
				string w = (string)l.wordsTbl[reference];
				return w==null?"":w;
			}
		}

		private static Language Load(string language)
		{
			string CacheKey = "Language_" + language;
			// Lookup in Cache
			Language l = (Language)HttpContext.Current.Cache[CacheKey];
			if(l != null) return l;

			// Load Language
			XmlTextReader xmlReader = new XmlTextReader(Config.GetLanguagePhysicalPath(language));
			try
			{
				l = (Language)xmlLanguage.Deserialize(xmlReader);
				if(l == null) throw new PortalException("Unable to load Language " + language);

				UpdateLanguageProperties(l);

				// Add to Cache
				HttpContext.Current.Cache.Insert(CacheKey, l, 
					new System.Web.Caching.CacheDependency(Config.GetLanguagePhysicalPath(language)));
			}
			finally
			{
				xmlReader.Close();
			}

			return l;
		}

		private static Language Load(Module module, string language)
		{
			string CacheKey = "Language_" + module.ModuleType + "_" + language;
			// Lookup in Cache
			Language l = (Language)HttpContext.Current.Cache[CacheKey];
			if(l != null) return l;

			// Load Portaldefinition
			XmlTextReader xmlReader = new XmlTextReader(Config.GetModuleLanguagePhysicalPath(module.ModuleType, language));
			try
			{
				l = (Language)xmlLanguage.Deserialize(xmlReader);
				if(l == null) throw new PortalException("Unable to load Language " + language);

				UpdateLanguageProperties(l);

				// Add to Cache
				HttpContext.Current.Cache.Insert(CacheKey, l, 
					new System.Web.Caching.CacheDependency(Config.GetModuleLanguagePhysicalPath(module.ModuleType, language)));
			}
			finally
			{
				xmlReader.Close();
			}

			return l;
		}

		private static void UpdateLanguageProperties(Language l)
		{
			if(l.words == null)
				throw new PortalException("No words found in current Language");

			foreach(Word w in l.words)
			{
				l.wordsTbl[w.reference] = w.val;
			}
		}

		[XmlElement("word")]
		public Word[] words = new Word[] {};

		[XmlIgnore]
		private Hashtable wordsTbl = new Hashtable();
	}

    [Serializable]
    public class Word
    {
        [XmlAttribute("ref")]
        public string reference = "";

        [XmlText]
        public string val = "";
    }
}
