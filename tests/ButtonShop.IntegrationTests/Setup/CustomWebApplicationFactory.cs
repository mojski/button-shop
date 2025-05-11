
using ButtonShop.Infrastructure.Monitoring.Elastic;
using ButtonShop.Infrastructure.Monitoring.Elastic.Interfaces;
using ButtonShop.IntegrationTests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ButtonShop.IntegrationTests.Setup;

public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    public IConfiguration? Configuration;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureServices);
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "ButtonShop_")
                .Build();

        // removing services
        //var removeService = services.Single(s => s.ImplementationType == typeof(Service));
        //services.Remove(removeService);

        services.AddSingleton<IElasticSearchService, FakeElasticSearchService>();
    }
}
