using System;
using ImageBrowser.Entities;

namespace ImageBrowser
{
	/// <summary>
	/// Zusammenfassung für CacheUpdater.
	/// </summary>
	public class CacheUpdater
	{
    private string m_path;
    private ImageTools m_imageTools;

		public CacheUpdater(String path, ImageTools it)
		{
			m_path = path;
      m_imageTools = it;
		}

    public void UpdateAll()
    {
      // Rebuild the thumbails.     
      string path = m_imageTools.GetPath(m_path);
      DirectoryWrapper directory = new DirectoryWrapper(path, m_imageTools);
      UpdateDirectory(directory);
    }

    private void UpdateDirectory(DirectoryWrapper directory)
    {
      foreach(ImageWrapper image in directory.Images)
        image.UpdateCache();

      foreach(SubDirectoryWrapper subDir in directory.Directories)
      {
        subDir.UpdateCache();
        DirectoryWrapper SubDir = new DirectoryWrapper(m_imageTools.GetPath(directory.VirtualPath) + "\\" + 
          subDir.Caption, m_imageTools);
        UpdateDirectory(SubDir);
      }
    }
	}
}
