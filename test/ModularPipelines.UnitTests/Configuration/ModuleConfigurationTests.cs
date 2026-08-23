using Kevlar;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Configuration;

public class ModuleConfigurationTests
{
    private sealed class DependencyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }

    #region Default Tests

    [Test]
    public async Task Default_SkipCondition_IsNull()
    {
        var config = ModuleConfiguration.Default;

        await Assert.That((object?) config.SkipCondition).IsNull();
    }

    [Test]
    public async Task Default_Timeout_IsNull()
    {
        var config = ModuleConfiguration.Default;

        await Assert.That(config.Timeout).IsNull();
    }

    [Test]
    public async Task Default_RetryConfiguration_IsNull()
    {
        var config = ModuleConfiguration.Default;

        using (Assert.Multiple())
        {
            await Assert.That(config.RetryConfiguration).IsNull();
            await Assert.That(config.ResilienceShieldFactory).IsNull();
        }
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
    public async Task Create_ReturnsBuilder()
    {
        var builder = ModuleConfiguration.Create();

        await Assert.That(builder).IsNotNull();
        await Assert.That(builder).IsTypeOf<ModuleConfigurationBuilder>();
    }

    #endregion

    #region WithSkipWhen Tests

    [Test]
    public async Task WithSkipWhen_ExposesOnlyComposableOverloads()
    {
        var parameterTypes = typeof(ModuleConfigurationBuilder)
            .GetMethods()
            .Where(method => method.Name == nameof(ModuleConfigurationBuilder.WithSkipWhen))
            .Select(method => method.GetParameters().Single().ParameterType)
            .ToArray();

        await Assert.That(parameterTypes).IsEquivalentTo(
        [
            typeof(Func<IModuleContext, SkipDecision>),
            typeof(Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>>),
        ]);
    }

    [Test]
    public async Task WithSkipWhenAll_ExposesSyncAndAsyncGroups()
    {
        var parameterTypes = typeof(ModuleConfigurationBuilder)
            .GetMethods()
            .Where(method => method.Name == nameof(ModuleConfigurationBuilder.WithSkipWhenAll))
            .Select(method => method.GetParameters().Single().ParameterType)
            .ToArray();

        await Assert.That(parameterTypes).IsEquivalentTo(
        [
            typeof(Func<IModuleContext, SkipDecision>[]),
            typeof(Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>>[]),
        ]);
    }

    [Test]
    public async Task WithSkipWhen_RepeatedCalls_OrComposeAndShortCircuit()
    {
        var evaluatedConditions = new List<string>();
        var config = ModuleConfiguration.Create()
            .WithSkipWhen(_ =>
            {
                evaluatedConditions.Add("first");
                return SkipDecision.Skip("First reason");
            })
            .WithSkipWhen(_ =>
            {
                evaluatedConditions.Add("second");
                return SkipDecision.DoNotSkip;
            })
            .Build();

        var decision = await config.SkipCondition!(Mock.Of<IModuleContext>(), CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsTrue();
            await Assert.That(decision.Reason).IsEqualTo("First reason");
            await Assert.That(evaluatedConditions).IsEquivalentTo(["first"]);
        }
    }

    [Test]
    public async Task WithSkipWhen_RepeatedCalls_SkipWhenLaterConditionMatches()
    {
        var config = ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.DoNotSkip)
            .WithSkipWhen(_ => SkipDecision.Skip("Second reason"))
            .Build();

        var decision = await config.SkipCondition!(Mock.Of<IModuleContext>(), CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsTrue();
            await Assert.That(decision.Reason).IsEqualTo("Second reason");
        }
    }

    [Test]
    public async Task WithSkipWhen_SyncCondition_ReceivesContext()
    {
        var context = Mock.Of<IModuleContext>();
        var expectedDecision = SkipDecision.Skip("Test reason");

        var config = ModuleConfiguration.Create()
            .WithSkipWhen(receivedContext =>
                ReferenceEquals(receivedContext, context) ? expectedDecision : SkipDecision.DoNotSkip)
            .Build();

        var decision = await config.SkipCondition!(context, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsTrue();
            await Assert.That(decision.Reason).IsEqualTo("Test reason");
        }
    }

    [Test]
    public async Task WithSkipWhen_AsyncCondition_SetsSkipCondition()
    {
        var expectedDecision = SkipDecision.Skip("Async reason");

        var config = ModuleConfiguration.Create()
            .WithSkipWhen(async (_, _) =>
            {
                await Task.Delay(1).ConfigureAwait(false);
                return expectedDecision;
            })
            .Build();

        var context = Mock.Of<IModuleContext>();
        var decision = await config.SkipCondition!(context, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsTrue();
            await Assert.That(decision.Reason).IsEqualTo("Async reason");
        }
    }

    [Test]
    public async Task WithSkipWhen_AsyncCondition_ReceivesCancellationToken()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        CancellationToken? receivedToken = null;
        var config = ModuleConfiguration.Create()
            .WithSkipWhen((_, cancellationToken) =>
            {
                receivedToken = cancellationToken;
                return ValueTask.FromResult(SkipDecision.Skip("Testing"));
            })
            .Build();

        await config.SkipCondition!(Mock.Of<IModuleContext>(), cancellationTokenSource.Token);

        await Assert.That(receivedToken).IsEqualTo(cancellationTokenSource.Token);
    }

    [Test]
    public async Task WithSkipWhenAll_AllConditionsSkip_CombinesReasons()
    {
        var config = ModuleConfiguration.Create()
            .WithSkipWhenAll(
                _ => SkipDecision.Skip("First reason"),
                _ => SkipDecision.Skip("Second reason"))
            .Build();

        var decision = await config.SkipCondition!(Mock.Of<IModuleContext>(), CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsTrue();
            await Assert.That(decision.Reason).IsEqualTo("First reason; Second reason");
        }
    }

    [Test]
    public async Task WithSkipWhenAll_StopsWhenConditionDoesNotSkip()
    {
        var evaluatedConditions = new List<string>();
        var config = ModuleConfiguration.Create()
            .WithSkipWhenAll(
                _ =>
                {
                    evaluatedConditions.Add("first");
                    return SkipDecision.DoNotSkip;
                },
                _ =>
                {
                    evaluatedConditions.Add("second");
                    return SkipDecision.Skip("Should not be evaluated");
                })
            .Build();

        var decision = await config.SkipCondition!(Mock.Of<IModuleContext>(), CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsFalse();
            await Assert.That(evaluatedConditions).IsEquivalentTo(["first"]);
        }
    }

    [Test]
    public async Task WithSkipWhenAll_SnapshotsAsyncConditionGroups()
    {
        Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>>[] conditions =
        [
            (_, _) => ValueTask.FromResult(SkipDecision.Skip("Original reason")),
        ];
        var config = ModuleConfiguration.Create()
            .WithSkipWhenAll(conditions)
            .Build();

        conditions[0] = (_, _) => ValueTask.FromResult(SkipDecision.DoNotSkip);

        var decision = await config.SkipCondition!(
            Mock.Of<IModuleContext>(),
            CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(decision.ShouldSkip).IsTrue();
            await Assert.That(decision.Reason).IsEqualTo("Original reason");
        }
    }

    [Test]
    public void WithSkipWhenAll_RejectsEmptyGroups()
    {
        var builder = ModuleConfiguration.Create();

        Assert.Throws<ArgumentException>(() =>
            builder.WithSkipWhenAll(Array.Empty<Func<IModuleContext, SkipDecision>>()));
    }

    [Test]
    public async Task Build_SnapshotsSkipConditions()
    {
        var builder = ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.DoNotSkip);
        var firstConfig = builder.Build();

        builder.WithSkipWhen(_ => SkipDecision.Skip("Second reason"));
        var secondConfig = builder.Build();

        var context = Mock.Of<IModuleContext>();
        var firstDecision = await firstConfig.SkipCondition!(context, CancellationToken.None);
        var secondDecision = await secondConfig.SkipCondition!(context, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(firstDecision.ShouldSkip).IsFalse();
            await Assert.That(secondDecision.ShouldSkip).IsTrue();
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

    #region WithRetry Tests

    [Test]
    public async Task WithRetry_UsesDefaultBaseDelay()
    {
        var config = ModuleConfiguration.Create()
            .WithRetry(3)
            .Build();

        using (Assert.Multiple())
        {
            await Assert.That(config.RetryConfiguration).IsNotNull();
            await Assert.That(config.RetryConfiguration!.Count).IsEqualTo(3);
            await Assert.That(config.RetryConfiguration.BaseDelay).IsEqualTo(TimeSpan.FromMilliseconds(100));
            await Assert.That(config.RetryConfiguration.ShouldRetry).IsNull();
            await Assert.That(config.ResilienceShieldFactory).IsNull();
        }
    }

    [Test]
    public async Task WithRetry_StoresBaseDelayAndExceptionFilter()
    {
        var baseDelay = TimeSpan.FromSeconds(2);
        Func<Exception, bool> shouldRetry = exception => exception is TimeoutException;

        var config = ModuleConfiguration.Create()
            .WithRetry(5, baseDelay, shouldRetry)
            .Build();

        using (Assert.Multiple())
        {
            await Assert.That(config.RetryConfiguration).IsNotNull();
            await Assert.That(config.RetryConfiguration!.Count).IsEqualTo(5);
            await Assert.That(config.RetryConfiguration.BaseDelay).IsEqualTo(baseDelay);
            await Assert.That(config.RetryConfiguration.ShouldRetry).IsSameReferenceAs(shouldRetry);
        }
    }

    [Test]
    public async Task WithRetry_RejectsNegativeValues()
    {
        var countException = Assert.Throws<ArgumentOutOfRangeException>(
            () => ModuleConfiguration.Create().WithRetry(-1));
        var delayException = Assert.Throws<ArgumentOutOfRangeException>(
            () => ModuleConfiguration.Create().WithRetry(1, TimeSpan.FromMilliseconds(-1)));

        using (Assert.Multiple())
        {
            await Assert.That(countException.ParamName).IsEqualTo("count");
            await Assert.That(delayException.ParamName).IsEqualTo("baseDelay");
        }
    }

    [Test]
    public async Task Advanced_WithShield_Direct_SetsResilienceShieldFactory()
    {
        var shield = Shield.Retry(0);

        var config = ModuleConfiguration.Create()
            .Advanced
            .WithShield(shield)
            .Build();

        await Assert.That(config.ResilienceShieldFactory).IsNotNull();

        var context = Mock.Of<IModuleContext>();
        var result = config.ResilienceShieldFactory!(context);

        await Assert.That(result).IsEqualTo(shield);
    }

    [Test]
    public async Task Advanced_WithShield_Factory_SetsResilienceShieldFactory()
    {
        var shield = Shield.Retry(0);

        var config = ModuleConfiguration.Create()
            .Advanced
            .WithShield(_ => shield)
            .Build();

        await Assert.That(config.ResilienceShieldFactory).IsNotNull();

        var context = Mock.Of<IModuleContext>();
        var result = config.ResilienceShieldFactory!(context);

        await Assert.That(result).IsEqualTo(shield);
    }

    [Test]
    public async Task StandardConfigurationSurface_DoesNotExposeKevlarTypes()
    {
        var publicSurfaceTypes = typeof(ModuleConfigurationBuilder)
            .GetMethods()
            .SelectMany(method => method.GetParameters()
                .Select(parameter => parameter.ParameterType)
                .Append(method.ReturnType))
            .Concat(typeof(ModuleConfiguration)
                .GetProperties()
                .Select(property => property.PropertyType));

        await Assert.That(publicSurfaceTypes.Any(ContainsKevlarType)).IsFalse();
    }

    [Test]
    [Arguments(0, 100)]
    [Arguments(1, 200)]
    public async Task RetryDelayCalculator_AddsBoundedJitter(double jitterFactor, int expectedMilliseconds)
    {
        var delay = ModuleRetryShieldFactory.CalculateDelay(
            retryAttempt: 2,
            baseDelay: TimeSpan.FromMilliseconds(100),
            jitterFactor);

        await Assert.That(delay).IsEqualTo(TimeSpan.FromMilliseconds(expectedMilliseconds));
    }

    [Test]
    public async Task RetryDelayCalculator_ZeroDelay_RemainsZeroAtMaximumAttempt()
    {
        var delay = ModuleRetryShieldFactory.CalculateDelay(
            retryAttempt: int.MaxValue,
            baseDelay: TimeSpan.Zero,
            jitterFactor: 0.5);

        await Assert.That(delay).IsEqualTo(TimeSpan.Zero);
    }

    [Test]
    public async Task RetryDelayCalculator_MaximumDelay_RemainsInTimeSpanRange()
    {
        var delay = ModuleRetryShieldFactory.CalculateDelay(
            retryAttempt: 2,
            baseDelay: TimeSpan.MaxValue,
            jitterFactor: 1);

        await Assert.That(delay).IsEqualTo(TimeSpan.MaxValue);
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

        var context = Mock.Of<IModuleContext>();
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

        var context = Mock.Of<IModuleContext>();

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

        var context = Mock.Of<IModuleContext>();

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

    #region Fluent Chaining Tests

    [Test]
    public async Task Builder_Configures_Scheduling_Metadata_And_Dependencies()
    {
        var config = ModuleConfiguration.Create()
            .WithNotInParallel("database")
            .WithPriority(ModulePriority.Critical)
            .WithExecutionHint(ExecutionType.IoIntensive)
            .WithTags("deploy", "production")
            .WithCategory("release")
            .DependsOn<DependencyModule>()
            .Build();

        using (Assert.Multiple())
        {
            await Assert.That(config.ParallelConstraintKeys).IsEquivalentTo(new[] { "database" });
            await Assert.That(config.Priority).IsEqualTo(ModulePriority.Critical);
            await Assert.That(config.ExecutionType).IsEqualTo(ExecutionType.IoIntensive);
            await Assert.That(config.Tags).IsEquivalentTo(new[] { "deploy", "production" });
            await Assert.That(config.Category).IsEqualTo("release");
            await Assert.That(config.Dependencies.Select(dependency => dependency.ModuleType))
                .IsEquivalentTo(new[] { typeof(DependencyModule) });
        }
    }

    [Test]
    public async Task Builder_FluentChaining_AllMethodsChain()
    {
        var config = ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.DoNotSkip)
            .WithTimeout(TimeSpan.FromMinutes(1))
            .WithRetry(3)
            .WithIgnoreFailures()
            .WithAlwaysRun()
            .WithNotInParallel("shared")
            .WithPriority(ModulePriority.High)
            .WithExecutionHint(ExecutionType.CpuIntensive)
            .WithTags("build")
            .WithCategory("ci")
            .DependsOnOptional<DependencyModule>()
            .Build();

        using (Assert.Multiple())
        {
            await Assert.That((object?) config.SkipCondition).IsNotNull();
            await Assert.That(config.Timeout).IsEqualTo(TimeSpan.FromMinutes(1));
            await Assert.That(config.RetryConfiguration).IsNotNull();
            await Assert.That(config.IgnoreFailuresCondition).IsNotNull();
            await Assert.That(config.AlwaysRun).IsTrue();
            await Assert.That(config.ParallelConstraintKeys).IsEquivalentTo(new[] { "shared" });
            await Assert.That(config.Priority).IsEqualTo(ModulePriority.High);
            await Assert.That(config.ExecutionType).IsEqualTo(ExecutionType.CpuIntensive);
            await Assert.That(config.Tags).Contains("build");
            await Assert.That(config.Category).IsEqualTo("ci");
            await Assert.That(config.Dependencies).HasSingleItem();
        }
    }

    #endregion

    private static bool ContainsKevlarType(Type type) =>
        type.Namespace?.StartsWith("Kevlar", StringComparison.Ordinal) == true
        || type.GetGenericArguments().Any(ContainsKevlarType);
}
