using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleConditionHandlerTests
{
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

    private static ModuleConditionHandler CreateHandler(DistributedOptions distributedOptions)
    {
        var contextProvider = new Mock<IPipelineContextProvider>();
        contextProvider.Setup(x => x.GetModuleContext()).Returns(Mock.Of<IPipelineHookContext>());

        return new ModuleConditionHandler(
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            Microsoft.Extensions.Options.Options.Create(distributedOptions),
            contextProvider.Object);
    }

    private static IModule CreateForeignOsModule()
    {
        return OperatingSystem.IsWindows()
            ? new LinuxOnlyModule()
            : new WindowsOnlyModule();
    }

    [RunOnLinuxOnly]
    private sealed class LinuxOnlyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>(null);
        }
    }

    [RunOnWindowsOnly]
    private sealed class WindowsOnlyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>(null);
        }
    }
}
