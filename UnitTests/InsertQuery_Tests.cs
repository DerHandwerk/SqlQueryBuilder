using System;
using SqlQueryBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass()]
    public class InsertQuery_Tests
    {
        [TestMethod()]
        public void InsertQuery_test()
        {
            var i = new InsertQuery
            {
                Table = "TestTable",
                Values =
                {
                    { "Column1", 1},
                    { "Column2", true},
                    { "Column3", "test"}
                }
            };

            Assert.AreEqual("INSERT INTO TestTable ( Column1, Column2, Column3 ) VALUES ( 1, 1, 'test' );", i.ToString());
        }
    }
}
