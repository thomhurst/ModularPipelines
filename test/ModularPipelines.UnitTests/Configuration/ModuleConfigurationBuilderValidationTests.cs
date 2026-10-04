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

        // TimeSpan.Zero is the configuration's "no timeout" value, which also overrides the pipeline default.
        await Assert.That(configuration.Timeout).IsEqualTo(Timeout.InfiniteTimeSpan);
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
