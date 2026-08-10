using CliWrap;
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

    internal sealed class SecretOptions
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
    [Arguments("abc\"def")]
    [Arguments("abc\\\"def")]
    [Arguments("abc \\")]
    public async Task CliWrapEscapedCommandInput_MasksEmbeddedSecret(string secret)
    {
        var provider = CreateProvider(out _);
        provider.AddSecret(secret);
        var commandInput = Cli.Wrap("tool")
            .WithArguments([$"--value={secret}"])
            .ToString();

        await Assert.That(CreateObfuscator(provider).Obfuscate(commandInput, null))
            .IsEqualTo("tool \"--value=**********\"");
    }

    [Test]
    public async Task CommandScriptEscapedSecret_IsMasked()
    {
        const string secret = "abc\"def";
        var provider = CreateProvider(out _);
        provider.AddSecret(secret);
        var commandScriptEscaped = secret.Replace("\"", "\"\"");

        await Assert.That(CreateObfuscator(provider).Obfuscate($"before {commandScriptEscaped} after", null))
            .IsEqualTo("before ********** after");
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
    public async Task DirectCharacterWrites_ReuseSecretPatterns()
    {
        var provider = new Mock<ISecretProvider>();
        provider.Setup(x => x.GetSnapshot()).Returns(new SecretSnapshot(0, ["split-secret"]));
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string>(), null))
            .Returns((string input, object? _) => input);
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            obfuscator.Object,
            provider.Object);

        foreach (var character in "ordinary output")
        {
            writer.Write(character);
        }

        writer.Write(['\r', '\n'], 0, 2);

        await Assert.That(realConsole.ToString()).IsEqualTo($"ordinary output{Environment.NewLine}");
        provider.Verify(x => x.GetSnapshot(), Times.Once);
    }

    [Test]
    public void CompletedOutput_OnlyObfuscatesActualSecretMatches()
    {
        var provider = new Mock<ISecretProvider>();
        provider.Setup(x => x.GetSnapshot()).Returns(new SecretSnapshot(0, ["split-secret"]));
        var obfuscator = new Mock<ITrackedSecretObfuscator>();
        obfuscator.SetupGet(x => x.PatternComparison).Returns(StringComparison.Ordinal);
        obfuscator
            .Setup(x => x.ObfuscateWithConsumption("split-secret", null))
            .Returns(new SecretObfuscationResult("**********", "split-secret".Length));
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            obfuscator.Object,
            provider.Object);

        writer.WriteLine("ordinary output");
        writer.WriteLine("split-secret");

        obfuscator.Verify(x => x.ObfuscateWithConsumption("split-secret", null), Times.Once);
        obfuscator.Verify(x => x.ObfuscateWithConsumption("ordinary output", null), Times.Never);
    }

    [Test]
    public async Task CustomObfuscatorProcessesOutputWithoutRegisteredPatterns()
    {
        var provider = CreateProvider(out _);
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string>(), null))
            .Returns((string input, object? _) => input.Replace("custom-secret", "[masked]", StringComparison.Ordinal));
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            obfuscator.Object,
            provider);

        writer.WriteLine("before custom-secret after");

        await Assert.That(realConsole.ToString())
            .IsEqualTo($"before [masked] after{Environment.NewLine}");
    }

    [Test]
    public async Task CustomObfuscatorCanRegisterDiscoveredSecret()
    {
        var provider = CreateProvider(out _);
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string>(), null))
            .Returns((string input, object? _) =>
            {
                provider.AddSecret("discovered-secret");
                return input;
            });
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            obfuscator.Object,
            provider);

        writer.WriteLine("ordinary output");

        using (Assert.Multiple())
        {
            await Assert.That(provider.Secrets).Contains("discovered-secret");
            await Assert.That(realConsole.ToString())
                .IsEqualTo($"ordinary output{Environment.NewLine}");
        }
    }

    [Test]
    public async Task CustomObfuscatorCanWriteReentrantly()
    {
        var provider = CreateProvider(out _);
        var obfuscator = new Mock<ISecretObfuscator>();
        CoordinatedTextWriter? writer = null;
        var isReentrantWrite = false;
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string>(), null))
            .Returns((string input, object? _) =>
            {
                if (!isReentrantWrite)
                {
                    isReentrantWrite = true;
                    try
                    {
                        writer!.WriteLine("custom diagnostic");
                    }
                    finally
                    {
                        isReentrantWrite = false;
                    }
                }

                return input.Replace("custom-secret", "[masked]", StringComparison.Ordinal);
            });
        var realConsole = new StringWriter();

        using (writer = new CoordinatedTextWriter(
                   Mock.Of<IConsoleCoordinator>(),
                   realConsole,
                   () => false,
                   obfuscator.Object,
                   provider))
        {
            writer.WriteLine("custom-secret");
        }

        await Assert.That(realConsole.ToString()).IsEqualTo(
            $"custom diagnostic{Environment.NewLine}[masked]{Environment.NewLine}");
    }

    [Test]
    public async Task DirectConsoleWrite_RefreshesPatternsAfterComparisonChanges()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("ABC");
        var maskingOptions = Microsoft.Extensions.Options.Options.Create(
            new SecretMaskingOptions());
        var obfuscator = new SecretObfuscator(provider, maskingOptions);
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            obfuscator,
            provider);

        writer.WriteLine("abc");
        maskingOptions.Value.CaseInsensitive = true;
        writer.WriteLine("abc");

        await Assert.That(realConsole.ToString()).IsEqualTo(
            $"abc{Environment.NewLine}**********{Environment.NewLine}");
    }

    [Test]
    public async Task DifferentModuleBuffers_ProcessConcurrently()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("split-secret");
        using var firstWriteStarted = new ManualResetEventSlim();
        using var releaseFirstWrite = new ManualResetEventSlim();
        var firstBuffer = new Mock<IModuleOutputBuffer>();
        firstBuffer
            .Setup(x => x.WriteLine("**********"))
            .Callback(() =>
            {
                firstWriteStarted.Set();
                if (!releaseFirstWrite.Wait(TimeSpan.FromSeconds(10)))
                {
                    throw new TimeoutException("Timed out waiting to release the first module write.");
                }
            });
        var secondBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetModuleBuffer(typeof(FirstModule))).Returns(firstBuffer.Object);
        coordinator.Setup(x => x.GetModuleBuffer(typeof(SecondModule))).Returns(secondBuffer.Object);

        using var writer = new CoordinatedTextWriter(
            coordinator.Object,
            new StringWriter(),
            () => true,
            CreateObfuscator(provider),
            provider);

        var firstWrite = Task.Run(() => WriteForModule(writer, typeof(FirstModule), "split-secret"));
        try
        {
            await Assert.That(firstWriteStarted.Wait(TimeSpan.FromSeconds(5))).IsTrue();

            var secondWrite = Task.Run(() => WriteForModule(writer, typeof(SecondModule), "ordinary output"));
            await secondWrite.WaitAsync(TimeSpan.FromSeconds(5));
        }
        finally
        {
            releaseFirstWrite.Set();
            await firstWrite;
        }

        secondBuffer.Verify(x => x.WriteLine("ordinary output"), Times.Once);
        firstBuffer.Verify(x => x.WriteLine("**********"), Times.Once);
    }

    [Test]
    public async Task DifferentModuleBuffers_SerializeCustomObfuscatorCalls()
    {
        var provider = CreateProvider(out _);
        using var firstObfuscationStarted = new ManualResetEventSlim();
        using var secondWriterStarted = new ManualResetEventSlim();
        using var secondObfuscationStarted = new ManualResetEventSlim();
        using var secondBufferReached = new ManualResetEventSlim();
        using var releaseFirstObfuscation = new ManualResetEventSlim();
        var callCount = 0;
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string>(), null))
            .Returns((string input, object? _) =>
            {
                if (Interlocked.Increment(ref callCount) == 1)
                {
                    firstObfuscationStarted.Set();
                    if (!releaseFirstObfuscation.Wait(TimeSpan.FromSeconds(10)))
                    {
                        throw new TimeoutException("Timed out waiting to release the first obfuscation.");
                    }
                }
                else
                {
                    secondObfuscationStarted.Set();
                }

                return input;
            });
        var firstBuffer = new Mock<IModuleOutputBuffer>();
        var secondBuffer = new Mock<IModuleOutputBuffer>();
        secondBuffer
            .Setup(x => x.WriteLine("second output"))
            .Callback(() => secondBufferReached.Set());
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetModuleBuffer(typeof(FirstModule))).Returns(firstBuffer.Object);
        coordinator.Setup(x => x.GetModuleBuffer(typeof(SecondModule))).Returns(secondBuffer.Object);

        using var writer = new CoordinatedTextWriter(
            coordinator.Object,
            new StringWriter(),
            () => true,
            obfuscator.Object,
            provider);

        var firstWrite = Task.Run(() => WriteForModule(writer, typeof(FirstModule), "first output"));
        var secondWrite = Task.CompletedTask;
        try
        {
            await Assert.That(firstObfuscationStarted.Wait(TimeSpan.FromSeconds(5))).IsTrue();
            secondWrite = Task.Factory.StartNew(
                () =>
                {
                    secondWriterStarted.Set();
                    WriteForModule(writer, typeof(SecondModule), "second output");
                },
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            await Assert.That(secondWriterStarted.Wait(TimeSpan.FromSeconds(5))).IsTrue();
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            await Assert.That(secondObfuscationStarted.IsSet).IsFalse();
            await Assert.That(secondBufferReached.IsSet).IsFalse();
        }
        finally
        {
            releaseFirstObfuscation.Set();
            await Task.WhenAll(firstWrite, secondWrite).WaitAsync(TimeSpan.FromSeconds(5));
        }

        await Assert.That(secondObfuscationStarted.IsSet).IsTrue();
        await Assert.That(secondBufferReached.IsSet).IsTrue();
        obfuscator.Verify(x => x.Obfuscate(It.IsAny<string>(), null), Times.Exactly(2));
        firstBuffer.Verify(x => x.WriteLine("first output"), Times.Once);
        secondBuffer.Verify(x => x.WriteLine("second output"), Times.Once);
    }

    [Test]
    public void BufferedConsoleWrites_RefreshPatternsAfterSecretRegistration()
    {
        var provider = CreateProvider(out _);
        var outputBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetUnattributedBuffer()).Returns(outputBuffer.Object);

        using var writer = new CoordinatedTextWriter(
            coordinator.Object,
            new StringWriter(),
            () => true,
            CreateObfuscator(provider),
            provider);

        writer.WriteLine("ordinary output");

        var secret = $"private-key-line-1{Environment.NewLine}private-key-line-2";
        provider.AddSecret(secret);
        writer.WriteLine(secret);

        outputBuffer.Verify(x => x.WriteLine("ordinary output"), Times.Once);
        outputBuffer.Verify(x => x.WriteLine("**********"), Times.Once);
        outputBuffer.Verify(x => x.WriteLine(It.Is<string>(value => value.Contains("private-key"))), Times.Never);
    }

    [Test]
    public async Task AvailableFlush_RetainsPotentialSecretPrefix()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("split-secret");
        var outputBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetUnattributedBuffer()).Returns(outputBuffer.Object);

        using var writer = new CoordinatedTextWriter(
            coordinator.Object,
            new StringWriter(),
            () => true,
            CreateObfuscator(provider),
            provider);

        writer.Write("split-");
        await writer.FlushAvailableAsync();

        outputBuffer.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);

        writer.WriteLine("secret");

        outputBuffer.Verify(x => x.WriteLine("**********"), Times.Once);
        outputBuffer.Verify(x => x.WriteLine(It.Is<string>(value => value.Contains("split-secret"))), Times.Never);
    }

    [Test]
    public async Task AvailableFlush_MasksCompleteSecretOverlappingRetainedPrefix()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("abc");
        provider.AddSecret("bcx");
        var outputBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetUnattributedBuffer()).Returns(outputBuffer.Object);

        using var writer = new CoordinatedTextWriter(
            coordinator.Object,
            new StringWriter(),
            () => true,
            CreateObfuscator(provider),
            provider);

        writer.Write("abc");
        await writer.FlushAvailableAsync();
        writer.WriteLine("y");

        outputBuffer.Verify(x => x.WriteLine(It.Is<string>(value => value.Contains("abc"))), Times.Never);
        outputBuffer.Verify(x => x.WriteLine(It.Is<string>(value => value.Contains("**********"))), Times.Once);
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
    public async Task DirectConsoleWrite_MasksContainedSecretBeforeRetainedPrefix()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("abcXYZdef");
        provider.AddSecret("XYZ");
        provider.AddSecret("efGHI");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write("helloabcXYZdef");

        await Assert.That(realConsole.ToString()).IsEqualTo("helloabc**********d");
    }

    [Test]
    public async Task DirectConsoleWrite_MasksShorterSameStartSecretBeforeRetainedPrefix()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("abc");
        provider.AddSecret("abcdef");
        provider.AddSecret("efxyZ");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write("abcdefxy");

        await Assert.That(realConsole.ToString()).IsEqualTo("**********d");
    }

    [Test]
    public async Task DirectConsoleWrite_HonorsCaseSensitiveOverlappingSecrets()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("ABC");
        provider.AddSecret("bcx");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider, caseInsensitive: false),
            provider);

        writer.Write("abcx");

        await Assert.That(realConsole.ToString()).IsEqualTo("a**********");
    }

    [Test]
    public async Task DirectConsoleWrite_RescansPartiallyObfuscatedCandidates()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("ABCDEF");
        provider.AddSecret("abc");
        provider.AddSecret("defx");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write("abcdefx");

        await Assert.That(realConsole.ToString()).IsEqualTo("********************");
    }

    [Test]
    public async Task DirectConsoleWrite_DoesNotMistakeCustomMaskSuffixForUnconsumedInput()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("bAb");
        var obfuscator = new SecretObfuscator(
            provider,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions
            {
                CaseInsensitive = true,
                MaskValue = "ab",
            }));
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            obfuscator,
            provider);

        writer.Write("babaBABb");
        writer.Flush();

        await Assert.That(realConsole.ToString()).IsEqualTo("abaabb");
    }

    [Test]
    public async Task DirectConsoleWrite_PreservesTailAfterCaseSensitiveFalseMatch()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("a");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider, caseInsensitive: false),
            provider);

        writer.Write("aA");

        await Assert.That(realConsole.ToString()).IsEqualTo("**********A");
    }

    [Test]
    public async Task DirectConsoleWrite_MasksSelfOverlappingMatchBeforeRetainedPrefix()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("abcabc");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write("ABCabcabcabcabc");
        writer.Flush();

        await Assert.That(realConsole.ToString()).IsEqualTo("ABC********************");
    }

    [Test]
    public async Task DirectConsoleWrite_MasksCompleteSecretOverlappingAnotherSecretPrefix()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("abcdef");
        provider.AddSecret("cdefgh");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write("abcdef");
        writer.Flush();

        await Assert.That(realConsole.ToString()).IsEqualTo("**********");
    }

    [Test]
    public async Task DirectConsoleWrite_MasksCompleteMatchBeforeFalseCaseRetainedPrefix()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("AAAA");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider, caseInsensitive: false),
            provider);

        writer.Write("AAAAAa");
        writer.Flush();

        await Assert.That(realConsole.ToString()).IsEqualTo("**********Aa");
    }

    [Test]
    public async Task DirectConsoleWrite_PreservesCaseDistinctPatternsAcrossWrites()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("abc");
        provider.AddSecret("ABC");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write("AB");
        writer.Write("C");
        writer.Flush();

        await Assert.That(realConsole.ToString()).IsEqualTo("**********");
    }

    [Test]
    public async Task DirectConsoleWrite_RescansAfterRetainedPrefixIsInvalidated()
    {
        var provider = CreateProvider(out _);
        provider.AddSecret("token");
        provider.AddSecret("secret");
        provider.AddSecret("okensecretx");
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        writer.Write("tokensecret");
        writer.Flush();

        await Assert.That(realConsole.ToString()).IsEqualTo("********************");
    }

    [Test]
    public async Task DirectConsoleWrite_SerializesRegistrationDuringMasking()
    {
        var provider = CreateProvider(out var nativeMasker);
        provider.AddSecret("known-secret");
        using var obfuscationStarted = new ManualResetEventSlim();
        using var releaseObfuscation = new ManualResetEventSlim();
        using var registrationStarted = new ManualResetEventSlim();
        nativeMasker
            .Setup(masker => masker.MaskSecrets(It.IsAny<IEnumerable<string>>()))
            .Callback(() => registrationStarted.Set());
        var obfuscator = new Mock<ITrackedSecretObfuscator>();
        obfuscator.SetupGet(candidate => candidate.PatternComparison).Returns(StringComparison.Ordinal);
        obfuscator
            .Setup(candidate => candidate.ObfuscateWithConsumption(It.IsAny<string>(), null))
            .Returns((string input, object? _) =>
            {
                if (input == "known-secret")
                {
                    obfuscationStarted.Set();
                    if (!releaseObfuscation.Wait(TimeSpan.FromSeconds(10)))
                    {
                        throw new TimeoutException("Timed out waiting to release obfuscation.");
                    }
                }

                return input is "known-secret" or "dynamic-secret"
                    ? new SecretObfuscationResult("**********", input.Length)
                    : new SecretObfuscationResult(input, 0);
            });
        obfuscator
            .Setup(candidate => candidate.Obfuscate(It.IsAny<string>(), null))
            .Returns((string input, object? _) =>
                input.Replace("dynamic-secret", "**********", StringComparison.Ordinal));
        var realConsole = new StringWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            obfuscator.Object,
            provider);

        var write = Task.Run(() => writer.WriteLine("known-secret dynamic-secret"));
        var registration = Task.CompletedTask;
        try
        {
            await Assert.That(obfuscationStarted.Wait(TimeSpan.FromSeconds(5))).IsTrue();
            registration = Task.Run(() => provider.AddSecret("dynamic-secret"));
            await Assert.That(registrationStarted.Wait(TimeSpan.FromSeconds(5))).IsTrue();
            await Assert.That(async () =>
                    await registration.WaitAsync(TimeSpan.FromMilliseconds(500)))
                .Throws<TimeoutException>();

            releaseObfuscation.Set();
            await Task.WhenAll(write, registration).WaitAsync(TimeSpan.FromSeconds(5));
        }
        finally
        {
            releaseObfuscation.Set();
            await Task.WhenAll(write, registration).WaitAsync(TimeSpan.FromSeconds(5));
        }

        await Assert.That(realConsole.ToString())
            .IsEqualTo($"********** dynamic-secret{Environment.NewLine}");
        obfuscator.Verify(
            candidate => candidate.ObfuscateWithConsumption("dynamic-secret", null),
            Times.Never);
    }

    [Test]
    public async Task DirectConsoleWrite_LinearizesWithSecretRegistration()
    {
        var provider = CreateProvider(out var nativeMasker);
        using var writeStarted = new ManualResetEventSlim();
        using var releaseWrite = new ManualResetEventSlim();
        using var registrationStarted = new ManualResetEventSlim();
        nativeMasker
            .Setup(masker => masker.MaskSecrets(It.IsAny<IEnumerable<string>>()))
            .Callback(() => registrationStarted.Set());
        var realConsole = new BlockingWriteStringWriter(writeStarted, releaseWrite);

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        var write = Task.Run(() => writer.WriteLine("dynamic-secret"));
        var registration = Task.CompletedTask;
        try
        {
            await Assert.That(writeStarted.Wait(TimeSpan.FromSeconds(5))).IsTrue();
            registration = Task.Run(() => provider.AddSecret("dynamic-secret"));
            await Assert.That(registrationStarted.Wait(TimeSpan.FromSeconds(5))).IsTrue();
            await Assert.That(async () =>
                    await registration.WaitAsync(TimeSpan.FromMilliseconds(500)))
                .Throws<TimeoutException>();

            releaseWrite.Set();
            await Task.WhenAll(write, registration).WaitAsync(TimeSpan.FromSeconds(5));
        }
        finally
        {
            releaseWrite.Set();
            await Task.WhenAll(write, registration).WaitAsync(TimeSpan.FromSeconds(5));
        }

        await Assert.That(realConsole.ToString())
            .IsEqualTo($"dynamic-secret{Environment.NewLine}");
    }

    [Test]
    public async Task FlushAsync_UsesUnderlyingAsynchronousFlush()
    {
        var provider = CreateProvider(out _);
        var realConsole = new AsyncFlushTrackingWriter();

        using var writer = new CoordinatedTextWriter(
            Mock.Of<IConsoleCoordinator>(),
            realConsole,
            () => false,
            CreateObfuscator(provider),
            provider);

        await writer.FlushAsync();

        using (Assert.Multiple())
        {
            await Assert.That(realConsole.AsyncFlushCount).IsEqualTo(1);
            await Assert.That(realConsole.SynchronousFlushCount).IsEqualTo(0);
        }
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

    private static SecretObfuscator CreateObfuscator(
        ISecretProvider provider,
        bool caseInsensitive = false)
    {
        return new SecretObfuscator(
            provider,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions
            {
                CaseInsensitive = caseInsensitive,
            }));
    }

    private static void WriteForModule(CoordinatedTextWriter writer, Type moduleType, string value)
    {
        var previousModule = ModuleLogger.CurrentModuleType.Value;
        try
        {
            ModuleLogger.CurrentModuleType.Value = moduleType;
            writer.WriteLine(value);
        }
        finally
        {
            ModuleLogger.CurrentModuleType.Value = previousModule;
        }
    }

    private sealed class BlockingWriteStringWriter(
        ManualResetEventSlim writeStarted,
        ManualResetEventSlim releaseWrite) : StringWriter
    {
        public override void WriteLine(string? value)
        {
            writeStarted.Set();
            if (!releaseWrite.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new TimeoutException("Timed out waiting to release the console write.");
            }

            base.WriteLine(value);
        }
    }

    private sealed class AsyncFlushTrackingWriter : StringWriter
    {
        public int AsyncFlushCount { get; private set; }

        public int SynchronousFlushCount { get; private set; }

        public override void Flush()
        {
            SynchronousFlushCount++;
        }

        public override Task FlushAsync()
        {
            AsyncFlushCount++;
            return Task.CompletedTask;
        }
    }
}
