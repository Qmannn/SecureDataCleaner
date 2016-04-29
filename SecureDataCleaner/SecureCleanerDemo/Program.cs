using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SecureDataCleaner;
using SecureDataCleaner.CleanModes;
using SecureDataCleaner.Interfaces;
using SecureDataCleaner.Types;

namespace SecureCleanerDemo
{
    class AgodaHttpResult : IHttpResult
    {
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            HttpResultCleaner resCleaner = new HttpResultCleaner(new DefaultCleaner(
                new NoSpaces("<auth><user>{templValue:user}</user><pass>{templValue:pass}</pass></auth>"), 
                null, 
                null)
            {
                DataSaver = Saver
            });
            
            AgodaHttpResult agodaHttpResult = new AgodaHttpResult
            {
                Url = "<auth><user>max</user><pass>123456</pass></auth>",
                ResponseBody = "lol",
                RequestBody = "Как бы не лохануться.. Чет запутался совсем."
            };
            var result = resCleaner.Clean(agodaHttpResult) as AgodaHttpResult;

            agodaHttpResult = new AgodaHttpResult
            {
                Url = "<auth><user>KJLSLHjh*+-asd0_+_{}</user><pass>qwertyСЛОЖНААА</pass></auth>",
                ResponseBody = "lol",
                RequestBody = "Как бы не лохануться.. Чет запутался совсем."
            };
            result = resCleaner.Clean(agodaHttpResult) as AgodaHttpResult;

            Console.WriteLine("Url = {0}", result.Url);
            Console.WriteLine("RequestBody = {0}", result.RequestBody);
            Console.WriteLine("ResponseBody = {0}", result.ResponseBody);
            Console.ReadKey();
        }

        static void Saver(List<SecureData> data)
        {
            foreach (var val in data)
            {
                Console.WriteLine("key: {0}, val: {1}", val.Key, val.Value);
            }
        }

        
    }
}
