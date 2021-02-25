using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AbpTemplate.App.Synchronization.Cache;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace AbpTemplate.App.Synchronization
{
    public class SynchronizationService : ITransientDependency
    {
        private readonly LockerCache _lockerCache;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SynchronizationService(LockerCache cache, IUnitOfWorkManager unitOfWorkManager)
        {
            _lockerCache = cache;
            _unitOfWorkManager = unitOfWorkManager;
        }

        #region WithResult

        /// <summary>
        /// Locks by name <see cref="lockName"/>
        /// </summary>
        public async Task<TResult> LockAsync<TResult>(string lockName, Func<Task<TResult>> function)
        {
            var locker = _lockerCache.GetLocker(-1, lockName);
            locker.WaitOne(SynchronizationTimeout);

            try
            {
                var res = await CompleteUnitOfWorkAsync(function);
                return res;
            }
            finally
            {
                locker.Set();
            }
        }

        /// <summary>
        /// Locks by id <see cref="id"/> and method's name <see cref="function"/>
        /// </summary>
        public async Task<TResult> LockAsync<TResult>(int id, Expression<Func<Task<TResult>>> function)
        {
            var body = (MethodCallExpression)function.Body;
            var methodName = body.Method.Name;

            var locker = _lockerCache.GetLocker(id, methodName);
            locker.WaitOne(SynchronizationTimeout);

            try
            {
                var res = await CompleteUnitOfWorkAsync(function.Compile());
                return res;
            }
            finally
            {
                locker.Set();
            }
        }

        /// <summary>
        /// Locks by id <see cref="id"/> and name <see cref="lockName"/>
        /// </summary>
        public async Task<TResult> LockAsync<TResult>(int id, string lockName, Func<Task<TResult>> function)
        {
            var locker = _lockerCache.GetLocker(id, lockName);
            locker.WaitOne(SynchronizationTimeout);

            try
            {
                var res = await CompleteUnitOfWorkAsync(function);
                return res;
            }
            finally
            {
                locker.Set();
            }
        }

        #endregion

        #region WithoutResult

        /// <summary>
        /// Locks by name <see cref="lockName"/>
        /// </summary>
        public async Task LockAsync(string lockName, Func<Task> function)
        {
            var locker = _lockerCache.GetLocker(-1, lockName);
            locker.WaitOne(SynchronizationTimeout);

            try
            {
                await CompleteUnitOfWorkAsync(function);
            }
            finally
            {
                locker.Set();
            }
        }

        /// <summary>
        /// Locks by id <see cref="id"/> and method's name <see cref="function"/>
        /// </summary>
        public async Task LockAsync(int id, Expression<Func<Task>> function)
        {
            var body = (MethodCallExpression)function.Body;
            var methodName = body.Method.Name;

            var locker = _lockerCache.GetLocker(id, methodName);
            locker.WaitOne(SynchronizationTimeout);

            try
            {
                await CompleteUnitOfWorkAsync(function.Compile());
            }
            finally
            {
                locker.Set();
            }
        }

        /// <summary>
        /// Locks by id <see cref="id"/> and name <see cref="lockName"/>
        /// </summary>
        public async Task LockAsync(int id, string lockName, Func<Task> function)
        {
            var locker = _lockerCache.GetLocker(id, lockName);
            locker.WaitOne(SynchronizationTimeout);

            try
            {
                await CompleteUnitOfWorkAsync(function);
            }
            finally
            {
                locker.Set();
            }
        }

        #endregion

        #region UnitOfWork

        private async Task CompleteUnitOfWorkAsync(Func<Task> task)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                await task();
                await uow.CompleteAsync();
            }
        }

        private async Task<TResult> CompleteUnitOfWorkAsync<TResult>(Func<Task<TResult>> task)
        {
            TResult res = default;

            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                res = await task();
                await uow.CompleteAsync();
            }

            return res;
        }

        #endregion

        /// <summary>
        /// Sync timeout, 10 minutes
        /// </summary>
        public static readonly TimeSpan SynchronizationTimeout = TimeSpan.FromMinutes(10);
    }
}
