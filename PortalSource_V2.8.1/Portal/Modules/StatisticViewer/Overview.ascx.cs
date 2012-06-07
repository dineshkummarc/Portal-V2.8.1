using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Portal.API.Statistics.Data;
using Portal.API.Statistics.Service;
using Portal.API;

namespace Portal.Modules.StatisticViewer
{
  public partial class Overview : Portal.StateBase.ModuleState
  {
    // Structure that represents one line of the login table.
    internal struct LoginEntry
    {
      public string Login;
      public string FirstName;
      public string LastName;
      public int LoginCount;
      public DateTime LastLogin;
    };

    // Structure that represents one line of the request table.
    internal struct RequestSummaryEntry
    {
      public DateTime Month;
      public double RequestsPerDay;
      public double VisitsPerDay;
      public int TotalRequests;
      public int TotalVisits;
    };

    protected void Page_Load(object sender, EventArgs e)
    {
      // Load data of the login statistics.
      LoadLoginData();

      // Load data of the request statistics.
      LoadRequestData();
    }

    private void LoadRequestData()
    {
      RequestStatisticService service = (RequestStatisticService)Portal.API.Statistics.Statistic.GetService(typeof(RequestStatisticService));
      RequestSummaryData data = service.GetRequestSummaryData(Context);
      //data.TRequestSummary.DefaultView.Sort = "Month DESC";

      List<RequestSummaryEntry> entries = new List<RequestSummaryEntry>();
      // Max. number of months to display will be set to 12 (1 year).
      int monthCount = data.TRequestSummary.Rows.Count > 12 ? 12 : data.TRequestSummary.Rows.Count;
      for (int monthIndex = 0; monthIndex < monthCount; ++monthIndex)
      {
        RequestSummaryData.TRequestSummaryRow row = (RequestSummaryData.TRequestSummaryRow)data.TRequestSummary.Rows[(data.TRequestSummary.Rows.Count-1)-monthIndex];
        RequestSummaryEntry entry = new RequestSummaryEntry();
        entry.Month = row.Month;
        entry.RequestsPerDay = GetRequestsPerDay(row.Month, row.RequestCount);
        entry.VisitsPerDay = GetVisitsPerDay(row.Month, row.VisitsCount);
        entry.TotalRequests = row.RequestCount;
        entry.TotalVisits = row.VisitsCount;
        entries.Add(entry);
      }

      repeaterOverview.DataSource = entries;
      repeaterOverview.DataBind();

      // Show the error message, if an error is occured in the service.
      Exception ex = service.ConsumeLastException();
      if (ex != null)
      {
        _errorMsg.Visible = true;
        _errorMsg.Text = ex.Message;
      }
    }

    private void LoadLoginData()
    {
      LoginStatisticService service = (LoginStatisticService)Portal.API.Statistics.Statistic.GetService(typeof(LoginStatisticService));
      LoginStatisticData data = service.GetData(Context);

      List<LoginEntry> entries = new List<LoginEntry>();
      foreach (LoginStatisticData.TLoginRow row in data.TLogin.Rows)
      {
        Portal.API.Users.UserRow user = UserManagement.Users.FindById(row.UserId);
        LoginEntry entry = new LoginEntry();
        if (null != user)
        {
          // User was found, so use the actual user infos to show.
          entry.Login = user.login;
          entry.FirstName = user.SafeFirstName;
          entry.LastName = user.SafeSurName;
        }
        else
        {
          // User was deleted, so use cached infos.
          entry.Login = row.Login;
          entry.FirstName = row.FirstName;
          entry.LastName = row.LastName;
        }
        entry.LoginCount = row.LoginCount;
        entry.LastLogin = row.LastLogin;
        entries.Add(entry);
      }

      repeaterLogin.DataSource = entries;
      repeaterLogin.DataBind();
    }

    public void OnLoginItemDataBound(Object Sender, RepeaterItemEventArgs args)
    {
      HtmlTableRow row = (HtmlTableRow)args.Item.FindControl("Row");
      if (args.Item.ItemType == ListItemType.Item || args.Item.ItemType == ListItemType.AlternatingItem)
      {
        LoginEntry entry = (LoginEntry)args.Item.DataItem;
        row.Controls.Add(GetTextCell(entry.Login, "left"));
        row.Controls.Add(GetTextCell(entry.FirstName, "left"));
        row.Controls.Add(GetTextCell(entry.LastName, "left"));
        row.Controls.Add(GetTextCell(entry.LastLogin.ToString(CultureInfo.CurrentUICulture), "left"));
        row.Controls.Add(GetTextCell(entry.LoginCount.ToString(CultureInfo.CurrentUICulture), "right"));
      }
      else if (args.Item.ItemType == ListItemType.Header)
      {
        row.Controls.Add(GetTextCell(Language.GetText(Portal.API.Module.GetModuleControl(this), "login"), "left"));
        row.Controls.Add(GetTextCell(Language.GetText(Portal.API.Module.GetModuleControl(this), "firstName"), "left"));
        row.Controls.Add(GetTextCell(Language.GetText(Portal.API.Module.GetModuleControl(this), "lastName"), "left"));
        row.Controls.Add(GetTextCell(Language.GetText(Portal.API.Module.GetModuleControl(this), "lastLogin"), "left"));
        row.Controls.Add(GetTextCell(Language.GetText(Portal.API.Module.GetModuleControl(this), "loginCount"), "left"));
      }
    }

    public void OnRequestItemDataBound(Object Sender, RepeaterItemEventArgs args)
    {
      HtmlTableRow row = (HtmlTableRow)args.Item.FindControl("Row");
      if (args.Item.ItemType == ListItemType.Item || args.Item.ItemType == ListItemType.AlternatingItem)
      {
        RequestSummaryEntry entry = (RequestSummaryEntry)args.Item.DataItem;
        row.Controls.Add(GetMonthButtonCell(entry.Month, "left"));
        row.Controls.Add(GetTextCell(entry.RequestsPerDay.ToString("0.#", CultureInfo.CurrentUICulture), "right"));
        row.Controls.Add(GetTextCell(entry.VisitsPerDay.ToString("0.#", CultureInfo.CurrentUICulture), "right"));
        row.Controls.Add(GetTextCell(entry.TotalRequests.ToString(CultureInfo.CurrentUICulture), "right"));
        row.Controls.Add(GetTextCell(entry.TotalVisits.ToString(CultureInfo.CurrentUICulture), "right"));
      }
    }

    private double GetVisitsPerDay(DateTime dateTime, int visitsCountq)
    {
      DateTime now = DateTime.Now;
      if ((now.Year == dateTime.Year) && (now.Month == dateTime.Month))
      {
        return visitsCountq / (double)now.Day;
      }
      else
      {
        return visitsCountq / (double)GetDayCountForMonth(dateTime);
      }
    }

    private double GetRequestsPerDay(DateTime dateTime, int requestCount)
    {
      DateTime now = DateTime.Now;
      if ((now.Year == dateTime.Year) && (now.Month == dateTime.Month))
      {
        return requestCount / (double)now.Day;
      }
      else
      {
        return requestCount / (double)GetDayCountForMonth(dateTime);
      }
    }

    private int GetDayCountForMonth(DateTime dateTime)
    {
      return DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
    }

    private string GetMonthString(DateTime dateTime)
    {
      return dateTime.ToString("MMMM yyyy");
    }

    private HtmlTableCell GetMonthButtonCell(DateTime dateTime, string align)
    {
      LinkButton button = new LinkButton();
      button.Command += new CommandEventHandler(OnMonthClick);
      button.Text = GetMonthString(dateTime);
      button.CommandArgument = dateTime.ToString(CultureInfo.InvariantCulture);

      HtmlTableCell cell = new HtmlTableCell();
      cell.Controls.Add(button);
      cell.Align = align;
      return cell;
    }

    private HtmlTableCell GetTextCell(string value, string align)
    {
      HtmlTableCell cell = new HtmlTableCell();
      cell.Align = align;
      cell.InnerHtml = value;
      return cell;
    }

    void OnMonthClick(object sender, CommandEventArgs e)
    {
      // Use ConfigAgent to store the month to view.
      ConfigAgent configAgent = ((IStateProcessor)Parent).ConfigAgent;
      configAgent.Month = DateTime.Parse((string)e.CommandArgument, CultureInfo.InvariantCulture);

      // Change the state so we get to the monthly request page.
      ProcessEvent((int)StateEvent.showMonthlyRequests);
    }
  }
}
