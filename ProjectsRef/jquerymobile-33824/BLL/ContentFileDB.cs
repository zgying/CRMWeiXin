/*  
 *  Copyright © 2012 Matthew David Elgert - mdelgert@yahoo.com
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; either version 2.1 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA 
 * 
 *  URLS
 *  http://www.telerik.com/community/forums/aspnet-ajax/file-explorer/bind-filesystemcontentprovider-to-database.aspx
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Configuration;
using System.Web;
using Telerik.Web.UI.Widgets;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using DAL;

namespace BLL
{
    //FileBrowserContentProvider
    public class ContentFileDB : FileBrowserContentProvider
    {
        private string _itemHandlerPath;
        private string ItemHandlerPath
        {
            get
            {
                return _itemHandlerPath;
            }
        }

        /// <summary>
        /// Initializes an instance of the ContentFileDB class
        /// </summary>
        /// <param name="context">The HttpContext object</param>
        /// <param name="searchPatterns">A string array of search patterns</param>
        /// <param name="viewPaths">A string array of view paths</param>
        /// <param name="uploadPaths">A string array of upload paths</param>
        /// <param name="deletePaths">A string array of delete paths</param>
        /// <param name="selectedUrl">A string representing the selected URL</param>
        /// <param name="selectedItemTag">A string representing the selected Item Tag</param>
        public ContentFileDB(HttpContext context, string[] searchPatterns, string[] viewPaths, string[] uploadPaths, string[] deletePaths, string selectedUrl, string selectedItemTag)
            : base(context, searchPatterns, viewPaths, uploadPaths, deletePaths, selectedUrl, selectedItemTag)
        {
            //_itemHandlerPath = ConfigurationManager.AppSettings["Telerik.WebControls.EditorExamples.ItemHandler"];
            _itemHandlerPath = ConfigurationManager.AppSettings["ContentFileDB"];
            if (_itemHandlerPath.StartsWith("~/"))
            {
                _itemHandlerPath = HttpContext.Current.Request.ApplicationPath + _itemHandlerPath.Substring(1);
            }

            //Received the following message in VS commented out to test if still an issue
            //Warning	2	'Telerik.Web.UI.Widgets.FileBrowserContentProvider.SelectedItemTag' is obsolete: 'This property is no longer used'	C:\codeplex\jQueryMobile\jQueryMobile\BLL\ContentFileDB.cs	73	17	BLL
            //if (SelectedItemTag != null && SelectedItemTag != string.Empty)
            //{
            //    SelectedItemTag = ExtractPath(RemoveProtocolNameAndServerName(SelectedItemTag));
            //}
        }

        /// <summary>
        /// Gets the value of the Portal ID for the current session or returns 1 if
        /// there is no Portal ID selected for the current session
        /// </summary>
        public int PortalID
        {
            get
            {
                object temp = HttpContext.Current.Session["PortalID"]; 
                return temp == null ? 1 : (int)temp;
            }
        }

        /// <summary>
        /// Gets the Url for the item's virtual path
        /// </summary>
        /// <param name="virtualItemPath">The virtual path</param>
        /// <returns>Returns a string</returns>
        private string GetItemUrl(string virtualItemPath)
        {
            string escapedPath = Context.Server.UrlEncode(virtualItemPath);
            return string.Format("{0}?path={1}", ItemHandlerPath, escapedPath);
        }

        private string ExtractPath(string itemUrl)
        {
            if (itemUrl == null)
            {
                return string.Empty;
            }
            if (itemUrl.StartsWith(_itemHandlerPath))
            {
                return itemUrl.Substring(GetItemUrl(string.Empty).Length);
            }
            return itemUrl;
        }

        private string GetName(string path)
        {
            if (String.IsNullOrEmpty(path) || path == "/")
            {
                return string.Empty;
            }
            path = VirtualPathUtility.RemoveTrailingSlash(path);
            return path.Substring(path.LastIndexOf('/') + 1);
        }
        
        private string GetDirectoryPath(string path)
        {
            return path.Substring(0, path.LastIndexOf('/') + 1);
        }

        private bool IsChildOf(string parentPath, string childPath)
        {
            return childPath.StartsWith(parentPath);
        }

        private string CombinePath(string path1, string path2)
        {
            if (path1.EndsWith("/"))
            {
                return string.Format("{0}{1}", path1, path2);
            }
            if (path1.EndsWith("\\"))
            {
                path1 = path1.Substring(0, path1.Length - 1);
            }
            return string.Format("{0}/{1}", path1, path2);
        }

        private DirectoryItem[] GetChildDirectories(string path)
        {
            List<DirectoryItem> directories = new List<DirectoryItem>();
            try
            {
                DataRow[] childRows = GetChildDirectoryRows(path);
                int i = 0;
                while (i < childRows.Length)
                {
                    DataRow childRow = childRows[i];
                    string name = childRow["Name"].ToString();
                    string itemFullPath = VirtualPathUtility.AppendTrailingSlash(CombinePath(path, name));

                    DirectoryItem newDirItem = new DirectoryItem(name,
                                                                 string.Empty,
                                                                 itemFullPath,
                                                                 string.Empty,
                                                                 GetPermissions(itemFullPath),
                                                                 null, // The files are added in ResolveDirectory() 
                                                                 null // Directories are added in ResolveRootDirectoryAsTree()
                                                                 );

                    directories.Add(newDirItem);
                    i = i + 1;
                }
                return directories.ToArray();
            }
            catch (Exception)
            {
                return new DirectoryItem[] { };
            }
        }

        private FileItem[] GetChildFiles(string _path)
        {
            try
            {
                DataRow[] childRows = GetChildFileRows(_path);
                List<FileItem> files = new List<FileItem>();

                for (int i = 0; i < childRows.Length; i++)
                {
                    DataRow childRow = childRows[i];
                    string name = childRow["Name"].ToString();
                    if (IsExtensionAllowed(System.IO.Path.GetExtension(name)))
                    {
                        string itemFullPath = CombinePath(_path, name);

                        FileItem newFileItem = new FileItem(name,
                                                            Path.GetExtension(name),
                                                            (int)childRow["Size"],
                                                            itemFullPath,
                                                            GetItemUrl(itemFullPath),
                                                            string.Empty,
                                                            GetPermissions(itemFullPath)
                                                            );

                        files.Add(newFileItem);
                    }
                }
                return files.ToArray();
            }
            catch (Exception)
            {
                return new FileItem[] { };
            }
        }

        private bool IsExtensionAllowed(string extension)
        {
            return Array.IndexOf(SearchPatterns, "*.*") >= 0 || Array.IndexOf(SearchPatterns, "*" + extension.ToLower()) >= 0;
        }

        /// <summary>
        /// Checks Upload permissions
        /// </summary>
        /// <param name="path">Path to an item</param>
        /// <returns></returns>
        private bool HasUploadPermission(string path)
        {
            foreach (string uploadPath in this.UploadPaths)
            {
                if (path.StartsWith(uploadPath, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks Delete permissions
        /// </summary>
        /// <param name="path">Path to an item</param>
        /// <returns></returns>
        private bool HasDeletePermission(string path)
        {
            foreach (string deletePath in this.DeletePaths)
            {
                if (path.StartsWith(deletePath, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the permissions for the provided path
        /// </summary>
        /// <param name="pathToItem">Path to an item</param>
        /// <returns></returns>
        private PathPermissions GetPermissions(string pathToItem)
        {
            PathPermissions permission = PathPermissions.Read;
            permission = HasUploadPermission(pathToItem) ? permission | PathPermissions.Upload : permission;
            permission = HasDeletePermission(pathToItem) ? permission | PathPermissions.Delete : permission;

            return permission;
        }


        /// <summary>
        /// Loads a root directory with given path, where all subdirectories 
        /// contained in the SelectedUrl property are loaded
        /// </summary>
        /// <remarks>
        /// The ImagesPaths, DocumentsPaths, etc properties of RadEditor
        /// allow multiple root items to be specified, separated by comma, e.g.
        /// Photos,Paintings,Diagrams. The FileBrowser class calls the 
        /// ResolveRootDirectoryAsTree method for each of them.
        /// </remarks>
        /// <param name="path">the root directory path, passed by the FileBrowser</param>
        /// <returns>The root DirectoryItem or null if such does not exist</returns>
        public override DirectoryItem ResolveRootDirectoryAsTree(string path)
        {
            DirectoryItem returnValue = new DirectoryItem(GetName(path),
                                                            GetDirectoryPath(path),
                                                            path,
                                                            string.Empty,
                                                            GetPermissions(path),
                                                            null, // The files  are added in ResolveDirectory()
                                                            GetChildDirectories(path));
            return returnValue;
        }

        /// <summary>
        /// Loads a root directory with given path
        /// </summary>
        /// <param name="path">the root directory path, passed by the FileBrowser</param>
        /// <returns>The root DirectoryItem or null if such does not exist</returns>
        public override DirectoryItem ResolveDirectory(string path)
        {
            DirectoryItem[] directories = GetChildDirectories(path);

            DirectoryItem returnValue = new DirectoryItem(
                GetName(path),
                VirtualPathUtility.AppendTrailingSlash(GetDirectoryPath(path)),
                path,
                string.Empty,
                GetPermissions(path),
                GetChildFiles(path),
                null // Directories are added in ResolveRootDirectoryAsTree()
                );

            return returnValue;
        }

        /// <summary>
        /// Returns the file name from the url
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>Returns a string containing the file name or an empty string</returns>
        public override string GetFileName(string url)
        {
            return GetName(url);
        }

        /// <summary>
        /// Gets the directory path from the current url
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>Returns a string containing the directory path</returns>
        public override string GetPath(string url)
        {
            return GetDirectoryPath(ExtractPath(RemoveProtocolNameAndServerName(url)));
        }

        /// <summary>
        /// Returns a stream object created from the url
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>Returns a MemoryStream object or null</returns>
        public override Stream GetFile(string url)
        {
            byte[] content = GetContent(ExtractPath(RemoveProtocolNameAndServerName(url)));
            if (!Object.Equals(content, null))
            {
                return new MemoryStream(content);
            }
            return null;
        }

        public override string StoreBitmap(Bitmap bitmap, string url, ImageFormat format)
        {
            string newItemPath = ExtractPath(RemoveProtocolNameAndServerName(url));
            string name = GetName(newItemPath);
            string _path = GetPath(newItemPath);
            string tempFilePath = System.IO.Path.GetTempFileName();
            bitmap.Save(tempFilePath);
            byte[] content;
            using (FileStream inputStream = File.OpenRead(tempFilePath))
            {
                long size = inputStream.Length;
                content = new byte[size];
                inputStream.Read(content, 0, Convert.ToInt32(size));
            }

            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }

            CreateItem(name, _path, "image/gif", false, content.Length, content);
            return string.Empty;
        }

        public override string MoveFile(string path, string newPath)
        {
            try
            {
                bool destFileExists = this.IsItemExists(newPath);
                if (destFileExists)
                    return "A file with the same name exists in the destination folder";

                string newFileName = GetName(newPath);
                string destinationDirPath = newPath.Substring(0, newPath.Length - newFileName.Length);

                if (destinationDirPath.Length == 0)
                {
                    destinationDirPath = path.Substring(0, path.LastIndexOf("/"));
                }
                // destination directory row
                DataRow newPathRow = GetItemRow(destinationDirPath);
                UpdateItemPath(path, newFileName, (int)newPathRow["FileID"]);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return string.Empty;
        }

        public override string MoveDirectory(string path, string newPath)
        {
            if (newPath.EndsWith("/")) newPath = newPath.Remove(newPath.Length - 1, 1);

            bool destFileExists = this.IsItemExists(newPath);
            if (destFileExists)
                return "A directory with the same name exists in the destination folder";

            return MoveFile(path, newPath);
        }

        public override string CopyFile(string path, string newPath)
        {
            try
            {
                bool destFileExists = this.IsItemExists(newPath);
                if (destFileExists)
                    return "A file with the same name exists in the destination folder";

                string newFileName = GetName(newPath);
                string newFilePath = newPath.Substring(0, newPath.Length - newFileName.Length);
                if (newFilePath.Length == 0)
                {
                    newFilePath = path.Substring(0, path.LastIndexOf("/"));
                }
                DataRow oldPathRow = GetItemRow(path);

                CreateItem(newFileName, newFilePath, (string)oldPathRow["MimeType"], (bool)oldPathRow["IsDirectory"], (int)oldPathRow["Size"], GetContent(path));

                if ((bool)oldPathRow["IsDirectory"])
                {
                    //copy all child items of the folder as well
                    FileItem[] files = GetChildFiles(path);
                    foreach (FileItem childFile in files)
                    {
                        CopyFile(childFile.Tag, CombinePath(newPath, childFile.Name));
                    }
                    //copy all child folders as well
                    DirectoryItem[] subFolders = GetChildDirectories(path);
                    foreach (DirectoryItem subFolder in subFolders)
                    {
                        CopyFile(subFolder.Tag, CombinePath(newPath, subFolder.Name));
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return string.Empty;
        }

        public override string CopyDirectory(string path, string newPath)
        {
            string[] pathParts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (pathParts.Length > 0)
            {
                string fullNewPath = CombinePath(newPath, pathParts[pathParts.Length - 1]);
                bool destFileExists = this.IsItemExists(fullNewPath);
                if (destFileExists)
                    return "A file with the same name exists in the destination folder";

                return CopyFile(path, fullNewPath);
            }
            else
                return "Old path is invalid";
        }

        public override string StoreFile(Telerik.Web.UI.UploadedFile file, string path, string name, params string[] arguments)
        {
            int fileLength = Convert.ToInt32(file.InputStream.Length);
            byte[] content = new byte[fileLength];
            file.InputStream.Read(content, 0, fileLength);
            string fullPath = CombinePath(path, name);
            if (!Object.Equals(GetItemRow(fullPath), null))
            {
                ReplaceItemContent(fullPath, content);
            }
            else
            {
                CreateItem(name, path, file.ContentType, false, fileLength, content);
            }
            return string.Empty;
        }

        public override string DeleteFile(string path)
        {
            DeleteItem(path);
            return string.Empty;
        }

        public override string DeleteDirectory(string path)
        {
            DeleteItem(path);
            return string.Empty;
        }

        public override string CreateDirectory(string path, string name)
        {
            CreateItem(name, path, string.Empty, true, 0, new byte[0]);
            return string.Empty;
        }

        public override bool CanCreateDirectory
        {
            get
            {
                return true;
            }
        }


        public static DataTable _data;
        public static DataTable Data
        {

            get
            {

                if (_data == null)
                {
                    _data = new DataTable();
                    _data.Columns.Add("FileID", typeof(int));
                    _data.Columns.Add("Name", typeof(string));
                    _data.Columns.Add("ParentID", typeof(int));
                    _data.Columns.Add("MimeType", typeof(string));
                    _data.Columns.Add("IsDirectory", typeof(Boolean));
                    _data.Columns.Add("Size", typeof(int));

                    //using (EntitiesContext context = new EntitiesContext())
                    //{
                    //    foreach (var I in context.ContentFiles)
                    //    {
                    //        _data.Rows.Add(I.FileID, I.Name, I.ParentID, I.MimeType, I.IsDirectory, I.Size);
                    //    }
                    //}

                    using (EntitiesContext context = new EntitiesContext())
                    {
                        var query = from q in context.ContentFiles
                                    //where q.PortalID == Portals.PortalID
                                    select q;

                        foreach (var I in query)
                        {
                            _data.Rows.Add(I.FileID, I.Name, I.ParentID, I.MimeType, I.IsDirectory, I.Size);
                        }

                    }

                }

                return _data;

            }

        }

        public static int GetItemId(string path)
        {
            DataRow itemRow = GetItemRow(path);
            if (itemRow == null)
            {
                return -1;
            }
            return (int)itemRow["FileID"];
        }

        public DataRow[] GetChildDirectoryRows(string path)
        {
            return Data.Select(string.Format("ParentID={0} AND IsDirectory", GetItemId(path)));
        }

        public DataRow[] GetChildFileRows(string path)
        {
            return Data.Select(string.Format("ParentID={0} AND NOT IsDirectory", GetItemId(path)));
        }

        public static DataRow GetItemRow(string path)
        {
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            string[] names = path.Split('/');
            //Start search in root;
            DataRow searchedRow = null;
            int itemId = 0;
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                DataRow[] rows = Data.Select(string.Format("Name='{0}' AND (ParentID={1} OR {1}=0)", name.Replace("'", "''"), itemId));
                if (rows.Length == 0)
                {
                    return null;
                }
                searchedRow = rows[0];
                itemId = (int)searchedRow["FileID"];
            }
            return searchedRow;
        }

        public static byte[] GetContent(string path)
        {
            int fileID = GetItemId(path);
            if (fileID <= 0)
            {
                return null;
            }

            byte[] returnValue = null;

            using (EntitiesContext context = new EntitiesContext())
            {
                //var result = context.ContentFiles.FirstOrDefault(i => i.FileID == fileID & i.PortalID == Portals.PortalID);
                //var result = context.ContentFiles.FirstOrDefault(i => i.FileID == fileID);
                var result = context.ContentFiles.Single(i => i.FileID == fileID);
                returnValue = result.FileContent;
            }

            return returnValue;
        }

        public void CreateItem(string name, string path, string mimeType, bool isDirectory, long size, byte[] content)
        {
            int parentId = GetItemId(path);
            if (parentId < 0)
            {
                return;
            }

            using (EntitiesContext context = new EntitiesContext())
            {
                ContentFile I = new ContentFile
                {
                    Name = name,
                    ParentID = parentId,
                    MimeType = mimeType,
                    IsDirectory = isDirectory,
                    Size = int.Parse(size.ToString()),
                    FileContent = content,
                    PortalID = this.PortalID
                };
                context.ContentFiles.AddObject(I);
                context.SaveChanges();
            }

            _data = null;

        }

        public void DeleteItem(string path)
        {
            //TODO: cascade delete when removing a non-empty folder
            int itemId = GetItemId(path);
            if (itemId <= 0)
            {
                return;
            }

            using (EntitiesContext context = new EntitiesContext())
            {
                //dynamic I = context.ContentFiles.FirstOrDefault(i => i.FileID == itemId & i.PortalID == Portals.PortalID);
                dynamic I = context.ContentFiles.FirstOrDefault(i => i.FileID == itemId);
                context.ContentFiles.DeleteObject(I);
                context.SaveChanges();
            }

            _data = null;
        }

        public DataRow[] GetAllDirectoryRows(string path)
        {
            DataRow rootRow = GetItemRow(path);
            if (rootRow == null)
            {
                return new DataRow[] { };
            }
            ArrayList allDirectoryRows = new ArrayList();
            allDirectoryRows.Add(rootRow);
            FillChildDirectoryRows((int)rootRow["FileID"], allDirectoryRows);
            return (DataRow[])allDirectoryRows.ToArray(typeof(DataRow));
        }

        private void FillChildDirectoryRows(int parentId, ArrayList toFill)
        {
            DataRow[] childRows = Data.Select(string.Format("ParentID={0} AND IsDirectory", parentId));
            foreach (DataRow childRow in childRows)
            {
                toFill.Add(childRow);
                FillChildDirectoryRows((int)childRow["FileID"], toFill);
            }
        }

        public string GetItemPath(DataRow row)
        {
            if (row["ParentID"] is DBNull)
            {
                return string.Format("{0}/", row["Name"]);
            }
            int parentId = (int)row["ParentID"];
            DataRow[] parents = Data.Select(string.Format("ItemID={0}", parentId));
            if (parents.Length == 0)
            {
                return string.Format("/{0}", row["Name"]);
            }
            return GetItemPath(parents[0]) + string.Format("{0}/", row["Name"]);
        }

        public void ReplaceItemContent(string path, byte[] content)
        {
            int fileID = GetItemId(path);
            if (fileID < 0)
            {
                return;
            }

            using (EntitiesContext context = new EntitiesContext())
            {
                //var I = context.ContentFiles.FirstOrDefault(i => i.FileID == fileID & i.PortalID == Portals.PortalID);
                var I = context.ContentFiles.FirstOrDefault(i => i.FileID == fileID);
                I.FileContent = content;
                context.SaveChanges();
            }

        }
        public void UpdateItemPath(string path, string newName, int newParentId)
        {
            int fileID = GetItemId(path);
            if (fileID < 0)
            {
                return;
            }

            using (EntitiesContext context = new EntitiesContext())
            {
                //var I = context.ContentFiles.FirstOrDefault(i => i.FileID == fileID & i.PortalID == Portals.PortalID);
                var I = context.ContentFiles.FirstOrDefault(i => i.FileID == fileID);
                I.Name = newName;
                I.ParentID = newParentId;
                context.SaveChanges();
            }

            _data = null;

        }

        /// <summary>
        /// Checks whether the item(file or folder) exists on the server
        /// </summary>
        /// <param name="pathToItem">Path to file or folder</param>
        /// <returns></returns>
        public bool IsItemExists(string pathToItem)
        {
            DataRow row = GetItemRow(pathToItem);

            return row != null;
        }
    }
}

