using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Distributed.UnitTests.Configuration;

[TUnit.Core.NotInParallel(nameof(DistributedOptionsTests))]
public class DistributedOptionsTests
{
    [Test]
    public async Task ModuleResultTimeout_Defaults_To_Forty_Five_Minutes()
    {
        var options = new DistributedOptions();

        using (Assert.Multiple())
        {
            await Assert.That(options.CapabilityTimeout).IsEqualTo(TimeSpan.FromMinutes(5));
            await Assert.That(options.MinimumWorkerCount).IsEqualTo(0);
            await Assert.That(options.ModuleResultTimeout).IsEqualTo(TimeSpan.FromMinutes(45));
            await Assert.That(options.RequireExplicitRunId).IsFalse();
        }
    }

    [Test]
    public async Task Capabilities_CanBeBoundFromConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Distributed:Capabilities:0"] = "docker",
                ["Distributed:Capabilities:1"] = "gpu",
                ["Distributed:CapabilityTimeout"] = "00:00:30",
                ["Distributed:MinimumWorkerCount"] = "2",
            })
            .Build();
        var options = new DistributedOptions();

        configuration.GetSection("Distributed").Bind(options);

        using (Assert.Multiple())
        {
            await Assert.That(options.Capabilities)
                .IsEquivalentTo([Capability.Docker, Capability.Gpu]);
            await Assert.That(options.CapabilityTimeout).IsEqualTo(TimeSpan.FromSeconds(30));
            await Assert.That(options.MinimumWorkerCount).IsEqualTo(2);
        }
    }

    [Test]
    public async Task Auto_Role_Uses_Instance_Index()
    {
        var master = new RoleDetector(Microsoft.Extensions.Options.Options.Create(new DistributedOptions
        {
            InstanceIndex = 0,
        }));
        var worker = new RoleDetector(Microsoft.Extensions.Options.Options.Create(new DistributedOptions
        {
            InstanceIndex = 2,
        }));

        using (Assert.Multiple())
        {
            await Assert.That(master.DetectRole()).IsEqualTo(DistributedRole.Master);
            await Assert.That(worker.DetectRole()).IsEqualTo(DistributedRole.Worker);
        }
    }

    [Test]
    public async Task Explicit_Role_Overrides_Instance_Index()
    {
        var master = new RoleDetector(Microsoft.Extensions.Options.Options.Create(new DistributedOptions
        {
            InstanceIndex = 4,
            Role = DistributedRole.Master,
        }));
        var worker = new RoleDetector(Microsoft.Extensions.Options.Options.Create(new DistributedOptions
        {
            InstanceIndex = 0,
            Role = DistributedRole.Worker,
        }));

        using (Assert.Multiple())
        {
            await Assert.That(master.DetectRole()).IsEqualTo(DistributedRole.Master);
            await Assert.That(worker.DetectRole()).IsEqualTo(DistributedRole.Worker);
        }
    }

    [Test]
    public async Task Legacy_Instance_Environment_Variable_Does_Not_Override_Role()
    {
        var previousValue = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");

        try
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "0");
            var detector = new RoleDetector(Microsoft.Extensions.Options.Options.Create(new DistributedOptions
            {
                InstanceIndex = 2,
            }));

            await Assert.That(detector.DetectRole()).IsEqualTo(DistributedRole.Worker);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousValue);
        }
    }

    [Test]
    public async Task Registration_Activates_Distributed_Executor_With_One_Instance()
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options.TotalInstances = 1);
        builder.AddModule<NoOpModule>();

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline.Services.GetRequiredService<IModuleExecutor>())
            .IsTypeOf<DistributedModuleExecutor>();
    }

    [Test]
    public async Task RequireExplicitRunId_Turns_On_The_Requirement_And_Startup_Validation()
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options => options.RunId = "explicit-run");

        builder.RequireExplicitRunId();

        using var provider = builder.Services.BuildServiceProvider();
        using (Assert.Multiple())
        {
            await Assert.That(provider.GetRequiredService<IOptions<DistributedOptions>>().Value.RequireExplicitRunId)
                .IsTrue();
            await Assert.That(provider.GetService<IStartupValidator>()).IsNotNull();
        }
    }

    [Test]
    public async Task DependencyBasedPostConfigure_Selects_Worker_Executor()
    {
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton(new RoleSelection(DistributedRole.Worker));
        builder.Services.AddOptions<DistributedOptions>()
            .PostConfigure<RoleSelection>((options, selection) => options.Role = selection.Role);
        builder.AddDistributedMode(_ => { });
        builder.AddModule<NoOpModule>();

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline.Services.GetRequiredService<IModuleExecutor>())
            .IsTypeOf<WorkerModuleExecutor>();
    }

    [Test]
    public async Task Parameterless_Registration_Binds_Standard_Environment_Variables()
    {
        var previousInstanceIndex = Environment.GetEnvironmentVariable("MODULARPIPELINES_INSTANCE_INDEX");
        var previousTotalInstances = Environment.GetEnvironmentVariable("MODULARPIPELINES_TOTAL_INSTANCES");
        var previousRunId = Environment.GetEnvironmentVariable("MODULARPIPELINES_RUN_ID");
        var previousRole = Environment.GetEnvironmentVariable("MODULARPIPELINES_ROLE");

        try
        {
            Environment.SetEnvironmentVariable("MODULARPIPELINES_INSTANCE_INDEX", "3");
            Environment.SetEnvironmentVariable("MODULARPIPELINES_TOTAL_INSTANCES", "5");
            Environment.SetEnvironmentVariable("MODULARPIPELINES_RUN_ID", "test-run");
            Environment.SetEnvironmentVariable("MODULARPIPELINES_ROLE", "worker");
            var builder = TestPipelineBuilder.Create();

            builder.AddDistributedMode();

            using var provider = builder.Services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptions<DistributedOptions>>().Value;
            using (Assert.Multiple())
            {
                await Assert.That(options.InstanceIndex).IsEqualTo(3);
                await Assert.That(options.TotalInstances).IsEqualTo(5);
                await Assert.That(options.RunId).IsEqualTo("test-run");
                await Assert.That(options.Role).IsEqualTo(DistributedRole.Worker);
                await Assert.That(options.Enabled).IsTrue();
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULARPIPELINES_INSTANCE_INDEX", previousInstanceIndex);
            Environment.SetEnvironmentVariable("MODULARPIPELINES_TOTAL_INSTANCES", previousTotalInstances);
            Environment.SetEnvironmentVariable("MODULARPIPELINES_RUN_ID", previousRunId);
            Environment.SetEnvironmentVariable("MODULARPIPELINES_ROLE", previousRole);
        }
    }

    private sealed class NoOpModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(0);
    }

    private sealed record RoleSelection(DistributedRole Role);
}
