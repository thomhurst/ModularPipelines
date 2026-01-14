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

public class RequireFactoryTests
{
    [Test]
    public async Task Require_That_With_True_Condition_Passes()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement(Require.That(_ => true, "Should not fail"))
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Require_That_With_False_Condition_Fails()
    {
        const string reason = "Custom failure reason";
        var executePipelineDelegate = () => TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement(Require.That(_ => false, reason))
            .ExecutePipelineAsync();

        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
            .And.HasMessageContaining(reason);
    }

    [Test]
    public async Task Require_ThatAsync_With_True_Condition_Passes()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement(Require.ThatAsync(async _ =>
            {
                await Task.Yield();
                return true;
            }, "Should not fail"))
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(DummyModule))!;
        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Require_ThatAsync_With_False_Condition_Fails()
    {
        const string reason = "Async failure reason";
        var executePipelineDelegate = () => TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement(Require.ThatAsync(async _ =>
            {
                await Task.Yield();
                return false;
            }, reason))
            .ExecutePipelineAsync();

        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
            .And.HasMessageContaining(reason);
    }

    [Test]
    public async Task Require_EnvironmentVariable_When_Set_Passes()
    {
        const string varName = "TEST_REQUIREMENT_VAR";
        Environment.SetEnvironmentVariable(varName, "some-value");
        try
        {
            var host = await TestPipelineHostBuilder.Create()
                .AddModule<DummyModule>()
                .AddRequirement(Require.EnvironmentVariable(varName))
                .BuildHostAsync();

            await host.ExecutePipelineAsync();

            var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
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
        const string varName = "UNLIKELY_TO_EXIST_VAR_12345";
        Environment.SetEnvironmentVariable(varName, null);

        var executePipelineDelegate = () => TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement(Require.EnvironmentVariable(varName))
            .ExecutePipelineAsync();

        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
            .And.HasMessageContaining(varName);
    }

    [Test]
    public async Task Require_EnvironmentVariable_With_Custom_Reason()
    {
        const string varName = "UNLIKELY_TO_EXIST_VAR_67890";
        const string customReason = "My custom message about the var";
        Environment.SetEnvironmentVariable(varName, null);

        var executePipelineDelegate = () => TestPipelineHostBuilder.Create()
            .AddModule<DummyModule>()
            .AddRequirement(Require.EnvironmentVariable(varName, customReason))
            .ExecutePipelineAsync();

        await Assert.That(executePipelineDelegate)
            .Throws<FailedRequirementsException>()
            .And.HasMessageContaining(customReason);
    }

    [Test]
    public async Task DelegateRequirement_Respects_Order()
    {
        var requirement = Require.That(_ => true, "test", order: 5);
        await Assert.That(requirement.Order).IsEqualTo(5);
    }

    private class DummyModule : Module<bool>
    {
        public override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return true;
        }
    }
}
