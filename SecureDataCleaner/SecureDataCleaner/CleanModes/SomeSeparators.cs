using System;
using System.Text.RegularExpressions;

namespace SecureDataCleaner.CleanModes
{
    public class SomeSeparators
    {
        public char Replacement { get; set; }

        public SomeSeparators(string template)
        {
            TemplateToRegex(template);
        }

        /// <summary>
        /// Преобразование шаблона в регулярное выражение.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private Regex TemplateToRegex(string template)
        {
            var cleanTemplate = template;

            //установка шабло на любое количество разделителей
            cleanTemplate = Regex.Replace(cleanTemplate, @"(\w+|\s+)|(\W)", "\\s+$1$2\\s+");
            cleanTemplate = Regex.Replace(cleanTemplate, @"\s*", String.Empty);
            cleanTemplate = Regex.Replace(cleanTemplate, @"(\\s\+)+", "\\s*");

            string testPattern;

            testPattern = Regex.Replace(@"{templKey:([^}]*)}", @"(\w+|\s+)|(\W)", "\\s+$1$2\\s+");
            testPattern = Regex.Replace(testPattern, @"\s*", String.Empty);
            testPattern = Regex.Replace(testPattern, @"(\\s\+)+", "\\s*");


            var templateKeys = Regex.Matches(cleanTemplate, Regex.Escape(testPattern));
            var templateValues = Regex.Matches(cleanTemplate, @"{value:([^}]*)}");

            string regexString = cleanTemplate;

            //экранирование управляющих символов в шаблоне
            regexString = Regex.Replace(regexString, @"[\-\[\]\(\)\*\+\?\.,\\\^|#]", @"\$&");

            //хранение имен ключевых полей вроде как бесполезно
            //TODO возможно удалить!
            foreach (Match templateKey in templateKeys)
            {
                regexString = regexString.Replace(templateKey.Groups[0].Value,
                    String.Format("(?<{0}>.*)", templateKey.Groups[1].Value));
            }

            foreach (Match templateValue in templateValues)
            {
                regexString = regexString.Replace(templateValue.Groups[0].Value,
                    String.Format("(?<value{0}>.*)", templateValue.Groups[1].Value));
                //_valueGroupNames.Add(String.Format("value{0}", templateValue.Groups[1].Value));
            }
            return new Regex(regexString);
        }
    }
}