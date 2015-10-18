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
 *  https://dotnetzip.codeplex.com/wikipage?title=CS-Examples&referringTitle=Examples
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

using System;
using Ionic.Zip;

namespace BLL
{
    /// <summary>
    /// The BLL.Theme class
    /// </summary>
    /// <remarks>
    /// The purpose of the Theme class is to allow the user to upload a zipped themeroller file.
    /// The Theme class saves the zipped file, unzips the contents of the file, then deletes
    /// the zipped file.
    /// </remarks>
    public static class Theme
    {
        /// <summary>
        /// Gets the system path to the appliation's App_Themes\ThemeRoller folder
        /// </summary>
        public static string Path
        {
            get
            {
                return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("bin", "App_Themes\\ThemeRoller\\").Replace(@"\\", @"\").Replace(@"file:\", "");
            }
        }

        /// <summary>
        /// Deletes the zipped file
        /// </summary>
        public static void DeleteZippedFile(string fileName)
        {
            try
            {
                System.IO.File.Delete(Path + fileName);
            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Extracts the contents of the zipped file
        /// </summary>
        /// <param name="fileName">The name of the zipped file</param>
        /// <returns>Returns a value indicating whether the contents were unzipped successfully</returns>
        public static bool ExtractZippedData(string fileName)
        {
            try
            {
                string pathToFile = Path + fileName;
                string unpackDirectory = pathToFile.Replace(".zip", "");

                using (ZipFile zip1 = ZipFile.Read(pathToFile))
                {
                    foreach (ZipEntry e in zip1)
                    {
                        e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
