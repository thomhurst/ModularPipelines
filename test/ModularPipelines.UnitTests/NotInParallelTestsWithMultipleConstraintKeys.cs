using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

[Repeat(5)]
public class NotInParallelTestsWithMultipleConstraintKeys : TestBase
{
    [ModularPipelines.Attributes.NotInParallel("A")]
    public class Module1 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("A", "B")]
    public class Module2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("B", "C")]
    public class Module3 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return GetType().Name;
        }
    }

    [ModularPipelines.Attributes.NotInParallel("D")]
    public class Module4 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            return GetType().Name;
        }
    }

    [Test, Retry(3)]
    public async Task NotInParallel_If_Any_Modules_Executing_With_Any_Of_Same_ConstraintKey()
    {
        var resultsTask = TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>()
            .ExecutePipelineAsync();

        var results = await resultsTask;
        
        var one = results.Modules.OfType<Module1>().First();
        var two = results.Modules.OfType<Module2>().First();
        var three = results.Modules.OfType<Module3>().First();
        var four = results.Modules.OfType<Module4>().First();

        await AssertAfter(one, two);

        await AssertParallel(one, three);
        await AssertParallel(one, four);
    }

    private async Task AssertAfter(ModuleBase one, ModuleBase two)
    {
        var modules = new[] { one, two };
        var firstModule = modules.OrderBy(x => x.StartTime).First();
        var secondModule = modules.OrderBy(x => x.StartTime).Last();
        
        await Assert.That(secondModule.StartTime)
            .Is
            .GreaterThan(firstModule.StartTime + TimeSpan.FromSeconds(1));
    }

    private async Task AssertParallel(ModuleBase one, ModuleBase two)
    {
        var modules = new[] { one, two };
        var firstModule = modules.OrderBy(x => x.StartTime).First();
        var secondModule = modules.OrderBy(x => x.StartTime).Last();

        await Assert.That(secondModule.StartTime)
            .Is
            .GreaterThanOrEqualTo(firstModule.StartTime)
            .And
            .Is
            .LessThanOrEqualTo(firstModule.EndTime);
    }
}