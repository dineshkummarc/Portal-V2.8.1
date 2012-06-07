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

namespace Portal.Modules.StatisticViewer
{
    public partial class StatisticViewerModule : Portal.StateBase.StateContainer<State, StateEvent>, StatisticViewer.IStateProcessor
    {
        #region Members

        private ConfigAgent configAgent;

        #endregion

        #region Construction / Destruction

        static StatisticViewerModule()
        {
            // Add states.
            AddState(State.Overview, "Overview.ascx");
            AddState(State.MonthlyRequests, "MonthlyRequestStatistic.ascx");

            // Add transitioins.
            AddTransition(State.Overview, StateEvent.showMonthlyRequests, State.MonthlyRequests);
            AddTransition(State.MonthlyRequests, StateEvent.showOverview, State.Overview);
        }

        #endregion

        #region IStateProcessor Member

        /// <summary>
        /// Ermittelt das Verwaltungsobjekt der Konfiguration.
        /// </summary>
        public ConfigAgent ConfigAgent
        {
            get
            {
                if (this.configAgent == null)
                {
                    string sessionKey = UniqueID + "Cfg";
                    if (Session[sessionKey] != null)
                    {
                        this.configAgent = (ConfigAgent)Session[sessionKey];
                    }
                    else
                    {
                        this.configAgent = new ConfigAgent();
                        Session[sessionKey] = this.configAgent;
                    }
                }

                return this.configAgent;
            }
            private set
            {
                this.configAgent = value;
                string sessionKey = UniqueID + "Cfg";
                Session[sessionKey] = value;
            }
        }

        #endregion

        #region Eventhandlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ConfigAgent = null; // Zurücksetzen des Config Agents, damit änderungen an der Konfiguration übernommen werden.

            // Laden des entsprechenden Ccontrols zum aktuellen Status.
            LoadCurrentCtrl(IsPostBack);
        }

        #endregion
    }
}
