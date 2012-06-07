//===========================================================================
// Diese Datei wurde als Teil einer ASP.NET 2.0-Webprojektkonvertierung geändert.
// Der Klassenname wurde geändert, und die Klasse wurde so geändert, dass sie von der abstrakten 
//-Basisklasse in Datei "App_Code\Migrated\modules\forum\Stub_forum_ascx_cs.cs" erbt.
// Andere Klassen in der Webanwendung können während der Laufzeit die Code-Behind-Seite
//" mithilfe der abstrakten Basisklasse binden und darauf zugreifen.
// Die zugehörige Inhaltsseite "modules\forum\forum.ascx" wurde ebenfalls geändert und verweist auf den neuen Klassennamen.
// Weitere Informationen zu diesem Codemuster erhalten Sie unter http://go.microsoft.com/fwlink/?LinkId=46995. 
//===========================================================================
namespace Portal.Modules.Forum
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// Zusammenfassung für Forum.
    /// </summary>
    public partial class ForumControl : Forum
    {
        private StateMachine stateMachine;
        private ConfigAgent configAgent;

        #region Eigenschaften
//        public StateMachine StateMachine
//        {
        override public StateMachine StateMachine
        {
            get 
            {
                return stateMachine;
            }
        }
        #endregion
        	
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Erzeugen der State-Machine.  
            stateMachine = new StateMachine(UniqueID);

            // Wird das Modul neu geladen, so setzen wir das State-Objekt zurück.
            if (!IsPostBack)
            {
                stateMachine.ResetState();
                Session[UniqueID + "Cfg"] = null;
            }

            // Laden des entsprechenden Controls zum aktuellen Status.
            LoadCurrentCtrl();
        }

        override public  bool SetEvent(StateEvents newEvent)
        {
            bool changed = stateMachine.SetEvent(newEvent);
            if(changed)
            {
                Controls.Clear();
                LoadCurrentCtrl();
            }
            return changed;
        }


        /// <summary>
        /// Ermittelt das zentrale Konfigurationsobjekt.
        /// </summary>
        /// <returns></returns>
        override public ConfigAgent ConfigAgent
        {
            get
            {
                if (configAgent == null)
                {
                    string szSessionKey = UniqueID + "Cfg";
                    if (Session[szSessionKey] != null)
                    {
                        configAgent = (ConfigAgent)Session[szSessionKey];
                    }
                    else
                    {
                        configAgent = new ConfigAgent();
                        configAgent.ModuleConfig = (ModuleConfig)ReadConfig(typeof(ModuleConfig));
                        Session[szSessionKey] = configAgent;
                    }
                }

                configAgent.Module = this;
                return configAgent;
            }
        }

        /// <summary>
        /// Lädt das momentan aktuelle Control.
        /// </summary>
        protected void LoadCurrentCtrl()
        {
            StateMachine.StateInfos CtrlInfo = stateMachine.StateInfo;
            Control CurrCtrl = LoadControl(CtrlInfo.ControlPath);
            Controls.Add(CurrCtrl);
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
        }
        #endregion
    }
}
