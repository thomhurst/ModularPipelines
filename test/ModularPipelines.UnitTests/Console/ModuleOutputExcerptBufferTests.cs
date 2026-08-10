using System.Text;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Console;

public class ModuleOutputExcerptBufferTests
{
    [Test]
    public async Task SeparatesStandardOutputAndStandardError()
    {
        var buffer = new ModuleOutputExcerptBuffer(1024);

        buffer.Append("normal output", ModuleOutputStream.StandardOutput);
        buffer.Append("error output", ModuleOutputStream.StandardError);

        var excerpt = buffer.CreateExcerpt();
        using (Assert.Multiple())
        {
            await Assert.That(excerpt!.StdoutTail).IsEqualTo("normal output" + Environment.NewLine);
            await Assert.That(excerpt.StderrTail).IsEqualTo("error output" + Environment.NewLine);
            await Assert.That(excerpt.TruncatedBytes).IsEqualTo(0);
        }
    }

    [Test]
    public async Task AppliesOneUtf8TailLimitAcrossBothStreams()
    {
        const int maximumBytes = 10;
        var buffer = new ModuleOutputExcerptBuffer(maximumBytes);

        buffer.Append("old output", ModuleOutputStream.StandardOutput);
        buffer.Append("🙂new", ModuleOutputStream.StandardError);

        var excerpt = buffer.CreateExcerpt()!;
        var retainedBytes = Encoding.UTF8.GetByteCount(excerpt.StdoutTail ?? string.Empty)
                            + Encoding.UTF8.GetByteCount(excerpt.StderrTail ?? string.Empty);
        using (Assert.Multiple())
        {
            await Assert.That(retainedBytes).IsLessThanOrEqualTo(maximumBytes);
            await Assert.That(excerpt.StderrTail).EndsWith("🙂new" + Environment.NewLine);
            await Assert.That(excerpt.StdoutTail).DoesNotContain("old output");
            await Assert.That(excerpt.StdoutTail ?? string.Empty).DoesNotContain("�");
            await Assert.That(excerpt.StderrTail).DoesNotContain("�");
            await Assert.That(excerpt.TruncatedBytes).IsGreaterThan(0);
        }
    }

    [Test]
    public async Task RetainsValidUnicodeWhenTailStartsAtSurrogatePair()
    {
        var expectedTail = "🙂" + Environment.NewLine;
        var buffer = new ModuleOutputExcerptBuffer(Encoding.UTF8.GetByteCount(expectedTail));

        buffer.Append("12345🙂", ModuleOutputStream.StandardOutput);

        var excerpt = buffer.CreateExcerpt()!;
        using (Assert.Multiple())
        {
            await Assert.That(excerpt.StdoutTail).IsEqualTo(expectedTail);
            await Assert.That(excerpt.StdoutTail).DoesNotContain("�");
            await Assert.That(excerpt.TruncatedBytes).IsEqualTo(5);
        }
    }

    [Test]
    public async Task MasksLateRegisteredSecretBeforeSelectingTail()
    {
        const string secret = "late-secret";
        var snapshot = new SecretSnapshot(0, []);
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(() => snapshot.Version);
        secretProvider.Setup(provider => provider.GetSnapshot()).Returns(() => snapshot);
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(
                new SecretMaskingOptions { MaskValue = "***" }));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes: 16,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append($"prefix {secret} suffix", ModuleOutputStream.StandardOutput);

        snapshot = new SecretSnapshot(2, [secret]);
        var excerpt = buffer.CreateExcerpt();

        using (Assert.Multiple())
        {
            await Assert.That(excerpt).IsNotNull();
            await Assert.That(excerpt!.StdoutTail).Contains("***");
            await Assert.That(excerpt.StdoutTail).DoesNotContain("secret");
            await Assert.That(Encoding.UTF8.GetByteCount(excerpt.StdoutTail!)).IsLessThanOrEqualTo(16);
        }
    }

    [Test]
    public async Task RecomputesTailBudgetAfterLateMaskExpansion()
    {
        var snapshot = new SecretSnapshot(0, []);
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(() => snapshot.Version);
        secretProvider.Setup(provider => provider.GetSnapshot()).Returns(() => snapshot);
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes: 8192,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append("a", ModuleOutputStream.StandardOutput);

        snapshot = new SecretSnapshot(2, ["a"]);
        var excerpt = buffer.CreateExcerpt()!;

        using (Assert.Multiple())
        {
            await Assert.That(excerpt.StdoutTail).IsEqualTo("**********" + Environment.NewLine);
            await Assert.That(excerpt.TruncatedBytes).IsEqualTo(0);
        }
    }

    [Test]
    public async Task PreservesChunkRecencyWhenLateMasksExpand()
    {
        const int maximumBytes = 30;
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(2);
        secretProvider
            .Setup(provider => provider.GetSnapshot())
            .Returns(new SecretSnapshot(2, ["a"]));
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append("a", ModuleOutputStream.StandardOutput);
        buffer.Append("a", ModuleOutputStream.StandardError);
        buffer.Append("a", ModuleOutputStream.StandardOutput);

        var excerpt = buffer.CreateExcerpt()!;
        var maskedChunk = "**********" + Environment.NewLine;
        var remainingStdoutBytes = maximumBytes - Encoding.UTF8.GetByteCount(maskedChunk);

        using (Assert.Multiple())
        {
            await Assert.That(excerpt.StderrTail).IsEqualTo(maskedChunk);
            await Assert.That(excerpt.StdoutTail).EndsWith(maskedChunk);
            await Assert.That(Encoding.UTF8.GetByteCount(excerpt.StdoutTail!)).IsEqualTo(remainingStdoutBytes);
        }
    }

    [Test]
    public async Task ReallocatesFullMaskedBudgetByChunkRecency()
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(2);
        secretProvider
            .Setup(provider => provider.GetSnapshot())
            .Returns(new SecretSnapshot(2, ["a"]));
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes: 10,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append("aaaa", ModuleOutputStream.StandardOutput);
        buffer.Append("a", ModuleOutputStream.StandardError);

        var excerpt = buffer.CreateExcerpt()!;

        using (Assert.Multiple())
        {
            await Assert.That(excerpt.StdoutTail).IsNull();
            await Assert.That(Encoding.UTF8.GetByteCount(excerpt.StderrTail!)).IsEqualTo(10);
            await Assert.That(excerpt.StderrTail).EndsWith(Environment.NewLine);
        }
    }

    [Test]
    public async Task DoesNotCountLateMaskContractionAsTruncation()
    {
        const string secret = "late-secret";
        var snapshot = new SecretSnapshot(0, []);
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(() => snapshot.Version);
        secretProvider.Setup(provider => provider.GetSnapshot()).Returns(() => snapshot);
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(
                new SecretMaskingOptions { MaskValue = "***" }));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes: 64,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append(secret, ModuleOutputStream.StandardOutput);

        snapshot = new SecretSnapshot(2, [secret]);
        var excerpt = buffer.CreateExcerpt()!;

        using (Assert.Multiple())
        {
            await Assert.That(excerpt.StdoutTail).IsEqualTo("***" + Environment.NewLine);
            await Assert.That(excerpt.TruncatedBytes).IsEqualTo(0);
        }
    }

    [Test]
    public async Task OmitsExcerptWhenMaskingContractionReachesTrimmedBoundary()
    {
        const string secret = "late-secret";
        var snapshot = new SecretSnapshot(0, []);
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(() => snapshot.Version);
        secretProvider.Setup(provider => provider.GetSnapshot()).Returns(() => snapshot);
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(
                new SecretMaskingOptions { MaskValue = "***" }));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes: 16,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append(
            $"AAAA{secret}{secret}{secret}",
            ModuleOutputStream.StandardOutput);

        snapshot = new SecretSnapshot(2, [secret]);

        await Assert.That(buffer.CreateExcerpt()).IsNull();
    }

    [Test]
    public async Task OmitsExcerptWhenSecretExceedsSafeBoundaryContext()
    {
        const string secret = "long-secret";
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(2);
        secretProvider
            .Setup(provider => provider.GetSnapshot())
            .Returns(new SecretSnapshot(2, [secret]));
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes: 8,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append($"prefix {secret} suffix", ModuleOutputStream.StandardOutput);

        await Assert.That(buffer.CreateExcerpt()).IsNull();
    }

    [Test]
    public async Task CountsUtf8BoundaryBytesAsTruncated()
    {
        var maximumBytes = Encoding.UTF8.GetByteCount(Environment.NewLine) + 1;
        var buffer = new ModuleOutputExcerptBuffer(maximumBytes);

        buffer.Append("🙂", ModuleOutputStream.StandardOutput);

        var excerpt = buffer.CreateExcerpt()!;
        using (Assert.Multiple())
        {
            await Assert.That(excerpt.StdoutTail).IsEqualTo(Environment.NewLine);
            await Assert.That(excerpt.TruncatedBytes).IsEqualTo(4);
        }
    }

    [Test]
    public async Task OmitsExcerptWhenCaseInsensitiveMatchCanExceedBoundaryContext()
    {
        const string secret = "SSSS";
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(2);
        secretProvider
            .Setup(provider => provider.GetSnapshot())
            .Returns(new SecretSnapshot(2, [secret]));
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(
                new SecretMaskingOptions { CaseInsensitive = true }));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes: 4,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append("prefixſſſſ", ModuleOutputStream.StandardOutput);

        await Assert.That(buffer.CreateExcerpt()).IsNull();
    }

    [Test]
    public async Task OmitsExcerptWhenSecretsChangeDuringMasking()
    {
        var version = 2L;
        var snapshotCalls = 0;
        var snapshot = new SecretSnapshot(version, ["registered-secret"]);
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(() => version);
        secretProvider
            .Setup(provider => provider.GetSnapshot())
            .Returns(() =>
            {
                if (++snapshotCalls == 2)
                {
                    version = 4;
                }

                return snapshot;
            });
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes: 32,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append("registered-secret suffix", ModuleOutputStream.StandardOutput);

        await Assert.That(buffer.CreateExcerpt()).IsNull();
    }

    [Test]
    public async Task PreservesExistingMaskDuringExcerptObfuscation()
    {
        const string secret = "already-masked-secret";
        const string maskedOutput = "diagnostic **********";
        var expected = maskedOutput + Environment.NewLine;
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(2);
        secretProvider
            .Setup(provider => provider.GetSnapshot())
            .Returns(new SecretSnapshot(2, [secret]));
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
        var buffer = new ModuleOutputExcerptBuffer(
            Encoding.UTF8.GetByteCount(expected),
            secretObfuscator,
            secretProvider.Object);

        buffer.Append(maskedOutput, ModuleOutputStream.StandardOutput);

        var excerpt = buffer.CreateExcerpt()!;
        using (Assert.Multiple())
        {
            await Assert.That(excerpt.StdoutTail).IsEqualTo(expected);
            await Assert.That(excerpt.TruncatedBytes).IsEqualTo(0);
        }
    }

    [Test]
    public async Task OmitsExcerptWhenMaskValueContainsLateSecret()
    {
        const string secret = "REDACT";
        var snapshot = new SecretSnapshot(0, []);
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(() => snapshot.Version);
        secretProvider.Setup(provider => provider.GetSnapshot()).Returns(() => snapshot);
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(
                new SecretMaskingOptions { MaskValue = "[REDACTED]" }));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes: 64,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append("[REDACTED]", ModuleOutputStream.StandardOutput);

        snapshot = new SecretSnapshot(2, [secret]);

        await Assert.That(buffer.CreateExcerpt()).IsNull();
    }

    [Test]
    public async Task MasksLateSecretContainingExistingMaskValue()
    {
        const string secret = "prefix**********suffix";
        var snapshot = new SecretSnapshot(0, []);
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(() => snapshot.Version);
        secretProvider.Setup(provider => provider.GetSnapshot()).Returns(() => snapshot);
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
        var buffer = new ModuleOutputExcerptBuffer(
            maximumBytes: 64,
            secretObfuscator,
            secretProvider.Object);
        buffer.Append(secret, ModuleOutputStream.StandardOutput);

        snapshot = new SecretSnapshot(2, [secret]);

        var excerpt = buffer.CreateExcerpt()!;
        await Assert.That(excerpt.StdoutTail).IsEqualTo(
            $"**********{Environment.NewLine}");
    }

    [Test]
    public async Task RebalancesExpandedMaskAcrossAppendBoundaries()
    {
        var secret = $"abc{Environment.NewLine}def";
        var snapshot = new SecretSnapshot(0, []);
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(() => snapshot.Version);
        secretProvider.Setup(provider => provider.GetSnapshot()).Returns(() => snapshot);
        var secretObfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
        var expected = $"**********{Environment.NewLine}";
        var buffer = new ModuleOutputExcerptBuffer(
            Encoding.UTF8.GetByteCount(expected),
            secretObfuscator,
            secretProvider.Object);
        buffer.Append("abc", ModuleOutputStream.StandardOutput);
        buffer.Append("def", ModuleOutputStream.StandardOutput);

        snapshot = new SecretSnapshot(2, [secret]);

        var excerpt = buffer.CreateExcerpt()!;
        using (Assert.Multiple())
        {
            await Assert.That(excerpt.StdoutTail).IsEqualTo(expected);
            await Assert.That(excerpt.TruncatedBytes).IsEqualTo(0);
        }
    }
}
