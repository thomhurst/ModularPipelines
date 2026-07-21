using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class SecretMaskingPatternTests
{
    private const string Secret = "p@ss word\"value";

    private sealed class SecretOptions
    {
        [SecretValue]
        public string Value { get; init; } = Secret;
    }

    [Test]
    public async Task RegisteredSecret_MasksCommonEncodedForms()
    {
        var provider = CreateProvider(out var nativeMasker);
        provider.AddSecret(Secret);
        var obfuscator = CreateObfuscator(provider);
        var patterns = SecretMaskingPatternGenerator.Generate(Secret);

        foreach (var pattern in patterns)
        {
            await Assert.That(obfuscator.Obfuscate($"before {pattern} after", null))
                .IsEqualTo("before ********** after");
        }

        nativeMasker.Verify(x => x.MaskSecrets(It.Is<IEnumerable<string>>(
            values => patterns.All(pattern => values.Contains(pattern)))));
    }

    [Test]
    public async Task OptionsObject_MasksCommonEncodedFormsWithoutPriorRegistration()
    {
        var provider = CreateProvider(out _);
        var obfuscator = CreateObfuscator(provider);

        foreach (var pattern in SecretMaskingPatternGenerator.Generate(Secret))
        {
            await Assert.That(obfuscator.Obfuscate(pattern, new SecretOptions()))
                .IsEqualTo("**********");
        }
    }

    [Test]
    public async Task ShortSecret_RequestsNativeMaskingAndLogsWarningWithoutValue()
    {
        var logger = new Mock<ILogger<SecretProvider>>();
        var provider = CreateProvider(out var nativeMasker, logger, minimumSecretLength: 3);

        provider.AddSecret("qz");

        await Assert.That(provider.Secrets).IsEmpty();
        nativeMasker.Verify(x => x.MaskSecrets(It.Is<IEnumerable<string>>(values => values.Contains("qz"))));
        logger.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains("length 2", StringComparison.Ordinal)
                && !state.ToString()!.Contains("qz", StringComparison.Ordinal)),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public void BufferedConsoleWrites_MaskSecretSplitAcrossChunks()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("split-secret");
        var obfuscator = CreateObfuscator(provider);
        var outputBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetUnattributedBuffer()).Returns(outputBuffer.Object);

        using var writer = new CoordinatedTextWriter(
            coordinator.Object,
            new StringWriter(),
            () => true,
            obfuscator);

        writer.Write("split-");
        writer.WriteLine("secret");

        outputBuffer.Verify(x => x.WriteLine("**********"), Times.Once);
        outputBuffer.Verify(x => x.WriteLine(It.Is<string>(value => value.Contains("split-secret"))), Times.Never);
    }

    [Test]
    public async Task DirectConsoleWrites_MaskSecretSplitAcrossChunks()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("split-secret");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider));

        writer.Write("split-");
        writer.WriteLine("secret");

        await Assert.That(realConsole.ToString()).IsEqualTo($"**********{Environment.NewLine}");
    }

    private static SecretProvider CreateProvider(
        out Mock<IBuildSystemSecretMasker> nativeMasker,
        Mock<ILogger<SecretProvider>>? logger = null,
        int minimumSecretLength = 1)
    {
        nativeMasker = new Mock<IBuildSystemSecretMasker>();
        var optionsProvider = new Mock<IOptionsProvider>();
        optionsProvider.Setup(x => x.GetOptions()).Returns([]);

        return new SecretProvider(
            optionsProvider.Object,
            nativeMasker.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions
            {
                MinimumSecretLength = minimumSecretLength,
            }),
            logger?.Object ?? Mock.Of<ILogger<SecretProvider>>());
    }

    private static SecretObfuscator CreateObfuscator(ISecretProvider provider)
    {
        return new SecretObfuscator(
            provider,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
    }
}
