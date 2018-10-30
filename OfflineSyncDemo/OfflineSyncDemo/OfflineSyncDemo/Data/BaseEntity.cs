using SQLite;
using System;
using System.ComponentModel;

namespace OfflineSyncDemo.Data
{
    public abstract class BaseEntity : IEntity, IServerEntity
    {
        #region IEntity

        [PrimaryKey, AutoIncrement]
        public int DbId
        {
            get; set;
        }

        public DateTime UpdatedOn
        {
            get; set;
        }

        public BaseEntity()
        {
            //CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        public int ServerId { get; set; }
        /// <summary>
        /// SyncStatus
        /// </summary>
        public int SyncStatus { get; set; }

        #endregion
    }

}
