using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class NotInParallelTestsWithConstraintKeys : TestBase
{
    [ModularPipelines.Attributes.NotInParallel(ConstraintKey = "A")]
    public class ModuleWithAConstraintKey1 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel(ConstraintKey = "A")]
    public class ModuleWithAConstraintKey2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel(ConstraintKey = "B")]
    public class ModuleWithBConstraintKey1 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel(ConstraintKey = "B")]
    public class ModuleWithBConstraintKey2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return GetType().Name;
        }
    }

    [Test, Retry(3)]
    public async Task NotInParallel_If_Same_ConstraintKey()
    {
        var results = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithAConstraintKey1>()
            .AddModule<ModuleWithAConstraintKey2>()
            .AddModule<ModuleWithBConstraintKey1>()
            .AddModule<ModuleWithBConstraintKey2>()
            .ExecutePipelineAsync();

        var a1 = results.Modules.OfType<ModuleWithAConstraintKey1>().First();
        var a2 = results.Modules.OfType<ModuleWithAConstraintKey2>().First();
        var b1 = results.Modules.OfType<ModuleWithBConstraintKey1>().First();
        var b2 = results.Modules.OfType<ModuleWithBConstraintKey2>().First();

        await AssertAfter(a1, a2, TimeSpan.FromSeconds(1));
        await AssertAfter(b1, b2, TimeSpan.FromSeconds(1));

        await AssertParallel(a1, b1);
        await AssertParallel(a2, b2);
    }

    private async Task AssertAfter(ModuleBase firstModule, ModuleBase nextModule, TimeSpan expectedTimeAfter)
    {
        await Assert.That(nextModule.StartTime)
            .Is.EqualToWithTolerance((firstModule.StartTime + expectedTimeAfter), TimeSpan.FromMilliseconds(350));
    }

    private async Task AssertParallel(ModuleBase firstModule, ModuleBase nextModule)
    {
        await Assert.That(nextModule.StartTime).
            Is.EqualToWithTolerance(firstModule.StartTime, TimeSpan.FromMilliseconds(350));
    }
}