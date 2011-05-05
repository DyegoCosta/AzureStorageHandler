using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureStorageHandler.Handlers
{
    /// <summary>
    /// This class performs operations in the Table of the Azure Storage
    /// "StorageAzureAccount" - Windows Azure user account
    /// "StorageAzureKey" - Storage acess key
    /// </summary>
    public class TableStorageHandler<TElement> where TElement : class
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
                if (StorageCredentialsAccountAndKey != null)
                    return new CloudStorageAccount(StorageCredentialsAccountAndKey, false);
                else
                    return CloudStorageAccount.DevelopmentStorageAccount;
            }
        }
        private TableServiceContext TableServiceContext = new TableServiceContext(CloudStorageAccount.TableEndpoint.ToString(), CloudStorageAccount.Credentials);

        /// <summary>
        /// Insert a new item in a table
        /// </summary>
        /// <param name="tableName">Precisely table's name</param>
        /// <param name="entity">The item which need to be inserted</param>
        public void Insert(string tableName, TElement entity)
        {
            CloudStorageAccount.CreateCloudTableClient().CreateTableIfNotExist(tableName);
            TableServiceContext.AddObject(tableName, entity);
            TableServiceContext.SaveChanges();
        }

        /// <summary>
        /// Delete a new item in a table
        /// </summary>
        /// <param name="tableName">Precisely table's name</param>
        /// <param name="entity">The item which need to be deleted</param>
        public void Delete(string tableName, TElement entity)
        {
            TableServiceContext.DeleteObject(entity);
            TableServiceContext.SaveChanges();
        }

        /// <summary>
        /// Update a new item in a table
        /// </summary>
        /// <param name="tableName">Precisely table's name</param>
        /// <param name="entity">The item which need to be updated</param>
        public void Update(string tableName, TElement entity)
        {
            TableServiceContext.Detach(entity);
            TableServiceContext.AttachTo(tableName, entity, "*");
            TableServiceContext.UpdateObject(entity);
            TableServiceContext.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);
        }

        /// <summary>
        /// Get all elements of a specific table
        /// </summary>
        /// <param name="tableName">Precisely table's name</param>
        /// <returns>List of the table's elements</returns>
        public IEnumerable<TElement> GetAll(string tableName)
        {
            var query = TableServiceContext.CreateQuery<TElement>(tableName);
            return query.ToList();
        }

        /// <summary>
        /// Get the elements of a specific table in the conditions passed
        /// </summary>
        /// <param name="tableName">Precisely table's name</param>
        /// <returns>List of the table's elements</returns>
        public IEnumerable<TElement> Where(string tableName, Expression<Func<TElement, bool>> expression)
        {
            var query = TableServiceContext.CreateQuery<TElement>(tableName);
            return query.Where(expression);
        }

        /// <summary>
        /// Delete a table
        /// </summary>
        /// <param name="tableName">Precisely table's name</param>
        public void DeleteTable(string tableName)
        {
            try
            {
                CloudStorageAccount.CreateCloudTableClient().DeleteTable(tableName);
            }
            catch
            {
                throw;
            }
        }
    }
}
