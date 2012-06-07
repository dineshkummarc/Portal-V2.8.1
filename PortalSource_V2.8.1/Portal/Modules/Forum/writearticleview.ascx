<%@ Control Language="c#" Inherits="Portal.Modules.Forum.WriteArticleView" CodeFile="WriteArticleView.ascx.cs" %>
<%@ Register TagPrefix="portal" assembly="Portal.API" Namespace="Portal.API.Controls" %>
<div id="m_Toolbar" runat="server"><portal:linkbutton id="m_SubmitBtn" runat="server" CssClass="LinkButton" LanguageRef="Submit" onclick="m_SubmitBtn_Click" />&nbsp;|
  <portal:linkbutton id="m_CancelBtn" runat="server" CssClass="LinkButton" LanguageRef="Back" onclick="m_CancelBtn_Click" /></div>
<table width="100%">
  <tr>
    <td style="WIDTH: 59px" vAlign="top"><portal:Label runat="server" id="NameLbl" LanguageRef="Name" /></td>
    <td><asp:textbox id="m_NameTB" runat="server" Width="216px"></asp:textbox>
      <asp:TextBox id="m_UserIdTB" runat="server" Visible="False"></asp:TextBox></td>
  </tr>
  <tr>
    <td style="WIDTH: 59px" vAlign="top"><portal:Label runat="server" id="EmailLbl" LanguageRef="Email" /></td>
    <td><asp:textbox id="m_EmailTB" runat="server" Width="216px"></asp:textbox></td>
  </tr>
  <tr>
    <td style="WIDTH: 59px" vAlign="top"><portal:Label runat="server" id="TitleLbl" LanguageRef="Title" /></td>
    <td><asp:textbox id="m_TitleTB" runat="server" Width="326px"></asp:textbox></td>
  </tr>
  <tr>
    <td style="WIDTH: 59px" vAlign="top"><portal:Label runat="server" id="TextLbl" LanguageRef="Text" /></td>
    <td><asp:textbox id="m_TextTB" runat="server" Width="100%" Height="200px" TextMode="MultiLine"></asp:textbox></td>
  </tr>
  <tr>
    <td style="WIDTH: 59px" vAlign="middle"><portal:Label runat="server" id="SmileyLbl" LanguageRef="Smiley" /></td>
    <td>
      <a href="./" name="emoticon" onclick="return InsertText(':)')"><img alt="Insert a smile :)" border="0" src="Modules/Forum/images/emoticons/smiley_smile.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(';)')"><img alt="Insert a wink ;)" border="0" src="Modules/Forum/images/emoticons/smiley_wink.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(';P')"><img alt="Stick out your tongue ;P" border="0" src="Modules/Forum/images/emoticons/smiley_tongue.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':-D')"><img alt="Insert a big grin :-D" border="0" src="Modules/Forum/images/emoticons/smiley_biggrin.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':(')"><img alt="Insert a frown :(" border="0" src="Modules/Forum/images/emoticons/smiley_frown.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':((')"><img alt="Insert tears :((" border="0" src="Modules/Forum/images/emoticons/smiley_cry.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':-O')"><img alt="Insert blush :-O" border="0" src="Modules/Forum/images/emoticons/smiley_redface.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':rolleyes:')"><img alt="Insert roll eyes :rolleyes:" border="0" src="Modules/Forum/images/emoticons/smiley_rolleyes.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':laugh:')"><img alt="Insert laugh :laugh:" border="0" src="Modules/Forum/images/emoticons/smiley_laugh.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':mad:')"><img alt="Insert mad :mad:" border="0" src="Modules/Forum/images/emoticons/smiley_mad.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':confused:')"><img alt="Insert confused :confused:" border="0" src="Modules/Forum/images/emoticons/smiley_confused.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':|')"><img alt="Insert Unimpressed :|" border="0" src="Modules/Forum/images/emoticons/smiley_line.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(' X| ')"><img alt="Insert unwell X| " border="0" src="Modules/Forum/images/emoticons/smiley_dead.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':suss:')"><img alt="Insert a suspicious :suss:" border="0" src="Modules/Forum/images/emoticons/smiley_suss.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':cool:')"><img alt="Insert cool :cool:" border="0" src="Modules/Forum/images/emoticons/smiley_cool.gif"></a>
      <a href="./" name="emoticon" onclick="return InsertText(':eek:')"><img alt="Insert an eek! :eek:" border="0" src="Modules/Forum/images/emoticons/smiley_eek.gif"></a>
    </td>
  </tr>
</table>
