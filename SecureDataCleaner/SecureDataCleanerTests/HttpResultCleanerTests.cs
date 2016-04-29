using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecureDataCleaner;
using SecureDataCleaner.CleanModes;
using SecureDataCleaner.Interfaces;

namespace SecureDataCleanerTests
{
    class AgodaHttpResult : IHttpResult
    {
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
    }

    [TestClass]
    public class HttpResultCleanerTests
    {
        [TestMethod]
        public void HttpResultCleanerTest()
        {
            HttpResultCleaner resCleaner = new HttpResultCleaner(new DefaultCleaner(
                new NoSpaces("http://test.com?user={templValue:user}&pass={templValue:pass}"), 
                new NoSpaces("<auth><user>{templValue:user}</user><pass>{templValue:pass}</pass></auth>"),
                new SomeSeparators("<auth user='{templValue:user}' pass='{templValue:pass}'>")));
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
                agodaHttpResult = (AgodaHttpResult) resCleaner.Clean(agodaHttpResult);
                Assert.AreEqual(agodaHttpResult.Url,
                    "http://test.com?user=" + new String('X', rndStringUsr.Length) + "&pass=" +
                    new String('X', rndStringUsr.Length));
                Assert.AreEqual(agodaHttpResult.RequestBody,
                    "<auth><user>" + new String('X', rndStringUsr.Length) + "</user><pass>" +
                    new String('X', rndStringUsr.Length) + "</pass></auth>");
                Assert.AreEqual(agodaHttpResult.ResponseBody,
                    "<auth user='" + new String('X', rndStringUsr.Length) + "' pass='" +
                    new String('X', rndStringUsr.Length) + "'>");
            }
        }


        [TestMethod]
        public void HttpResultCleanerThreadingTest()
        {
            HttpResultCleaner resCleaner = new HttpResultCleaner(new DefaultCleaner(
                new NoSpaces("http://test.com?user={templValue:user}&pass={templValue:pass}"),
                new NoSpaces("<auth><user>{templValue:user}</user><pass>{templValue:pass}</pass></auth>"),
                new SomeSeparators("<auth user='{templValue:user}' pass='{templValue:pass}'>")));
            for (int j = 0; j < 100; j++)
            {
                new Thread(() =>
                {
                    for (int i = 1; i < 10000; i++)
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
                        Assert.AreEqual(agodaHttpResult.Url,
                            "http://test.com?user=" + new String('X', rndStringUsr.Length) + "&pass=" +
                            new String('X', rndStringUsr.Length));
                        Assert.AreEqual(agodaHttpResult.RequestBody,
                            "<auth><user>" + new String('X', rndStringUsr.Length) + "</user><pass>" +
                            new String('X', rndStringUsr.Length) + "</pass></auth>");
                        Assert.AreEqual(agodaHttpResult.ResponseBody,
                            "<auth user='" + new String('X', rndStringUsr.Length) + "' pass='" +
                            new String('X', rndStringUsr.Length) + "'>");
                    }
                }).Start();
            }
        }
    }
}
