using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoctorsApp.Model;

namespace DoctorsAppUnitTest
{
    [TestClass]
    public class AddressModelTest
    {
        /// <summary>
        /// Check that all properties are set by the address constructor
        /// </summary>
        [TestMethod]
        public void Address_Constructor_SetsAllProperties()
        {
            var address = GenerateAddress();

            Assert.AreEqual(address.Street, "123 Main St");
            Assert.AreEqual(address.City, "San Francisco");
            Assert.AreEqual(address.StateCode, "CA");
            Assert.AreEqual(address.Zip, 93413);
        }

        /// <summary>
        /// Test the FullAddress getters address format
        /// </summary>
        [TestMethod]
        public void Address_FullAddress_ReturnsFullAddress()
        {
            var address = GenerateAddress();

            Assert.AreEqual(address.FullAddress, "123 Main St, San Francisco, CA 93413");
        }

        private Address GenerateAddress()
        {
            return new Address("123 Main St", "San Francisco", "CA", 93413);
        }
    }
}
