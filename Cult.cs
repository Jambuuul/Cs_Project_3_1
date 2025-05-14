//using cs_project_1;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using JsonLibrary;
using System.Security.Cryptography;

namespace cs_project_1
{
    public struct Cult : IJSONObject
    {

        private readonly JsonObject _data;
        public string? Id { get; set; }

        public Dictionary<string, string>? Slots { get; set; }

        public Cult(JsonObject? obj)
        {
            _data = obj ?? throw new ArgumentNullException(nameof(obj));
            Id = obj.GetField("id") ?? obj.GetField("ID") ?? null;
            Slots = [];
            JsonValue? t = obj.GetValue("slots") ?? null;

            if (t is not null and JsonObject trig)
            {
                foreach (string? key in trig.GetAllFields())
                {
                    if (key == null)
                    {
                        continue;
                    }
                    string? value = trig.GetField(key) ?? null;

                    if (value != null)
                    {
                        Slots.Add(key, value);
                    }

                }
            }
        }


        /// <summary>
        /// Проверяет, подходит ли объект по фильтру
        /// </summary>
        /// <param name="fieldName"> имя поля </param>
        /// <param name="keys"> массив значений</param>
        /// <returns> true, если подходит, иначе false </returns>
        public readonly bool Filter(string field, List<string> keys)
        {
            HashSet<string> keysSet = [];
            foreach (string key in keys)
            {
                _ = keysSet.Add(key);
            }
            
            string? res = _data.GetField(field);
            return res != null && keysSet.Contains(res); 
        }


        public readonly IEnumerable<string> GetAllFields()
        {
            //string[] keys = [.. _data.Keys];
            IEnumerable<string>? lst = _data?.GetAllFields();
            if (lst == null)
            {
                return [];
            }
            List<string> strs = lst.ToList();
            strs ??= [];
            return strs;
        }

        public readonly string? GetField(string fieldName)
        {
            return _data?.GetField(fieldName);
        }

        public readonly void SetField(string fieldName, string value)
        {
            _data?.SetField(fieldName, value);
        }

        public override readonly string ToString()
        {
            return _data.ToString();
        }
    }
}
