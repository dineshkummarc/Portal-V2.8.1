using System;
using System.Collections;
using System.Text;

namespace RiversideInternet.WebSolution
{
	public class ForumText
	{
		private string _text;

		private class Helper
		{
			public Helper(bool open, int index, int level)
			{
				_open  = open;
				_index = index;
				_level = level;
			}

			private bool _open;
			private bool _valid = false;
			private int  _index;
			private int	 _level;

			public bool Open
			{
				get
				{
					return _open;
				}
			}

			public int Level
			{
				get
				{
					return _level;
				}
			}

			public int Index
			{
				get
				{
					return _index;
				}
			}

			public bool Valid
			{
				get
				{
					return _valid;
				}
				set
				{
					_valid = value;
				}
			}
		}

		public ForumText(string text)
		{
			_text = text;
		}

		public void FormatMultiLine()
		{
			_text = _text.Replace("\r\n", "\n");
			_text = _text.Replace("\r", "\n");
			_text = _text.Replace("\n", "<br>");
		}

		public void FormatDisableHtml()
		{
			// From Code Project forums...
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<script",	"&lt;script");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</script",	"&lt;/script");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<input",	"&lt;input");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</input",	"&lt;/input");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<object",	"&lt;object");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</object",	"&lt;/object");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<applet",	"&lt;applet");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</applet",	"&lt;/applet");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<form",		"&lt;form");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</form",	"&lt;/form");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<table",	"&lt;table");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</table",	"&lt;/table");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<tr",		"&lt;tr");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</tr",		"&lt;/tr");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<td",		"&lt;td");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</td",		"&lt;/td");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<select",	"&lt;select");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</select",	"&lt;/select");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<option",	"&lt;option");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</option",	"&lt;/option");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "<iframe",	"&lt;iframe");
			_text = WebSolutionUtils.ReplaceCaseInsensitive(_text, "</iframe",	"&lt;/iframe");
		}

		private void FormatSmiley(string images)
		{
			_text = _text.Replace(":)", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_smile.gif"));
			_text = _text.Replace(";)", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_wink.gif"));
			_text = _text.Replace(";P", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_tongue.gif"));
			_text = _text.Replace(":-D", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_biggrin.gif"));
			_text = _text.Replace(":((", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_cry.gif"));
			_text = _text.Replace(":(", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_frown.gif"));
			_text = _text.Replace(":-O", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_redface.gif"));
			_text = _text.Replace(":rolleyes:", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_rolleyes.gif"));
			_text = _text.Replace(":laugh:", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_laugh.gif"));
			_text = _text.Replace(":mad:", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_mad.gif"));
			_text = _text.Replace(":confused:", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_confused.gif"));
			_text = _text.Replace(":|", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_line.gif"));
			_text = _text.Replace("X|", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_dead.gif"));
			_text = _text.Replace(":suss:", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_suss.gif"));
			_text = _text.Replace(":cool:", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_cool.gif"));
			_text = _text.Replace(":eek:", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_eek.gif"));
			_text = _text.Replace(":bunny:", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_bunny.gif"));
			_text = _text.Replace(":gunfire:", string.Format("<img src=\"{0}\" align=absmiddle border=0 \\>", images + "smiley_gunfire.gif"));
		}

		private void ProcessOpenClose(ArrayList array)
		{
			int  index = 0;
			int  level = 0;
			int  indexQuoteOpen = 0;
			int  indexQuoteClose = 0;
			bool previousQuoteOpen = false;

			while (indexQuoteOpen >= 0 || indexQuoteClose >= 0)
			{
				indexQuoteOpen = _text.IndexOf("[QUOTE]", index);
				indexQuoteClose = _text.IndexOf("[/QUOTE]", index);
			
				if (indexQuoteOpen >= 0 || indexQuoteClose >= 0)
				{
					if (indexQuoteOpen >= 0 && (indexQuoteOpen < indexQuoteClose || indexQuoteClose == -1))
					{
						if (index > 0 && previousQuoteOpen)
							level++;
						array.Add(new Helper(true, indexQuoteOpen, level));
						index = indexQuoteOpen + 7;
						previousQuoteOpen = true;
					}
					else if (indexQuoteClose >= 0 && (indexQuoteClose < indexQuoteOpen || indexQuoteOpen == -1))
					{
						if (index > 0 && !previousQuoteOpen)
							level--;
						array.Add(new Helper(false, indexQuoteClose, level));
						index = indexQuoteClose + 8;
						previousQuoteOpen = false;
					}
				}
			}
		}

		private void ValidateFindClose(ArrayList array, int index)
		{
			Helper helperOpen = (Helper)array[index];

			index++;
			bool foundClose = false;

			while (!foundClose && index < array.Count)
			{
				Helper helperClose = (Helper)array[index];

				if (helperClose.Level == helperOpen.Level && !helperClose.Valid)
				{
					helperOpen.Valid = true;
					helperClose.Valid = true;
					foundClose = true;
				}
				index++;
			}
		}

		private void ValidateOpenClose(ArrayList array)
		{
			for (int index = 0; index < array.Count; index++)
			{
				Helper helper = (Helper)array[index];
				if (helper.Open)
					ValidateFindClose(array, index);
			}
		}

		private void FormatQuoteOpen(StringBuilder sb)
		{
			sb.Append("<TABLE border=\"0\" cellpadding=\"10\" cellspacing=\"0\" width=\"100%\">");
			sb.Append("<TR>");
			sb.Append("<TD>");
			sb.Append("<TABLE border=\"0\" cellpadding=\"1\" cellspacing=\"1\" width=\"100%\">");
			sb.Append("<TR class=\"Normal\"><TD><B>Quote:</B></TD></TR></TABLE>");
			sb.Append("<TABLE border=\"0\" cellpadding=\"1\" cellspacing=\"1\" width=\"100%\" class=\"WebSolutionQuote\">");
			sb.Append("<TR class=\"Normal\">");
			sb.Append("<TD>");
		}

		private void FormatQuoteClose(StringBuilder sb)
		{
			sb.Append("</TD>");
			sb.Append("</TR>");
			sb.Append("</TABLE>");
			sb.Append("</TD>");
			sb.Append("</TR>");
			sb.Append("</TABLE>");
		}

		private void FormatStripQuotes()
		{
			ArrayList array = new ArrayList();
			ProcessOpenClose(array);
			ValidateOpenClose(array);

			StringBuilder sb = new StringBuilder();
			
			int startIndex = 0;
			int index = 0;

			while (index < array.Count)
			{
				Helper helper = (Helper)array[index];

				if (helper.Valid)
				{
					// helper.Open will be true, meaning there must be a corresponding
					// closing helper item at the same level as helper.Open
					bool found = false;
					int level = helper.Level;
					index++;
					Helper helperClose = null;
					while (index < array.Count && !found)
					{
						helperClose = (Helper)array[index];
						if (helperClose.Valid && !helperClose.Open && helperClose.Level == level)
							found = true;
						index++;
					}
					if (found)
					{
						int length = helper.Index - startIndex;
						if (length > 0)
							sb.Append(_text.Substring(startIndex, length));
						startIndex = helperClose.Index + 8;
					}
				}
				else
				{
					index++;
				}
			}

			if ((_text.Length - startIndex) > 0)
				sb.Append(_text.Substring(startIndex, _text.Length - startIndex));

			_text = sb.ToString();
		}

		private void FormatQuote()
		{
			ArrayList array = new ArrayList();
			ProcessOpenClose(array);
			ValidateOpenClose(array);

			StringBuilder sb = new StringBuilder();
			
			int startIndex = 0;
			for (int index = 0; index < array.Count; index++)
			{
				Helper helper = (Helper)array[index];

				if (helper.Valid)
				{
					int length = helper.Index - startIndex;
					if (length > 0)
						sb.Append(_text.Substring(startIndex, length));
					if (helper.Open)
					{
						FormatQuoteOpen(sb);
						startIndex = helper.Index + 7;
					}
					else
					{
						FormatQuoteClose(sb);
						startIndex = helper.Index + 8;
					}
				}
			}

			if ((_text.Length - startIndex) > 0)
				sb.Append(_text.Substring(startIndex, _text.Length - startIndex));

			_text = sb.ToString();
		}

		public string ProcessSingleLine(string images)
		{
			FormatSmiley(images);
			FormatDisableHtml();

			return _text;
		}

		public string Process(string images)
		{
			FormatSmiley(images);
			FormatMultiLine();
			FormatDisableHtml();
			FormatQuote();

			return _text;
		}

		public string ProcessQuoteBody(string parentPoster)
		{
			FormatStripQuotes();

			StringBuilder sb = new StringBuilder();
			sb.Append("[QUOTE]<B><I>Originally posted by ");
			sb.Append(parentPoster);
			sb.Append("</I></B>");
			sb.Append("<BR><BR>");
			sb.Append(_text);
			sb.Append("[/QUOTE]");

			_text = sb.ToString();

			return _text;
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}
	}
}
