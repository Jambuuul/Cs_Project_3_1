using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using JsonLibrary;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static cs_project_1.ConsoleApp;
using Terminal.Gui;

/*
 * Абдулов Джамал Олегович
 * БПИ 248-1
 * Повторная сдача проекта 3_1
 * Вариант 3
*/


namespace cs_project_1
{

    /// <summary>
    /// Класс Program, отвечающий за консольное приложение
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Метод Main, реализующий функционал консольного приложения
        /// </summary>
        internal static void Main()
        {
            
            while (true)
            {
                Console.WriteLine("Какую версию приложения хотите запустить?\n" +
                    "1. Консольная версия\n" +
                    "2. Версия на Terminal.GUI\n" +
                    "3. Выход");
                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    Run();
                    break;
                } else if (choice == "2")
                {
                    GuiApp.Run();
                    break;
                }
                else if (choice == "3")
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Некорректный выбор!\nВведите Enter для продолжения...");
                    _ = Console.ReadLine();
                    Console.Clear();
                    continue;
                    
                }
                
            }
        }
    }
}
