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
using System.Collections.Generic;

namespace Portal.Modules.FormBuilder
{
  public partial class EditValidationState : Portal.StateBase.ModuleState
  {
    private List<ModuleConfig.ValidationData> _validators;

    protected void Page_Load(object sender, EventArgs e)
    {
      // Die Daten abfüllen, bzw. aus dem Viewstate laden.
      if (!ControlPostback)
      {
        // Lokalisierung der Spaltentitel.
        foreach (DataControlField column in _fieldValidators.Columns)
          column.HeaderText = Portal.API.Language.GetText(this, column.HeaderText);

        // Daten abfüllen.
        ModuleConfig config = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
        if (config == null)
          config = new ModuleConfig();

        _validators = config.FormValidation;
        ViewState["ValidatorData"] = _validators;
      }
      else
        _validators = (List<ModuleConfig.ValidationData>) ViewState["ValidatorData"];

      _fieldValidators.DataSource = _validators;
      _fieldValidators.DataBind();
    }


    protected void OnCancel(object sender, EventArgs e)
    {
      ProcessEvent((int)StateEvent.Cancel);
    }

    protected void OnSave(object sender, EventArgs e)
    {
      lock (ModuleConfig.ConfigLock)
      {
        // Bestehende Konfiguration einlesen.
        ModuleConfig cfg = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
        if (cfg == null)
          cfg = new ModuleConfig();

        cfg.FormValidation = _validators;
        
        WriteConfig(cfg);
      }

      ProcessEvent((int)StateEvent.Save);
    }

    protected void OnAddNewValidator(object sender, EventArgs e)
    {
      int pos = _validators.Count;
      ModuleConfig.ValidationData validData = new ModuleConfig.ValidationData();
      validData.ErrorMessage = Portal.API.Language.GetText(this, "DefaultErrorMessage");
      _validators.Insert(pos, validData);
      _fieldValidators.EditIndex = pos;
      _fieldValidators.DataSource = _validators;
      _fieldValidators.DataBind();
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
      _fieldValidators.EditIndex = e.NewEditIndex;
      _fieldValidators.DataSource = _validators;
      _fieldValidators.DataBind();
    }
    protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
      _fieldValidators.EditIndex = -1;
      _fieldValidators.DataSource = _validators;
      _fieldValidators.DataBind();
    }
    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
      GridViewRow row = _fieldValidators.Rows[e.RowIndex];
      ModuleConfig.ValidationData data = _validators[e.RowIndex];
      ApplyData(row, data);

      _fieldValidators.EditIndex = -1;
      _fieldValidators.DataSource = _validators;
      _fieldValidators.DataBind();
    }

    private static void ApplyData(GridViewRow row, ModuleConfig.ValidationData data)
    {
      data.FieldName = ((DropDownList)row.FindControl("_fieldName")).Text;
      data.FieldType = (ModuleConfig.ValidationData.ValidationType)Enum.Parse(typeof(ModuleConfig.ValidationData.ValidationType), ((DropDownList)row.FindControl("_fieldType")).SelectedValue);
      data.ErrorMessage = ((TextBox)row.FindControl("_errorMessage")).Text;
      data.IsMandatory = ((CheckBox)row.FindControl("_mandatoryCheck")).Checked;

      IValidator validatorObj = ValidatorFactory.CreateValidator(data);
      if (validatorObj != null)
        validatorObj.SaveValidatorSettings(row.FindControl("_detailsEdit"));
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
      _validators.RemoveAt(e.RowIndex);
            _fieldValidators.DataSource = _validators;
      _fieldValidators.DataBind();
    }


    protected void OnTypeComboDataBind(object sender, EventArgs e)
    {
      // Die Liste mit den verfügbaren Typen vorbereiten.
      ListItemCollection typeListTitems = new ListItemCollection();
      ListItem li = null;
      foreach (ModuleConfig.ValidationData.ValidationType validationType
        in Enum.GetValues(typeof(ModuleConfig.ValidationData.ValidationType)))
      {
        li = new ListItem();
        li.Text = Portal.API.Language.GetText(this, validationType.ToString());
        li.Value = validationType.ToString();
        typeListTitems.Add(li);
      }

      DropDownList ddList = ((DropDownList)sender);
      ddList.DataSource = typeListTitems;
    }


    protected void OnNameComboDataBind(object sender, EventArgs e)
    {
      DropDownList ddList = ((DropDownList)sender);

      // Verfügbare Felder laden.
      ModuleConfig config = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
      if (config == null)
        config = new ModuleConfig();

      string[] fields = Portal.API.HtmlAnalyzer.GetInputNames(config.FormContent);
    
      // Falls der gewählte Eintrag nicht mehr existiert, wird der erste Wert ausgewählt.
      ModuleConfig.ValidationData validationData = (ModuleConfig.ValidationData)((GridViewRow)((Control)sender).BindingContainer).DataItem;
      if (null == Array.Find<string>(fields, delegate(string test) { return test == validationData.FieldName; }))
        ddList.SelectedValue = fields[0];

      ddList.DataSource = fields;
    }

    protected void OnDetailsEditDataBind(object sender, EventArgs e)
    {
      ModuleConfig.ValidationData validationData = (ModuleConfig.ValidationData)((GridViewRow)((Control)sender).BindingContainer).DataItem;
      IValidator validatorObj = ValidatorFactory.CreateValidator(validationData);
      if (validatorObj != null)
        validatorObj.AddValidatorSettingsEdit((Control)sender);
    }


    protected void OnDetailsViewDataBind(object sender, EventArgs e)
    {
      ModuleConfig.ValidationData validationData = (ModuleConfig.ValidationData) ((GridViewRow)((Control)sender).BindingContainer).DataItem;
      IValidator validatorObj = ValidatorFactory.CreateValidator(validationData);
      if (validatorObj != null)
        validatorObj.AddValidatorSettingsView((Control)sender);
    }

    protected void OnTypeChanged(Object sender, EventArgs e)
    {
      DropDownList dropDown = (DropDownList)sender;
      string typeValue = dropDown.SelectedValue;
      GridViewRow gridRow = (GridViewRow)dropDown.BindingContainer;
      ModuleConfig.ValidationData data = _validators[gridRow.DataItemIndex];
      
      // Zuerst die gemachten Änderungen übernehmen.
      ApplyData(gridRow, data);

      data.FieldType = (ModuleConfig.ValidationData.ValidationType)Enum.Parse(typeof(ModuleConfig.ValidationData.ValidationType), typeValue);
      _fieldValidators.DataSource = _validators;
      _fieldValidators.DataBind();
    }
  }
}