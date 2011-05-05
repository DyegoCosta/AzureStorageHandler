using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureStorageHandler.Handlers
{
    /// <summary>
    /// This class performs operations in the Blob of the Azure Storage
    /// "StorageAzureAccount" - Windows Azure user account
    /// "StorageAzureKey" - Storage acess key
    /// </summary>
    public class BlobStorageHandler
    {
        // Recomended using
        // CloudStorageAccount.FromConfigurationSetting("StorageAzureAccount")
        // CloudStorageAccount.FromConfigurationSetting("StorageAzureKey")

        private static string Account { get { return ""; } }
        private static string Key { get { return ""; } }

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
                if(StorageCredentialsAccountAndKey != null)
                    return new CloudStorageAccount(StorageCredentialsAccountAndKey, false);
                else
                    return CloudStorageAccount.DevelopmentStorageAccount;
            }
        }        
        private static CloudBlobClient CloudBlobClient
        {
            get
            {
                if (StorageCredentialsAccountAndKey != null)
                    return new CloudBlobClient(CloudStorageAccount.BlobEndpoint, StorageCredentialsAccountAndKey);
                else
                    return new CloudBlobClient(CloudStorageAccount.BlobEndpoint, CloudStorageAccount.Credentials);
            }
        }

        /// <summary>
        /// Returns the file content from a path
        /// </summary>
        /// <param name="path">The path must be passed as "ContainerName/FileName"</param>
        /// <returns>File stream content</returns>
        public Stream GetFileFromPath(string path)
        {
            string[] cont = path.Split('/');
            string containerName = cont[0];
            string fileName = cont[1];

            CloudBlobContainer objContainer;
            objContainer = CloudBlobClient.GetContainerReference(containerName);

            CloudBlockBlob blob = objContainer.GetBlockBlobReference(fileName);
            return new MemoryStream(blob.DownloadByteArray());
        }

        /// <summary>
        /// Save the content of the file into the blob
        /// </summary>
        /// <param name="bytes">File stream content</param>
        /// <param name="path">The path must be passed as "ContainerName/FileName"</param>
        public void SaveFile(Stream stream, string path)
        {
            string[] cont = path.Split('/');
            string containerName = cont[0];
            string fileName = cont[1];

            CloudBlobContainer objContainer;
            objContainer = CloudBlobClient.GetContainerReference(containerName);

            if (objContainer.CreateIfNotExist())
            {
                objContainer.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
                objContainer.FetchAttributes(new BlobRequestOptions() { Timeout = new TimeSpan(1, 0, 0) });
            }

            CloudBlockBlob objBlob = objContainer.GetBlockBlobReference(fileName);

            objBlob.UploadFromStream(stream);
        }

        /// <summary>
        /// Delete a blob
        /// </summary>
        /// <param name="path">The path must be passed as "ContainerName/FileName"</param>
        public void DeleteFile(string path)
        {
            string[] cont = path.Split('/');
            string containerName = cont[0];
            string fileName = cont[1];

            CloudBlobContainer objContainer;
            objContainer = CloudBlobClient.GetContainerReference(containerName);

            var blobReference = objContainer.GetBlobReference(fileName);
            blobReference.DeleteIfExists();
        }

        /// <summary>
        /// List contents of container
        /// </summary>
        /// <param name="containerName">Must pass the exactly container name</param>
        public IList<IListBlobItem> ListContainerContents(string containerName)
        {
            CloudBlobContainer container =
            CloudBlobClient.GetContainerReference(containerName);
            return container.ListBlobs().ToList();
        }
    }
}
