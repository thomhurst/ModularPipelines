using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

[TUnit.Core.NotInParallel]
public class NotInParallelTestsWithMultipleConstraintKeys : TestBase
{
    private static readonly NotInParallelTracker Tracker = new();

    [ModularPipelines.Attributes.NotInParallel("A")]
    public class Module1 : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTestsWithMultipleConstraintKeys.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => ["Module2"];
    }

    [ModularPipelines.Attributes.NotInParallel("A", "B")]
    public class Module2 : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTestsWithMultipleConstraintKeys.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => ["Module1", "Module3"];
    }

    [ModularPipelines.Attributes.NotInParallel("B", "C")]
    public class Module3 : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTestsWithMultipleConstraintKeys.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => ["Module2"];
    }

    [ModularPipelines.Attributes.NotInParallel("D")]
    public class Module4 : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTestsWithMultipleConstraintKeys.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => [];
        protected override int DelayMs => 50;
    }

    [Test]
    public async Task NotInParallel_If_Any_Modules_Executing_With_Any_Of_Same_ConstraintKey()
    {
        Tracker.Reset();

        await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>()
            .ExecutePipelineAsync();

        await Assert.That(Tracker.Violations).IsEmpty();
    }
}
