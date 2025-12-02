using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Exceptions;
using ModularPipelines.TestHelpers;
using Polly;
using Polly.Retry;

namespace ModularPipelines.UnitTests;

public class SubModuleTests : TestBase
{
    private class SubModulesWithReturnTypeModule : Module<string[]>
    {
        private int _subModuleRunCount;
        public int SubModuleRunCount => _subModuleRunCount;

        public override async Task<string[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .SelectAsync(async name =>
                {
                    Interlocked.Increment(ref _subModuleRunCount);
                    context.Logger.LogInformation("Running Submodule {Submodule}", name);
                    await Task.Yield();
                    return name;
                })
                .ProcessInParallel();
        }
    }

    private class SubModulesWithoutReturnTypeModule : Module<string[]>
    {
        private int _subModuleRunCount;
        public int SubModuleRunCount => _subModuleRunCount;

        public override async Task<string[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .ForEachAsync(async name =>
                {
                    Interlocked.Increment(ref _subModuleRunCount);
                    context.Logger.LogInformation("Running Submodule {Submodule}", name);
                    await Task.Yield();
                }, cancellationToken)
                .ProcessInParallel();

            return null;
        }
    }

    private class SubModulesWithReturnTypeModuleSynchronous : Module<string[]>
    {
        private int _subModuleRunCount;
        public int SubModuleRunCount => _subModuleRunCount;

        public override async Task<string[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .SelectAsync(name =>
                {
                    Interlocked.Increment(ref _subModuleRunCount);
                    context.Logger.LogInformation("Running Submodule {Submodule}", name);
                    return Task.FromResult(name);
                })
                .ProcessInParallel();
        }
    }

    private class SubModulesWithoutReturnTypeModuleSynchronous : Module<string[]>
    {
        private int _subModuleRunCount;
        public int SubModuleRunCount => _subModuleRunCount;

        public override async Task<string[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .ForEachAsync(name =>
                {
                    Interlocked.Increment(ref _subModuleRunCount);
                    context.Logger.LogInformation("Running Submodule {Submodule}", name);
                    return Task.CompletedTask;
                }, cancellationToken)
                .ProcessInParallel();

            return null;
        }
    }

    private class FailingSubModulesWithReturnTypeModule : Module<string[]>
    {
        public override async Task<string[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .SelectAsync(async (string name) =>
                {
                    await Task.Yield();
                    throw new SubModuleFailedException($"The Sub-Module {name} has failed.");
#pragma warning disable CS0162
                    return name;
#pragma warning restore CS0162
                })
                .ProcessInParallel();
        }
    }

    private class FailingSubModulesWithoutReturnTypeModule : Module<string[]>
    {
        public override async Task<string[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .ForEachAsync(async name =>
                {
                    await Task.Yield();
                    throw new SubModuleFailedException($"The Sub-Module {name} has failed.");
                }, cancellationToken)
                .ProcessInParallel();

            return null;
        }
    }

    private class FailingSubModulesWithReturnTypeModuleSynchronous : Module<string[]>
    {
        public override async Task<string[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await new[] { "1", "2", "3" }
                .ToAsyncProcessorBuilder()
                .SelectAsync((string name) =>
                {
                    if (1.ToString() == "1")
                    {
                        throw new SubModuleFailedException($"The Sub-Module {name} has failed.");
                    }

                    return Task.FromResult(name);
                })
                .ProcessInParallel();
        }
    }

    private class FailingSubModulesWithoutReturnTypeModuleSynchronous : Module<string[]>
    {
        public override async Task<string[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .ForEachAsync(name =>
                {
                    if (name == "1")
                    {
                        throw new SubModuleFailedException($"The Sub-Module {name} has failed.");
                    }
                    return Task.CompletedTask;
                }, cancellationToken)
                .ProcessInParallel();

            return null;
        }
    }

    private class SucceedingSubModulesDoNotRetryModule : Module<string[]>, IRetryable<string[]>
    {
        public int _oneCount;
        public int _twoCount;
        public int _threeCount;

        public AsyncRetryPolicy<string[]?> GetRetryPolicy(IPipelineContext context)
        {
            return Policy<string[]?>.Handle<Exception>().RetryAsync(3);
        }

        public override async Task<string[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            foreach (var name in new[] { "1", "2", "3" })
            {
                if (name == "1")
                {
                    _oneCount++;
                }

                if (name == "2")
                {
                    _twoCount++;
                }

                if (name == "3")
                {
                    _threeCount++;
                    throw new Exception();
                }

                await Task.Yield();
            }

            return null;
        }
    }

    private class SucceedingSubModulesDoNotRetryModule_WithReturnType : Module<string[]>, IRetryable<string[]>
    {
        public int _oneCount;
        public int _twoCount;
        public int _threeCount;

        public AsyncRetryPolicy<string[]?> GetRetryPolicy(IPipelineContext context)
        {
            return Policy<string[]?>.Handle<Exception>().RetryAsync(3);
        }

        public override async Task<string[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            foreach (var name in new[] { "1", "2", "3" })
            {
                if (name == "1")
                {
                    _oneCount++;
                }

                if (name == "2")
                {
                    _twoCount++;
                }

                if (name == "3")
                {
                    _threeCount++;
                    throw new Exception();
                }

                await Task.Yield();
            }

            return null;
        }
    }

    [Test]
    public async Task Submodule_With_Progress()
    {
        var host = await TestPipelineHostBuilder.Create(new TestHostSettings { ShowProgressInConsole = true })
            .AddModule<SubModulesWithReturnTypeModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();
        var module = host.RootServices.GetServices<IModule>().OfType<SubModulesWithReturnTypeModule>().First();

        using (Assert.Multiple())
        {
            await Assert.That(module.SubModuleRunCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Submodule_With_Return_Type_Does_Not_Fail_And_Runs_Once()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SubModulesWithReturnTypeModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();
        var module = host.RootServices.GetServices<IModule>().OfType<SubModulesWithReturnTypeModule>().First();

        using (Assert.Multiple())
        {
            await Assert.That(module.SubModuleRunCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Submodule_Without_Return_Type_Does_Not_Fail_And_Runs_Once()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SubModulesWithoutReturnTypeModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();
        var module = host.RootServices.GetServices<IModule>().OfType<SubModulesWithoutReturnTypeModule>().First();

        using (Assert.Multiple())
        {
            await Assert.That(module.SubModuleRunCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Submodule_With_Return_Type_Does_Not_Fail_Synchronous_And_Runs_Once()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SubModulesWithReturnTypeModuleSynchronous>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();
        var module = host.RootServices.GetServices<IModule>().OfType<SubModulesWithReturnTypeModuleSynchronous>().First();

        using (Assert.Multiple())
        {
            await Assert.That(module.SubModuleRunCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Submodule_Without_Return_Type_Does_Not_Fail_Synchronous_And_Runs_Once()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SubModulesWithoutReturnTypeModuleSynchronous>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();
        var module = host.RootServices.GetServices<IModule>().OfType<SubModulesWithoutReturnTypeModuleSynchronous>().First();

        using (Assert.Multiple())
        {
            await Assert.That(module.SubModuleRunCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Failing_Submodule_With_Return_Type_Fails()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await TestPipelineHostBuilder.Create()
                .AddModule<FailingSubModulesWithReturnTypeModule>()
                .ExecutePipelineAsync());

        using (Assert.Multiple())
        {
            await Assert.That(moduleFailedException.InnerException).IsTypeOf<SubModuleFailedException>();
            await Assert.That(moduleFailedException.InnerException).HasMessageEqualTo("The Sub-Module 1 has failed.");
        }
    }

    [Test]
    public async Task Failing_Submodule_Without_Return_Type_Fails()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await TestPipelineHostBuilder.Create()
                .AddModule<FailingSubModulesWithoutReturnTypeModule>()
                .ExecutePipelineAsync());

        await Assert.That(exception.InnerException).IsTypeOf<SubModuleFailedException>();

        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await TestPipelineHostBuilder.Create()
                .AddModule<FailingSubModulesWithoutReturnTypeModule>()
                .ExecutePipelineAsync());

        using (Assert.Multiple())
        {
            await Assert.That(moduleFailedException?.InnerException).IsTypeOf<SubModuleFailedException>();
            await Assert.That(moduleFailedException!.InnerException!)
                .HasMessageEqualTo("The Sub-Module 1 has failed.")
                .Or.HasMessageEqualTo("The Sub-Module 2 has failed.")
                .Or.HasMessageEqualTo("The Sub-Module 3 has failed.");
        }
    }

    [Test]
    public async Task Failing_Submodule_With_Return_Type_Fails_Synchronous()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await TestPipelineHostBuilder.Create()
                .AddModule<FailingSubModulesWithReturnTypeModuleSynchronous>()
                .ExecutePipelineAsync());

        using (Assert.Multiple())
        {
            await Assert.That(moduleFailedException?.InnerException).IsTypeOf<SubModuleFailedException>();
            await Assert.That(moduleFailedException!.InnerException!).HasMessageEqualTo("The Sub-Module 1 has failed.");
        }
    }

    [Test]
    public async Task Failing_Submodule_Without_Return_Type_Fails_Synchronous()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await TestPipelineHostBuilder.Create()
                .AddModule<FailingSubModulesWithoutReturnTypeModuleSynchronous>()
                .ExecutePipelineAsync());

        using (Assert.Multiple())
        {
            await Assert.That(moduleFailedException.InnerException).IsTypeOf<SubModuleFailedException>();
            await Assert.That(moduleFailedException.InnerException!).HasMessageEqualTo("The Sub-Module 1 has failed.");
        }
    }

    [Test]
    public async Task Module_With_Retry_Policy_Retries_Entire_Execution()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SucceedingSubModulesDoNotRetryModule>()
            .BuildHostAsync();

        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await host.ExecutePipelineAsync());

        var module = host.RootServices.GetServices<IModule>().OfType<SucceedingSubModulesDoNotRetryModule>().First();

        // Polly retries the entire module execution, so all counters increment on each retry
        // With RetryAsync(3), we get 1 original + 3 retries = 4 total executions
        using (Assert.Multiple())
        {
            await Assert.That(module._oneCount).IsEqualTo(4);
            await Assert.That(module._twoCount).IsEqualTo(4);
            await Assert.That(module._threeCount).IsEqualTo(4);
        }
    }

    [Test]
    public async Task Module_With_Retry_Policy_Retries_Entire_Execution_With_Return_Type()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SucceedingSubModulesDoNotRetryModule_WithReturnType>()
            .BuildHostAsync();

        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await host.ExecutePipelineAsync());

        var module = host.RootServices.GetServices<IModule>().OfType<SucceedingSubModulesDoNotRetryModule_WithReturnType>().First();

        // Polly retries the entire module execution, so all counters increment on each retry
        // With RetryAsync(3), we get 1 original + 3 retries = 4 total executions
        using (Assert.Multiple())
        {
            await Assert.That(module._oneCount).IsEqualTo(4);
            await Assert.That(module._twoCount).IsEqualTo(4);
            await Assert.That(module._threeCount).IsEqualTo(4);
        }
    }
}
