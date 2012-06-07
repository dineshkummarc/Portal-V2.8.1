namespace Portal.Modules.Guestbook
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Portal;
  using Portal.API;

	/// <summary>
	///		Zusammenfassung für EditGuestbook.
	/// </summary>
	public partial class EditGuestbook : Portal.API.EditModule
	{
		protected DataSet m_Data;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				// Schema exists - cannot be null!
				m_Data = ReadConfig();
				Bind();
			}
			else
			{
				m_Data = (DataSet)ViewState["DataSet"];
			}
		}


		void Bind()
		{
			if (m_Data.Tables["Guestbook"].Rows.Count == 0)
			{
				DataRow row = m_Data.Tables["Guestbook"].NewRow();
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
        m_Data.Tables["Guestbook"].Rows.Add(row);
			}

			m_EmailCB.Checked = (bool)m_Data.Tables["Guestbook"].Rows[0]["UseEmail"];
			m_UrlCB.Checked = (bool)m_Data.Tables["Guestbook"].Rows[0]["UseUrl"];
			m_EmailNotificationCB.Checked = (bool)m_Data.Tables["Guestbook"].Rows[0]["SendNotification"];
			m_EmailFromTB.Text = (string)m_Data.Tables["Guestbook"].Rows[0]["EmailFrom"];
			m_EmailToTB.Text = (string)m_Data.Tables["Guestbook"].Rows[0]["EmailTo"];
			m_EmailSubjectTB.Text = (string)m_Data.Tables["Guestbook"].Rows[0]["EmailSubject"];
			m_EmailServerTB.Text = (string)m_Data.Tables["Guestbook"].Rows[0]["EmailServer"];
      m_EmailAuthentification.Checked = (bool)m_Data.Tables["Guestbook"].Rows[0]["UseAuthentication"];
      m_EmailUserName.Text = (string)m_Data.Tables["Guestbook"].Rows[0]["EmailUserName"];
			try
			{
				m_EmailPassword.Text = Crypto.Decrypt((string)m_Data.Tables["Guestbook"].Rows[0]["EmailPassword"]);
			}
			catch (Exception)
			{
				m_EmailPassword.Text = (string)m_Data.Tables["Guestbook"].Rows[0]["EmailPassword"];
			}
			ViewState["DataSet"] = m_Data;
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

    }
		#endregion

		protected void m_CancelBtn_Click(object sender, System.EventArgs e)
		{
			RedirectBack();
		}

		protected void m_SpeichernBtn_Click(object sender, System.EventArgs e)
		{
			m_Data.Tables["Guestbook"].Rows[0]["UseEmail"] = m_EmailCB.Checked;
			m_Data.Tables["Guestbook"].Rows[0]["UseUrl"] = m_UrlCB.Checked;
			m_Data.Tables["Guestbook"].Rows[0]["SendNotification"] = m_EmailNotificationCB.Checked;
			m_Data.Tables["Guestbook"].Rows[0]["EmailFrom"] = m_EmailFromTB.Text;
			m_Data.Tables["Guestbook"].Rows[0]["EmailTo"] = m_EmailToTB.Text;
			m_Data.Tables["Guestbook"].Rows[0]["EmailSubject"] = m_EmailSubjectTB.Text;
			m_Data.Tables["Guestbook"].Rows[0]["EmailServer"] = m_EmailServerTB.Text;
			m_Data.Tables["Guestbook"].Rows[0]["UseAuthentication"] = m_EmailAuthentification.Checked;
			m_Data.Tables["Guestbook"].Rows[0]["EmailUserName"] = m_EmailUserName.Text;
			m_Data.Tables["Guestbook"].Rows[0]["EmailPassword"] = Crypto.Encrypt(m_EmailPassword.Text);

			WriteConfig(m_Data);
			RedirectBack();
		}
	}
}
