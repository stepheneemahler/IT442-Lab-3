using NUnit.Framework;

using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    public class ProductDBTests
    {
        ProductDB db;

        [SetUp]
        public void ResetData()
        {
            db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            Assert.AreEqual(1, p.ProductID);
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Assert.AreEqual(56.50m, p.UnitPrice);
            Assert.AreEqual(4637, p.OnHandQuantity);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll();
            Assert.AreEqual(16, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            ProductProps p = new ProductProps();
            p.ProductCode = "DEL1";
            p.Description = "Product to delete";
            p.UnitPrice = 10.00m;
            p.OnHandQuantity = 1;
            db.Create(p);

            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(p.ProductID));
        }

        [Test]
        public void TestUpdate()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            p.Description = "Updated Description";
            p.UnitPrice = 60.00m;
            Assert.True(db.Update(p));
            p = (ProductProps)db.Retrieve(1);
            Assert.AreEqual("Updated Description", p.Description);
            Assert.AreEqual(60.00m, p.UnitPrice);
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            p.Description = new string('x', 51);
            Assert.Throws<MySqlException>(() => db.Update(p));
        }

        [Test]
        public void TestCreate()
        {
            ProductProps p = new ProductProps();
            p.ProductCode = "NEW1";
            p.Description = "Test Product";
            p.UnitPrice = 19.99m;
            p.OnHandQuantity = 10;
            db.Create(p);
            ProductProps p2 = (ProductProps)db.Retrieve(p.ProductID);
            Assert.AreEqual(p.ProductCode, p2.ProductCode);
            Assert.AreEqual(p.Description, p2.Description);
            Assert.AreEqual(p.UnitPrice, p2.UnitPrice);
            Assert.AreEqual(p.OnHandQuantity, p2.OnHandQuantity);
        }

        [Test]
        public void TestCreateCodeTooLong()
        {
            ProductProps p = new ProductProps();
            p.ProductCode = "TOOLONGCODE1";
            p.Description = "Test Product";
            p.UnitPrice = 19.99m;
            p.OnHandQuantity = 10;
            Assert.Throws<MySqlException>(() => db.Create(p));
        }
    }
}