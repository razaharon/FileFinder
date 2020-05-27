using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    class Menu
    {
        public SearchEngine engine = new SearchEngine();

        public Menu()
        {
            engine.FileFound += (sender, obj) => Console.WriteLine(obj.path+"\\"+obj.fileName+obj.extension);
        }

        public void MainMenu()
        {
            Console.WriteLine("Main Menu");
            Console.WriteLine("-------------");
            Console.WriteLine("1. Enter file name to search");
            Console.WriteLine("2. Enter file name + path to search");
            Console.WriteLine("3. View Search History");
            Console.WriteLine("4. Reset Database");
            Console.WriteLine("5. Exit");
            try
            {
                int input = int.Parse(Console.ReadLine());
                Console.Clear();
                Navigate(input);
            }
            #region Exceptions
            catch (FormatException ex)
            {
                FormatInputError();
                MainMenu();
            }
            catch (Exception ex)
            {
                GeneralInputError();
                MainMenu();
            }
            #endregion
        }

        public void GeneralInputError()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong input! Please try again!\n");
            Console.ResetColor();
        }

        public void FormatInputError()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong input! Please enter a valid number!\n");
            Console.ResetColor();
        }

        public void Navigate(int value)
        {
            switch (value)
            {
                case 1:
                    UserSelection(false);
                    break;
                case 2:
                    UserSelection(true);
                    break;
                case 3:
                    DisplaySearchHistory();
                    break;
                case 4:
                    ResetDatabase();
                    break;
                case 5: return;
                default:
                    GeneralInputError();
                    MainMenu();
                    break;
            }
        }

        public void UserSelection(bool withPath)
        {
            string path = null;
            Console.Write("Enter file name to search: ");
            string filename = Console.ReadLine();
            if (withPath)
            {
                Console.Write("Enter path: ");
                path = Console.ReadLine();
            }
            try
            {
                Console.WriteLine("Searching, Please wait...");
                engine.Search(filename, path);
                DisplayCounters();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            EndMenu(engine.TotalErrorCounter > 0);
        }

        public void EndMenu(bool WithErrorInfo)
        {
            if (WithErrorInfo)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("0. View Error Log");
                Console.ResetColor();
            }
            Console.WriteLine("1. Back to Main Menu");
            Console.WriteLine("2. Exit");
            try
            {
                int input = int.Parse(Console.ReadLine());
                GetEndMenuSelection(input, WithErrorInfo);
            }
            #region Exceptions
            catch (FormatException ex)
            {
                FormatInputError();
                EndMenu(WithErrorInfo);
            }
            catch (Exception ex)
            {
                GeneralInputError();
                EndMenu(WithErrorInfo);
            }
            #endregion
        }

        public void GetEndMenuSelection(int value, bool withErrorInfo)
        {
            if (withErrorInfo && value == 0)
            {
                engine.ErrorLog.ForEach(ex => Console.WriteLine(ex));
                Console.WriteLine();
                EndMenu(false);
            }
            else
            {
                switch (value)
                {

                    case 1:
                        Console.Clear();
                        MainMenu();
                        break;
                    case 2: break;
                    default:
                        Console.WriteLine("Wrong input! please try again:");
                        int input = int.Parse(Console.ReadLine());
                        GetEndMenuSelection(input, withErrorInfo);
                        break;
                }
            }
        }

        public void DisplayCounters()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Search done!");
            Console.WriteLine($"Files Scanned: {engine.FileCounter}");
            Console.WriteLine($"Files Found: {engine.FileFoundCounter}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (engine.UnauthorizedCounter > 0)
                Console.WriteLine($"Unauthorized Folders: {engine.UnauthorizedCounter}");
            if (engine.UnreachableCounter > 0)
                Console.WriteLine($"Unreachable Files: {engine.UnreachableCounter}");
            Console.ForegroundColor = ConsoleColor.Red;
            if (engine.UnhandledErrors > 0)
                Console.WriteLine($"Unhandled Errors: {engine.UnhandledErrors}");
            Console.ResetColor();
        }

        public void DisplaySearchHistory()
        {
            using (SearchLogic logic = new SearchLogic())
            {
                List<SearchDTO> history = logic.GetAllSearches();
                if (history.Count > 0) { 
                Console.WriteLine("{0,-25} | {1,-25} | {2,-10} |", "Value", "Path", "Results");
                Console.WriteLine("--------------------------------------------------------------------");
                history.ForEach(h => Console.WriteLine("{0,-25} | {1,-25} | {2,-10} |", h.Value, h.Path, h.Results));
                Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("There is no Searches to display.");
                    Console.WriteLine();
                }
                EndMenu(false);
            }
        }

        public void ResetDatabase()
        {
            try
            {
                using (BaseLogic logic = new BaseLogic())
                {
                    Console.WriteLine("Are you sure you want to RESET the Database?");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You will not be able to reverse this action!");
                    Console.ResetColor();
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");
                    int input = int.Parse(Console.ReadLine());
                    switch (input)
                    {
                        case 1:
                            Console.WriteLine("Work in progress, Please wait...");
                            logic.ResetDB();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("DATABASE CLEAR!");
                            Console.ResetColor();
                            EndMenu(false);
                            break;
                        case 2:
                            Console.Clear();
                            MainMenu();
                            break;
                        default:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Wrong Input! Please try again!");
                            Console.ResetColor();
                            ResetDatabase();
                            break;
                    }
                }
            }
            catch (FormatException ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong Input! Please enter a valid number!");
                Console.ResetColor();
                ResetDatabase();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong Input! Please try again!");
                Console.ResetColor();
                ResetDatabase();
            }
        }
    }
}
