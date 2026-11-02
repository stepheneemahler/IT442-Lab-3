using NUnit.Framework;

using MMABooksBusiness;
using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {

        [SetUp]
        public void TestResetDatabase()
        {
            ProductDB db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewProductConstructor()
        {
            // not in Data Store - no values yet
            Product p = new Product();
            Assert.AreEqual(string.Empty, p.ProductCode);
            Assert.AreEqual(string.Empty, p.Description);
            Assert.AreEqual(0m, p.UnitPrice);
            Assert.AreEqual(0, p.OnHandQuantity);
            Assert.IsTrue(p.IsNew);
            Assert.IsFalse(p.IsValid);
        }

        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Product p = new Product(1);
            Assert.AreEqual(1, p.ProductID);
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.AreEqual(56.50m, p.UnitPrice);
            Assert.IsFalse(p.IsNew);
            Assert.IsTrue(p.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Product p = new Product();
            p.ProductCode = "NEW1";
            p.Description = "Test Product";
            p.UnitPrice = 19.99m;
            p.OnHandQuantity = 10;
            p.Save();
            Product p2 = new Product(p.ProductID);
            Assert.AreEqual(p2.ProductCode, p.ProductCode);
            Assert.AreEqual(p2.Description, p.Description);
            Assert.AreEqual(p2.UnitPrice, p.UnitPrice);
            Assert.AreEqual(p2.OnHandQuantity, p.OnHandQuantity);
        }

        [Test]
        public void TestUpdate()
        {
            Product p = new Product(1);
            p.Description = "Edited Description";
            p.Save();

            Product p2 = new Product(1);
            Assert.AreEqual(p2.ProductID, p.ProductID);
            Assert.AreEqual(p2.Description, p.Description);
        }

        [Test]
        public void TestDelete()
        {
            Product p = new Product();
            p.ProductCode = "DEL1";
            p.Description = "Product to delete";
            p.UnitPrice = 10.00m;
            p.Save();
            int id = p.ProductID;

            Product p2 = new Product(id);
            p2.Delete();
            p2.Save();
            Assert.Throws<Exception>(() => new Product(id));
        }

        [Test]
        public void TestGetList()
        {
            Product p = new Product();
            List<Product> products = (List<Product>)p.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - code, description, and price must be provided
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - code, description, and price must be provided
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
            p.ProductCode = "NEW2";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Product p = new Product();
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = "TOOLONGCODE1");
            Assert.Throws<ArgumentOutOfRangeException>(() => p.UnitPrice = -1m);
            Assert.Throws<ArgumentOutOfRangeException>(() => p.OnHandQuantity = -5);
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Product p1 = new Product(1);
            Product p2 = new Product(1);

            p1.Description = "Updated first";
            p1.Save();

            p2.Description = "Updated second";
            Assert.Throws<Exception>(() => p2.Save());
        }
    }
}