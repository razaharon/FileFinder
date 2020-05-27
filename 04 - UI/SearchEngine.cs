using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    public class SearchEngine
    {
        public int UnreachableCounter;
        public int UnauthorizedCounter;
        public int UnhandledErrors;
        public int FileCounter;
        public int TotalErrorCounter;
        public int DriverErrorCounter;
        public int FileFoundCounter;
        public List<string> ErrorLog;

        public event EventHandler<FileEventArgs> FileFound;

        private void ResetCounters()
        {
            FileCounter = 0;
            UnreachableCounter = 0;
            UnauthorizedCounter = 0;
            UnhandledErrors = 0;
            TotalErrorCounter = 0;
            DriverErrorCounter = 0;
            FileFoundCounter = 0;
            ErrorLog = new List<string>();
        }

        public void Search(string searchValue, string path)
        {
            ResetCounters();

            // Add new Search to DB and get a new SearchID
            #region GetSearchID
            int SearchID;
            using (SearchLogic logic = new SearchLogic())
            {
                SearchDTO result = logic.AddSearch(searchValue, path);
                SearchID = result.ID;
            }
            #endregion

            if (path == null || path == "")
            {
                // Search All the Drivers on the machine if path is not specified
                #region SearchDrivers
                List<string> drivers = Directory.GetLogicalDrives().ToList();
                drivers.ForEach(d =>
                {
                    try
                    {
                        SearchFiles(d, searchValue, SearchID);
                    }
                    catch (Exception e)
                    {
                        DriverErrorCounter++;
                    }
                });
                #endregion
            }
            else
            {
                // Checking given path and search it
                #region SearchPath
                if (Directory.Exists(path))
                    SearchFiles(path, searchValue, SearchID);
                else
                    throw new Exception("Path does not exists!");
                #endregion
            }
            using (SearchLogic logic = new SearchLogic())
            {
                logic.SetSearchResults(SearchID, FileFoundCounter);
            }
        }

        private void SearchFiles(string path, string searchValue, int searchId)
        {
            searchValue = searchValue.ToLower();

            // Search Files
            #region SearchFiles
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                try
                {
                    // Get file Info
                    #region GetFileInfo
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string filePath = Path.GetFullPath(file);
                    string fileExtension = Path.GetExtension(file);
                    #endregion
                    FileCounter++;
                    if (fileName.ToLower().Contains(searchValue))
                    {
                        // Add Result to DB and Notify File Found
                        using (ResultLogic logic = new ResultLogic())
                        {
                            FileFoundCounter++;
                            logic.AddResult(fileName, filePath, searchId);
                            FileFound?.Invoke(this, new FileEventArgs(fileName, fileExtension, filePath));
                        }
                    }
                }
                #region Exceptions
                catch (PathTooLongException ex)
                {
                    UnreachableCounter++;
                    TotalErrorCounter++;
                }
                catch (Exception ex)
                {
                    UnhandledErrors++;
                    TotalErrorCounter++;
                    ErrorLog.Add(ex.Message);
                }
                #endregion
            }
            #endregion

            // Search Directories
            #region SearchDirectories
            string[] directories = Directory.GetDirectories(path);
            foreach (string d in directories)
                try
                {
                    SearchFiles(d, searchValue, searchId);
                }
                #region Exceptions
                catch (UnauthorizedAccessException ex)
                {
                    UnauthorizedCounter++;
                    TotalErrorCounter++;
                    ErrorLog.Add(ex.Message);
                }
                catch (DirectoryNotFoundException ex)
                {
                    UnreachableCounter++;
                    TotalErrorCounter++;
                    ErrorLog.Add(ex.Message);
                }
                catch (Exception ex)
                {
                    UnhandledErrors++;
                    TotalErrorCounter++;
                    ErrorLog.Add(ex.Message);
                }
            #endregion
        #endregion
        }

    }
}
