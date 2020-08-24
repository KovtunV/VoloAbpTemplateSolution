using System;
using System.Collections.Concurrent;
using AbpTemplate.EF.EntityGroups.Groups.Base;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace AbpTemplate.EF.EntityGroups
{
    public class EntitiGroupService : ITransientDependency
    {
        private readonly ConcurrentDictionary<Type, BaseEntityGroup> _entityGroups;
        private readonly IServiceProvider _serviceProvider;

        public EntitiGroupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _entityGroups = new ConcurrentDictionary<Type, BaseEntityGroup>();
        }

        public T GetEntityGroup<T>()
            where T : BaseEntityGroup
        {
            var groupType = typeof(T);

            if (!_entityGroups.TryGetValue(groupType, out var group))
            {
                group = _serviceProvider.GetRequiredService<T>();
                if (!_entityGroups.TryAdd(groupType, group))
                {
                    return GetEntityGroup<T>();
                };
            }

            return (T)group;
        }
    }
}
