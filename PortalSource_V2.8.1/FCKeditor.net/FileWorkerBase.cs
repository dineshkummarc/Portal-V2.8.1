/*
 * FCKeditor - The text editor for internet
 * Copyright (C) 2003-2005 Frederico Caldeira Knabben
 * 
 * Licensed under the terms of the GNU Lesser General Public License:
 * 		http://www.opensource.org/licenses/lgpl-license.php
 * 
 * For further information visit:
 * 		http://www.fckeditor.net/
 * 
 * "Support Open Source software. What about a donation today?"
 * 
 * File Name: FileWorkerBase.cs
 * 	Base class used by the FileBrowserConnector and Uploader.
 * 
 * File Authors:
 * 		Frederico Caldeira Knabben (fredck@fckeditor.net)
 */

using System;

namespace FredCK.FCKeditorV2
{
	public abstract class FileWorkerBase : System.Web.UI.Page
	{
		private const string DEFAULT_USER_FILES_PATH = "/UserFiles/" ;

		private string sUserFilesPath ;
		private string sUserFilesDirectory ;

		protected string UserFilesPath
		{
			get
			{
				if ( sUserFilesPath == null )
				{
					// Try to get from the "Application".
					sUserFilesPath = (string)Application["FCKeditor:UserFilesPath"] ;

					// Try to get from the "Session".
					if ( sUserFilesPath == null || sUserFilesPath.Length == 0 )
					{
						sUserFilesPath = (string)Session["FCKeditor:UserFilesPath"] ;
						
						// Try to get from the Web.config file.
						if ( sUserFilesPath == null || sUserFilesPath.Length == 0 )
						{
							sUserFilesPath = System.Configuration.ConfigurationSettings.AppSettings["FCKeditor:UserFilesPath"] ;
							
							// Otherwise use the default value.
							if ( sUserFilesPath == null || sUserFilesPath.Length == 0 ) 
								sUserFilesPath = DEFAULT_USER_FILES_PATH ;

							// Try to get from the URL.
							if ( sUserFilesPath == null || sUserFilesPath.Length == 0 ) 
							{
								sUserFilesPath = Request.QueryString["ServerPath"] ;
							}
						}
					}

					// Check that the user path ends with slash ("/")
					if ( ! sUserFilesPath.EndsWith("/") )
						sUserFilesPath += "/" ;
				}
				return sUserFilesPath ;
			}
		}

		/// <summary>
		/// The absolution path (server side) of the user files directory. It 
		/// is based on the <see cref="FileWorkerBase.UserFilesPath"/>.
		/// </summary>
		protected string UserFilesDirectory
		{
			get	
			{
				if ( sUserFilesDirectory == null )
				{
					// Get the local (server) directory path translation.
					sUserFilesDirectory = Server.MapPath( this.UserFilesPath ) ;
				}
				return sUserFilesDirectory ;
			}
		}
	}
}
