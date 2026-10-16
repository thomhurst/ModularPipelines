using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Console;
using ModularPipelines.Engine.BuildSystemFormatters;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using Moq;

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
    [Arguments(LogLevel.Warning)]
    [Arguments(LogLevel.Error)]
    [Arguments(LogLevel.Critical)]
    public async Task Logger_MasksRegisteredSecretsInMessagesAndExceptions(LogLevel logLevel)
    {
        const string secret = "annotation-secret";
        var options = Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions());
        var optionsProvider = new Mock<IOptionsProvider>();
        optionsProvider.Setup(x => x.GetOptions()).Returns([]);
        var secrets = new SecretProvider(
            optionsProvider.Object,
            Mock.Of<IBuildSystemSecretMasker>(),
            options,
            Mock.Of<ILogger<SecretProvider>>());
        secrets.AddSecret(secret);
        var formatterProvider = new Mock<IBuildSystemFormatterProvider>();
        formatterProvider.Setup(x => x.GetFormatter()).Returns(new AzurePipelinesFormatter());
        using var writer = new StringWriter();
        using var services = new ServiceCollection()
            .AddSingleton(formatterProvider.Object)
            .AddSingleton<IBuildSystemCommandWriter>(new BuildSystemCommandWriter(writer))
            .AddSingleton<ISecretObfuscator>(new SecretObfuscator(secrets, options))
            .AddSingleton<BuildSystemLogIssueLoggerProvider>()
            .BuildServiceProvider();
        var logger = services.GetRequiredService<BuildSystemLogIssueLoggerProvider>()
            .CreateLogger("Example.Pipeline");

        logger.Log(logLevel, "Message {Secret}", secret);
        var encodedSecret = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(secret));
        logger.Log(logLevel, new InvalidOperationException($"Failure {encodedSecret}"), "Message {Secret}", secret);

        var issueType = logLevel == LogLevel.Warning ? "warning" : "error";
        await Assert.That(writer.ToString()).IsEqualTo(
            $"##vso[task.logissue type={issueType};]Message **********{Environment.NewLine}"
            + $"##vso[task.logissue type={issueType};]Message **********: Failure **********{Environment.NewLine}");
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
    public async Task Logger_WritesOnlyOneErrorAfterBufferedExceptionObfuscation()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var logger = provider.CreateLogger("Example.Module");
        var exception = new InvalidOperationException("failure");
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string>(), null))
            .Returns((string input, object? _) => input);
        var firstEvent = CreateBufferedEvent("first message");
        var secondEvent = CreateBufferedEvent("second message");

        firstEvent.WriteTo(logger);
        secondEvent.WriteTo(logger);

        await Assert.That(writer.ToString())
            .IsEqualTo(
                $"##vso[task.logissue type=error;]first message: failure{Environment.NewLine}");

        BufferedLogEvent<string> CreateBufferedEvent(string message) =>
            new(
                LogLevel.Error,
                default,
                message,
                message,
                exception,
                static (state, _) => state,
                obfuscator.Object);
    }

    [Test]
    public async Task Logger_DoesNotWriteIssueForModuleStatusEvent()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var logger = provider.CreateLogger("Example.Module");

        logger.Log(LogLevel.Error, ModuleLogEvents.Status, "Module failed");

        await Assert.That(writer.ToString()).IsEmpty();
    }

    [Test]
    public async Task Logger_OriginatingFailureWinsAfterIgnoredDependencyFailure()
    {
        using var writer = new StringWriter();
        using var provider = CreateProvider(new AzurePipelinesFormatter(), writer);
        var logger = provider.CreateLogger("Example.Module");
        var exception = new InvalidOperationException("failure");

        logger.LogIgnoredDependencyFailure(exception);
        logger.LogError(exception, "originating failure");

        await Assert.That(writer.ToString())
            .IsEqualTo(
                $"##vso[task.logissue type=error;]originating failure: failure{Environment.NewLine}");
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
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator.Setup(x => x.Obfuscate(It.IsAny<string>(), null))
            .Returns((string input, object? _) => input);
        return new BuildSystemLogIssueLoggerProvider(
            formatter,
            new BuildSystemCommandWriter(writer),
            new Lazy<ISecretObfuscator>(() => obfuscator.Object));
    }
}
