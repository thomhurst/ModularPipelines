using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Attributes;

internal class GeneratedCharacterSecretOptions
{
    [SecretValue]
    public string StringSecret { get; init; } = "string-secret";

    [SecretValue]
    public char[] CharacterArraySecret { get; init; } = "array-secret".ToCharArray();

    [SecretValue]
    public IEnumerable<char> CharacterEnumerableSecret { get; init; } =
        "enumerable-secret".ToCharArray().Where(_ => true);

    [SecretValue]
    public Memory<char> MemorySecret { get; init; } = "memory-secret".ToCharArray().AsMemory();

    [SecretValue]
    public ReadOnlyMemory<char> ReadOnlyMemorySecret { get; init; } =
        "read-only-memory-secret".ToCharArray().AsMemory();

    [SecretValue]
    public ArraySegment<char> CharacterSegmentSecret { get; init; } =
        new("segment-secret".ToCharArray());

    [SecretValue]
    public IEnumerable<string> SecretCollection { get; init; } =
        ["collection-secret-one", "collection-secret-two"];

    [SecretValue]
    public CliOptionValue OptionalValueSecret { get; init; } = "optional-value-secret";

    [SecretValue]
    public CliOptionValue BareOptionalValue { get; init; } = CliOptionValue.Bare;
}

internal sealed class ReflectionCharacterSecretOptions : GeneratedCharacterSecretOptions
{
    [SecretValue]
    private string InaccessibleSecret => " ";
}

public class SecretValueNormalizationTests
{
    private static readonly string[] CharacterSecrets =
    [
        "array-secret",
        "enumerable-secret",
        "memory-secret",
        "read-only-memory-secret",
        "segment-secret",
        "optional-value-secret",
    ];

    private static readonly string[] ExpectedSecrets =
    [
        "string-secret",
        .. CharacterSecrets,
        "collection-secret-one",
        "collection-secret-two",
    ];

    [Test]
    public async Task GeneratedMetadata_NormalizesCharacterSequencesAndCollections()
    {
        var provider = CreateProvider(out _);
        var metadataFound = GeneratedSecretMetadata.TryGetAccessors(
            typeof(GeneratedCharacterSecretOptions),
            out _);

        var secrets = provider.GetSecretsInObject(new GeneratedCharacterSecretOptions()).ToList();

        await Assert.That(metadataFound).IsTrue();
        await Assert.That(secrets).IsEquivalentTo(ExpectedSecrets);
    }

    [Test]
    public async Task ReflectionFallback_NormalizesCharacterSequencesAndCollections()
    {
        var provider = CreateProvider(out _);
        var metadataFound = GeneratedSecretMetadata.TryGetAccessors(
            typeof(ReflectionCharacterSecretOptions),
            out _);

        var secrets = provider.GetSecretsInObject(new ReflectionCharacterSecretOptions()).ToList();

        await Assert.That(metadataFound).IsFalse();
        await Assert.That(secrets).IsEquivalentTo(ExpectedSecrets);
    }

    [Test]
    public async Task NativeMasker_ReceivesWholeCharacterSecretsOnce()
    {
        var provider = CreateProvider(out var nativeMasker);
        var options = new GeneratedCharacterSecretOptions();

        provider.AddSecrets(provider.GetSecretsInObject(options));

        var registeredSecrets = nativeMasker.Invocations
            .SelectMany(invocation => (IEnumerable<string>) invocation.Arguments[0])
            .ToList();

        foreach (var characterSecret in CharacterSecrets)
        {
            await Assert.That(registeredSecrets.Count(secret => secret == characterSecret)).IsEqualTo(1);
        }

        await Assert.That(registeredSecrets.Where(secret => secret.Length == 1)).IsEmpty();
    }

    private static SecretProvider CreateProvider(out Mock<IBuildSystemSecretMasker> nativeMasker)
    {
        nativeMasker = new Mock<IBuildSystemSecretMasker>();
        var optionsProvider = new Mock<IOptionsProvider>();
        optionsProvider.Setup(x => x.GetOptions()).Returns([]);

        return new SecretProvider(
            optionsProvider.Object,
            nativeMasker.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()),
            Mock.Of<ILogger<SecretProvider>>());
    }
}
