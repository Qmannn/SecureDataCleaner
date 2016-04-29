using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecureDataCleaner.CleanModes;

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
            var noSpacesMode = new NoSpaces("{{key:user}:'{value:user}',{key:pass}:'{value:pass}'}");
            for (int j = 0; j < 1000; j++)
            {
                new Thread(() =>
                {
                    for (int i = 1; i < 100; i++)
                    {
                        var rndStringUsr = Path.GetRandomFileName();
                        var rndStringPass = Path.GetRandomFileName();
                        var cleanRes = noSpacesMode.CleanString("{user:'" + rndStringUsr + "',pass:'" + rndStringPass + "'}");
                        Assert.AreEqual(cleanRes.CleanString,
                            "{user:'" + new String(noSpacesMode.Replacement, rndStringUsr.Length) + "',pass:'" +
                            new String(noSpacesMode.Replacement, rndStringPass.Length) + "'}");
                        Assert.AreEqual(cleanRes.SecureData[0].Value, rndStringUsr);
                        Assert.AreEqual(cleanRes.SecureData[1].Value, rndStringPass);
                    }
                }).Start();
            }
        }

        [TestMethod]
        public void SomeSeparatorsTest()
        {
            
        }
    }
}
