using System.IO;
using System.Text;
using AzureStorageHandler.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureStorageHandler.Tests
{
    /// <summary>
    ///This is a test class for BlobStorageHandlerTest and is intended
    ///to contain the Get Method Unit Tests
    ///</summary>
    [TestClass()]
    public class BlobStorageHandler_Get_Tests
    {
        BlobStorageHandler BlobStorageHandler;
        string path;
        Stream stream;

        /// <summary>
        /// Run this before run my tests in this class
        /// </summary>
        [TestInitialize]
        public void Start()
        {
            //Arrange
            path = "test-container/TestFile.txt";
            BlobStorageHandler = new BlobStorageHandler();
            stream = new MemoryStream(ASCIIEncoding.Default.GetBytes("StreamTest"));

            //Act
            BlobStorageHandler.SaveFile(stream, path);
        }

        [TestCleanup]
        public void Finish()
        {
            BlobStorageHandler.DeleteFile(path);
        }

        /// <summary>
        ///A test for GetFileFromPath method
        ///</summary>
        [TestMethod()]
        public void GetFileFromPathTest()
        {
            Stream target;

            //Act
            target = BlobStorageHandler.GetFileFromPath(path);

            //Assert
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for GetFileFromPath method returned type
        ///</summary>
        [TestMethod()]
        public void GetFileFromPathReturnedTypeTest()
        {
            Stream target;

            //Act
            target = BlobStorageHandler.GetFileFromPath(path);

            //Assert
            Assert.AreEqual(stream.GetType(), target.GetType());
        }
    }

    /// <summary>
    ///This is a test class for BlobStorageHandlerTest and is intended
    ///to contain the Save Method Unit Tests
    ///</summary>
    [TestClass()]
    public class BlobStorageHandler_Save_Tests
    {
        string path;
        BlobStorageHandler BlobStorageHandler;
        Stream stream;

        /// <summary>
        /// Run this before run my tests in this class
        /// </summary>
        [TestInitialize]
        public void Start()
        {
            //Arrange
            path = "test-container/TestFile.txt";
            BlobStorageHandler = new BlobStorageHandler();
            stream = new MemoryStream(ASCIIEncoding.Default.GetBytes("Aggressive Project: StreamTest"));
        }

        /// <summary>
        /// Run this after run my tests in this class
        /// </summary>
        [TestCleanup]
        public void Finish()
        {
            BlobStorageHandler.DeleteFile(path);
        }

        /// <summary>
        ///A test for SaveFile method
        ///</summary>
        [TestMethod()]
        public void SaveFileTest()
        {
            //Act
            BlobStorageHandler.SaveFile(stream, path);
        }
    }

    /// <summary>
    ///This is a test class for BlobStorageHandlerTest and is intended
    ///to contain the Delete Method Unit Tests
    ///</summary>
    [TestClass()]
    public class BlobStorageHandler_Delete_Tests
    {
        string path;
        BlobStorageHandler BlobStorageHandler;
        Stream stream;

        /// <summary>
        /// Run this before run my tests in this class
        /// </summary>
        [TestInitialize]
        public void Start()
        {
            //Arrange
            path = "test-container/TestFile.txt";
            BlobStorageHandler = new BlobStorageHandler();
            stream = new MemoryStream(ASCIIEncoding.Default.GetBytes("StreamTest"));

            //Act
            BlobStorageHandler.SaveFile(stream, path);
        }

        /// <summary>
        ///A test for DeleteFile method
        ///</summary>
        [TestMethod()]
        public void DeleteFileTest()
        {
            //Arrange
            BlobStorageHandler BlobStorageHandler = new BlobStorageHandler();

            //Act
            BlobStorageHandler.DeleteFile(path);
        }
    }

    /// <summary>
    ///This is a test class for BlobStorageHandlerTest and is intended
    ///to contain the List Method Unit Tests
    ///</summary>
    [TestClass()]
    public class BlobStorageHandler_List_Tests
    {
        string containerName;
        BlobStorageHandler BlobStorageHandler;
        Stream firstFile;
        Stream secondFile;
        Stream thirdFile;

        /// <summary>
        /// Run this before run my tests in this class
        /// </summary>
        [TestInitialize]
        public void Start()
        {
            //Arrange
            containerName = "test-container";
            BlobStorageHandler = new BlobStorageHandler();
            firstFile = new MemoryStream(ASCIIEncoding.Default.GetBytes("First File StreamTest"));
            secondFile = new MemoryStream(ASCIIEncoding.Default.GetBytes("Second File StreamTest"));
            thirdFile = new MemoryStream(ASCIIEncoding.Default.GetBytes("Third File StreamTest"));

            //Act
            BlobStorageHandler.SaveFile(firstFile, string.Format("{0}/{1}", containerName, "firstFile"));
            BlobStorageHandler.SaveFile(secondFile, string.Format("{0}/{1}", containerName, "secondFile"));
            BlobStorageHandler.SaveFile(thirdFile, string.Format("{0}/{1}", containerName, "thirdFile"));
        }

        /// <summary>
        /// Run this after run my tests in this class
        /// </summary>
        [TestCleanup]
        public void Finish()
        {
            BlobStorageHandler.DeleteFile(string.Format("{0}/{1}", containerName, "firstFile"));
            BlobStorageHandler.DeleteFile(string.Format("{0}/{1}", containerName, "secondFile"));
            BlobStorageHandler.DeleteFile(string.Format("{0}/{1}", containerName, "thirdFile"));
        }

        /// <summary>
        ///A test for ListContainerContents method
        ///</summary>
        [TestMethod()]
        public void ListContainerContentsTest()
        {
            //Arrange
            BlobStorageHandler BlobStorageHandler = new BlobStorageHandler();

            //Act
            var containerContents = BlobStorageHandler.ListContainerContents(containerName);

            //Assert
            Assert.AreEqual(3, containerContents.Count);
        }
    }
}
