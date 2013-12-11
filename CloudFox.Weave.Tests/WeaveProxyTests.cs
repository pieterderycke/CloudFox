using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CloudFox.Weave.Tests
{
    [TestClass]
    public class WeaveProxyTests
    {
        private const string TestUserName = "testwp7user";
        private const string TestPassword = "testwp7password";
        private const string TestPassphrase = "testwp7passphrase";
        private const string TestWeaveNodeAddress = "https://testhost";

        //[TestMethod]
        public void TestProxyInitializesCommunicationChannel()
        {
            // Configure the channel mock
            CommunicationChannelMock channelMock = new CommunicationChannelMock();
            channelMock.AddResponse("https://auth.services.mozilla.com/user/1.0/" + TestUserName + "/node/weave", 
                RestOperation.Get, TestWeaveNodeAddress);
            channelMock.AddResponse(TestWeaveNodeAddress + "/1.1/" + TestUserName + "/storage/meta/global",
                RestOperation.Get, new GlobalMetaData() { StorageVersion = 5, SyncId = "dfsf" });

            WeaveProxy proxy = new WeaveProxy(channelMock, TestUserName, TestPassword, TestPassphrase);

            Assert.IsTrue(channelMock.IsInitialized);
        }
    }
}
