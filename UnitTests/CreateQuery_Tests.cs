using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlQueryBuilder;

namespace UnitTests
{
    [TestClass()]
    public class CreateQuery_Tests
    {
        [TestMethod()]
        public void test_Create()
        {
            var c = new CreateQuery
            {
                Table = "TestTable",
                Columns =
                {
                    new Column
                    {
                        Name = "Column1",
                        Type = typeof(int)
                    },
                    new Column
                    {
                        Name = "Column2",
                        Type = typeof(bool)
                    },
                    new Column
                    {
                        Name = "Column3",
                        Type = typeof(string)
                    }
                }
            };

            Assert.AreEqual("CREATE TABLE TestTable ( Column1 INTEGER, Column2 INTEGER, Column3 TEXT );", c.ToString());
        }

        [TestMethod()]
        public void test_Create_PrimaryKey()
        {
            var c = new CreateQuery
            {
                Table = "TestTable",
                Columns =
                {
                    new Column
                    {
                        Name = "Column1",
                        Type = typeof(int),
                        Attributes = ColumnAttributes.PrimaryKey
                    },
                    new Column
                    {
                        Name = "Column2",
                        Type = typeof(bool)
                    },
                    new Column
                    {
                        Name = "Column3",
                        Type = typeof(string)
                    }
                }
            };

            Assert.AreEqual("CREATE TABLE TestTable ( Column1 INTEGER PRIMARY KEY, Column2 INTEGER, Column3 TEXT );", c.ToString());
        }

        [TestMethod()]
        public void test_Create_PrimaryKey_AutoIncrement()
        {
            var c = new CreateQuery
            {
                Table = "TestTable",
                Columns =
                {
                    new Column
                    {
                        Name = "Column1",
                        Type = typeof(int),
                        Attributes = ColumnAttributes.PrimaryKey | ColumnAttributes.AutoIncrement
                    },
                    new Column
                    {
                        Name = "Column2",
                        Type = typeof(bool)
                    },
                    new Column
                    {
                        Name = "Column3",
                        Type = typeof(string)
                    }
                }
            };

            Assert.AreEqual("CREATE TABLE TestTable ( Column1 INTEGER PRIMARY KEY AUTOINCREMENT, Column2 INTEGER, Column3 TEXT );", c.ToString());
        }

        [TestMethod()]
        public void test_Create_Unique()
        {
            var c = new CreateQuery
            {
                Table = "TestTable",
                Columns =
                {
                    new Column
                    {
                        Name = "Column1",
                        Type = typeof(int),
                        Attributes = ColumnAttributes.Unique
                    },
                    new Column
                    {
                        Name = "Column2",
                        Type = typeof(bool)
                    },
                    new Column
                    {
                        Name = "Column3",
                        Type = typeof(string)
                    }
                }
            };

            Assert.AreEqual("CREATE TABLE TestTable ( Column1 INTEGER UNIQUE, Column2 INTEGER, Column3 TEXT );", c.ToString());
        }

        [TestMethod()]
        public void test_Create_PrimaryKey_NotNull()
        {
            var c = new CreateQuery
            {
                Table = "TestTable",
                Columns =
                {
                    new Column
                    {
                        Name = "Column1",
                        Type = typeof(int),
                        Attributes = ColumnAttributes.NotNull
                    },
                    new Column
                    {
                        Name = "Column2",
                        Type = typeof(bool)
                    },
                    new Column
                    {
                        Name = "Column3",
                        Type = typeof(string)
                    }
                }
            };

            Assert.AreEqual("CREATE TABLE TestTable ( Column1 INTEGER NOT NULL, Column2 INTEGER, Column3 TEXT );", c.ToString());
        }
    }
}
