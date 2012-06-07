using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Portal.Modules.StatisticViewer
{
    /// <summary>
    /// Die möglichen Stati.
    /// </summary>
    public enum State : int
    {
        Overview = 0,
        MonthlyRequests
    };

    /// <summary>
    /// Die Events welche den Status beeinflussen können.
    /// </summary>
    public enum StateEvent
    {
        showMonthlyRequests = 0,
        showOverview
    }

    /// <summary>
    /// Interface für zentrale Klasse, welche den Status verwaltet.
    /// </summary>
    public interface IStateProcessor
    {
        /// <summary>
        /// Ermittelt das zentrale Konfigurationsobjekt.
        /// </summary>
        /// <returns></returns>
        ConfigAgent ConfigAgent { get; }
    }
}
