using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Reflection.Emit;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.FSharp.TestFixtures;
using ModularPipelines.Options;
using ModularPipelines.VisualBasic.TestFixtures;
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
}

internal sealed class GeneratedNoSecretsOptions;

public class SecretValueNormalizationTests
{
    private sealed class PrivateNoSecretsOptions;

    private sealed class GenericNoSecretsOptions<T>;

    private sealed partial class PartialNoSecretsOptions;

    private static readonly string[] CharacterSecrets =
    [
        "array-secret",
        "enumerable-secret",
        "memory-secret",
        "read-only-memory-secret",
        "segment-secret",
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
    public async Task GeneratedExactEmptyMetadata_ReturnsEmpty()
    {
        var provider = CreateProvider(out _);
        var metadataFound = GeneratedSecretMetadata.TryGetAccessors(
            typeof(GeneratedNoSecretsOptions),
            out var accessors);
        var secrets = provider.GetSecretsInObject(new GeneratedNoSecretsOptions()).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(metadataFound).IsTrue();
            await Assert.That(accessors).IsEmpty();
            await Assert.That(secrets).IsEmpty();
        }
    }

    [Test]
    public async Task GeneratedNamedCoverage_HandlesPrivateAndGenericTypes()
    {
        var privateFound = GeneratedSecretMetadata.TryGetAccessors(typeof(PrivateNoSecretsOptions), out _);
        var genericFound = GeneratedSecretMetadata.TryGetAccessors(typeof(GenericNoSecretsOptions<string>), out _);

        using (Assert.Multiple())
        {
            await Assert.That(privateFound).IsTrue();
            await Assert.That(genericFound).IsTrue();
        }
    }

    [Test]
    public async Task ArrayOptionsAreRecognizedAsEmptyMetadata()
    {
        var provider = CreateProvider(out _);
        var options = Array.Empty<GeneratedNoSecretsOptions>();

        var metadataFound = GeneratedSecretMetadata.TryGetAccessors(options.GetType(), out var accessors);
        var secrets = provider.GetSecretsInObject(options).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(metadataFound).IsTrue();
            await Assert.That(accessors).IsEmpty();
            await Assert.That(secrets).IsEmpty();
        }
    }

    [Test]
    public async Task VisualBasicOptions_UseReflectionFallback()
    {
        var provider = CreateProvider(out _);

        var secrets = provider.GetSecretsInObject(new VisualBasicSecretOptions()).ToList();

        await Assert.That(secrets).IsEquivalentTo(["visual-basic-secret"]);
    }

    [Test]
    public async Task FSharpOptions_UseReflectionFallback()
    {
        var provider = CreateProvider(out _);

        var secrets = provider.GetSecretsInObject(new FSharpSecretOptions()).ToList();

        await Assert.That(secrets).IsEquivalentTo(["fsharp-secret"]);
    }

    [Test]
    public async Task DerivedOptions_UseBaseAssemblyForReflectionFallback()
    {
        var provider = CreateProvider(out _);
        var value = CreateDerivedDynamicOptions();

        var secrets = provider.GetSecretsInObject(value).ToList();

        await Assert.That(secrets).IsEquivalentTo(["inherited-secret"]);
    }

    [Test]
    public async Task MissingExactMetadata_ThrowsActionableException()
    {
        var provider = CreateProvider(out _);
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName("MissingSecretMetadata"),
            AssemblyBuilderAccess.Run);
        var typeBuilder = assembly.DefineDynamicModule("Main")
            .DefineType("ExternalOptions", TypeAttributes.Public);
        typeBuilder.DefineField(
            "SecretAttributeReference",
            typeof(SecretValueAttribute),
            FieldAttributes.Public);
        var objectType = typeBuilder.CreateType()!;
        var value = Activator.CreateInstance(objectType)!;
        GeneratedSecretMetadata.RegisterAssembly(objectType.Assembly);

        var exception = await Assert.That(() => provider.GetSecretsInObject(value).ToList())
            .Throws<MissingSecretMetadataException>();

        await Assert.That(exception!.ObjectType).IsEqualTo(objectType);
        await Assert.That(exception.Message).Contains("ModularPipelines.SourceGenerator");
    }

    [Test]
    public async Task PartialOptionsWithoutCompleteMetadataThrow()
    {
        var provider = CreateProvider(out _);

        var exception = await Assert.That(() =>
                provider.GetSecretsInObject(new PartialNoSecretsOptions()).ToList())
            .Throws<MissingSecretMetadataException>();

        await Assert.That(exception!.ObjectType).IsEqualTo(typeof(PartialNoSecretsOptions));
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

    private static object CreateDerivedDynamicOptions()
    {
        var baseAssembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName($"SecretBase_{Guid.NewGuid():N}"),
            AssemblyBuilderAccess.Run);
        var baseTypeBuilder = baseAssembly.DefineDynamicModule("Main")
            .DefineType("SecretBase", TypeAttributes.Public);
        var getter = baseTypeBuilder.DefineMethod(
            "get_Token",
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
            typeof(string),
            Type.EmptyTypes);
        var il = getter.GetILGenerator();
        il.Emit(OpCodes.Ldstr, "inherited-secret");
        il.Emit(OpCodes.Ret);
        var property = baseTypeBuilder.DefineProperty("Token", PropertyAttributes.None, typeof(string), null);
        property.SetGetMethod(getter);
        property.SetCustomAttribute(new CustomAttributeBuilder(
            typeof(SecretValueAttribute).GetConstructor(Type.EmptyTypes)!,
            []));
        var baseType = baseTypeBuilder.CreateType()!;

        var derivedAssembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName($"SecretDerived_{Guid.NewGuid():N}"),
            AssemblyBuilderAccess.Run);
        var derivedType = derivedAssembly.DefineDynamicModule("Main")
            .DefineType("SecretDerived", TypeAttributes.Public, baseType)
            .CreateType()!;

        return Activator.CreateInstance(derivedType)!;
    }
}
