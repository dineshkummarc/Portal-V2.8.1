namespace Portal.Modules.Guestbook
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Web;
    using System.Web.Mail;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Xml;
    using System.Xml.Serialization;
    using Portal;

    abstract public class Guestbook : Portal.API.Module
    {
        abstract public GuestbookData ReadData();
        abstract public string GuestbookConfigFile
        {
            get;
        }
        abstract public string GuestbookDataSchemaFile
        {
            get;
        }
        abstract public void WriteData(GuestbookData ds);
    }
}
