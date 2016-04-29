using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SecureDataCleaner.Interfaces;
using SecureDataCleaner.Types;

namespace SecureDataCleaner.CleanModes
{
    /// <summary>
    /// Очистка любой строки по шаблону 
    /// </summary>
    public class SomeSeparators : ICleanMode
    {
        private readonly Regex _regexTemplate;

        private readonly List<string> _valueGroupNames = new List<string>();

#region Public

        /// <summary>
        /// Символ замены, default = 'X'
        /// </summary>
        public char Replacement { get; set; }

        /// <summary>
        /// Инициализирует экземпляр класса с шаблоном template
        /// </summary>
        /// <param name="template">Шаблон для объекта</param>
        public SomeSeparators(string template)
        {
            Replacement = 'X';
            try
            {
                _regexTemplate = TemplateToRegex(template);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid template");
            }
        }
        
        /// <summary>
        /// Очистка строки по шаблону объекта
        /// </summary>
        /// <param name="secureString">Входная строка с secure-данными</param>
        /// <returns>Результат очистки</returns>
        public CleanResult CleanString(string secureString)
        {
            var matches = _regexTemplate.Matches(secureString);
            if (matches.Count == 0)
            {
                throw new ArgumentException("Invalid string");
            }
            var cleanResult = new CleanResult();
            foreach (var groupName in _valueGroupNames)
            {
                Group secureValue = matches[0].Groups[groupName];
                secureString = secureString.Remove(secureValue.Index, secureValue.Length);
                secureString = secureString.Insert(secureValue.Index, new String(Replacement, secureValue.Length));
                cleanResult.SecureData.Add(new SecureData
                {
                    Key = groupName,
                    Value = secureValue.Value
                });
            }
            cleanResult.CleanString = secureString;
            return cleanResult;
        }

#endregion

#region Private

        /// <summary>
        /// Преобразование шаблона в регулярное выражение.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private Regex TemplateToRegex(string template)
        {
            var cleanTemplate = template;

            //выборка из шаблона key и value
            var templateKeys = Regex.Matches(cleanTemplate, @"{templKey:([^}]*)}");
            var templateValues = Regex.Matches(cleanTemplate, @"{templValue:([^}]*)}");

            //установка шаблона на любое количество разделителей
            cleanTemplate = Regex.Replace(cleanTemplate, @"(\W)", @"\$&");
            cleanTemplate = Regex.Replace(cleanTemplate, @"(\w+|\s+)|(\\\W)", @"\s*$&");
            cleanTemplate = Regex.Replace(cleanTemplate, @"$", @"\s*");

            // возвращение значний value и key в шаблон (разбиты символами  -разделителями)
            RepairTemplInset(templateKeys, ref cleanTemplate);
            RepairTemplInset(templateValues, ref cleanTemplate);

            string regexString = cleanTemplate;

            //хранение имен ключевых полей вроде как бесполезно
            //TODO возможно удалить!
            foreach (Match templateKey in templateKeys)
            {
                regexString = regexString.Replace(templateKey.Groups[0].Value,
                    String.Format("(?<key{0}>[.\\S]*)", templateKey.Groups[1].Value));
            }

            foreach (Match templateValue in templateValues)
            {
                regexString = regexString.Replace(templateValue.Groups[0].Value,
                    String.Format("(?<{0}>[.\\S]*)", templateValue.Groups[1].Value));
                _valueGroupNames.Add(String.Format("{0}", templateValue.Groups[1].Value));
            }
            return new Regex(regexString);
        }

        /// <summary>
        /// Восстановление шаблонных вставок типа {templKey:...} & {templValue:...}
        /// </summary>
        /// <param name="collection">Сохраненные значения вставок</param>
        /// <param name="templString">Восстанавливаемая строка</param>
        private void RepairTemplInset(MatchCollection collection, ref string templString)
        {
            foreach (Match templKey in collection)
            {
                var pattern = Regex.Replace(templKey.Value, @"(\W)", @"\$&");
                pattern = Regex.Replace(pattern, @"(\w+|\s+)|(\\\W)", @"\s*$&");
                pattern = Regex.Replace(pattern, @"^\\s\*|\\s\*$", String.Empty);
                templString = Regex.Replace(templString, Regex.Escape(pattern), templKey.Value);
            }
        }

#endregion
    }
}