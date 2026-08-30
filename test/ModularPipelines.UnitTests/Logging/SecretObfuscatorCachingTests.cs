using ModularPipelines.Secrets;
using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class SecretObfuscatorCachingTests
{
    [Test]
    public async Task HasSecrets_TracksDynamicSecretRegistration()
    {
        var optionsProvider = new Mock<IOptionsProvider>();
        optionsProvider.Setup(x => x.GetOptions()).Returns([]);
        var secretProvider = new SecretProvider(
            optionsProvider.Object,
            Mock.Of<IBuildSystemSecretMasker>(),
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()),
            Mock.Of<ILogger<SecretProvider>>());
        var obfuscator = CreateObfuscator(secretProvider);

        await Assert.That(obfuscator.HasSecrets).IsFalse();

        secretProvider.AddSecret("dynamic-secret");

        await Assert.That(obfuscator.HasSecrets).IsTrue();
    }

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
    public async Task UsesLongestSecretWhenPatternsMatchAtSamePosition()
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(0, ["secret", "secret-value"]));
        var obfuscator = CreateObfuscator(secretProvider.Object);

        var result = obfuscator.Obfuscate("secret-value and secret", null);

        await Assert.That(result).IsEqualTo("********** and **********");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task DoesNotRescanMaskReplacement(bool caseInsensitive)
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(0, ["secret", "*"]));
        var obfuscator = CreateObfuscator(
            secretProvider.Object,
            caseInsensitive,
            maskValue: "***");

        var result = obfuscator.Obfuscate(caseInsensitive ? "SECRET" : "secret", null);

        await Assert.That(result).IsEqualTo("***");
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

        var registeredCache = obfuscator.GetSecretCache(null, maskingOptions, false);
        var optionsCache = obfuscator.GetSecretCache(optionsObject, maskingOptions, false);
        var repeatedOptionsCache = obfuscator.GetSecretCache(optionsObject, maskingOptions, false);

        using (Assert.Multiple())
        {
            await Assert.That(optionsCache).IsSameReferenceAs(registeredCache);
            await Assert.That(repeatedOptionsCache).IsSameReferenceAs(registeredCache);
        }

        secretProvider.Verify(x => x.GetSecretsInObject(optionsObject), Times.Exactly(2));
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
        var registeredCache = obfuscator.GetSecretCache(
            null,
            new SecretMaskingOptions(),
            caseInsensitive: true);

        using (Assert.Multiple())
        {
            await Assert.That(result).IsEqualTo("**********");
            await Assert.That(registeredCache.ExactSecrets.Contains(optionSecret)).IsTrue();
        }
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

    [Test]
    public async Task InvalidatesOptionsCacheWhenRegisteredSecretsChange()
    {
        var version = 0L;
        var optionsObject = new object();
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(x => x.Version).Returns(() => version);
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(() => new SecretSnapshot(version, []));
        secretProvider.Setup(x => x.GetSecretsInObject(optionsObject)).Returns(["option-secret"]);
        var obfuscator = CreateObfuscator(secretProvider.Object);
        var maskingOptions = new SecretMaskingOptions();

        var first = obfuscator.GetSecretCache(optionsObject, maskingOptions, false);
        var repeated = obfuscator.GetSecretCache(optionsObject, maskingOptions, false);
        version += 2;
        var refreshed = obfuscator.GetSecretCache(optionsObject, maskingOptions, false);

        using (Assert.Multiple())
        {
            await Assert.That(repeated).IsSameReferenceAs(first);
            await Assert.That(refreshed).IsNotSameReferenceAs(first);
        }

        secretProvider.Verify(x => x.GetSecretsInObject(optionsObject), Times.Exactly(3));
    }

    [Test]
    public async Task InvalidatesOptionsCacheWhenOptionsObjectChanges()
    {
        var optionSecret = "first-option-secret";
        var optionsObject = new object();
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.Setup(x => x.GetSnapshot()).Returns(new SecretSnapshot(0, []));
        secretProvider.Setup(x => x.GetSecretsInObject(optionsObject))
            .Returns(() => [optionSecret]);
        var obfuscator = CreateObfuscator(secretProvider.Object);
        var maskingOptions = new SecretMaskingOptions();

        var first = obfuscator.GetSecretCache(optionsObject, maskingOptions, false);
        optionSecret = "second-option-secret";
        var refreshed = obfuscator.GetSecretCache(optionsObject, maskingOptions, false);

        using (Assert.Multiple())
        {
            await Assert.That(refreshed).IsNotSameReferenceAs(first);
            await Assert.That(obfuscator.Obfuscate(optionSecret, optionsObject)).IsEqualTo("**********");
        }
    }

    private static SecretObfuscator CreateObfuscator(
        ISecretProvider secretProvider,
        bool caseInsensitive = false,
        string? maskValue = null)
    {
        return new SecretObfuscator(
            secretProvider,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions
            {
                CaseInsensitive = caseInsensitive,
                MaskValue = maskValue ?? "**********",
            }));
    }
}
