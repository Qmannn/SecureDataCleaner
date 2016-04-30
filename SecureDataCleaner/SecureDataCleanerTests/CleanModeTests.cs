using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecureDataCleaner;
using SecureDataCleaner.CleanModes;
using SecureDataCleaner.Interfaces;

namespace SecureDataCleanerTests
{
    [TestClass]
    public class CleanModeTests
    {
        [TestMethod]
        public void NoSpacesTest()
        {
            var noSpacesMode = new NoSpaces("{{templKey:user}:'{templValue:user}',{templKey:pass}:'{templValue:pass}'}");
            for (int i = 1; i < 100000; i++)
            {
                var rndStringUsr = Path.GetRandomFileName();
                var rndStringPass = Path.GetRandomFileName();
                var cleanRes = noSpacesMode.CleanString("{user    :'" + rndStringUsr + "',   pass:'" + rndStringPass + "'  }");
                Assert.AreEqual(cleanRes.CleanString,
                    "{user:'" + new String(noSpacesMode.Replacement, rndStringUsr.Length) + "',pass:'" +
                    new String(noSpacesMode.Replacement, rndStringPass.Length) + "'}");
                Assert.AreEqual(cleanRes.SecureData[0].Value, rndStringUsr);
                Assert.AreEqual(cleanRes.SecureData[1].Value, rndStringPass);
            }

            noSpacesMode = new NoSpaces("http://test.com/users/{templValue:user}/info");
            for (int i = 1; i < 100; i++)
            {
                var rndStringUsr = Path.GetRandomFileName();
                var cleanRes = noSpacesMode.CleanString("http://test.com/users/"+ rndStringUsr +"/info");
                Assert.AreEqual(cleanRes.CleanString,
                    "http://test.com/users/" + new String(noSpacesMode.Replacement, rndStringUsr.Length) + "/info");
                Assert.AreEqual(cleanRes.SecureData[0].Value, rndStringUsr);
            }
        }

        [TestMethod]
        public void NoSpacesThreadingTest()
        {
            var noSpacesMode = new NoSpaces("{{templKey:user}:'{templValue:user}',{templKey:pass}:'{templValue:pass}'}");
            var threaadList = new List<Thread>();
            bool someThreadHasError = false;
            for (int j = 0; j < 50; j++)
            {
                var thread = new Thread(() =>
                {
                    for (int i = 1; i < 10000 && !someThreadHasError; i++)
                    {
                        var rndStringUsr = Path.GetRandomFileName();
                        var rndStringPass = Path.GetRandomFileName();
                        var cleanRes = noSpacesMode.CleanString("{user:'" + rndStringUsr + "',pass:'" + rndStringPass + "'}");
                        someThreadHasError = cleanRes.CleanString !=
                            "{user:'" + new String(noSpacesMode.Replacement, rndStringUsr.Length) + "',pass:'" +
                            new String(noSpacesMode.Replacement, rndStringPass.Length) + "'}" && !someThreadHasError;
                        someThreadHasError = cleanRes.SecureData[0].Value != rndStringUsr && !someThreadHasError;
                        someThreadHasError = cleanRes.SecureData[1].Value != rndStringPass && !someThreadHasError;
                    }
                    lock (threaadList)
                    {
                        threaadList.Remove(Thread.CurrentThread);
                    }
                });
                lock (threaadList)
                {
                    threaadList.Add(thread);
                }
                thread.Start();
            }
            int threadCount = threaadList.Count;
            while (threadCount != 0)
            {
                Thread.Sleep(1000);
                lock (threaadList)
                {
                    threadCount = threaadList.Count;
                }
            }
            Assert.AreEqual(threadCount, 0);
            Assert.AreEqual(someThreadHasError, false);
        }

        [TestMethod]
        public void SomeSeparatorsTest()
        {
            var someSeparate = new SomeSeparators("{{templKey:user}:'{templValue:user}',{templKey:pass}:'{templValue:pass}'}");
            for (int i = 1; i < 100000; i++)
            {
                var rndStringUsr = Path.GetRandomFileName();
                var rndStringPass = Path.GetRandomFileName();
                var cleanRes = someSeparate.CleanString("{user    :'" + rndStringUsr + "',   pass:'" + rndStringPass + "'}");
                Assert.AreEqual(cleanRes.CleanString,
                    "{user    :'" + new String(someSeparate.Replacement, rndStringUsr.Length) + "',   pass:'" +
                    new String(someSeparate.Replacement, rndStringPass.Length) + "'}");
                Assert.AreEqual(cleanRes.SecureData[0].Value, rndStringUsr);
                Assert.AreEqual(cleanRes.SecureData[1].Value, rndStringPass);
            }

            someSeparate = new SomeSeparators("http://test.com/users/{templValue:user}/info");
            for (int i = 1; i < 100; i++)
            {
                var rndStringUsr = Path.GetRandomFileName();
                var cleanRes = someSeparate.CleanString("http://test.com/users/" + rndStringUsr + "/info");
                Assert.AreEqual(cleanRes.CleanString,
                    "http://test.com/users/" + new String(someSeparate.Replacement, rndStringUsr.Length) + "/info");
                Assert.AreEqual(cleanRes.SecureData[0].Value, rndStringUsr);
            }

            someSeparate = new SomeSeparators("<auth><user>{templValue:user}</user><pass>{templValue:pass}</pass></auth>");
            for (int i = 1; i < 100; i++)
            {
                var rndStringUsr = Path.GetRandomFileName();
                var rndStringPass = Path.GetRandomFileName();
                var cleanRes = someSeparate.CleanString("<auth><user>" + rndStringUsr + "</user><pass>" + rndStringPass +"</pass></auth>");
                Assert.AreEqual(cleanRes.CleanString,
                    "<auth><user>" + new String(someSeparate.Replacement, rndStringUsr.Length) + "</user><pass>" + new String(someSeparate.Replacement, rndStringUsr.Length) + "</pass></auth>");
                Assert.AreEqual(cleanRes.SecureData[0].Value, rndStringUsr);
            }
        }

        [TestMethod]
        public void SomeSeparatorsThreadingTest()
        {
            var someSeparators = new SomeSeparators("{{templKey:user}:'{templValue:user}',{templKey:pass}:'{templValue:pass}'}");
            var threaadList = new List<Thread>();
            bool someThreadHasError = false;
            for (int j = 0; j < 50; j++)
            {
                var thread = new Thread(() =>
                {
                    for (int i = 1; i < 10000 && !someThreadHasError; i++)
                    {
                        var rndStringUsr = Path.GetRandomFileName();
                        var rndStringPass = Path.GetRandomFileName();
                        var cleanRes = someSeparators.CleanString("{user:'" + rndStringUsr + "',pass:'" + rndStringPass + "'}");
                        someThreadHasError = cleanRes.CleanString !=
                            "{user:'" + new String(someSeparators.Replacement, rndStringUsr.Length) + "',pass:'" +
                            new String(someSeparators.Replacement, rndStringPass.Length) + "'}" && !someThreadHasError;
                        someThreadHasError = cleanRes.SecureData[0].Value != rndStringUsr && !someThreadHasError;
                        someThreadHasError = cleanRes.SecureData[1].Value != rndStringPass && !someThreadHasError;
                    }
                    lock (threaadList)
                    {
                        threaadList.Remove(Thread.CurrentThread);
                    }
                });
                lock (threaadList)
                {
                    threaadList.Add(thread);
                }
                thread.Start();
            }
            int threadCount = threaadList.Count;
            while (threadCount != 0)
            {
                Thread.Sleep(100);
                lock (threaadList)
                {
                    threadCount = threaadList.Count;
                }
            }
            Assert.AreEqual(threadCount, 0);
            Assert.AreEqual(someThreadHasError, false);
        }

        [TestMethod]
        public void SomeSeparatorsPerformanceTest()
        {
            HttpResultCleaner agodaCleaner = new HttpResultCleaner(new DefaultCleaner(
                new SomeSeparators("<auth><user>{templValue:user}</user><pass>{templValue:pass}</pass></auth>"), 
                null,
                null));
            int clearCount = 1000000;
            var timer = Stopwatch.StartNew();
            for (int i = 1; i < clearCount; i++)
            {
                var rndStringUsr = Path.GetRandomFileName();
                var rndStringPass = Path.GetRandomFileName();
                AgodaHttpResult agodaHttpResult = new AgodaHttpResult
                {
                    Url = "<auth><user>" + rndStringUsr + "</user><pass>" + rndStringPass + "</pass></auth>",
                    RequestBody = rndStringUsr,
                    ResponseBody = rndStringPass
                };
                agodaHttpResult = (AgodaHttpResult) agodaCleaner.Clean(agodaHttpResult);
                Assert.AreEqual(agodaHttpResult.Url, "<auth><user>" + new String('X', rndStringUsr.Length) + "</user><pass>" + new String('X', rndStringUsr.Length) + "</pass></auth>");
            }
            timer.Stop();
            using (FileStream fs = new FileInfo("SomeSeparatorsPerformanceTestResult.txt").OpenWrite())
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("Time for {0} cleans: {1} ms", clearCount, timer.Elapsed.TotalMilliseconds);
                }
            }
            
        }

        [TestMethod]
        public void NoSpacesPerformanceTest()
        {
            NoSpacesTest();
            HttpResultCleaner agodaCleaner = new HttpResultCleaner(new DefaultCleaner(
                new NoSpaces("<auth><user>{templValue:user}</user><pass>{templValue:pass}</pass></auth>"),
                null,
                null));
            int clearCount = 1000000;
            var timer = Stopwatch.StartNew();
            for (int i = 1; i < clearCount; i++)
            {
                var rndStringUsr = Path.GetRandomFileName();
                var rndStringPass = Path.GetRandomFileName();
                AgodaHttpResult agodaHttpResult = new AgodaHttpResult
                {
                    Url = "<auth><user>" + rndStringUsr + "</user><pass>" + rndStringPass + "</pass></auth>",
                    RequestBody = rndStringUsr,
                    ResponseBody = rndStringPass
                };
                agodaHttpResult = (AgodaHttpResult) agodaCleaner.Clean(agodaHttpResult);
                Assert.AreEqual(agodaHttpResult.Url, "<auth><user>" + new String('X', rndStringUsr.Length) + "</user><pass>" + new String('X', rndStringUsr.Length) + "</pass></auth>");
            }
            timer.Stop();
            using (FileStream fs = new FileInfo("NoSpacesPerformanceTestResult.txt").OpenWrite())
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("Time for {0} cleans: {1} ms", clearCount, timer.Elapsed.TotalMilliseconds);
                }
            }

        }


        class AgodaHttpResult : IHttpResult
        {
            public string Url { get; set; }
            public string RequestBody { get; set; }
            public string ResponseBody { get; set; }
        }
    }
}
