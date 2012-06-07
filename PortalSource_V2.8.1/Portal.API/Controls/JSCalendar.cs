using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace Portal.API.Controls
{
  /// <summary>
  /// Wrapper for the JsCalendar (http://sourceforge.net/projects/jscalendar/)
  /// Based on the Wrapper of Stefan Holmberg, at http://www.aspcode.net/articles/l_en-US/t_default/ASP.NET/ASP.NET2.0/Controls/ASP.NET-Date-and-time-popup-calendar/category_59.aspx
  /// Modified by Andreas Hauri.
  /// </summary>
  [DefaultProperty("Text")]
  [ToolboxData("<{0}:JSCalendar runat=server></{0}:JSCalendar>"), Designer("Portal.API.Controls.JSCalendarControlDesigner")]
  public class JSCalendar : TextBox
  {
    protected HiddenField m_oField;
    protected Image m_TriggerButton;
    //protected TextBox m_oField;

    protected override void CreateChildControls()
    {
      EnableViewState = true;
      m_oField = new HiddenField();
      //m_oField = new TextBox();
      m_oField.EnableViewState = true;
      m_oField.ID = "cal_" + ClientID;
      m_oField.Value = "0";
      //m_oField.Value = "0";
      Controls.Add(m_oField);
      m_TriggerButton = new Image();
      m_TriggerButton.ID = "cal_img_" + ClientID;
      m_TriggerButton.ImageUrl = TriggerImagePath;
      m_TriggerButton.Style.Add("cursor", "hand");
      Controls.Add(m_TriggerButton);
      base.CreateChildControls();
    }

    protected override void OnLoad(EventArgs e)
    {
      if (Page.IsPostBack)
      {
        EnsureChildControls();
        //string sWhat = Page.Request[m_oField.ID];
        //DateTimeValue = new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(m_oField.Text));
      }
      base.OnLoad(e);
    }


    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [Localizable(true)]
    public string PathToCalendarScriptDir
    {
      get
      {
        return "~/jscalendar/";
      }
    }

    public bool Clock24Hour
    {
      get
      {
        if (ShowTime == false)
          return false;
        if (OutputFormat.IndexOf("tt") < 0)
          return true;
        return false;
      }
    }



    private static string GetDaFormat(string sFormat)
    {
      //Convert C# format to what's wanted from the JSCalendar
      string sWhat = sFormat.Replace("yyyy", "%Y");
      
      Regex regex = new Regex("(?<![y%])y+(?!y)");
      sWhat = regex.Replace(sWhat, "%y");
      
      regex = new Regex("(?<![M%])M+(?!M)");
      sWhat = regex.Replace(sWhat, "%m");

      regex = new Regex("(?<![d%])d+(?!d)");
      sWhat = regex.Replace(sWhat, "%d");

      regex = new Regex("(?<![H%])H+(?!H)");
      sWhat = regex.Replace(sWhat, "%H");

      regex = new Regex("(?<![h%])h+(?!h)");
      sWhat = regex.Replace(sWhat, "%I");

      regex = new Regex("(?<![m%])m+(?!m)");
      sWhat = regex.Replace(sWhat, "%M");

      regex = new Regex("(?<![s%])s+(?!s)");
      sWhat = regex.Replace(sWhat, "%S");

      regex = new Regex("(?<![t%])t+(?!t)");
      sWhat = regex.Replace(sWhat, "%p");

//       sWhat = sWhat.Replace("yy", "%y");
//       sWhat = sWhat.Replace("MM", "%m");
//       sWhat = sWhat.Replace("dd", "%d");
//       sWhat = sWhat.Replace("HH", "%H");
//       sWhat = sWhat.Replace("mm", "%M");
//       sWhat = sWhat.Replace("ss", "%S");
//       sWhat = sWhat.Replace("tt", "%p");
      return sWhat;
      /*
       * %a  abbreviated weekday name  
%A  full weekday name  
%b  abbreviated month name  
%B  full month name  
%C  century number  
%d  the day of the month ( 00 .. 31 )  
%e  the day of the month ( 0 .. 31 )  
%H  hour ( 00 .. 23 )  
%I  hour ( 01 .. 12 )  
%j  day of the year ( 000 .. 366 )  
%k  hour ( 0 .. 23 )  
%l  hour ( 1 .. 12 )  
%m  month ( 01 .. 12 )  
%M  minute ( 00 .. 59 )  
%n  a newline character  
%p  ``PM'' or ``AM''  
%P  ``pm'' or ``am''  
%S  second ( 00 .. 59 )  
%s  number of seconds since Epoch (since Jan 01 1970 00:00:00 UTC)  
%t  a tab character  
%U, %W, %V  the week number 
%u  the day of the week ( 1 .. 7, 1 = MON ) 
%w  the day of the week ( 0 .. 6, 0 = SUN ) 
%y  year without the century ( 00 .. 99 ) 
%Y  year including the century ( ex. 1979 ) 
%%  a literal % character  

       * */
    }


    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue(false)]
    [Localizable(true)]
    public bool ShowTime
    {
      get
      {
        if(null == ViewState["ShowTime"])
          return false;
        else
          return (bool)ViewState["ShowTime"];
      }

      set
      {
        ViewState["ShowTime"] = value;
      }
    }


    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [Localizable(true)]
    public string OutputFormat
    {
      get
      {
        string format;
        if(ShowTime)
        {
          format = Portal.API.Config.DateTimeFormat;
        }
        else
        {
          format = Portal.API.Config.DateFormat;
        }

        return format;
      }
    }

    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [Localizable(true)]
    public override bool ReadOnly
    {
      get
      {
        return true;
      }

    }

    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [Localizable(true)]
    public string TriggerImagePath
    {
      get
      {
        String s = (String)ViewState["TriggerImagePath"];
        return ((s == null) ? "~/PortalImages/pickdate.gif" : s);
      }

      set
      {
        ViewState["TriggerImagePath"] = value;
      }
    }
    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [Localizable(true)]
    public string DisplayArea
    {
      get
      {
        String s = (String)ViewState["DisplayArea"];
        return ((s == null) ? "" : s);
      }

      set
      {
        ViewState["DisplayArea"] = value;
      }
    }





    [Bindable(true)]
    [Category("Appearance")]
    [Localizable(true)]
    public DateTime DateTimeValue
    {
      get
      {
        EnsureChildControls();
        if (string.IsNullOrEmpty(m_oField.Value))
          return DateTime.MinValue;
        if (m_oField.Value == "0")
          return DateTime.MinValue;

        string sYear = m_oField.Value.Substring(0, 4);
        string sMonth = m_oField.Value.Substring(5, 2);
        string sDay = m_oField.Value.Substring(8, 2);
        string sHour = m_oField.Value.Substring(11, 2);
        string sMin = m_oField.Value.Substring(15, 2);
        string sSec = m_oField.Value.Substring(19, 2);
        if (ShowTime == false)
        {
          sHour = "0";
          sMin = "0";
          sSec = "0";
        }
        return new DateTime(Convert.ToInt32(sYear),
            Convert.ToInt32(sMonth),
            Convert.ToInt32(sDay),
            Convert.ToInt32(sHour),
            Convert.ToInt32(sMin),
            Convert.ToInt32(sSec));
      }

      set
      {
        EnsureChildControls();

        //m_oField.Text = value.ToString("yyyyMMddhhmmss");
        m_oField.Value = value.ToString("yyyy-MM-dd HH::mm::ss");
      }
    }

    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [Localizable(true)]
    public string LanguageFile
    {
      get
      {
        // Language File Override in the settings.
        string langFile = System.Configuration.ConfigurationManager.AppSettings["SpecificLanguageFile"];
        if (string.IsNullOrEmpty(langFile))
        {
          // Apply default language file.
          string defaultLangCode = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Substring(0, 2);
          langFile = "~/jscalendar/lang/calendar-" + defaultLangCode + ".js";
        }
        return langFile;
      }
    }

    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue(false)]
    [Localizable(true)]
    public bool ShowWeekNumber
    {
      get
      {
        if (ViewState["ShowWeekNumber"] == null)
          return false;
        return (bool)ViewState["ShowWeekNumber"];
      }

      set
      {
        ViewState["ShowWeekNumber"] = value;
      }
    }


    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Localizable(true)]
    public bool SingleClick
    {
      get
      {
        if (ViewState["SingleClick"] == null)
          return true;
        return (bool)ViewState["SingleClick"];
      }

      set
      {
        ViewState["SingleClick"] = value;
      }
    }




    protected override void OnPreRender(EventArgs e)
    {
      // Register CSS. Make sure that it's not allready reggistered.
      bool isRegistered = false;
      foreach (Control ctrl in Page.Header.Controls)
      {
        if (ctrl.ID == "JsCalendarCss")
        {
          isRegistered = true;
          break;
        }
      }
      if (!isRegistered)
      {
        // Retrieve Calendar CSS File.
        string calCss = System.Configuration.ConfigurationManager.AppSettings["CalendarTheme"];
        if (string.IsNullOrEmpty(calCss))
          calCss = "jscalendar/calendar-blue.css";

        string tag = "<link rel=\"stylesheet\" type=\"text/css\" media=\"all\" href=\"" + calCss + "\" />";
        Control cssInclude = new LiteralControl(tag);
        cssInclude.ID = "JsCalendarCss";
        Page.Header.Controls.Add(cssInclude);
      }

      EnsureChildControls();
      if (Page.ClientScript.IsClientScriptBlockRegistered("calendar123") == false)
      {
        string sPath = ResolveUrl(PathToCalendarScriptDir);
        if (sPath.EndsWith("/") == false)
          sPath = sPath + "/";
        string sScript = "<script type=\"text/javascript\" src=\"" + sPath + "calendar.js\"></script>" + Environment.NewLine;
        //Language
        string sLanguageFile = ResolveUrl(LanguageFile);
        sScript += "<script type=\"text/javascript\" src=\"" + sLanguageFile + "\"></script>" + Environment.NewLine;
        sScript += "<script type=\"text/javascript\" src=\"" + sPath + "calendar-setup.js\"></script>" + Environment.NewLine;

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "calendar123", sScript);
      }
      if (DateTimeValue.Year < 1900)
        Text = string.Empty;
      else
        Text = DateTimeValue.ToString(OutputFormat);
      base.OnPreRender(e);
    }
    protected override void Render(HtmlTextWriter output)
    {
      base.Render(output);
      foreach (Control c in Controls)
        c.RenderControl(output);

      string sScript = "<script type=\"text/javascript\">" + Environment.NewLine;
      sScript += "    Calendar.setup({" + Environment.NewLine;
      if (DisplayArea.Length > 0)
      {
        sScript += "displayArea        :    \"" + DisplayArea + "\"," + Environment.NewLine;
        sScript += "daFormat        :    \"" + GetDaFormat(OutputFormat) + "\"," + Environment.NewLine;
      }
      sScript += "ifFormat       :    \"%Y-%m-%d %H::%M::%S\"," + Environment.NewLine;
      sScript += "showsTime      :    " + ShowTime.ToString().ToLower() + ",   " + Environment.NewLine;
      sScript += "timeFormat      :    " + (Clock24Hour == true ? "24" : "12") + ",   " + Environment.NewLine;
      sScript += "weekNumbers      :    " + ShowWeekNumber.ToString().ToLower() + ",   " + Environment.NewLine;
      sScript += "button         :    \"" + m_TriggerButton.ClientID + "\"," + Environment.NewLine;
      sScript += "singleClick    :    " + SingleClick.ToString().ToLower() + "," + Environment.NewLine;
      //if ( DateTimeValue.Year > 1970  )
      //    sScript += "date    :    new Date(" + DateTimeValue.Year.ToString() + "," + (DateTimeValue.Month-1).ToString() + "," + DateTimeValue.Day.ToString() + "," + (DateTimeValue.Hour-1).ToString() + "," + DateTimeValue.Minute.ToString() + "," + DateTimeValue.Second.ToString() + ")," + Environment.NewLine;
      //else
      sScript += "inputField  : \"" + m_oField.ClientID + "\"," + Environment.NewLine;
      sScript += "onSelect         :    " + ClientID + "_onSelect," + Environment.NewLine;
      sScript += "step    :    1" + Environment.NewLine;
      sScript += "   });" + Environment.NewLine;

      sScript += "function " + ChangeHandler + "(calendar, date) {" + Environment.NewLine;
      sScript += "var input_field = document.getElementById(\"" + ClientID + "\");" + Environment.NewLine;
      sScript += "var dDat = Date.parseDate(date, '%Y-%m-%d %H::%M::%S');";
      sScript += "input_field.value = dDat.print(\"" + GetDaFormat(OutputFormat) + "\");" + Environment.NewLine;
      sScript += "input_field = document.getElementById(\"" + m_oField.ClientID + "\");" + Environment.NewLine;
      sScript += "input_field.value = date;" + Environment.NewLine;
      sScript += "};" + Environment.NewLine;
      sScript += "</script>" + Environment.NewLine;

      output.Write(sScript);

    }


    /// <summary>
    /// The Name of the Javascript function that applys changes.
    /// </summary>
    protected string ChangeHandler
    {
      get
      {
        return ClientID + "_onSelect";
      }
    }

    /// <summary>
    /// Gets the code for a Clientside Javascript Change Handler.
    /// </summary>
    /// <param name="setTime"></param>
    /// <returns></returns>
    public string GetChangeHandlerCode(DateTime setTime)
    {
      string jsCalDate = setTime.ToString("yyyy-MM-dd HH::mm::ss");
      return ChangeHandler + "(null,'" + jsCalDate + "')";
    }
  }


  class JSCalendarControlDesigner : System.Web.UI.Design.ControlDesigner
  {
    public override string GetDesignTimeHtml()
    {
      System.IO.StringWriter sw = new System.IO.StringWriter();
      HtmlTextWriter tw = new HtmlTextWriter(sw);

      JSCalendar oCal = Component as JSCalendar;
      TextBox oBox = new TextBox();
      oBox.Text = oCal.DateTimeValue.ToString(oCal.OutputFormat);
      oBox.CssClass = oCal.CssClass;
      oBox.RenderControl(tw);

      return sw.ToString();

    }
  }
}
