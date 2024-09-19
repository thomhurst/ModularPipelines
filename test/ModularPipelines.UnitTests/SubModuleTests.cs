using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
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

        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .SelectAsync(name => SubModule(name, () =>
                {
                    Interlocked.Increment(ref _subModuleRunCount);
                    context.Logger.LogInformation("Running Submodule {Submodule}", name);
                    return Task.FromResult(name);
                }))
                .ProcessInParallel();
        }
    }

    private class SubModulesWithoutReturnTypeModule : Module<string[]>
    {
        private int _subModuleRunCount;
        public int SubModuleRunCount => _subModuleRunCount;

        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .ForEachAsync(name => SubModule(name, async () =>
                {
                    Interlocked.Increment(ref _subModuleRunCount);
                    context.Logger.LogInformation("Running Submodule {Submodule}", name);
                    await Task.Yield();
                }), cancellationToken)
                .ProcessInParallel();

            return await NothingAsync();
        }
    }

    private class SubModulesWithReturnTypeModuleSynchronous : Module<string[]>
    {
        private int _subModuleRunCount;
        public int SubModuleRunCount => _subModuleRunCount;

        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .SelectAsync(name => SubModule(name, () =>
                {
                    Interlocked.Increment(ref _subModuleRunCount);
                    context.Logger.LogInformation("Running Submodule {Submodule}", name);
                    return name;
                }))
                .ProcessInParallel();
        }
    }

    private class SubModulesWithoutReturnTypeModuleSynchronous : Module<string[]>
    {
        private int _subModuleRunCount;
        public int SubModuleRunCount => _subModuleRunCount;

        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .ForEachAsync(name => SubModule(name, () =>
                {
                    Interlocked.Increment(ref _subModuleRunCount);
                    context.Logger.LogInformation("Running Submodule {Submodule}", name);
                }), cancellationToken)
                .ProcessInParallel();

            return await NothingAsync();
        }
    }

    private class FailingSubModulesWithReturnTypeModule : Module<string[]>
    {
        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .SelectAsync(name => SubModule<string>(name, async () =>
                {
                    await Task.Yield();
                    throw new Exception();
                }))
                .ProcessInParallel();
        }
    }

    private class FailingSubModulesWithoutReturnTypeModule : Module<string[]>
    {
        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .ForEachAsync(name => SubModule(name, async () =>
                {
                    await Task.Yield();
                    throw new Exception();
                }), cancellationToken)
                .ProcessInParallel();

            return await NothingAsync();
        }
    }

    private class FailingSubModulesWithReturnTypeModuleSynchronous : Module<string[]>
    {
        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await new[] { "1", "2", "3" }
                .ToAsyncProcessorBuilder()
                .SelectAsync(async name => await SubModule<string>(name, () =>
                {
                    if (1.ToString() == "1")
                    {
                        throw new Exception();
                    }

                    return "";
                }))
                .ProcessInParallel();
        }
    }

    private class FailingSubModulesWithoutReturnTypeModuleSynchronous : Module<string[]>
    {
        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await new[] { "1", "2", "3" }.ToAsyncProcessorBuilder()
                .ForEachAsync(name => SubModule(name, () =>
                {
                    if (name == "1")
                    {
                        throw new Exception();
                    }
                }), cancellationToken)
                .ProcessInParallel();

            return await NothingAsync();
        }
    }
    
    private class SucceedingSubModulesDoNotRetryModule : Module<string[]>
    {
        public int _oneCount;
        public int _twoCount;
        public int _threeCount;

        protected override AsyncRetryPolicy<string[]?> RetryPolicy { get; } =
            Policy<string[]?>.Handle<Exception>().RetryAsync(3);

        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            foreach (var name in new[] { "1", "2", "3" })
            {
                await SubModule<string>(name, () =>
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

                    return "";
                });
            }

            return null;
        }
    }
    
    private class SucceedingSubModulesDoNotRetryModule_WithReturnType : Module<string[]>
    {
        public int _oneCount;
        public int _twoCount;
        public int _threeCount;

        protected override AsyncRetryPolicy<string[]?> RetryPolicy { get; } =
            Policy<string[]?>.Handle<Exception>().RetryAsync(3);

        protected override async Task<string[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            foreach (var name in new[] { "1", "2", "3" })
            {
                await SubModule<string>(name, () =>
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

                    return "";
                });
            }

            return null;
        }
    }

    [Test]
    public async Task Submodule_With_Return_Type_Does_Not_Fail_And_Runs_Once()
    {
        var module = await RunModule<SubModulesWithReturnTypeModule>();

        var results = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(results.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(results.Value).IsEquivalentTo(new List<string> { "1", "2", "3" });
            await Assert.That(module.SubModuleRunCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Submodule_Without_Return_Type_Does_Not_Fail_And_Runs_Once()
    {
        var module = await RunModule<SubModulesWithoutReturnTypeModule>();

        var results = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(results.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(results.Value).IsNull();
            await Assert.That(module.SubModuleRunCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Submodule_With_Return_Type_Does_Not_Fail_Synchronous_And_Runs_Once()
    {
        var module = await RunModule<SubModulesWithReturnTypeModuleSynchronous>();

        var results = await module;

        await using (Assert.Multiple())
        {
            await Assert.That(results.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(results.Value!).IsEquivalentTo(new List<string> { "1", "2", "3" });
            await Assert.That(module.SubModuleRunCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Submodule_Without_Return_Type_Does_Not_Fail_Synchronous_And_Runs_Once()
    {
        var module = await RunModule<SubModulesWithoutReturnTypeModuleSynchronous>();

        var results = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(results.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(results.Value).IsNull();
            await Assert.That(module.SubModuleRunCount).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Failing_Submodule_With_Return_Type_Fails()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<FailingSubModulesWithReturnTypeModule>);

        await using (Assert.Multiple())
        {
            await Assert.That(moduleFailedException.InnerException).IsTypeOf(typeof(SubModuleFailedException));
            await Assert.That(moduleFailedException.InnerException).HasMessage().EqualTo("The Sub-Module 1 has failed.");
        }
    }

    [Test]
    public async Task Failing_Submodule_Without_Return_Type_Fails()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<FailingSubModulesWithoutReturnTypeModule>);
            
        await Assert.That(exception.InnerException).IsTypeOf(typeof(SubModuleFailedException));

        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<FailingSubModulesWithoutReturnTypeModule>);

        await using (Assert.Multiple())
        {
            await Assert.That(moduleFailedException?.InnerException).IsTypeOf(typeof(SubModuleFailedException));
            await Assert.That(moduleFailedException!.InnerException!)
                .HasMessage().EqualTo("The Sub-Module 1 has failed.")
                .Or.HasMessage().EqualTo("The Sub-Module 2 has failed.")
                .Or.HasMessage().EqualTo("The Sub-Module 3 has failed.");
        }
    }

    [Test]
    public async Task Failing_Submodule_With_Return_Type_Fails_Synchronous()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<FailingSubModulesWithReturnTypeModuleSynchronous>);

        await using (Assert.Multiple())
        {
            await Assert.That(moduleFailedException?.InnerException).IsTypeOf(typeof(SubModuleFailedException));
            await Assert.That(moduleFailedException!.InnerException!).HasMessage().EqualTo("The Sub-Module 1 has failed.");
        }
    }

    [Test]
    public async Task Failing_Submodule_Without_Return_Type_Fails_Synchronous()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<FailingSubModulesWithoutReturnTypeModuleSynchronous>);

        await using (Assert.Multiple())
        {
            await Assert.That(moduleFailedException.InnerException).IsTypeOf(typeof(SubModuleFailedException));
            await Assert.That(moduleFailedException.InnerException!).HasMessage().EqualTo("The Sub-Module 1 has failed.");
        }
    }
    
    [Test]
    public async Task Succeeding_Submodules_Do_Not_Retry()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<SucceedingSubModulesDoNotRetryModule>);

        var module = (SucceedingSubModulesDoNotRetryModule) moduleFailedException.Module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(module._oneCount).IsEqualTo(1);
            await Assert.That(module._twoCount).IsEqualTo(1);
            await Assert.That(module._threeCount).IsEqualTo(4);
        }
    }
    
    [Test]
    public async Task Succeeding_Submodules_Do_Not_Retry_With_Return_Type()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<SucceedingSubModulesDoNotRetryModule_WithReturnType>);

        var module = (SucceedingSubModulesDoNotRetryModule_WithReturnType) moduleFailedException.Module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(module._oneCount).IsEqualTo(1);
            await Assert.That(module._twoCount).IsEqualTo(1);
            await Assert.That(module._threeCount).IsEqualTo(4);
        }
    }
}