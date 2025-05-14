using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JsonLibrary;
using Terminal.Gui;

namespace cs_project_1
{
    /// <summary>
    /// Класс, реализующий те же методы, что в MenuMethods, но для TUI-приложения
    /// </summary>
    internal static class GuiMethods
    {
        /// <summary>
        /// Метод, отвечающий за догрузку данных.
        /// </summary>
        /// <param name="cults">Список культов.</param>
        /// <returns>true, если отработал без ошибок.</returns>
        public static bool LoadMoreData(ref List<Cult> cults)
        {
            List<Cult> another = [];
            bool inputResult = InputData(ref another);
            if (inputResult == false)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Данные отсутствуют", "OK");
                return false;
            }
            Dictionary<string, Cult> added = [];
            HashSet<string> contains = [];

            foreach (Cult c in cults)
            {
                if (c.Id != null)
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
                if (cults[i].Id != null && added.ContainsKey(cults[i].Id ?? ""))
                {
                    cults[i] = added[cults[i].Id ?? ""];
                }
            }

            foreach (KeyValuePair<string, Cult> pair in added)
            {
                if (pair.Key != null && contains.Contains(pair.Key) == false)
                {
                    cults.Add(pair.Value);
                }
            }
            return true;
        }

        /// <summary>
        /// Отвечает за удаление культа по id.
        /// </summary>
        /// <param name="cults">Список культов.</param>
        /// <returns>true, если отработал без ошибок.</returns>
        public static bool DeleteData(ref List<Cult> cults)
        {
            string? id = InputManager.Query("Удаление культа", "Введите id для удаления:", "");
            if (id == null)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Не введен id", "OK");
                return false;
            }
            for (int i = 0; i < cults.Count; i++)
            {
                if (cults[i].Id == id)
                {
                    cults.RemoveAt(i);
                    break;
                }
            }
            _ = MessageBox.Query("Успех", "Культ удален", "OK");
            return true;
        }

        /// <summary>
        /// Отвечает за слияние данных с двух файлов.
        /// </summary>
        /// <param name="cults">Список культов.</param>
        /// <returns>true, если отработал без ошибок.</returns>
        public static bool MergeCults(ref List<Cult> cults)
        {
            List<Cult> c1 = [];
            List<Cult> c2 = [];
            if (InputData(ref c1) == false || InputData(ref c2) == false)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Ошибка с данными", "OK");
                return false;
            }

            Dictionary<string, Cult> addedCults = [];

            _ = MessageBox.Query("Слияние", "Из какого файла сохранять культы? 1, если из первого", "OK");
            string? choice = InputManager.Query("Слияние", "Введите 1 для первого файла, иначе для второго:", "2");
            foreach (Cult c in c1)
            {
                if (c.Id != null)
                {
                    addedCults.Add(c.Id, c);
                }
            }

            foreach (Cult c in c2)
            {
                if (c.Id != null)
                {
                    if (choice == "1")
                    {
                        if (addedCults.ContainsKey(c.Id) == false)
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
                if (c.Id != null)
                {
                    _ = contains.Add(c.Id);
                }
            }

            for (int i = 0; i < cults.Count; i++)
            {
                if (cults[i].Id != null && addedCults.ContainsKey(cults[i].Id ?? ""))
                {
                    cults[i] = addedCults[cults[i].Id ?? ""];
                }
            }

            foreach (KeyValuePair<string, Cult> pair in addedCults)
            {
                if (pair.Key != null && contains.Contains(pair.Key) == false)
                {
                    cults.Add(pair.Value);
                }
            }

            _ = MessageBox.Query("Слияние", "Слияние выполнено успешно", "OK");
            return true;
        }

        

        /// <summary>
        /// Отвечает за редактирование данных культа по его id.
        /// </summary>
        /// <param name="cults">Список культов.</param>
        /// <returns>true, если отработал без ошибок.</returns>
        public static bool EditData(ref List<Cult> cults)
        {
            string? id = InputManager.Query("Редактирование", "Введите id культа для редактирования:", "");
            if (id == null)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Ошибка при введении id", "OK");
                return false;
            }

            int ind = -1;
            for (int i = 0; i < cults.Count && ind < 0; i++)
            {
                if (cults[i].Id == id)
                {
                    ind = i;
                    break;
                }
            }
            if (ind == -1)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "id не найден", "OK");
                return false;
            }
            _ = MessageBox.Query("Редактирование", "Введите на разных строках название поля для редактирования, его тип и значение", "OK");
            string? name = InputManager.Query("Редактирование", "Введите название поля для редактирования:", "");
            string? fieldType = InputManager.Query("Редактирование", "Введите тип поля (string, int, double):", "");
            string? value = InputManager.Query("Редактирование", "Введите новое значение:", "");
            if (name == null || fieldType == null || value == null)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Введены некорректные значения!", "OK");
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
                    _ = MessageBox.ErrorQuery("Ошибка", "Ошибка при присвоении значения!", "OK");
                    return false;
                }
            }
            catch (Exception)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Некорректный тип данных!", "OK");
                return false;
            }
            _ = MessageBox.Query("Успех", "Данные успешно обновлены.", "OK");
            return true;
        }

        /// <summary>
        /// Отвечает за вывод информации.
        /// </summary>
        /// <param name="cults">Список культов.</param>
        /// <returns>true, если отработал без ошибок.</returns>
        public static bool OutputData(ref List<Cult> cults)
        {
            string? source = InputManager.Query("Вывод данных", "Выберите источник (console/file):", "console");
            if (source == null)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Источник вывода не выбран.", "OK");
                return false;
            }
            if (source.ToLower() == "file")
            {
                string? path = InputManager.Query("Вывод данных", "Введите путь к файлу:", "");
                if (path == null || !JsonParser.IsValidFullPath(path))
                {
                    _ = MessageBox.ErrorQuery("Ошибка", "Некорректный путь", "OK");
                    return false;
                }
                JsonParser.WriteJson(CultParsing.CultsToJson(cults), path, true);
                _ = MessageBox.Query("Успех", "Файл успешно сохранен.", "OK");
            }
            else if (source.ToLower() == "console")
            {
                JsonParser.WriteJson(CultParsing.CultsToJson(cults), "", false);
                _ = MessageBox.Query("Вывод данных", "Данные выведены в консоль.", "OK");
            }
            else
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Ошибка. Попробуйте еще раз", "OK");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Отвечает за сортировку по какому-то столбцу.
        /// </summary>
        /// <param name="cults">Список культов.</param>
        /// <returns>true, если отработал без ошибок.</returns>
        public static bool SortData(ref List<Cult> cults)
        {
            string? key = InputManager.Query("Сортировка", "Введите ключ, по которому необходимо сортировать:", "");
            string? dir = InputManager.Query("Сортировка", "Выберите направление: Введите 'Y' для прямого порядка, иначе для обратного:", "");
            if (key == null || dir == null)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Сортировка не удалась: встретились null параметры", "OK");
                return false;
            }
            List<Cult> a = [.. cults.OrderBy(delegate (Cult x) { return x.GetField(key); })];
            if (dir != "Y")
            {
                a.Reverse();
            }
            cults.Clear();
            foreach (Cult c in a)
            {
                cults.Add(c);
            }
            _ = MessageBox.Query("Успех", "Данные отсортированы.", "OK");
            return true;
        }

        /// <summary>
        /// Отвечает за фильтрацию данных по одному или нескольким значениям.
        /// </summary>
        /// <param name="cults">Список культов.</param>
        /// <returns>true, если отработал без ошибок.</returns>
        public static bool FilterData(ref List<Cult> cults)
        {
            string? filter = InputManager.Query("Фильтрация", "По какому столбцу фильтровать?", "");
            if (filter == null)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Столбец не может быть пустым.", "OK");
                return false;
            }
            // Запрашиваем значения через запятую (простейший способ)
            string? keysInput = InputManager.Query("Фильтрация", "Введите значения для фильтрации через запятую:", "");
            List<string> keys = [];
            if (keysInput != null)
            {
                string[] parts = keysInput.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < parts.Length; i++)
                {
                    keys.Add(parts[i].Trim());
                }
            }
            int deletedAmount = 0;
            for (int i = 0; i < cults.Count; i++)
            {
                Cult cult = cults[i];
                if (cult.Filter(filter, keys) == false)
                {
                    if (cults.Remove(cult))
                    {
                        i--;
                        deletedAmount++;
                    }
                }
            }
            _ = MessageBox.Query("Оповещение", $"Элементов удалено: {deletedAmount}", "OK");
            _ = MessageBox.Query("Успех", "Данные отфильтрованы.", "OK");
            return true;
        }

        /// <summary>
        /// Отвечает за ввод данных.
        /// </summary>
        /// <param name="cults">Список культов.</param>
        /// <returns>true, если отработал без ошибок.</returns>
        public static bool InputData(ref List<Cult> cults)
        {
            string? source = InputManager.Query("Ввод данных", "Выберите источник (console/file):", "file");
            StringBuilder data = new();
            if (source == null)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Некорректная опция!", "OK");
                return false;
            }
            if (source.ToLower() == "file")
            {
                string? path = InputManager.Query("Ввод данных", "Введите путь к файлу:", "");
                if (path == null || !JsonParser.IsValidFullPath(path))
                {
                    _ = MessageBox.ErrorQuery("Ошибка", "Некорректный путь", "OK");
                    return false;
                }
                try
                {
                    _ = data.Append(File.ReadAllText(path));
                }
                catch (Exception ex)
                {
                    _ = MessageBox.ErrorQuery("Ошибка", "Ошибка чтения файла: " + ex.Message, "OK");
                    return false;
                }
            }
            else if (source.ToLower() == "console")
            {
                // Для ввода JSON используем многострочный диалог
                string jsonInput = ShowMultilineInputDialog("Ввод данных", "Введите данные JSON. Для прекращения ввода в конце строки введите 'stop':", "");
                if (jsonInput == null || jsonInput.Trim() == "")
                {
                    _ = MessageBox.ErrorQuery("Ошибка", "Введены некорректные данные.", "OK");
                    return false;
                }
                // Если введено 'stop' в конце, удаляем его
                int stopIndex = jsonInput.LastIndexOf("stop");
                if (stopIndex >= 0)
                {
                    jsonInput = jsonInput[..stopIndex];
                }
                _ = data.Append(jsonInput);
            }
            else
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Ошибка. Попробуйте еще раз", "OK");
                return false;
            }
            JsonObject? json = JsonParser.ReadJson(data.ToString());
            if (json == null)
            {
                _ = MessageBox.ErrorQuery("Ошибка", "Некорректный формат JSON-файла!", "OK");
                return false;
            }
            List<Cult>? parsedCults = CultParsing.GetCults(json);
            cults = parsedCults ?? ([]);
            _ = MessageBox.Query("Успех", "Данные успешно загружены.", "OK");
            return true;
        }

        /// <summary>
        /// Метод для многострочного ввода через диалог с TextView
        /// </summary>
        /// <param name="title">Заголовок диалога</param>
        /// <param name="message">Подсказка для ввода</param>
        /// <param name="defaultText">Начальное содержимое</param>
        /// <returns>Введённый текст или пустую строку при отмене ввода</returns>
        private static string ShowMultilineInputDialog(string title, string message, string defaultText)
        {
            int width = 80;
            int height = 20;
            Dialog dlg = new(title, width, height);
            Label lbl = new(message)
            {
                X = 1,
                Y = 1
            };
            dlg.Add(lbl);
            TextView textView = new()
            {
                X = 1,
                Y = Pos.Bottom(lbl) + 1,
                Width = Dim.Fill(2),
                Height = Dim.Fill(2),
                Text = defaultText
            };
            dlg.Add(textView);
            string? result = "";
            Button okButton = new("OK");
            okButton.Clicked += delegate
            {
                result = textView.Text.ToString();
                Application.RequestStop();
            };
            dlg.AddButton(okButton);
            Button cancelButton = new("Cancel");
            cancelButton.Clicked += delegate
            {
                result = "";
                Application.RequestStop();
            };
            dlg.AddButton(cancelButton);
            Application.Run(dlg);
            return result;
        }
    }
}
