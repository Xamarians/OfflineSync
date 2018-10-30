using OfflineSyncDemo.Models;
using OfflineSyncDemo.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfflineSyncDemo.Data
{
    public static class Repository
    {
        const string DatabaseName = "OfflineSyncDemo.db5";
        static readonly string DBPath;

        private static SQLiteConnection _connection;
        private static SQLiteConnection DBConnection
        {
            get
            {
                try
                {
                    if (_connection == null)
                        _connection = new SQLiteConnection(DBPath);
                    return _connection;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        static Repository()
        {
            DBPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName);
            InitializeDatabase();
        }

        #region Private Methods

        public static void InitializeDatabase()
        {
            DBConnection.CreateTable<User>();
        }

        static void ClearAllTablesData()
        {
            lock (DBConnection)
            {
                DBConnection.DeleteAll<User>();
            }
        }

        #endregion

        #region Generic Methods

        /// <summary>
        /// Execute query in database and return result
        /// </summary>
        /// <typeparam name="T">type of record</typeparam>
        /// <returns></returns>
        public static IQueryable<T> AsQueryable<T>() where T : IEntity, new()
        {
            return DBConnection.Table<T>().AsQueryable();
        }


        /// <summary>
        /// Save or update item by Id
        /// </summary>
        /// <typeparam name="T">item type</typeparam>
        /// <param name="item">Item to sav eor update</param>
        public static void SaveOrUpdate<T>(T item) where T : IEntity
        {
            if (item == null)
                return;
            lock (DBConnection)
            {
                SaveOrUpdate(new List<T> { item });
            }
        }

        /// <summary>
        /// Save or update items by Id
        /// </summary>
        /// <typeparam name="T">item type</typeparam>
        /// <param name="items">Items to save or update</param>
        public static void SaveOrUpdate<T>(List<T> items) where T : IEntity
        {
            if (items == null)
                return;
            lock (DBConnection)
            {
                items.ForEach(x => x.UpdatedOn = DateTime.UtcNow);
                DBConnection.InsertAll(items.Where(x => x.DbId == 0));
                DBConnection.UpdateAll(items.Where(x => x.DbId > 0));
            }
        }

        /// <summary>
        /// Execute query in database and return result
        /// </summary>
        /// <typeparam name="T">type of record</typeparam>
        /// <returns></returns>
        public static List<T> Find<T>(int page, int limit) where T : IEntity, new()
        {
            return Find<T>(null, page, limit);
        }

        /// <summary>
        /// Execute query in database and return result
        /// </summary>
        /// <typeparam name="T">type of record</typeparam>
        /// <param name="query">query to filter the records</param>
        /// <returns></returns>
        public static List<T> Find<T>(Func<T, bool> query, int page, int limit) where T : IEntity, new()
        {
            page = (page < 2 ? 0 : page - 1) * limit;
            lock (DBConnection)
            {
                if (query == null)
                    return AsQueryable<T>().OrderByDescending(x => x.UpdatedOn)
                        .Skip(page).Take(limit).ToList();
                return AsQueryable<T>().Where(query).OrderByDescending(x => x.UpdatedOn)
                    .Skip(page).Take(limit).ToList();
            }
        }

        public static T FindOne<T>(Func<T, bool> query) where T : new()
        {
            lock (DBConnection)
            {
                return DBConnection.Table<T>().FirstOrDefault(query);
            }
        }


        /// <summary>
        /// check for given primary key return result
        /// </summary>
        /// <typeparam name="T">type of record</typeparam>
        /// <param name="id">primary key</param>
        /// <returns></returns>
        public static T FindOne<T>(int id) where T : new()
        {
            lock (DBConnection)
            {
                return DBConnection.Find<T>(id);
            }
        }

        /// <summary>
        /// Execute query in database and return result
        /// </summary>
        /// <typeparam name="T">type of record</typeparam>
        /// <param name="query">query to filter the records</param>
        /// <returns></returns>
        public static int Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> query) where T : new()
        {
            lock (DBConnection)
            {
                return DBConnection.Table<T>().Delete(query);
            }
        }

        //public static int DeleteAll<T>() where T : new()
        //{
        //    lock (DBConnection)
        //    {
        //        return DBConnection.DeleteAll<T>();
        //    }
        //}

        //public static bool Exists<T>(Func<T, bool> query) where T : new()
        //{
        //    lock (DBConnection)
        //    {
        //        return DBConnection.Table<T>().Any(query);
        //    }
        //}

        public static void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public static void ClearDatabase()
        {
            ClearAllTablesData();
        }

        public static Task SyncInsert<T>(T item) where T : IEntity, IServerEntity
        {
            item.SyncStatus = (int)SyncStatus.New;
            lock (DBConnection)
            {
                DBConnection.Insert(item);
            }
            return SyncService.Instance.SyncTable(item);
        }

        public static Task SyncUpdate<T>(T item) where T : IEntity, IServerEntity
        {
            item.SyncStatus = (int)SyncStatus.Modified;
            lock (DBConnection)
            {
                DBConnection.Update(item);
            }
            return SyncService.Instance.SyncTable(item);
        }

        public static Task SyncDelete<T>(T item) where T : IEntity, IServerEntity
        {
            item.SyncStatus = (int)SyncStatus.Deleted;
            lock (DBConnection)
            {
                DBConnection.Update(item);
            }
            return SyncService.Instance.SyncTable(item);
        }

        #endregion

        public static void BeginTransaction()
        {
            DBConnection.BeginTransaction();
        }

        public static void Commit()
        {
            DBConnection.Commit();
        }

        public static void Rollback()
        {
            DBConnection.Rollback();
        }

        public static int Merge<T>(T item) where T : IEntity, new()
        {
            var dbItem = DBConnection.Table<T>().FirstOrDefault();
            if (dbItem != null)
                item.DbId = dbItem.DbId;
            if (!IsEqual(dbItem, item))
            {
                SaveOrUpdate(item);
                return 1;
            }
            return 0;
        }

        static bool IsEqual<T>(T item1, T item2) where T : IEntity
        {
            if (item1 == null || item2 == null)
                return object.Equals(item1, item2);

            foreach (var prop in typeof(T).GetProperties())
            {
                if ("DbId".Equals(prop.Name) || prop.Name == "CreatedOn" || prop.Name == "UpdatedOn")
                    continue;
                if (!object.Equals(prop.GetValue(item1), prop.GetValue(item2)))
                    return false;
            }
            return true;
        }

        private static int GetOffset(int index, int limit)
        {
            return index > 1 ? (index - 1) * limit : 0;
        }

    }
}
