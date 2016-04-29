using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SecureDataCleaner.CleanModes;

namespace SecureCleanerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var lalka = new SomeSeparators("{{templKey:user}:'{templValue:user}',{templKey:pass}:[]'{templValue:pass}'}");
            var rndStringUsr = Path.GetRandomFileName();
            var rndStringPass = Path.GetRandomFileName();
            var cleanRes = lalka.Check("{user    :'" + rndStringUsr + "',   pass:[]'" + rndStringPass + "'  }");
            lalka.Check("{user:   {value     :    ”max”},     pass:{value:”123456”}}");

            var cleanTemplate = "{user:   {value     :    ”   max   ”},     pass:{value:”123456”}}";
            cleanTemplate = Regex.Replace(cleanTemplate, @"(\w+|\s+)|(\W)", "\\s+$1$2\\s+");
            cleanTemplate = Regex.Replace(cleanTemplate, @"\s*", String.Empty);
            cleanTemplate = Regex.Replace(cleanTemplate, @"(\\s\+)+", "\\s*");
            var matches = Regex.Matches("{user:{value:”max”},pass:{value:”123456”}}", cleanTemplate);
            NoSpaces lol = new NoSpaces("http://test.com/users/{value:user}/info");
            var newString = lol.CleanString(" <auth user=\"max\" pass=\"123456\">");
            Console.WriteLine(newString.CleanString);
        }

        
    }
}
