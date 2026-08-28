using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

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
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Successful);
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
            .Throws<FailedRequirementsException>()
            .And.HasMessageContaining(reason);
    }

    [Test]
    public async Task Require_ThatAsync_With_True_Condition_Passes()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement(Require.ThatAsync(async _ =>
            {
                await Task.Yield();
                return true;
            }, "Should not fail"))
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Require_ThatAsync_With_False_Condition_Fails()
    {
        const string reason = "Async failure reason";
        var executePipelineDelegate = async () =>
        {
            await TestPipelineBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement(Require.ThatAsync(async _ =>
                {
                    await Task.Yield();
                    return false;
                }, reason))
                .RunAsync();
        };

        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
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
            await Assert.That(result.ModuleStatus).IsEqualTo(Status.Successful);
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
            .Throws<FailedRequirementsException>()
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
            .Throws<FailedRequirementsException>()
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
            using var builder = Pipeline.CreateBuilder(new PipelineBuilderOptions
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
                await Assert.That(result.ModuleStatus)
                    .IsEqualTo(Status.Successful);
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

    private class DummyModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }
}
