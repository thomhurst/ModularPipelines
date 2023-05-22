using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace Pipeline.NET;

public class PipelineHostBuilder : IPipelineHostBuilder
{
    private readonly IHostBuilder _internalHost;

    internal PipelineHostBuilder()
    {
        _internalHost = Host.CreateDefaultBuilder();
        _internalHost.ConfigureServices(services =>
        {
            services
                .AddInitializers()
                .AddSingleton<IModuleContext, ModuleContext>()
                .AddSingleton<IDependencyCollisionDetector, DependencyCollisionDetector>()
                .AddSingleton<IEnvironmentContext, EnvironmentContext>();
        });
    }

    public static IPipelineHostBuilder Create() => new PipelineHostBuilder();
    
    public IPipelineHostBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> configureDelegate)
    {
        _internalHost.ConfigureAppConfiguration(configureDelegate);
        return this;
    }

    public IPipelineHostBuilder ConfigureServices(Action<IServiceCollection> configureDelegate)
    {
        _internalHost.ConfigureServices(configureDelegate);
        return this;
    }
    
    public PipelineHostBuilder AddModule<TModule>() where TModule : class, IModule
    {
        _internalHost.ConfigureServices(services => services.AddSingleton<IModule, TModule>());
        return this;
    }
    
    public PipelineHostBuilder AddModulesFromAssemblyContainingType<T>()
    {
        return AddModulesFromAssembly(typeof(T).Assembly);
    }

    public PipelineHostBuilder AddModulesFromAssembly(Assembly assembly)
    {
        _internalHost.ConfigureServices(services =>
        {
            AddModulesFromAssembly(services, assembly);
        });

        return this;
    }

    public async Task<IModule[]> ExecutePipelineAsync()
    {
        var host = _internalHost.Build();

        await host.Services.InitializeAsync();

        var options = host.Services.GetRequiredService<IOptions<PipelineOptions>>();
        
        var modules = host.Services.GetServices<IModule>().ToArray();

        var moduleProcessingTasks = modules.Select(x => x.StartProcessingModule()).ToList();
        
        if (options.Value.StopOnFirstException)
        {
            while (moduleProcessingTasks.Any())
            {
                var finished = await Task.WhenAny(moduleProcessingTasks);
                moduleProcessingTasks.Remove(finished);
            }
        }
        else
        {
            await Task.WhenAll(moduleProcessingTasks);
        }

        return modules;
    }

    private static void AddModulesFromAssembly(IServiceCollection services, Assembly assembly)
    {
        var modules = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IModule)))
            .Where(type => type.IsClass)
            .Where(type => !type.IsAbstract);

        foreach (var module in modules)
        {
            services.AddSingleton(typeof(IModule), module);
        }
    }
}