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
using Portal.API;

namespace Portal.Modules.FormBuilder
{
  public partial class FormBuilder : Portal.API.Module
  {
    private enum FormState : int
    {
      Form = 0,     // Das Formular wird angzeigt.
      Success = 1,  // Erfolg.
      Error = 2,    // Fehler.
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      FormState reqState = RequestedState;

      // Konfiguration einlesen.
      ModuleConfig cfg = LoadConfig();

      if (reqState == FormState.Form)
      {
        // Wurde das Formular bereits übermittelt?
        if (IsPostBack && IsSubmitted(cfg.FormContent))
        {
          // Eingabedaten auswerten.
          Guid errorValidator = Guid.Empty;
          bool success = ValidateRequest(cfg, out errorValidator);
          bool redirect = true;

          if (success)
          {
            if (cfg.SendEmail)
            {
              EmailSender emailSender = new EmailSender();
              try
              {
                emailSender.Process(cfg);
              }
              catch (System.Exception ex)
              {
                // Falls der Benutzer Edit-Rechte hat, wird die Meldung aus der Exception angezeigt.
                if (ModuleHasEditRights)
                {
                  _content.Text = "Error: " + ex.Message;
                  redirect = false;
                }

                success = false;
              }
            }
          }

          if(redirect)
          {
            // Falls die Erfolgsseite angezeigt werden soll, wird diese eingeblendet.
            if (success && cfg.ShowSuccessPage)
            {
              _content.Text = cfg.SuccessContent;
              NavigateTo(FormState.Success);
            }
            else if (!success)
            {
              _content.Text = cfg.ErrorContent;
              NavigateTo(FormState.Error, errorValidator);
            }
            else
            {
              // Die eigene Seite wieder laden, damit die Formulardaten nicht ein zweites mal übermittelt werden können.
              NavigateTo(FormState.Form);
            }
          }
        }
        else
        {
          // Formular anzeigen.
          _content.Text = cfg.FormContent;
        }
      }
      else 
      {
        // Html ermitteln.
        string html = GetCurrentHtml(reqState, cfg);

        // Wurde bereits die Meldung quittiert?
        if (IsPostBack && IsSubmitted(html))
        {
          // Zurück zum Formular.
          NavigateTo(FormState.Form);
        }
        else
        {
          // Meldung anzeigen.
          _content.Text = html;
        }
      }
    }

    /// <summary>
    /// Validiert den Request.
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="validatorId"></param>
    /// <returns></returns>
    private bool ValidateRequest(ModuleConfig cfg, out Guid validatorId)
    {
      foreach(ModuleConfig.ValidationData validatorData in cfg.FormValidation)
      {
        IValidator validatorObj = ValidatorFactory.CreateValidator(validatorData);
        if(validatorObj != null)
        {
          // Wert validieren.
          if(!validatorObj.Validate(Request.Form[validatorData.FieldName]))
          {
            // Validierung fehlgeschlagen.
            validatorId = validatorData.Id;
            return false;
          }
        }
      }
      validatorId = Guid.Empty;
      return true;
    }


    /// <summary>
    /// Ermittelt das aktuelle HTML zum anzeigen.
    /// </summary>
    /// <param name="reqState"></param>
    /// <param name="cfg"></param>
    /// <returns></returns>
    private string GetCurrentHtml(FormState reqState, ModuleConfig cfg)
    {
      if(reqState == FormState.Success)
      {
        return cfg.SuccessContent;
      }
      else
      {
        string html = cfg.ErrorContent;
        string errorMsg = null;

        // Suche nach dem Validator.
        string Id = Request.QueryString["Id"];
        if(!string.IsNullOrEmpty(Id))
        {
          Guid validatorId = new Guid(Id);
          if (Guid.Empty != validatorId)
          {
            // Suche den Validator.
            foreach(ModuleConfig.ValidationData validatorData in cfg.FormValidation)
            {
              if (validatorData.Id == validatorId)
              {
                errorMsg = validatorData.ErrorMessage;
                break;
              }
            }
          }
          if (errorMsg == null)
            errorMsg = cfg.CommonErrorMsg;
          html = html.Replace(ModuleConfig.Messageplaceholder, errorMsg);
        }
        return html;
      }
    }


    /// <summary>
    /// Überprüft ob ein Submit Button aus dem angegebenen HTML gedrückt wurde.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    private bool IsSubmitted(string html)
    {
      // Überprüfen aller Post Variablen, ob einer der Submit Buttons enthalten ist.
      string[] submitNames = Portal.API.HtmlAnalyzer.GetSubmitNames(html);
      foreach (string name in submitNames)
      {
        if (Request.Form[name] != null)
          return true;
      }
      return false;
    }

    private ModuleConfig LoadConfig()
    {
      ModuleConfig cfg = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
      if (cfg == null)
        cfg = new ModuleConfig();
      return cfg;
    }

    private FormState RequestedState
    {
      get
      {
        string stateKey = UniqueID + "State";

        // Ermitteln des aktuellen Status aus dem QueryString.
        string reqState = Request.QueryString[stateKey];
        FormState requestedState = FormState.Form;
        if (!string.IsNullOrEmpty(reqState))
        {
          try
          {
            requestedState = (FormState) Enum.Parse(typeof(FormState), reqState);
          }
          catch(ArgumentException) {}
        }

        return requestedState;
      }
    }

    /// <summary>
    /// Führt eine weiterleitung zur Seite mit den entsprechenden Argumenten aus. Die Weiterleitung wird benötigt, damit
    /// die Daten nicht ein zweites mal übermittelt werden können.
    /// </summary>
    /// <param name="newState"></param>
    private void NavigateTo(FormState newState)
    {
      // Ziel Url zusammenstellen.
      string stateKey = UniqueID + "State";

      UrlBuilder urlBuilder = new UrlBuilder(Request);
      if (newState != FormState.Form)
        urlBuilder.QueryString[stateKey] = newState.ToString();
      else
        urlBuilder.QueryString.Remove(stateKey);  // Beim Formular wird keine Argument angegeben.
      urlBuilder.RedirectTo();
    }


    /// <summary>
    /// Führt eine weiterleitung zur Seite mit den entsprechenden Argumenten aus. Die Weiterleitung wird benötigt, damit
    /// die Daten nicht ein zweites mal übermittelt werden können.
    /// </summary>
    /// <param name="newState"></param>
    private void NavigateTo(FormState newState, Guid Id)
    {
      // Ziel Url zusammenstellen.
      string stateKey = UniqueID + "State";

      UrlBuilder urlBuilder = new UrlBuilder(Request);
      if (newState != FormState.Form)
        urlBuilder.QueryString[stateKey] = newState.ToString();
      else
        urlBuilder.QueryString.Remove(stateKey);  // Beim Formular wird keine Argument angegeben.
      urlBuilder.QueryString["Id"] = Id.ToString();
      urlBuilder.RedirectTo();
    }
  }
}
