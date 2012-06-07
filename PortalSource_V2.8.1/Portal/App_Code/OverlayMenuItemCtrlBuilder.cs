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

    public class OverlayMenuItemCtrlBuilder : System.Web.UI.ControlBuilder
    {
        public override Type GetChildControlType(String tagName,
            IDictionary attributes)
        {
            if (String.Compare(tagName, "MenuItem", true) == 0)
            {
                return typeof(OverlayMenuItem);
            }
            if (String.Compare(tagName, "SeparatorItem", true) == 0)
            {
                return typeof(OverlayMenuSeparatorItem);
            }
            return null;
        }
    }
}