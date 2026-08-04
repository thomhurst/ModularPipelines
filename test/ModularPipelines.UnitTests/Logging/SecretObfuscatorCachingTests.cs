using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class SecretObfuscatorCachingTests
{
    [Test]
    public async Task ReusesSecretSnapshotUntilProviderChanges()
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(0, ["long-secret", "secret"]));
        var obfuscator = CreateObfuscator(secretProvider.Object);

        await Assert.That(obfuscator.Obfuscate("long-secret", null)).IsEqualTo("**********");
        await Assert.That(obfuscator.Obfuscate("secret", null)).IsEqualTo("**********");

        secretProvider.Verify(x => x.GetSnapshot(), Times.Once);
    }

    [Test]
    public async Task ReturnsOriginalStringWhenNoSecretMatches()
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(0, ["secret"]));
        var obfuscator = CreateObfuscator(secretProvider.Object);
        const string input = "safe output";

        var result = obfuscator.Obfuscate(input, null);

        await Assert.That(result).IsSameReferenceAs(input);
    }

    [Test]
    public async Task RebuildsSecretSnapshotWhenProviderVersionChanges()
    {
        var version = 0L;
        string[] secrets = ["first-secret"];
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(x => x.Version).Returns(() => version);
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(() => new SecretSnapshot(version, secrets));
        var obfuscator = CreateObfuscator(secretProvider.Object);

        await Assert.That(obfuscator.Obfuscate("first-secret", null)).IsEqualTo("**********");

        secrets = ["second-secret"];
        version += 2;

        await Assert.That(obfuscator.Obfuscate("second-secret", null)).IsEqualTo("**********");
        secretProvider.Verify(x => x.GetSnapshot(), Times.Exactly(2));
    }

    [Test]
    public async Task MasksSecretRegisteredAfterSnapshotWasCreated()
    {
        var optionsProvider = new Mock<IOptionsProvider>();
        optionsProvider.Setup(x => x.GetOptions()).Returns([]);
        var secretProvider = new SecretProvider(
            optionsProvider.Object,
            Mock.Of<IBuildSystemSecretMasker>(),
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()),
            Mock.Of<ILogger<SecretProvider>>());
        var obfuscator = CreateObfuscator(secretProvider);

        await Assert.That(obfuscator.Obfuscate("safe output", null)).IsEqualTo("safe output");

        secretProvider.AddSecret("dynamic-secret");

        await Assert.That(obfuscator.Obfuscate("dynamic-secret", null)).IsEqualTo("**********");
    }

    [Test]
    public async Task RevalidatesCacheWhenPublicationStartsDuringFastPath()
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupSequence(x => x.Version)
            .Returns(0)
            .Returns(0)
            .Returns(1);
        secretProvider.SetupSequence(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(0, ["initial-secret"]))
            .Returns(new SecretSnapshot(2, ["initial-secret", "dynamic-secret"]));
        var obfuscator = CreateObfuscator(secretProvider.Object);

        await Assert.That(obfuscator.Obfuscate("initial-secret", null)).IsEqualTo("**********");
        await Assert.That(obfuscator.Obfuscate("dynamic-secret", null)).IsEqualTo("**********");

        secretProvider.Verify(x => x.GetSnapshot(), Times.Exactly(2));
    }

    [Test]
    public async Task ReusesRegisteredCacheWhenOptionSecretsAreAlreadyRegistered()
    {
        const string secret = "registered-secret";
        var optionsObject = new object();
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(0, SecretMaskingPatternGenerator.Generate(secret)));
        secretProvider.Setup(x => x.GetSecretsInObject(optionsObject)).Returns([secret]);
        var obfuscator = CreateObfuscator(secretProvider.Object);
        var maskingOptions = new SecretMaskingOptions();

        var registeredCache = GetSecretCache(obfuscator, null, maskingOptions, false);
        var optionsCache = GetSecretCache(obfuscator, optionsObject, maskingOptions, false);

        await Assert.That(optionsCache).IsSameReferenceAs(registeredCache);
    }

    [Test]
    public async Task BuildsExtraPatternsForCaseVariantOptionSecrets()
    {
        const string registeredSecret = "CaseSensitiveSecret";
        const string optionSecret = "casesensitivesecret";
        var optionsObject = new object();
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(
                0,
                SecretMaskingPatternGenerator.Generate(registeredSecret)));
        secretProvider.Setup(x => x.GetSecretsInObject(optionsObject)).Returns([optionSecret]);
        var obfuscator = CreateObfuscator(secretProvider.Object, caseInsensitive: true);
        var encodedOptionSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(optionSecret));

        var result = obfuscator.Obfuscate(encodedOptionSecret, optionsObject);

        await Assert.That(result).IsEqualTo("**********");
    }

    [Test]
    public async Task BuildsExtraPatternsWhenOptionSecretMatchesRegisteredDerivedPattern()
    {
        const string registeredSecret = "foo";
        var optionSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(registeredSecret));
        var optionsObject = new object();
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(
                0,
                SecretMaskingPatternGenerator.Generate(registeredSecret)));
        secretProvider.Setup(x => x.GetSecretsInObject(optionsObject)).Returns([optionSecret]);
        var obfuscator = CreateObfuscator(secretProvider.Object);
        var encodedOptionSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(optionSecret));

        var result = obfuscator.Obfuscate(encodedOptionSecret, optionsObject);

        await Assert.That(result).IsEqualTo("**********");
    }

    private static object GetSecretCache(
        SecretObfuscator obfuscator,
        object? optionsObject,
        SecretMaskingOptions maskingOptions,
        bool caseInsensitive)
    {
        var method = typeof(SecretObfuscator).GetMethod(
            "GetSecretCache",
            BindingFlags.Instance | BindingFlags.NonPublic)!;
        return method.Invoke(
            obfuscator,
            [optionsObject, maskingOptions, caseInsensitive])!;
    }

    private static SecretObfuscator CreateObfuscator(
        ISecretProvider secretProvider,
        bool caseInsensitive = false)
    {
        return new SecretObfuscator(
            secretProvider,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions
            {
                CaseInsensitive = caseInsensitive,
            }));
    }
}
