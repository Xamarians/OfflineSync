using OfflineSyncDemo.Data;
using OfflineSyncDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfflineSyncDemo.Services
{
    public class SyncService
    {
        private bool _busy = false;
        private static readonly SyncService _instance = new SyncService();
        public static SyncService Instance => _instance;
        private SyncService()
        {
        }

        /// <summary>
        /// Send Create, update and delete request to server
        /// </summary>
        private Task<IRestResult<int>> SendRequestAsync<TEntity>(TEntity user) where TEntity : IServerEntity
        {
            if (user.SyncStatus == (int)SyncStatus.New)
            {
                return RestAPI.PostAsync<int>($"{Constants.ApiBaseUrl}/users", user);
            }
            else if (user.SyncStatus == (int)SyncStatus.Deleted)
            {
                return RestAPI.DeleteAsync<int>($"{Constants.ApiBaseUrl}/users/{user.ServerId}");
            }
            // Modfied
            return RestAPI.PutAsync<int>($"{Constants.ApiBaseUrl}/users/{user.ServerId}", user);
        }

        /// <summary>
        /// Sync single user to server 
        /// </summary>
        public async Task<bool> SyncTable<TEntity>(TEntity item) where TEntity : IEntity, IServerEntity
        {
            if (item.SyncStatus == (int)SyncStatus.Ok)
                return true;
            try
            {
                IRestResult<int> response = await SendRequestAsync(item);
                if (!response.IsSuccess)
                    return false;
                if (item.SyncStatus == (int)SyncStatus.Deleted)
                {
                    Repository.Delete<User>(x => x.DbId == item.DbId);
                    return true;
                }
                if (response.Data > 0)
                {
                    item.ServerId = response.Data;
                    item.SyncStatus = (int)SyncStatus.Ok;
                    Repository.SaveOrUpdate(item);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                // LOG EXCEPTION
                return false;
            }
        }

        /// <summary>
        /// Sync all user to server
        /// </summary>
        public async void SyncAll()
        {
            if (_busy)
                return;
            _busy = true;
            List<Task> taskList = new List<Task>();
            foreach (var item in Repository.AsQueryable<User>()
                .Where(x => x.SyncStatus != (int)SyncStatus.Ok)
                .OrderByDescending(x => x.UpdatedOn))
            {
                taskList.Add(SyncTable(item));
            }
            await Task.WhenAll(taskList);
            _busy = false;
        }

    }
}
