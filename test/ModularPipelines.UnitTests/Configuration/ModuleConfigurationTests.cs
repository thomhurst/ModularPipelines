using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Models;
using Moq;
using Polly;

namespace ModularPipelines.UnitTests.Configuration;

public class ModuleConfigurationTests
{
    #region Default Tests

    [Test]
    public async Task Default_SkipCondition_IsNull()
    {
        var config = ModuleConfiguration.Default;

        await Assert.That(config.SkipCondition).IsNull();
    }

    [Test]
    public async Task Default_Timeout_IsNull()
    {
        var config = ModuleConfiguration.Default;

        await Assert.That(config.Timeout).IsNull();
    }

    [Test]
    public async Task Default_RetryPolicyFactory_IsNull()
    {
        var config = ModuleConfiguration.Default;

        await Assert.That(config.RetryPolicyFactory).IsNull();
    }

    [Test]
    public async Task Default_IgnoreFailuresCondition_IsNull()
    {
        var config = ModuleConfiguration.Default;

        await Assert.That(config.IgnoreFailuresCondition).IsNull();
    }

    [Test]
    public async Task Default_AlwaysRun_IsFalse()
    {
        var config = ModuleConfiguration.Default;

        await Assert.That(config.AlwaysRun).IsFalse();
    }

    [Test]
    public async Task Default_OnBeforeExecute_IsNull()
    {
        var config = ModuleConfiguration.Default;

        await Assert.That(config.OnBeforeExecute).IsNull();
    }

    [Test]
    public async Task Default_OnAfterExecute_IsNull()
    {
        var config = ModuleConfiguration.Default;

        await Assert.That(config.OnAfterExecute).IsNull();
    }

    [Test]
    public async Task Create_ReturnsBuilder()
    {
        var builder = ModuleConfiguration.Create();

        await Assert.That(builder).IsNotNull();
        await Assert.That(builder).IsTypeOf<ModuleConfigurationBuilder>();
    }

    #endregion

    #region WithSkipWhen Tests

    [Test]
    public async Task WithSkipWhen_SyncBool_SetsSkipCondition()
    {
        var config = ModuleConfiguration.Create()
            .WithSkipWhen(() => true)
            .Build();

        await Assert.That(config.SkipCondition).IsNotNull();

        var context = Mock.Of<IPipelineContext>();
        var decision = await config.SkipCondition!(context);

        await Assert.That(decision.ShouldSkip).IsTrue();
    }

    [Test]
    public async Task WithSkipWhen_SyncBoolFalse_ReturnsDoNotSkip()
    {
        var config = ModuleConfiguration.Create()
            .WithSkipWhen(() => false)
            .Build();

        var context = Mock.Of<IPipelineContext>();
        var decision = await config.SkipCondition!(context);

        await Assert.That(decision.ShouldSkip).IsFalse();
    }

    [Test]
    public async Task WithSkipWhen_AsyncBool_SetsSkipCondition()
    {
        var config = ModuleConfiguration.Create()
            .WithSkipWhen(async () =>
            {
                await Task.Delay(1).ConfigureAwait(false);
                return true;
            })
            .Build();

        await Assert.That(config.SkipCondition).IsNotNull();

        var context = Mock.Of<IPipelineContext>();
        var decision = await config.SkipCondition!(context);

        await Assert.That(decision.ShouldSkip).IsTrue();
    }

    [Test]
    public async Task WithSkipWhen_SyncSkipDecision_SetsSkipCondition()
    {
        var expectedDecision = SkipDecision.Skip("Test reason");

        var config = ModuleConfiguration.Create()
            .WithSkipWhen(() => expectedDecision)
            .Build();

        var context = Mock.Of<IPipelineContext>();
        var decision = await config.SkipCondition!(context);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsTrue();
            await Assert.That(decision.Reason).IsEqualTo("Test reason");
        }
    }

    [Test]
    public async Task WithSkipWhen_AsyncSkipDecision_SetsSkipCondition()
    {
        var expectedDecision = SkipDecision.Skip("Async reason");

        var config = ModuleConfiguration.Create()
            .WithSkipWhen(async () =>
            {
                await Task.Delay(1).ConfigureAwait(false);
                return expectedDecision;
            })
            .Build();

        var context = Mock.Of<IPipelineContext>();
        var decision = await config.SkipCondition!(context);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsTrue();
            await Assert.That(decision.Reason).IsEqualTo("Async reason");
        }
    }

    [Test]
    public async Task WithSkipWhen_WithContext_SyncBool_SetsSkipCondition()
    {
        var config = ModuleConfiguration.Create()
            .WithSkipWhen(ctx => ctx != null)
            .Build();

        var context = Mock.Of<IPipelineContext>();
        var decision = await config.SkipCondition!(context);

        await Assert.That(decision.ShouldSkip).IsTrue();
    }

    [Test]
    public async Task WithSkipWhen_WithContext_AsyncBool_SetsSkipCondition()
    {
        var config = ModuleConfiguration.Create()
            .WithSkipWhen(async ctx =>
            {
                await Task.Delay(1).ConfigureAwait(false);
                return ctx != null;
            })
            .Build();

        var context = Mock.Of<IPipelineContext>();
        var decision = await config.SkipCondition!(context);

        await Assert.That(decision.ShouldSkip).IsTrue();
    }

    [Test]
    public async Task WithSkipWhen_WithContext_SyncSkipDecision_SetsSkipCondition()
    {
        var config = ModuleConfiguration.Create()
            .WithSkipWhen(ctx => ctx != null ? SkipDecision.Skip("Has context") : SkipDecision.DoNotSkip)
            .Build();

        var context = Mock.Of<IPipelineContext>();
        var decision = await config.SkipCondition!(context);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsTrue();
            await Assert.That(decision.Reason).IsEqualTo("Has context");
        }
    }

    [Test]
    public async Task WithSkipWhen_WithContext_AsyncSkipDecision_SetsSkipCondition()
    {
        var config = ModuleConfiguration.Create()
            .WithSkipWhen(async ctx =>
            {
                await Task.Delay(1).ConfigureAwait(false);
                return ctx != null ? SkipDecision.Skip("Async context") : SkipDecision.DoNotSkip;
            })
            .Build();

        var context = Mock.Of<IPipelineContext>();
        var decision = await config.SkipCondition!(context);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsTrue();
            await Assert.That(decision.Reason).IsEqualTo("Async context");
        }
    }

    #endregion

    #region WithTimeout Tests

    [Test]
    public async Task WithTimeout_SetsTimeout()
    {
        var timeout = TimeSpan.FromMinutes(5);

        var config = ModuleConfiguration.Create()
            .WithTimeout(timeout)
            .Build();

        await Assert.That(config.Timeout).IsEqualTo(timeout);
    }

    #endregion

    #region WithRetryPolicy Tests

    [Test]
    public async Task WithRetryPolicy_Direct_SetsRetryPolicyFactory()
    {
        var policy = Policy.NoOpAsync();

        var config = ModuleConfiguration.Create()
            .WithRetryPolicy(policy)
            .Build();

        await Assert.That(config.RetryPolicyFactory).IsNotNull();

        var context = Mock.Of<IPipelineContext>();
        var result = config.RetryPolicyFactory!(context);

        await Assert.That(result).IsEqualTo(policy);
    }

    [Test]
    public async Task WithRetryPolicy_Factory_SetsRetryPolicyFactory()
    {
        var policy = Policy.NoOpAsync();

        var config = ModuleConfiguration.Create()
            .WithRetryPolicy(ctx => policy)
            .Build();

        await Assert.That(config.RetryPolicyFactory).IsNotNull();

        var context = Mock.Of<IPipelineContext>();
        var result = config.RetryPolicyFactory!(context);

        await Assert.That(result).IsEqualTo(policy);
    }

    [Test]
    public async Task WithRetryCount_SetsRetryPolicyFactory()
    {
        var config = ModuleConfiguration.Create()
            .WithRetryCount(3)
            .Build();

        await Assert.That(config.RetryPolicyFactory).IsNotNull();
    }

    #endregion

    #region WithIgnoreFailures Tests

    [Test]
    public async Task WithIgnoreFailures_Always_SetsIgnoreFailuresCondition()
    {
        var config = ModuleConfiguration.Create()
            .WithIgnoreFailures()
            .Build();

        await Assert.That(config.IgnoreFailuresCondition).IsNotNull();

        var context = Mock.Of<IPipelineContext>();
        var result = await config.IgnoreFailuresCondition!(context, new Exception("test"));

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task WithIgnoreFailuresWhen_SyncCondition_SetsIgnoreFailuresCondition()
    {
        var config = ModuleConfiguration.Create()
            .WithIgnoreFailuresWhen((ctx, ex) => ex.Message == "ignore")
            .Build();

        await Assert.That(config.IgnoreFailuresCondition).IsNotNull();

        var context = Mock.Of<IPipelineContext>();

        var shouldIgnore = await config.IgnoreFailuresCondition!(context, new Exception("ignore"));
        await Assert.That(shouldIgnore).IsTrue();

        var shouldNotIgnore = await config.IgnoreFailuresCondition!(context, new Exception("fail"));
        await Assert.That(shouldNotIgnore).IsFalse();
    }

    [Test]
    public async Task WithIgnoreFailuresWhen_AsyncCondition_SetsIgnoreFailuresCondition()
    {
        var config = ModuleConfiguration.Create()
            .WithIgnoreFailuresWhen(async (ctx, ex) =>
            {
                await Task.Delay(1).ConfigureAwait(false);
                return ex.Message == "ignore";
            })
            .Build();

        var context = Mock.Of<IPipelineContext>();

        var shouldIgnore = await config.IgnoreFailuresCondition!(context, new Exception("ignore"));
        await Assert.That(shouldIgnore).IsTrue();

        var shouldNotIgnore = await config.IgnoreFailuresCondition!(context, new Exception("fail"));
        await Assert.That(shouldNotIgnore).IsFalse();
    }

    #endregion

    #region WithAlwaysRun Tests

    [Test]
    public async Task WithAlwaysRun_SetsAlwaysRun()
    {
        var config = ModuleConfiguration.Create()
            .WithAlwaysRun()
            .Build();

        await Assert.That(config.AlwaysRun).IsTrue();
    }

    #endregion

    #region WithBeforeExecute Tests

    [Test]
    public async Task WithBeforeExecute_SetsOnBeforeExecute()
    {
        var executed = false;

        var config = ModuleConfiguration.Create()
            .WithBeforeExecute(async ctx =>
            {
                await Task.Delay(1).ConfigureAwait(false);
                executed = true;
            })
            .Build();

        await Assert.That(config.OnBeforeExecute).IsNotNull();

        var context = Mock.Of<IPipelineContext>();
        await config.OnBeforeExecute!(context);

        await Assert.That(executed).IsTrue();
    }

    #endregion

    #region WithAfterExecute Tests

    [Test]
    public async Task WithAfterExecute_SetsOnAfterExecute()
    {
        var executed = false;

        var config = ModuleConfiguration.Create()
            .WithAfterExecute(async ctx =>
            {
                await Task.Delay(1).ConfigureAwait(false);
                executed = true;
            })
            .Build();

        await Assert.That(config.OnAfterExecute).IsNotNull();

        var context = Mock.Of<IPipelineContext>();
        await config.OnAfterExecute!(context);

        await Assert.That(executed).IsTrue();
    }

    #endregion

    #region Fluent Chaining Tests

    [Test]
    public async Task Builder_FluentChaining_AllMethodsChain()
    {
        var policy = Policy.NoOpAsync();

        var config = ModuleConfiguration.Create()
            .WithSkipWhen(() => false)
            .WithTimeout(TimeSpan.FromMinutes(1))
            .WithRetryPolicy(policy)
            .WithIgnoreFailures()
            .WithAlwaysRun()
            .WithBeforeExecute(ctx => Task.CompletedTask)
            .WithAfterExecute(ctx => Task.CompletedTask)
            .Build();

        using (Assert.Multiple())
        {
            await Assert.That(config.SkipCondition).IsNotNull();
            await Assert.That(config.Timeout).IsEqualTo(TimeSpan.FromMinutes(1));
            await Assert.That(config.RetryPolicyFactory).IsNotNull();
            await Assert.That(config.IgnoreFailuresCondition).IsNotNull();
            await Assert.That(config.AlwaysRun).IsTrue();
            await Assert.That(config.OnBeforeExecute).IsNotNull();
            await Assert.That(config.OnAfterExecute).IsNotNull();
        }
    }

    #endregion
}
