<%@ Control Language="c#" AutoEventWireup="true" Inherits="Portal.API.Module" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" enableViewState="False"%>
<img src='<%=Portal.API.Config.GetModuleVirtualPath("HitCounter").Substring(2)%>counter.aspx?src=reddigits.gif&digits=5&id=<%=ModuleRef %>.txt'/>
