using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

public class AlwaysRunTests : TestBase
{
    public class MyModule1 : ThrowingTestModule<bool>
    {
    }

    [ModularPipelines.Attributes.DependsOn<MyModule1>]
    public class MyModule2 : ThrowingTestModule<bool>, IAlwaysRun
    {
    }

    [ModularPipelines.Attributes.DependsOn<MyModule2>]
    public class MyModule3 : ThrowingTestModule<bool>, IAlwaysRun
    {
    }

    [ModularPipelines.Attributes.DependsOn<MyModule3>]
    public class MyModule4 : ThrowingTestModule<bool>, IAlwaysRun
    {
    }

    [Test]
    public async Task AlwaysRunModules_Will_Run_Even_With_Exception()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<MyModule1>()
            .AddModule<MyModule2>()
            .AddModule<MyModule3>()
            .AddModule<MyModule4>()
            .BuildHostAsync();

        try
        {
            await host.ExecutePipelineAsync();
        }
        catch
        {
            // Expected - pipeline will fail
        }

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        var result1 = resultRegistry.GetResult(typeof(MyModule1))!;
        var result2 = resultRegistry.GetResult(typeof(MyModule2))!;
        var result3 = resultRegistry.GetResult(typeof(MyModule3))!;
        var result4 = resultRegistry.GetResult(typeof(MyModule4))!;

        using (Assert.Multiple())
        {
            await Assert.That(result1.ModuleStatus).IsEqualTo(Status.Failed);
            await Assert.That(result2.ModuleStatus).IsEqualTo(Status.Failed);
            await Assert.That(result3.ModuleStatus).IsEqualTo(Status.Failed);
            await Assert.That(result4.ModuleStatus).IsNotEqualTo(Status.NotYetStarted);
        }
    }
}
