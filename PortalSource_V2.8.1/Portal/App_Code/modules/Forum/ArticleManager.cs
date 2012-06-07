using System;
using System.Data;
using System.IO;
using System.Web;
using System.Globalization;
using System.Web.Caching;

namespace Portal.Modules.Forum
{
    /// <summary>
    /// Zusammenfassung für ArticleManager.
    /// </summary>
    public class ArticleManager
    {
        /// <summary>
        /// Speichert eine Instanz des Konfigurationsobjektes.
        /// </summary>
        private ConfigAgent configAgent;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="config">Aktuelle Instanz des Konfigurationsobjektes.</param>
        public ArticleManager(ConfigAgent configAgent)
        {
            this.configAgent = configAgent;

            // Prüfen, ob das Directory für diese Forum-Instanz existiert.
            // Ist es noch nicht vorhanden, so wird es erzeugt.
            string szDirectory = GetDataPath("");
            szDirectory = szDirectory.Substring(0, szDirectory.Length - 1);
            if (!Directory.Exists(szDirectory))
                Directory.CreateDirectory(szDirectory);
        }

        /// <summary>
        /// Führt die Suche durch und liefert deren Ergebnisse als DataSet zurück.
        /// </summary>
        /// <returns></returns>
        public DataTable SearchResults
        {
            get
            {
                DataTable data = null;
                if ((null != configAgent.SearchResults) && (configAgent.LastSearchText == configAgent.SearchText))
                {
                    data = configAgent.SearchResults;
                }
                else
                {
                    // Erzeugen der Table.
                    data = new DataTable();
                    data.Columns.Add("ThreadFile", typeof(string));
                    data.Columns.Add("Title", typeof(string));
                    data.Columns.Add("dateTime", typeof(DateTime));
                    data.Columns.Add("Author", typeof(string));
                    data.Columns.Add("UserId", typeof(string));
                    data.Columns.Add("Id", typeof(int));

                    // Holen des Suchtextes.
                    string szSearchText = configAgent.SearchText.ToLower(CultureInfo.CurrentCulture);

                    // Iterieren über alle Threads.
                    ForumData fData = ThreadRootData;
                    foreach (ForumData.ForumRow fRow in fData.Forum.Rows)
                    {
                        ThreadData tData = GetThreadData(fRow.ThreadFile);
                        foreach (ThreadData.ThreadRow tRow in tData.Thread.Rows)
                        {
                            if ((tRow.Text.ToLower(CultureInfo.CurrentCulture).IndexOf(szSearchText) != -1) ||
                              (tRow.Title.ToLower(CultureInfo.CurrentCulture).IndexOf(szSearchText) != -1))
                            {
                                DataRow row = data.NewRow();
                                row["ThreadFile"] = fRow.ThreadFile;
                                row["Title"] = tRow.Title;
                                row["dateTime"] = tRow.DateTime;
                                row["Author"] = tRow["Author"];
                                row["UserId"] = tRow["UserId"];
                                row["Id"] = tRow.Id;
                                data.Rows.Add(row);
                            }
                        }
                    }
                    configAgent.LastSearchText = configAgent.SearchText;
                    configAgent.SearchResults = data;
                }
                return data;
            }
        }

        /// <summary>
        /// Setzt die in der Session gespeicherte Suche zurück.
        /// </summary>
        public void ResetSearch()
        {
            configAgent.LastSearchText = null;
            configAgent.SearchResults = null;
        }

        /// <summary>
        /// Speichert einen neuen Artikel innerhalb eines bestehenden Threads.
        /// </summary>
        /// <param name="threadFile"></param>
        /// <param name="parentMessage"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public void AddThreadArticle(string threadFile,
                                     int parentMessage,
                                     string name,
                                     string email,
                                     string title,
                                     string text)
        {
            object objLock = new object();
            lock (objLock)
            {
                // Existiert das Thread-File nicht, so ist etwas schief gelaufen und 
                // die Daten dürfen hier nicht gespeichert werden.
                if (File.Exists(GetDataPath(threadFile)))
                {
                    // Merken des aktuellen Zeitpunktes.
                    DateTime CurrentDate = DateTime.Now;

                    // Erzeugen des Thread-Datenobjektes.
                    ThreadData CurrentThreadData = GetThreadData(threadFile);

                    // Speichern des neuen Artikels.
                    ThreadData.ThreadRow tRow = CurrentThreadData.Thread.NewThreadRow();
                    tRow.Title = HttpUtility.HtmlEncode(title);
                    tRow.Text = HttpUtility.HtmlEncode(text);
                    tRow.Author = HttpUtility.HtmlEncode(name);
                    tRow.Email = HttpUtility.HtmlEncode(email);
                    tRow.DateTime = CurrentDate;
                    tRow.Parent = parentMessage;
                    CurrentThreadData.Thread.AddThreadRow(tRow);
                    WriteThreadData(CurrentThreadData, threadFile);

                    // Laden der Forumsdaten.
                    ForumData CurrentForumData = ThreadRootData;

                    // Holen der entsprechenden Zeile.
                    ForumData.ForumRow fRow = (ForumData.ForumRow)CurrentForumData.Forum.Select("ThreadFile = '" + threadFile + "'", "DateTime DESC")[0];
                    fRow.DateTime = CurrentDate;
                    fRow.CommentCount = CurrentThreadData.Thread.Rows.Count - 1;
                    fRow.LastPosterName = HttpUtility.HtmlEncode(name);
                    ThreadRootData = CurrentForumData;
                }
            }
        }

        public void AddThreadArticle(string threadFile,
                                     int parentMessage,
                                     string userId,
                                     string title,
                                     string text)
        {
            object objLock = new object();
            lock (objLock)
            {
                // Existiert das Thread-File nicht, so ist etwas schief gelaufen und 
                // die Daten dürfen hier nicht gespeichert werden.
                if (File.Exists(GetDataPath(threadFile)))
                {
                    // Merken des aktuellen Zeitpunktes.
                    DateTime CurrentDate = DateTime.Now;

                    // Erzeugen des Thread-Datenobjektes.
                    ThreadData CurrentThreadData = GetThreadData(threadFile);

                    // Speichern des neuen Artikels.
                    ThreadData.ThreadRow tRow = CurrentThreadData.Thread.NewThreadRow();
                    tRow.Title = HttpUtility.HtmlEncode(title);
                    tRow.Text = HttpUtility.HtmlEncode(text);
                    tRow.UserId = new Guid(HttpUtility.HtmlEncode(userId));
                    tRow.DateTime = CurrentDate;
                    tRow.Parent = parentMessage;
                    CurrentThreadData.Thread.AddThreadRow(tRow);
                    WriteThreadData(CurrentThreadData, threadFile);

                    // Laden der Forumsdaten.
                    ForumData CurrentForumData = ThreadRootData;

                    // Holen der entsprechenden Zeile.
                    ForumData.ForumRow fRow = (ForumData.ForumRow)CurrentForumData.Forum.Select("ThreadFile = '" + threadFile + "'", "DateTime DESC")[0];
                    fRow.DateTime = CurrentDate;
                    fRow.CommentCount = CurrentThreadData.Thread.Rows.Count - 1;
                    fRow.LastPosterId = new Guid(HttpUtility.HtmlEncode(userId));
                    ThreadRootData = CurrentForumData;
                }
            }
        }

        /// <summary>
        /// Startet einen neuen Forum-Thread und speichert darin den neuen Artikel.
        /// </summary>
        /// <param name="threadFile"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public void AddForumArticle(string threadFile,
                                    string name,
                                    string email,
                                    string title,
                                    string text)
        {
            object objLock = new object();
            lock (objLock)
            {
                // Existiert das Thread-File bereits, so ist etwas schief gelaufen und 
                // die Daten dürfen hier nicht gespeichert werden.
                if (!File.Exists(GetDataPath(threadFile)))
                {
                    // Laden der Forumsdaten.
                    ForumData CurrentForumData = ThreadRootData;

                    // Merken des aktuellen Zeitpunktes.
                    DateTime CurrentDate = DateTime.Now;

                    // Speichern des neuen Artikels.
                    ForumData.ForumRow fRow = CurrentForumData.Forum.NewForumRow();
                    fRow.ThreadFile = HttpUtility.HtmlEncode(threadFile);
                    fRow.DateTime = CurrentDate;
                    fRow.Author = HttpUtility.HtmlEncode(name);
                    fRow.Email = HttpUtility.HtmlEncode(email);
                    fRow.Title = HttpUtility.HtmlEncode(title);
                    fRow.Text = HttpUtility.HtmlEncode(text);
                    fRow.CommentCount = 0;
                    CurrentForumData.Forum.AddForumRow(fRow);
                    ThreadRootData = CurrentForumData;

                    // Erzeugen des Thread-Datenobjektes.
                    ThreadData CurrentThreadData = GetThreadData(threadFile);

                    // Speichern des neuen Artikels.
                    ThreadData.ThreadRow tRow = CurrentThreadData.Thread.NewThreadRow();
                    tRow.Title = HttpUtility.HtmlEncode(title);
                    tRow.Text = HttpUtility.HtmlEncode(text);
                    tRow.Author = HttpUtility.HtmlEncode(name);
                    tRow.Email = HttpUtility.HtmlEncode(email);
                    tRow.DateTime = CurrentDate;
                    tRow.Id = 0;
                    CurrentThreadData.Thread.AddThreadRow(tRow);
                    WriteThreadData(CurrentThreadData, threadFile);
                }
            }
        }


        public void AddForumArticle(string threadFile,
                                    string userId,
                                    string title,
                                    string text)
        {
            object objLock = new object();
            lock (objLock)
            {
                // Existiert das Thread-File bereits, so ist etwas schief gelaufen und 
                // die Daten dürfen hier nicht gespeichert werden.
                if (!File.Exists(GetDataPath(threadFile)))
                {
                    // Laden der Forumsdaten.
                    ForumData CurrentForumData = ThreadRootData;

                    // Merken des aktuellen Zeitpunktes.
                    DateTime CurrentDate = DateTime.Now;

                    // Speichern des neuen Artikels.
                    ForumData.ForumRow fRow = CurrentForumData.Forum.NewForumRow();
                    fRow.ThreadFile = HttpUtility.HtmlEncode(threadFile);
                    fRow.DateTime = CurrentDate;
                    fRow.UserId = new Guid(HttpUtility.HtmlEncode(userId));
                    fRow.Title = HttpUtility.HtmlEncode(title);
                    fRow.Text = HttpUtility.HtmlEncode(text);
                    fRow.CommentCount = 0;
                    CurrentForumData.Forum.AddForumRow(fRow);
                    ThreadRootData = CurrentForumData;

                    // Erzeugen des Thread-Datenobjektes.
                    ThreadData CurrentThreadData = GetThreadData(threadFile);

                    // Speichern des neuen Artikels.
                    ThreadData.ThreadRow tRow = CurrentThreadData.Thread.NewThreadRow();
                    tRow.Title = HttpUtility.HtmlEncode(title);
                    tRow.Text = HttpUtility.HtmlEncode(text);
                    tRow.UserId = new Guid(HttpUtility.HtmlEncode(userId));
                    tRow.DateTime = CurrentDate;
                    tRow.Id = 0;
                    CurrentThreadData.Thread.AddThreadRow(tRow);
                    WriteThreadData(CurrentThreadData, threadFile);
                }
            }
        }

        /// <summary>
        /// Entfernt einen Beitrag samt allen untergeordneten Beiträge aus dem Forum.
        /// Ist articleId == 0, so handelt es sich um den Root-Node.
        /// </summary>
        /// <param name="articleId"></param>
        public void DeleteArticle(int articleId)
        {
            object objLock = new object();
            lock (objLock)
            {
                if (articleId == 0)
                {
                    // Es handelt sich um den Root-Node, also muss das File gelöscht werden und
                    // der Root-Node aus dem Übersichtsfile entfernt werden.

                    // Ermitteln des Threadfiles.
                    string threadFile = GetDataPath(configAgent.ThreadFile);

                    // Löschen des Threadfiles.
                    if (File.Exists(threadFile))
                        File.Delete(threadFile);

                    // Laden der Forumsdaten.
                    ForumData CurrentForumData = ThreadRootData;
                    foreach (ForumData.ForumRow row in CurrentForumData.Forum.Rows)
                    {
                        if (row.ThreadFile == configAgent.ThreadFile)
                        {
                            CurrentForumData.Forum.RemoveForumRow(row);
                            break;
                        }
                    }
                    ThreadRootData = CurrentForumData;
                }
                else
                {
                    ThreadData CurrentThreadData = GetThreadData(configAgent.ThreadFile);

                    ThreadData.ThreadRow row = null;
                    foreach (ThreadData.ThreadRow r in CurrentThreadData.Thread.Rows)
                    {
                        if (r.Id == articleId)
                        {
                            row = r;
                            break;
                        }
                    }

                    if (null != row)
                        CurrentThreadData.Thread.RemoveThreadRow(row);

                    WriteThreadData(CurrentThreadData, configAgent.ThreadFile);

                    // Laden der Forumsdaten.
                    ForumData CurrentForumData = ThreadRootData;
                    foreach (ForumData.ForumRow fRow in CurrentForumData.Forum.Rows)
                    {
                        if (fRow.ThreadFile == configAgent.ThreadFile)
                        {
                            fRow.CommentCount = CurrentThreadData.Thread.Rows.Count - 1;
                            break;
                        }
                    }
                    ThreadRootData = CurrentForumData;

                }
            }
        }

        /// <summary>
        /// Liest die aktuellen Forumsdaten ein oder speichert sie ab.
        /// </summary>
        /// <returns></returns>
        public ForumData ThreadRootData
        {
            get
            {
                ForumData data = null;

                // Die Forumsdaten werden aus dem Cache geholt. 
                // Sind sie dort nicht vorhanden, werden sie aus dem entsprechenden File geholt.
                object obj = HttpContext.Current.Cache.Get("ForumData:" + configAgent.Module.ModuleRef);
                if ((null != obj) && obj.GetType().Equals(typeof(ForumData)))
                {
                    data = (ForumData)obj;
                }
                else
                {
                    // Ermitteln des Forum-Files.
                    string forumFile = GetDataPath(configAgent.Module.ModuleRef + ".xml");
                    data = new ForumData();

                    // Existiert das File, wird es eingelesen.
                    if (File.Exists(forumFile))
                        data.ReadXml(forumFile);

                    // Die Forumsdaten werden im Cache abgelegt.
                    HttpContext.Current.Cache.Insert("ForumData:" + configAgent.Module.ModuleRef, data, null, Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0));
                }

                return data;
            }
            set
            {
                // Ermitteln des Forum-Files.
                string forumFile = GetDataPath(configAgent.Module.ModuleRef + ".xml");

                // Speichern der Daten.
                value.WriteXml(forumFile);

                // Die Forumsdaten werden im Cache abgelegt.
                HttpContext.Current.Cache.Insert("ForumData:" + configAgent.Module.ModuleRef, value, null, Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0));
            }
        }

        /// <summary>
        /// Liefert die aktuellen Threaddaten.
        /// </summary>
        /// <param name="threadFile"></param>
        /// <returns></returns>
        public ThreadData GetThreadData(string threadFile)
        {
            // Ermitteln des Threadfiles.
            threadFile = GetDataPath(threadFile);

            // Existiert das File, wird es eingelesen.
            ThreadData CurrentThreadData = new ThreadData();
            if (File.Exists(threadFile))
                CurrentThreadData.ReadXml(threadFile);

            return CurrentThreadData;
        }

        /// <summary>
        /// Speichert die Threaddaten.
        /// </summary>
        /// <param name="data"></param>
        public void WriteThreadData(ThreadData data, string threadFile)
        {
            data.WriteXml(GetDataPath(threadFile));
        }

        /// <summary>
        /// Gibt den vollständigen Pfad zum angegebenen File zurück.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetDataPath(string file)
        {
            return configAgent.Module.MapPath(configAgent.Module.ModuleDataVirtualPath + configAgent.Module.ModuleRef + "/" + file);
        }
    }
}
