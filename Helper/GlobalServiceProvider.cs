using System;
using Microsoft.Extensions.DependencyInjection;

namespace Helper
{
    public class GlobalServiceProvider
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static T GetService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

    }
}
