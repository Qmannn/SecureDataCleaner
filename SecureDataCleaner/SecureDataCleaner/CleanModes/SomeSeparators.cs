using System;
using System.Text.RegularExpressions;

namespace SecureDataCleaner.CleanModes
{
    public class SomeSeparators
    {
        private readonly Regex _regex;

        public char Replacement { get; set; }

        public SomeSeparators(string template)
        {
            _regex = TemplateToRegex(template);
        }

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


            //cleanTemplate = Regex.Replace(cleanTemplate, @"[\-\[\]\(\)\*\+\?\.,\\\^|#]", String.Empty);

            //установка шаблона на любое количество разделителей
            cleanTemplate = Regex.Replace(cleanTemplate, @"(\w+|\s+)|(\W)", "\\s+$1\\s+\\$2\\s+");
            cleanTemplate = Regex.Replace(cleanTemplate, @"\s*", String.Empty);
            cleanTemplate = Regex.Replace(cleanTemplate, @"(\\s\+)+", "\\s*");

            // возвращение значний value и key в шаблон 
            foreach (Match templKey in templateKeys)
            {
                var pattern = Regex.Replace(templKey.Value, @"(\w+|\s+)|(\W)", "\\s+$1\\s+\\$2\\s+");
                pattern = Regex.Replace(pattern, @"\s*", String.Empty);
                pattern = Regex.Replace(pattern, @"(\\s\+)+", "\\s*");
                pattern = Regex.Replace(pattern, @"^\\s\*|\\s\*$", String.Empty);
                cleanTemplate = Regex.Replace(cleanTemplate, Regex.Escape(pattern), templKey.Value);
            }
            foreach (Match templValue in templateValues)
            {
                var pattern = Regex.Replace(templValue.Value, @"(\w+|\s+)|(\W)", "\\s+$1\\s+\\$2\\s+");
                pattern = Regex.Replace(pattern, @"\s*", String.Empty);
                pattern = Regex.Replace(pattern, @"(\\s\+)+", "\\s*");
                pattern = Regex.Replace(pattern, @"^\\s\*|\\s\*$", String.Empty);
                cleanTemplate = Regex.Replace(cleanTemplate, Regex.Escape(pattern), templValue.Value);
            }

            //экранирование управляющих символов в шаблоне

            string regexString = cleanTemplate;

            
            //

            //хранение имен ключевых полей вроде как бесполезно
            //TODO возможно удалить!
            foreach (Match templateKey in templateKeys)
            {
                regexString = regexString.Replace(templateKey.Groups[0].Value,
                    String.Format("(?<{0}>[.\\S]*)", templateKey.Groups[1].Value));
            }

            foreach (Match templateValue in templateValues)
            {
                regexString = regexString.Replace(templateValue.Groups[0].Value,
                    String.Format("(?<value{0}>[.\\S]*)", templateValue.Groups[1].Value));
                //_valueGroupNames.Add(String.Format("value{0}", templateValue.Groups[1].Value));
            }
            return new Regex(regexString);
        }

        public string Check(string val)
        {
            var lol = _regex.Matches(val);
            return null;
        }
    }
}