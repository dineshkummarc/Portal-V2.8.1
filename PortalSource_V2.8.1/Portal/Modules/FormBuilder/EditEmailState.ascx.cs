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

namespace Portal.Modules.FormBuilder
{
  public partial class EditEmailState : Portal.StateBase.ModuleState
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!ControlPostback)
      {
        // Konfiguration laden.
        ModuleConfig cfg = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
        if (cfg == null)
          cfg = new ModuleConfig();

        _sendEmailCheck.Checked = cfg.SendEmail;
        _serverEdit.Text = cfg.EmailConfig.Server;
        _serverNeedAuthCheck.Checked = cfg.EmailConfig.ServerNeedAuth;
        _userNameEdit.Text = cfg.EmailConfig.LoginUser;
        _senderEdit.Text = cfg.EmailConfig.Sender;
        _subjectEdit.Text = cfg.EmailConfig.Subject;
        _body.Text = cfg.EmailConfig.Body;
        _bodyAsHtmlCheck.Checked = cfg.EmailConfig.BodyIsHtml;

        string allAddress = "";
        foreach(string address in cfg.EmailConfig.ToRecipient)
          allAddress += address + ";";
        _toEdit.Text = allAddress;

        allAddress = "";
        foreach(string address in cfg.EmailConfig.CcRecipient)
          allAddress += address + ";";
        _ccEdit.Text = allAddress;

        allAddress = "";
        foreach(string address in cfg.EmailConfig.BccRecipient)
          allAddress += address + ";";
        _bccEdit.Text = allAddress;

        // Die Namen der Platzhalter in die Liste mit der Auswahl abfüllen.
        string[] placeHolderNames = Portal.API.HtmlAnalyzer.GetInputNames(cfg.FormContent);
        _availablePlaceholder.DataSource = placeHolderNames;
        _availablePlaceholder.DataBind();

        // Die Client-Seitige Aktion zum Hinzufügen eines Platzhalters ermöglichen.
        if(placeHolderNames.Length > 0)
        {
          string jsInsert = "InsertText('{{' + document.getElementById('{0}').value + '}}', '{1}');return false;";

          _insertBodyBtn.Enabled = true;
          _insertBodyBtn.OnClientClick = string.Format(jsInsert, _availablePlaceholder.ClientID, _body.ClientID);

          _insertSubjectBtn.Enabled = true;
          _insertSubjectBtn.OnClientClick = string.Format(jsInsert, _availablePlaceholder.ClientID, _subjectEdit.ClientID);

          _insertBccBtn.Enabled = true;
          _insertBccBtn.OnClientClick = string.Format(jsInsert, _availablePlaceholder.ClientID, _bccEdit.ClientID);

          _insertCcBtn.Enabled = true;
          _insertCcBtn.OnClientClick = string.Format(jsInsert, _availablePlaceholder.ClientID, _ccEdit.ClientID);

          _insertToBtn.Enabled = true;
          _insertToBtn.OnClientClick = string.Format(jsInsert, _availablePlaceholder.ClientID, _toEdit.ClientID);

          _insertSenderBtn.Enabled = true;
          _insertSenderBtn.OnClientClick = string.Format(jsInsert, _availablePlaceholder.ClientID, _senderEdit.ClientID);
        }
      }

      AddInsertScript();
    }


    private void AddInsertScript()
    {
      string jScript = "<script language=\"javascript\">"
     + "function InsertText(str, objId) { "
     + "  var e = document.getElementById(objId), s = null, r = null; "
     + "  if(e) { "
     + "    if((s = document.selection) && s.createRange) {"
     + "      var repos = false; "
     + "      e.focus(); "
     + "      if((r = s.createRange())) { "
     + "        if(r.text.length) {repos = true;} "
     + "        r.text = str; "
     + "        if(repos && s.empty) {s.empty();} "
     + "      } "
     + "    } else { "
     + "      e.value += str; "
     + "    } "
     + "  } "
     + "} "
     + "</script>";
      if (!Page.ClientScript.IsClientScriptBlockRegistered(typeof(Page), "InsertText"))
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "InsertText", jScript);
    }


    protected void Save()
    {
      lock (ModuleConfig.ConfigLock)
      {
        ModuleConfig cfg = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
        if (cfg == null)
          cfg = new ModuleConfig();

        cfg.SendEmail = _sendEmailCheck.Checked;
        cfg.EmailConfig.Server = _serverEdit.Text;
        cfg.EmailConfig.ServerNeedAuth = _serverNeedAuthCheck.Checked;
        cfg.EmailConfig.LoginUser = _userNameEdit.Text;
        if (!string.IsNullOrEmpty(_passwordEdit.Text))
          cfg.EmailConfig.LoginPassword = _passwordEdit.Text;
        cfg.EmailConfig.Sender = _senderEdit.Text;
        cfg.EmailConfig.Subject = _subjectEdit.Text;
        cfg.EmailConfig.Body = _body.Text;
        cfg.EmailConfig.BodyIsHtml = _bodyAsHtmlCheck.Checked;

        cfg.EmailConfig.ToRecipient.Clear();
        string[] allAddress = _toEdit.Text.Split(';');
        foreach (string address in allAddress)
        {
          string trimAddress = address.Trim();
          if (!string.IsNullOrEmpty(trimAddress))
            cfg.EmailConfig.ToRecipient.Add(trimAddress);
        }

        cfg.EmailConfig.CcRecipient.Clear();
        allAddress = _ccEdit.Text.Split(';');
        foreach (string address in allAddress)
        {
          string trimAddress = address.Trim();
          if (!string.IsNullOrEmpty(trimAddress))
            cfg.EmailConfig.CcRecipient.Add(trimAddress);
        }

        cfg.EmailConfig.BccRecipient.Clear();
        allAddress = _bccEdit.Text.Split(';');
        foreach (string address in allAddress)
        {
          string trimAddress = address.Trim();
          if (!string.IsNullOrEmpty(trimAddress))
            cfg.EmailConfig.BccRecipient.Add(trimAddress);
        }

        WriteConfig(cfg);
      }
    }

    protected void OnSave(object sender, EventArgs e)
    {
      Save();

    }

    protected void OnCancel(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.Cancel);
    }

    protected void OnSaveBack(object sender, EventArgs e)
    {
      Save();
      ProcessEvent((int)StateEvent.Save);
    }
    protected void _senderEdit_TextChanged(object sender, EventArgs e)
    {

    }
    protected void _subjectEdit_TextChanged(object sender, EventArgs e)
    {

    }
}
}
