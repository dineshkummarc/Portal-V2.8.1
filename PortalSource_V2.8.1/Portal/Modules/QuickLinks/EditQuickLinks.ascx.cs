namespace Portal.Modules.QuickLinks
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Portal;

	/// <summary>
	///		Zusammenfassung für EditQuickLinks.
	/// </summary>
	public partial class EditQuickLinks : Portal.API.EditModule
	{
		protected DataSet m_Data;

		/// <summary>
		/// Wird beim Aufruf der Seite aufgerufen.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				// Schema exists - cannot be null!
				m_Data = ReadConfig();
        ReorderLinks();
				BindGrid();
			}
			else
			{
				// Auslesen der Konfigurationsdaten aus dem ViewState.
				m_Data = (DataSet)ViewState["DataSet"];
			}
		}

		/// <summary>
		/// Verbinden des Datenobjektes mit dem DataGrid.
		/// </summary>
		private void BindGrid()
		{
			m_Data.Tables["links"].DefaultView.Sort = "Position ASC";
			LinksGrid.DataSource = m_Data.Tables["links"];
			LinksGrid.DataBind();

			// Speichern der Konfigurationsdaten im ViewState.
			ViewState["DataSet"] = m_Data;
		}

		/// <summary>
		/// Verschiebt den aktuellen Eintrag um eine Position nach oben.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected void OnUpLink(object sender, CommandEventArgs args)
		{
			int nPos = (int)Int32.Parse((string)args.CommandArgument);

			if (nPos > 0)
			{
				DataRow[] curRow = m_Data.Tables["links"].Select("Position = " + nPos.ToString());
				DataRow[] prevRow = m_Data.Tables["links"].Select("Position = " + (nPos-1).ToString());
				if ((curRow.Length > 0) && (prevRow.Length > 0))
				{
					curRow[0]["Position"] = nPos - 1;
					prevRow[0]["Position"] = nPos;
				}
			}

			BindGrid();
		}

		/// <summary>
		/// Verschiebt den aktuellen Eintrag um eine Position nach unten.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected void OnDownLink(object sender, CommandEventArgs args)
		{
			int nPos = (int)Int32.Parse((string)args.CommandArgument);

			DataRow[] curRow = m_Data.Tables["links"].Select("Position = " + nPos.ToString());
			DataRow[] nextRow = m_Data.Tables["links"].Select("Position = " + (nPos+1).ToString());
			if ((curRow.Length > 0) && (nextRow.Length > 0))
			{
				curRow[0]["Position"] = nPos + 1;
				nextRow[0]["Position"] = nPos;
			}

			BindGrid();
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
			this.LinksGrid.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.LinksGrid_CancelCommand);
			this.LinksGrid.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.LinksGrid_EditCommand);
			this.LinksGrid.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.LinksGrid_UpdateCommand);
			this.LinksGrid.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.LinksGrid_DeleteCommand);

			CheckBoxColumn col = new CheckBoxColumn();
			col.HeaderText = Portal.API.Language.GetText(Portal.API.Module.GetModuleControl(this), "NewWindow");
			col.DataField = "OpenInNewWindow";
			col.HeaderStyle.Width = new Unit("100px");
			col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
			LinksGrid.Columns.Add(col);
		}
		#endregion

		/// <summary>
		/// Beim Klicken des Link-Buttons "Abbrechen" beenden wir den Edit-Modus.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CancelLB_Click(object sender, System.EventArgs e)
		{
			RedirectBack();
		}

		/// <summary>
		/// Beim Klicken des Link-Buttons "Speichern" wird die aktuelle Konfiguration gespeichert und der Edit-Modus beendet.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void SaveLB_Click(object sender, System.EventArgs e)
		{
			ReorderLinks();
			WriteConfig(m_Data);
			RedirectBack();
		}

		/// <summary>
		/// Beim Klicken des Link-Buttons "Hinzufügen" wird ein neuer Eintrag erstellt.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void AddLB_Click(object sender, System.EventArgs e)
		{
			// Ermitteln der Position des neuen Eintrages.
			DataRow[] rows = m_Data.Tables["links"].Select("Position = MAX(Position)");
			int nPos = 0;
			if (rows.GetLength(0) > 0)
				nPos = 1 + (int)rows[0]["Position"];

			// Toolbar ausblenden.
			lbLinks.Visible = false;

			// Default-Werte einfüllen.
			DataTable td = m_Data.Tables["links"];
			DataRow dr = td.NewRow();
			dr["Text"] = "";
			dr["URL"] = "http://";
			dr["OpenInNewWindow"] = true;
			dr["Position"] = nPos;
			td.Rows.Add(dr);
			ViewState["CurrentPos"] = nPos;

			// Unnötige Spalten ausblenden.
			LinksGrid.Columns[1].Visible = false;
			LinksGrid.Columns[2].Visible = false;
			LinksGrid.Columns[3].Visible = false;
			LinksGrid.EditItemIndex = td.Rows.Count-1;

			BindGrid();
		}

		/// <summary>
		/// Wechselt den aktuellen Eintrag in den Edit-Modus.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void LinksGrid_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			ViewState["CurrentPos"] = e.Item.ItemIndex;

			// Toolbar ausblenden.
			lbLinks.Visible = false;

			// Unnötige Spalten ausblenden.
			LinksGrid.Columns[1].Visible = false;
			LinksGrid.Columns[2].Visible = false;
			LinksGrid.Columns[3].Visible = false;
			LinksGrid.EditItemIndex = e.Item.ItemIndex;

			BindGrid();
		}

		/// <summary>
		/// Übernimmt den aktuellen Eintrag.
		/// Er wird aber noch nicht in das Konfigurationsfile geschrieben. Dies geschieht erst,
		/// wenn der Link-Button "Speichern" gedrückt wird.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void LinksGrid_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			int idx = LinksGrid.EditItemIndex;
			DataRow[] rows = m_Data.Tables["links"].Select("", "Position ASC");
			DataRow dr = rows[idx];
			dr["Text"] = HttpUtility.HtmlEncode(((TextBox)e.Item.Cells[4].Controls[0]).Text);
			dr["URL"] = ((TextBox)e.Item.Cells[5].Controls[0]).Text;
			dr["OpenInNewWindow"] = ((CheckBox)e.Item.Cells[7].Controls[0]).Checked;
			dr["Position"] = (int)ViewState["CurrentPos"];

			// Toolbar einblenden.
			lbLinks.Visible = true;

			// Unnötige Spalten einblenden.
			LinksGrid.Columns[1].Visible = true;
			LinksGrid.Columns[2].Visible = true;
			LinksGrid.Columns[3].Visible = true;
			LinksGrid.EditItemIndex = -1;

			BindGrid();
		}

		/// <summary>
		/// Abbrechen des Bearbeitens des aktuellen Eintrages.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void LinksGrid_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			// Toolbar einblenden.
			lbLinks.Visible = true;

			// Unnötige Spalten einblenden.
			LinksGrid.Columns[1].Visible = true;
			LinksGrid.Columns[2].Visible = true;
			LinksGrid.Columns[3].Visible = true;
			LinksGrid.EditItemIndex = -1;
			
			BindGrid();
		}

		/// <summary>
		/// Löscht den aktuellen Eintrag.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void LinksGrid_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			int idx = e.Item.ItemIndex;
			DataRow[] rows = m_Data.Tables["links"].Select("", "Position ASC");
			DataRow dr = rows[idx];
			dr.Delete();

			// Position neu sortieren
			ReorderLinks();

			BindGrid();
		}

		/// <summary>
		/// Nummeriert die Links neu, damit die Nummerierung durchgehend von 0 bis n ist.
		/// </summary>
		private void ReorderLinks()
		{
			// Position neu sortieren
			DataRow[] rows = m_Data.Tables["links"].Select("", "Position ASC");
			for (int nzIndex = 0; nzIndex < m_Data.Tables["links"].Rows.Count; nzIndex++)
			{
				rows[nzIndex]["Position"] = nzIndex;
			}
		}
	}
}
