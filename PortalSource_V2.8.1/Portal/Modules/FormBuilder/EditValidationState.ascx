<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditValidationState.ascx.cs"
  Inherits="Portal.Modules.FormBuilder.EditValidationState" %>
<%@ Register TagPrefix="portal" Assembly="Portal.API" Namespace="Portal.API.Controls" %>
<portal:LinkButton ID="_addNewValidator" runat="server" CausesValidation="False" EnableViewState="False"
  LanguageRef="AddNewValidator" OnClick="OnAddNewValidator" >Neue Überprüfung hinzufügen</portal:LinkButton>&nbsp;
|&nbsp; &nbsp;<portal:LinkButton ID="_SaveBtn" runat="server" CausesValidation="False" EnableViewState="False"
  LanguageRef="Save" OnClick="OnSave">Speichern</portal:LinkButton>&nbsp;
|&nbsp;
<portal:LinkButton ID="_CancelBtn" runat="server" CausesValidation="False" EnableViewState="False"
  LanguageRef="Cancel" OnClick="OnCancel">Abbrechen</portal:LinkButton><br />
<hr />
<asp:GridView ID="_fieldValidators" runat="server" AutoGenerateColumns="False" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting">
  <Columns>
    <asp:CommandField ButtonType="Image" CancelImageUrl="~/PortalImages/cancel.gif" DeleteImageUrl="~/PortalImages/Delete.gif"
      EditImageUrl="~/PortalImages/Edit.gif" ShowEditButton="True" UpdateImageUrl="~/PortalImages/save.gif" />
    <asp:CommandField ButtonType="Image" DeleteImageUrl="~/PortalImages/delete.gif" ShowDeleteButton="True" />
    <asp:TemplateField HeaderText="FieldName">
      <EditItemTemplate>
        <asp:DropDownList ID="_fieldName" runat="server" OnDataBinding="OnNameComboDataBind" SelectedValue ='<%# Bind("FieldName") %>'>
        </asp:DropDownList>
      </EditItemTemplate>
      <ItemTemplate>
        <asp:Label ID="_fieldNameLabel" runat="server" Text='<%# Bind("FieldName") %>'></asp:Label>
      </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="FieldType">
      <EditItemTemplate>
        <asp:DropDownList ID="_fieldType" runat="server" OnDataBinding="OnTypeComboDataBind"  DataTextField="Text" 
          DataValueField="Value" SelectedValue ='<%# Bind("FieldType") %>' AutoPostBack="true" OnSelectedIndexChanged="OnTypeChanged">
        </asp:DropDownList>
      </EditItemTemplate>
      <itemtemplate>
      <asp:Label ID="Label1" runat="server" Text='<%# Portal.API.Language.GetText(this, DataBinder.Eval(Container.DataItem, "FieldType").ToString()) %>'></asp:Label>
      </itemtemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="ErrorMessage">
      <EditItemTemplate>
        <asp:TextBox ID="_errorMessage" runat="server" Text='<%# Bind("ErrorMessage") %>' Width="20em"></asp:TextBox>
      </EditItemTemplate>
      <ItemTemplate>
        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ErrorMessage") %>'></asp:Label>
      </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="MandatoryField">
      <EditItemTemplate>
        <asp:CheckBox ID="_mandatoryCheck" runat="server" Checked='<%# Bind("IsMandatory") %>' />
      </EditItemTemplate>
      <ItemTemplate>
        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("IsMandatory") %>'
          Enabled="false" />
      </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="ValidatorDetails">
      <EditItemTemplate>
        <asp:PlaceHolder ID="_detailsEdit" runat="server" OnDataBinding="OnDetailsEditDataBind"></asp:PlaceHolder>
      </EditItemTemplate>
      <ItemTemplate>
        <asp:PlaceHolder ID="_detailsView" runat="server" OnDataBinding="OnDetailsViewDataBind"></asp:PlaceHolder>
      </ItemTemplate>
    </asp:TemplateField>
  </Columns>
</asp:GridView>

