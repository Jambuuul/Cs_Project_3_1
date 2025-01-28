using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_project_1
{
    /// <summary>
    /// Интерфейс JSON объектов
    /// </summary>
    public interface IJSONObject
    {
        /// <summary>
        /// возвращает коллекцию строк, представляющую имена
        /// всех полей объекта JSON
        /// </summary>
        /// <returns> Коллекция строк </returns>
        IEnumerable<string> GetAllFields();

        /// <summary>
        /// возвращает значение поля в формате строки
        /// </summary>
        /// <param name="fieldName"> Название поля </param>
        /// <returns> Значение поля </returns>
        string? GetField(string fieldName);

        /// <summary>
        ///  присваивает полю значение
        /// </summary>
        /// <param name="fieldName"> имя поля </param>
        /// <param name="value"> значение </param>
        /// <exception cref="KeyNotFoundException"> Если поля не существует </exception>
        void SetField(string fieldName, string value);
    }
}
