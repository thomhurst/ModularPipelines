using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class MultilineSecretMaskingTests
{
    [Test]
    [Arguments("\n", "\n", false)]
    [Arguments("\n", "\r\n", true)]
    [Arguments("\r\n", "\n", true)]
    [Arguments("\r\n", "\r\n", false)]
    [Arguments("\r", "\r\n", false)]
    public async Task StreamedCommandOutput_MasksEverySecretLine(string secretNewline, string outputNewline, bool standardError)
    {
        var options = new SecretMaskingOptions();
        var provider = CreateProvider(out _, options);
        provider.AddSecret($"private-key-line-1{secretNewline}private-key-line-2");
        var logger = new CollectingLogger();
        var accessor = new Mock<IModuleLoggerAccessor>();
        accessor.Setup(x => x.Logger).Returns(logger);
        ICommandOutputLogger commandLogger = new CommandLogger(
            accessor.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            new SecretObfuscator(provider, Microsoft.Extensions.Options.Options.Create(options)));
        var toolOptions = new CommandLineToolOptions("tool");
        var executionOptions = new CommandExecutionOptions();
        using var output = new StringReader($"private-key-line-1{outputNewline}private-key-line-2{outputNewline}");

        while (output.ReadLine() is { } line)
        {
            if (standardError)
            {
                commandLogger.LogStandardErrorLine(toolOptions, executionOptions, line);
            }
            else
            {
                commandLogger.LogStandardOutputLine(toolOptions, executionOptions, line);
            }
        }

        await Assert.That(logger.Messages).IsEquivalentTo(["  ↳ **********", "  ↳ **********"]);
    }

    [Test]
    [Arguments("\n", "\r\n")]
    [Arguments("\r\n", "\n")]
    [Arguments("\r", "\r\n")]
    public async Task RegisteredSecret_MasksWholeValueWithDifferentNewlines(string secretNewline, string outputNewline)
    {
        var options = new SecretMaskingOptions();
        var provider = CreateProvider(out _, options);
        provider.AddSecret($"private-key-line-1{secretNewline}private-key-line-2");
        var obfuscator = new SecretObfuscator(provider, Microsoft.Extensions.Options.Options.Create(options));

        foreach (var pattern in SecretMaskingPatternGenerator.Generate($"private-key-line-1{outputNewline}private-key-line-2"))
        {
            await Assert.That(obfuscator.Obfuscate(pattern, null)).IsEqualTo("**********");
        }
    }

    [Test]
    public async Task MultilineSecret_RegistersEligibleLinesAndTheirEncodedFormsWithNativeMasker()
    {
        var options = new SecretMaskingOptions { MinimumSecretLength = 4 };
        var provider = CreateProvider(out var nativeMasker, options);
        provider.AddSecret("long-secret\n\n     \r\nxy\rquoted\"secret\nlong-secret\nedge");
        var obfuscator = new SecretObfuscator(provider, Microsoft.Extensions.Options.Options.Create(options));

        foreach (var line in new[] { "long-secret", "quoted\"secret", "edge" })
        {
            foreach (var pattern in SecretMaskingPatternGenerator.Generate(line))
            {
                await Assert.That(obfuscator.Obfuscate(pattern, null)).IsEqualTo("**********");
                nativeMasker.Verify(x => x.MaskSecrets(It.Is<IEnumerable<string>>(values => values.Contains(pattern))), Times.Once);
            }
        }

        await Assert.That(obfuscator.Obfuscate("xy   unrelated", null)).IsEqualTo("xy   unrelated");
        nativeMasker.Verify(x => x.MaskSecrets(It.Is<IEnumerable<string>>(values =>
            values.Any(value => string.IsNullOrWhiteSpace(value) || value == "xy"))), Times.Never);
    }

    [Test]
    public async Task OptionsObject_UsesMinimumLengthForMultilineSecretLines()
    {
        var options = new SecretMaskingOptions { MinimumSecretLength = 4 };
        var provider = CreateProvider(out _, options);
        var obfuscator = new SecretObfuscator(provider, Microsoft.Extensions.Options.Options.Create(options));
        var commandOptions = new SecretOptions { Value = "long-secret\nxy" };

        await Assert.That(obfuscator.Obfuscate("long-secret", commandOptions)).IsEqualTo("**********");
        await Assert.That(obfuscator.Obfuscate("xy", commandOptions)).IsEqualTo("xy");
    }

    private static SecretProvider CreateProvider(out Mock<IBuildSystemSecretMasker> nativeMasker, SecretMaskingOptions options)
    {
        nativeMasker = new Mock<IBuildSystemSecretMasker>();
        var optionsProvider = new Mock<IOptionsProvider>();
        optionsProvider.Setup(x => x.GetOptions()).Returns([]);
        return new SecretProvider(
            optionsProvider.Object,
            nativeMasker.Object,
            Microsoft.Extensions.Options.Options.Create(options),
            Mock.Of<ILogger<SecretProvider>>());
    }

    private sealed class SecretOptions
    {
        [SecretValue]
        public required string Value { get; init; }
    }

    private sealed class CollectingLogger : ILogger
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
            Messages.Add(formatter(state, exception));
    }
}
