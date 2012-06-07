using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.IO;
using System.Globalization;
using System.Collections.Generic;

namespace ImageBrowser.Entities
{
    /// <summary>
    /// Holds all the information of the contents of a directoty
    /// </summary>
    public class DirectoryWrapper
    {
        private List<SubDirectoryWrapper> directories = new List<SubDirectoryWrapper>();
        private List<ImageWrapper> images = new List<ImageWrapper>();
        private string directory;
        private ImageTools imageTools = null;
        private DirectorySettingsHandler dirSettings = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir">Directory to hold contents of</param>
        public DirectoryWrapper(string dir, ImageTools imgTools)
        {
            imageTools = imgTools;
            directory = dir;

            dirSettings = new DirectorySettingsHandler(imageTools.cfg.PictureRootDirectory + "/" + directory, Name);

            // add the sub-directories
            string[] subDirectories = Directory.GetDirectories(imageTools.cfg.PictureRootDirectory + "/" + directory);
            foreach (string s in subDirectories)
            {
                string[] path = s.Replace("\\", "/").Split('/');

                if (path[path.Length - 1] != "thumbs" && path[path.Length - 1] != "webpics" && path[path.Length - 1][0] != '_')
                {
                    directories.Add(imageTools.GetSubDirectoryWrapper(directory + "/" + path[path.Length - 1]));
                }
            }

            // add pictures
            string[] files = Directory.GetFiles(imageTools.cfg.PictureRootDirectory + "/" + directory);
            foreach (string s in files)
            {
                string[] path = s.Replace(@"\", "/").Split('/');
                string fileName = path[path.Length - 1];
                if (fileName[0] != '_')
                {
                    string extension = null;
                    if (fileName.IndexOf(".") > 0)
                    {
                        string[] parts = fileName.Split('.');
                        extension = parts[parts.Length - 1];
                    }
                    if (extension == null) continue;

                    extension = extension.ToLower(CultureInfo.InvariantCulture);

                    if (extension == "jpg" ||
                        extension == "png" ||
                        extension == "gif")
                    {
                        images.Add(imageTools.GetImageWrapper(directory + "/" + fileName, dirSettings));
                    }
                }
            }
        }

        /// <summary>
        /// Subdirectories
        /// </summary>
        public List<SubDirectoryWrapper> Directories
        {
            get
            {
                return directories;
            }
        }

        /// <summary>
        /// images
        /// </summary>
        public List<ImageWrapper> Images
        {
            get
            {
                return images;
            }
        }

        /// <summary>
        /// The name of this directory
        /// </summary>
        public string Name
        {
            get
            {
                string[] paths = directory.Replace(@"\", "/").Split('/');
                return paths[paths.Length - 1];
            }
        }

        public string VirtualPath
        {
            get
            {
                return imageTools.cfg.PictureVirtualDirectory + directory;
            }
        }

        /// <summary>
        /// The Caption of the directory.
        /// </summary>
        public string Caption
        {
            get
            {
                return dirSettings.DirectoryCaption;
            }
            set
            {
                dirSettings.DirectoryCaption = value;
            }
        }

        /// <summary>
        /// The Tooltip of the directory.
        /// </summary>
        public string Tooltip
        {
            get
            {
                return dirSettings.DirectoryTooltip;
            }
            set
            {
                dirSettings.DirectoryTooltip = value;
            }
        }


        /// <summary>
        /// The Description of the directory.
        /// </summary>
        public string Description
        {
            get
            {
                return dirSettings.DirectoryDescription;
            }
            set
            {
                dirSettings.DirectoryDescription = value;
            }
        }
    }
}
