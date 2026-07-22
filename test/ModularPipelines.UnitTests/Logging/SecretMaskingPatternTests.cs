using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Engine.BuildSystemFormatters;
using ModularPipelines.Logging;
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

    private sealed class FirstModule
    {
    }

    private sealed class SecondModule
    {
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
            obfuscator,
            provider);

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
            CreateObfuscator(provider),
            provider);

        writer.Write("split-");
        writer.WriteLine("secret");

        await Assert.That(realConsole.ToString()).IsEqualTo($"**********{Environment.NewLine}");
    }

    [Test]
    public async Task DirectConsoleWrites_Retain_CompleteSecret_ThatPrefixesLongerSecret()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("abc");
        provider.AddSecret("abcdef");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write("abc");

        await Assert.That(realConsole.ToString()).IsEmpty();

        writer.WriteLine("def");

        await Assert.That(realConsole.ToString()).IsEqualTo($"**********{Environment.NewLine}");
    }

    [Test]
    [Arguments("abcdef", "cde", "abcde", "f")]
    [Arguments("abc\ndef", "c\nde", "abc\nde", "f")]
    public async Task DirectConsoleWrites_Retain_WholePrefix_Before_InnerSecretMatch(
        string longerSecret,
        string innerSecret,
        string firstChunk,
        string secondChunk)
    {
        var provider = CreateProvider(out _);
        provider.AddSecret(longerSecret);
        provider.AddSecret(innerSecret);
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write(firstChunk);

        await Assert.That(realConsole.ToString()).IsEmpty();

        writer.Write(secondChunk);

        await Assert.That(realConsole.ToString()).IsEqualTo("**********");
    }

    [Test]
    [Arguments("a%b", "::add-mask::a%25b")]
    [Arguments("line 1\r\nline 2", "::add-mask::line 1%0D%0Aline 2")]
    [Arguments("a%25b", "::add-mask::a%2525b")]
    public async Task GitHubActionsMaskCommand_EscapesWorkflowCommandData(string value, string expected)
    {
        await Assert.That(new GitHubActionsFormatter().GetMaskSecretCommand(value)).IsEqualTo(expected);
    }

    [Test]
    public async Task DirectConsoleWrites_Mask_MultilineSecrets_Before_Splitting_Lines()
    {
        var secret = $"private-key-line-1{Environment.NewLine}private-key-line-2";
        var provider = CreateProvider(out _);
        provider.AddSecret(secret);
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.WriteLine(secret);

        await Assert.That(realConsole.ToString()).IsEqualTo($"**********{Environment.NewLine}");
    }

    [Test]
    public void BufferedConsoleWrites_Mask_MultilineSecrets_Before_Splitting_Lines()
    {
        var secret = $"private-key-line-1{Environment.NewLine}private-key-line-2";
        var provider = CreateProvider(out _);
        provider.AddSecret(secret);
        var outputBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetUnattributedBuffer()).Returns(outputBuffer.Object);

        using var writer = new CoordinatedTextWriter(
            coordinator.Object,
            new StringWriter(),
            () => true,
            CreateObfuscator(provider),
            provider);

        writer.WriteLine(secret);

        outputBuffer.Verify(x => x.WriteLine("**********"), Times.Once);
        outputBuffer.Verify(x => x.WriteLine(It.Is<string>(value => value.Contains("private-key"))), Times.Never);
    }

    [Test]
    public async Task DirectConsoleWrites_Mask_MultilineSecrets_SplitAcrossLines()
    {
        var secret = $"private-key-line-1{Environment.NewLine}private-key-line-2";
        var provider = CreateProvider(out _);
        provider.AddSecret(secret);
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.WriteLine("private-key-line-1");

        await Assert.That(realConsole.ToString()).IsEmpty();

        writer.WriteLine("private-key-line-2");

        await Assert.That(realConsole.ToString()).IsEqualTo($"**********{Environment.NewLine}");
    }

    [Test]
    public void BufferedConsoleWrites_Mask_MultilineSecrets_SplitAcrossLines()
    {
        var secret = $"private-key-line-1{Environment.NewLine}private-key-line-2";
        var provider = CreateProvider(out _);
        provider.AddSecret(secret);
        var outputBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetUnattributedBuffer()).Returns(outputBuffer.Object);

        using var writer = new CoordinatedTextWriter(
            coordinator.Object,
            new StringWriter(),
            () => true,
            CreateObfuscator(provider),
            provider);

        writer.WriteLine("private-key-line-1");

        outputBuffer.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);

        writer.WriteLine("private-key-line-2");

        outputBuffer.Verify(x => x.WriteLine("**********"), Times.Once);
        outputBuffer.Verify(x => x.WriteLine(It.Is<string>(value => value.Contains("private-key"))), Times.Never);
    }

    [Test]
    public void BufferedConsoleWrites_Isolate_Retained_Secret_Prefixes_Per_Module()
    {
        var secret = $"private-key-line-1{Environment.NewLine}private-key-line-2";
        var provider = CreateProvider(out _);
        provider.AddSecret(secret);
        var firstBuffer = new Mock<IModuleOutputBuffer>();
        var secondBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetModuleBuffer(typeof(FirstModule))).Returns(firstBuffer.Object);
        coordinator.Setup(x => x.GetModuleBuffer(typeof(SecondModule))).Returns(secondBuffer.Object);

        var previousModule = ModuleLogger.CurrentModuleType.Value;
        try
        {
            using var writer = new CoordinatedTextWriter(
                coordinator.Object,
                new StringWriter(),
                () => true,
                CreateObfuscator(provider),
                provider);

            ModuleLogger.CurrentModuleType.Value = typeof(FirstModule);
            writer.WriteLine("private-key-line-1");

            ModuleLogger.CurrentModuleType.Value = typeof(SecondModule);
            writer.WriteLine("second-module-output");

            ModuleLogger.CurrentModuleType.Value = typeof(FirstModule);
            writer.WriteLine("private-key-line-2");
        }
        finally
        {
            ModuleLogger.CurrentModuleType.Value = previousModule;
        }

        firstBuffer.Verify(x => x.WriteLine("**********"), Times.Once);
        firstBuffer.Verify(x => x.WriteLine(It.Is<string>(value => value.Contains("private-key"))), Times.Never);
        secondBuffer.Verify(x => x.WriteLine("second-module-output"), Times.Once);
        secondBuffer.Verify(x => x.WriteLine(It.Is<string>(value => value.Contains("private-key"))), Times.Never);
    }

    [Test]
    public async Task DirectConsoleWrite_WithoutNewline_IsImmediate()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("split-secret");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write("Enter value: ");

        await Assert.That(realConsole.ToString()).IsEqualTo("Enter value: ");
    }

    [Test]
    public async Task DirectConsoleWrite_OnlyRetainsPotentialSecretPrefix()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("split-secret");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write("before split-");

        await Assert.That(realConsole.ToString()).IsEqualTo("before ");

        writer.Write("secret after");

        await Assert.That(realConsole.ToString()).IsEqualTo("before ********** after");
    }

    [Test]
    public async Task PartialLine_Keeps_Its_Original_Destination_When_Buffering_Starts()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("split-secret");
        var realConsole = new StringWriter();
        var outputBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetUnattributedBuffer()).Returns(outputBuffer.Object);
        var shouldBuffer = false;

        using var writer = new CoordinatedTextWriter(
            coordinator.Object,
            realConsole,
            () => shouldBuffer,
            CreateObfuscator(provider),
            provider);

        writer.Write("split-");
        shouldBuffer = true;
        writer.WriteLine("secret");

        await Assert.That(realConsole.ToString()).IsEqualTo($"**********{Environment.NewLine}");
        outputBuffer.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
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
