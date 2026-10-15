using Kevlar;
using Moq;

namespace ModularPipelines.UnitTests.Configuration;

public class ModuleConfigurationBuilderValidationTests
{
    [Test]
    [Arguments(0)]
    [Arguments(-2)]
    [Arguments(-5000)]
    public async Task WithTimeout_Rejects_Zero_And_Negative_Timeouts(int milliseconds)
    {
        await Assert.That(() => new ModuleConfigurationBuilder().WithTimeout(TimeSpan.FromMilliseconds(milliseconds)))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task WithTimeout_Accepts_Positive_Timeout()
    {
        var configuration = new ModuleConfigurationBuilder().WithTimeout(TimeSpan.FromSeconds(5)).Build();

        await Assert.That(configuration.Timeout).IsEqualTo(TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task WithTimeout_Infinite_Disables_The_Timeout()
    {
        var configuration = new ModuleConfigurationBuilder().WithTimeout(Timeout.InfiniteTimeSpan).Build();

        // Timeout.InfiniteTimeSpan disables the timeout and overrides the pipeline default.
        await Assert.That(configuration.Timeout).IsEqualTo(Timeout.InfiniteTimeSpan);
    }

    [Test]
    [Arguments(false, 0)]
    [Arguments(false, 3)]
    [Arguments(true, 0)]
    [Arguments(true, 3)]
    public async Task WithRetry_Rejects_An_Existing_Shield_Without_Changing_Configuration(bool useFactory, int retryCount)
    {
        var shield = Shield.Retry(0);
        var builder = new ModuleConfigurationBuilder();
        if (useFactory)
        {
            builder.WithShield(_ => shield);
        }
        else
        {
            builder.WithShield(shield);
        }

        var originalFactory = builder.Build().ResilienceShieldFactory;

        var exception = Assert.Throws<InvalidOperationException>(() => builder.WithRetry(retryCount));
        var configuration = builder.Build();

        using (Assert.Multiple())
        {
            await Assert.That(exception.Message).Contains("WithRetry cannot be combined with WithShield");
            await Assert.That(configuration.ResilienceShieldFactory).IsSameReferenceAs(originalFactory);
            await Assert.That(configuration.ResilienceShieldFactory!(Mock.Of<IModuleContext>())).IsSameReferenceAs(shield);
            await Assert.That(configuration.RetryConfiguration).IsNull();
        }
    }

    [Test]
    [Arguments(false, 0)]
    [Arguments(false, 3)]
    [Arguments(true, 0)]
    [Arguments(true, 3)]
    public async Task WithShield_Rejects_Existing_Retries_Without_Changing_Configuration(bool useFactory, int retryCount)
    {
        var builder = new ModuleConfigurationBuilder().WithRetry(retryCount);
        var originalRetry = builder.Build().RetryConfiguration;
        var shield = Shield.Retry(0);

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            if (useFactory)
            {
                builder.WithShield(_ => shield);
            }
            else
            {
                builder.WithShield(shield);
            }
        });
        var configuration = builder.Build();

        using (Assert.Multiple())
        {
            await Assert.That(exception.Message).Contains("WithRetry cannot be combined with WithShield");
            await Assert.That(configuration.RetryConfiguration).IsSameReferenceAs(originalRetry);
            await Assert.That(configuration.ResilienceShieldFactory).IsNull();
        }
    }

    [Test]
    public async Task WithIgnoreFailuresWhen_Rejects_Null_Conditions()
    {
        var builder = new ModuleConfigurationBuilder();

        using (Assert.Multiple())
        {
            await Assert.That(() => builder.WithIgnoreFailuresWhen((Func<IModuleContext, Exception, bool>) null!))
                .Throws<ArgumentNullException>();
            await Assert.That(() => builder.WithIgnoreFailuresWhen(
                    (Func<IModuleContext, Exception, CancellationToken, ValueTask<bool>>) null!))
                .Throws<ArgumentNullException>();
        }
    }

    [Test]
    public async Task WithIgnoreFailuresWhen_Passes_The_Cancellation_Token()
    {
        CancellationToken observed = default;
        using var cancellationTokenSource = new CancellationTokenSource();
        var configuration = new ModuleConfigurationBuilder()
            .WithIgnoreFailuresWhen((_, _, cancellationToken) =>
            {
                observed = cancellationToken;
                return ValueTask.FromResult(true);
            })
            .Build();

        var ignored = await configuration.IgnoreFailuresCondition!(
            Mock.Of<IModuleContext>(),
            new InvalidOperationException(),
            cancellationTokenSource.Token);

        using (Assert.Multiple())
        {
            await Assert.That(ignored).IsTrue();
            await Assert.That(observed).IsEqualTo(cancellationTokenSource.Token);
        }
    }
}
