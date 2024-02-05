using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Exceptions;
using ModularPipelines.TestHelpers;
using TUnit.Assertions;
using TUnit.Core;

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
                    if (1.ToString() == "1")
                    {
                        throw new Exception();
                    }
                }), cancellationToken)
                .ProcessInParallel();

            return await NothingAsync();
        }
    }

    [Test]
    public async Task Submodule_With_Return_Type_Does_Not_Fail_And_Runs_Once()
    {
        var module = await RunModule<SubModulesWithReturnTypeModule>();

        var results = await module;

        Assert.Multiple(() =>
        {
            Assert.That(results.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            Assert.That(results.Value).Is.EquivalentTo(new List<string> { "1", "2", "3" });
            Assert.That(module.SubModuleRunCount).Is.EqualTo(3);
        });
    }

    [Test]
    public async Task Submodule_Without_Return_Type_Does_Not_Fail_And_Runs_Once()
    {
        var module = await RunModule<SubModulesWithoutReturnTypeModule>();

        var results = await module;

        Assert.Multiple(() =>
        {
            Assert.That(results.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            Assert.That(results.Value).Is.Null();
            Assert.That(module.SubModuleRunCount).Is.EqualTo(3);
        });
    }

    [Test]
    public async Task Submodule_With_Return_Type_Does_Not_Fail_Synchronous_And_Runs_Once()
    {
        var module = await RunModule<SubModulesWithReturnTypeModuleSynchronous>();

        var results = await module;

        await Assert.Multiple(() =>
        {
            Assert.That(results.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            Assert.That(results.Value!).Is.EquivalentTo(new List<string> { "1", "2", "3" });
            Assert.That(module.SubModuleRunCount).Is.EqualTo(3);
        });
    }

    [Test]
    public async Task Submodule_Without_Return_Type_Does_Not_Fail_Synchronous_And_Runs_Once()
    {
        var module = await RunModule<SubModulesWithoutReturnTypeModuleSynchronous>();

        var results = await module;

        Assert.Multiple(() =>
        {
            Assert.That(results.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            Assert.That(results.Value).Is.Null();
            Assert.That(module.SubModuleRunCount).Is.EqualTo(3);
        });
    }

    [Test]
    public void Failing_Submodule_With_Return_Type_Fails()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<FailingSubModulesWithReturnTypeModule>);

        Assert.Multiple(() =>
        {
            Assert.That(moduleFailedException?.InnerException).Is.TypeOf<SubModuleFailedException>();
            Assert.That((SubModuleFailedException) moduleFailedException!.InnerException!).Has.Message.EqualTo("The Sub-Module 1 has failed.");
        });
    }

    [Test]
    public async Task Failing_Submodule_Without_Return_Type_Fails()
    {
        Assert.That(RunModule<FailingSubModulesWithoutReturnTypeModule>)
            .Throws.TypeOf<SubModuleFailedException>();
        
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<FailingSubModulesWithoutReturnTypeModule>);

        Assert.Multiple(() =>
        {
            Assert.That(moduleFailedException?.InnerException).Is.TypeOf<SubModuleFailedException>();
            Assert.That((SubModuleFailedException) moduleFailedException!.InnerException!).Has.Message.EqualTo("The Sub-Module 1 has failed.");
        });
    }

    [Test]
    public void Failing_Submodule_With_Return_Type_Fails_Synchronous()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<FailingSubModulesWithReturnTypeModuleSynchronous>);

        Assert.Multiple(() =>
        {
            Assert.That(moduleFailedException?.InnerException).Is.TypeOf<SubModuleFailedException>();
            Assert.That((SubModuleFailedException) moduleFailedException!.InnerException!).Has.Message.EqualTo("The Sub-Module 1 has failed.");
        });
    }

    [Test]
    public void Failing_Submodule_Without_Return_Type_Fails_Synchronous()
    {
        var moduleFailedException = await Assert.ThrowsAsync<ModuleFailedException>(RunModule<FailingSubModulesWithoutReturnTypeModuleSynchronous>);

        Assert.Multiple(() =>
        {
            Assert.That(moduleFailedException?.InnerException).Is.TypeOf<SubModuleFailedException>();
            Assert.That((SubModuleFailedException) moduleFailedException!.InnerException!).Has.Message.EqualTo("The Sub-Module 1 has failed.");
        });
    }
}