using SqlQueryBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass()]
    public class SelectQuery_Tests
    {
        [TestMethod()]
        public void test_SelectAll()
        {
            var s = new SelectQuery()
            {
                From = "TestTable"
            };

            Assert.AreEqual("SELECT * FROM TestTable;", s.ToString());
        }

        [TestMethod()]
        public void test_SelectColumns()
        {
            var s = new SelectQuery()
            {
                Columns = {"Column1", "Column2", "Column3"},
                From = "TestTable"
            };

            Assert.AreEqual("SELECT Column1, Column2, Column3 FROM TestTable;", s.ToString());
        }

        [TestMethod()]
        public void test_SelectColumns_Where()
        {
            var s = new SelectQuery()
            {
                Columns = {"Column1", "Column2", "Column3"},
                From = "TestTable",
                Where = new Equals
                {
                    Left = "Column1",
                    Right = 1
                }
            };

            Assert.AreEqual("SELECT Column1, Column2, Column3 FROM TestTable WHERE ( Column1 = 1 );", s.ToString());

            s.Where = new And
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

            Assert.AreEqual("SELECT Column1, Column2, Column3 FROM TestTable WHERE ( ( Column1 = 1 ) AND ( Column2 = 1 ) );", s.ToString());
        }
    }
}