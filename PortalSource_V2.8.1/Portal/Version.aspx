<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Version.aspx.cs" Inherits="Version" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Personal .NET Portal Version Info</title>
</head>
<body>
    <form id="form1" runat="server">
    <h1>Versions</h1>
        <table>
            <tr>
                <td >Personal .NET Portal
                </td>
                <td >
                    <asp:Label ID="_portalVersion" runat="server"></asp:Label></td>
            </tr>
        <tr>
                <td >ASP.NET
                </td>
                <td >
                    <asp:Label ID="_aspVersion" runat="server"></asp:Label></td>
        </tr>
    </table>
    </form>
</body>
</html>
