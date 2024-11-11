using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class AlwaysRunTests : TestBase
{
    public class MyModule1 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<MyModule1>]
    public class MyModule2 : Module
    {
        public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun;

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<MyModule2>]
    public class MyModule3 : Module
    {
        public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun;

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [ModularPipelines.Attributes.DependsOn<MyModule3>]
    public class MyModule4 : Module
    {
        public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun;

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            throw new Exception();
        }
    }

    [Test]
    public async Task AlwaysRunModules_Will_Run_Even_With_Exception()
    {
        var (myModule1, myModule2, myModule3, myModule4)
            = await RunModules<MyModule1, MyModule2, MyModule3, MyModule4>();
        
        using (Assert.Multiple())
        {
            await Assert.That(myModule1.Status).IsEqualTo(Status.Failed);
            await Assert.That(myModule2.Status).IsEqualTo(Status.Failed);
            await Assert.That(myModule3.Status).IsEqualTo(Status.Failed);
            await Assert.That(myModule4.Status).IsNotEqualTo(Status.NotYetStarted);
        }
    }
}