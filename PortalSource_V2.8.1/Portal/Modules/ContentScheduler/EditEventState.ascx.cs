using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.ContentScheduler
{
  public partial class EditEventState : Portal.StateBase.ModuleState
  {
    ContentSchedulerHandler _contentSchedulerHandler;

    protected void Page_Load(object sender, EventArgs e)
    {
      if (TransitionArgument != null)
      {
        // Es handelt sich um ein Edit. Darum wird der aktuelle Datensatz geladen.
        Guid Id = new Guid(TransitionArgument.ToString());
        _contentSchedulerHandler = new ContentSchedulerHandler(this, Id);
      }
      else
        _contentSchedulerHandler = new ContentSchedulerHandler(this);
      
      if (!ControlPostback)
      {
        if (_contentSchedulerHandler.CurrentContentEvent == null)
        {
          // Ein New.
          // Standardzeitpunkt: Nächster Tag.
          DateTime newDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
          _activationDate.DateTimeValue = newDate.AddDays(1);
          _deleteBtn.Visible = false;
        }
        else
        {
          // Ein Edit.
          _activationDate.DateTimeValue = _contentSchedulerHandler.CurrentContentEvent.ActivationDate;
          _hintEdit.Text = _contentSchedulerHandler.CurrentContentEvent.Hint;
          _deleteBtn.Visible = true;
        }       
        
        // Die Controls für New / Edit aktivieren.
        bool hasPage = (_contentSchedulerHandler.CurrentContentEvent != null) && !_contentSchedulerHandler.CurrentContentEvent.IsHtmlPageNull();
        _createPage.Visible = !hasPage;
        _templateSelection.Visible = !hasPage;
        _editPage.Visible = hasPage;

        if(!hasPage)
        {
          // Abfüllen der bestehenden Seiten als Templates.
          string textData = " - " + Portal.API.Language.GetText(this, "EmptyPage") + " - ";
          _templateSelection.Items.Add(new ListItem(textData, Guid.Empty.ToString()));
          foreach (DataRowView  viewPageRow in _contentSchedulerHandler.ConfigurationData.ContentEvent.DefaultView)
          {
            ContentSchedulerData.ContentEventRow pageRow = (ContentSchedulerData.ContentEventRow)viewPageRow.Row;
            if (!pageRow.IsHtmlPageNull())
            {
              textData = pageRow.Hint + " (" +
                                 pageRow.ActivationDate.ToString(Portal.API.Config.DateTimeFormat) + ")";
              _templateSelection.Items.Add(new ListItem(textData, pageRow.Id.ToString()));
            }
          }
        }
      }
    }


    protected void OnSave(object sender, EventArgs e)
    {
      Save();
      ProcessEvent((int)StateEvent.Save);
    }


    private void Save()
    {
      _contentSchedulerHandler.SetData(_hintEdit.Text, _activationDate.DateTimeValue);
    }


    protected void OnCancel(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.Cancel);
    }


    protected void OnDelete(object sender, EventArgs e)
    {
      _contentSchedulerHandler.Delete();

      ProcessEvent((int)StateEvent.Cancel);
    }


    protected void OnCreatePage_Click(object sender, EventArgs e)
    {
      Save();

      // Ermittle das Template, falls eines angegeben ist.
      Guid templateId = new Guid(_templateSelection.SelectedValue);
      if (templateId != Guid.Empty)
      {
        _contentSchedulerHandler.CopyContent(templateId);
      }

      ProcessEvent((int)StateEvent.EditPage, _contentSchedulerHandler.CurrentContentEventId);

    }


    protected void OnEditPage_Click(object sender, EventArgs e)
    {
      Save();
      ProcessEvent((int)StateEvent.EditPage, _contentSchedulerHandler.CurrentContentEventId);
    }
  }

}