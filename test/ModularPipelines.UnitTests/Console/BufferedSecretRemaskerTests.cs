using ModularPipelines.Console;
using ModularPipelines.Enums;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using Moq;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Console;

public class BufferedSecretRemaskerTests
{
    [Test]
    public async Task IncrementalFlush_Retains_Unterminated_Maskable_Tail()
    {
        var remasker = CreateRemasker("token");
        BufferedOutput[] outputs =
        [
            BufferedOutput.FromString(
                "to",
                ModuleOutputStream.StandardOutput,
                appendNewLine: false),
        ];

        var count = remasker.GetIncrementalFlushableOutputCount(outputs);

        await Assert.That(count).IsEqualTo(0);
    }

    [Test]
    public async Task TryWrite_Masks_Secret_Split_Across_Fragments()
    {
        var remasker = CreateRemasker("token");
        BufferedOutput[] outputs =
        [
            BufferedOutput.FromString(
                "to",
                ModuleOutputStream.StandardOutput,
                appendNewLine: false),
            BufferedOutput.FromString("ken"),
        ];
        using var writer = new StringWriter();

        var count = remasker.TryWrite(CreateConsole(writer), outputs, 0);

        using (Assert.Multiple())
        {
            await Assert.That(count).IsEqualTo(2);
            await Assert.That(writer.ToString()).Contains("**********");
            await Assert.That(writer.ToString()).DoesNotContain("token");
        }
    }

    [Test]
    public async Task TryWrite_Masks_Secret_Split_Across_Line_Boundary()
    {
        var secret = $"to{Environment.NewLine}ken";
        var remasker = CreateRemasker(secret);
        BufferedOutput[] outputs =
        [
            BufferedOutput.FromString("to"),
            BufferedOutput.FromString("ken"),
        ];
        using var writer = new StringWriter();

        var count = remasker.TryWrite(CreateConsole(writer), outputs, 0);

        using (Assert.Multiple())
        {
            await Assert.That(count).IsEqualTo(2);
            await Assert.That(writer.ToString()).Contains("**********");
            await Assert.That(writer.ToString()).DoesNotContain(secret);
        }
    }

    [Test]
    public async Task TryWrite_Stops_After_Completed_NonSecret_Line()
    {
        var remasker = CreateRemasker("token");
        BufferedOutput[] outputs =
        [
            BufferedOutput.FromString("ordinary"),
            BufferedOutput.FromString("token"),
        ];
        using var writer = new StringWriter();

        var count = remasker.TryWrite(CreateConsole(writer), outputs, 0);

        using (Assert.Multiple())
        {
            await Assert.That(count).IsEqualTo(1);
            await Assert.That(writer.ToString()).Contains("ordinary");
            await Assert.That(writer.ToString()).DoesNotContain("**********");
        }
    }

    [Test]
    public async Task TryWrite_Remasks_Only_Registered_Secrets_In_PreObfuscated_Output()
    {
        const string secret = "secret";
        var provider = CreateSecretProvider(secret);
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? input, object? _) => input switch
            {
                "secret" => "masked",
                "masked" => "masked-twice",
                _ => input ?? string.Empty,
            });
        var remasker = new BufferedSecretRemasker(obfuscator.Object, provider.Object);
        BufferedOutput[] outputs =
        [
            BufferedOutput.FromString(
                "masked ",
                ModuleOutputStream.StandardOutput,
                appendNewLine: false,
                isPreObfuscated: true),
            BufferedOutput.FromString(
                secret,
                ModuleOutputStream.StandardOutput,
                appendNewLine: true,
                isPreObfuscated: true),
        ];
        using var writer = new StringWriter();

        var count = remasker.TryWrite(CreateConsole(writer), outputs, 0);

        using (Assert.Multiple())
        {
            await Assert.That(count).IsEqualTo(2);
            await Assert.That(writer.ToString()).Contains("masked masked");
            await Assert.That(writer.ToString()).DoesNotContain("masked-twice");
            await Assert.That(writer.ToString()).DoesNotContain(secret);
        }
    }

    private static BufferedSecretRemasker CreateRemasker(string secret)
    {
        var provider = CreateSecretProvider(secret);
        return new BufferedSecretRemasker(
            new SecretObfuscator(
                provider.Object,
                Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions())),
            provider.Object);
    }

    private static Mock<ISecretProvider> CreateSecretProvider(string secret)
    {
        var provider = new Mock<ISecretProvider>();
        provider.SetupGet(x => x.Version).Returns(0);
        provider.Setup(x => x.GetSnapshot()).Returns(new SecretSnapshot(0, [secret]));
        return provider;
    }

    private static IAnsiConsole CreateConsole(TextWriter writer) =>
        AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer),
        });
}
