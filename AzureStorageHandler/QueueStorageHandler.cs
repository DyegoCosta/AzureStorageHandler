using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;

namespace AzureStorageHandler
{
    public class QueueStorageHandler
    {
        // Recomended using
        // CloudStorageAccount.FromConfigurationSetting("StorageAzureAccount")
        // CloudStorageAccount.FromConfigurationSetting("StorageAzureKey")

        private static string Account { get { return ""; } }
        private static string Key { get { return ""; } }
        private CloudQueue CloudQueue = null;

        private static StorageCredentialsAccountAndKey StorageCredentialsAccountAndKey
        {
            get
            {
                if (!string.IsNullOrEmpty(Key) && !string.IsNullOrEmpty(Account))
                    return new StorageCredentialsAccountAndKey(Account, Key);
                else
                    return null;
            }
        }
        private static CloudStorageAccount CloudStorageAccount
        {
            get
            {
                if (StorageCredentialsAccountAndKey != null)
                    return new CloudStorageAccount(StorageCredentialsAccountAndKey, false);
                else
                    return CloudStorageAccount.DevelopmentStorageAccount;
            }
        }
        private CloudQueueClient CloudQueueClient { get { return CloudStorageAccount.CreateCloudQueueClient(); } }

        /// <summary>
        /// Instance the QueueStorageHandler class and create a queue container if not exist
        /// </summary>
        /// <param name="queueContainerName">Container name for the queues</param>
        public QueueStorageHandler(string queueContainerName)
        {
            CloudQueue = CloudQueueClient.GetQueueReference(queueContainerName);
            CloudQueue.CreateIfNotExist();
        }

        /// <summary>
        /// Retrieves the messages in the queue
        /// </summary>
        /// <param name="messageCount">Number of messages that need to be retrieved</param>
        /// <returns></returns>
        public IEnumerable<CloudQueueMessage> RetrieveMessages(int messageCount)
        {
             return CloudQueue.GetMessages(messageCount);
        }

        /// <summary>
        /// Gets a single message from the queue
        /// </summary>
        /// <returns>CloudQueueMessage</returns>
        public CloudQueueMessage RetrieveMessage()
        {
            return CloudQueue.GetMessage();
        }

        /// <summary>
        /// Insert a new text message to the queue
        /// </summary>
        /// <param name="entity">The text message which needs to be inserted</param>
        public void InsertMessage(string textMessage)
        {
            CloudQueueMessage message = new CloudQueueMessage(textMessage);
            CloudQueue.AddMessage(message);
        }

        /// <summary>
        /// Insert a new byte array message to the queue
        /// </summary>
        /// <param name="byteArrayMessage">The byte array message wich needs to be inserted</param>
        public void InsertMessage(byte[] byteArrayMessage)
        {
            CloudQueueMessage message = new CloudQueueMessage(byteArrayMessage);
            CloudQueue.AddMessage(message);
        }

        /// <summary>
        /// Insert a new cloud queue message to the queue
        /// </summary>
        /// <param name="cloudQueueMessage">CloudQueueMessage</param>
        public void InsertMessage(CloudQueueMessage cloudQueueMessage)
        {
            CloudQueue.AddMessage(cloudQueueMessage);
        }

        /// <summary>
        /// Delete a queue message
        /// </summary>
        /// <param name="message"></param>
        public void DeleteMessage(CloudQueueMessage message)
        {
            CloudQueue.DeleteMessage(message);
        }

        /// <summary>
        /// Delete a queue message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="popReceipt"></param>
        public void DeleteMessage(string messageId, string popReceipt)
        {
            CloudQueue.DeleteMessage(messageId, popReceipt);
        }

        /// <summary>
        /// Clear all messages of the queue
        /// </summary>
        public void CleanQueue()
        {
            CloudQueue.Clear();
        }

        /// <summary>
        /// Delete the queue
        /// </summary>
        public void DeleteQueue()
        {
            CloudQueue.Delete();
        }
    }
}
