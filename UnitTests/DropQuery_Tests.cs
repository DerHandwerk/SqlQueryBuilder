using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlQueryBuilder;

namespace UnitTests
{
    [TestClass()]
    public class DropQuery_Tests
    {
        [TestMethod()]
        public void test_Drop()
        {
            var d = new DropQuery
            {
                Table = "TestTable"  
            };

            Assert.AreEqual("DROP TABLE TestTable;", d.ToString());
        }

        [TestMethod()]
        public void test_Drop_IfExists()
        {
            var d = new DropQuery
            {
                Table = "TestTable",
                IfExists = true
            };

            Assert.AreEqual("DROP TABLE IF EXISTS TestTable;", d.ToString());
        }
    }
}
