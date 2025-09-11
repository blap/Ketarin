using System;
using System.IO;
using System.Security;
using Microsoft.Win32;

namespace CDBurnerXP.IO
{
    public static partial class PathEx
    {
        private static bool m_CancelCopy = false;
        
        #region Properties

        /// <summary>
        /// Allows to cancel the copy process.
        /// </summary>
        public static bool CancelCopy
        {
            set { m_CancelCopy = value; }
        }

        /// <summary>
        /// Determines whether or not hidden files are shown in Windows Explorer.
        /// </summary>
        public static bool ShowHiddenFiles
        {
            get
            {
                try
                {
                    RegistryKey? key = Registry.CurrentUser;
                    key = key?.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced");
                    object? valueObj = key?.GetValue("Hidden");
                    int value = (valueObj != null && valueObj is int) ? Convert.ToInt32(valueObj) : 0;
                    return (value == 1);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion


        public static event EventHandler<GenericEventArgs<string>>? FileCopying;

        /// <summary>
        /// Determines whether a drive with the given drive letter is set in the unitmask.
        /// </summary>
        public static bool IsDriveInUnitmask(int bitmask, char driveLetter)
        {
            int pos = char.ToLower(driveLetter) - 'a';
            if (pos < 0) return false;

            return ((bitmask & (int)Math.Pow(2, pos)) > 0);
        }

        /// <summary>
        /// Determines whether or not a given drive is hidden per group policies.
        /// </summary>
        public static bool IsDriveHidden(char driveLetter)
        {
            try
            {
                RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                if (key == null) return false;

                object? noDrivesObj = key.GetValue("NoDrives");
                object? noViewOnDrivesObj = key.GetValue("NoViewOnDrives");
                
                int noDrives = (noDrivesObj != null && noDrivesObj is int) ? Convert.ToInt32(noDrivesObj) : 0;
                int noViewOnDrives = (noViewOnDrivesObj != null && noViewOnDrivesObj is int) ? Convert.ToInt32(noViewOnDrivesObj) : 0;

                return IsDriveInUnitmask(noDrives, driveLetter) || IsDriveInUnitmask(noViewOnDrives, driveLetter);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a local temp directory within %temp%/FromRemovableMedia/{GUID}
        /// and replaces the source path with the new directory.
        /// </summary>
        /// <returns>The original path on failure, otherwise the new temp dir</returns>
        public static string CacheLocally(string tempPath, string sourcePath)
        {
            try
            {
                string newPath = tempPath;
                newPath = Path.Combine(newPath, Guid.NewGuid().ToString());
                // Make sure that we still have the same directory name
                newPath = Path.Combine(newPath, Path.GetFileName(sourcePath));

                CopyFolder(sourcePath, newPath);
                return newPath;
            }
            catch (Exception)
            {
                return sourcePath;
            }
        }

        /// <summary>
        /// Tries to determine whether or not a source path
        /// is on a removable device. On failure, it will return
        /// false.
        /// </summary>
        public static bool IsRemovableSource(string sourcePath)
        {
            try
            {
                string? rootPath = Path.GetPathRoot(sourcePath);
                if (string.IsNullOrEmpty(rootPath)) return false;
                
                DriveInfo info = new DriveInfo(rootPath ?? string.Empty);
                return (info.DriveType == DriveType.Removable || info.DriveType == DriveType.CDRom);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to determine whether or not a source path
        /// is a network location. On failure, it will return
        /// false.
        /// </summary>
        public static bool IsNetworkLocation(string sourcePath)
        {
            try
            {
                if (sourcePath.StartsWith("\\\\")) return true;
                
                string? rootPath = Path.GetPathRoot(sourcePath);
                if (string.IsNullOrEmpty(rootPath)) return false;
                
                DriveInfo info = new DriveInfo(rootPath ?? string.Empty);
                return (info.DriveType == DriveType.Network);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Copies files or directories recursively to a
        /// new location. Both arguments must either be of 
        /// type directory or file.
        /// </summary>
        public static void CopyFolder(string sourceFolder, string destFolder)
        {
            // Only a file?
            if (File.Exists(sourceFolder))
            {
                string? targetDir = Path.GetDirectoryName(destFolder);
                Directory.CreateDirectory(targetDir ?? string.Empty);
                if (FileCopying != null) FileCopying(null, new GenericEventArgs<string>(sourceFolder));
                File.Copy(sourceFolder, destFolder, true);
                return;
            }

            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                if (m_CancelCopy) break;

                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                if (FileCopying != null) FileCopying(null, new GenericEventArgs<string>(file));
                File.Copy(file, dest);
            }

            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                if (m_CancelCopy) break;

                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }
        /// <summary>
        /// Returns the full absolute path for a given location and file.
        /// </summary>
        /// <param name="basePath">File or folder to use as base</param>
        /// <param name="file">Absolute or relative path to a file</param>
        public static string GetFullPath(string basePath, string file)
        {
            if (Path.IsPathRooted(file))
            {
		file = PathEx.FixDirectorySeparator(file);

                if (file.StartsWith("\\\\"))
                {
                    // UNC path
                    return file;
                }
                else if (file.StartsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    // If path is rooted relatively, combine with playlist file location
                    string? rootPath = Path.GetPathRoot(basePath);
                    return Path.Combine(rootPath ?? string.Empty, file.TrimStart(Path.DirectorySeparatorChar));
                }
                else
                {
                    return file;
                }
            }
            else
            {
                string? baseDir = Path.GetDirectoryName(basePath);
                return Path.Combine(baseDir ?? string.Empty, file);
            }
        }

        public static bool IsFolderEmpty(string sPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(sPath);
            if (dirInfo.GetFiles().Length == 0 & dirInfo.GetDirectories().Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string AppendToFileName(string append, string filename)
        {
            string? path = Path.GetDirectoryName(filename);
            string ext = Path.GetExtension(filename);
            filename = Path.GetFileNameWithoutExtension(filename);
            return Path.Combine(path ?? string.Empty, filename + append + ext);
        }

        public static string ReplaceInvalidFileNameChars(string text)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char check in invalidChars)
            {
                text = text.Replace(check, '_');
            }
            return text;
        }

        public static bool IsPathDirectory(string strPath)
        {
            try
            {
                FileInfo FileProps = new FileInfo(strPath);
                if (((int)FileProps.Attributes > -1) && (FileProps.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns whether or not the file has the given extension.
        /// </summary>
        public static bool IsExtension(string fileName, string ext)
        {
            return string.Compare(Path.GetExtension(fileName), "." + ext.TrimStart('.'), true) == 0;
        }

        public static string GetLastPathItem(string strPath)
        {
            return Path.GetFileName(strPath);
        }

        public static string FixDirectorySeparator(string sPath)
        {
            if (Path.DirectorySeparatorChar == '/')
                return sPath.Replace('\\', Path.DirectorySeparatorChar);

            return sPath.Replace('/', Path.DirectorySeparatorChar);
        }

        public static string QualifyPath(string sPath)
        {
            sPath = FixDirectorySeparator(sPath);
            if (sPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                return sPath;
            }
            else
            {
                return sPath + Path.DirectorySeparatorChar;
            }
        }

        public static bool FileBusy(string sPath)
        {
            FileStream oFileStream;
            try
            {
                oFileStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                if (ex is IOException)
                {
                    return true;
                }
                else if (ex is UnauthorizedAccessException)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            oFileStream.Close();
            return false;
        }

        public static bool FileBusy(string sPath, ref string sInfo)
        {
            FileStream oFileStream;
            try
            {
                oFileStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            }
            catch (IOException ex)
            {
                sInfo = ex.Message;
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                sInfo = ex.Message;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            oFileStream.Close();
            return false;
        }

        /// <summary>
        /// Simple function to check for some invalid chars in a file name
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>No path!</returns>
        public static bool IsInvalidFileName(string filename)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char check in invalidChars)
            {
                if (filename.Contains(check.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetFileTypeDescription(string ext)
        {
            try
            {
                RegistryKey? rk = Registry.ClassesRoot.OpenSubKey(ext);
                if (rk?.GetValue("") is string type)
                {
                    RegistryKey? exta = rk.OpenSubKey(type);
                    if (exta == null)
                    {
                        return "*." + ext;
                    }

                    if (exta.GetValue("") is string desc)
                    {
                        return desc;
                    }

                    RegistryKey? exta2 = rk.OpenSubKey((string?)exta.GetValue("") ?? string.Empty);
                    if (exta2 == null)
                    {
                        return "*." + ext;
                    }

                    if (exta2.GetValue("") is string desc2)
                    {
                        return desc2;
                    }
                }
            }
            catch (Exception)
            {
            }

            return "*." + ext;
        }

        public static string GetFileTypeFromExt(string ext)
        {
            RegistryKey rk = Registry.ClassesRoot;
            try
            {
                RegistryKey? exta = rk.OpenSubKey(ext);
                if (exta == null)
                {
                    return "";
                }
                if (exta.GetValue("") == null)
                {
                    return "";
                }
                RegistryKey? exta2 = rk.OpenSubKey((string)exta.GetValue("")!);
                if (exta2 == null)
                {
                    return "";
                }
                return exta2.GetValue("") as string ?? string.Empty;
            }
            catch (SecurityException)
            {
                return "";
            }
        }

        public static void TryDeleteDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path)) return;

                foreach (string subDir in Directory.GetDirectories(path))
                {
                    TryDeleteDirectory(subDir);
                }

                TryDeleteFiles(Directory.GetFiles(path));

                Directory.Delete(path, true);
            }
            catch
            {
                // ignore errors
            }
        }

        public static void TryDeleteFiles(string[] sTempFilePaths)
        {
            if (sTempFilePaths == null) return;

            foreach (string file in sTempFilePaths)
            {
                TryDeleteFiles(file);
            }
        }

        public static void TryDeleteFiles(string file)
        {
            try
            {
                if (string.IsNullOrEmpty(file))
                {
                    return;
                }

                FileInfo info = new FileInfo(file);
                    
                if (info.Exists)
                {
                    info.IsReadOnly = false;
                    File.Delete(file);
                }
            }
            catch
            {
                // don't care
            }
        }

        public static string TryGetFolderPath(Environment.SpecialFolder folder)
        {
            // the .NET function fails, if the path is for example set to e: instead of e:\
            string result = string.Empty;

            try
            {
                result = Environment.GetFolderPath(folder);
            }
            catch (ArgumentException)
            {
                // Not so nice. Let's check the registry ourselves.
                string folderName = folder.ToString();
                try
                {
                    RegistryKey key = Registry.CurrentUser;
                    key = key.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\User Shell Folders");
                    if ((key != null) && (key.GetValue(folderName) != null))
                    {
                        object? value = key.GetValue(folderName);
                        if (value != null)
                        {
                            string? valueStr = value.ToString();
                            result = (valueStr ?? string.Empty).TrimEnd(Path.DirectorySeparatorChar);
                        }
                    }
                }
                catch (Exception)
                {
                    // if we can't access registry, we have no choice but to return an empty string
                }
            }

            return result;
        }

        public static string GetSpecialFolder(string folderName)
        {
            try
            {
                RegistryKey? key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders");
                if (key != null)
                {
                    object? value = key.GetValue(folderName);
                    if (value is string shellFolder)
                    {
                        return shellFolder;
                    }
                }

                key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\User Shell Folders");
                if (key != null)
                {
                    object? value = key.GetValue(folderName);
                    if (value is string userShellFolder)
                    {
                        return Environment.ExpandEnvironmentVariables(userShellFolder);
                    }
                }
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        public static long TryGetFileSize(string path)
        {
            if (string.IsNullOrEmpty(path)) return 0;

            try
            {
                if (!File.Exists(path)) return 0;

                FileInfo info = new FileInfo(path);
                return info.Length;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns a valid filename which does not yet exist.
        /// </summary>
        public static string GetNonExistingFilename(string filename)
        {
            if (!File.Exists(filename)) return filename;

            string? path = Path.GetDirectoryName(filename);
            string? ext = Path.GetExtension(filename);
            filename = Path.GetFileNameWithoutExtension(filename);

            int counter = 1;
            string newFilename;
            do
            {
                newFilename = Path.Combine(path ?? string.Empty, filename + " (" + counter + ")" + ext);
                counter++;
            }
            while (File.Exists(newFilename));

            return newFilename;
        }
    }
}
