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
            var lalka = new SomeSeparators("{user:{value:'{templValue:user}},pass:{value:'{templValue:pass}'}}");
            var rndStringUsr = Path.GetRandomFileName();
            var rndStringPass = Path.GetRandomFileName();
            lalka.CleanString("{\r\n       user : {\r\n             value : \'max\'\r\n       },\r\n       pass : {\r\n             value : \'123456\'\r\n       }\r\n}");
        }

        
    }
}
