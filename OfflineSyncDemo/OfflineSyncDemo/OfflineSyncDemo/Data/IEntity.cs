using System;

namespace OfflineSyncDemo.Data
{
    public interface IEntity
    {
        int DbId { get; set; }
        DateTime UpdatedOn { get; set; }
    }

    public interface IServerEntity 
    {
        int ServerId { get; set; }
        /// <summary>
        /// SyncStatus
        /// </summary>
        int SyncStatus { get; set; }
    }
}
