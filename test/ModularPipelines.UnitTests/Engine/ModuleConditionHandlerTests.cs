using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.UnitTests.Attributes;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class ModuleConditionHandlerTests
{
    private static int _conditionEvaluationCount;
    private static int _deferredDiscoveryConditionConstructions;
    private static int _mixedAlternativeEvaluationCount;

    [Test]
    public async Task Distributed_Master_Does_Not_Filter_Foreign_Os_Module()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });

        var result = await handler.ShouldIgnore(CreateForeignOsModule());

        await Assert.That(result.ShouldIgnore).IsFalse();
    }

    [Test]
    public async Task Standalone_Execution_Filters_Foreign_Os_Module()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = false,
            InstanceIndex = 0,
            TotalInstances = 1,
        });

        var result = await handler.ShouldIgnore(CreateForeignOsModule());

        await Assert.That(result.ShouldIgnore).IsTrue();
    }

    [Test]
    public async Task Environment_Master_Override_Does_Not_Filter_Foreign_Os_Module()
    {
        var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");

        try
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "0");

            var handler = CreateHandler(new DistributedOptions
            {
                Enabled = true,
                InstanceIndex = 2,
                TotalInstances = 3,
            });

            var result = await handler.ShouldIgnore(CreateForeignOsModule());

            await Assert.That(result.ShouldIgnore).IsFalse();
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
        }
    }

    [Test]
    public async Task Distributed_Master_Filters_Module_With_Contradictory_Os_Conditions()
    {
        // A module requiring more than one operating system can never run on any single
        // worker, so the master must still skip it rather than publish an assignment that
        // requires multiple mutually exclusive OS capabilities and waiting forever for it.
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });

        var result = await handler.ShouldIgnore(new ContradictoryOsModule());

        await Assert.That(result.ShouldIgnore).IsTrue();
    }

    [Test]
    public async Task Distributed_Master_Discovery_Filters_Module_With_Contradictory_Os_Conditions()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });

        var result = await handler.ShouldIgnoreByCategory(new ContradictoryOsModule());

        await Assert.That(result.ShouldIgnore).IsTrue();
    }

    [Test]
    public async Task Distributed_Master_Discovery_Filters_Contradictory_Alternative_Os_Conditions()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });

        var result = await handler.ShouldIgnoreByCategory(new ContradictoryAlternativeOsModule());

        await Assert.That(result.ShouldIgnore).IsTrue();
    }

    [Test]
    public async Task Distributed_Master_Discovery_Filters_Contradictory_Os_Condition_Group()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });

        var result = await handler.ShouldIgnoreByCategory(new ContradictoryUnixOsModule());

        await Assert.That(result.ShouldIgnore).IsTrue();
    }

    [Test]
    public async Task Distributed_Master_Discovery_Does_Not_Filter_Routable_Os_Condition()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });

        var result = await handler.ShouldIgnoreByCategory(CreateForeignOsModule());

        await Assert.That(result.ShouldIgnore).IsFalse();
    }

    [Test]
    public async Task Distributed_Master_Discovery_Does_Not_Construct_Deferred_Conditions()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });
        _deferredDiscoveryConditionConstructions = 0;

        _ = await handler.ShouldIgnoreByCategory(new DeferredDiscoveryConditionModule());

        await Assert.That(_deferredDiscoveryConditionConstructions).IsEqualTo(0);
    }

    [Test]
    public async Task Distributed_Master_Graph_Defers_Routable_Os_Condition()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });

        var result = await handler.ShouldIgnoreForGraphPlanning(
            CreateForeignOsModule(),
            Mock.Of<IModuleMetadataRegistry>());

        using (Assert.Multiple())
        {
            await Assert.That(result.ShouldIgnore).IsFalse();
            await Assert.That(result.IsResolved).IsFalse();
        }
    }

    [Test]
    public async Task Distributed_Master_Does_Not_Filter_Unix_Condition_Group()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });

        var result = await handler.ShouldIgnore(new UnixModule());

        await Assert.That(result.ShouldIgnore).IsFalse();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task Distributed_Master_Does_Not_Filter_Alternative_Os_Condition()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });

        var result = await handler.ShouldIgnore(new UnixAlternativeModule());

        await Assert.That(result.ShouldIgnore).IsFalse();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task Distributed_Master_Graph_Defers_Alternative_Os_Condition()
    {
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        });

        var result = await handler.ShouldIgnoreForGraphPlanning(
            new UnixAlternativeModule(),
            Mock.Of<IModuleMetadataRegistry>());

        using (Assert.Multiple())
        {
            await Assert.That(result.ShouldIgnore).IsFalse();
            await Assert.That(result.IsResolved).IsFalse();
        }
    }

    [Test]
    public async Task Grouped_Alternatives_Run_When_One_Condition_Matches()
    {
        var handler = CreateHandler(new DistributedOptions());

        var result = await handler.ShouldIgnore(new MatchingAlternativeModule());

        await Assert.That(result.ShouldIgnore).IsFalse();
    }

    [Test]
    public async Task Grouped_Alternatives_Skip_When_No_Condition_Matches()
    {
        var handler = CreateHandler(new DistributedOptions());

        var result = await handler.ShouldIgnore(new NoMatchingAlternativeModule());

        await Assert.That(result.ShouldIgnore).IsTrue();
    }

    [Test]
    public async Task Distributed_Master_Prepares_Non_Platform_Grouped_Alternatives()
    {
        _mixedAlternativeEvaluationCount = 0;
        var conditionRouting = new DistributedConditionRouting();
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        }, distributedConditionRouting: conditionRouting);
        var module = new MixedMatchingAlternativeModule();

        await handler.PrepareDistributedRoutingAsync(module);

        using (Assert.Multiple())
        {
            await Assert.That(_mixedAlternativeEvaluationCount).IsEqualTo(1);
            await Assert.That(conditionRouting.IsLocallySatisfied(
                module,
                typeof(MixedAlternativeModule))).IsTrue();
        }
    }

    [Test]
    public async Task Distributed_Master_Routing_Preserves_Condition_ShortCircuiting()
    {
        _mixedAlternativeEvaluationCount = 0;
        var handler = CreateHandler(new DistributedOptions
        {
            Enabled = true,
            InstanceIndex = 0,
            TotalInstances = 3,
        }, distributedConditionRouting: new DistributedConditionRouting());

        await handler.PrepareDistributedRoutingAsync(new MandatoryFalseMixedAlternativeModule());

        await Assert.That(_mixedAlternativeEvaluationCount).IsEqualTo(0);
    }

    [Test]
    public async Task Distributed_Worker_Honors_Restored_Satisfied_Group()
    {
        _mixedAlternativeEvaluationCount = 0;
        var masterModule = new MixedMatchingAlternativeModule();
        var masterRouting = new DistributedConditionRouting();
        masterRouting.MarkLocallySatisfied(masterModule, typeof(MixedAlternativeModule));
        var workerModule = new MixedMatchingAlternativeModule();
        var workerRouting = new DistributedConditionRouting();
        workerRouting.RestoreLocallySatisfiedGroups(
            workerModule,
            masterRouting.GetLocallySatisfiedGroupNames(masterModule));
        var handler = CreateHandler(
            new DistributedOptions(),
            distributedConditionRouting: workerRouting);

        var result = await handler.ShouldIgnore(workerModule);

        using (Assert.Multiple())
        {
            await Assert.That(result.ShouldIgnore).IsFalse();
            await Assert.That(_mixedAlternativeEvaluationCount).IsEqualTo(0);
        }
    }

    [Test]
    public async Task Mandatory_Condition_Is_Not_Overridden_By_Optional_Alternative()
    {
        var logger = Mock.Of<IModuleLogger>();
        var context = Mock.Of<IPipelineContext>(x => x.Logger == logger);
        var handler = CreateHandler(new DistributedOptions(), context);

        var result = await handler.ShouldIgnore(new MandatoryFalseOptionalTrueModule());

        await Assert.That(result.ShouldIgnore).IsTrue();
    }

    [Test]
    public async Task ShouldIgnore_EvaluatesConditionsOncePerModuleInstance()
    {
        _conditionEvaluationCount = 0;
        var handler = CreateHandler(new DistributedOptions());
        var firstModule = new CountingConditionModule();

        await handler.ShouldIgnore(firstModule);
        await handler.ShouldIgnore(firstModule);
        await handler.ShouldIgnore(new CountingConditionModule());

        await Assert.That(_conditionEvaluationCount).IsEqualTo(2);
    }

    private static ModuleConditionHandler CreateHandler(
        DistributedOptions distributedOptions,
        IPipelineContext? pipelineContext = null,
        DistributedConditionRouting? distributedConditionRouting = null)
    {
        var contextProvider = new Mock<IPipelineContextProvider>();
        contextProvider
            .Setup(x => x.GetModuleContext())
            .Returns(pipelineContext ?? Mock.Of<IPipelineContext>());

        // A bare mock reports no category (GetCategory returns null) and a no-op
        // FinalizeMetadata, so category filtering never interferes with these OS-condition tests.
        var metadataRegistry = Mock.Of<IModuleMetadataRegistry>();

        return new ModuleConditionHandler(
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            Microsoft.Extensions.Options.Options.Create(distributedOptions),
            new RoleDetector(Microsoft.Extensions.Options.Options.Create(distributedOptions)),
            contextProvider.Object,
            metadataRegistry,
            distributedConditionRouting);
    }

    private static IModule CreateForeignOsModule()
    {
        return OperatingSystem.IsWindows()
            ? new LinuxOnlyModule()
            : new WindowsOnlyModule();
    }

    [RunIf<OnLinux>]
    private sealed class LinuxOnlyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }

    [RunIf<OnWindows>]
    private sealed class WindowsOnlyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }

    [RunIf<OnWindows>]
    [RunIf<OnLinux>]
    private sealed class ContradictoryOsModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }

    [RunIfAny<OnLinux, OnMacOS>]
    [RunIf<OnWindows>]
    private sealed class ContradictoryAlternativeOsModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [RunIf<OnUnix>]
    [RunIf<OnWindows>]
    private sealed class ContradictoryUnixOsModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [DeferredDiscoveryCondition]
    private sealed class DeferredDiscoveryConditionModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(string.Empty);
    }

    private sealed class DeferredDiscoveryConditionAttribute : Attribute, IConditionAttribute
    {
        public DeferredDiscoveryConditionAttribute() =>
            Interlocked.Increment(ref _deferredDiscoveryConditionConstructions);

        public ConditionLogic Logic => ConditionLogic.All;

        public string ConditionNames => nameof(DeferredDiscoveryConditionAttribute);

        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(true);
    }

    [RunIf<OnUnix>]
    private sealed class UnixModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }

    [RunIfAny<OnLinux, OnMacOS>]
    private sealed class UnixAlternativeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(string.Empty);
    }

    [AlternativeCondition(false)]
    [AlternativeCondition(true)]
    private sealed class MatchingAlternativeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }

    [AlternativeCondition(false)]
    [AlternativeCondition(false)]
    private sealed class NoMatchingAlternativeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }

    [MixedOperatingSystem<OnLinux>]
    [MixedAlternativeCondition(false)]
    private sealed class MixedAlternativeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [MixedOperatingSystem<OnLinux>]
    [MixedAlternativeCondition(true)]
    private sealed class MixedMatchingAlternativeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [MandatoryCondition(false)]
    [MixedOperatingSystem<OnLinux>]
    [MixedAlternativeCondition(true)]
    private sealed class MandatoryFalseMixedAlternativeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    private sealed class MixedOperatingSystemAttribute<TCondition> : RunIfAnyAttribute,
        IGroupedConditionAttribute
        where TCondition : IRunCondition, new()
    {
        public Type ConditionGroupType => typeof(MixedAlternativeModule);

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            new TCondition().EvaluateAsync(context);
    }

    private sealed class MixedAlternativeConditionAttribute(bool result) : Attribute,
        IGroupedConditionAttribute
    {
        public ConditionLogic Logic => ConditionLogic.Any;

        public Type ConditionGroupType => typeof(MixedAlternativeModule);

        public string ConditionNames => nameof(MixedAlternativeConditionAttribute);

        public Task<bool> EvaluateAsync(IPipelineContext context)
        {
            Interlocked.Increment(ref _mixedAlternativeEvaluationCount);
            return Task.FromResult(result);
        }
    }

    [MandatoryCondition(false)]
    [AlternativeCondition(true)]
    private sealed class MandatoryFalseOptionalTrueModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    private sealed class AlternativeConditionAttribute(bool result) : Attribute, IGroupedConditionAttribute
    {
        public ConditionLogic Logic => ConditionLogic.Any;

        public Type ConditionGroupType => typeof(AlternativeConditionAttribute);

        public string ConditionNames => nameof(AlternativeConditionAttribute);

        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(result);
    }

    private sealed class MandatoryConditionAttribute(bool result) : Attribute, IConditionAttribute
    {
        public ConditionLogic Logic => ConditionLogic.All;

        public string ConditionNames => nameof(MandatoryConditionAttribute);

        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(result);
    }

    private sealed class CountingCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context)
        {
            Interlocked.Increment(ref _conditionEvaluationCount);
            return Task.FromResult(true);
        }
    }

    [RunIf<CountingCondition>]
    private sealed class CountingConditionModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
