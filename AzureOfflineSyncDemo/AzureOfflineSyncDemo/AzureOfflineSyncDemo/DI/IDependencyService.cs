using System;

namespace AzureOfflineSyncDemo.DI
{
    /// <summary>
    /// Register Network Changed Event
    /// </summary>
    public interface IDependencyService
    {
        event EventHandler OnStatusChanged;
    }
}
