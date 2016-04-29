using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                new NoSpaces("http://test.com?user={templValue:user}&pass={templValue:pass}"),
                new NoSpaces("<auth><user>{templValue:user}</user><pass>{templValue:pass}</pass></auth>"),
                new SomeSeparators("<auth user='{templValue:user}' pass='{templValue:pass}'>"))
            {
                DataSaver = Saver
            });
            for (int j = 0; j < 20; j++)
            {
                new Thread(() =>
                {
                    for (int i = 1; i < 100; i++)
                    {
                        var rndStringUsr = Path.GetRandomFileName();
                        var rndStringPass = Path.GetRandomFileName();
                        AgodaHttpResult agodaHttpResult = new AgodaHttpResult
                        {
                            Url = "http://test.com?user=" + rndStringPass + "&pass=" + rndStringPass,
                            RequestBody = "<auth><user>" + rndStringUsr + "</user><pass>" + rndStringPass + "</pass></auth>",
                            ResponseBody = "<auth user='" + rndStringUsr + "' pass='" + rndStringPass + "'>"
                        };
                        agodaHttpResult = (AgodaHttpResult)resCleaner.Clean(agodaHttpResult);
                    }
                }).Start();
            }

        }

        private static readonly object syncRoot = new object();

        static void Saver(List<SecureData> data)
        {
            foreach (var val in data)
            {
                lock (syncRoot)
                {
                    Console.WriteLine("key: {0}, val: {1}", val.Key, val.Value);
                    using (FileStream fs = new FileInfo("test.txt").OpenWrite())
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine("Time for {0} cleans: {1} ms", val.Key, val.Value);
                        }
                    }   
                }
            }
        }

        
    }
}
