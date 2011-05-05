using System;
using System.Collections.Generic;
using System.Linq;
using AzureStorageHandler.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureStorageHandler.Tests
{
    /// <summary>
    ///This intended to contain the TableStorageHandler Unit Tests
    ///</summary>
    [TestClass()]
    public class TableStorageHandlerTests
    {
        TableStorageHandler<TestModel> TableStorageHandler;
        TestModel firstEntity;
        TestModel secondEntity;
        TestModel thirdEntity;
        string tableName;

        /// <summary>
        /// Run this before run my tests in this class
        /// </summary>
        [TestInitialize]
        public void Start()
        {
            //Arrange
            tableName = "UnitTestTable";
            TableStorageHandler = new TableStorageHandler<TestModel>();
            firstEntity = new TestModel() { PartitionKey = "1", RowKey = "", Timestamp = new DateTime(2011, 04, 05, 22, 55, 00), Name = "Sheldon Cooper", BirthDate = new DateTime(1973, 03, 24) };
            secondEntity = new TestModel() { PartitionKey = "2", RowKey = "", Timestamp = new DateTime(2011, 04, 05, 22, 55, 00), Name = "Leonard Hofstadter", BirthDate = new DateTime(1975, 04, 30) };
            thirdEntity = new TestModel() { PartitionKey = "3", RowKey = "", Timestamp = new DateTime(2011, 04, 05, 22, 55, 00), Name = "Howard Wolowitz", BirthDate = new DateTime(1780, 12, 9) };

            //Act
            TableStorageHandler.Insert(tableName, firstEntity);
            TableStorageHandler.Insert(tableName, secondEntity);
            TableStorageHandler.Insert(tableName, thirdEntity);
        }

        /// <summary>
        /// Run this after run my tests in this class
        /// </summary>
        [TestCleanup]
        public void Finish()
        {
            try
            {
                TableStorageHandler.DeleteTable(tableName);
            }
            catch
            {

            }
        }

        /// <summary>
        /// A test for GetAll method
        /// </summary>
        [TestMethod()]
        public void GetAllTest()
        {
            //Arrange
            IEnumerable<TestModel> target;

            //Act
            target = TableStorageHandler.GetAll(tableName);

            //Assert
            Assert.AreEqual(3, target.Count());
        }

        /// <summary>
        /// A test for Where method
        /// </summary>
        [TestMethod()]
        public void Where()
        {
            //Arrange
            IEnumerable<TestModel> actual;

            //Act
            actual = TableStorageHandler.Where(tableName, e => e.Name == "Sheldon Cooper");
            var expected = TableStorageHandler.Where(tableName, e => e.PartitionKey == "1");

            //Assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(expected.FirstOrDefault().Name, actual.FirstOrDefault().Name);
            Assert.AreEqual(expected.FirstOrDefault().BirthDate, actual.FirstOrDefault().BirthDate);
        }

        /// <summary>
        /// A test for Insert method
        /// </summary>
        [TestMethod()]
        public void InsertTest()
        {
            //Arrange
            var newEntity = new TestModel() { PartitionKey = "4", RowKey = "", Timestamp = DateTime.Now, Name = "Amy Farrah Fowler", BirthDate = new DateTime(1975, 12, 12) };

            //Act
            TableStorageHandler.Insert(tableName, newEntity);
        }

        /// <summary>
        /// A test for Update method
        /// </summary>
        [TestMethod()]
        public void UpdateTest()
        {
            //Arrange
            var target = TableStorageHandler.Where(tableName, e => e.PartitionKey == "3").FirstOrDefault();
            target.Name = "Rajesh Koothrappali";
            target.BirthDate = new DateTime(1981, 03, 30);

            var expected = target;

            //Act
            TableStorageHandler.Update(tableName, target);
            var actual = TableStorageHandler.Where(tableName, e => e.PartitionKey == "3").FirstOrDefault();

            //Assert
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.BirthDate, actual.BirthDate);
        }

        /// <summary>
        /// A test for Delete method
        /// </summary>
        [TestMethod()]
        public void DeleteTest()
        {
            TableStorageHandler.Delete(tableName, secondEntity);
        }

        /// <summary>
        /// A test for DeleteTable method
        /// </summary>
        [TestMethod()]
        public void DeleteTableTest()
        {
            TableStorageHandler.DeleteTable(tableName);
        }
    }

    public class TestModel : TableServiceEntity
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
