Personal .NET Web Portal
========================

Introduction
------------
This is a .NET based Web Portal for personal use. The purpose is to build a homepage easily. 
Pages are built through tabs and modules. 
The main difference to other portals is that there is no need for a database. So it is
easy to install even on free hosting servers.


Install
-------
1)   Unzip to the Web Applications folder.
1.1) If you are upgrading, take notice of the upgrading information.
2)   Create a Web Application in IIS (if not done yet).
3)   Grant to the "Data" Directory "Full Control Access" for the ASP.NET worker process (if not automatically done)
4)   Have fun!


Customizing
-----------
- Most of the customizing functionality is accessible through the browser, by login in to the portal as user 
  "admin" with the password "admin".
- Some basic settings are located in the web.config file. I.e. the User Interface language.
  To set the User Interface Language, change the value of the "globalization" tag in this file.
- Settings of the portal can be changed in the file "Data\PortalData\settings.config". 
  I.e. the title of the website, meta tags...
- Most of the layout can be modified in the file Data\Resources\Portal.css. Information to 
  CSS can be found here: http://de.selfhtml.org/css.
- The Portal header is customizable by editing the "Data\PortalData\PortalHeader.htm" file. The text 
  "{PlaceHolderLogout}" defines the position of the logout button and the welcome text.
- The Portal footer is customizable by editing the "Data\PortalData\PortalFooter.htm" file. 
 


Security
--------
Each User has Roles, minimum the built-in Role "User".
There are 4 built-in Roles:
	
	Admin - Administrator Role. May edit/view everything
	User - Signed in User
	Everyone - Signed in User or Anonymous User
	Anonymous - Not signed in User

Besides these Roles every Administrator may define own Roles ("Editor" for example).
Built-in Roles should never be deleted!!


Upgrading from the Portal 1.x versions
---------------------------------------
- Copy the file OldPortal\Users.config to NewPortal\Data\PortalData.
- Copy the file OldPortal\Portal.config to NewPortal\Data\PortalData.
- Copy the CSS file from OldPortal\portal.css to NewPortal\Data\Resources\Portal.css
- Copy the content files from the OldPortal\Modules\... to the NewPortal\Data\PortalData\... Directory.
  I.e. copy OldPortal\Modules\HtmlEdit\*.htm to NewPortal\Data\PortalData\HtmlEdit
    or copy OldPortal\Modules\Guestbook\Guestbook_*.config to NewPortal\Data\PortalData\Guestbook
       and
       copy OldPortal\Modules\Guestbook\Module_*.config to NewPortal\Data\PortalData\Guestbook
    ...
- Copy the HTML Content (manual) from the OldPortal\PortalHeader.ascx to the NewPortal\Data\PortalData\PortalHeader.htm.
- Copy the HTML Content (manual) from the OldPortal\PortalFooter.ascx to the NewPortal\Data\PortalData\PortalFooter.htm.
- Transmit the settings (manual) from the OldPortal\web.config to newPortal\Data\PortalData\Settings.config.

In future versions most of the settings will be transferred by copy the "Data" directory, to the new installation. 
So this will be much easier.


Backup
------
All the specific files are in the directory "Data". It sufficient to backup this directory.


Data Protection
---------------
If you provide data (i.e. content or file downloads) on the portal only for a certain user-group, make sure that
public access is denied for the directory "Data\PortalData" and all the subdirectories. Only the ASP.NET worker process
needs access to this directory. This is necessary to protect the configuration files against direct access over the 
internet (i.e. over http://myDomain/Data/PortalData/HtmlEdit/myConfidentialContent.htm).


Credits
-------
Developers of the Personal .NET Portal:
 - Martin Bühlmann
 - Andreas Hauri
 - Arthur Zaczek


------------------------------------------------------------------------------------------------------------------------

	
Change Log
----------
(2.8.1) Release 2.8.1  - 09 September 2008
 - Improve stability of the statistics background threads.
 
(2.8.0) Release 2.8  - 29 August 2008
 - Enhanced picture creation for thumbnails and preview in the module ImageBrowser (Optimized resizing quality and speed).
 - Configuration of the JPG compression Level in Module ImageBrowser.
 - Resolved wrong default folder image location in module ImageBrowser.
 - Resolved cache update bug in module ContentScheduler.
 - Improved error handling for statistics background thread. This Bug could avoid the start of the 
   Portal on some servers or even causes IIS-Application pool crash.
 - Possibility to customize the CSS-File for the HTML Edit Mode. (File Data\Resources\PortalEdit.css).
 - Possibility to suppress the module hierarchy (Module Path) on SubTabs. (Key "ShowSubTabPath" in the settings.config).
 - Update to FCKeditor V2.6.3
 
(2.7.1) Release 2.7.1  - 19 May 2008
 - Fixed File Connector Bug. Could not proper resolve the image url in HTML Edition modules. (Release 2.7 Bug)
 - Enhanced ContentScheduler Module: Direct link from the overview to the edit page.

(2.7.0) Release 2.7  - 03 May 2008
 - Update to FCKEditor V2.6 affects HTML mutations in various modules.
 - Improved FCKEditor HTML Editor Integration
 - Statistic Bot / Spider filtering improvement.
 - Improved error Message for file upload in module FileBrowser.
 - Fixed file upload bug in module FileBrowser
 - Removed space between Shadow and picture in ImageBrowser for some Browser (i.e. Firefox)
 
(2.6.0) Release 2.6  - 25 Mar 2008
 - Implemented Preload mechanism to load pictures in advance into the browser cache (ImageBrowser).
 - Bot / Spider filtering functionality for the statistic. (Search engine Bot and Spider requests are not logged in the statistic anymore).
 - Fixed the Login-Module default-Button bug. (Key "return" did not work on some browsers).
 - Filebrowser takes the modification date from the file, if not explicit specified.
 - FCKeditor updated to version 2.5
 - Project updated to Visual Studio 2008 (still .NET 2.0)
 - New development project structure

(2.5.0) Release 2.5  - 01 Dec 2007
 - Possibility to add delimiters between tabs (Portal.css: #TabMenuDelimiterHor)
 - Improved error handling for statistics, when access to directory is denied. This Bug could avoid the start of the 
   Portal on some servers.
 - ContentScheduler: Direct link from the preview to the edit mode added.
 - Newsticker: Asynchronous data loading for mobile clients disabled.
 - Server Information: Error handling added, when not supported by the server.
 - Replaced the navigation images of the Image Browser with language independent samples.
 
(2.4.0) Release 2.4  - 05 Sep 2007
 - New Module "FormBuilder", a module that provides the ability to build your own Web-Forms that will be submitted 
   by email. You can configure your own email layout and the data validation rules.
 - Improved statistics visit detection.

(2.3.0) Release 2.3  - 13 Jul 2007
 - New Module "ContentScheduler", a Module that switches the content based on your defined schedule
 - Meta tags are configurable in the data\portaldata\settings.config
 - FileBrowser supports translation in different languages
 - Inplace Mode for module HtmlEdit
 - Changed Mainpage from RenderTable.aspx to default.aspx
 - Removed Portal Type "Frame"
 - Browser Cache prevention is now configurable in the settings.config
 - Added Version information in version.aspx

(2.2.0) Release 2.2  - 07 Feb 2007
 - Bug in ImageBrowser with images that are smaller than the preview image size.
 - Small performance improvements in ImageBrowser.
 - Some basic statistic data recording mehtods implemented (more will follow).
 - New module StatisticViewer implemented.
 
(2.1.0) Release 2.1  - 31 Jan 2007
 - FCKeditor updated to version 2.4
 - Location of the cascading style sheet (CSS) can be defined in settings.config
 - Default CSS moved to the data directory for easy update and backup purpose.
 
(2.0.1) Bugfix for Release 2.0  - 21 Nov 2006
 - HitCounter was not working. This is fixed now.
 - FileBrowser sets MIME type when downloading a file.

(2.0.0) Release 2.0  - 02 Nov 2006
 - Portal updated for ASP.NET 2.0
 - All configuration data and userdata in a configurable directory.
 - Header and Footer in a independent HTML file.
 - Several bugfixes in Portal API
 - Security leak in tab editor fixed
 - Tab representation with a images possible.
 - Use typed datasets for module data (not module configuration)
 - Added caching for XML based Forum, Guestbook and Newsticker
 - Title of the portal (displayed in title list) is now configurable in web.config
 - FavIcon is now configuratble in web.config
 - New Features in Module ImageBrowser (Better navigation, shadows...)
 - If requested RSS-feed in Newsticker is not in cache, it will be loaded asynchronous to the requesting client.
 - New module FileBrowser (German Alpha Release)
 - New Version FckEditor 2.3.2 in Module HtmlEdit.
 - Compatibility bugs with the Webmatrix and Windows XP home.
 - Browser compatibility improved.
 - Keep session alive as long as portal page is displayed (configurable in web.config)
 - XML based forum added
 - My User module added to let a user edit his password and email
 - ...

(1.1.0) Release 1.1  - 22 Jan 2005
	- Added Caching Support
	- Bugfixes in Tab Editing
	- TabTree Module added
	- SubTabs in TabMenu added (switch with web.config key)
	- Feedback Module added
	- Mail Module added
	- Multilanguage Support added
	- Guestbook Module added 
	- Newsticker Module added 
	- ServerInformation Module added 
	- BrowserInformation Module added
	- QuickLinks: Open Link in a new Window
	- If there are no middle Modules the width of left and right Modules is 49% now
	- HtmlEdit: Use FCKeditor to edit html files
	- Installs files automatically
	- Message Control added
	- Check for Script Tags added
	
(1.0.2) Beta 2  - 16 Oct 2003
	- Forum
	- TabHttpHandler supports now URL Parameter

(1.0.1) Beta 1
	- Exeption Message: More Informations
	- throws Exceptions in DEBUG Mode
	- HasEditRight Property in Module
	- HtmlEdit Module - like the Html Module but with a Editor
	- BuildURL Method
	- Edit Portal Definition inplace (on the Tabs)

(1.0) First Release
	- Hit Counter Module
	- Validators in EditModule.ascx
	- ModuleFailed.ascx added
	- TabHttpHandler added
	- XSD Schema added to Module Config
	- News Module added
	- UrlReferrer logging
	- TabPath added

(0.6) Beta 2
	- Configuration Files renamed to .config
	- ImageBrowser Module added and adapted (http://www.codeproject.com/useritems/ImageBrowser.asp)
	- QuickLinks Module added
	- Design/Lock and Feel improved
	- "Make Installer" Batch files
	- Portal.API Assembly created (there where problems with referencing Web-Applications)
	- Read/Write Configuration Support for Modules

(0.5) Beta 1 (Bug Fix)
	- Fixed a bug in the HTML Edit Module. "The File could not be created."

(0.4) Beta 1
	- Admin Pages
	- WebSite Module added
	- TabList Module added
	- Table Module added

(0.3) 
	- Usereditor, Roleeditor
	
(0.2)
	- Security (Rolebased security added)
	- CSS

(Preview) Initial Version
	- Render Table/Frames
	- Edit Modules
	- HTML Sample Module




------------------------------------------------------------------------------------------------------------------------


Foreign Controls
----------------

TreeCtrl:
	- Originally by Iulian Iuga 
	- Found at codeproject.com (http://www.codeproject.com/useritems/TreeWeb.asp)

ImageBrowser:
	- Originally by Dan Glass (http://www.danglass.com/Web/)
	- Found at codeproject.com (http://www.codeproject.com/useritems/ImageBrowser.asp)

HitCounter:
	- by JediBaron (?) - No Full Name provided
	- Found at codeproject.com (http://www.codeproject.com/aspnet/fastcustomhitcounter.asp?target=counter)

FCKeditor:
  - by Frederico Caldeira Knabben (http://www.fckeditor.net)
  - Found at sourceforge.net (http://sourceforge.net/projects/fckeditor/)
  
JSCalendar:
  - Found at sourceforge.net (http://sourceforge.net/projects/jscalendar/)
  
Riverside Forum:
	- by Mike Puddephat
	- Project Name: Riverside Internet Forums
	- Found at codeproject.com (http://www.codeproject.com/aspnet/aspnetforums.asp?target=forum)