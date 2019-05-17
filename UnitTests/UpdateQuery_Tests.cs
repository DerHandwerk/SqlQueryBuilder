using SqlQueryBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass()]
    public class UpdateQuery_Tests
    {
        [TestMethod()]
        public void test_Update()
        {
            var u = new UpdateQuery()
            {
                Table = "TestTable",
                Values =
                {
                    { "Column1", 1},
                    { "Column2", true},
                    { "Column3", "test"}
                }
            };

            Assert.AreEqual("UPDATE TestTable SET Column1 = 1, Column2 = 1, Column3 = 'test';", u.ToString());
        }

        [TestMethod()]
        public void test_Update_Where()
        {
            var u = new UpdateQuery()
            {
                Table = "TestTable",
                Values =
                {
                    { "Column1", 1},
                    { "Column2", true},
                    { "Column3", "test"}
                },
                Where = new Equals
                {
                    Left = "Column1",
                    Right = 1
                }
            };

            Assert.AreEqual("UPDATE TestTable SET Column1 = 1, Column2 = 1, Column3 = 'test' WHERE ( Column1 = 1 );", u.ToString());

            u.Where = new And
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

            Assert.AreEqual("UPDATE TestTable SET Column1 = 1, Column2 = 1, Column3 = 'test' WHERE ( ( Column1 = 1 ) AND ( Column2 = 1 ) );", u.ToString());
        }
    }
}