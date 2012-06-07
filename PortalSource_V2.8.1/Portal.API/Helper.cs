using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Portal.API
{
    /// <summary>
    /// Common Helper Class.
    /// </summary>
    public sealed class Helper
    {
        /// <summary>
        /// Helper-Class must not be instantiated, so set the constructor to private.
        /// </summary>
        private Helper() { }

        /// <summary>
        /// Copies a .install file in the given virtual directory.
        /// </summary>
        /// <param name="vFileName">Virtual path to the file.</param>
        /// <returns>true if the file was copied.</returns>
        public static bool InstallFile(string vFileName)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath(vFileName);
                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.Copy(path + ".install", path);
                    return true;
                }
            }
            catch (NotSupportedException) { }
            catch (ArgumentNullException) { }
            catch (ArgumentException) { }
            catch (PathTooLongException) { }
            catch (FileNotFoundException) { }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
            catch (IOException) { }

            return false;
        }

        /// <summary>
        /// Checks the given string for an URL and makes the URL browsable.
        /// </summary>
        /// <param name="emailString"></param>
        /// <returns></returns>
        public static string ActivateEmailAddress(string email)
        {
            string buf = email;
            string patternEmail = @"[a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+";
            Regex re = new Regex(patternEmail);
            if (re.IsMatch(buf))
                buf = re.Replace(buf, new MatchEvaluator(MailToMatchEvaluator));
            return buf;
        }

        /// <summary>
        /// Checks the given string for a valid Filename for Windows Systems.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CheckValidFileName(string fileName)
        {
          string patternFile = @"^[^\\\./:\*\?\""<>\|]{1}[^\\/:\*\?\""<>\|]{0,254}$";
          Regex re = new Regex(patternFile);
          return re.IsMatch(fileName);
        }

        /// <summary>
        /// Creates a valid filename or a Directoryname for a given string. If the string is empty, it will create 
        /// a random filename.
        /// </summary>
        /// <param name="title">The string that will be used to create the filename</param>
        /// <param name="extension">The filename extension</param>
        /// <returns>Valid filename</returns>
        public static string CreateValidFileName(string title, string extension)
        {
          string validFileName = title.Trim(); 

          foreach (char invalChar in Path.GetInvalidFileNameChars())
            validFileName = validFileName.Replace(invalChar.ToString(), "");
          foreach (char invalChar in Path.GetInvalidPathChars())
            validFileName = validFileName.Replace(invalChar.ToString(), "");

          if (validFileName.Length > 260) 
          validFileName = validFileName.Remove(260);

          if (String.IsNullOrEmpty(validFileName))
            validFileName = Path.GetRandomFileName();

          if (!string.IsNullOrEmpty(extension))
            validFileName = validFileName + "." + extension;

          return validFileName;
        } 


        /// <summary>
        /// Checks the given string for an email address and replaces it with a "mailto".
        /// </summary>
        /// <param name="siteString"></param>
        /// <returns></returns>
        public static string ActivateWebSiteUrl(string site)
        {
            string buf = site;
            string patternSite = @"(((http|https|ftp)\://\w+)|(www))+\.[\-_\w]+\.\w+[/\-_\w+]*[.\-_?=&;#\w+]*";
            Regex re = new Regex(patternSite);
            if (re.IsMatch(buf))
                buf = re.Replace(buf, new MatchEvaluator(WebSiteMatchEvaluator));
            return buf;
        }

        private static string MailToMatchEvaluator(Match m)
        {
            StringBuilder sb = new StringBuilder("<a href='mailto:");
            sb.Append(m.Value);
            sb.Append("'>");
            sb.Append(m.Value);
            sb.Append("</a>");
            return sb.ToString();
        }

        private static string WebSiteMatchEvaluator(Match m)
        {
            UriBuilder ub = new UriBuilder(m.Value);
            StringBuilder sb = new StringBuilder("<a href='");
            sb.Append(ub.ToString());
            sb.Append("'>");
            sb.Append(m.Value);
            sb.Append("</a>");
            return sb.ToString();
        }
    }
}
