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
 * File Name: Uploader.cs
 * 	This is the code behind of the uploader.aspx page used for Quick Uploads.
 * 
 * File Authors:
 * 		Frederico Caldeira Knabben (fredck@fckeditor.net)
 */

using System ;
using System.Globalization ;
using System.Xml ;
using System.Web ;

namespace FredCK.FCKeditorV2
{
	public class Uploader : FileWorkerBase
	{
		protected override void OnLoad(EventArgs e)
		{
			// Get the posted file.
			HttpPostedFile oFile = Request.Files["NewFile"] ;

			// Check if the file has been correctly uploaded
			if ( oFile == null || oFile.ContentLength == 0 )
			{
				SendResults( 202 ) ;
				return ;
			}

			int iErrorNumber = 0 ;
			string sFileUrl = "" ;

			// Get the uploaded file name.
			string sFileName = System.IO.Path.GetFileName( oFile.FileName ) ;

			int iCounter = 0 ;

			while ( true )
			{
				string sFilePath = System.IO.Path.Combine( this.UserFilesDirectory, sFileName ) ;

				if ( System.IO.File.Exists( sFilePath ) )
				{
					iCounter++ ;
					sFileName = 
						System.IO.Path.GetFileNameWithoutExtension( oFile.FileName ) +
						"(" + iCounter + ")" +
						System.IO.Path.GetExtension( oFile.FileName ) ;

					iErrorNumber = 201 ;
				}
				else
				{
					oFile.SaveAs( sFilePath ) ;

					sFileUrl = this.UserFilesPath + sFileName ;
					break ;
				}
			}

			SendResults( iErrorNumber, sFileUrl, sFileName ) ;
		}

		#region SendResults Method

		private void SendResults( int errorNumber )
		{
			SendResults( errorNumber, "", "", "" ) ;
		}

		private void SendResults( int errorNumber, string fileUrl, string fileName )
		{
			SendResults( errorNumber, fileUrl, fileName, "" ) ;
		}

		private void SendResults( int errorNumber, string fileUrl, string fileName, string customMsg )
		{
			Response.Clear() ;

			Response.Write( "<script type=\"text/javascript\">" ) ;
			Response.Write( "window.parent.OnUploadCompleted(" + errorNumber + ",'" + fileUrl.Replace( "'", "\\'" ) + "','" + fileName.Replace( "'", "\\'" ) + "','" + customMsg.Replace( "'", "\\'" ) + "') ;" ) ;
			Response.Write( "</script>" ) ;

			Response.End() ;
		}

		#endregion
	}
}
