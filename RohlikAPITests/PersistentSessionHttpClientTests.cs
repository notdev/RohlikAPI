﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPISharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RohlikAPISharp.Tests
{
    [TestClass()]
    public class PersistentSessionHttpClientTests
    {
        [TestMethod]
        public void GetTest()
        {
            const string testCookieName = "testCookieName";
            const string testCookieValue = "testCookieValue";

            var client = new PersistentSessionHttpClient();

            client.Get($"http://httpbin.org/cookies/set/{testCookieName}/{testCookieValue}");
            var response = client.Get("http://httpbin.org/cookies");

            Assert.IsTrue(response.Contains(testCookieName) && response.Contains(testCookieValue), $"Response should contain both {testCookieName} and {testCookieValue}");
        }
    }
}