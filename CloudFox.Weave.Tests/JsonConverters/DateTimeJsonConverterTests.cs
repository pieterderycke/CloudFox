using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CloudFox.Weave.Util;

namespace CloudFox.Weave.Tests.JsonConverters
{
    [TestClass]
    public class DateTimeJsonConverterTests
    {
        [TestMethod]
        public void ConvertDateTimeToString()
        {
            DateTime date = new DateTime(2011, 12, 21);

            JsonWriterMock writerMock = new JsonWriterMock();
            writerMock.OnWriteValue<double>(v => Assert.AreEqual(1324425600000000, v));

            DateTimeJsonConverter converter = new DateTimeJsonConverter();
            converter.WriteJson(writerMock, date, null);
        }

        [TestMethod]
        public void ConvertStringToDateTime()
        {
            long dateValue = 1324425600000000;

            DateTimeJsonConverter converter = new DateTimeJsonConverter();
            DateTime parsedDate = (DateTime)converter.ReadJson(new JsonReaderMock(dateValue), 
                typeof(DateTime), null, null);

            Assert.IsNotNull(parsedDate);
            Assert.AreEqual(new DateTime(2011, 12, 21), parsedDate);
        }
    }
}
