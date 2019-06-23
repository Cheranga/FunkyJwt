using System;
using System.Security.Cryptography;
using Xunit;

namespace FunkyDI.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[64];
                rng.GetBytes(key);

                var randomKey = Convert.ToBase64String(key);
            }
        }
    }
}