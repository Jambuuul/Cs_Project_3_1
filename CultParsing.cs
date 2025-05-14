using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using JsonLibrary;

namespace cs_project_1
{
    /// <summary>
    /// Класс для парсинга в культы
    /// </summary>
    internal static class CultParsing
    {
        /// <summary>
        /// Вспомогательный метод, достает из исходного JSONa JsonList с культами
        /// </summary>
        /// <param name="obj"> сам JSON-объект</param>
        /// <returns> JsonList культов</returns>
        private static JsonList? GetCultJsonList(JsonObject obj)
        {
            JsonValue? elems = obj?.GetValue("elements");
            return elems is not null and JsonList list ? list : null;
        }

        /// <summary>
        /// Достает из исходного JSON-объекта список (list) культов
        /// </summary>
        /// <param name="obj">JSON-объект</param>
        /// <returns> список культов</returns>
        public static List<Cult>? GetCults(JsonObject? obj)
        {

            if (obj == null)
            {
                return null;
            }

            JsonList? arr = GetCultJsonList((JsonObject)obj);
            List<Cult> cults = [];
            if (arr == null)
            {
                return cults; 
            }

            foreach (JsonValue? val in arr.Data)
            {
                if (val is not null and JsonObject ab)
                {
                    cults.Add(new Cult(ab));
                }
            }
            return cults;
        }

        /// <summary>
        /// Преобразует список культов в JSON-объект
        /// </summary>
        /// <param name="cults"> список культов</param>
        /// <returns> JSON-объект </returns>
        public static JsonObject CultsToJson(List<Cult> cults)
        {
            StringBuilder sb = new();
            _ = sb.Append("{\"elements\" :  [");

            for (int i = 0; i < cults.Count; i++)
            {
                _ = sb.Append(cults[i].ToString());
                if (i < cults.Count - 1)
                {
                    _ = sb.Append(", ");
                }
            }
            _ = sb.Append("]}");
            return JsonParser.ReadJson(sb.ToString()) ?? new();
        }
    }
}
