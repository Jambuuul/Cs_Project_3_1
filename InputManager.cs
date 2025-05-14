using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
namespace cs_project_1
{

    public static class InputManager
    {
        /// <summary>
        /// Открывает диалог для ввода строки.
        /// </summary>
        /// <param name="title">Заголовок диалога</param>
        /// <param name="prompt">Подсказка для ввода</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Введённое значение или null, если пользователь отменил ввод</returns>
        public static string? Query(string title, string prompt, string defaultValue = "")
        {
            string? result = null;
            bool okPressed = false;

            // Создаем диалоговое окно
            Dialog dialog = new(title, 60, 7);

            // Метка с подсказкой
            Label label = new(prompt)
            {
                X = 1,
                Y = 1
            };

            // Текстовое поле для ввода
            TextField textField = new(defaultValue)
            {
                X = 1,
                Y = Pos.Bottom(label) + 1,
                Width = Dim.Fill() - 2
            };

            // Кнопка OK
            Button okButton = new("OK");
            okButton.Clicked += () =>
            {
                result = textField.Text.ToString();
                okPressed = true;
                Application.RequestStop();
            };

            // Кнопка Cancel
            Button cancelButton = new ("Cancel");
            cancelButton.Clicked += () => Application.RequestStop();

            // Располагаем кнопки
            dialog.Add(label, textField);
            dialog.AddButton(okButton);
            dialog.AddButton(cancelButton);

            Application.Run(dialog);

            return okPressed ? result : null;
        }
    }

}
