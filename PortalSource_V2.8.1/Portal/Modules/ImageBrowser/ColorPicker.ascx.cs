namespace ImageBrowser
{
  using System;
  using System.Data;
  using System.Drawing;
  using System.Web;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;
  using System.Globalization;

  /// <summary>
  ///		Zusammenfassung für ColorPicker.
  /// </summary>
  public partial class ColorPicker : System.Web.UI.UserControl
  {
    static char[] hexDigits = {
         '0', '1', '2', '3', '4', '5', '6', '7',
         '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
    
    private void Page_Load(object sender, System.EventArgs e)
    {
      Page.ClientScript.RegisterClientScriptInclude(GetType(), "ColorPickerInclude", "js/ColorPicker/ColorPicker2.js");
      Page.ClientScript.RegisterClientScriptBlock(GetType(), "ColorPickerDiv", "<SCRIPT LANGUAGE=\"JavaScript\">var cp2 = new ColorPicker();cp2.writeDiv();</SCRIPT>");
      LinkPick.Attributes.Add("onClick", GetClickCode());
      LinkPick.HRef = HttpContext.Current.Request.Url + "#";
      LinkPick.Name = LinkPick.ClientID.Replace('_', '$');
    }

    public string GetClickCode()
    {
      string command = string.Format(CultureInfo.InvariantCulture, "var cp2 = new ColorPicker();cp2.select(document.forms[0].{0},'{1}');return false;", TextBoxColor.ClientID.Replace('_', '$'), LinkPick.ClientID.Replace('_', '$'));
      return command;
    }

    public Color SelectedColor
    {
      get
      {
        string color = TextBoxColor.Text;
        if (color.StartsWith("#"))
          color = color.Substring(1);

        int r = Convert.ToInt32(color.Substring(0, 2), 16);
        int g = Convert.ToInt32(color.Substring(2, 2), 16);
        int b = Convert.ToInt32(color.Substring(4, 2), 16);
        return Color.FromArgb(r, g, b);
      }
      set
      {
        TextBoxColor.Text = "#" + ColorToHexString(value);
      }
    }

    /// <summary>
    /// Convert a .NET Color to a hex string.
    /// </summary>
    /// <returns>ex: "FFFFFF", "AB12E9"</returns>
    public static string ColorToHexString(Color color)
    {
      byte[] bytes = new byte[3];
      bytes[0] = color.R;
      bytes[1] = color.G;
      bytes[2] = color.B;
      char[] chars = new char[bytes.Length * 2];
      for (int i = 0; i < bytes.Length; i++)
      {
        int b = bytes[i];
        chars[i * 2] = hexDigits[b >> 4];
        chars[i * 2 + 1] = hexDigits[b & 0xF];
      }
      return new string(chars);
    }

    public bool Enabled
    {
      get
      {
        return TextBoxColor.Enabled;
      }
      set
      {
        TextBoxColor.Enabled = value;
        LinkPick.Visible = value;
      }
    }

    protected void OnServerValidate(object source, ServerValidateEventArgs args)
    {
      if (!args.Value.StartsWith("#"))
      {
        args.IsValid = false;
        return;
      }

      try
      {
        int r = Convert.ToInt32(args.Value.Substring(1, 2), 16);
        int g = Convert.ToInt32(args.Value.Substring(3, 2), 16);
        int b = Convert.ToInt32(args.Value.Substring(5, 2), 16);
        Color col = Color.FromArgb(r, g, b);
        args.IsValid = true;
      }
      catch
      {
        args.IsValid = false;
      }
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
      this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion
}
}
