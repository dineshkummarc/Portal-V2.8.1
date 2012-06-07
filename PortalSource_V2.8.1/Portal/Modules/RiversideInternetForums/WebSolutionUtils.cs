using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace RiversideInternet.WebSolution
{
	public class WebSolutionUtils
	{
		private static Hashtable CreateHashtableFromQueryString(Page page)
		{
			Hashtable ht = new Hashtable();

			foreach (string query in page.Request.QueryString)
				ht.Add(query, page.Request.QueryString[query]);

			return ht;
		}

		private static string CreateQueryString(Hashtable current, Hashtable add, Hashtable remove)
		{
			IDictionaryEnumerator myEnumerator = add.GetEnumerator();
			while (myEnumerator.MoveNext())
			{
				if (current.ContainsKey(myEnumerator.Key))
					current.Remove(myEnumerator.Key);
				current.Add(myEnumerator.Key, myEnumerator.Value);
			}

			myEnumerator = remove.GetEnumerator();
			while (myEnumerator.MoveNext())
			{
				string removeKey = (string)myEnumerator.Key;

				if (current.ContainsKey(removeKey))
				{
					string removeValue = (string)myEnumerator.Value;

					if (((string)current[removeKey] == removeValue) || removeValue == string.Empty)
						current.Remove(removeKey);
				}
			}

			int count = 0;
			StringBuilder sb = new StringBuilder();
			myEnumerator = current.GetEnumerator();
			while (myEnumerator.MoveNext())
			{
				if (count == 0)
					sb.Append("?");
				else
					sb.Append("&");
				sb.Append(myEnumerator.Key);
				sb.Append("=");
				sb.Append(myEnumerator.Value);
				count++;
			}

			return sb.ToString();
		}

		private static Hashtable CreateHashtableFromQueryString(string query)
		{
			Hashtable ht = new Hashtable();

			int startIndex = 0;
			while (startIndex >= 0)
			{
				int oldStartIndex = startIndex;
				int equalIndex = query.IndexOf("=", startIndex);
				startIndex = query.IndexOf("&", startIndex);
				if (startIndex >= 0)
					startIndex++;

				if (equalIndex >= 0)
				{
					int lengthValue = 0;
					if (startIndex >= 0)
						lengthValue = startIndex - equalIndex - 2;
					else
						lengthValue = query.Length - equalIndex - 1;
					string key = query.Substring(oldStartIndex, equalIndex - oldStartIndex);
					string val = query.Substring(equalIndex + 1, lengthValue);

					ht.Add(key, val);
				}
			}

			return ht;
		}

		public static string GetURL(string baseURL, Page page, string add, string remove)
		{
			if (remove != string.Empty)
				remove += "&";
			remove += "DocumentID=";

			Hashtable currentQueries = CreateHashtableFromQueryString(page);
			Hashtable addQueries	 = CreateHashtableFromQueryString(add);
			Hashtable removeQueries	 = CreateHashtableFromQueryString(remove);

			string newQueryString = CreateQueryString(currentQueries, addQueries, removeQueries);

			return baseURL + newQueryString;
		}

		public static string GetCaseInsensitiveSearch(string search)
		{
			string result = string.Empty;
	
			for (int index = 0; index < search.Length; index++)
			{
				char character = search[index];
				char characterLower = char.ToLower(character);
				char characterUpper = char.ToUpper(character);

				if (characterUpper == characterLower)
					result = result + character;
				else 
					result = result + "[" + characterLower + characterUpper + "]";
			}
			return result;
		}

		// See http://www.aspalliance.com/bbilbro/viewarticle.aspx?paged_article_id=4
		public static string ReplaceCaseInsensitive(string text, string oldValue, string newValue)
		{
			oldValue = GetCaseInsensitiveSearch(oldValue);

			return Regex.Replace(text, oldValue, newValue);
		}
	}
}
