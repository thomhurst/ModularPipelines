using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Host;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers.Extensions;

namespace ModularPipelines.TestHelpers;

/// <summary>
/// Base class for tests that need to run pipeline modules.
/// Provides helper methods to execute modules and automatically disposes hosts after each test.
/// </summary>
public abstract class TestBase
{
    private readonly List<IPipelineHost> _hosts = [];

    private class DummyModule : Module<IDictionary<string, object>?>
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    /// <summary>
    /// Runs a single module with default test settings.
    /// </summary>
    /// <typeparam name="T">The module type to run.</typeparam>
    /// <returns>The executed module instance.</returns>
    public Task<T> RunModule<T>() where T : class, IModule => RunModule<T>(new TestHostSettings());

    /// <summary>
    /// Runs a single module with the specified test settings.
    /// </summary>
    /// <typeparam name="T">The module type to run.</typeparam>
    /// <param name="testHostSettings">Settings for configuring the test host.</param>
    /// <returns>The executed module instance.</returns>
    public Task<T> RunModule<T>(TestHostSettings testHostSettings)
        where T : class, IModule
    {
        return ExecuteModulesAsync<T>(
            testHostSettings,
            builder => builder.AddModule<T>(),
            modules => modules.OfType<T>().Single());
    }

    /// <summary>
    /// Runs two modules with default test settings.
    /// </summary>
    /// <typeparam name="T">The first module type to run.</typeparam>
    /// <typeparam name="T2">The second module type to run.</typeparam>
    /// <returns>A tuple containing both executed module instances.</returns>
    public Task<(T, T2)> RunModules<T, T2>()
        where T : class, IModule
        where T2 : class, IModule
        => RunModules<T, T2>(new TestHostSettings());

    /// <summary>
    /// Runs two modules with the specified test settings.
    /// </summary>
    /// <typeparam name="T">The first module type to run.</typeparam>
    /// <typeparam name="T2">The second module type to run.</typeparam>
    /// <param name="testHostSettings">Settings for configuring the test host.</param>
    /// <returns>A tuple containing both executed module instances.</returns>
    public Task<(T, T2)> RunModules<T, T2>(TestHostSettings testHostSettings)
        where T : class, IModule
        where T2 : class, IModule
    {
        return ExecuteModulesAsync<(T, T2)>(
            testHostSettings,
            builder => builder.AddModule<T>().AddModule<T2>(),
            modules => (
                modules.OfType<T>().Single(),
                modules.OfType<T2>().Single()));
    }

    /// <summary>
    /// Runs three modules with default test settings.
    /// </summary>
    /// <typeparam name="T">The first module type to run.</typeparam>
    /// <typeparam name="T2">The second module type to run.</typeparam>
    /// <typeparam name="T3">The third module type to run.</typeparam>
    /// <returns>A tuple containing all executed module instances.</returns>
    public Task<(T, T2, T3)> RunModules<T, T2, T3>()
        where T : class, IModule
        where T2 : class, IModule
        where T3 : class, IModule
        => RunModules<T, T2, T3>(new TestHostSettings());

    /// <summary>
    /// Runs three modules with the specified test settings.
    /// </summary>
    /// <typeparam name="T">The first module type to run.</typeparam>
    /// <typeparam name="T2">The second module type to run.</typeparam>
    /// <typeparam name="T3">The third module type to run.</typeparam>
    /// <param name="testHostSettings">Settings for configuring the test host.</param>
    /// <returns>A tuple containing all executed module instances.</returns>
    public Task<(T, T2, T3)> RunModules<T, T2, T3>(TestHostSettings testHostSettings)
        where T : class, IModule
        where T2 : class, IModule
        where T3 : class, IModule
    {
        return ExecuteModulesAsync<(T, T2, T3)>(
            testHostSettings,
            builder => builder.AddModule<T>().AddModule<T2>().AddModule<T3>(),
            modules => (
                modules.OfType<T>().Single(),
                modules.OfType<T2>().Single(),
                modules.OfType<T3>().Single()));
    }

    /// <summary>
    /// Runs four modules with default test settings.
    /// </summary>
    /// <typeparam name="T">The first module type to run.</typeparam>
    /// <typeparam name="T2">The second module type to run.</typeparam>
    /// <typeparam name="T3">The third module type to run.</typeparam>
    /// <typeparam name="T4">The fourth module type to run.</typeparam>
    /// <returns>A tuple containing all executed module instances.</returns>
    public Task<(T, T2, T3, T4)> RunModules<T, T2, T3, T4>()
        where T : class, IModule
        where T2 : class, IModule
        where T3 : class, IModule
        where T4 : class, IModule
        => RunModules<T, T2, T3, T4>(new TestHostSettings());

    /// <summary>
    /// Runs four modules with the specified test settings.
    /// </summary>
    /// <typeparam name="T">The first module type to run.</typeparam>
    /// <typeparam name="T2">The second module type to run.</typeparam>
    /// <typeparam name="T3">The third module type to run.</typeparam>
    /// <typeparam name="T4">The fourth module type to run.</typeparam>
    /// <param name="testHostSettings">Settings for configuring the test host.</param>
    /// <returns>A tuple containing all executed module instances.</returns>
    public Task<(T, T2, T3, T4)> RunModules<T, T2, T3, T4>(TestHostSettings testHostSettings)
        where T : class, IModule
        where T2 : class, IModule
        where T3 : class, IModule
        where T4 : class, IModule
    {
        return ExecuteModulesAsync<(T, T2, T3, T4)>(
            testHostSettings,
            builder => builder.AddModule<T>().AddModule<T2>().AddModule<T3>().AddModule<T4>(),
            modules => (
                modules.OfType<T>().Single(),
                modules.OfType<T2>().Single(),
                modules.OfType<T3>().Single(),
                modules.OfType<T4>().Single()));
    }

    /// <summary>
    /// Core helper method that executes modules and extracts results.
    /// Consolidates the common pattern of building a host, executing, and extracting modules.
    /// </summary>
    /// <typeparam name="TResult">The return type (single module or tuple of modules).</typeparam>
    /// <param name="testHostSettings">Settings for configuring the test host.</param>
    /// <param name="configureModules">Action to add modules to the builder.</param>
    /// <param name="extractResults">Function to extract the desired modules from the results.</param>
    /// <returns>The extracted module(s).</returns>
    private async Task<TResult> ExecuteModulesAsync<TResult>(
        TestHostSettings testHostSettings,
        Func<PipelineHostBuilder, PipelineHostBuilder> configureModules,
        Func<IEnumerable<IModule>, TResult> extractResults)
    {
        var builder = TestPipelineHostBuilder.Create(testHostSettings);
        var host = await configureModules(builder).BuildHostAsync();

        _hosts.Add(host);

        using var cts = CreateTimeoutCancellationTokenSource(testHostSettings.TestTimeout);
        var pipelineResult = await host.ExecutePipelineAsync(cts.Token);

        return extractResults(pipelineResult.Modules);
    }

    public async Task<T> GetService<T>()
        where T : notnull
    {
        var valueTuple = await GetService<T>(null);
        return valueTuple.T;
    }

    public async Task<(T T, IPipelineHost Host)> GetService<T>(Action<HostBuilderContext, IServiceCollection>? configureServices)
        where T : notnull
    {
        var host = await TestPipelineHostBuilder
            .Create()
            .AddModule<DummyModule>()
            .ConfigureServices((context, collection) => configureServices?.Invoke(context, collection))
            .BuildHostAsync();

        _hosts.Add(host);

        var serviceProvider = host.Services;

        return (serviceProvider.GetRequiredService<T>(), host);
    }

    /// <summary>
    /// Creates a CancellationTokenSource with the specified timeout.
    /// Returns a non-cancelling source if timeout is infinite.
    /// </summary>
    protected static CancellationTokenSource CreateTimeoutCancellationTokenSource(TimeSpan timeout)
    {
        return timeout == Timeout.InfiniteTimeSpan
            ? new CancellationTokenSource()
            : new CancellationTokenSource(timeout);
    }

    [After(Test)]
    public async Task DisposeCreatedHost()
    {
        await _hosts.ToAsyncProcessorBuilder()
            .ForEachAsync(Disposer.DisposeObjectAsync)
            .ProcessInParallel();

        _hosts.Clear();
    }
}
