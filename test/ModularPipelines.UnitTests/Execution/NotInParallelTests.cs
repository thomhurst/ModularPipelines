using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

[TUnit.Core.NotInParallel]
public class NotInParallelTests : TestBase
{
    private static readonly TimeSpan ModuleDelay = TimeSpan.FromMilliseconds(100);
    private static readonly NotInParallelTracker Tracker = new();

    [ModularPipelines.Attributes.NotInParallel]
    public class Module1 : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTests.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => ["Module2"];
    }

    [ModularPipelines.Attributes.NotInParallel]
    public class Module2 : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTests.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => ["Module1"];
    }

    [ModularPipelines.Attributes.NotInParallel]
    [ModularPipelines.Attributes.DependsOn<ParallelDependency>]
    public class NotParallelModuleWithParallelDependency : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTests.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => [];
        protected override int DelayMs => 50;
    }

    public class ParallelDependency : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(ModuleDelay, cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel]
    [ModularPipelines.Attributes.DependsOn<NotParallelModuleWithParallelDependency>]
    public class NotParallelModuleWithNonParallelDependency : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTests.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => ["NotParallelModuleWithParallelDependency"];
    }

    [Test]
    public async Task NotInParallel()
    {
        Tracker.Reset();

        await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .ExecutePipelineAsync();

        await Assert.That(Tracker.Violations).IsEmpty();
    }

    [Test]
    public async Task NotInParallel_With_ParallelDependency()
    {
        Tracker.Reset();

        await TestPipelineHostBuilder.Create()
            .AddModule<NotParallelModuleWithParallelDependency>()
            .AddModule<ParallelDependency>()
            .ExecutePipelineAsync();

        await Assert.That(Tracker.Violations).IsEmpty();
    }

    [Test]
    public async Task NotInParallel_With_NonParallelDependency()
    {
        Tracker.Reset();

        await TestPipelineHostBuilder.Create()
            .AddModule<NotParallelModuleWithParallelDependency>()
            .AddModule<ParallelDependency>()
            .AddModule<NotParallelModuleWithNonParallelDependency>()
            .ExecutePipelineAsync();

        await Assert.That(Tracker.Violations).IsEmpty();
    }
}
