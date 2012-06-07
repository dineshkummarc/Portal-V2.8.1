using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Portal.API;
using System.Web.UI;

namespace Portal
{
	/// <summary>
	///	Renders a Tab.
	/// </summary>
	public partial  class PortalTab : System.Web.UI.UserControl
	{

		private void RenderModules(HtmlTableCell td, PortalDefinition.Tab tab, ArrayList modules)
		{
			if(modules.Count == 0)
			{
				td.Visible = false;
				return;
			}
			foreach(PortalDefinition.Module md in modules)
			{
				if(UserManagement.HasViewRights(Page.User, md.roles))
				{
					md.LoadModuleSettings();

					// Initialize the Module
					Control m = null;
          bool visible = false;
					try
					{
            // Is the Edit Mode Requested?
            if (Helper.IsEditModuleRequested(md))
						{
              // Load the Edit Mode of the module.
              m = Helper.GetEditControl(Page);
            }

            if(m == null)
            {
              // Load the View of the module.
              if(md.moduleSettings == null)
						    m = LoadControl(Config.GetModuleVirtualPath(md.type) + md.type + ".ascx");
						  else
						    m = LoadControl(Config.GetModuleVirtualPath(md.type) + md.moduleSettings.ctrl);
						  
						  ((Module)m).InitModule(tab.reference, md.reference, md.type,
							  Config.GetModuleDataVirtualPath(md.type), UserManagement.HasEditRights(Page.User, md.roles));

              visible = ((Module)m).IsVisible();
            }
            else
              visible = true;

            if (visible)
						{
							// Add ModuleContainer
							HtmlGenericControl cont = new HtmlGenericControl("div");
							cont.Attributes.Add("class", "ModuleContainer");
							td.Controls.Add(cont);

							// Add Module Header
              ModuleHeader mh = (ModuleHeader)LoadControl("ModuleHeader.ascx");
							mh.SetModuleConfig(md);
              cont.Controls.Add(mh);

              // Add Module Body Container
              HtmlGenericControl bodyCont = new HtmlGenericControl("div");
              bodyCont.Attributes.Add("class", "Module");
              cont.Controls.Add(bodyCont);

              // Add Module
							HtmlGenericControl div = new HtmlGenericControl("div");
							div.Controls.Add(m);
              bodyCont.Controls.Add(div);
						}
					}
					catch(Exception e)
					{
						if(Config.ShowModuleExceptions)
						{
							throw new Exception(e.Message, e);
						}
						// Add ModuleContainer
						HtmlGenericControl cont = new HtmlGenericControl("div");
						cont.Attributes.Add("class", "ModuleContainer");
						cont.Controls.Add(m);
						td.Controls.Add(cont);

						// Add Module Header
						ModuleHeader mh = (ModuleHeader)LoadControl("ModuleHeader.ascx");
						mh.SetModuleConfig(md);
						cont.Controls.Add(mh);

						// Add Error Module
						ModuleFailed mf = (ModuleFailed)LoadControl("ModuleFailed.ascx");
						while(e != null)
						{
							mf.Message += e.GetType().Name + ": ";
							mf.Message += e.Message + "<br>";
							e = e.InnerException;
						}

						mf.Message = mf.Message.Remove(mf.Message.Length - 4, 4);

						HtmlGenericControl div = new HtmlGenericControl("div");
						div.Attributes.Add("class", "Module");
						div.Controls.Add(mf);
						cont.Controls.Add(div);
					}
				}
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
      TabMainMenuContainer.Visible = Page.User.IsInRole(Portal.API.Config.AdminRole);

			PortalDefinition pd = PortalDefinition.Load();
			PortalDefinition.Tab t = PortalDefinition.CurrentTab;

			if(t.left.Count == 0)
			{
				TabMiddle.Style["margin-left"] = "0em";
			}
			if(t.right.Count == 0)
			{
				TabMiddle.Style["margin-right"] = "0em";
			}

      bool showPath = true;
      try
      {
        if (!bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["ShowSubTabPath"], out showPath))
          showPath = true;
      }
      catch (System.Configuration.ConfigurationErrorsException) { }
      if (showPath)
        TabPath.Controls.Add(LoadControl("TabPath.ascx"));

		}

		protected void OnAddTab(object sender, EventArgs args)
		{
			PortalDefinition pd = PortalDefinition.Load();
			PortalDefinition.Tab t = PortalDefinition.Tab.Create();

			pd.GetTab(Request["TabRef"]).tabs.Add(t);

			pd.Save();

			Response.Redirect(Helper.GetEditTabLink(t.reference));
		}
		protected void OnEditTab(object sender, EventArgs args)
		{
			Response.Redirect(Helper.GetEditTabLink());
		}
		protected void OnDeleteTab(object sender, EventArgs args)
		{
			PortalDefinition.Tab t = PortalDefinition.CurrentTab;
			PortalDefinition.DeleteTab(t.reference);

			if(t.parent == null)
			{
				Response.Redirect(Helper.GetTabLink(""));
			}
			else
			{
				Response.Redirect(Helper.GetTabLink(t.parent.reference));
			}
		}
		protected void OnAddLeftModule(object sender, EventArgs args)
		{
			PortalDefinition pd = PortalDefinition.Load();
			// Do NOT use GetCurrentTab! You will be unable to save
			PortalDefinition.Tab t = pd.GetTab(Request["TabRef"]);

			PortalDefinition.Module m = PortalDefinition.Module.Create();
			t.left.Add(m);

			pd.Save();

			Response.Redirect(Helper.GetEditModuleLink(m.reference));
		}
		protected void OnAddMiddleModule(object sender, EventArgs args)
		{
			PortalDefinition pd = PortalDefinition.Load();
			// Do NOT use GetCurrentTab! You will be unable to save
			PortalDefinition.Tab t = pd.GetTab(Request["TabRef"]);

			PortalDefinition.Module m = PortalDefinition.Module.Create();
			t.middle.Add(m);

			pd.Save();

			Response.Redirect(Helper.GetEditModuleLink(m.reference));
		}
		protected void OnAddRightModule(object sender, EventArgs args)
		{
			PortalDefinition pd = PortalDefinition.Load();
			// Do NOT use GetCurrentTab! You will be unable to save
			PortalDefinition.Tab t = pd.GetTab(Request["TabRef"]);

			PortalDefinition.Module m = PortalDefinition.Module.Create();
			t.right.Add(m);

			pd.Save();
		
			Response.Redirect(Helper.GetEditModuleLink(m.reference));
		}

		override protected void CreateChildControls()
		{
			PortalDefinition.Tab tab = PortalDefinition.CurrentTab;
			if(tab == null) return;

			if(UserManagement.HasViewRights(Page.User, tab.roles))
			{
				// Render
				RenderModules(TabLeft, tab, tab.left);
				RenderModules(TabMiddle, tab, tab.middle);
				RenderModules(TabRight, tab, tab.right);

        // Special case, if the middle part is not visible.
        if (!TabMiddle.Visible && TabLeft.Visible && TabRight.Visible)
        {
          TabLeft.Attributes["class"] = "TabLeftTwoCol";
          TabRight.Attributes["class"] = "TabRightTwoCol";
        }
			}
		}

		override protected void OnInit(EventArgs e)
		{
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			InitializeComponent();
			base.OnInit(e);
			EnsureChildControls();
		}
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
	}
}
