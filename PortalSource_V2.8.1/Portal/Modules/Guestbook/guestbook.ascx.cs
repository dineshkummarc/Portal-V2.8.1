//===========================================================================
// Diese Datei wurde als Teil einer ASP.NET 2.0-Webprojektkonvertierung geändert.
// Der Klassenname wurde geändert, und die Klasse wurde so geändert, dass sie von der abstrakten 
//-Basisklasse in Datei "App_Code\Migrated\modules\guestbook\Stub_guestbook_ascx_cs.cs" erbt.
// Andere Klassen in der Webanwendung können während der Laufzeit die Code-Behind-Seite
//" mithilfe der abstrakten Basisklasse binden und darauf zugreifen.
// Die zugehörige Inhaltsseite "modules\guestbook\guestbook.ascx" wurde ebenfalls geändert und verweist auf den neuen Klassennamen.
// Weitere Informationen zu diesem Codemuster erhalten Sie unter http://go.microsoft.com/fwlink/?LinkId=46995. 
//===========================================================================
namespace Portal.Modules.Guestbook
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Xml;
    using System.Xml.Serialization;
    using Portal;
    using Portal.API;
    using System.Globalization;

    /// <summary>
    ///		Zusammenfassung für Guestbook.
    /// </summary>
    public partial class GuestbookControl : Guestbook
    {
        protected DataSet m_ConfigData;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Als Voreinstellung wird die Darstellung auf "Benutzermodus" gesetzt.
            SetVisualMode(false);

            // Ist ein Benutzer eingeloggt, wird sein Name als Default genommen.
            SetDefaultName();

            // Standardmässig wird die Fehler-Zeile ausgeblendet.
            m_ErrorTR.Visible = false;

            if (!IsPostBack)
            {
                // Schema exists - cannot be null!
                m_ConfigData = ReadConfig();

                // Falls keine Konfiguration existiert, erstellen wir eine Default-Konfiguration.
                if (m_ConfigData.Tables["Guestbook"].Rows.Count == 0)
                {
                    DataRow row = m_ConfigData.Tables["Guestbook"].NewRow();
                    row["UseEmail"] = true;
                    row["UseUrl"] = true;
                    row["SendNotification"] = false;
                    row["EmailFrom"] = "";
                    row["EmailTo"] = "";
                    row["EmailSubject"] = "";
                    row["EmailServer"] = "";
                    row["UseAuthentication"] = false;
                    row["EmailUserName"] = "";
                    row["EmailPassword"] = "";
                    m_ConfigData.Tables["Guestbook"].Rows.Add(row);
                }
                Session[ModuleRef + ":GuestbookConfigData"] = m_ConfigData;

                // Darstellen der Gästebuchdaten mit Hilfe des Repeaters.
                Bind();
            }
            else
            {
                m_ConfigData = (DataSet)Session[ModuleRef + ":GuestbookConfigData"];
            }
        }

        /// <summary>
        /// Verbindet den Repeater mit den Gästebuchdaten.
        /// </summary>
        private void Bind()
        {
            // Die Felder für die Eingabe von Email und URL werden nur bei Bedarf dargestellt.
            m_EmailTR.Visible = ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseEmail"]);
            m_UrlTR.Visible = ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseUrl"]);

            // Schema exists - cannot be null!
            GuestbookData data = ReadData();

            // Abfüllen der Tabelle.
            data.Guestbook.DefaultView.Sort = "Created DESC";
            DataGrid.DataSource = data.Guestbook.DefaultView;
            DataGrid.DataBind();
        }

        /// <summary>
        /// Liest das Guestbook-File.
        /// </summary>
        /// <returns>
        /// Null, falls das Guestbook-File nicht existiert oder kein Schema verfügbar ist.
        /// Existiert ein Schema, wird es gelesen und ein leeres Datenset zurückgegeben.
        /// </returns>
        override public GuestbookData ReadData()
        {
            GuestbookData data = new GuestbookData();
            try
            {
                if (System.IO.File.Exists(GuestbookConfigFile))
                {
                    data.ReadXml(GuestbookConfigFile);
                }
            }
            catch (Exception e)
            {
                // Do not throw exceptions
                Trace.Warn("Module", "Error loading Guestbook data as Dataset", e);
                return null;
            }

            return data;
        }

        /// <summary>
        /// Liefert den Pfad des Datenfiles des Gästebuches.
        /// </summary>
        override public string GuestbookConfigFile
        {
            get
            {
                return ModuleDataPhysicalPath + "/" + "Guestbook_" + ModuleRef + ".config";
            }
        }

        /// <summary>
        /// Liefert den Pfad des Schemafiles des Gästebuches.
        /// </summary>
        //		public string GuestbookDataSchemaFile
        //		{
        override public string GuestbookDataSchemaFile
        {
            get
            {
                return ModuleDataPhysicalPath + "/" + "GuestbookData.xsd";
            }
        }

        /// <summary>
        /// Entfernt alle Texte aus den Text-Feldern.
        /// </summary>
        private void Clear()
        {
            // Zurücksetzen der Eingabefelder.
            m_FromTB.Text = "";
            m_EmailTB.Text = "";
            m_UrlTB.Text = "";
            m_TitleTB.Text = "";
            m_MessageTB.Text = "";
            m_CommentTB.Text = "";

            // Ist ein Benutzer eingeloggt, wird sein Name als Default genommen.
            SetDefaultName();
        }

        /// <summary>
        /// Ist ein Benutzer eingeloggt, wird sein Name als Default genommen.
        /// </summary>
        private void SetDefaultName()
        {
            if (m_FromTB.Text.Length == 0)
            {
                // Ist der Benutzer eingeloggt, wird sein Name als Default vorgegeben.
                Users u = UserManagement.Users;
                Users.UserRow user = u.User.FindBylogin(Page.User.Identity.Name.ToLower(CultureInfo.CurrentCulture));
                if (user != null)
                {
                    string szName = "";
                    if (!user.IsfirstNameNull())
                        szName += user.SafeFirstName + " ";
                    if (!user.IssurNameNull())
                        szName += user.SafeSurName;
                    m_FromTB.Text = szName.Trim();
                    m_EmailTB.Text = user.email;
                }
            }
        }

        /// <summary>
        /// Legt fest, ob das Gästebuch im Edit-Modus dargestellt werden soll oder nicht.
        /// </summary>
        /// <param name="bEditMode"></param>
        private void SetVisualMode(bool bEditMode)
        {
            // Defaults setzen.
            m_AddBtn.Visible = !bEditMode;
            DataGrid.Visible = !bEditMode;
            m_CommentTR.Visible = bEditMode;
            m_EditButtonTR.Visible = bEditMode;
        }

        #region Vom Web Form-Designer generierter Code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: Dieser Aufruf ist für den ASP.NET Web Form-Designer erforderlich.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Erforderliche Methode für die Designerunterstützung
        ///		Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.DataGrid.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.OnItemCommand);
            this.DataGrid.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGrid_PageIndexChanged);

        }
        #endregion


        void OnItemCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                GuestbookData data = ReadData();
                foreach (GuestbookData.GuestbookRow row in data.Guestbook.Rows)
                {
                    if (row.Id == new Guid((string)e.CommandArgument))
                    {
                        row.Delete();
                        WriteData(data);
                        break;
                    }
                }

                // Darstellen der Gästebuchdaten mit Hilfe des Repeaters.
                Bind();
            }
            else if (e.CommandName == "Edit")
            {
                OnEdit(new Guid((string)e.CommandArgument));
            }
        }

        /// <summary>
        /// Fügt den Eintrag ins Gästebuch ein.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void OnAdd(Object sender, EventArgs args)
        {
            try
            {
                // Prüfen, ob gültige Daten vorliegen. Falls nicht, speichern wir den eintrag nicht ab.
                m_MessageTB.Text = m_MessageTB.Text.Trim();
                if (m_MessageTB.Text == "") return;
                m_FromTB.Text = m_FromTB.Text.Trim();
                if (m_FromTB.Text == "") return;

                // Speichern des Eintrages.
                GuestbookData data = ReadData();
                GuestbookData.GuestbookRow row = data.Guestbook.NewGuestbookRow();
                row.Created = DateTime.Now;
                row.Name = HttpUtility.HtmlEncode(m_FromTB.Text);
                row.Email = HttpUtility.HtmlEncode(m_EmailTB.Text);
                row.Url = HttpUtility.HtmlEncode(m_UrlTB.Text.Trim());
                if ((((string)row.Url).Length > 0) && !((string)row.Url).StartsWith("http://"))
                    row.Url = "http://" + (string)row.Url;
                row.Title = HttpUtility.HtmlEncode(m_TitleTB.Text);
                row.Message = HttpUtility.HtmlEncode(m_MessageTB.Text).Replace("\n", "<br/>");
                row.Id = Guid.NewGuid();
                row.Comment = "";

                data.Guestbook.Rows.Add(row);
                WriteData(data);

                // Wenn gewünscht, senden wir hier ein Notifizierungsmail.
                if ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["SendNotification"])
                    SendMail();

                // Zurücksetzen der Eingabefelder.
                Clear();

                // Darstellen der Gästebuchdaten mit Hilfe des Repeaters.
                Bind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        /// <summary>
        /// Bearbeiten des gewählten Eintrages.
        /// </summary>
        /// <param name="id"></param>
        private void OnEdit(Guid id)
        {
            // Setzen der ID als CommandArgument des Speicher-Buttons.
            m_EditSaveBtn.CommandArgument = id.ToString();

            GuestbookData data = ReadData();
            GuestbookData.GuestbookRow row = (GuestbookData.GuestbookRow)data.Guestbook.Select("ID = '" + id + "'")[0];
            m_FromTB.Text = HttpUtility.HtmlDecode(row.Name);
            m_EmailTB.Text = HttpUtility.HtmlDecode(row.Email);
            m_UrlTB.Text = HttpUtility.HtmlDecode(row.Url);
            m_TitleTB.Text = HttpUtility.HtmlDecode(row.Title);
            m_MessageTB.Text = HttpUtility.HtmlDecode(row.Message.Replace("<br/>", "\n"));
            if (!string.IsNullOrEmpty(row.Comment))
                m_CommentTB.Text = HttpUtility.HtmlDecode(row.Comment.Replace("<br/>", "\n"));

            // Die Darstellung wird auf "Editmodus" gesetzt.
            SetVisualMode(true);
        }

        /// <summary>
        /// Bricht das Editieren eines Eintrages ab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnEditCancel(object sender, System.EventArgs e)
        {
            // Die Darstellung wird auf "Benutzermodus" gesetzt.
            SetVisualMode(false);

            // Zurücksetzen der Eingabefelder.
            Clear();

            // Darstellen der Gästebuchdaten mit Hilfe des Repeaters.
            Bind();
        }

        /// <summary>
        /// Speichert einen vom Administrator geänderten Eintrag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnEditSave(object sender, System.EventArgs e)
        {
            // TODO: Aktuellen Eintrag speichern.
            string id = ((Portal.API.Controls.LinkButton)sender).CommandArgument;

            GuestbookData data = ReadData();
            GuestbookData.GuestbookRow row = (GuestbookData.GuestbookRow)data.Guestbook.Select("ID = '" + id + "'")[0];
            row.Name = HttpUtility.HtmlEncode(m_FromTB.Text);
            row.Email = HttpUtility.HtmlEncode(m_EmailTB.Text);
            row.Url = HttpUtility.HtmlEncode(m_UrlTB.Text);
            row.Title = HttpUtility.HtmlEncode(m_TitleTB.Text);
            row.Message = HttpUtility.HtmlEncode(m_MessageTB.Text).Replace("\n", "<br/>");
            row.Comment = HttpUtility.HtmlEncode(m_CommentTB.Text).Replace("\n", "<br/>");
            WriteData(data);

            // Die Darstellung wird auf "Benutzermodus" gesetzt.
            SetVisualMode(false);

            // Zurücksetzen der Eingabefelder.
            Clear();

            // Darstellen der Gästebuchdaten mit Hilfe des Repeaters.
            Bind();
        }

        /// <summary>
        /// Sendet den erfassten Eintrag via Email.
        /// </summary>
        private void SendMail()
        {
            Exception e = null;
            try
            {
                string szEmailServer = (string)m_ConfigData.Tables["Guestbook"].Rows[0]["EmailServer"];
                string szEmailSubject = (string)m_ConfigData.Tables["Guestbook"].Rows[0]["EmailSubject"];
                string szEmailTo = (string)m_ConfigData.Tables["Guestbook"].Rows[0]["EmailTo"];
                string szEmailFrom = (string)m_ConfigData.Tables["Guestbook"].Rows[0]["EmailFrom"];
                string szEmailUserName = (string)m_ConfigData.Tables["Guestbook"].Rows[0]["EmailUserName"];
                string szEmailPassword = (string)m_ConfigData.Tables["Guestbook"].Rows[0]["EmailPassword"];

                if (szEmailServer != "")
                {
                    MailMessage m = new MailMessage(szEmailFrom, szEmailTo);
                    m.Subject = szEmailSubject;

                    string body = "";
                    if (m_FromTB.Text != "")
                    {
                        body += Language.GetText("From") + ": " + m_FromTB.Text + "\n";
                    }
                    if (m_EmailTB.Text != "")
                    {
                        body += Language.GetText("Email") + ": " + m_EmailTB.Text + "\n";
                    }
                    if (m_UrlTB.Text != "")
                    {
                        body += Language.GetText("Url") + ": " + m_UrlTB.Text + "\n";
                    }
                    if (m_TitleTB.Text != "")
                    {
                        body += Language.GetText("Title") + ": " + m_TitleTB.Text + "\n";
                    }
                    if (body != "")
                    {
                        body += "\n";
                    }
                    body += m_MessageTB.Text;

                    m.Body = body;

                    // Falls der Email-Account Authentifizierung erfordert, 
                    // setzen wir hier die erforderlichen Parameter.
                    SmtpClient client = new SmtpClient(szEmailServer);
                    if ((bool)m_ConfigData.Tables["Guestbook"].Rows[0]["UseAuthentication"])
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.Credentials = new NetworkCredential(szEmailUserName, szEmailPassword.Length > 0 ? Crypto.Decrypt(szEmailPassword) : szEmailPassword);
                    }
                    client.Send(m);
                }
            }
            catch (SmtpFailedRecipientsException ex) { e = ex; }
            catch (SmtpException ex) { e = ex; }
            catch (ObjectDisposedException ex) { e = ex; }
            catch (InvalidOperationException ex) { e = ex; }
            catch (ArgumentOutOfRangeException ex) { e = ex; }
            catch (ArgumentNullException ex) { e = ex; }
            catch (Exception ex) { e = ex; }
            finally
            {
                if (e != null)
                {
                    // Die Mail konnte nicht geschickt werden.
                    Trace.Warn("Module", "Error sending notification email", e);

                    // Hat der User Edit-Rechte, so wird eine allfällige Fehlermeldung ausgegeben.
                    if (ModuleHasEditRights)
                    {
                        m_ErrorLbl.Text = Portal.API.Language.GetText(this, "ErrorSendingNotification") +
                                          "\n\nException:\n" + e.Message + "\n\nStackTrace:\n" + e.StackTrace;
                        m_ErrorLbl.Text = m_ErrorLbl.Text.Replace("\n", "<br/>");
                        m_ErrorTR.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        ///  Schreibt die Daten des Gästebuches.
        /// </summary>
        /// <param name="data"></param>
        override public void WriteData(GuestbookData data)
        {
            try
            {
                data.WriteXml(GuestbookConfigFile);
            }
            catch (Exception e)
            {
                // Do not throw exceptions
                Trace.Warn("Module", "Error writing Modules Config Dataset", e);
            }
        }

        private void DataGrid_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            DataGrid.CurrentPageIndex = e.NewPageIndex;
            Bind();
        }
    }
}
