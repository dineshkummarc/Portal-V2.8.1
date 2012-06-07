using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Portal.API;
using Portal.API.Statistics.Data;
using Portal.API.Statistics.Service;

namespace Portal.Modules.StatisticViewer
{
    public partial class MonthlyRequestStatistic : Portal.StateBase.ModuleState
    {
        internal struct RequestUrlEntry
        {
            public int Rank;
            public int RequestCount;
            public double Percentage;
            public string Url;
        };

        internal struct RequestSummaryEntry
        {
            public RequestSummaryEntry(string name, double value)
            {
                this.Name = name;
                this.Value = value;
            }

            public string Name;
            public double Value;
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            // Load data of the summary statistics.
            LoadSummaryData();

            // Load data of the request statistics.
            LoadRequestData();
        }

        private void LoadSummaryData()
        {

            // Use ConfigAgent to get the month to view.
            ConfigAgent configAgent = ((IStateProcessor)Parent).ConfigAgent;

            RequestStatisticService service = (RequestStatisticService)Portal.API.Statistics.Statistic.GetService(typeof(RequestStatisticService));
            RequestUrlData urlData = service.GetRequestUrlData(Context, configAgent.Month);

            RequestSummaryData summaryData = service.GetRequestSummaryData(Context);

            // Contains all entries for the summary table.
            List<RequestSummaryEntry> entries = new List<RequestSummaryEntry>();

            // Calculate total requests of this month.
            double totalRequests = 0.0;
            foreach (RequestUrlData.TRequestUrlRow row in urlData.TRequestUrl.Rows)
            {
                totalRequests += row.RequestCount;
            }

            DataRow[] rows = summaryData.TRequestSummary.Select("Month = " + configAgent.Month.ToString(@"#MM\/dd\/yyyy#"));
            double totalVisits = 0.0;
            if (rows.Length > 0)
            {
                RequestSummaryData.TRequestSummaryRow row = (RequestSummaryData.TRequestSummaryRow)rows[0];
                totalVisits = row.VisitsCount;
            }

            // Get the number of days.
            int dayCount = 1;
            DateTime now = DateTime.Now;
            if ((now.Year == configAgent.Month.Year) && (now.Month == configAgent.Month.Month))
            {
                // Current month, so we use the days since 1st of this month.
                dayCount = now.Day;
            }
            else
            {
                dayCount = DateTime.DaysInMonth(configAgent.Month.Year, configAgent.Month.Month);
            }

            entries.Add(new RequestSummaryEntry(Language.GetText(Portal.API.Module.GetModuleControl(this), "sumRequests"), totalRequests));
            entries.Add(new RequestSummaryEntry(Language.GetText(Portal.API.Module.GetModuleControl(this), "sumVisits"), totalVisits));
            //entries.Add(new RequestSummaryEntry("", 0.0));
            entries.Add(new RequestSummaryEntry(Language.GetText(Portal.API.Module.GetModuleControl(this), "requestsPerDay"), totalRequests / (double)dayCount));
            entries.Add(new RequestSummaryEntry(Language.GetText(Portal.API.Module.GetModuleControl(this), "visitsPerDay"), totalVisits / (double)dayCount));

            repeaterSummary.DataSource = entries;
            repeaterSummary.DataBind();

            labelMonthlySummary.Text = string.Format(Language.GetText(Portal.API.Module.GetModuleControl(this), "monthlyStatisticTitle"), configAgent.Month.ToString("MMMM"), configAgent.Month.ToString("yyyy"));
        }

        private void LoadRequestData()
        {
            // Use ConfigAgent to get the month to view.
            ConfigAgent configAgent = ((IStateProcessor)Parent).ConfigAgent;

            RequestStatisticService service = (RequestStatisticService)Portal.API.Statistics.Statistic.GetService(typeof(RequestStatisticService));
            RequestUrlData data = service.GetRequestUrlData(Context, configAgent.Month);
            data.TRequestUrl.DefaultView.Sort = data.TRequestUrl.RequestCountColumn.ColumnName + " DESC";

            // Calculate the total logged requests.
            int totalRequests = 0;
            foreach (RequestUrlData.TRequestUrlRow row in data.TRequestUrl.Rows)
            {
                totalRequests += row.RequestCount;
            }

            List<RequestUrlEntry> entries = new List<RequestUrlEntry>();
            int rank = 1;
            foreach (DataRowView rv in data.TRequestUrl.DefaultView)
            {
                RequestUrlData.TRequestUrlRow row = (RequestUrlData.TRequestUrlRow)rv.Row;
                RequestUrlEntry entry = new RequestUrlEntry();
                entry.Rank = rank++;
                entry.RequestCount = row.RequestCount;
                entry.Percentage = (double)row.RequestCount / (double)totalRequests;
                entry.Url = row.Url;
                entry.Url = entry.Url.Substring(entry.Url.IndexOf('/')+1);
                entry.Url = entry.Url.Substring(entry.Url.IndexOf('/')+1);
                entry.Url = entry.Url.Substring(entry.Url.IndexOf('/'));
                entries.Add(entry);

                if (rank > 30)
                    break;
            }

            labelMonthlyRequests.Text = string.Format(Language.GetText(Portal.API.Module.GetModuleControl(this), "monthlyRequestsTitle"), rank-1, data.TRequestUrl.Rows.Count);

            repeaterTopUrls.DataSource = entries;
            repeaterTopUrls.DataBind();
        }

        public void OnSummaryItemDataBound(Object Sender, RepeaterItemEventArgs args)
        {
            HtmlTableRow row = (HtmlTableRow)args.Item.FindControl("Row");
            if (args.Item.ItemType == ListItemType.Item || args.Item.ItemType == ListItemType.AlternatingItem)
            {
                RequestSummaryEntry entry = (RequestSummaryEntry)args.Item.DataItem;
                if (!string.IsNullOrEmpty(entry.Name))
                {
                    row.Controls.Add(GetTextCell(entry.Name, "left"));
                    row.Controls.Add(GetTextCell(entry.Value.ToString("0.##", CultureInfo.CurrentUICulture), "right"));
                }
                else
                {
                    row.Controls.Add(GetTextCell("&nbsp;", "left"));
                    row.Controls.Add(GetTextCell("&nbsp;", "right"));
                    row.Attributes["Class"] = "ListHeader";
                }
            }
        }

        public void OnTopUrlsItemDataBound(Object Sender, RepeaterItemEventArgs args)
        {
            HtmlTableRow row = (HtmlTableRow)args.Item.FindControl("Row");
            if (args.Item.ItemType == ListItemType.Item || args.Item.ItemType == ListItemType.AlternatingItem)
            {
                RequestUrlEntry entry = (RequestUrlEntry)args.Item.DataItem;
                row.Controls.Add(GetTextCell(entry.Rank.ToString(CultureInfo.CurrentUICulture), "center"));
                row.Controls.Add(GetTextCell(entry.RequestCount.ToString(CultureInfo.CurrentUICulture), "right"));
                row.Controls.Add(GetTextCell(entry.Percentage.ToString("0.##%", CultureInfo.CurrentUICulture), "right"));
                row.Controls.Add(GetTextCell(entry.Url, "left"));
            }
        }

        private HtmlTableCell GetTextCell(string value, string align)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Align = align;
            cell.InnerHtml = value;
            return cell;
        }

        protected void linkButtonBack_Click(object sender, EventArgs e)
        {
            // Change the state so we get to the monthly request page.
            ProcessEvent((int)StateEvent.showOverview);
        }
}
}
