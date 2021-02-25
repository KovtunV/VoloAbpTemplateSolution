using System.Collections.Concurrent;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace AbpTemplate.App.Synchronization.Cache
{
    public class LockerCache : ISingletonDependency
    {
        private readonly ConcurrentDictionary<LockGroup, AutoResetEvent> _lockerCache;

        public LockerCache()
        {
            _lockerCache = new ConcurrentDictionary<LockGroup, AutoResetEvent>();
        }

        public AutoResetEvent GetLocker(int forecastVersionId, string lockName)
        {
            var lockGroup = new LockGroup(forecastVersionId, lockName);

            if (!_lockerCache.TryGetValue(lockGroup, out var locker))
            {
                locker = new AutoResetEvent(true);

                // If it isn't possible to add, then try again,
                // several threads can simultaneously try to add a different locker to the dictionary
                if (!_lockerCache.TryAdd(lockGroup, locker))
                {
                    return GetLocker(forecastVersionId, lockName);
                }
            }

            return locker;
        }
    }
}
