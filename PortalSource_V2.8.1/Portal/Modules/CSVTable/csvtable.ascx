<%@ Control Language="c#" AutoEventWireup="true" Inherits="Portal.API.Module" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" enableViewState="False"%>
<%@ Import namespace="System.IO" %>
<script runat="server">

	private string headerLine = "";

    private string GetPath()
    {
        return ModuleDataPhysicalPath + ModuleRef + ".csv";
    }

    void Page_Load(object sender, EventArgs args)
    {
		// Random Numbers
		Random r = new Random(DateTime.Now.Millisecond);
    
        // Open file            
        if(File.Exists(GetPath()))
        {
            FileStream fs = File.OpenRead(GetPath());
            StreamReader sr = new StreamReader(fs);
            string[] lines = sr.ReadToEnd().Split('\n');
            fs.Close();
            
            headerLine = lines[0];
            
            content.DataSource = new ArrayList(lines).GetRange(1, lines.Length-1);
            content.DataBind();
        }        
    }
    
    void ItemDataBound(Object Sender, RepeaterItemEventArgs args) 
    {
		string[] cols = null;
		if (args.Item.ItemType == ListItemType.Item || args.Item.ItemType == ListItemType.AlternatingItem)
		{
			string line = (string)args.Item.DataItem;
			cols = line.Split(';');
		}
		else if(args.Item.ItemType == ListItemType.Header)
		{
			cols = headerLine.Split(';');
		}

		HtmlTableRow row = (HtmlTableRow)args.Item.FindControl("Row");
		foreach(string l in cols)
		{
			HtmlTableCell col = new HtmlTableCell();
			row.Controls.Add(col);
			col.InnerHtml = l;
		}		
    }


</script>
<table rules="all" border="1" class="List" width="99%">
	<asp:Repeater id="content" runat="server" OnItemDataBound="ItemDataBound" EnableViewState="False">
		<HeaderTemplate>
			<tr id="Row" runat="server" class="ListHeader">
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr id="Row" runat="server" class="ListLine">
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr id="Row" runat="server" class="AlternatingListLine">
			</tr>
		</AlternatingItemTemplate>
	</asp:Repeater>
</table>
