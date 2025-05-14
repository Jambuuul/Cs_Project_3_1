using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cs_project_1.CultParsing;
using static cs_project_1.MenuMethods;

namespace cs_project_1
{
    /// <summary>
    /// Класс, отвечающий за консольную версию приложения из основной задачи
    /// </summary>
    internal static class ConsoleApp
    {
        /// <summary>
        /// Метод, запускающий консольную версию
        /// </summary>
        public static void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<Cult> cults = [];

            bool isLoaded = false;
            while (true)
            {

                bool repeat = true;
                while (repeat)
                {
                    Menu(isLoaded);

                    repeat = false;
                    string? choice = Console.ReadLine();
                    if (!isLoaded && choice == "2")
                    {
                        choice = "7";
                    }
                    switch (choice)
                    {
                        case "1":
                            repeat = !InputData(ref cults);
                            break;
                        case "2":
                            repeat = !FilterData(ref cults);
                            break;
                        case "3":
                            repeat = !SortData(ref cults);
                            break;
                        case "4.1":
                            repeat = !LoadMoreData(ref cults);
                            break;
                        case "4.2":
                            repeat = !EditData(ref cults);
                            break;
                        case "4.3":
                            repeat = DeleteData(ref cults);
                            break;
                        case "4.4":
                            repeat = !MergeCults(ref cults);
                            break;
                        case "5":
                            break;
                        case "6":
                            repeat = !OutputData(ref cults);
                            break;
                        case "7":
                            System.Environment.Exit(0);
                            break;
                        default:
                            repeat = true;
                            break;
                    }

                    if (repeat)
                    {
                        Console.WriteLine("Ошибка! Попробуйте еще раз. Нажмите Enter для продолжения...");
                        _ = Console.ReadLine();
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        isLoaded = true;
                        break;
                    }
                }

                Console.WriteLine("Нажмите Enter для продолжения...");
                _ = Console.ReadLine();
            }

        }

        /// <summary>
        /// Метод, отображающий главное меню
        /// </summary>
        /// <param name="dataLoaded"></param>
        public static void Menu(bool dataLoaded)
        {

            Console.Clear();
            Console.WriteLine("Добро пожаловать!");
            Console.WriteLine(dataLoaded ? "1. Заменить данные" : "1. Ввести данные (консоль/файл)");

            if (dataLoaded)
            {
                Console.WriteLine("2. Отфильтровать данные");
                Console.WriteLine("3. Отсортировать данные");
                Console.WriteLine("4.1. Дозагрузка данных");
                Console.WriteLine("4.2. Редактирование данных");
                Console.WriteLine("4.3. Удаление данных");
                Console.WriteLine("4.4. Слияние данных");
                Console.WriteLine("5. Дополнительная задача");
                Console.WriteLine("6. Вывести данные");
                Console.WriteLine("7. Выход");
            }
            else
            {
                Console.WriteLine("2. Выход");
            }

            Console.Write("Выберите пункт меню: ");

        }
    }
}
