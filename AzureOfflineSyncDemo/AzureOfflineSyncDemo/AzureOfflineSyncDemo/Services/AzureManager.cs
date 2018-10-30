using AzureOfflineSyncDemo.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AzureOfflineSyncDemo.Services
{
    public class AzureManager
    {
        static AzureManager defaultInstance = new AzureManager();
        MobileServiceClient client;
        IMobileServiceSyncTable<EmployeeItem> EmployeeItems;

        public AzureManager()
        {
            try
            {
                client = new MobileServiceClient(Constants.ApplicationURL);
                var store = new MobileServiceSQLiteStore("localstore.db");
                store.DefineTable<EmployeeItem>();
                this.client.SyncContext.InitializeAsync(store);
                this.EmployeeItems = client.GetSyncTable<EmployeeItem>();
            }
            catch(Exception ex)
            {

            }
        }
        public static AzureManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        /// <summary>
        /// Get mobile service client
        /// </summary>
        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return EmployeeItems is IMobileServiceSyncTable<EmployeeItem>; }
        }

        /// <summary>
        /// Get user list from local database
        /// 
        /// /// </summary>
        public async Task<ObservableCollection<EmployeeItem>> GetUserAsync(bool syncItems = false)
        {
            try
            {
                if (syncItems)
                {
                    await this.SyncAsync();
                }
                IEnumerable<EmployeeItem> items = await EmployeeItems.Where(x => true).ToEnumerableAsync();
                return new ObservableCollection<EmployeeItem>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
            }
            catch (Exception e)
            {
            }
            return null;
        }

        /// <summary>
        /// Delete user from local databse
        /// </summary>
        public async Task Delete(EmployeeItem employee)
        {
            await  EmployeeItems.DeleteAsync(employee);
        }

        /// <summary>
        /// Create and update user to local database as well as on the Azure server
        /// </summary>
        public async Task SaveTaskAsync(EmployeeItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Id) || item.Id == "0")
            {
                await EmployeeItems.InsertAsync(item);
            }
            else
            {
                await EmployeeItems.UpdateAsync(item);
            }
        }

        /// <summary>
        /// Sync all user to Azure server
        /// </summary>
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.EmployeeItems.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allUserItems",
                    this.EmployeeItems.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }
                }
            }
        }
    }
}