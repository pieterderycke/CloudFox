using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using CloudFox.Weave.Util;

namespace CloudFox.Weave.Tests.JsonConverters
{
    [TestClass]
    public class WeaveJsonDateTimeConverterTests
    {
        [TestMethod]
        public void WriteDateTime()
        {
            DateTime date = new DateTime(2011, 12, 21);

            JsonWriterMock writerMock = new JsonWriterMock();
            writerMock.OnWriteValue<double>(v => Assert.AreEqual(1324425600.0, v));

            WeaveJsonDateTimeConverter converter = new WeaveJsonDateTimeConverter();
            converter.WriteJson(writerMock, date, null);
        }

        [TestMethod]
        public void ReadLongAsDateTime()
        {
            long dateValue = 1324425600;

            WeaveJsonDateTimeConverter converter = new WeaveJsonDateTimeConverter();
            DateTime parsedDate = (DateTime)converter.ReadJson(new JsonReaderMock(dateValue), typeof(DateTime), null, null);

            Assert.IsNotNull(parsedDate);
            Assert.AreEqual(new DateTime(2011, 12, 21), parsedDate);
        }

        [TestMethod]
        public void ReadDoubleAsDateTime()
        {
            double dateValue = 1324425600.0;

            WeaveJsonDateTimeConverter converter = new WeaveJsonDateTimeConverter();
            DateTime parsedDate = (DateTime)converter.ReadJson(new JsonReaderMock(dateValue), typeof(DateTime), null, null);

            Assert.IsNotNull(parsedDate);
            Assert.AreEqual(new DateTime(2011, 12, 21), parsedDate);
        }
    }
}
