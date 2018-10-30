using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineSyncDemo.DI
{
   public interface IDependencyService
    {
        event EventHandler OnStatusChanged;
    }
}
