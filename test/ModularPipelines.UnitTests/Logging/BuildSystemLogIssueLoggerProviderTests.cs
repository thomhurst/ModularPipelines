using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Engine.BuildSystemFormatters;
using ModularPipelines.Logging;

namespace ModularPipelines.UnitTests.Logging;

public class BuildSystemLogIssueLoggerProviderTests
{
    [Test]
    [Arguments(LogLevel.Warning, "warning")]
    [Arguments(LogLevel.Error, "error")]
    [Arguments(LogLevel.Critical, "error")]
    public async Task Logger_WritesAzureIssueCommands(LogLevel logLevel, string issueType)
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var logger = provider.CreateLogger("test");

        logger.Log(logLevel, "message");

        await Assert.That(writer.ToString())
            .IsEqualTo($"##vso[task.logissue type={issueType};]message{Environment.NewLine}");
    }

    [Test]
    public async Task Logger_IgnoresNonIssueLevels()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var logger = provider.CreateLogger("test");

        logger.LogInformation("message");

        await Assert.That(writer.ToString()).IsEmpty();
    }

    [Test]
    public async Task Logger_WritesNothingWhenBuildSystemHasNoIssueCommand()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new DefaultFormatter(), writer);
        var logger = provider.CreateLogger("test");

        logger.LogWarning("message");

        await Assert.That(writer.ToString()).IsEmpty();
    }

    private static BuildSystemLogIssueLoggerProvider CreateProvider(
        IBuildSystemFormatter formatter,
        TextWriter writer)
    {
        return new BuildSystemLogIssueLoggerProvider(formatter, () => writer);
    }
}
