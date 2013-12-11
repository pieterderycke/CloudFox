﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CloudFox.Weave.Util;

namespace CloudFox.Weave.Tests
{
    [TestClass]
    public class WeaveKeysTests
    {
        [TestMethod]
        public void TestSimplifiedCryptoKeyDerivation()
        {
            string userName = "stefan@arentz.ca";
            string passphrase = "p5zmnhdhc4v2ek5mfvzkzvb23i";

            byte[] correctEncrKeyBytes = new byte[32] {
		        0x19, 0xe4, 0x95, 0x74, 0x18, 0x25, 0x68, 0x9b, 0x2a, 0xe2, 0x94, 0xc4, 0xa2, 0xc8, 0x36, 0x1c,
                0xae, 0x31, 0x39, 0x78, 0x35, 0x24, 0xbe, 0xfd, 0xad, 0xaa, 0xff, 0x80, 0xe7, 0x5a, 0x36, 0xc0
	        };

            byte[] correctHmacKeyBytes = new byte[32] {
		        0xa5, 0x1c, 0xf1, 0x2f, 0xf3, 0xb0, 0x08, 0xf7, 0xff, 0x07, 0x28, 0x5e, 0x10, 0x34, 0x02, 0x4d,
                0x25, 0x2b, 0x2e, 0x89, 0xf2, 0x20, 0xd4, 0xca, 0x84, 0x4e, 0x29, 0x1a, 0x2a, 0x11, 0x12, 0x36
	        };

            byte[] passphraseData = Base32Convert.UserfriendlyBase32Decoding(passphrase);

            WeaveKeys keys = new WeaveKeys(passphraseData, userName);

            AssertExtended.AreEqual(correctHmacKeyBytes, keys.HmacKey);
            AssertExtended.AreEqual(correctEncrKeyBytes, keys.CryptoKey);
        }
    }
}