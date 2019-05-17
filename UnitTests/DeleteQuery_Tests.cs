using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlQueryBuilder;

namespace UnitTests
{
    [TestClass()]
    public class DeleteQuery_Tests
    {
        [TestMethod()]
        public void test_Delete()
        {
            var d = new DeleteQuery()
            {
                Table = "TestTable"
            };

            Assert.AreEqual("DELETE FROM TestTable;", d.ToString());
        }

        [TestMethod()]
        public void test_Delete_Where()
        {
            var d = new DeleteQuery()
            {
                Table = "TestTable",
                Where = new Equals
                {
                    Left = "Column1",
                    Right = 1
                }
            };

            Assert.AreEqual("DELETE FROM TestTable WHERE ( Column1 = 1 );", d.ToString());

            d.Where = new And
            {
                Left = new Equals
                {
                    Left = "Column1",
                    Right = 1
                },
                Right = new Equals
                {
                    Left = "Column2",
                    Right = 1
                },
            };

            Assert.AreEqual("DELETE FROM TestTable WHERE ( ( Column1 = 1 ) AND ( Column2 = 1 ) );", d.ToString());
        }
    }
}
