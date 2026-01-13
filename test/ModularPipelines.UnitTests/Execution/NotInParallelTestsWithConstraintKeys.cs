using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Execution;

[TUnit.Core.NotInParallel]
public class NotInParallelTestsWithConstraintKeys : TestBase
{
    private static readonly NotInParallelTracker Tracker = new();

    [ModularPipelines.Attributes.NotInParallel("A")]
    public class ModuleWithAConstraintKey1 : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTestsWithConstraintKeys.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => ["ModuleWithAConstraintKey2"];
    }

    [ModularPipelines.Attributes.NotInParallel("A")]
    public class ModuleWithAConstraintKey2 : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTestsWithConstraintKeys.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => ["ModuleWithAConstraintKey1"];
    }

    [ModularPipelines.Attributes.NotInParallel("B")]
    public class ModuleWithBConstraintKey1 : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTestsWithConstraintKeys.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => ["ModuleWithBConstraintKey2"];
    }

    [ModularPipelines.Attributes.NotInParallel("B")]
    public class ModuleWithBConstraintKey2 : NotInParallelTestModule
    {
        protected override NotInParallelTracker Tracker => NotInParallelTestsWithConstraintKeys.Tracker;
        protected override IEnumerable<string> ConflictingModuleNames => ["ModuleWithBConstraintKey1"];
    }

    [Test]
    public async Task NotInParallel_If_Same_ConstraintKey()
    {
        Tracker.Reset();

        await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithAConstraintKey1>()
            .AddModule<ModuleWithAConstraintKey2>()
            .AddModule<ModuleWithBConstraintKey1>()
            .AddModule<ModuleWithBConstraintKey2>()
            .ExecutePipelineAsync();

        await Assert.That(Tracker.Violations).IsEmpty();
    }
}
