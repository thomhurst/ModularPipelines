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
        var logger = provider.CreateLogger("Example.Pipeline");

        logger.Log(logLevel, "message");

        await Assert.That(writer.ToString())
            .IsEqualTo($"##vso[task.logissue type={issueType};]message{Environment.NewLine}");
    }

    [Test]
    public async Task Logger_IgnoresNonIssueLevels()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var logger = provider.CreateLogger("Example.Pipeline");

        logger.LogInformation("message");

        await Assert.That(writer.ToString()).IsEmpty();
    }

    [Test]
    public async Task Logger_IncludesExceptionDetailsInIssueCommand()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var logger = provider.CreateLogger("Example.Pipeline");
        var exception = new InvalidOperationException("failure");

        logger.LogError(exception, "message");

        var escapedNewLine = Environment.NewLine
            .Replace("\r", "%0D", StringComparison.Ordinal)
            .Replace("\n", "%0A", StringComparison.Ordinal);
        await Assert.That(writer.ToString())
            .IsEqualTo(
                $"##vso[task.logissue type=error;]message{escapedNewLine}{exception}{Environment.NewLine}");
    }

    [Test]
    public async Task Logger_WritesNothingWhenBuildSystemHasNoIssueCommand()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new DefaultFormatter(), writer);
        var logger = provider.CreateLogger("Example.Pipeline");

        logger.LogWarning("message");

        await Assert.That(writer.ToString()).IsEmpty();
    }

    [Test]
    [Arguments("Microsoft")]
    [Arguments("Microsoft.Extensions.Http")]
    [Arguments("System")]
    [Arguments("System.Net.Http")]
    public async Task Logger_IgnoresInfrastructureCategories(string category)
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var logger = provider.CreateLogger(category);

        logger.LogWarning("message");

        await Assert.That(writer.ToString()).IsEmpty();
    }

    private static BuildSystemLogIssueLoggerProvider CreateProvider(
        IBuildSystemFormatter formatter,
        TextWriter writer)
    {
        return new BuildSystemLogIssueLoggerProvider(
            formatter,
            new BuildSystemCommandWriter(writer));
    }
}
