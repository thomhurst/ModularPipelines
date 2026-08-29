using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Requirements;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class RequireFactoryTests
{
    [Test]
    public async Task Require_That_With_True_Condition_Passes()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement(Require.That(_ => true, "Should not fail"))
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Require_That_With_False_Condition_Fails()
    {
        const string reason = "Custom failure reason";
        var executePipelineDelegate = async () =>
        {
            await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement(Require.That(_ => false, reason))
                .RunAsync();
        };

        await Assert.That(executePipelineDelegate)
            .Throws<RequirementNotMetException>()
            .And.HasMessageContaining(reason);
    }

    [Test]
    public async Task Require_ThatAsync_With_True_Condition_Passes()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement(Require.ThatAsync(async (_, _) =>
            {
                await Task.Yield();
                return true;
            }, "Should not fail"))
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Require_ThatAsync_With_False_Condition_Fails()
    {
        const string reason = "Async failure reason";
        var executePipelineDelegate = async () =>
        {
            await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement(Require.ThatAsync(async (_, _) =>
                {
                    await Task.Yield();
                    return false;
                }, reason))
                .RunAsync();
        };

        await Assert.That(executePipelineDelegate)
            .Throws<RequirementNotMetException>()
            .And.HasMessageContaining(reason);
    }

    [Test]
    public async Task Require_EnvironmentVariable_When_Set_Passes()
    {
        var varName = $"TEST_REQUIREMENT_VAR_{Guid.NewGuid():N}";
        Environment.SetEnvironmentVariable(varName, "some-value");
        try
        {
            var host = await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement(Require.EnvironmentVariable(varName))
                .BuildAsync();

            await host.RunAsync();

            var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
            var result = resultRegistry.GetResult(typeof(DummyModule))!;
            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
        }
        finally
        {
            Environment.SetEnvironmentVariable(varName, null);
        }
    }

    [Test]
    public async Task Require_EnvironmentVariable_When_Not_Set_Fails()
    {
        var varName = $"UNLIKELY_TO_EXIST_VAR_{Guid.NewGuid():N}";

        var executePipelineDelegate = async () =>
        {
            await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement(Require.EnvironmentVariable(varName))
                .RunAsync();
        };

        await Assert.That(executePipelineDelegate)
            .Throws<RequirementNotMetException>()
            .And.HasMessageContaining(varName);
    }

    [Test]
    public async Task Require_EnvironmentVariable_With_Custom_Reason()
    {
        var varName = $"UNLIKELY_TO_EXIST_VAR_{Guid.NewGuid():N}";
        const string customReason = "My custom message about the var";

        var executePipelineDelegate = async () =>
        {
            await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement(Require.EnvironmentVariable(varName, customReason))
                .RunAsync();
        };

        await Assert.That(executePipelineDelegate)
            .Throws<RequirementNotMetException>()
            .And.HasMessageContaining(customReason);
    }

    [Test]
    public async Task FileSystemRequirementsResolveFromPipelineWorkingDirectory()
    {
        var processDirectory = Environment.CurrentDirectory;
        var workingDirectory = Directory.CreateTempSubdirectory("pipeline-requirements-");
        await File.WriteAllTextAsync(
            Path.Combine(workingDirectory.FullName, "appsettings.json"),
            "{}");
        Directory.CreateDirectory(Path.Combine(workingDirectory.FullName, "artifacts"));

        try
        {
            var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
            {
                WorkingDirectory = workingDirectory.FullName,
            });
            builder.AddModule<DummyModule>();
            builder.AddRequirement(Require.FileExists("./appsettings.json"));
            builder.AddRequirement(Require.DirectoryExists("./artifacts"));

            var summary = await builder.RunAsync();
            var result = await summary.Modules.OfType<DummyModule>().Single();

            using (Assert.Multiple())
            {
                await Assert.That(result.Status)
                    .IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(Environment.CurrentDirectory).IsEqualTo(processDirectory);
            }
        }
        finally
        {
            workingDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task DelegateRequirement_Respects_Order()
    {
        var requirement = Require.That(_ => true, "test", order: 5);
        await Assert.That(requirement.Order).IsEqualTo(5);
    }

    [Test]
    public async Task Platform_Factories_Match_Pipeline_Operating_System()
    {
        var environment = new Mock<IEnvironmentContext>();
        var context = new Mock<IPipelineContext>();
        context.SetupGet(static pipelineContext => pipelineContext.Environment)
            .Returns(environment.Object);

        var cases = new (OSPlatform Platform, Func<IPipelineRequirement> Create)[]
        {
            (OSPlatform.Windows, static () => Require.Windows()),
            (OSPlatform.Linux, static () => Require.Linux()),
            (OSPlatform.OSX, static () => Require.MacOS()),
        };

        foreach (var (platform, create) in cases)
        {
            environment.SetupGet(static environmentContext => environmentContext.OperatingSystem)
                .Returns(platform);

            var decision = await create().EvaluateAsync(context.Object, CancellationToken.None);

            await Assert.That(decision.IsSatisfied).IsTrue();
        }
    }

    [Test]
    public async Task WindowsAdmin_Passes_On_Non_Windows_Platforms()
    {
        var environment = new Mock<IEnvironmentContext>();
        environment.SetupGet(static environmentContext => environmentContext.OperatingSystem)
            .Returns(OSPlatform.Linux);
        var context = new Mock<IPipelineContext>();
        context.SetupGet(static pipelineContext => pipelineContext.Environment)
            .Returns(environment.Object);

        var decision = await Require.WindowsAdmin()
            .EvaluateAsync(context.Object, CancellationToken.None);

        await Assert.That(decision.IsSatisfied).IsTrue();
    }

    private class DummyModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }
}
