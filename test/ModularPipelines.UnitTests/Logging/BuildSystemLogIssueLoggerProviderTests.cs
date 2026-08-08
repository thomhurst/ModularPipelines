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
    public async Task Logger_IncludesConciseExceptionMessageInIssueCommand()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var logger = provider.CreateLogger("Example.Pipeline");
        var exception = new InvalidOperationException("failure");

        logger.LogError(exception, "message");

        await Assert.That(writer.ToString())
            .IsEqualTo(
                $"##vso[task.logissue type=error;]message: failure{Environment.NewLine}");
    }

    [Test]
    public async Task Logger_WritesOnlyOneErrorForRepeatedRootException()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var moduleLogger = provider.CreateLogger("Example.Module");
        var runnerLogger = provider.CreateLogger("Example.Runner");
        var rootException = new InvalidOperationException("failure");

        moduleLogger.LogError(new Exception("first wrapper", rootException), "first message");
        runnerLogger.LogError(new Exception("second wrapper", rootException), "second message");

        await Assert.That(writer.ToString())
            .IsEqualTo(
                $"##vso[task.logissue type=error;]first message: failure{Environment.NewLine}");
    }

    [Test]
    public async Task Logger_WritesIndependentErrorsWithIdenticalDiagnostics()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var logger = provider.CreateLogger("Example.Pipeline");

        logger.LogError(new InvalidOperationException("failure"), "first message");
        logger.LogError(new InvalidOperationException("failure"), "second message");

        await Assert.That(writer.ToString())
            .IsEqualTo(
                $"##vso[task.logissue type=error;]first message: failure{Environment.NewLine}"
                + $"##vso[task.logissue type=error;]second message: failure{Environment.NewLine}");
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
