using JsonLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_project_1
{
    /// <summary>
    /// Статический класс, реализующий методы консольного приложения.
    /// Методы принимают по ссылке текущий список культов
    /// Возвращают true, если метод отработал без ошибок, иначе false
    /// </summary>
    internal static class MenuMethods
    {
        /// <summary>
        /// Метод, отвечающий за догрузку данных
        /// </summary>
        /// <param name="cults"> список культов </param>
        /// <returns> true, если отработал без ошибок</returns>
        public static bool LoadMoreData(ref List<Cult> cults)
        {
            List<Cult> another = [];
            _ = InputData(ref another);

            if (another is null)
            {
                Console.WriteLine("Данные отсутствуют");
                return false;
            }
            Dictionary<string, Cult> added = [];
            HashSet<string> contains = [];

            foreach (Cult c in cults)
            {
                if (c.Id is not null)
                {
                    _ = contains.Add(c.Id);
                }
            }

            foreach (Cult c in another)
            {
                added.Add(c.Id ?? "", c);
            }

            for (int i = 0; i < cults.Count; i++)
            {
                if (cults[i].Id is not null && added.ContainsKey(cults[i].Id ?? ""))
                {
                    cults[i] = added[cults[i].Id!];
                }
            }

            foreach ((string id, Cult c) in added)
            {
                if (id != null && !contains.Contains(id))
                {
                    cults.Add(c);
                }
            }
            return true;
        }

        /// <summary>
        /// Отвечает за удаление культа по id
        /// </summary>
        /// <param name="cults"></param>
        /// <returns></returns>
        public static bool DeleteData(ref List<Cult> cults)
        {
            Console.WriteLine($"Введите id для удаления");
            string? id = Console.ReadLine() ?? string.Empty;

            for (int i = 0; i < cults.Count; i++)
            {
                if (cults[i].Id == id)
                {
                    cults.RemoveAt(i);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Отвечает за слияние данных с двух файлов
        /// </summary>
        /// <param name="cults"></param>
        /// <returns></returns>
        public static bool MergeCults(ref List<Cult> cults)
        {
            List<Cult> c1 = [];
            List<Cult> c2 = [];
            if (!InputData(ref c1) || !InputData(ref c2))
            {
                Console.WriteLine("Ошибка с данными");
                return false;
            }

            Dictionary<string, Cult> addedCults = [];

            Console.WriteLine("Из какого файла сохранять культы? 1, если из первого");

            string? choice = Console.ReadLine() ?? "2";
            foreach (Cult c in c1)
            {
                if (c.Id is not null)
                {
                    addedCults.Add(c.Id, c);
                }
            }

            foreach (Cult c in c2)
            {
                if (c.Id is not null)
                {
                    if (choice == "1")
                    {
                        if (!addedCults.ContainsKey(c.Id))
                        {
                            addedCults.Add(c.Id, c);
                        }
                    }
                    else
                    {
                        if (addedCults.ContainsKey(c.Id))
                        {
                            addedCults[c.Id] = c;
                        }
                        else
                        {
                            addedCults.Add(c.Id, c);
                        }
                    }
                }
            }
            HashSet<string> contains = [];

            foreach (Cult c in cults)
            {
                if (c.Id is not null)
                {
                    _ = contains.Add(c.Id);
                }
            }



            for (int i = 0; i < cults.Count; i++)
            {
                if (cults[i].Id is not null && addedCults.ContainsKey(cults[i].Id ?? ""))
                {
                    cults[i] = addedCults[cults[i].Id ?? ""];
                }
            }

            foreach ((string id, Cult c) in addedCults)
            {
                if (id != null && !contains.Contains(id))
                {
                    cults.Add(c);
                }
            }

            return true;
        }

        /// <summary>
        /// Отвечает за редактирование данных культа по его айди
        /// </summary>
        /// <param name="cults"></param>
        /// <returns></returns>
        public static bool EditData(ref List<Cult> cults)
        {
            Console.WriteLine("Введите id культа для редактирования");
            string? id = Console.ReadLine();
            if (id == null)
            {
                Console.WriteLine("Ошибка при введении id");
                return false;
            }

            int ind = -1;
            for (int i = 0; ind < cults.Count; i++)
            {
                if (cults[i].Id == id)
                {
                    ind = i;
                    break;
                }
            }
            if (ind == -1)
            {
                Console.WriteLine("id не найден");
                return false;
            }
            Console.WriteLine("Введите на разных строках название поля для редактирования, его тип и значение");
            string? name = Console.ReadLine();
            string? fieldType = Console.ReadLine();
            string? value = Console.ReadLine();
            if (name == null || fieldType == null || value == null)
            {
                Console.WriteLine("Введены некорректные значения!");
                return false;
            }
            try
            {
                fieldType = fieldType.ToLower();
                if (fieldType is "string" or "int" or "double")
                {
                    cults[ind].SetField(name, value);
                }
                else
                {
                    Console.WriteLine("Ошибка при присвоении значения!");
                    return false;
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Некорректный тип данных!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Отвечает за вывод информации
        /// </summary>
        /// <param name="cults"></param>
        /// <returns></returns>
        public static bool OutputData(ref List<Cult> cults)
        {

            Console.Write("Выберите источник (console/file): ");
            string? source = Console.ReadLine();


            if (source == "file")
            {
                Console.Write("Введите путь к файлу: ");
                string? path = Console.ReadLine();

                if (path == null || !JsonParser.IsValidFullPath(path))
                {
                    Console.WriteLine("Некорректный путь");
                    return false;
                }
                JsonParser.WriteJson(CultParsing.CultsToJson(cults), path, true);
                Console.WriteLine("Файл успешно сохранен.");
            }
            else if (source == "console")
            {
                JsonParser.WriteJson(CultParsing.CultsToJson(cults), "", false);
            }
            else
            {
                Console.WriteLine("Ошибка. Попробуйте еще раз");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Отвечает за сортировку по какому-то столбцу
        /// </summary>
        /// <param name="cults"></param>
        /// <returns></returns>
        public static bool SortData(ref List<Cult> cults)
        {
            Console.WriteLine("Введите ключ, по которому необходимо сортировать");
            string? key = Console.ReadLine();

            Console.WriteLine("Выберите направление: Y, если вверх, иначе вниз");
            string? dir = Console.ReadLine();

            if (key == null || dir == null)
            {
                Console.WriteLine("Сортировка не удалась: встретились null параметры"); return false;
            }
            List<Cult> a;


            a = [.. cults.OrderBy(x => x.GetField(key))];
            if (dir != "Y")
            {
                a.Reverse();
            }
            cults = a;

            return true;
        }

        /// <summary>
        /// Отвечает за фильтрацию данных по одному или нескольким значениям
        /// </summary>
        /// <param name="cults"></param>
        /// <returns></returns>
        public static bool FilterData(ref List<Cult> cults)
        {
            Console.Write("По какому столбцу фильтровать?");
            string? filter = Console.ReadLine();
            Console.WriteLine("Вводите значения в новой строке. Чтобы прекратить, введите пустую строку.");
            List<string> keys = [];

            if (filter == null)
            {
                return false;
            }
            string? s;
            while ((s = Console.ReadLine()) != "" && s != null)
            {
                keys.Add(s);
            }



            for (int i = 0; i < cults.Count; i++)
            {
                Cult cult = cults[i];
                if (!cult.Filter(filter, keys))
                {
                    if (cults.Remove(cult))
                    {
                        i--;
                        Console.WriteLine("deleted");
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Отвечает за ввод данных
        /// </summary>
        /// <param name="cults"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool InputData(ref List<Cult> cults)
        {
            Console.Write("Выберите источник (console/file): ");
            string? source = Console.ReadLine();

            StringBuilder data = new();

            if (source == "file")
            {
                TextReader original = Console.In;
                Console.Write("Введите путь к файлу: ");
                string? path = Console.ReadLine();

                if (path == null || !JsonParser.IsValidFullPath(path))
                {
                    Console.WriteLine("Некорректный путь");
                    return false;
                }
                StreamReader sr = new(path);
                Console.SetIn(sr);

                while (sr.Peek() > -1)
                {
                    _ = data.AppendLine(sr.ReadLine());
                }

                sr.Close();
                Console.SetIn(original);
            }
            else if (source == "console")
            {
                Console.WriteLine("Введите данные JSON. Для прекращения ввода введите stop.");
                string? s;

                while ((s = Console.ReadLine()) != "stop")
                {
                    _ = data.AppendLine(s);
                }

            }
            else
            {
                Console.WriteLine("Ошибка. Попробуйте еще раз");
                return false;
            }

            JsonObject? json = JsonParser.ReadJson(data.ToString()) ?? throw new Exception("Некорректный формат JSON-файла!");
            cults = CultParsing.GetCults(json) ?? [];


            return true;
        }
    }
}
