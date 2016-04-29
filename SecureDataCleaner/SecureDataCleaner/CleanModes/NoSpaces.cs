using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SecureDataCleaner.Interfaces;
using SecureDataCleaner.Types;

namespace SecureDataCleaner.CleanModes
{
    public class NoSpaces : ICleanMode
    {
        private readonly Regex _regexTemplate;

        private readonly List<string> _valueGroupNames = new List<string>();

        public char Replacement { get; set; }

        /// <summary>
        /// Создание объекта чистки по шаблону
        /// <exception cref="ArgumentException">При неверном шаблоне</exception>
        /// </summary>
        /// <param name="template">Строковый шаблон</param>
        public NoSpaces(string template)
        {
            Replacement = 'X'; //default
            try
            {
                _regexTemplate = TemplateToRegex(template);
            }
            catch (Exception)
            {
                throw new ArgumentException("Template is not vald");
            }
        }

        /// <summary>
        /// Преобразование шаблона в регулярное выражение.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private Regex TemplateToRegex(string template)
        {
            var cleanTemplate = template;

            cleanTemplate = Regex.Replace(cleanTemplate, @"\s*", String.Empty);

            var templateKeys = Regex.Matches(cleanTemplate, @"{templKey:([^}]*)}");
            var templateValues = Regex.Matches(cleanTemplate, @"{templValue:([^}]*)}");

            string regexString = cleanTemplate;

            //экранирование управляющих символов в шаблоне
            regexString = Regex.Replace(regexString, @"[\-\[\]\(\)\*\+\?\.,\\\^|#]", @"\$&");
            
            //хранение имен ключевых полей вроде как бесполезно
            //TODO возможно удалить!
            foreach(Match templateKey in templateKeys)
            {
                regexString = regexString.Replace(templateKey.Groups[0].Value,
                    String.Format("(?<key{0}>.*)", templateKey.Groups[1].Value));
            }

            foreach (Match templateValue in templateValues)
            {
                regexString = regexString.Replace(templateValue.Groups[0].Value,
                    String.Format("(?<{0}>.*)",templateValue.Groups[1].Value));
                _valueGroupNames.Add(String.Format("{0}", templateValue.Groups[1].Value));
            }
            return new Regex(regexString);
        }

        /// <summary>
        /// Очистка строки от secure-данных
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="secureString">Очищаемая строка</param>
        /// <returns>Результат очистки</returns>
        public CleanResult CleanString( string secureString)
        {
            var cleanString = Regex.Replace(secureString, @"\s", String.Empty);
            var matches = _regexTemplate.Matches(cleanString);
            if (matches.Count == 0)
            {
                throw new ArgumentException("Invalid string");
            }
            var cleanResult = new CleanResult();
            foreach (var groupName in _valueGroupNames)
            {
                Group secureValue = matches[0].Groups[groupName];
                cleanString = cleanString.Remove(secureValue.Index, secureValue.Length);
                cleanString = cleanString.Insert(secureValue.Index, new String(Replacement, secureValue.Length));
                cleanResult.SecureData.Add(new Data
                {
                    Key = groupName,
                    Value = secureValue.Value
                });
            }
            cleanResult.CleanString = cleanString;
            return cleanResult;
        }
    }
}