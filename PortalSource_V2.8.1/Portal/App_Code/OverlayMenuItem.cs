namespace Portal
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;

    public class OverlayMenuItem
    {
        private string text = "";
        public string Text
        {
            get
            {
                if (languageRef == "")
                    return text;
                return Portal.API.Language.GetText(languageRef);
            }
            set { text = value; languageRef = ""; }
        }
        private string languageRef = "";
        public string LanguageRef
        {
            get { return languageRef; }
            set { languageRef = value; }
        }

        private string icon = "";
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        private int menuItemIndex = -1;
        public int MenuItemIndex
        {
            get { return menuItemIndex; }
            set { menuItemIndex = value; }
        }

        public event EventHandler Click = null;
        public void InvokeClick()
        {
            if (Click != null)
            {
                Click(this, new EventArgs());
            }
        }

        private bool visible = true;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
    }
}