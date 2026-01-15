using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers.Extensions;

namespace ModularPipelines.TestHelpers;

/// <summary>
/// Base class for tests that need to run pipeline modules.
/// Provides helper methods to execute modules and automatically disposes pipelines after each test.
/// </summary>
public abstract class TestBase
{
    private readonly List<IPipeline> _pipelines = [];

    private class DummyModule : SimpleTestModule<IDictionary<string, object>?>
    {
        protected override IDictionary<string, object>? Result => null;
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
            builder => builder.Services.AddModule<T>(),
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
            builder => builder.Services.AddModule<T>().AddModule<T2>(),
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
            builder => builder.Services.AddModule<T>().AddModule<T2>().AddModule<T3>(),
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
            builder => builder.Services.AddModule<T>().AddModule<T2>().AddModule<T3>().AddModule<T4>(),
            modules => (
                modules.OfType<T>().Single(),
                modules.OfType<T2>().Single(),
                modules.OfType<T3>().Single(),
                modules.OfType<T4>().Single()));
    }

    /// <summary>
    /// Core helper method that executes modules and extracts results.
    /// Consolidates the common pattern of building a pipeline, executing, and extracting modules.
    /// </summary>
    /// <typeparam name="TResult">The return type (single module or tuple of modules).</typeparam>
    /// <param name="testHostSettings">Settings for configuring the test host.</param>
    /// <param name="configureModules">Action to add modules to the builder.</param>
    /// <param name="extractResults">Function to extract the desired modules from the results.</param>
    /// <returns>The extracted module(s).</returns>
    private async Task<TResult> ExecuteModulesAsync<TResult>(
        TestHostSettings testHostSettings,
        Action<PipelineBuilder> configureModules,
        Func<IEnumerable<IModule>, TResult> extractResults)
    {
        var builder = TestPipelineHostBuilder.Create(testHostSettings);
        configureModules(builder);
        var pipeline = builder.Build();

        _pipelines.Add(pipeline);

        using var cts = CreateTimeoutCancellationTokenSource(testHostSettings.TestTimeout);
        var pipelineResult = await pipeline.RunAsync(cts.Token);

        return extractResults(pipelineResult.Modules);
    }

    public async Task<T> GetService<T>()
        where T : notnull
    {
        var valueTuple = await GetService<T>((Action<IServiceCollection>?)null);
        return valueTuple.T;
    }

    public async Task<(T T, IPipeline Pipeline)> GetService<T>(Action<IServiceCollection>? configureServices)
        where T : notnull
    {
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddModule<DummyModule>();
        configureServices?.Invoke(builder.Services);
        var pipeline = builder.Build();

        _pipelines.Add(pipeline);

        // Trigger initialization by running the pipeline
        await pipeline.RunAsync();

        return (pipeline.Services.GetRequiredService<T>(), pipeline);
    }

    /// <summary>
    /// Gets a service from a freshly built pipeline with custom service configuration.
    /// This overload accepts a 2-argument delegate for backward compatibility.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <param name="configureServices">Action to configure services, receiving the builder as context.</param>
    /// <returns>A tuple containing the service and the pipeline (accessible via Host for backward compatibility).</returns>
    public async Task<(T T, IPipeline Host)> GetService<T>(Action<PipelineBuilder, IServiceCollection> configureServices)
        where T : notnull
    {
        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddModule<DummyModule>();
        configureServices(builder, builder.Services);
        var pipeline = builder.Build();

        _pipelines.Add(pipeline);

        // Trigger initialization by running the pipeline
        await pipeline.RunAsync();

        return (pipeline.Services.GetRequiredService<T>(), pipeline);
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
    public async Task DisposeCreatedPipelines()
    {
        await _pipelines.ToAsyncProcessorBuilder()
            .ForEachAsync(Disposer.DisposeObjectAsync)
            .ProcessInParallel();

        _pipelines.Clear();
    }
}
