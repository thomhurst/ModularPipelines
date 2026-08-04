using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class ModuleConditionHandlerTests
{
    private static int _conditionEvaluationCount;

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
        IPipelineContext? pipelineContext = null)
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
            metadataRegistry);
    }

    private static IModule CreateForeignOsModule()
    {
        return OperatingSystem.IsWindows()
            ? new LinuxOnlyModule()
            : new WindowsOnlyModule();
    }

    [RunIfAll<OnLinux>]
    private sealed class LinuxOnlyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }

    [RunIfAll<OnWindows>]
    private sealed class WindowsOnlyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }

    [RunIfAll<OnWindows>]
    [RunIfAll<OnLinux>]
    private sealed class ContradictoryOsModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
    }

    [RunIfAll<OnUnix>]
    private sealed class UnixModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(string.Empty);
        }
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

    [RunIfAll<CountingCondition>]
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
