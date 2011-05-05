using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureStorageHandler.Tests
{
    /// <summary>
    ///This intended to contain the QueueStorageHandler Unit Tests
    ///</summary>
    [TestClass]
    public class QueueStorageHandlerTest
    {
        QueueStorageHandler QueueStorageHandler;
        CloudQueueMessage firstQueue;
        CloudQueueMessage secondQueue;
        CloudQueueMessage thirdQueue;
        CloudQueueMessage forthQueue;

        /// <summary>
        /// Run this before run my tests in this class
        /// </summary>
        [TestInitialize]
        public void Start()
        {
            //Arrange
            QueueStorageHandler = new QueueStorageHandler("queuecontainertest");
            byte[] bytes = new MemoryStream(ASCIIEncoding.Default.GetBytes("ByteArrayTest")).ToArray();

            firstQueue = new CloudQueueMessage("FirstTextMessageTest");
            secondQueue = new CloudQueueMessage("SecondTextMessageTest");
            thirdQueue = new CloudQueueMessage("ThirdTextMessageTest");
            forthQueue = new CloudQueueMessage(bytes);

            
            //Act
            QueueStorageHandler.InsertMessage(firstQueue);
            QueueStorageHandler.InsertMessage(secondQueue);
            QueueStorageHandler.InsertMessage(thirdQueue);
            QueueStorageHandler.InsertMessage(forthQueue);
        }

        /// <summary>
        /// Run this after run my tests in this class
        /// </summary>
        [TestCleanup]
        public void Finish()
        {
            try
            {
                QueueStorageHandler.DeleteQueue();
            }
            catch
            {

            }
        }

        /// <summary>
        /// A test for RetrieveMessage method
        /// </summary>
        [TestMethod()]
        public void RetrieveMessageTest()
        {
            //Arrange
            string expected = "FirstTextMessageTest";

            //Act
            var actual = QueueStorageHandler.RetrieveMessage();

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.AsString);
        }

        /// <summary>
        /// A test for Insert method
        /// </summary>
        [TestMethod()]
        public void InsertTextMessageTest()
        {
            //Act
            QueueStorageHandler.InsertMessage("InsertTextMessageTest");
        }

        /// <summary>
        /// A test for Insert method
        /// </summary>
        [TestMethod()]
        public void InsertByteArrayMessageTest()
        {
            //Arrange
            byte[] bytes = new MemoryStream(ASCIIEncoding.Default.GetBytes("ByteArrayMessageTest")).ToArray();

            //Act
            QueueStorageHandler.InsertMessage(bytes);
        }

        /// <summary>
        /// A test for Insert method
        /// </summary>
        [TestMethod()]
        public void InsertCloudQueueMessageTest()
        {
            //Arrange
            var cloudQueueMessage = new CloudQueueMessage("InsertCloudQueueMessageTest");

            //Act
            QueueStorageHandler.InsertMessage(cloudQueueMessage);
        }

        /// <summary>
        /// A test for Delete method
        /// </summary>
        [TestMethod()]
        public void DeleteTest1()
        {
            //Arrange
            var messageToDelete = QueueStorageHandler.RetrieveMessage();
            
            //Act
            QueueStorageHandler.DeleteMessage(messageToDelete);
            var notExpected = QueueStorageHandler.RetrieveMessage();

            //Assert
            Assert.AreNotEqual(notExpected, messageToDelete);
        }

        /// <summary>
        /// A test for Delete method
        /// </summary>
        [TestMethod()]
        public void DeleteTest2()
        {
            //Arrange
            var messageToDelete = QueueStorageHandler.RetrieveMessage();
            
            //Act
            QueueStorageHandler.DeleteMessage(messageToDelete);
            var notExpected = QueueStorageHandler.RetrieveMessage();

            //Assert
            Assert.AreNotEqual(notExpected, messageToDelete);
        }

        /// <summary>
        /// A test for CleanQueue method
        /// </summary>
        [TestMethod()]
        public void CleanQueue()
        {
            //Act
            QueueStorageHandler.CleanQueue();
        }
    }
}
