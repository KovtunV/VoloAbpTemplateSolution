using Volo.Abp.ObjectMapping;

namespace AbpTemplate.App.Utils
{
    public static class AppExtensions
    {
        public static TDest Map<TDest>(this IObjectMapper mapper, object source)
        {
            if (source is null)
            {
                return default;
            }

            return (TDest)mapper.Map(source.GetType(), typeof(TDest), source);
        }
    }
}
