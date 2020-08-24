using System;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Localization.ExceptionHandling;

namespace AbpTemplate.WebApi.Start
{
    public class FIX_ExceptionToErrorInfoConverter : DefaultExceptionToErrorInfoConverter
    {
        public FIX_ExceptionToErrorInfoConverter(
            IOptions<AbpExceptionLocalizationOptions> localizationOptions, 
            IStringLocalizerFactory stringLocalizerFactory,
            IStringLocalizer<AbpUiResource> abpUiStringLocalizer, 
            IServiceProvider serviceProvider) 
            : base(localizationOptions, stringLocalizerFactory, abpUiStringLocalizer, serviceProvider)
        {
            // Waiting for the fix: https://github.com/abpframework/abp/issues/5066
            SendAllExceptionsToClients = true;
        }
    }
}
