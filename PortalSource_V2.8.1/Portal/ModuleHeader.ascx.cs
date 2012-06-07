namespace Portal
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using Portal.API;

    /// <summary>
    ///	This is the Modules Header Control. Loaded by the RenderTab.ascx Control.
    ///	Makes also the EditLink invisible if the user has no rights or there is no
    ///	edit module.
    /// </summary>
    public partial class ModuleHeaderControl : ModuleHeader
    {
        protected PortalDefinition.Module ModuleDef;

        /// <summary>
        /// Initializes the Control
        /// </summary>
        /// <param name="md"></param>
        public override void SetModuleConfig(PortalDefinition.Module md)
        {
            ModuleDef = md;
            lnkEditLink.Module = md;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
          // Initially are all Edit Controls and the title invisible. 
          HeaderContainer.Visible = false;
          ModuleMenuDiv.Visible = false;
          lnkEditLink.Visible = false;
          ovm.Visible = false;
          ModuleTitle.Visible = false;

          // No Menu if in edit mode.
          if (!Helper.IsEditModuleRequested(ModuleDef))
          {
            // Set the link per default invisible
            bool showedit = false;
            if (UserManagement.HasEditRights(Page.User, ModuleDef.roles))
            {
              // User has right, continue
              if (ModuleDef.moduleSettings == null)
              {
                // no Module Settings, set visible
                showedit = true;
              }
              else
              {
                // Module has module settings
                if (ModuleDef.moduleSettings.HasEditCtrl)
                {
                  // Module has a edit control, set visible
                  showedit = true;
                }
              }
            }

            // Show Edit-Link.
            if (showedit)
            {
              HeaderContainer.Visible = true;
              ModuleMenuDiv.Visible = true;
              lnkEditLink.Visible = true;
            }

            // Ist ovm Menu visible?
            if (Page.User.IsInRole(Portal.API.Config.AdminRole))
            {
              HeaderContainer.Visible = true;
              ModuleMenuDiv.Visible = true;
              ovm.Visible = true;
              if (!showedit)  // No Edit Link.
              {
                // Remove Edit Link
                ((OverlayMenuItem)ovm.Items[0]).Visible = false;
              }
              else
                lnkEditLink.Visible = false; // Allready exist in ovm.
            }
          }

          // Show Title.
          if (ModuleDef.title.Length > 0)
          {
            HeaderContainer.Visible = true;
            ModuleTitle.InnerHtml = ModuleDef.title;
            ModuleTitle.Visible = true;
          }
        }

        protected void OnEditContent(object sender, System.EventArgs args)
        {
            Response.Redirect(Helper.GetEditLink(ModuleDef));
        }

        protected void OnEditModule(object sender, System.EventArgs args)
        {
            Response.Redirect(Helper.GetEditModuleLink(ModuleDef.reference));
        }
        protected void OnMoveUp(object sender, System.EventArgs args)
        {
            PortalDefinition pd = PortalDefinition.Load();
            PortalDefinition.Tab t = pd.GetTab(Request["TabRef"]);

            for (int i = 0; i < 3; i++)
            {
                ArrayList a = null;
                switch (i)
                {
                    case 0:
                        a = t.left;
                        break;
                    case 1:
                        a = t.middle;
                        break;
                    case 2:
                        a = t.right;
                        break;
                }

                for (int idx = 0; idx < a.Count; idx++)
                {
                    if (((PortalDefinition.Module)a[idx]).reference == ModuleDef.reference)
                    {
                        if (idx == 0) return;

                        PortalDefinition.Module m = (PortalDefinition.Module)a[idx];
                        a.RemoveAt(idx);
                        a.Insert(idx - 1, m);

                        pd.Save();
                        Server.Transfer(Request.Url.PathAndQuery);
                        return;
                    }
                }
            }
        }
        protected void OnMoveDown(object sender, System.EventArgs args)
        {
            PortalDefinition pd = PortalDefinition.Load();
            PortalDefinition.Tab t = pd.GetTab(Request["TabRef"]);

            for (int i = 0; i < 3; i++)
            {
                ArrayList a = null;
                switch (i)
                {
                    case 0:
                        a = t.left;
                        break;
                    case 1:
                        a = t.middle;
                        break;
                    case 2:
                        a = t.right;
                        break;
                }

                for (int idx = 0; idx < a.Count; idx++)
                {
                    if (((PortalDefinition.Module)a[idx]).reference == ModuleDef.reference)
                    {
                        if (idx >= a.Count - 1) return;

                        PortalDefinition.Module m = (PortalDefinition.Module)a[idx];
                        a.RemoveAt(idx);
                        a.Insert(idx + 1, m);

                        pd.Save();
                        Server.Transfer(Request.Url.PathAndQuery);
                        return;
                    }
                }
            }
        }
        protected void OnMoveLeft(object sender, System.EventArgs args)
        {
            PortalDefinition pd = PortalDefinition.Load();
            PortalDefinition.Tab t = pd.GetTab(Request["TabRef"]);

            for (int i = 1; i < 3; i++)
            {
                ArrayList a = null;
                switch (i)
                {
                    case 1:
                        a = t.middle;
                        break;
                    case 2:
                        a = t.right;
                        break;
                }

                for (int idx = 0; idx < a.Count; idx++)
                {
                    if (((PortalDefinition.Module)a[idx]).reference == ModuleDef.reference)
                    {
                        PortalDefinition.Module m = (PortalDefinition.Module)a[idx];
                        a.RemoveAt(idx);
                        if (i == 1)
                        {
                            t.left.Insert(t.left.Count, m);
                        }
                        else if (i == 2)
                        {
                            t.middle.Insert(t.middle.Count, m);
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid Column");
                        }

                        pd.Save();
                        Server.Transfer(Request.Url.PathAndQuery);
                        return;
                    }
                }
            }
        }
        protected void OnMoveRight(object sender, System.EventArgs args)
        {
            PortalDefinition pd = PortalDefinition.Load();
            PortalDefinition.Tab t = pd.GetTab(Request["TabRef"]);

            for (int i = 0; i < 2; i++)
            {
                ArrayList a = null;
                switch (i)
                {
                    case 0:
                        a = t.left;
                        break;
                    case 1:
                        a = t.middle;
                        break;
                }

                for (int idx = 0; idx < a.Count; idx++)
                {
                    if (((PortalDefinition.Module)a[idx]).reference == ModuleDef.reference)
                    {
                        PortalDefinition.Module m = (PortalDefinition.Module)a[idx];
                        a.RemoveAt(idx);
                        if (i == 0)
                        {
                            t.middle.Insert(t.middle.Count, m);
                        }
                        else if (i == 1)
                        {
                            t.right.Insert(t.right.Count, m);
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid Column");
                        }

                        pd.Save();
                        Server.Transfer(Request.Url.PathAndQuery);
                        return;
                    }
                }
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
