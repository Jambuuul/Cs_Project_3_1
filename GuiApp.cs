using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace cs_project_1
{
    
    /// <summary>
    /// Класс, реализующий TUI версию приложения
    /// </summary>
    public static class GuiApp
    {
        
        public static List<Cult> cults = [];

        /// <summary>
        /// Запуск TUI-приложения
        /// </summary>
        public static void Run()
        {
            Application.Init();
            Toplevel top = Application.Top;

            Window mainWin = new("Cult Manager, проект №3")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            MenuBar menu = new(
            [
                new MenuBarItem("_File", new MenuItem[]
                {
                    new("_Select File", "", FileSelectionDialog),
                    new("_Exit", "", () => Application.RequestStop())
                }),
                new MenuBarItem("_Operations", new MenuItem[]
                {
                    new("_Input Data", "", InputData),
                    new("_Filter Data", "", FilterData),
                    new("_Sort Data", "", SortData),
                    new("_Merge Data", "", MergeData),
                    new("_Edit Data", "", EditData),
                    new("_Delete Data", "", DeleteData),
                    new ("_Output Data", "", OutputData),
                    new("_Load more Data", "", LoadMoreData)
                }),
                new MenuBarItem("_About", new MenuItem[]
                {
                    new("_About app", "", ShowAbout)
                })
            ]);

            top.Add(menu, mainWin);
            Application.Run();
        }

        /// <summary>
        /// Обертка для TUI с вызовом нужного метода
        /// </summary>
        private static void LoadMoreData()
        {
            _ = GuiMethods.LoadMoreData(ref cults);
        }


        /// <summary>
        /// Обертка для TUI с вызовом нужного метода
        /// </summary>
        private static void InputData()
        {
            
            _ = GuiMethods.InputData(ref cults);
        }

        /// <summary>
        /// Обертка для TUI с вызовом нужного метода
        /// </summary>
        private static void FilterData()
        {
            _ = GuiMethods.FilterData(ref cults);
        }

        /// <summary>
        /// Обертка для TUI с вызовом нужного метода
        /// </summary>
        private static void SortData()
        {
            _ = GuiMethods.SortData(ref cults);
        }

        /// <summary>
        /// Обертка для TUI с вызовом нужного метода
        /// </summary>
        private static void MergeData()
        {
            _ = GuiMethods.MergeCults(ref cults);
        }

        /// <summary>
        /// Обертка для TUI с вызовом нужного метода
        /// </summary>
        private static void EditData()
        {
            _ = GuiMethods.EditData(ref cults);
        }

        /// <summary>
        /// Обертка для TUI с вызовом нужного метода
        /// </summary>
        private static void DeleteData()
        {
            _ = GuiMethods.DeleteData(ref cults);
        }

        /// <summary>
        /// Обертка для TUI с вызовом нужного метода
        /// </summary>
        private static void OutputData()
        {
            Dialog dlg = new ("Output Data", 100, 30);
            ListView listView = new (cults)
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            dlg.Add(listView);

            Button closeButton = new("Close");
            closeButton.Clicked += () => Application.RequestStop();
            dlg.AddButton(closeButton);

            Application.Run(dlg);
        }

        /// <summary>
        /// Обертка для TUI с вызовом нужного метода
        /// </summary>
        private static void ShowAbout()
        {
            _ = MessageBox.Query(50, 7, "About", "Абдулов Джамал Олегович, 3 вариант", "OK");
        }

      

        /// <summary>
        /// Диалог для выбора файла с возможностью навигации по директориям
        /// Отображаются файлы и поддиректории текущей директории
        /// </summary>
        private static void FileSelectionDialog()
        {
            
            string currentDir = Directory.GetCurrentDirectory();
            Dialog dlg = new ($"Select File - {currentDir}", 60, 20);

            
            string[] files = Directory.GetFiles(currentDir);
            string[] directories = Directory.GetDirectories(currentDir);

            
            List<string> items = [".. (Parent Directory)"];

            
            foreach (string dir in directories)
            {
                items.Add("[DIR] " + Path.GetFileName(dir));
            }
            
            foreach (string file in files)
            {
                items.Add(Path.GetFileName(file));
            }

            ListView listView = new(items)
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            dlg.Add(listView);

            
            Button selectButton = new("Select");
            selectButton.Clicked += () =>
            {
                int selected = listView.SelectedItem;
                if (selected == 0)
                {
                    
                    DirectoryInfo? parent = Directory.GetParent(currentDir);
                    if (parent != null)
                    {
                        Directory.SetCurrentDirectory(parent.FullName);
                        
                        Application.RequestStop();
                        FileSelectionDialog();
                    }
                }
                else if (selected <= directories.Length)
                {
                    
                    int index = selected - 1;
                    Directory.SetCurrentDirectory(directories[index]);
                    Application.RequestStop();
                    FileSelectionDialog();
                }
                else
                {
                    
                    int fileIndex = selected - 1 - directories.Length;
                    string fileName = files[fileIndex];
                    _ = MessageBox.Query(50, 7, "File Selected", $"Вы выбрали: {fileName}", "OK");
                    Application.RequestStop();
                }
            };
            dlg.AddButton(selectButton);

            
            Button cancelButton = new("Cancel");
            cancelButton.Clicked += () => Application.RequestStop();
            dlg.AddButton(cancelButton);

            Application.Run(dlg);
        }



        /// <summary>
        /// Метод для проверки наличия культа с заданным id.
        /// Если культ существует – заменяет его, иначе добавляет в коллекцию.
        /// </summary>
        private static void ReplaceOrAddCult(Cult cult)
        {
            Cult? existing = cults.Find(c => c.Id == cult.Id);
            if (existing != null)
            {
                int index = cults.IndexOf((Cult)existing);
                cults[index] = cult;
            }
            else
            {
                cults.Add(cult);
            }
        }
    }
}

