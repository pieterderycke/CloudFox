using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CloudFox.Weave.Util;
using System.Text;

namespace CloudFox.Weave.Tests
{
    [TestClass]
    public class Base32ConvertTests
    {
        [TestMethod]
        public void TestBase32Decoding()
        {
            byte[] bytes = Base32Convert.FromBase32String("MZXW6YTBOJRGCZTPN5RGC4TCME");
            string value = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            Assert.AreEqual("foobarbafoobarba\0", value);
        }

        [TestMethod]
        public void TestUserFriendlyBase32Decoding()
        {
            byte[] bytes = Base32Convert.UserfriendlyBase32Decoding("mzxw6ytb9jrgcztpn5rgc4tcme");
            string value = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            Assert.AreEqual("foobarbafoobarba\0", value);
        }
    }
}
